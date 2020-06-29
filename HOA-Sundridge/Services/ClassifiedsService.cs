//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using HOASunridge.Models;
//using Microsoft.EntityFrameworkCore;
//
//namespace HOASunridge.Services {
//
//    public class ClassifiedsService : IClassifiedsService {
//        private readonly HOAContext _context;
//
//        public ClassifiedsService() {
//            var options = new DbContextOptionsBuilder<HOAContext>()
//                .UseInMemoryDatabase("HOASunridge")
//                .Options;
//
//            _context = new HOAContext(options);
//        }
//
//        public ClassifiedsService(HOAContext context) {
//            _context = context;
//        }
//
//        public ClassifiedListing Find(int id) {
//            var s = _context.ClassifiedListing.FirstOrDefault(x => x.ClassifiedListingID == id);
//            return s;
//        }
//    }
//}