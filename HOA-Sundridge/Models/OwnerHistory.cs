using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_OwnerHistory")]
    public class OwnerHistory {

        [Key]
        public int OwnerHistoryID { get; set; }

        public int? LotID { get; set; }
        public int OwnerID { get; set; }

        [Required]
        [Display(Name = "History Type")]
        public int HistoryTypeID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        public DateTime Date { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Privacy Level")]
        public string PrivacyLevel { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Owner Owner { get; set; }

        public Lot Lot { get; set; }
        public HistoryType HistoryType { get; set; }
    }
}