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

namespace HOASunridge.Pages.Admin.Transactions {

    public class EditModel : TypeNamePageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public EditModel(HOASunridge.Models.HOAContext context) {
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
                .Include(t => t.TransactionType).FirstOrDefaultAsync(m => m.TransactionID == id);

            if (Transaction == null) {
                return NotFound();
            }

            var owners = new List<string>();
            owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();

            ViewData["Owners"] = owners;
            PopulateTransactionDropDownList(_context);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string ownerName = "") {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (ownerName == null) {
                ModelState.AddModelError("Account", "You must specify a associated account.");
                await OnGetAsync(id);
            }

            var transaction = await _context.Transaction.FindAsync(id);

            if (await TryUpdateModelAsync<Transaction>(transaction, "transaction",
                s => s.TransactionID, s => s.TransactionTypeID, s => s.Status, s => s.Description, s => s.Amount)) {
                transaction.OwnerID = _context.Owner.FirstOrDefault(x => x.FullName == ownerName).OwnerID;

                if (transaction.DatePaid == null && transaction.Status == "Paid")
                    transaction.DatePaid = DateTime.Now;
                if (transaction.Status == "Open")
                    transaction.DatePaid = null;

                transaction.LastModifiedDate = DateTime.Now;
                var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                Transaction.LastModifiedBy = user != null ? user.Initials : "SYS";

                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateTransactionDropDownList(_context, transaction.TransactionTypeID);

            return Page();
        }
    }
}