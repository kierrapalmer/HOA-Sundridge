using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.ViewModels {

    public class AssignedLotInventory {
        public int InventoryID { get; set; }
        public string Description { get; set; }
        public bool Assigned { get; set; }
    }
}