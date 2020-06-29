using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Transactions {

    public class TypeNamePageModel : PageModel {
        public SelectList TransactionTypeSL { get; set; }

        public void PopulateTransactionDropDownList(HOAContext _context, object selectedType = null) {
            var typeQuery = from d in _context.TransactionType
                            orderby d.Description
                            select d;

            TransactionTypeSL = new SelectList(typeQuery.AsNoTracking(),
                "TransactionTypeID", "Description", selectedType);
        }
    }
}