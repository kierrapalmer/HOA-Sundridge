using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HOASunridge.Models;
using HOASunridge.Pages.Classifieds;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace HOASunridge.Pages.Classifieds {

    public class CreateModel : CategoryNamePageModel {
        private readonly HOASunridge.Models.HOAContext _context;
        private readonly IHostingEnvironment _environment;

        public CreateModel(HOASunridge.Models.HOAContext context, IHostingEnvironment environment) {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet() {
            PopulateCategoryDropDownList(_context);
            Owner = _context.Owner
                .Include(x => x.OwnerContactType)
                .ThenInclude(x => x.ContactType)
                .FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            return Page();
        }

        [BindProperty]
        public ClassifiedListing ClassifiedListing { get; set; }

        public Models.Owner Owner { get; set; }

        public int? ID { get; set; }

        public async Task<IActionResult> OnPostAsync(IList<IFormFile> formFiles) {
            if (!ModelState.IsValid) {
                return Page();
            }

            Owner = _context.Owner
                .Include(x => x.OwnerContactType)
                .ThenInclude(x => x.ContactType)
                .FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));

            var emptyClassifiedListing = new ClassifiedListing();
            if (await TryUpdateModelAsync<ClassifiedListing>(
                emptyClassifiedListing,
                "classifiedListing",
                s => s.ClassifiedListingID, s => s.ClassifiedCategoryID, s => s.ItemName, s => s.Price, s => s.Description, s => s.Phone, s => s.Email).ConfigureAwait(false)) {
                emptyClassifiedListing.ListingDate = DateTime.Now;

                emptyClassifiedListing.LastModifiedDate = DateTime.Now;
                emptyClassifiedListing.LastModifiedBy = Owner.FirstName.Substring(0, 1) + Owner.LastName.Substring(0, 1);
                emptyClassifiedListing.OwnerID = Owner.OwnerID;

                if (formFiles != null) {
                    foreach (var file in formFiles) {
                        if (file.ContentType == "image/jpeg" || file.ContentType == "image/jpg" || file.ContentType == "image/png")
                            emptyClassifiedListing.SetImage(file);
                        else {
                            ModelState.AddModelError("file", "Invalid file format. Please only enter jpeg or png.");
                            PopulateCategoryDropDownList(_context);
                            Owner = _context.Owner
                                .Include(x => x.OwnerContactType)
                                .ThenInclude(x => x.ContactType)
                                .FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                            return Page();
                        }
                    }
                }

                _context.ClassifiedListing.Add(emptyClassifiedListing);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return RedirectToPage("./Index");
            }

            PopulateCategoryDropDownList(_context, emptyClassifiedListing.ClassifiedCategoryID);
            return Page();
        }
    }
}