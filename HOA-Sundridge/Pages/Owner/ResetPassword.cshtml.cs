using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Owner {

    public class ResetPasswordModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public ResetPasswordModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        public IActionResult OnGet(int? id) {
            if (id == null) {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id) //This is called if someone presses the password reset button. CL (Only viewable when logged in as an Admin
        {
            if (!ModelState.IsValid) {
                return Page();
            }

            var ownerId = _context.User.FirstOrDefault(x => x.UserID == id).OwnerID;
            var phone = _context.OwnerContactType
                .FirstOrDefault(c => c.Owner.User.UserID == id && c.ContactType.Value == "Primary Phone");

            var userToUpdate = await _context.User.FirstOrDefaultAsync(x => x.UserID == id);

            userToUpdate.UserPassword = Extensions.CalculateSHA256(Extensions.CleanPhone(phone.ContactValue)); // This is the line that sets the users password to their hashed primary phone number after it's cleaned of non numeric characters

            await _context.SaveChangesAsync();
            return Redirect("/Owner/Profile?id=" + ownerId);
        }
    }
}