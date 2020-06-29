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

    public class CreateModel : PageModel {
        private readonly Context _context;

        public CreateModel(Context context) {
            _context = context;
        }

        public DateTime Today { get; set; }

        [BindProperty]
        public Model.CommonAreaAsset CommonAreaAsset { get; set; }

        public IActionResult OnGet() {
            Today = DateTime.Now;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            CommonAreaAsset.Status = "Active";
            CommonAreaAsset.LastModifiedDate = DateTime.Now;
            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            CommonAreaAsset.LastModifiedBy = user != null ? user.Initials : "SYS";

            _context.CommonAreaAsset.Add(CommonAreaAsset);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Index");
        }
    }
}