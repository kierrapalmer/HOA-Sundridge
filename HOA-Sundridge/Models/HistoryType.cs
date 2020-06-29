using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_HistoryTable")]
    public class HistoryType {
        public int HistoryTypeID { get; set; }
        public string Description { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}