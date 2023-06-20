namespace HelpApi.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
    }

    public class UserChangepasswordInput
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
