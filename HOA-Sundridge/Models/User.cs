using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_User")]
    public class User {
        public int UserID { get; set; }
        public int? OwnerID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserType { get; set; }
        public string LastModifiedBy { get; set; }

        [DefaultValue("false")]
        public bool? IsArchive { get; set; }

        public DateTime LastModifiedDate { get; set; }

        public Owner Owner { get; set; }
    }
}