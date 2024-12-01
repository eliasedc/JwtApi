namespace JwtApi.Models
{
    public class UserModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Function { get; set; }
        public string ApiClientName { get; set; }
        /*
         * Custom user data can be created to add Custom Claim to validate by policy or manually in controller method
         * public string CustomClaim { get; internal set; }*/
    }
}
