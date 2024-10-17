namespace Users.Infrastructure.Authentication
{
    public class JsonWebTokenData
    {
        public string Key { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationDays { get; set; }
    }
}
