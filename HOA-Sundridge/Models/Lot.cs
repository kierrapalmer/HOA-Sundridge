using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;

namespace HOASunridge.Models {

    [Table("HOA_Lot")]
    public class Lot {

        [Required]
        [Display(Name = "Tax ID")]
        public int LotID { get; set; }

        public int? OwnerID { get; set; }
        public int AddressID { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Lot #")]
        public string LotNumber { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        [DefaultValue("SYS")]
        public string LastModifiedBy { get; set; }

        [DefaultValue(typeof(DateTime), "")]
        public DateTime LastModifiedDate { get; set; }

        public Owner Owner { get; set; }
        public Address Address { get; set; }
        public ICollection<LotInventory> LotInventory { get; set; }

        //Helps with searches and filters
        public string InventoryItems {
            get {
                if (LotInventory != null)
                {

                    return string.Join(", ", LotInventory.Select(i => i.Inventory.Description));
                }
                else
                {
                    return "";
                }
            }
        }
    }
}