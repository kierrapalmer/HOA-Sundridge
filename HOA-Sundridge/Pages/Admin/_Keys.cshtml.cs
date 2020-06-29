using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages {

    public class KeysModel : PageModel {
        private readonly Models.HOAContext _context;

        public KeysModel(Models.HOAContext context) {
            _context = context;
        }

        public IList<Key> Keys { get; set; }

        public async Task<IActionResult> OnGetAsync() {
            Keys = await _context.Key
                .Include(s => s.KeyHistory)
                .ThenInclude(s => s.Owner)
                .ToListAsync().ConfigureAwait(false);

            return Page();
        }
    }
}