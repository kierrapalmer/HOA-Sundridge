using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Manage.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HOASunridge.Pages.Admin.Lots {

    public class EditModel : LotInventoryPageModel {
        private readonly HOAContext _context;

        public EditModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Lot Lot { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Lot = await _context.Lots
                .Include(l => l.Address)
                .Include(a => a.Owner)
                .Include(l => l.LotInventory)
                .ThenInclude(li => li.Inventory)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.LotID == id);

            if (Lot == null) {
                return NotFound();
            }

            //populate lists
            ViewData["Status"] = new SelectList(_context.Lots.Select(l => l.Status).Distinct());
            ViewData["State"] = States.GetStateSelectList(Lot.Address?.State);

            var owners = new List<string>();
            owners = _context.Owner.Where(x => x.IsPrimary == true).Select(x => x.FullName).ToList();

            ViewData["Owners"] = owners;

            PopulateAssignedLotInventoryData(_context, Lot);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedInventory, string ownerName = "") {
            if (!ModelState.IsValid) {
                return Page();
            }

            var lotToUpdate = await _context.Lots
                .Include(l => l.LotInventory).ThenInclude(l => l.Inventory)
                .Include(l => l.Address).ThenInclude(a => a.Owner)
                .FirstOrDefaultAsync(l => l.LotID == id);

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            lotToUpdate.LastModifiedBy = user != null ? user.Initials : "SYS";
            lotToUpdate.LastModifiedDate = DateTime.Now;

            if (await TryUpdateModelAsync<Lot>(
                lotToUpdate, "Lot",
                l => l.Status, l => l.LotNumber, l => l.Address,
                l => l.LastModifiedBy, l => l.LastModifiedDate)) {
                if (ownerName == null || lotToUpdate.Status == "Vacant") {
                    lotToUpdate.OwnerID = null;
                }
                else {
                    var owner = _context.Owner.FirstOrDefault(x => x.FullName == ownerName);
                    if (owner != null) {
                        lotToUpdate.OwnerID = owner.OwnerID;
                    }
                    else {
                        ModelState.AddModelError("Owner", "Please enter the name of an existing primary owner.");
                        await OnGetAsync(id);
                        return Page();
                    }
                }

                UpdateLotInventory(_context, selectedInventory, lotToUpdate, id);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            UpdateLotInventory(_context, selectedInventory, lotToUpdate, id);
            PopulateAssignedLotInventoryData(_context, lotToUpdate);
            return RedirectToPage("./Index");
        }
    }
}