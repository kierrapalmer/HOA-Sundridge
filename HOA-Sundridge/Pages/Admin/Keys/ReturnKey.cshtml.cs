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

    public class ReturnKeyModel : PageModel {
        private readonly HOAContext _context;

        public ReturnKeyModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Key Key { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            var uId = HttpContext.Session.GetInt32("SessionUserID");
            if (uId == null)
                return RedirectToPage("./User/Login");

            if (id == null) {
                return NotFound();
            }

            Key = await _context.Key
                .Include(s => s.KeyHistory)
                .Where(k => k.KeyID == id)
                .FirstOrDefaultAsync().ConfigureAwait(false);

            if (Key == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            var uId = HttpContext.Session.GetInt32("SessionUserID");
            if (uId == null)
                return RedirectToPage("./User/Login");

            if (id == null) {
                return NotFound();
            }
            if (!ModelState.IsValid) {
                return Redirect("./Index");
            }

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Key.LastModifiedBy = user != null ? user.Initials : "SYS";
            Key.LastModifiedDate = DateTime.Now;

            Key.KeyHistory.Status = "Returned";
            Key.KeyHistory.DateReturned = DateTime.Now;
            _context.Attach(Key).State = EntityState.Modified;

            if (KeyHistoryExists(Key.KeyID, Key.KeyHistory.KeyHistoryID)) {
                Key.KeyHistory.LastModifiedBy = user != null ? user.Initials : "SYS";
                Key.KeyHistory.LastModifiedDate = DateTime.Now;
                Key.KeyHistory.Status = "Returned";
                _context.Attach(Key.KeyHistory).State = EntityState.Modified;
            }
            else {
                Key.KeyHistory = null;
            }

            try {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException) {
                if (!KeyExists(Key.KeyID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return Redirect("./Index");
        }

        private bool KeyExists(int id) {
            return _context.Key.Any(e => e.KeyID == id);
        }

        private bool KeyHistoryExists(int keyId, int histId) {
            return _context.Key.Any(e => e.KeyID == keyId && e.KeyHistory.KeyHistoryID == histId);
        }
    }
}