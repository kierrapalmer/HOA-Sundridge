using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HOASunridge.Models;

namespace HOASunridge.Pages.Admin {

    public class CreateModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public CreateModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Lot Lot { get; set; }

        public IActionResult OnGet() {
            ViewData["AddressID"] = new SelectList(_context.Address, "AddressID", "AddressID");

            Lot = new Lot();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync() {
            if (!ModelState.IsValid) {
                return Page();
            }

            _context.Lots.Add(Lot);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return RedirectToPage("./Index");
        }
    }
}