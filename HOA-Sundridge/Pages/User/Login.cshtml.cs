using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace HOASunridge.Pages.User {

    public class LoginModel : PageModel {
        public static string EmailMessage = "";
        public static string PasswordMessage = "";
        public bool validEmail = false;
        public static string tempUserFill = ""; //Used to hold the email so they don't have to retype it in on failed login attempt.

        private readonly HOAContext _context;

        public LoginModel(HOAContext context) {
            _context = context;
        }

        public IActionResult OnPost(string email, string password, string rememberMe) {
            IQueryable<Models.User> userIQ = from u in _context.User select u;

            tempUserFill = email; // This is so they don't have to re-type their username each time CL

            foreach (var user in userIQ) {
                if (user.UserName == email) {
                    validEmail = true;

                    if (!string.IsNullOrEmpty(password)) // If the password is blank/null it causes problems with CalculateSHA256 CL
                    {
                        if (user.UserPassword == CalculateSHA256(password)) {
                            PasswordMessage = "";

                            HttpContext.Session.SetString("SessionUser", user.UserName);
                            HttpContext.Session.SetString("SessionUserType", user.UserType);
                            HttpContext.Session.SetInt32("SessionUserID", user.UserID);
                            HttpContext.Session.SetInt32("SessionOwnerID", _context.Owner.FirstOrDefault(x => x.User.UserID == user.UserID).OwnerID);
                            HttpContext.Session.SetString("SessionOwnerName", _context.Owner.FirstOrDefault(x => x.User.UserID == user.UserID).FullName);

                            if (rememberMe != "remember") //If the remember me box is not checked, it will clear the email value. CL
                            {
                                tempUserFill = "";
                            }
                        }
                        else {
                            PasswordMessage = "Invalid Password";
                        }
                    }
                    else {
                        PasswordMessage = "Password field is blank";
                    }
                }
            }

            if (!validEmail) {
                EmailMessage = "No user account found with this email address.";
            }
            else {
                EmailMessage = "";
                var url = TempData["PageUrl"]?.ToString();          //gets last navigated from page if any
                if (url != null)
                    return Redirect(url);
                else
                    return RedirectToPage("/Index");
            }

            return Page();
        }

        private string CalculateSHA256(string str) {
            StringBuilder hashString = new StringBuilder();

            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));

            foreach (byte b in hashValue)
                hashString.Append(b.ToString("x2"));

            return hashString.ToString();
        }

        public IActionResult OnPostRequest(string requestMessage, string fullname, string requestEmail) {
            var title = "Account Access Request submitted by " + fullname;

            var body = "Hello HOA Admin, \r\n" + fullname + " needs help logging into their account. \r\n\n"
                       + "Name of owner: " + fullname + "\r\nEmail address of account: " + requestEmail + "\r\nAdditional information: " + requestMessage;

            // TOD0: Change to HOA email
            var client = new SmtpClient("smtp.mailtrap.io", 2525) {
                Credentials = new NetworkCredential("32a991a4b394d1", "2186a7f103e535"),
                EnableSsl = true
            };

            client.Send(requestEmail, "test@test.com", title, body);

            return RedirectToPage(Url.Content("~/User/Login"));
        }
    }
}