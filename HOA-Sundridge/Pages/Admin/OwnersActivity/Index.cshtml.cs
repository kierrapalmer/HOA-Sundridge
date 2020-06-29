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

namespace HOASunridge.Pages.Admin.OwnersActivity {
    public class IndexModel : PageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        public PaginatedList<OwnerHistory> OwnerHistory { get; set; }
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

            IQueryable<OwnerHistory> ownerHistoryIq = _context.OwnerHistory
                                    .Include(i => i.Owner)
                                    .Include(i => i.Lot)
                                    .Include(i => i.HistoryType)
                                    .Where(s => s.IsArchive == false);;

            switch (sortOrder) {
                case "date_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.Date);
                    break;

                case "date_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.Date);
                    break;

                case "name_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.Owner.FullName);
                    break;

                case "name_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.Owner.FullName);
                    break;

                case "lot_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.Lot.LotNumber);
                    break;

                case "lot_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.Lot.LotNumber);
                    break;

                case "type_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.HistoryType.Description);
                    break;

                case "type_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.HistoryType.Description);
                    break;

                case "privacy_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.PrivacyLevel);
                    break;

                case "privacy_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.PrivacyLevel);
                    break;

                case "description_desc":
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.Description);
                    break;

                case "description_asc":
                    ownerHistoryIq = ownerHistoryIq.OrderBy(o => o.Description);
                    break;

                default:
                    //ordered by the most recently added violation
                    ownerHistoryIq = ownerHistoryIq.OrderByDescending(o => o.Date);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString)) {
                ownerHistoryIq = ownerHistoryIq
                    .Where(o => o.Date.ToString("MM/dd/yyyy").Contains(searchString)
                        || o.Owner.FullName.ToLower().Contains(searchString)
                        || o.Lot.LotNumber.ToString().Contains(searchString)
                        || o.HistoryType.Description.ToLower().Contains(searchString)
                        || o.PrivacyLevel.ToLower().Contains(searchString)
                        || o.Description.ToLower().Contains(searchString)
                        || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(o.Date.Month).ToLower().Contains(searchString)
                );
            }

            int pageSize = 15;
            OwnerHistory = await PaginatedList<OwnerHistory>.CreateAsync(
                ownerHistoryIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
