using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using UniversityApiBackend.Models.DataModels;

namespace UniversityApiBackend
{
    public static class AddJwtTokenServiceExtensions
    {
        public static void AddJwtTokenService(this IServiceCollection Services, IConfiguration Configuration)
        {
            //Add appsettings.json
            var bindJwtSettings = new JwtSettings();
            //All propierties
            Configuration.Bind("JsonWebTokenKeys", bindJwtSettings);
            //Add singleton of JWT settings
            Services.AddSingleton(bindJwtSettings);
            //Add Authentication with JWT type Bearer
            Services.AddAuthentication(options =>
            {
                //1.Options use to authenticate users
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //2.OPtions checks the users
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = bindJwtSettings.ValidateIssuerSignKey,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(bindJwtSettings.IssuerSignKey)),
                    ValidateIssuer = bindJwtSettings.ValidateIssuer,
                    ValidIssuer = bindJwtSettings.ValidIssuer,
                    ValidateAudience = bindJwtSettings.ValidateAudience,
                    ValidAudience = bindJwtSettings.ValidAudience,
                    RequireExpirationTime = bindJwtSettings.RequireExpirationTime,
                    ValidateLifetime = bindJwtSettings.ValidateLifeTime,
                    ClockSkew = TimeSpan.FromDays(1),
                };
            });
        }
    }
}
