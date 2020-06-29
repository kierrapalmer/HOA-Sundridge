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
using Model = HOASunridge.Models.Maintenance;

namespace HOASunridge.Pages.Admin.Maintenance {
    public class EditModel : AssetNamePageModel {
        private readonly Context _context;

        public EditModel(Context context) {
            _context = context;
        }

        [BindProperty]
        public Model Maintenance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Maintenance = await _context.Maintenance
                .Include(m => m.CommonAreaAsset).FirstOrDefaultAsync(m => m.MaintenanceID == id).ConfigureAwait(false);

            if (Maintenance == null) {
                return NotFound();
            }
            PopulateAssetsDropDownList(_context, Maintenance.CommonAreaAsset.AssetName);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            var maintenance = await _context.Maintenance.FindAsync(id).ConfigureAwait(false);

            if (await TryUpdateModelAsync<Model>(
                maintenance,
                "maintenance",
                c => c.Description, c => c.CommonAreaAssetID, c => c.Cost, c => c.DateCompleted).ConfigureAwait(false)) {
                maintenance.LastModifiedDate = DateTime.Now;
                var user = _context.Owner?.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                maintenance.LastModifiedBy = user != null ? user.Initials : "SYS";

                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToPage("./Index");
            }

            PopulateAssetsDropDownList(_context, maintenance.CommonAreaAssetID);
            return Page();
        }

        private bool MaintenanceExists(int id) {
            return _context.Maintenance.Any(e => e.MaintenanceID == id);
        }
    }
}