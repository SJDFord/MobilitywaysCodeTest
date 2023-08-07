namespace MobilitywaysCodeTest.Authentication.Abstractions
{
    public class AuthenticationOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
