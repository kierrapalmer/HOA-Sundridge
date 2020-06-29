using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_OwnerContactType")]
    public class OwnerContactType {
        public int OwnerContactTypeID { get; set; }
        public int OwnerID { get; set; }
        public int ContactTypeID { get; set; }
        public string ContactValue { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Owner Owner { get; set; }
        public ContactType ContactType { get; set; }
    }
}