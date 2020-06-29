using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Pages.Classifieds {

    public class IndexModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public IndexModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        public IList<ClassifiedListing> AllClassifiedListing { get; set; }
        public IList<ClassifiedListing> OwnerClassifiedListing { get; set; }
        public Models.Owner SignedInOwner { get; set; }

        public async Task OnGetAsync() {
            SignedInOwner = _context.Owner.Include(y => y.User)
                .FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));

            AllClassifiedListing = await _context.ClassifiedListing
                .Include(c => c.Owner)
                .Include(f => f.Files)
                .ToListAsync().ConfigureAwait(false);

            OwnerClassifiedListing = await _context.ClassifiedListing
                .Include(c => c.Owner)
                .Include(f => f.Files)
                .Where(x => x.Owner.OwnerID == SignedInOwner.OwnerID)
                .ToListAsync().ConfigureAwait(false);
        }
    }
}