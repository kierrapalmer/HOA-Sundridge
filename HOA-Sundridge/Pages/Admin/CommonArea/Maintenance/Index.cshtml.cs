using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Context = HOASunridge.Models.HOAContext;
using Model = HOASunridge.Models;

namespace HOASunridge.Pages.Admin.Maintenance {

    public class IndexModel : PageModel {
        private readonly Context _context;

        public IndexModel(Context context) {
            _context = context;
        }

        public PaginatedList<Model.Maintenance> Maintenance { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(int? id, string sortOrder, string currentFilter, string searchString, int? pageIndex) {
            IQueryable<Model.Maintenance> maintenanceIq = _context.Maintenance.Include(m => m.CommonAreaAsset);

            if (id != null && String.IsNullOrEmpty(searchString)) {
                searchString = _context.CommonAreaAsset.FirstOrDefault(m => m.CommonAreaAssetID == id).AssetName;
            }

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
                case "name_desc":
                    maintenanceIq = maintenanceIq.OrderByDescending(s => s.CommonAreaAsset.AssetName);
                    break;

                case "name_asc":
                    maintenanceIq = maintenanceIq.OrderBy(s => s.CommonAreaAsset.AssetName);
                    break;

                case "date_desc":
                    maintenanceIq = maintenanceIq.OrderByDescending(s => s.DateCompleted);
                    break;

                case "date_asc":
                    maintenanceIq = maintenanceIq.OrderBy(s => s.DateCompleted);
                    break;

                case "cost_desc":
                    maintenanceIq = maintenanceIq.OrderByDescending(s => s.Cost);
                    break;

                case "cost_asc":
                    maintenanceIq = maintenanceIq.OrderBy(s => s.Cost);
                    break;

                default:
                    maintenanceIq = maintenanceIq.OrderBy(s => s.DateCompleted);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString)) {
                maintenanceIq = maintenanceIq.Where(s => s.CommonAreaAsset.AssetName.ToLower().Contains(searchString)
                                                 || s.Cost.ToString().Contains(searchString)
                                                 || s.DateCompleted.ToString("MM/dd/yy").Contains(searchString)
                                                 || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.DateCompleted.Month).ToLower().Contains(searchString));
            }

            int pageSize = 25;
            Maintenance = await PaginatedList<Model.Maintenance>.CreateAsync(
                maintenanceIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}