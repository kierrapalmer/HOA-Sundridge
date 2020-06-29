using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_File")]
    public class File {
        public int FileID { get; set; }
        public int? OwnerHistoryID { get; set; }
        public int? ClassifiedListingID { get; set; }
        public int isMainImage { get; set; }
        public string FileURL { get; set; }
        public string ImageContentType { get; set; }
        public byte[] FileStream { get; set; }

        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}