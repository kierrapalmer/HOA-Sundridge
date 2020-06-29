using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_Transaction")]
    public class Transaction {
        public int TransactionID { get; set; }
        public int OwnerID { get; set; }

        [Display(Name = "Transaction Type")]
        public int TransactionTypeID { get; set; }

        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public Double Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        [Display(Name = "Date Paid")]
        public DateTime? DatePaid { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string Status { get; set; }

        // links to other tables
        public Owner Owner { get; set; }

        public TransactionType TransactionType { get; set; }
    }
}