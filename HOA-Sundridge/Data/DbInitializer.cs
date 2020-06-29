using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using HOASunridge.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace HOASunridge.Data {
    /*public class DbInitializer {
        public static void Initialize(HOAContext context) {
            //context.Database.EnsureCreated();

            // Look for any owners.
            if (context.Owner.Any()) {
                return;   // DB has been seeded
            }

            //Address
            var address = new Address[]{
                new Address{ StreetAddress = "4550 Scelerisque Av.",    City = "Tenby",             State = "NV", Zip = "4670",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "9225 Ipsum. Rd.",         City = "Corswarem",         State = "CA", Zip = "7281 NI",   LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "5108 Cursus Av.",         City = "Waren",             State = "UT", Zip = "41809",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "174 Cursus Rd.",          City = "Chiari",            State = "NV", Zip = "7315",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "4796 Eu, Av.",            City = "Dubuisson",         State = "NV", Zip = "58389",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "436 Parturient Rd.",      City = "Thurso",            State = "UT", Zip = "9829",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "3645 Penatibus St.",      City = "Verres",            State = "UT", Zip = "4833",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "7543 Eget Av.",           City = "Ogden",             State = "ID", Zip = "94964",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1053 Nascetur St.",       City = "High Wycombe",      State = "CA", Zip = "IZ90 1RY",  LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "4429 Eleifend St.",       City = "Cumberland County", State = "CA", Zip = "9260 EB",   LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "370 Sit Rd.",             City = "Boise",             State = "ID", Zip = "9207",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "2099 Suspendisse Street", City = "Ogden",             State = "UT", Zip = "9405",      LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "7666 Adipiscing, Rd.",    City = "Masterton",         State = "UT", Zip = "92674",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "4868 Proin Avenue",       City = "Cumberland County", State = "NV", Zip = "396406",    LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "464-3300 At St.",         City = "Tonk",              State = "CA", Zip = "053970",    LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "657-1716 Lacinia Rd.",    City = "Dorval",            State = "CA", Zip = "44434",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "5726 Ornare Rd.",         City = "Oklahoma City",     State = "CA", Zip = "89316",     LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1115 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1117 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1118 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1119 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1120 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1122 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1124 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1125 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1129 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                new Address{ StreetAddress = "1126 Smith St.",          City = "Ogden",             State = "UT", Zip = "84405-828", LastModifiedDate = DateTime.Parse("11/09/18"), LastModifiedBy = "DIS" },
                };

            foreach (Address a in address) {
                context.Address.Add(a);
            }
            context.SaveChanges();

            //Owners
            var owners = new Owner[]{
                new Owner {IsHoaOwner = true,  FirstName = "Quynn",    LastName = "Mann",       EmergencyContactName = "Isla Mann",      EmergencyContactPhone= "2223435443", Occupation = "Fire Fighter",  Birthday = DateTime.Parse("04-24-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "9225 Ipsum. Rd.").AddressID,      IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18") },
                new Owner {IsHoaOwner = true,  FirstName = "Casey",    LastName = "Buchanan",   EmergencyContactName = "Candy Buchanan", EmergencyContactPhone= "2223665742", Occupation = "Taxi Driver",   Birthday = DateTime.Parse("02-12-1978"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "5108 Cursus Av.").AddressID,      IsPrimary = true,  LastModifiedBy = "RLB", LastModifiedDate = DateTime.Now },
                new Owner {IsHoaOwner = true,  FirstName = "Tamekah",  LastName = "Patel",      EmergencyContactName = "Lily Patel",     EmergencyContactPhone= "9695554325", Occupation = "Accountant",    Birthday = DateTime.Parse("10-09-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "436 Parturient Rd.").AddressID,   IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Candice",  LastName = "Greer",      EmergencyContactName = "Grace Oscar",    EmergencyContactPhone= "1253457365", Occupation = "Receptionist",  Birthday = DateTime.Parse("08-27-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "4868 Proin Avenue").AddressID,    IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Marshall", LastName = "Rice",       EmergencyContactName = "Olivia Jack",    EmergencyContactPhone= "9043035454", Occupation = "Baker",         Birthday = DateTime.Parse("05-08-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "464-3300 At St.").AddressID,      IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Sara",     LastName = "Camacho",                                                                                  Occupation = "Florist",       Birthday = DateTime.Parse("07-24-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "7543 Eget Av.").AddressID,        IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Kuame",    LastName = "Collier",                                                                                  Occupation = " Student ",     Birthday = DateTime.Parse("06-28-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "657-1716 Lacinia Rd.").AddressID, IsPrimary = false, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Abbot",    LastName = "Adams",                                                                                    Occupation = "Accountant ",   Birthday = DateTime.Parse("07-01-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "370 Sit Rd.").AddressID,          IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Reagan",   LastName = "Morrow",                                                                                   Occupation = " Hairdresser ", Birthday = DateTime.Parse("08-19-2018" ), AddressID = address.SingleOrDefault( i => i.StreetAddress == "174 Cursus Rd.").AddressID,       IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Bree",     LastName = "Lawrence",                                                                                 Occupation = "Student",       Birthday = DateTime.Parse("04-30-2018"),  AddressID = address.SingleOrDefault( i => i.StreetAddress == "1117 Smith St.").AddressID,       IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = true,  FirstName = "Darius",   LastName = "Rogers",                                                                                   Occupation = " Doctor ",      Birthday = DateTime.Parse("09-11-2018" ), AddressID = address.SingleOrDefault( i => i.StreetAddress == "4550 Scelerisque Av.").AddressID, IsPrimary = true,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = false, FirstName = "Fire",     LastName = "Department",                                                                                                                                                                                                                                                       IsPrimary = false, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = false, FirstName = "Samuel",   LastName = "Singer",                                                                                                                                                                                                                                                           IsPrimary = false, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
                new Owner {IsHoaOwner = false, FirstName = "Robert",   LastName = "Winchy",                                                                                                                                                                                                                                                           IsPrimary = false, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11-09-18")},
             };

            foreach (Owner o in owners) {
                context.Owner.Add(o);
            }
            context.SaveChanges();

            //Lots
            var lots = new Lot[]{
                new Lot { LotNumber = "11", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Bree"     && i.LastName == "Lawrence").OwnerID, AddressID = address.SingleOrDefault( i => i.StreetAddress == "1117 Smith St.").AddressID,          LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "12", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID, AddressID = address.SingleOrDefault( i => i.StreetAddress == "5108 Cursus Av.").AddressID,         LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "22", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Tamekah"  && i.LastName == "Patel").OwnerID,    AddressID = address.SingleOrDefault( i => i.StreetAddress == "4550 Scelerisque Av.").AddressID,    LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "14", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     AddressID = address.SingleOrDefault( i => i.StreetAddress == "1118 Smith St.").AddressID,         LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "15", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     AddressID = address.SingleOrDefault( i => i.StreetAddress == "1119 Smith St.").AddressID,         LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "33", Status = "For Sale", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     AddressID = address.SingleOrDefault( i => i.StreetAddress == "1120 Smith St.").AddressID,         LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "54", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,   AddressID = address.SingleOrDefault( i => i.StreetAddress == "4550 Scelerisque Av.").AddressID,    LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "66", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Kuame"    && i.LastName == "Collier").OwnerID,  AddressID = address.SingleOrDefault( i => i.StreetAddress == "2099 Suspendisse Street").AddressID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "77", Status = "Vacant",   OwnerID = owners.SingleOrDefault( i => i.FirstName == "Reagan"   && i.LastName == "Morrow").OwnerID,   AddressID = address.SingleOrDefault( i => i.StreetAddress == "174 Cursus Rd.").AddressID,          LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Lot { LotNumber = "88", Status = "Occupied", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Marshall" && i.LastName == "Rice").OwnerID,     AddressID = address.SingleOrDefault( i => i.StreetAddress == "370 Sit Rd.").AddressID,             LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
            };

            foreach (Lot l in lots) {
                context.Lots.Add(l);
            }
            context.SaveChanges();

            //History Type
            var historyTypes = new HistoryType[]
            {
                new HistoryType { Description = "Infraction", LastModifiedBy = "RLB", LastModifiedDate = DateTime.Parse("11-13-18") },
                new HistoryType { Description = "WIK",        LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new HistoryType { Description = "Exception",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
            };

            foreach (HistoryType h in historyTypes) {
                context.HistoryType.Add(h);
            }
            context.SaveChanges();

            //Owner History
            //Note: for some reason this table doesn't like lot 22
            var ownerHistories = new OwnerHistory[]{
                new OwnerHistory { Date = DateTime.Parse("12/23/13"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "Infraction").HistoryTypeID, PrivacyLevel = "Public",   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault( i => i.FirstName == "Quynn"   && i.LastName == "Mann").OwnerID,     LotID = lots.SingleOrDefault(i => i.LotNumber == "11").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("12/31/12"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "WIK").HistoryTypeID,        PrivacyLevel = "Public",   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID, LotID = lots.SingleOrDefault(i => i.LotNumber == "12").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("02/15/10"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "Infraction").HistoryTypeID, PrivacyLevel = "Internal", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Tamekah"  && i.LastName == "Patel").OwnerID,    LotID = lots.SingleOrDefault(i => i.LotNumber == "14").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("03/25/17"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "Exception").HistoryTypeID,  PrivacyLevel = "Public",   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID ,    LotID = lots.SingleOrDefault(i => i.LotNumber == "88").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("05/16/08"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "Infraction").HistoryTypeID, PrivacyLevel = "Public",   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Marshall" && i.LastName == "Rice").OwnerID,     LotID = lots.SingleOrDefault(i => i.LotNumber == "33").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("01/10/08"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "Infraction").HistoryTypeID, PrivacyLevel = "Internal", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Reagan"   && i.LastName == "Morrow").OwnerID,   LotID = lots.SingleOrDefault(i => i.LotNumber == "54").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("05/15/08"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "WIK").HistoryTypeID,        PrivacyLevel = "Internal", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Bree"     && i.LastName == "Lawrence").OwnerID, LotID = lots.SingleOrDefault(i => i.LotNumber == "66").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerHistory { Date = DateTime.Parse("06/02/09"), HistoryTypeID = historyTypes.SingleOrDefault(i=>i.Description == "WIK").HistoryTypeID,        PrivacyLevel = "Internal", Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris", OwnerID = owners.SingleOrDefault(i => i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,   LotID = lots.SingleOrDefault(i => i.LotNumber == "77").LotID, LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
            };

            foreach (OwnerHistory o in ownerHistories) {
                context.OwnerHistory.Add(o);
            }
            context.SaveChanges();

            var classifiedCategories = new ClassifiedCategory[]
            {
                new ClassifiedCategory{ Description = "Lots"},
                new ClassifiedCategory{ Description = "Cabins"},
                new ClassifiedCategory{ Description = "Other Services"}
            };

            foreach (ClassifiedCategory c in classifiedCategories) {
                context.ClassifiedCategory.Add(c);
            }
            context.SaveChanges();

            //Classified Listing
            var classifiedListings = new ClassifiedListing[]
            {
                new ClassifiedListing { OwnerID = owners.SingleOrDefault( i => i.FirstName == "Quynn"  && i.LastName == "Mann").OwnerID,     ItemName = "Lot #33",   ClassifiedCategoryID = classifiedCategories.SingleOrDefault( i => i.Description == "Lots").ClassifiedCategoryID,           Price = 19.35, Email = "quynn@mail.com", Phone= "333-234-3333", Description = "Are you looking for a quiet, secluded place to get away to? This gated property in Sunridge is close enough to home you can get away for the " +
                                                                                                                                                                                                                                                                                                                                                                             "weekend but far enough away to escape and relax! This cabin is 2433 sqft and has all the hard work done for you. No maintenance exterior is complete " +
                                                                                                                                                                                                                                                                                                                                                                             "with cinderblock walls, lifetime aluminum lock shingles, and aluminum siding. Septic system, culinary water, driveway, and RV parking are all in place. "+
                                                                                                                                                                                                                                                                                                                                                                             "The interior is ready to be finished and customized to your liking. 1.82 acre",                                                                                            ListingDate = DateTime.Parse("12/24/18"),     LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ClassifiedListing { OwnerID = owners.SingleOrDefault( i => i.FirstName == "Casey"  && i.LastName == "Buchanan").OwnerID, ItemName = "Cabin #14", ClassifiedCategoryID= classifiedCategories.SingleOrDefault( i => i.Description == "Cabins").ClassifiedCategoryID,         Price = 19.35, Email = "casey@mail.com", Phone= "333-234-3333", Description = "1.066 hundred square ft. 2.3 acres. 2 bedroom (upstairs very large) big closet. Sleeps many people.Bedroom downstairs (master). Full bath. Nice size kitchen, " +
                                                                                                                                                                                                                                                                                                                                                                             "lots of cabinet space. 15 cu. ft. propane refrigerator/freezer, excellent condition. Upgraded windows. 4000 watt electric start generator. Very little upkeep on " +
                                                                                                                                                                                                                                                                                                                                                                             "outside. Aluminum siding, front deck is Trex. Back deck and balcony are wood.Shed on property. Wood heat. Cabin is secluded and is bordered on two sides by common land.", ListingDate = DateTime.Parse("01/11/2018"),   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ClassifiedListing { OwnerID = owners.SingleOrDefault( i => i.FirstName == "Casey"  && i.LastName == "Buchanan").OwnerID, ItemName = "Lot #21",   ClassifiedCategoryID= classifiedCategories.SingleOrDefault( i => i.Description == "Lots").ClassifiedCategoryID,           Price = 33.98, Email = "casey@mail.com",                        Description = "Sed eget lacus. Mauris non dui nec. Vivamus non lorem vitae odio sagittis semper. Nam",                                                                                    ListingDate = DateTime.Parse("Dec 16, 2017"), LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ClassifiedListing { OwnerID = owners.SingleOrDefault( i => i.FirstName == "Darius" && i.LastName == "Rogers").OwnerID,   ItemName = "Lawn Care", ClassifiedCategoryID= classifiedCategories.SingleOrDefault( i => i.Description == "Other Services").ClassifiedCategoryID, Price = 75.86,                           Phone= "887-234-2343", Description = "ded eget lacus. Vivamus non lorem vitae odio sagittis semper. Nam",                                                                                                        ListingDate = DateTime.Parse("02/26/18"),     LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
            };

            foreach (ClassifiedListing c in classifiedListings) {
                context.ClassifiedListing.Add(c);
            }
            context.SaveChanges();

            //Common Area Asset
            var commonAreaAssets = new CommonAreaAsset[]
            {
                new CommonAreaAsset { AssetName = "Picnic Table #1", Description = "in felis. Nulla tempor augue ac ipsum. Phasellus",                            Status = "Active",     PurchasePrice=140.20,  Date = DateTime.Parse("12/20/17") },
                new CommonAreaAsset { AssetName = "Trees",           Description = "sem ut dolor Nulla tempor augue ac ipsum.",                                   Status = "Active",     PurchasePrice=420.00,  Date = DateTime.Parse("09/09/19") },
                new CommonAreaAsset { AssetName = "Lawn",            Description = "vitae aliquam eros turpis non enim. Mauris quis",                             Status = "Disabled",   PurchasePrice=77.80,  Date = DateTime.Parse("10/10/19") },
                new CommonAreaAsset { AssetName = "Fence1",          Description = "odio. Aliquam vulputate ullamcorper magna. Sed eu eros. Nam consequat",       Status = "Active",     PurchasePrice=44.20,  Date = DateTime.Parse("04/03/19") },
                new CommonAreaAsset { AssetName = "Well #1",         Description = "non enim commodo hendrerit. Donec porttitor tellus non magna. Nam",           Status = "Active",     PurchasePrice=420.00,  Date = DateTime.Parse("08/14/18") },
                new CommonAreaAsset { AssetName = "Picnic Table #2", Description = "Phasellus vitae mauris sit amet lorem semper",                                Status = "Active",     PurchasePrice=85.00,  Date = DateTime.Parse("10/10/18") },
                new CommonAreaAsset { AssetName = "Well #2",         Description = "gravida nunc sed pede. Cum sociis natoque penatibus et magnis dis",           Status = "Disabled",   PurchasePrice=47.66,  Date = DateTime.Parse("09/20/19") },
                new CommonAreaAsset { AssetName = "Swing",           Description = "Donec nibh enim, gravida sit, Nulla tempor augue ac ipsum.",                  Status = "Active",     PurchasePrice=95.55,  Date = DateTime.Parse("01/28/18") },
                new CommonAreaAsset { AssetName = "Well #3",         Description = "euismod est arcu ac orci. Ut semper pretium. Nulla tempor augue ac ipsum.",   Status = "Active",     PurchasePrice=20.00,  Date = DateTime.Parse("06/24/18") }
           };

            foreach (CommonAreaAsset c in commonAreaAssets) {
                context.CommonAreaAsset.Add(c);
            }
            context.SaveChanges();

            //Contact Type
            var contactTypes = new ContactType[]
            {
                new ContactType { Value = "Primary Phone",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ContactType { Value = "Secondary Phone", LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ContactType { Value = "Email",           LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new ContactType { Value = "Fax",             LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                };

            foreach (ContactType c in contactTypes) {
                context.ContactType.Add(c);
            }
            context.SaveChanges();

            //File
            var files = new File[]
            {
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 1).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.jpeg/09f/fff",                  Type = "jpeg",  Description = "consectetuer mauris id sapien. Cras dolor dolor,",           Date = DateTime.Parse("12/03/17"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 2).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.png/09f/fff",                   Type = "png",   Description = "egestas blandit. Nam nulla",                                 Date = DateTime.Parse("04/22/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 3).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 1).ClassifiedListingID, FileURL = "http://unec.edu.az/application/uploads/2014/12/pdf-sample.pdf", Type = "pdf",   Description = "mollis dui, in sodales elit erat vitae risus.",              Date = DateTime.Parse("08/24/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 4).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.png/09f/fff",                   Type = "png",   Description = "ultricies ligula. Nullam enim. Sed nulla ante, iaculis",     Date = DateTime.Parse("06/13/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 3).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.png/09f/fff",                   Type = "png",   Description = "Integer eu",                                                 Date = DateTime.Parse("01/22/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 4).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.jpeg/09f/fff",                  Type = "jpeg",  Description = "amet risus. Donec egestas. Aliquam nec enim. Nunc ut erat.", Date = DateTime.Parse("03/14/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 7).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 1).ClassifiedListingID, FileURL = "http://unec.edu.az/application/uploads/2014/12/pdf-sample.pdf", Type = "pdf",   Description = "lacinia. Sed congue, elit",                                  Date = DateTime.Parse("02/14/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 8).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 1).ClassifiedListingID, FileURL = "http://unec.edu.az/application/uploads/2014/12/pdf-sample.pdf", Type = "pdf",   Description = "vel lectus. Cum sociis",                                     Date = DateTime.Parse("07/26/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 2).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.jpeg/09f/fff",                  Type = "jpeg",  Description = "sed leo. Cras vehicula aliquet libero.",                     Date = DateTime.Parse("06/20/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 3).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.png/09f/fff",                   Type = "png",   Description = "bibendum sed, est. Nunc laoreet",                            Date = DateTime.Parse("03/13/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 4).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.jpeg/09f/fff",                  Type = "jpeg",  Description = "dictum ultricies ligula.",                                   Date = DateTime.Parse("08/08/18"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new File { OwnerHistoryID = ownerHistories.SingleOrDefault(i=>i.OwnerHistoryID == 1).OwnerHistoryID, ClassifiedListingID = classifiedListings.SingleOrDefault(i=>i.ClassifiedListingID == 3).ClassifiedListingID, FileURL = "https://via.placeholder.com/300.png/09f/fff",                   Type = "png",   Description = "quam, elementum at, egestas a,",                             Date = DateTime.Parse("11/19/17"),  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") }
            };

            foreach (File f in files) {
                context.File.Add(f);
            }
            context.SaveChanges();

            //Inventory
            var inventories = new Inventory[]
            {
                new Inventory { Description = "Septic",        LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Inventory { Description = "Shed",          LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Inventory { Description = "Propane Tank",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Inventory { Description = "Water Hookup",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
              };

            foreach (Inventory i in inventories) {
                context.Inventory.Add(i);
            }
            context.SaveChanges();

            //Key
            var keys = new Key[]
            {
                new Key { SerialNumber = "Z1000" , LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z999" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z998" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z997" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z996" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z995" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z994" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z993" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z992" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z991" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z990" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z989" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z988" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z987" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z986" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z985" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z984" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z983" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z982" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z981" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z980" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z979" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z978" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z977" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z976" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Key { SerialNumber = "Z975" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")}
            };

            foreach (Key k in keys) {
                context.Key.Add(k);
            }
            context.SaveChanges();

            //Key History
            var keyHistories = new KeyHistory[]
            {
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z1000").KeyID, OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     Status = "Active",     DateIssued = DateTime.Parse("01/21/15"), DateReturned =  null,                    PaidAmount = 81.91,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z999").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID, Status = "Active",   DateIssued = DateTime.Parse("01/11/12"), DateReturned =  null,                      PaidAmount = 78.26,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z998").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Tamekah"  && i.LastName == "Patel").OwnerID,    Status = "Active",   DateIssued = DateTime.Parse("12/04/10"), DateReturned =  null,                      PaidAmount = 54.21,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z997").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Candice"  && i.LastName == "Greer").OwnerID,    Status = "Returned", DateIssued = DateTime.Parse("09/24/11"), DateReturned = DateTime.Parse("07/19/18"), PaidAmount = 87.13,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z996").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Marshall" && i.LastName == "Rice").OwnerID,     Status = "Active",   DateIssued = DateTime.Parse("05/27/14"), DateReturned = null,                       PaidAmount = 27.50,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z995").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Sara"     && i.LastName == "Camacho").OwnerID,  Status = "Returned", DateIssued = DateTime.Parse("01/10/12"), DateReturned = DateTime.Parse("06/21/18"), PaidAmount = 70.95,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z994").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Kuame"    && i.LastName == "Collier").OwnerID,  Status = "Returned", DateIssued = DateTime.Parse("11/01/16"), DateReturned = DateTime.Parse("01/29/18"), PaidAmount = 14.73,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z993").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Abbot"    && i.LastName == "Adams").OwnerID,    Status = "Active",   DateIssued = DateTime.Parse("09/19/15"), DateReturned = null,                       PaidAmount = 70.64,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z992").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Reagan"   && i.LastName == "Morrow").OwnerID,   Status = "Lost",     DateIssued = DateTime.Parse("08/13/11"), DateReturned = null,                       PaidAmount = 73.30,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z991").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Bree"     && i.LastName == "Lawrence").OwnerID, Status = "Returned", DateIssued = DateTime.Parse("11/10/11"), DateReturned = DateTime.Parse("03/27/18"), PaidAmount = 54.86,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z990").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,   Status = "Active",   DateIssued = DateTime.Parse("04/23/17"), DateReturned = null,                       PaidAmount = 72.46,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z989").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,   Status = "Returned", DateIssued = DateTime.Parse("02/19/15"), DateReturned = DateTime.Parse("01/11/18"), PaidAmount = 83.35,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z988").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Abbot"    && i.LastName == "Adams").OwnerID,    Status = "Active",   DateIssued = DateTime.Parse("10/27/16"), DateReturned = null,                       PaidAmount = 56.42,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z988").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Abbot"    && i.LastName == "Adams").OwnerID,    Status = "Active",   DateIssued = DateTime.Parse("12/01/12"), DateReturned = null,                       PaidAmount = 16.09,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z987").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Sara"     && i.LastName == "Camacho").OwnerID,  Status = "Lost",     DateIssued = DateTime.Parse("06/27/11"), DateReturned = null,                       PaidAmount = 1.88 ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new KeyHistory { KeyID = keys.SingleOrDefault(i=>i.SerialNumber == "Z986").KeyID,  OwnerID = owners.SingleOrDefault(i=>i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     Status = "Lost",     DateIssued = DateTime.Parse("01/21/15"), DateReturned =  null,                    PaidAmount = 81.91,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")}
            };

            try {
                foreach (KeyHistory k in keyHistories) {
                    context.KeyHistory.Add(k);
                }

                context.SaveChanges();
            }
            catch (InvalidCastException ex) {
                //MsgBox("Can't load web page" & vbCrLf & ex.message);
                Console.WriteLine("Exception: " + ex);
            }

            //Lot Inventorys
            var lotInventories = new LotInventory[]
            {
                new LotInventory { IsArchive= false, Description = "pede, malesuada vel, venenatis vel, faucibus id, libero. Donec",             LotsID = lots.SingleOrDefault(i=>i.LotNumber == "11").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 1).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "turpis. In condimentum. Donec",                                              LotsID = lots.SingleOrDefault(i=>i.LotNumber == "12").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 1).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "egestas blandit. Nam nulla magna, malesuada",                                LotsID = lots.SingleOrDefault(i=>i.LotNumber == "14").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 2).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "tempus",                                                                     LotsID = lots.SingleOrDefault(i=>i.LotNumber == "77").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 2).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "Nullam scelerisque neque sed sem",                                           LotsID = lots.SingleOrDefault(i=>i.LotNumber == "11").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 2).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "semper egestas, urna justo",                                                 LotsID = lots.SingleOrDefault(i=>i.LotNumber == "15").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 3).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "nisi. Mauris nulla. Integer urna. Vivamus molestie dapibus ligula. Aliquam", LotsID = lots.SingleOrDefault(i=>i.LotNumber == "11").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 3).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "at, egestas a, scelerisque sed, sapien. Nunc",                               LotsID = lots.SingleOrDefault(i=>i.LotNumber == "14").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 4).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new LotInventory { IsArchive= false, Description = "eleifend vitae, erat. Vivamus",                                              LotsID = lots.SingleOrDefault(i=>i.LotNumber == "15").LotID, InventoryID = inventories.SingleOrDefault(i=>i.InventoryID == 4).InventoryID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")}
            };

            foreach (LotInventory l in lotInventories) {
                context.LotInventory.Add(l);
            }
            context.SaveChanges();

            //Maintenance
            var maintenances = new Maintenance[]
            {
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Lawn").CommonAreaAssetID,            Cost = 503.16, DateCompleted = DateTime.Parse("06/11/18"), Description = "Ut tincidunt orci quis lectus. Nullam suscipit, est ac facilisis" ,              LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Swing").CommonAreaAssetID,           Cost = 485.58, DateCompleted = DateTime.Parse("03/15/18"), Description = "elit. Nulla facilisi. Sed neque. Sed eget lacus. Mauris non" ,                   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Well #2").CommonAreaAssetID,         Cost = 806.17, DateCompleted = DateTime.Parse("11/18/17"), Description = "a mi fringilla mi lacinia mattis. Integer eu lacus. Quisque" ,                   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Well #1").CommonAreaAssetID,         Cost = 124.52, DateCompleted = DateTime.Parse("10/24/18"), Description = "facilisis non, bibendum sed, est. Nunc laoreet lectus quis massa." ,             LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Picnic Table #1").CommonAreaAssetID, Cost = 523.72, DateCompleted = DateTime.Parse("04/30/18"), Description = "commodo at, libero. Morbi accumsan laoreet ipsum. Curabitur consequat, lectus" , LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Picnic Table #2").CommonAreaAssetID, Cost = 732.89, DateCompleted = DateTime.Parse("09/29/18"), Description = "Donec non justo. Proin non massa non ante bibendum ullamcorper." ,               LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Trees").CommonAreaAssetID,           Cost = 212.79, DateCompleted = DateTime.Parse("06/08/18"), Description = "Praesent interdum ligula eu enim. Etiam imperdiet dictum magna. Ut" ,            LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Well #3").CommonAreaAssetID,         Cost = 509.06, DateCompleted = DateTime.Parse("01/06/18"), Description = "Cras interdum. Nunc sollicitudin commodo ipsum. Suspendisse non leo. Vivamus" ,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Picnic Table #2").CommonAreaAssetID, Cost = 720.83, DateCompleted = DateTime.Parse("12/05/17"), Description = "libero. Proin mi. Aliquam gravida mauris ut mi. Duis risus" ,                    LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Lawn").CommonAreaAssetID,            Cost = 654.31, DateCompleted = DateTime.Parse("04/12/18"), Description = "iaculis quis, pede. Praesent eu dui. Cum sociis natoque penatibus" ,             LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Swing").CommonAreaAssetID,           Cost = 194.15, DateCompleted = DateTime.Parse("05/09/18"), Description = "consequat enim diam vel arcu. Curabitur ut odio vel est" ,                       LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new Maintenance { CommonAreaAssetID = commonAreaAssets.SingleOrDefault(i=>i.AssetName=="Trees").CommonAreaAssetID,           Cost = 785.85, DateCompleted = DateTime.Parse("10/03/18"), Description = "amet nulla. Donec non justo. Proin non massa non ante" ,                         LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
            };

            foreach (Maintenance m in maintenances) {
                context.Maintenance.Add(m);
            }
            context.SaveChanges();

            //Owner Contact Type
            var ownerContactTypes = new OwnerContactType[]
            {
                new OwnerContactType { ContactValue = "(208)333-2323",         ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Primary Phone").ContactTypeID,   OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,      LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "(801)353-2323",         ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Secondary Phone").ContactTypeID, OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,      LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "caseyb@outlook.com",    ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Email").ContactTypeID,           OwnerID = owners.SingleOrDefault(i => i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "(801)733-4423",         ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Primary Phone").ContactTypeID,   OwnerID = owners.SingleOrDefault(i => i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "fiines@gmail.com",      ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Email").ContactTypeID,           OwnerID = owners.SingleOrDefault(i => i.FirstName == "Marshall" && i.LastName == "Rice").OwnerID,      LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "quynnmann@gmail.com",   ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Email").ContactTypeID,           OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,      LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "breeLawrence",          ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Email").ContactTypeID,           OwnerID = owners.SingleOrDefault(i => i.FirstName == "Bree"     && i.LastName == "Lawrence").OwnerID,  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")},
                new OwnerContactType { ContactValue = "dariusrogers@mail.com", ContactTypeID = contactTypes.SingleOrDefault(i => i.Value == "Email").ContactTypeID,           OwnerID = owners.SingleOrDefault(i => i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,    LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18")}
            };

            foreach (OwnerContactType o in ownerContactTypes) {
                context.OwnerContactType.Add(o);
            }
            context.SaveChanges();

            //Transaction Type
            var transactionTypes = new TransactionType[]
            {
                new TransactionType { Description = "Monthly Dues",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new TransactionType { Description = "New Key",       LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new TransactionType { Description = "Fine",          LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
            };

            foreach (TransactionType t in transactionTypes) {
                context.TransactionType.Add(t);
            }
            context.SaveChanges();

            //Transaction
            var transactions = new Transaction[]
            {
                new Transaction { Description = "nascetur ridiculus mus. Proin vel arcu eu odio tristique pharetra.", Amount = 36.02, DateAdded = DateTime.Parse("06/07/18"), DatePaid = DateTime.Parse("04/19/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Monthly Dues").TransactionTypeID, Status="Open",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "et tristique pellentesque, tellus sem mollis",                       Amount = 59.90, DateAdded = DateTime.Parse("11/02/18"), DatePaid = DateTime.Parse("07/02/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Casey"    && i.LastName == "Buchanan").OwnerID, TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Monthly Dues").TransactionTypeID, Status="Exempt", LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "Proin dolor.",                                                       Amount = 34.27, DateAdded = DateTime.Parse("10/03/18"), DatePaid = DateTime.Parse("10/14/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Tamekah"  && i.LastName == "Patel").OwnerID,    TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Fine").TransactionTypeID,         Status="Paid",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "lectus rutrum",                                                      Amount = 50.32, DateAdded = DateTime.Parse("12/19/17"), DatePaid = DateTime.Parse("05/25/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Quynn"    && i.LastName == "Mann").OwnerID,     TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "New Key").TransactionTypeID,      Status="Paid",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "et, commodo at, libero. Morbi accumsan",                             Amount = 20.00, DateAdded = DateTime.Parse("11/26/17"), DatePaid = DateTime.Parse("04/02/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Marshall" && i.LastName == "Rice").OwnerID,     TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Monthly Dues").TransactionTypeID, Status="Open",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "eros. Proin ultrices. Duis volutpat nunc sit amet metus. Aliquam",   Amount = 20.00, DateAdded = DateTime.Parse("05/30/18"), DatePaid = DateTime.Parse("02/02/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Reagan"   && i.LastName == "Morrow").OwnerID,   TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "New Key").TransactionTypeID,      Status="Exempt", LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "Quisque fringilla euismod enim. Etiam gravida molestie arcu.",       Amount = 43.68, DateAdded = DateTime.Parse("05/31/18"), DatePaid = DateTime.Parse("03/21/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Bree"     && i.LastName == "Lawrence").OwnerID, TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Fine").TransactionTypeID,         Status="Open",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new Transaction { Description = "ut lacus. Nulla tincidunt,",                                         Amount = 49.38, DateAdded = DateTime.Parse("05/15/18"), DatePaid = DateTime.Parse("09/08/18"), OwnerID = owners.SingleOrDefault(i => i.FirstName == "Darius"   && i.LastName == "Rogers").OwnerID,   TransactionTypeID = transactionTypes.SingleOrDefault(i => i.Description == "Monthly Dues").TransactionTypeID, Status="Paid",   LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") }
            /*#1#};

            foreach (Transaction t in transactions) {
                context.Transaction.Add(t);
            }
            context.SaveChanges();

            //User
            var users = new User[]
            {
                new User { OwnerID=1, UserName = "admin@admin.com", UserPassword = "e4bc29dc8068a4f8be92dff03fa14336d460bf5bb185a28f77b3afad8de4fce8", UserType = "Admin",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=2, UserName = "user@user.com",   UserPassword = "e4bc29dc8068a4f8be92dff03fa14336d460bf5bb185a28f77b3afad8de4fce8", UserType = "Owner",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=3, UserName = "test@test.com,",  UserPassword = "9f86d081884c7d659a2feaa0c55ad015a3bf4f1b2b0b822cd15d6c15b0f00a08", UserType = "Owner",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=4, UserName = "nisl",            UserPassword = "SXF49OEW5LF",                                                      UserType = "Owner",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=5, UserName = "non",             UserPassword = "XZC40WTL9CC",                                                      UserType = "Admin",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=6, UserName = "neque",           UserPassword = "JZZ97ZKE0VQ",                                                      UserType = "Admin",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=7, UserName = "enim.",           UserPassword = "OYN90CJR1II",                                                      UserType = "Admin",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=8, UserName = "quis,",           UserPassword = "WTG01SIW5KM",                                                      UserType = "Admin",  LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
                new User { OwnerID=9, UserName = "luctus",          UserPassword = "LQK58APU6MM",                                                      UserType = "Admin,", LastModifiedBy = "DIS", LastModifiedDate = DateTime.Parse("11/09/18") },
            };

            foreach (User c in users) {
                context.User.Add(c);
            }
            context.SaveChanges();
        }
    }*/
}