using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Transactions {
    public class CreateModel : TypeNamePageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public CreateModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        public IActionResult OnGet() {
            ViewData["OwnerID"] = new SelectList(_context.Owner, "OwnerID", "OwnerID");
            var owners = new List<string>();
            owners = _context.Owner.Where(x => x.IsHoaOwner).Select(x => x.FullName).ToList();

            ViewData["Owners"] = owners;

            PopulateTransactionDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Transaction Transaction { get; set; }

        public async Task<IActionResult> OnPostAsync(int? allOwners, string ownerName = "") {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (allOwners == null && ownerName == null) {
                ModelState.AddModelError("Account", "You must specify at least one associated account.");
                OnGet();
            }

            var emptyTransaction = new Transaction();

            if (allOwners == 1) {
                foreach (var ownerId in _context.Owner.Where(x => x.IsPrimary == true).Select(x => x.OwnerID)) {
                    emptyTransaction = new Transaction();

                    if (await TryUpdateModelAsync<Transaction>(emptyTransaction, "transaction", // Prefix for form value.
                        s => s.TransactionID, s => s.TransactionTypeID, s => s.Description, s => s.Amount).ConfigureAwait(false)) {
                        emptyTransaction.Status = "Open";

                        emptyTransaction.DateAdded = DateTime.Now;
                        emptyTransaction.OwnerID = ownerId;

                        emptyTransaction.LastModifiedDate = DateTime.Now;
                        var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                        emptyTransaction.LastModifiedBy = user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1);

                        _context.Transaction.Add(emptyTransaction);
                    }
                }
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return RedirectToPage("./Index");
            }
            else {
                if (await TryUpdateModelAsync<Transaction>(emptyTransaction, "transaction",
                    s => s.TransactionID, s => s.TransactionTypeID, s => s.Description, s => s.Amount).ConfigureAwait(false)) {
                    emptyTransaction.Status = "Open";

                    emptyTransaction.DateAdded = DateTime.Now;
                    emptyTransaction.OwnerID = _context.Owner.FirstOrDefault(x => x.FullName == ownerName).OwnerID;

                    emptyTransaction.LastModifiedDate = DateTime.Now;
                    var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                    emptyTransaction.LastModifiedBy = user.FirstName.Substring(0, 1) + user.LastName.Substring(0, 1);

                    _context.Transaction.Add(emptyTransaction);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                    return RedirectToPage("./Index");
                }
            }

            PopulateTransactionDropDownList(_context, emptyTransaction.TransactionTypeID);
            return Page();
        }
    }
}