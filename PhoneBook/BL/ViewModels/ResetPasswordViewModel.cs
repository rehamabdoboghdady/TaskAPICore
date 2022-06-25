using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BL.ViewModels
{
   public class ResetPasswordViewModel
    {
        [DataType(DataType.Password), Required(ErrorMessage = "Old Password Required")]
        public string currentPassword { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "New Password Required")]
        public string newPassword { get; set; }
    }
}
