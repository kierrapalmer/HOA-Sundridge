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

namespace HOASunridge.Pages.Admin.Maintenance {

    public class DeleteModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public DeleteModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Model.Maintenance Maintenance { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Maintenance = await _context.Maintenance
                .Include(m => m.CommonAreaAsset).FirstOrDefaultAsync(m => m.MaintenanceID == id).ConfigureAwait(false);

            if (Maintenance == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Maintenance = await _context.Maintenance.FindAsync(id).ConfigureAwait(false);
            Maintenance.LastModifiedDate = DateTime.Now;
            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Maintenance.LastModifiedBy = user != null ? user.Initials : "SYS";

            Maintenance.IsArchive = true;

            _context.Attach(Maintenance).State = EntityState.Modified;
            if (Maintenance != null) {
                // _context.Maintenance.Remove(Maintenance);

                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}