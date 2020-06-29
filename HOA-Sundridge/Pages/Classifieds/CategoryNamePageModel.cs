using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Classifieds {

    public class CategoryNamePageModel : PageModel {
        public SelectList CategorySelectList { get; set; }

        public void PopulateCategoryDropDownList(HOAContext _context, object selectedCategory = null) {
            var categoriesQuery = from d in _context.ClassifiedCategory
                                  orderby d.Description
                                  select d;

            CategorySelectList = new SelectList(categoriesQuery.AsNoTracking(),
                "ClassifiedCategoryID", "Description", selectedCategory);
        }
    }
}