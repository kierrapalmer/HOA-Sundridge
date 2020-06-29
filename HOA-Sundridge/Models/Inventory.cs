using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_Inventory")]
    public class Inventory {
        public int InventoryID { get; set; }

        [Required]
        public string Description { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        [DefaultValue("SYS")]
        public string LastModifiedBy { get; set; }

        [DefaultValue(typeof(DateTime), "")]
        public DateTime LastModifiedDate { get; set; }

        public ICollection<LotInventory> LotInventory { get; set; }
    }
}