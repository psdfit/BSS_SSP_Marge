using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class TraineeGuruModel:ModelBase
    {
        public int? GuruProfileID { get; set; }

        [Required]
        [StringLength(15,ErrorMessage ="FullName cannot exceed 256 characters")]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [StringLength(15, ErrorMessage = "ContactNumber cannot exceed 15 characters.")]
        public string ContactNumber { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "CNIC cannot exceed 15 characters.")]
        public string CNIC { get; set; }

        [Required]
        public DateTime CNICIssuedDate { get; set; }

     

    }
}

