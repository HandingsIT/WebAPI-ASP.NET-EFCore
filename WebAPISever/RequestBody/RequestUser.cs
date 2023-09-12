
namespace WebAPISever.RequestBody
{
    public class RequestAddUser
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class RequestAddUsers
    {
        public List<RequestAddUser>? Users { get; set; }
    }

    public class RequestLogIn
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
    }

    public class RequestCheckAccount
    {
        public string? Account { get; set; }
    }

    public class RequestList
    {
        public string? SessionId { get; set; }
    }

    public class RequestSearch
    {
        public string? SessionId { get; set; }
        public string? SearchData { get; set; }
    }

}
