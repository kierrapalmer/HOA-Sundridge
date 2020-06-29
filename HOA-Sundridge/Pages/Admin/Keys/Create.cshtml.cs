using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Keys {

    public class CreateModel : PageModel {
        private readonly HOAContext _context;

        public CreateModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Key Key { get; set; }

        public IActionResult OnGet() {
            ViewData["OwnerDropdown"] = new SelectList(_context.Owner.Select(
                o => new { o.OwnerID, o.FirstName, o.LastName, o.FullName }).OrderBy(o => o.LastName).ThenBy(o => o.FirstName),
                "OwnerID", "FullName");

            var owners = new List<string>();
            owners = _context.Owner.Select(x => x.FullName).ToList();

            ViewData["Owners"] = owners;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string ownerName = "") {
            if (!ModelState.IsValid) {
                return Page();
            }

            IQueryable<Models.Owner> ownerIQ = from u in _context.Owner select u;

            bool temp = false;

            foreach (var own in ownerIQ)
            {
                if (own.FullName == ownerName)
                {
                    temp = true;
                    break;
                }
            }


            var owners = _context.Owner.Select(x => x.FullName).ToList();
            if (string.IsNullOrEmpty(ownerName)) {
                ModelState.AddModelError("OwnerName", "You must specify a associated owner or third party affiliate.");

                ViewData["Owners"] = owners;
                return Page();
            }

            if (temp == false)
            {
                ModelState.AddModelError("OwnerName", "That associated owner or third party affiliate doesn't exist.");

                ViewData["Owners"] = owners;
                return Page();
            }

            /* For version 2.0: Implement the ability to create a new affiliate when entering a name not currently in the database. Most the code is here already, we just ran out of time. CL
            if (!owners.Contains(ownerName) && !string.IsNullOrEmpty(ownerName)) {
                CreateAffiliate(ownerName);
            }
            */

            var newKey = new Key();

            if (await TryUpdateModelAsync<Key>(
                newKey,
                "key",
                k => k.SerialNumber)) {
                var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
                Key.LastModifiedBy = user != null ? user.Initials : "SYS";
                Key.LastModifiedDate = DateTime.Now;
                Key.KeyHistory.Status = "Active";
                Key.KeyHistory.OwnerID = _context.Owner.FirstOrDefault(o => o.FullName == ownerName).OwnerID;
                _context.Key.Add(Key);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        public void CreateAffiliate(string affiliate) {
            var newOwner = new Models.Owner();

            var user = _context.Owner.FirstOrDefault(
                x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            newOwner.LastModifiedBy = user != null ? user.Initials : "SYS";
            newOwner.LastModifiedDate = DateTime.Now;

            newOwner.IsHoaOwner = false;
            newOwner.IsPrimary = false;

            newOwner.FirstName = affiliate.IndexOf(" ") > -1
                ? affiliate.Substring(0, affiliate.IndexOf(" "))
                : affiliate;
            newOwner.LastName = affiliate.IndexOf(" ") > -1
                ? affiliate.Substring(affiliate.IndexOf(" ") + 1)
                : "";

            _context.Owner.Add(newOwner);
            _context.SaveChanges();
        }
    }
}