using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Context = HOASunridge.Models.HOAContext;
using Model = HOASunridge.Models;

namespace HOASunridge.Pages.Admin.CommonArea {

    public class ChangeStatusModel : PageModel {
        private readonly Context _context;

        public ChangeStatusModel(Context context) {
            _context = context;
        }

        [BindProperty]
        public Model.Owner Owner { get; set; }

        [BindProperty]
        public Model.CommonAreaAsset CommonAreaAsset { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            CommonAreaAsset = await _context.CommonAreaAsset.FirstOrDefaultAsync(m => m.CommonAreaAssetID == id).ConfigureAwait(false);

            if (CommonAreaAsset == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            CommonAreaAsset = await _context.CommonAreaAsset.FindAsync(id).ConfigureAwait(false);

            CommonAreaAsset.Status = (CommonAreaAsset.Status == "Active") ? "Disabled" : "Active";
            CommonAreaAsset.LastModifiedDate = DateTime.Now;
            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            CommonAreaAsset.LastModifiedBy = user != null ? user.Initials : "SYS";

            if (CommonAreaAsset != null) {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}