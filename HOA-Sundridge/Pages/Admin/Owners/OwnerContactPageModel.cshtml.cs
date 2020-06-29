using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HOASunridge.Models;
using HOASunridge.ViewModels;

using HOASunridge.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HOASunridge.Pages.Admin.Owners {
    public class OwnerContactPageModel : PageModel {
        public List<AssignedOwnerContactType> AssignedOwnerContacts;

        public void PopulateAssignedOwnerContacts(HOAContext context, Models.Owner owner) {
            var allContactTypes = context.ContactType;
            var allOwnerContacts = context.OwnerContactType.Where(o => o.OwnerID == owner.OwnerID);
            var ownerContacts = new HashSet<int>(owner.OwnerContactType.Select(o => o.ContactTypeID));
            AssignedOwnerContacts = new List<AssignedOwnerContactType>();

            foreach (var type in allContactTypes) {
                var contactInfo = "";
                foreach (var contact in allOwnerContacts) {
                    if (contact.ContactTypeID == type.ContactTypeID) {
                        contactInfo = contact.ContactValue;
                        break;
                    }
                }

                AssignedOwnerContacts.Add(new AssignedOwnerContactType {
                    ContactTypeID = type.ContactTypeID,
                    ContactTypeValue = type.Value,
                    ContactValue = contactInfo,
                    Assigned = ownerContacts.Contains(type.ContactTypeID)
                });
            }
        }

        public void UpdateOwnerContacts(HOAContext context, string[] selectedContacts, Models.Owner owner, Models.Owner user) {
            owner.OwnerContactType = new List<OwnerContactType>();
            List<string> contacts = new List<string>(selectedContacts);
            List<ContactType> contactTypes = new List<ContactType>(context.ContactType);

            foreach (var type in contactTypes) {
                if (!string.IsNullOrEmpty(contacts.ElementAt(contactTypes.IndexOf(type)))) {
                    owner.OwnerContactType.Add(new OwnerContactType {
                        ContactTypeID = context.ContactType.FirstOrDefault(c => c.Value == type.Value).ContactTypeID,
                        ContactValue = contacts.ElementAt(contactTypes.IndexOf(type)),
                        LastModifiedBy = user != null ? user.Initials : "SYS",
                        LastModifiedDate = DateTime.Now
                    });
                }
            }
        }
    }
}