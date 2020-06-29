using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_ContactType")]
    public class ContactType {
        public int ContactTypeID { get; set; }
        public string Value { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public ICollection<OwnerContactType> OwnerContactType { get; set; }
    }
}