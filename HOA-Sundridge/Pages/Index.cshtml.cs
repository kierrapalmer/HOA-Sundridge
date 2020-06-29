using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HOASunridge.Models;
using HOASunridge.Pages.Admin.Maintenance;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages {

    public class IndexModel : AssetNamePageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        [BindProperty] public Models.Owner Owner { get; set; }

        public IList<KeyHistory> KeyHistories { get; set; }
        public IList<Lot> Lots { get; set; }
        public string Balance { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            if (HttpContext.Session.GetInt32("SessionUserID") == null) {
                return RedirectToPage("User/Login");
            }

            Owner = await _context.Owner
                .Include(u => u.User)
                .Include(a => a.Address)
                .Include(oc => oc.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == HttpContext.Session.GetInt32("SessionOwnerID"));

            KeyHistories = await _context.KeyHistory
                .Include(s => s.Key)
                .Where(x => x.OwnerID == Owner.OwnerID)
                .ToListAsync().ConfigureAwait(false);

            Lots = await _context.Lots
                .Include(s => s.Address)
                .Include(l => l.LotInventory)
                .ThenInclude(i => i.Inventory)
                .Where(x => x.OwnerID == Owner.OwnerID && x.Status == "Occupied")
                .ToListAsync().ConfigureAwait(false);

            Balance =
                $"{_context.Transaction.Where(x => x.OwnerID == Owner.OwnerID && x.Status == "Open" && x.IsArchive != true).Select(x => x.Amount).Sum():C}";
            ViewData["State"] = States.GetStateSelectList();
            ViewData["CoOwner"] = _context.Owner.FirstOrDefault(x => x.OwnerID == Owner.CoOwnerID)?.FullName;

            PopulateAssetsDropDownList(_context);

            if (Owner == null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id, string primaryphone, string secondaryphone, string email,
            string fax, string coowner) {
            if (!ModelState.IsValid) {
                return Page();
            }

            var ownerToUpdate = await _context.Owner
                .Include(x => x.User)
                .Include(a => a.Address)
                .Include(oc => oc.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .FirstOrDefaultAsync(m => m.OwnerID == id);

            var owners = _context.Owner.Where(x => x.IsHoaOwner == true).Select(x => x.FullName).ToList();
            if (!owners.Contains(coowner) && !string.IsNullOrEmpty(coowner)) {
                CreateCoOwner(coowner);
            }

            ownerToUpdate.CoOwnerID = _context.Owner.FirstOrDefault(x => x.FullName == coowner)?.OwnerID;

            if (await TryUpdateModelAsync<Models.Owner>(
                ownerToUpdate,
                "owner",
                c => c.Address, c => c.OwnerContactType, c => c.Birthday, c => c.Occupation,
                c => c.EmergencyContactName, c => c.EmergencyContactPhone).ConfigureAwait(false)) {
                ownerToUpdate.LastModifiedBy = ownerToUpdate.Initials;
                ownerToUpdate.LastModifiedDate = DateTime.Now;

                ownerToUpdate.Address.LastModifiedBy = ownerToUpdate.Initials;
                ownerToUpdate.Address.LastModifiedDate = DateTime.Now;

                if (!String.IsNullOrWhiteSpace(primaryphone)) {
                    OwnerContactType toUpdate =
                        ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Primary Phone");
                    toUpdate.ContactValue = Extensions.CleanPhone(primaryphone);
                    toUpdate.LastModifiedBy = ownerToUpdate.Initials;
                    toUpdate.LastModifiedDate = DateTime.Now;
                }

                if (!String.IsNullOrWhiteSpace(secondaryphone)) {
                    OwnerContactType toUpdate =
                        ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Secondary Phone");
                    toUpdate.ContactValue = Extensions.CleanPhone(secondaryphone);
                    toUpdate.LastModifiedBy = ownerToUpdate.Initials;
                    toUpdate.LastModifiedDate = DateTime.Now;
                }

                if (!String.IsNullOrWhiteSpace(email)) {
                    OwnerContactType toUpdate =
                        ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Email");
                    toUpdate.ContactValue = email;
                    toUpdate.LastModifiedBy = ownerToUpdate.Initials;
                    toUpdate.LastModifiedDate = DateTime.Now;
                }

                if (!String.IsNullOrWhiteSpace(fax)) {
                    OwnerContactType toUpdate =
                        ownerToUpdate.OwnerContactType.FirstOrDefault(x => x.ContactType.Value == "Fax");
                    toUpdate.ContactValue = fax;
                    toUpdate.LastModifiedBy = ownerToUpdate.Initials;
                    toUpdate.LastModifiedDate = DateTime.Now;
                }

                _context.Attach(ownerToUpdate).State = EntityState.Modified;
                _context.Attach(ownerToUpdate.Address).State = EntityState.Modified;

                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Redirect("/Index");
            }

            return Page();
        }

        public IActionResult OnPostViolation(int? id, string violationBody, string violatorName, string violationLot) {
            Owner = _context.Owner.FirstOrDefault(x => x.OwnerID == id);
            var ownerName = Owner.FullName;
            var title = "HOA Rule Violation Report submitted by " + ownerName;

            violationLot = violationLot ?? "Not submitted";
            violatorName = violatorName ?? "Not submitted";

            var body = "Hello HOA Admin, \r\n" + ownerName +
                       " has noticed that a rule has been broken on HOA grounds. The details of the violation are listed below. \r\n\n"
                       + "Name of Violator: " + violatorName + "\r\nLot Number of Violation: " + violationLot +
                       "\r\nDescription of Violation: " + violationBody;

            // TOD0: Change to HOA email
            try {
                var client = new SmtpClient("smtp.mailtrap.io", 2525) {
                    Credentials = new NetworkCredential("32a991a4b394d1", "2186a7f103e535"),
                    EnableSsl = true
                };

                client.Send(Owner.Email, "test@test.com", title, body);
            }
            catch (Exception e) {
                TempData["MessageStatus"] = "Message failed to send";
                return RedirectToPage("./Index");
            }

            TempData["MessageStatus"] = "Message sent successfully";
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostMaintenance(int? id, string requestAsset, string requestBody) {
            requestAsset = _context.CommonAreaAsset
                .FirstOrDefault(x => x.CommonAreaAssetID == Int32.Parse(requestAsset)).AssetName;

            Owner = _context.Owner.FirstOrDefault(x => x.OwnerID == id);
            var ownerName = Owner.FullName;
            var title = "HOA Maintenance Request submitted by " + ownerName;

            var body = "Hello HOA Admin, \r\n" + ownerName +
                       " has noticed that a common area asset is in need of maintenance on the HOA grounds. The details of the request are listed below. \r\n\n"
                       + "Common Area Asset: " + requestAsset + "\r\nDescription of Maintenance Request: " +
                       requestBody;

            try {
                // TOD0: Change to HOA email
                var client = new SmtpClient("smtp.mailtrap.io", 2525) {
                    Credentials = new NetworkCredential("32a991a4b394d1", "2186a7f103e535"),
                    EnableSsl = true
                };

                client.Send(Owner.Email, "test@test.com", title, body);
            }
            catch (Exception e) {
                TempData["MessageStatus"] = "Message Failed to send";
                return RedirectToPage("./Index");
            }

            TempData["MessageStatus"] = "Message sent successfully";
            return RedirectToPage("./Index");
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