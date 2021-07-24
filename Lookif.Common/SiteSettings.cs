namespace Lookif.Library.Common
{
    public abstract class SiteSettings
    {
        public string ElmahPath { get; set; }
        public string ImageSource { get; set; }
        public string WebImageSource { get; set; }
        public JwtSettings JwtSettings { get; set; }       
        public IdentitySettings IdentitySettings { get; set; }
    }

    public abstract class IdentitySettings
    {
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumic { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool RequireUniqueEmail { get; set; }
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; }
        public string Encryptkey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int NotBeforeMinutes { get; set; }
        public int ExpirationMinutes { get; set; }
    }
    
 

}
