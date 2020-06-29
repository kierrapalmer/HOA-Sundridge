using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace HOASunridge.Pages {

    public class OwnerProfileModel : PageModel {
        private readonly HOAContext _context;

        public OwnerProfileModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Models.Owner Owner { get; set; }

        public IList<KeyHistory> KeyHistories { get; set; }
        public IList<OwnerHistory> OwnerHistories { get; set; }
        public IList<Lot> Lots { get; set; }
        public Models.Owner SignedInOwner { get; set; }
        public IList<OwnerContactType> Phone { get; set; }
        public bool PasswordIsDefault = false;

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }
            var authID = HttpContext.Session?.GetInt32("SessionUserID");
            if (authID != null) {
                SignedInOwner = _context.User
                    .Include(y => y.Owner)
                    .FirstOrDefault(x => x.UserID == authID).Owner;
            }

            if (SignedInOwner == null)
                RedirectToPage("./User/Login");

            Owner = await _context.Owner
                .Include(x => x.User)
                .Include(a => a.Address)
                .Include(oc => oc.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id); // This cannot be session user id because we are not looking at owners based on who is currently logged in. CL

            KeyHistories = await _context.KeyHistory
                .Include(s => s.Key)
                .Where(x => x.OwnerID == Owner.OwnerID)
                .ToListAsync();

            OwnerHistories = await _context.OwnerHistory
                .Include(i => i.HistoryType)
                .Include(i => i.Owner)
                .Include(i => i.Lot)
                .Where(x => x.OwnerID == Owner.OwnerID)
                .ToListAsync().ConfigureAwait(false);

            Lots = await _context.Lots
                .Include(l => l.LotInventory)
                .ThenInclude(i => i.Inventory)
                .Where(x => x.OwnerID == Owner.OwnerID && x.Status == "Occupied")
                .ToListAsync();

            Phone = await _context.OwnerContactType
                .Where(c => c.OwnerID == id && c.ContactTypeID == 1) //should grab the primary phone number associated with the accound id we are looking at. CL
                .ToListAsync();

            ViewData["State"] = States.GetStateSelectList();
            ViewData["CoOwner"] = _context.Owner.FirstOrDefault(x => x.OwnerID == Owner.CoOwnerID)?.FullName;

            foreach (var p in Phone) {
                if (p.OwnerID == Owner.OwnerID) //Checks to see if the owner is the one we want (the profile page we are on)
                {
                    if (Extensions.CalculateSHA256(Extensions.CleanPhone(p.ContactValue)) == Owner.User.UserPassword) // This compares the hashed password to the hashed phone number to see if the password is set to default. CL
                    {
                        PasswordIsDefault = true; // This dictates the display of YES or NO
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string primaryphone, string secondaryphone, string email, string fax, string coowner, string emergencyphone) {
            Owner = await _context.Owner
                .Include(x => x.User)
                .Include(a => a.Address)
                .Include(oc => oc.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id); // This cannot be session user id because we are not looking at owners based on who is currently logged in. CL

            if (!ModelState.IsValid) {
                return Page();
            }

            var owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();
            if (!owners.Contains(coowner) && !string.IsNullOrEmpty(coowner)) {
                CreateCoOwner(coowner);
            }

            Owner.CoOwnerID = _context.Owner.FirstOrDefault(x => x.FullName == coowner)?.OwnerID;

            var ownerToUpdate = await _context.Owner
                .Include(x => x.User)
                .Include(a => a.Address)
                .Include(oc => oc.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id).ConfigureAwait(false);

            if (await TryUpdateModelAsync<HOASunridge.Models.Owner>(
                ownerToUpdate,
                "owner",
                c => c.Address, c => c.OwnerContactType, c => c.Birthday, c => c.Occupation, c => c.EmergencyContactName)) {
                if (!String.IsNullOrWhiteSpace(primaryphone))
                    ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Primary Phone")
                        .ContactValue = Extensions.CleanPhone(primaryphone);
                if (!String.IsNullOrWhiteSpace(secondaryphone))
                    ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Secondary Phone")
                    .ContactValue = Extensions.CleanPhone(secondaryphone);
                if (!String.IsNullOrWhiteSpace(email))
                    ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Email")
                    .ContactValue = email;

                ownerToUpdate.EmergencyContactPhone = Extensions.CleanPhone(emergencyphone);

                await _context.SaveChangesAsync();
                return Redirect("/Owner/Profile/?id=" + Owner.OwnerID);
            }

            return Redirect("/Owner/Profile?id=" + Owner.OwnerID);
        }

        private bool OwnerExists(int id) {
            return _context.Owner.Any(e => e.OwnerID == id);
        }

        public void CreateCoOwner(string coowner) {
            var newOwner = new Models.Owner();

            var user = _context.Owner.FirstOrDefault(
                x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            newOwner.LastModifiedBy = user != null ? user.Initials : "SYS";
            newOwner.LastModifiedDate = DateTime.Now;

            newOwner.IsHoaOwner = true;
            newOwner.IsPrimary = false;

            newOwner.FirstName = coowner.IndexOf(" ") > -1
                ? coowner.Substring(0, coowner.IndexOf(" "))
                : coowner;
            newOwner.LastName = coowner.IndexOf(" ") > -1
                ? coowner.Substring(coowner.IndexOf(" ") + 1)
                : "";

            _context.Owner.Add(newOwner);
            _context.SaveChanges();
        }
    }
}