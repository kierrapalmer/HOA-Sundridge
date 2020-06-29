using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using HOASunridge.Models;
using System.Security.Cryptography;
using System.Text;
using HOASunridge.Pages.Shared;

namespace HOASunridge.Pages.User {

    public class ChangePasswordModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public static string oldPassMessage = "";
        public static string newPassMessage = "";
        public static string successPassMessage = "";

        public ChangePasswordModel(HOAContext context) {
            _context = context;
        }

        public IActionResult OnPost(string OldPassword, string NewPassword, string ReNewPassword) {
            oldPassMessage = "";
            newPassMessage = "";
            successPassMessage = "";

            var user = _context.User.Find(HttpContext.Session.GetInt32("SessionUserID"));

            if (NewPassword == ReNewPassword) // Passwords must match. This is to make sure that the password was typed correctly! CL
            {
                if (!string.IsNullOrEmpty(NewPassword)) // If the password is blank/null it causes problems with CalculateSHA256. We only check one cause they have to be the same in the if before. CL
                {
                    if (user.UserPassword == Extensions.CalculateSHA256(OldPassword)) // This compares the current password with the one stored in the database, a security step. CL
                    {
                        user.UserPassword = Extensions.CalculateSHA256(NewPassword); // This sets the current retreived users password to the new hashed password. CL
                        _context.SaveChanges(); // This saves the changes to the database CL

                        successPassMessage = "Password Updated Successfully";
                    }
                }
                else {
                    newPassMessage = "Password fields are blank";
                }
            }
            else {
                newPassMessage = "New passwords do not match!";
            }

            return Page();
        }
    }
}