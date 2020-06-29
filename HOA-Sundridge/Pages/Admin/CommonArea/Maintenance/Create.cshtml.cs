using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Pages.Admin.Maintenance;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Context = HOASunridge.Models.HOAContext;
using Model = HOASunridge.Models;

namespace HOASunridge.Pages.Admin.Maintenance {
    public class CreateModel : AssetNamePageModel {
        private readonly Context _context;

        public CreateModel(Context context) {
            _context = context;
        }

        public IActionResult OnGet(int? id) {
            if (id == null) {
                PopulateAssetsDropDownList(_context);
            }
            else {
                var commonAreaAssetName =
                    _context.CommonAreaAsset.FirstOrDefault(m => m.CommonAreaAssetID == id).AssetName;
                PopulateAssetsDropDownList(_context, commonAreaAssetName);
            }

            return Page();
        }

        [BindProperty]
        public Model.Maintenance Maintenance { get; set; }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            var emptyMaintenance = new Model.Maintenance();
            if (await TryUpdateModelAsync<Model.Maintenance>(
                emptyMaintenance,
                "maintenance", // Prefix for form value.
                s => s.MaintenanceID, s => s.CommonAreaAssetID, s => s.Description, s => s.Cost, s => s.DateCompleted).ConfigureAwait(false)) {
                emptyMaintenance.LastModifiedDate = DateTime.Now;
                var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                emptyMaintenance.LastModifiedBy = user != null ? user.Initials : "SYS";

                _context.Maintenance.Add(emptyMaintenance);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToPage("./Index");
            }

            PopulateAssetsDropDownList(_context, emptyMaintenance.CommonAreaAssetID);
            return Page();
        }
    }
}