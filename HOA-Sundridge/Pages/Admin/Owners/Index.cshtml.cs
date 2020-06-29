using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;

namespace HOASunridge.Pages.Admin.Owners {

    public class IndexModel : PageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        public PaginatedList<Models.Owner> Owners { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex) {
            if (searchString != null) {
                pageIndex = 1;
            }
            else {
                searchString = currentFilter;
            }

            searchString = searchString?.ToLower();
            CurrentFilter = searchString;
            CurrentSort = sortOrder;

            IQueryable<Models.Owner> ownersIq = _context.Owner
                .Include(s => s.Address)
                .Include(o => o.OwnerContactType)
                .ThenInclude(c => c.ContactType)
                .Where(x => x.IsHoaOwner)
                .Where(x => x.IsPrimary)
                .Where(s => s.IsArchive == false);

            switch (sortOrder) {
                case "firstName_asc":
                    ownersIq = ownersIq.OrderBy(o => o.FirstName);
                    break;

                case "firstName_desc":
                    ownersIq = ownersIq.OrderByDescending(o => o.FirstName);
                    break;

                case "lastName_asc":
                    ownersIq = ownersIq.OrderBy(o => o.LastName);
                    break;

                case "lastName_desc":
                    ownersIq = ownersIq.OrderByDescending(o => o.LastName);
                    break;

                case "address_asc":
                    ownersIq = ownersIq.OrderBy(o => o.Address.State).ThenBy(o => o.Address.City).ThenBy(o => o.Address.StreetAddress);
                    break;

                case "address_desc":
                    ownersIq = ownersIq.OrderByDescending(o => o.Address.State).ThenByDescending(o => o.Address.City).ThenByDescending(o => o.Address.StreetAddress);
                    break;

                case "phone_asc":
                    ownersIq = ownersIq.OrderBy(o => o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Primary Phone").ContactValue);
                    break;

                case "phone_desc":
                    ownersIq = ownersIq.OrderByDescending(o => o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Primary Phone").ContactValue);
                    break;

                case "email_asc":
                    ownersIq = ownersIq.OrderBy(o => o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Email").ContactValue);
                    break;

                case "email_desc":
                    ownersIq = ownersIq.OrderByDescending(o => o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Email").ContactValue);
                    break;

                default:
                    ownersIq = ownersIq.OrderByDescending(o => o.LastName);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString)) {
                ownersIq = ownersIq.Where(o => o.FirstName.ToLower().Contains(searchString)
                                       || o.LastName.ToLower().Contains(searchString)
                                       || o.Address.FullAddress.ToLower().Contains(searchString)
                                       || o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Primary Phone").ContactValue.ToLower().Contains(searchString)
                                       || o.OwnerContactType.FirstOrDefault(c=>c.ContactType.Value == "Email").ContactValue.ToLower().Contains(searchString)
                );
            }

            int pageSize = 15;
            Owners = await PaginatedList<Models.Owner>.CreateAsync(
                ownersIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
