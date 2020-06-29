using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Classifieds {
    public class EditModel : CategoryNamePageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public EditModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty] public ClassifiedListing ClassifiedListing { get; set; }

        [FromRoute]
        public int? Id { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            ClassifiedListing = await _context.ClassifiedListing
                .Include(c => c.Owner).Include(c => c.Files).FirstOrDefaultAsync(m => m.ClassifiedListingID == id).ConfigureAwait(false);

            if (ClassifiedListing == null) {
                return NotFound();
            }

            PopulateCategoryDropDownList(_context);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, IList<IFormFile> formFiles) {
            if (!ModelState.IsValid) {
                return Page();
            }

            var classifiedListing = _context.ClassifiedListing.FirstOrDefault(i => i.ClassifiedListingID == id);

            if (await TryUpdateModelAsync<ClassifiedListing>(
                classifiedListing,
                "classifiedListing",
                s => s.ClassifiedListingID, s => s.ClassifiedCategoryID, s => s.ItemName, s => s.Price, s => s.Description, s => s.Phone, s => s.Email).ConfigureAwait(false)) {
                classifiedListing.LastModifiedDate = DateTime.Now;
                classifiedListing.ListingDate = DateTime.Now;

                var owner = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                classifiedListing.LastModifiedBy = owner.FirstName.Substring(0, 1) + owner.LastName.Substring(0, 1);
                classifiedListing.OwnerID = owner.OwnerID;

                if (formFiles.Any()) {
                    var files = _context.File.Where(x => x.ClassifiedListingID == id);
                    if (files.Any())
                        _context.File.RemoveRange(files);

                    foreach (var file in formFiles) {
                        if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png")
                            classifiedListing.SetImage(file);
                        else {
                            ModelState.AddModelError("file", "Invalid file format. Please only enter jpeg or png.");
                            ClassifiedListing = await _context.ClassifiedListing
                                .Include(c => c.Owner).Include(c => c.Files).FirstOrDefaultAsync(m => m.ClassifiedListingID == id);
                            PopulateCategoryDropDownList(_context);
                            return Page();
                        }
                    }
                }
                else {
                    ClassifiedListing c = _context.ClassifiedListing.AsQueryable().AsNoTracking().FirstOrDefault(x => x.ClassifiedListingID == id);
                    classifiedListing.Files = c.Files;
                }

                _context.Attach(classifiedListing).State = EntityState.Modified;

                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToPage("./Index");
            }

            PopulateCategoryDropDownList(_context, classifiedListing.ClassifiedCategoryID);
            return Page();
        }

        //Deletes the classified
        public async Task<IActionResult> OnPostDeleteAsync(int? id) {
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

        private bool ClassifiedListingExists(int id) {
            return _context.ClassifiedListing.Any(e => e.ClassifiedListingID == id);
        }
    }
}