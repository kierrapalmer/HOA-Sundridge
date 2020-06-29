using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.OwnersActivity {

    public class DeleteModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public DeleteModel(HOASunridge.Models.HOAContext context) {
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
                .Include(o => o.HistoryType)
                .FirstOrDefaultAsync(m => m.OwnerHistoryID == id);

            if (OwnerHistory == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            OwnerHistory = await _context.OwnerHistory.FindAsync(id).ConfigureAwait(false);

            if (OwnerHistory != null) {
                var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                OwnerHistory.LastModifiedBy = user != null ? user.Initials : "SYS";
                OwnerHistory.LastModifiedDate = DateTime.Now;
                OwnerHistory.IsArchive = true;
                _context.Attach(OwnerHistory).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}