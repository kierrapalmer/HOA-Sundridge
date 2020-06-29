using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HOASunridge.Models;
using HOASunridge.Pages.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Lots {

    public class IndexModel : PageModel {
        private readonly HOAContext _context;

        public IndexModel(HOAContext context) {
            _context = context;
        }

        public PaginatedList<Lot> Lots { get; set; }

        //Sorts
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

            IQueryable<Lot> lotIq = _context.Lots
                .Include(l => l.Address)
                .Include(a => a.Owner)
                .Include(l => l.LotInventory)
                .ThenInclude(li => li.Inventory);

            switch (sortOrder) {
                case "taxId_desc":
                    lotIq = lotIq.OrderByDescending(l => l.LotID);
                    break;

                case "taxId_asc":
                    lotIq = lotIq.OrderBy(l => l.LotID);
                    break;

                case "lotNum_desc":
                    lotIq = lotIq.OrderByDescending(l => l.LotNumber);
                    break;

                case "lotNum_asc":
                    lotIq = lotIq.OrderBy(l => l.LotNumber);
                    break;

                case "address_desc":
                    lotIq = lotIq.OrderByDescending(l => l.Address.State).ThenBy(l => l.Address.City);
                    break;

                case "address_asc":
                    lotIq = lotIq.OrderBy(l => l.Address.State).ThenBy(l => l.Address.City);
                    break;

                case "status_desc":
                    lotIq = lotIq.OrderByDescending(l => l.Status);
                    break;

                case "status_asc":
                    lotIq = lotIq.OrderBy(l => l.Status);
                    break;

                case "owner_desc":
                    lotIq = lotIq.OrderByDescending(l => l.Owner.FullName);
                    break;

                case "owner_asc":
                    lotIq = lotIq.OrderBy(l => l.Owner.FullName);

                    break;

                case "inventory_desc":
                    lotIq = lotIq.OrderByDescending(l => l.LotInventory.FirstOrDefault().Inventory.Description);
                    break;

                case "inventory_asc":
                    lotIq = lotIq.OrderBy(l => l.LotInventory.FirstOrDefault().Inventory.Description);
                    break;

                default:
                    lotIq = lotIq.OrderBy(l => l.LotID);
                    break;
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                lotIq = lotIq.Where(l => l.LotID.ToString().ToLower().Contains(searchString)
                                      || l.LotNumber.ToString().ToLower().Contains(searchString)
                                      || l.Address.FullAddress.ToString().ToLower().Contains(searchString)
                                      || l.Status.ToLower().Contains(searchString)
                                      || l.InventoryItems.ToString().ToLower().Contains(searchString)
                                      || l.Owner.FullName.ToLower().Contains(searchString));
            }

            int pageSize = 15;
            Lots = await PaginatedList<Lot>.CreateAsync(
                lotIq.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}