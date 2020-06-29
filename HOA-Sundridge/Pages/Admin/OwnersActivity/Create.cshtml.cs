using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.OwnersActivity {

    public class CreateModel : PageModel {
        private readonly HOAContext _context;

        public CreateModel(HOAContext context) {
            _context = context;
        }

        public IActionResult OnGet() {
            ViewData["PrivacyLevel"] = new SelectList(new List<SelectListItem>
            {
                new SelectListItem{Value = "Internal", Text = "Internal"},
                new SelectListItem{Value = "Public", Text = "Public"}
            }, "Value", "Text");
            ViewData["Types"] = new SelectList(_context.HistoryType, "HistoryTypeID", "Description");
            ViewData["Lots"] = _context.Lots.Where(x => x.IsArchive == false).Select(x => x.LotNumber).ToList();
            ViewData["Owners"] = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();

            return Page();
        }

        [BindProperty]
        public OwnerHistory OwnerHistory { get; set; }

        public async Task<IActionResult> OnPostAsync(string ownerName, string lotNumber) {
            if (!ModelState.IsValid) {
                return Page();
            }
            if (ownerName == null) {
                ModelState.AddModelError("Account", "You must specify an owner.");
                return OnGet();
            }

            var ownerLotNumbers = _context.Lots
                .Where(x => x.Owner.FullName == ownerName).Select(x => x.LotNumber).ToList();
            if (!ownerLotNumbers.Contains(lotNumber) && lotNumber != null) {
                ModelState.AddModelError("LotNumber", "That lot does not currently belog to that owner.");
                return OnGet();
            }

            if (string.IsNullOrEmpty(ownerName)) {
                ModelState.AddModelError("ownerName", "You must specify a associated owner.");

                ViewData["PrivacyLevel"] = new SelectList(new List<SelectListItem>
                {
                    new SelectListItem{Value = "Internal", Text = "Internal"},
                    new SelectListItem{Value = "Public", Text = "Public"}
                }, "Value", "Text");
                ViewData["Types"] = new SelectList(_context.HistoryType, "HistoryTypeID", "Description");
                ViewData["Lots"] = _context.Lots.Where(x => x.IsArchive == false).Select(x => x.LotNumber).ToList();
                ViewData["Owners"] = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();

                return Page();
            }

            var newOwnerHistory = new OwnerHistory();
            newOwnerHistory.OwnerID = _context.Owner.FirstOrDefault(o => o.FullName == ownerName).OwnerID;

            if (!string.IsNullOrEmpty(lotNumber)) {
                newOwnerHistory.LotID = _context.Lots.FirstOrDefault(l => l.LotNumber == lotNumber).LotID;
            }
            else {
                newOwnerHistory.LotID = null;
            }

            if (await TryUpdateModelAsync<OwnerHistory>(
                newOwnerHistory,
                "ownerHistory",
                o => o.HistoryTypeID, o => o.LotID, o => o.OwnerID, o => o.Date, o => o.Description, o => o.PrivacyLevel)) {
                var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                newOwnerHistory.LastModifiedBy = user != null ? user.Initials : "SYS";
                newOwnerHistory.LastModifiedDate = DateTime.Now;

                _context.OwnerHistory.Add(newOwnerHistory);
                await _context.SaveChangesAsync();
            }
            else {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}