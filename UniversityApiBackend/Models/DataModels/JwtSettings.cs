namespace UniversityApiBackend.Models.DataModels
{
    public class JwtSettings
    {
        //Validate Signature
        public bool ValidateIssuerSignKey { get; set; }
        public string IssuerSignKey { get; set; } = string.Empty;
        //Validate User
        public bool ValidateIssuer { get; set; } = true;
        public string? ValidIssuer { get; set; }
        //
        public bool ValidateAudience { get; set; } = true;
        public string? ValidAudience { get; set;}
        //Time line of Token
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifeTime { get; set; } = true;


    }
}
