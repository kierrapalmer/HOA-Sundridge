using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_KeyHistory")]
    public class KeyHistory {
        public int KeyHistoryID { get; set; }

        [Required]
        public int KeyID { get; set; }

        [Required]
        public int OwnerID { get; set; }

        public string Status { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Issued")]
        public DateTime DateIssued { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Display(Name = "Date Returned")]
        public DateTime? DateReturned { get; set; }

        [DefaultValue(0)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Paid Amount")]
        public double PaidAmount { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        //Links to other tables
        public Owner Owner { get; set; }

        public Key Key { get; set; }
    }
}