using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UniversityApiBackend.Models.DataModels;

namespace UniversityApiBackend.Helpers
{
    public static class JwtHelper
    {
        public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, Guid Id)
        {
            List<Claim> claims = new List<Claim> 
            {
                new Claim("Id", userAccounts.Id.ToString()),
                new Claim(ClaimTypes.Name, userAccounts.UserName),
                new Claim(ClaimTypes.Email, userAccounts.EmailId),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Expiration, DateTime.UtcNow.AddDays(1).ToString("MMM ddd dd yyyy HH:mm:ss tt"))
            };

            if(userAccounts.UserName == "Admin")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));
            }
            else if (userAccounts.UserName == "User1")
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                //claims.Add(new Claim("UserOnly","User1"));
            }

            return claims;
        }

        //Method to return Id Guid
        public static IEnumerable<Claim> GetClaims(this UserTokens userAccounts, out Guid Id)
        {
            //Generate Id
            Id = Guid.NewGuid();
            return GetClaims(userAccounts, Id);
        }

        //Obtain Token
        public static UserTokens GenTokenKey(UserTokens model, JwtSettings jwtSettings)
        {
            try
            {
                var userToken = new UserTokens();
                if(model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                //Obtain Secret Key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSignKey);

                Guid Id;

                //Expires in 1 Day
                DateTime expireTime = DateTime.UtcNow.AddDays(1);

                //Validity of our Token
                userToken.Validity = expireTime.TimeOfDay;

                // GENERATE OUR JWT
                var jwtToken = new JwtSecurityToken(
                        issuer: jwtSettings.ValidIssuer,
                        audience: jwtSettings.ValidAudience,
                        claims: GetClaims(model, out Id),
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        expires: new DateTimeOffset(expireTime).DateTime,
                        signingCredentials: new SigningCredentials(
                                new SymmetricSecurityKey(key),
                                SecurityAlgorithms.HmacSha256
                            )
                    );
                userToken.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
                userToken.UserName = model.UserName;
                userToken.Id = model.Id;
                userToken.GuidId = Id;
                return userToken;

            }catch(Exception ex)
            {
                throw new Exception("Error Generating the JWT", ex);
            }
        }
    }
}
