using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using HOASunridge.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Lots {

    public class LotInventoryPageModel : PageModel {
        public List<AssignedLotInventory> AssignedLotInventoryList;

        public void PopulateAssignedLotInventoryData(HOAContext context, Lot lot) {
            var allInventoryItems = context.Inventory;
            var LotInventory = new HashSet<int>(
                lot.LotInventory.Select(l => l.InventoryID));
            AssignedLotInventoryList = new List<AssignedLotInventory>();

            foreach (var item in allInventoryItems) {
                AssignedLotInventoryList.Add(new AssignedLotInventory {
                    InventoryID = item.InventoryID,
                    Description = item.Description,
                    Assigned = LotInventory.Contains(item.InventoryID)
                });
            }
        }

        public void UpdateLotInventory(HOAContext context, string[] selectedInventory, Lot lotToUpdate, int? whoID) {
            if (selectedInventory == null) {
                lotToUpdate.LotInventory = new List<LotInventory>();
                return;
            }

            var selectedInventoryHS = new HashSet<string>(selectedInventory);
            var lotInventory = new HashSet<int>(lotToUpdate.LotInventory.Select(l => l.Inventory.InventoryID));
            var whoEdited = context.Owner
                .Include(o => o.User)
                .Include(o => o.Address)
                .Include(a => a.OwnerContactType)
                .ThenInclude(oc => oc.ContactType)
                .FirstOrDefault(u => u.OwnerID == whoID);

            foreach (var item in context.Inventory) {
                if (selectedInventoryHS.Contains(item.InventoryID.ToString())) {
                    if (!lotInventory.Contains(item.InventoryID)) {
                        lotToUpdate.LotInventory.Add(
                            new LotInventory {
                                LotsID = lotToUpdate.LotID,
                                InventoryID = item.InventoryID,
                                Description = "",
                                LastModifiedBy = whoEdited != null ? whoEdited.Initials : "SYS",
                                LastModifiedDate = DateTime.Now
                            });
                    }
                }
                else {
                    if (lotInventory.Contains(item.InventoryID)) {
                        LotInventory itemToRemove = lotToUpdate
                            .LotInventory
                            .SingleOrDefault(l => l.InventoryID == item.InventoryID);
                        context.Remove(itemToRemove);
                    }
                }
            }
        }
    }
}