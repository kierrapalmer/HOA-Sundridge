using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Context = HOASunridge.Models.HOAContext;
using Model = HOASunridge.Models;

namespace HOASunridge.Pages.Admin.CommonArea {

    public class EditModel : PageModel {
        private readonly Context _context;

        public EditModel(Context context) {
            _context = context;
        }

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
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Attach(CommonAreaAsset).State = EntityState.Modified;
            CommonAreaAsset.Status = "Active";
            CommonAreaAsset.LastModifiedDate = DateTime.Now;
            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            CommonAreaAsset.LastModifiedBy = user != null ? user.Initials : "SYS";

            try {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException) {
                if (!CommonAreaAssetExists(CommonAreaAsset.CommonAreaAssetID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool CommonAreaAssetExists(int id) {
            return _context.CommonAreaAsset.Any(e => e.CommonAreaAssetID == id);
        }
    }
}