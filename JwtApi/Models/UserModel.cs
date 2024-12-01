namespace JwtApi.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Function { get; set; }
        public string ApiClientName { get; set; }
        public string CustomClaim { get; internal set; }
    }
}
