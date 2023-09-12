namespace WebAPISever.ResponseBody
{
    public class ResultUser
    {
        public string? Account { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Mail { get; set; }
        public string? PhoneNumber { get; set; }
    }


    public class ResultLogOut
    {
        public string? Session { get; set; }
    }

    public class ResultFind
    {
        public string? Account { get; set; }
    }

}
