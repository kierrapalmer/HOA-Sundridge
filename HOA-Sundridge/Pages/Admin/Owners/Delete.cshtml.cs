using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Admin.Owners {

    public class DeleteModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public DeleteModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        [BindProperty]
        public Models.Owner Owner { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Owner = await _context.Owner
                .Include(o => o.Address).FirstOrDefaultAsync(m => m.OwnerID == id).ConfigureAwait(false);

            if (Owner == null) {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) {
            if (id == null) {
                return NotFound();
            }

            Owner = await _context.Owner.FindAsync(id);

            var user = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));
            Owner.LastModifiedBy = user != null ? user.Initials : "SYS";
            Owner.LastModifiedDate = DateTime.Now;
            Owner.IsArchive = true;
            _context.Attach(Owner).State = EntityState.Modified;

            if (Owner.Address != null) {
                Owner.Address.LastModifiedBy = user != null ? user.Initials : "SYS";
                Owner.Address.LastModifiedDate = DateTime.Now;
                Owner.Address.IsArchive = true;
                _context.Attach(Owner.Address).State = EntityState.Modified;
            }

            if (Owner.OwnerContactType != null) {
                foreach (var contact in Owner.OwnerContactType) {
                    OwnerContactType contactToRemove = Owner
                        .OwnerContactType
                        .SingleOrDefault(c => c.ContactTypeID == contact.ContactTypeID);
                    _context.Remove(contactToRemove);
                }
            }

            if (Owner.CoOwner != null) {
                var coowner = _context.Owner.FirstOrDefault(x => x.OwnerID == Owner.CoOwnerID);
                coowner.LastModifiedBy = user != null ? user.Initials : "SYS";
                coowner.LastModifiedDate = DateTime.Now;
                coowner.IsArchive = true;
                _context.Attach(coowner).State = EntityState.Modified;
            }

            if (Owner.User != null) {
                Owner.User.LastModifiedBy = user != null ? user.Initials : "SYS";
                Owner.User.LastModifiedDate = DateTime.Now;
                Owner.User.IsArchive = true;
                _context.Attach(Owner.User).State = EntityState.Modified;
            }

            try {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException) {
                if (!OwnerExists(Owner.OwnerID)) {
                    return NotFound();
                }
                else {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool OwnerExists(int id) {
            return _context.Owner.Any(e => e.OwnerID == id);
        }

        private bool OwnerContactsExists(int id) {
            bool exist = false;
            foreach (var contact in _context.OwnerContactType) {
                if (contact.OwnerID == id) {
                    exist = true;
                }
            }

            return exist;
        }
    }
}