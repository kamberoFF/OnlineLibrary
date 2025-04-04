﻿using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.ViewModels.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}