using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages {

    public class LotDetailModel : PageModel {
        private readonly HOAContext _context;

        public LotDetailModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public IList<Lot> Lots { get; set; }

        public Models.Owner SignedInOwner { get; set; }

        public int? ActiveLotID { get; set; }

        public bool IsMyLots { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            /*Note on routing: id refers to lotID
            If page is accessed through My Lots Side Navigation link, Lots will be found by matching the currently logged in users owner id
            If page is accessed through an individual lot number link(from any page), Lots contain a single lot found by the route id
*/
            var authID = HttpContext.Session?.GetInt32("SessionUserID");
            if (authID != null) {
                SignedInOwner = _context.User
                    .Include(y => y.Owner)
                    .FirstOrDefault(x => x.UserID == authID).Owner;
            }

            if (SignedInOwner == null)
                RedirectToPage("./User/Login");

            if (id != null) {
                var ownerId = _context.Lots.FirstOrDefault(x => x.LotID == id)?.OwnerID;

                Lots = await _context.Lots
                    .Include(a => a.Address)
                    .Include(l => l.Owner)
                    .ThenInclude(d => d.CoOwner)
                    .ThenInclude(b => b.OwnerHistory)
                    .ThenInclude(s => s.HistoryType)
                    .Include(r => r.LotInventory)
                    .ThenInclude(i => i.Inventory)
                    .Where(x => x.OwnerID == ownerId)
                    .ToListAsync();

                ActiveLotID = id;
            }
            else {
                var uId = SignedInOwner?.OwnerID;

                if (uId != null) {
                    IsMyLots = true;
                    try {
                        Lots = await _context.Lots
                            .Include(a => a.Address)
                            .Include(l => l.Owner)
                            .ThenInclude(d => d.CoOwner)
                            .ThenInclude(b => b.OwnerHistory)
                            .ThenInclude(s => s.HistoryType)
                            .Include(r => r.LotInventory)
                            .ThenInclude(i => i.Inventory)
                            .Where(x => x.Owner.OwnerID == uId && x.Status == "Occupied")
                            .ToListAsync();
                    }
                    catch {
                        Lots = await _context.Lots
                            .Include(a => a.Address)
                            .Include(l => l.Owner)
                            .ThenInclude(d => d.CoOwner)
                            .ThenInclude(b => b.OwnerHistory)
                            .ThenInclude(s => s.HistoryType)
                            .Include(r => r.LotInventory)
                            .ThenInclude(i => i.Inventory)
                            .Where(x => x.LotID == id)
                            .ToListAsync();
                    }
                    ActiveLotID = Lots.FirstOrDefault()?.LotID; //sets first lot in list to active
                }
            }

            return Page();
        }

        public IList<OwnerHistory> GetOwnerHistory(int? lotID) {
            var ownerHistory = _context.OwnerHistory
                .Include(i => i.HistoryType)
                .Include(i => i.Owner)
                .Include(i => i.Lot)
                .Where(x => x.LotID == lotID)
                .ToList();

            return ownerHistory;
        }

        public HOASunridge.Models.Owner GetCoOwner(Lot lot) {
            return _context.Owner.FirstOrDefault(d => d.OwnerID == lot.Owner.CoOwnerID);
        }
    }
}