using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.OwnersActivity {

    public class EditModel : PageModel {
        private readonly HOAContext _context;

        public EditModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public OwnerHistory OwnerHistory { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            OwnerHistory = await _context.OwnerHistory
                .Include(o => o.Lot)
                .Include(o => o.Owner)
                .FirstOrDefaultAsync(m => m.OwnerHistoryID == id).ConfigureAwait(false);

            if (OwnerHistory == null) {
                return NotFound();
            }

            ViewData["PrivacyLevel"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Value = "Internal", Text = "Internal"},
                new SelectListItem{Value = "Public", Text = "Public"}
            }, "Value", "Text");
            ViewData["Types"] = new SelectList(_context.HistoryType, "HistoryTypeID", "Description");
            ViewData["Lots"] = _context.Lots.Where(x => !x.IsArchive).Select(x => x.LotNumber).ToList();
            ViewData["Owners"] = _context.Owner.Where(x => x.IsHoaOwner).Select(x => x.FullName).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string ownerName, string lotNumber) {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (ownerName == null) {
                ModelState.AddModelError("Account", "You must specify an owner.");
                return await OnGetAsync(id);
            }

            var ownerLotNumbers = _context.Lots
                .Where(x => x.Owner.FullName == ownerName).Select(x => x.LotNumber).ToList();
            if (!ownerLotNumbers.Contains(lotNumber)) {
                ModelState.AddModelError("LotNumber", "That lot does not currently belog to that owner.");
                return await OnGetAsync(id);
            }

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            OwnerHistory.LastModifiedBy = user != null ? user.Initials : "SYS";
            OwnerHistory.LastModifiedDate = DateTime.Now;

            OwnerHistory.OwnerID = _context.Owner.FirstOrDefault(o => o.FullName == ownerName).OwnerID;

            if (!string.IsNullOrEmpty(lotNumber)) {
                OwnerHistory.LotID = _context.Lots.FirstOrDefault(l => l.LotNumber == lotNumber).LotID;
            }
            else {
                OwnerHistory.LotID = null;
            }

            _context.Attach(OwnerHistory).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException) {
                if (!OwnerHistoryExists(OwnerHistory.OwnerHistoryID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OwnerHistoryExists(int id) {
            return _context.OwnerHistory.Any(e => e.OwnerHistoryID == id);
        }
    }
}