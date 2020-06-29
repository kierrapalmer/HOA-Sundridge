using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Transactions {

    public class DeleteModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public DeleteModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Transaction = await _context.Transaction
                .Include(t => t.Owner)
                .Include(t => t.TransactionType).FirstOrDefaultAsync(m => m.TransactionID == id).ConfigureAwait(false);

            if (Transaction == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Transaction = await _context.Transaction.FindAsync(id).ConfigureAwait(false);
            Transaction.IsArchive = true;

            Transaction.LastModifiedDate = DateTime.Now;

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Transaction.LastModifiedBy = user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1);

            if (Transaction != null) {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}