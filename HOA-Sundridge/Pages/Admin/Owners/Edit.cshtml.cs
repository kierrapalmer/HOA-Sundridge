using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Owners {

    public class EditModel : OwnerContactPageModel {
        private readonly HOAContext _context;

        public EditModel(HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Models.Owner Owner { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Owner = await _context.Owner
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(o => o.OwnerContactType).ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id);

            if (Owner == null) {
                return NotFound();
            }
            ViewData["ContactType"] = new List<ContactType>(_context.ContactType);
            ViewData["State"] = States.GetStateSelectList(Owner.Address?.State);

            var owners = _context.Owner.Where(x => x.IsHoaOwner).Select(x => x.FullName).ToList();
            ViewData["Owners"] = owners;
            ViewData["CoOwner"] = _context.Owner.FirstOrDefault(x => x.OwnerID == Owner.CoOwnerID)?.FullName;

            PopulateAssignedOwnerContacts(_context, Owner);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedContacts, int? isAdmin, string coowner = "") {
            if (!ModelState.IsValid) {
                return Page();
            }
            if (selectedContacts[0] == null || selectedContacts[2] == null) {
                ModelState.AddModelError("Contacts", "You must enter both a primary phone and email.");
                await OnGetAsync(id);
                return Page();
            }

            Models.Owner ownerToUpdate = await _context.Owner
                .Include(o => o.Address)
                .Include(o => o.OwnerContactType).ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id);

            var user = _context.Owner.FirstOrDefault(
                x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            ownerToUpdate.LastModifiedBy = user != null ? user.Initials : "SYS";
            ownerToUpdate.LastModifiedDate = DateTime.Now;

            ownerToUpdate.IsHoaOwner = true;
            ownerToUpdate.IsPrimary = true;

            var owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();
            if (!owners.Contains(coowner) && !string.IsNullOrEmpty(coowner)) {
                CreateCoOwner(coowner);
            }

            ownerToUpdate.CoOwnerID = _context.Owner.FirstOrDefault(x => x.FullName == coowner)?.OwnerID;

            selectedContacts[0] = selectedContacts[0] != null ? Extensions.CleanPhone(selectedContacts[0]) : null;
            selectedContacts[1] = selectedContacts[1] != null ? Extensions.CleanPhone(selectedContacts[1]) : null;
            selectedContacts[3] = selectedContacts[3] != null ? Extensions.CleanPhone(selectedContacts[3]) : null;

            if (await TryUpdateModelAsync<Models.Owner>(
                ownerToUpdate, "Owner",
                o => o.Birthday, o => o.FirstName, o => o.LastName, o => o.Address, o => o.Occupation,
                o => o.IsPrimary, o => o.IsHoaOwner, o => o.EmergencyContactName, o => o.EmergencyContactPhone,
                o => o.LastModifiedBy, o => o.LastModifiedDate)) {
                if (string.IsNullOrWhiteSpace(ownerToUpdate.Address?.FullAddress)) {
                    ownerToUpdate.Address = null;
                }
                ownerToUpdate.EmergencyContactPhone = Extensions.CleanPhone(ownerToUpdate.EmergencyContactPhone);

                var email = selectedContacts[2];        //TODO: not super safe(will break if not in expected order
                UpdateUser(ownerToUpdate.OwnerID, isAdmin, email);

                //saves address
                _context.Attach(ownerToUpdate).State = EntityState.Modified;

                UpdateOwnerContacts(_context, selectedContacts, ownerToUpdate, user);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }

            UpdateOwnerContacts(_context, selectedContacts, ownerToUpdate, user);
            PopulateAssignedOwnerContacts(_context, ownerToUpdate);
            return Page();
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

        public void UpdateUser(int ownerId, int? isAdmin, string email) {
            var updateUser = _context.User.FirstOrDefault(x => x.OwnerID == ownerId);
            updateUser.OwnerID = ownerId;
            updateUser.UserName = email;
            if (isAdmin == 1) {
                updateUser.UserType = "Admin";
            }
            else {
                updateUser.UserType = "User";
            }
            _context.Attach(updateUser).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}