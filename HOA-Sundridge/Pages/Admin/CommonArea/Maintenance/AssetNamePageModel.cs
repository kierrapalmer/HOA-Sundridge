using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Maintenance {

    public class AssetNamePageModel : PageModel {
        public SelectList CommonAreaAssetSL { get; set; }

        public void PopulateAssetsDropDownList(HOAContext _context, object selectedAsset = null) {
            var departmentsQuery = from d in _context.CommonAreaAsset
                                   where d.Status == "Active"
                                   orderby d.AssetName // Sort by name.
                                   select d;

            CommonAreaAssetSL = new SelectList(departmentsQuery.AsNoTracking(),
                "CommonAreaAssetID", "AssetName", selectedAsset);
        }
    }
}