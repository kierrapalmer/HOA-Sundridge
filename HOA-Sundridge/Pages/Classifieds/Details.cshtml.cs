using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;

namespace HOASunridge.Pages.Classifieds {
    public class DetailsModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public DetailsModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        public ClassifiedListing ClassifiedListing { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ClassifiedListing = await _context.ClassifiedListing
                .Include(c => c.Owner)
                .Include(c => c.Files)
                .Include(c => c.ClassifiedCategory)
                .FirstOrDefaultAsync(m => m.ClassifiedListingID == id).ConfigureAwait(false);

            if (ClassifiedListing == null) {
                return NotFound();
            }
            return Page();
        }

        //Deletes classified
        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ClassifiedListing = await _context.ClassifiedListing.FindAsync(id).ConfigureAwait(false);

            if (ClassifiedListing != null) {
                var files = _context.File.Where(x => x.ClassifiedListingID == id);
                _context.File.RemoveRange(files);
                _context.ClassifiedListing.Remove(ClassifiedListing);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }

            return RedirectToPage("./Index");
        }
    }
}