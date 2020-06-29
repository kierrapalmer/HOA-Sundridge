using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_LotInventory")]
    public class LotInventory {
        public int LotInventoryID { get; set; }
        public int LotsID { get; set; }
        public int InventoryID { get; set; }
        public string Description { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        [DefaultValue("SYS")]
        public string LastModifiedBy { get; set; }

        [DefaultValue(typeof(DateTime), "")]
        public DateTime LastModifiedDate { get; set; }

        public Lot Lots { get; set; }
        public Inventory Inventory { get; set; }
    }
}