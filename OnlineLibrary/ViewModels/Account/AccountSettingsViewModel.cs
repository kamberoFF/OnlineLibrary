using System.ComponentModel.DataAnnotations;

namespace OnlineLibrary.ViewModels.Account
{
    public class AccountSettingsViewModel
    {
        public UpdateAccountViewModel UpdateAccountViewModel { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModel { get; set; }
    }
}