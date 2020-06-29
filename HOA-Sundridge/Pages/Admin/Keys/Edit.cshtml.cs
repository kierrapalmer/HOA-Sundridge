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

namespace HOASunridge.Pages.Admin.Keys {

    public class EditModel : PageModel {
        private readonly HOAContext _context;

        public EditModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Key Key { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ViewData["OwnerDropdown"] = new SelectList(_context.Owner.Select(
                    o => new { o.OwnerID, o.FirstName, o.LastName, o.FullName }).OrderBy(o => o.LastName).ThenBy(o => o.FirstName),
                "OwnerID", "FullName");
            ViewData["Status"] = new SelectList(_context.KeyHistory.Select(k => k.Status).Distinct());

            var owners = new List<string>();
            owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();

            ViewData["Owners"] = owners;

            Key = await _context.Key
                .Include(s => s.KeyHistory)
                .ThenInclude(s => s.Owner)
                .Where(k => k.KeyID == id)
                .FirstOrDefaultAsync();

            if (Key == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string ownerName = "") {
            if (id == null) {
                return NotFound();
            }

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Key.LastModifiedBy = user != null ? user.Initials : "SYS";
            Key.LastModifiedDate = DateTime.Now;
            _context.Attach(Key).State = EntityState.Modified;

            if (KeyHistoryExists(Key.KeyID, Key.KeyHistory.KeyHistoryID)) {
                Key.LastModifiedBy = user != null ? user.Initials : "SYS";
                Key.KeyHistory.LastModifiedDate = DateTime.Now;
                if ((Key.KeyHistory?.Status == "Returned" || Key.KeyHistory?.Status == "Lost") && Key.KeyHistory?.DateReturned == null)
                    Key.KeyHistory.DateReturned = DateTime.Now;
                if (Key.KeyHistory?.Status == "Active")
                    Key.KeyHistory.DateReturned = null;
                Key.KeyHistory.OwnerID = _context.Owner.FirstOrDefault(x => x.FullName == ownerName).OwnerID;
                _context.Attach(Key.KeyHistory).State = EntityState.Modified;
            }
            else {
                KeyHistory keyHistory = Key.KeyHistory;
                _context.KeyHistory.Add(keyHistory);
            }

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!KeyExists(Key.KeyID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool KeyExists(int id) {
            return _context.Key.Any(e => e.KeyID == id);
        }

        private bool KeyHistoryExists(int keyId, int histId) {
            return _context.Key.Any(e => e.KeyID == keyId && e.KeyHistory.KeyHistoryID == histId);
        }
    }
}