using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_Maintenance")]
    public class Maintenance {
        public int MaintenanceID { get; set; }

        public int CommonAreaAssetID { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.00}", ApplyFormatInEditMode = true)]
        public Double Cost { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        [Display(Name = "Date Completed")]
        public DateTime DateCompleted { get; set; }

        public string Description { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //linked tables
        public CommonAreaAsset CommonAreaAsset { get; set; }
    }
}