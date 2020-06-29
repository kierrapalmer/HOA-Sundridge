using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.User {
    public class SignOutModel : PageModel {
        public const string SessionUser = "";
        public const bool SessionIsAdmin = false;

        public void OnGet()
        {
        }
    }
}