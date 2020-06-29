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

namespace HOASunridge.Pages.Admin.Keys {

    public class IndexModel : PageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        public PaginatedList<Key> Keys { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex) {
            if (searchString != null) {
                pageIndex = 1;
            }
            else {
                searchString = currentFilter;
            }

            searchString = searchString?.ToLower();
            CurrentFilter = searchString;
            CurrentSort = sortOrder;

            IQueryable<Key> keyIq = _context.Key
                .Include(s => s.KeyHistory)
                .ThenInclude(s => s.Owner)
                .Where(s => s.IsArchive == false);

            switch (sortOrder) {
                case "serialNum_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyID);
                    break;

                case "serialNum_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyID);
                    break;

                case "issued_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyHistory.DateIssued);
                    break;

                case "issued_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.DateIssued);
                    break;

                case "returned_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyHistory.DateReturned);
                    break;

                case "returned_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.DateReturned);
                    break;

                case "status_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyHistory.Status);
                    break;

                case "status_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.Status);
                    break;

                case "amount_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyHistory.PaidAmount);
                    break;

                case "amount_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.PaidAmount);
                    break;

                case "owner_desc":
                    keyIq = keyIq.OrderByDescending(k => k.KeyHistory.Owner.FullName);
                    break;

                case "owner_asc":
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.Owner.FullName);
                    break;

                default:
                    keyIq = keyIq.OrderBy(k => k.KeyHistory.Status);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString)) {
                keyIq = keyIq.Where(k => k.SerialNumber.ToString().ToLower().Contains(searchString)
                                         || k.KeyHistory.DateIssued.ToString("MM/dd/yy").Contains(searchString)
                                         || k.KeyHistory.DateReturned.ToString().Contains(searchString)
                                         || k.KeyHistory.Status.ToLower().Contains(searchString)
                                         || k.KeyHistory.PaidAmount.ToString().Contains(searchString)
                                         || k.KeyHistory.Owner.FullName.ToLower().Contains(searchString)
                                         || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(k.KeyHistory.DateIssued.Month).ToLower().Contains(searchString)
                                         || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(k.KeyHistory.DateReturned != null ? ((DateTime)k.KeyHistory.DateReturned).Month : k.KeyHistory.DateIssued.Month).ToLower().Contains(searchString) //getMonthName() has to have a value to return
                                       );
            }

            int pageSize = 15;
            Keys = await PaginatedList<Key>.CreateAsync(
                keyIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
