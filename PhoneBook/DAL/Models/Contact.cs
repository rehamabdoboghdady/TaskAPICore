using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    [Table("Contact")]
    public class Contact
    {
        [Required]
        public int ContactID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PersonalPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        [ForeignKey("User")]
        public string UserID { get; set; }
        public virtual User user { get; set; }
    }
}
