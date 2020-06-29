using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HOASunridge.Models {

    [Table("HOA_ClassifiedCategory")]
    public class ClassifiedCategory {
        public int ClassifiedCategoryID { get; set; }

        [DefaultValue("false")]
        public bool IsArchive { get; set; }

        [Display(Name = "Category")]
        public string Description { get; set; }
    }
}