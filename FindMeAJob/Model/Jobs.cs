using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FindMeAJob.Model
{
    public partial class Jobs
    {
        public int JobId { get; set; }
        [Required]
        [StringLength(255)]
        public string JobTitle { get; set; }
        [Required]
        [StringLength(255)]
        public string WebUrl { get; set; }
        [Required]
        [StringLength(255)]
        public string CompanyName { get; set; }
        [Required]
        [StringLength(255)]
        public string Location { get; set; }
        [Required]
        [StringLength(255)]
        public string JobDescription { get; set; }
        [Required]
        [Column("ImageURL")]
        [StringLength(255)]
        public string ImageUrl { get; set; }
        public bool Applied { get; set; }
    }
}
