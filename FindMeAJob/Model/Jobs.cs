using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

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

    [DataContract]
    public class JobDTO
    {
        [DataMember]
        public int JobId { get; set; }

        [DataMember]
        public string JobTitle { get; set; }

        [DataMember]
        public string WebUrl { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public string Location { get; set; }

        [DataMember]
        public string JobDescription { get; set; }

        [DataMember]
        public string ImageURL { get; set; }

        [DataMember]
        public bool Applied { get; set; }
    }
}
