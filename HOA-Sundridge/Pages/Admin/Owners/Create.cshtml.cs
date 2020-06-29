using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using HOASunridge.Pages.Admin.Owners;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Owners {
    public class CreateModel : OwnerContactPageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public CreateModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty] public Models.Owner Owner { get; set; }

        public IActionResult OnGet() {
            ViewData["AddressID"] = new SelectList(_context.Address, "AddressID", "AddressID");
            ViewData["State"] = States.GetStateSelectList();
            ViewData["ContactType"] = new List<ContactType>(_context.ContactType);

            var owners = _context.Owner.Where(x => x.IsHoaOwner).Select(x => x.FullName).ToList();
            ViewData["Owners"] = owners;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string[] selectedContacts, string coowner, int? isAdmin) {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (selectedContacts[0] == null || selectedContacts[2] == null) {
                ModelState.AddModelError("Contacts", "You must enter both a primary phone and email.");
                OnGet();
                return Page();
            }

            var newOwner = new Models.Owner();

            var user = _context.Owner.FirstOrDefault(
                x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            newOwner.LastModifiedBy = user != null ? user.Initials : "SYS";
            newOwner.LastModifiedDate = DateTime.Now;

            newOwner.IsHoaOwner = true;
            newOwner.IsPrimary = true;

            var owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();
            if (!owners.Contains(coowner) && !string.IsNullOrEmpty(coowner)) {
                CreateCoOwner(coowner);
            }

            //Strip number inputs
            selectedContacts[0] = selectedContacts[0] != null ? Extensions.CleanPhone(selectedContacts[0]) : null;
            selectedContacts[1] = selectedContacts[1] != null ? Extensions.CleanPhone(selectedContacts[1]) : null;
            selectedContacts[3] = selectedContacts[3] != null ? Extensions.CleanPhone(selectedContacts[3]) : null;

            newOwner.CoOwnerID = _context.Owner.FirstOrDefault(x => x.FullName == coowner)?.OwnerID;

            if (await TryUpdateModelAsync<Models.Owner>( //this has to be here for a create to work
                newOwner,
                "Owner",
                o => o.Birthday, o => o.FirstName, o => o.LastName, o => o.Address, o => o.Occupation,
                o => o.IsPrimary, o => o.IsHoaOwner, o => o.EmergencyContactName, o => o.EmergencyContactPhone,
                o => o.LastModifiedBy, o => o.LastModifiedDate)) {
                if (string.IsNullOrWhiteSpace(newOwner.Address?.FullAddress)) {
                    newOwner.Address = null;
                }

                newOwner.EmergencyContactPhone = Extensions.CleanPhone(newOwner.EmergencyContactPhone);
                UpdateOwnerContacts(_context, selectedContacts, newOwner, user);

                _context.Owner.Add(newOwner);
                await _context.SaveChangesAsync();
            }

            var phone = selectedContacts[0];
            var email = selectedContacts[2];        //TODO: not super safe(will break if not in expected order
            CreateAccount(newOwner.OwnerID, isAdmin, phone, email);

            return RedirectToPage("./Index");
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

        public void CreateAccount(int ownerId, int? isAdmin, string phone, string email) {
            var newUser = new Models.User();
            var user = _context.Owner.FirstOrDefault(
                x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            newUser.LastModifiedBy = user != null ? user.Initials : "SYS";
            newUser.LastModifiedDate = DateTime.Now;

            newUser.OwnerID = ownerId;
            newUser.UserName = email;
            newUser.UserPassword = Extensions.CalculateSHA256(Extensions.CleanPhone(phone));
            if (isAdmin == 1) {
                newUser.UserType = "Admin";
            }
            else {
                newUser.UserType = "User";
            }
            _context.User.Add(newUser);
            _context.SaveChanges();
        }
    }
}