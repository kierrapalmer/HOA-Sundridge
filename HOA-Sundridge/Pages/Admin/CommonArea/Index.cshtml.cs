using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Context = HOASunridge.Models.HOAContext;
using Model = HOASunridge.Models;

namespace HOASunridge.Pages.Admin.CommonAreaAsset {

    public class IndexModel : PageModel {
        private readonly Context _context;

        public IndexModel(Context context) {
            _context = context;
        }

        [BindProperty]
        public Model.Owner Owner { get; set; }

        public PaginatedList<Model.CommonAreaAsset> CommonAreaAsset { get; set; }
        public Model.CommonAreaAsset Item { get; set; }

        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex) {
            IQueryable<Model.CommonAreaAsset> assetIq = _context.CommonAreaAsset;

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
                    assetIq = assetIq.OrderByDescending(s => s.AssetName);
                    break;

                case "name_asc":
                    assetIq = assetIq.OrderBy(s => s.AssetName);
                    break;

                case "date_desc":
                    assetIq = assetIq.OrderByDescending(s => s.Date);
                    break;

                case "date_asc":
                    assetIq = assetIq.OrderBy(s => s.Date);
                    break;

                case "cost_desc":
                    assetIq = assetIq.OrderByDescending(s => s.PurchasePrice);
                    break;

                case "cost_asc":
                    assetIq = assetIq.OrderBy(s => s.PurchasePrice);
                    break;

                case "status_desc":
                    assetIq = assetIq.OrderByDescending(s => s.Status);
                    break;

                case "status_asc":
                    assetIq = assetIq.OrderBy(s => s.Status);
                    break;

                default:
                    assetIq = assetIq.OrderBy(s => s.Status);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString)) {
                assetIq = assetIq.Where(s => s.AssetName.ToLower().Contains(searchString)
                                                 || s.PurchasePrice.ToString().Contains(searchString)
                                                 || s.Date.ToString("YYYY-MM-DD").Contains(searchString)
                                                 || CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(s.Date.Month).ToLower().Contains(searchString));
            }

            int pageSize = 10;
            CommonAreaAsset = await PaginatedList<Model.CommonAreaAsset>.CreateAsync(
                assetIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Item = await _context.CommonAreaAsset.FindAsync(id);
            Item.Status = "Paid";
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