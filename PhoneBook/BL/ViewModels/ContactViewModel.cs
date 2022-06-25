using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BL.ViewModels
{
   public  class ContactViewModel
    {
        [Required]
        public int ContactID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [Range(5, 11, ErrorMessage = "invalid number")]
        public string PersonalPhone { get; set; }
        [Range(5, 11, ErrorMessage = "invalid number")]
        public string WorkPhone { get; set; }
        [Range(5, 11, ErrorMessage = "invalid number")]
        public string HomePhone { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
    }
}
