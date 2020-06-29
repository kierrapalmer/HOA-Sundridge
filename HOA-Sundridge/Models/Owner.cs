using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_Owner")]
    public class Owner {
        public int OwnerID { get; set; }
        public int? AddressID { get; set; }

        public int? CoOwnerID { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsHoaOwner { get; set; }

        [Required]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Occupation { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yy}")]
        public DateTime? Birthday { get; set; }

        [DisplayName("Name")]
        public string EmergencyContactName { get; set; }

        [DisplayName("Phone")]
        [DisplayFormat(DataFormatString = "{0:###-###-####}", ApplyFormatInEditMode = true)]
        public string EmergencyContactPhone { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        //Links to other tables
        public Address Address { get; set; }

        public User User { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<OwnerContactType> OwnerContactType { get; set; }
        public ICollection<Owner> CoOwner { get; set; }
        public ICollection<Key> KeyHistory { get; set; }
        public ICollection<Lot> Lot { get; set; }
        public ICollection<ClassifiedListing> ClassifiedListing { get; set; }
        public ICollection<OwnerHistory> OwnerHistory { get; set; }

        //For display purposes
        [Display(Name = "Owner Name")]
        public string FullName {
            get { return FirstName + " " + LastName; }
        }

        public string Email {
            get {
                if (OwnerContactType != null) {
                    foreach (var contact in OwnerContactType) {
                        if (contact.ContactType.Value == "Email") {
                            return contact.ContactValue;
                        }
                    }
                }

                return FirstName + LastName + "@email.com";
            }
        }

        public string Phone {
            get {
                if (OwnerContactType != null) {
                    var secondary = "";
                    foreach (var contact in OwnerContactType) {
                        if (contact.ContactType.Value == "Primary Phone") {
                            return contact.ContactValue;
                        }

                        if (contact.ContactType.Value == "Secondary Phone") {
                            secondary = contact.ContactValue;
                        }
                    }
                }

                return "";
            }
        }

        public string Initials {
            get {
                var initials = "";
                initials += FirstName != null ? FirstName.Substring(0, 1) : "";
                initials += LastName != null ? LastName.Substring(0, 1) : "";

                return initials;
            }
        }
    }
}