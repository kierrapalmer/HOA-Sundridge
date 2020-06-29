using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages {

    public class BillingModel : PageModel {
        private readonly HOASunridge.Models.HOAContext _context;

        public BillingModel(HOASunridge.Models.HOAContext context) {
            _context = context;
        }

        public IList<Transaction> TransactionPast { get; set; }
        public IList<Transaction> TransactionOpen { get; set; }
        public string Balance { get; set; }

        public async Task OnGetAsync(int? id) {
            var owner = _context.Owner.FirstOrDefault(x => x.User.UserID == HttpContext.Session.GetInt32("SessionUserID"));

            TransactionOpen = await _context.Transaction
                .Include(t => t.Owner)
                .Include(t => t.TransactionType)
                .Where(t => t.OwnerID == owner.OwnerID && t.Status == "Open" && t.IsArchive != true)
                .ToListAsync().ConfigureAwait(false);

            TransactionPast = await _context.Transaction
                .Include(t => t.Owner)
                .Include(t => t.TransactionType)
                .Where(t => t.OwnerID == owner.OwnerID && t.Status != "Open" && t.IsArchive != true)
                .ToListAsync().ConfigureAwait(false);

            Balance = $"{TransactionOpen.Select(x => x.Amount).Sum():C}";
        }
    }
}