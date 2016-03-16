
namespace EncrpytionTest.Models
{
    public class ApiUser
    {
        public int ApiUser_Id { get; set; }
        public int Party_Id { get; set; }
        public string IPAddress { get; set; }
        public string Salt { get; set; }
        public string AESKey { get; set; }
        public string Username { get; set; }
    }

    public class AuthenticatedUser
    {
        public ApiUser ApiUser { get; set; }
        public string Error { get; set; }
    }
}