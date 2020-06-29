using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace HOASunridge.Models {

    [Table("HOA_ClassifiedListing")]
    public class ClassifiedListing {
        public int ClassifiedListingID { get; set; }
        public int OwnerID { get; set; }
        public int ClassifiedCategoryID { get; set; }

        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price. Please enter in digits are decimals only.")]
        public double? Price { get; set; }

        public string Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public DateTime ListingDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:###-###-####}")]
        public string Phone { get; set; }

        public string Email { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public Owner Owner { get; set; }
        public ClassifiedCategory ClassifiedCategory { get; set; }

        [Display(Name = "Files")]
        public ICollection<File> Files { get; set; }

        public string GetInlineImageSrc(File newFile) {
            if (newFile == null || newFile.ImageContentType == null)
                return null;

            var base64Image = System.Convert.ToBase64String(newFile.FileStream);
            return $"data:{newFile.ImageContentType};base64,{base64Image}";
        }

        public void SetImage(IFormFile formFile, int isMainImage = 0) {
            if (formFile == null)
                return;
            File newFile = new File();

            newFile.ImageContentType = formFile.ContentType;
            //newFile.isMainImage = isMainImage;                    //Allow for user to specify main image in future
            newFile.Description = formFile.FileName;

            using (var stream = new System.IO.MemoryStream()) {
                formFile.CopyTo(stream);
                newFile.FileStream = stream.ToArray();
            }

            Files = Files ?? new List<File>();
            Files.Add(newFile);
        }
    }
}