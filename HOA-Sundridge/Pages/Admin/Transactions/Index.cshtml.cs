using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Transactions {

    public class IndexModel : PageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        public PaginatedList<Transaction> Transaction { get; set; }
        public Transaction Item { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string OpenBalance { get; set; }

        public async Task OnGetAsync(int? id, string sortOrder, string currentFilter, string searchString, int? pageIndex) {
            IQueryable<Transaction> transactionsIq = _context.Transaction.Include(t => t.Owner).Include(t => t.TransactionType).Where(x => x.IsArchive == false);

            if (searchString != null) {
                pageIndex = 1;
            }
            else {
                searchString = currentFilter;
            }

            searchString = searchString?.ToLower();
            CurrentFilter = searchString;
            CurrentSort = sortOrder;

            switch (sortOrder) {
                case "type_desc":
                    transactionsIq = transactionsIq.OrderByDescending(s => s.TransactionType.Description);
                    break;

                case "type_asc":
                    transactionsIq = transactionsIq.OrderBy(s => s.TransactionType.Description);
                    break;

                case "date_added_desc":
                    transactionsIq = transactionsIq.OrderByDescending(s => s.DateAdded);
                    break;

                case "date_added_asc":
                    transactionsIq = transactionsIq.OrderBy(s => s.DateAdded);
                    break;

                case "amount_desc":
                    transactionsIq = transactionsIq.OrderByDescending(s => s.Amount);
                    break;

                case "amount_asc":
                    transactionsIq = transactionsIq.OrderBy(s => s.Amount);
                    break;

                case "status_desc":
                    transactionsIq = transactionsIq.OrderByDescending(s => s.Status);
                    break;

                case "status_asc":
                    transactionsIq = transactionsIq.OrderBy(s => s.Status);
                    break;

                case "account_asc":
                    transactionsIq = transactionsIq.OrderBy(s => s.Owner.LastName);
                    break;

                case "account_desc":
                    transactionsIq = transactionsIq.OrderByDescending(s => s.Owner.LastName);
                    break;

                default:
                    transactionsIq = transactionsIq.OrderBy(a => a.Status == "Open" ? 1 : 2)
                        .ThenBy(s => s);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString)) {
                transactionsIq = transactionsIq.Where(s => s.TransactionType.Description.ToLower().Contains(searchString)
                                                 || s.Status.ToLower().Contains(searchString)
                                                 || s.Amount.ToString().Contains(searchString)
                                                 || s.DateAdded.ToString("MM/dd/yy").Contains(searchString)
                                                 || s.Owner.FullName.Contains(searchString)
                                                 || (s.DatePaid == null ? "" : ((DateTime)s.DatePaid).ToString("MM/dd/yy")).Contains(searchString)
                                                 || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.DateAdded.Month).ToLower().Contains(searchString)
                                                 || s.Owner.FullName.ToLower().Contains(searchString));
            }

            int pageSize = 15;
            Transaction = await PaginatedList<Transaction>.CreateAsync(
                transactionsIq.AsNoTracking(), pageIndex ?? 1, pageSize);

            OpenBalance = $"{_context.Transaction.Where(x => x.Status == "Open" && x.IsArchive != true).Select(x => x.Amount).Sum():C}";
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Item = await _context.Transaction.FindAsync(id);
            Item.Status = "Paid";
            Item.DatePaid = DateTime.Now;

            Item.LastModifiedDate = DateTime.Now;
            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Item.LastModifiedBy = user != null ? user.Initials : "SYS";

            if (Item != null) {
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}