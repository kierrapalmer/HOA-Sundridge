using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_CommonAreaAsset")]
    public class CommonAreaAsset {
        public int CommonAreaAssetID { get; set; }

        [Display(Name = "Asset Name")]
        [Required]
        [StringLength(60, MinimumLength = 3)]
        public string AssetName { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Purchase Price")]
        public Double PurchasePrice { get; set; }

        public string Description { get; set; }
        public string Status { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}", ApplyFormatInEditMode = false)]
        //        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Purchased")]
        public DateTime Date { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}