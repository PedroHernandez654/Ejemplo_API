using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityApiBackend.DataAccess;
using UniversityApiBackend.Helpers;
using UniversityApiBackend.Models.DataModels;

namespace UniversityApiBackend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UniversityDBContext _dbContext;

        private readonly JwtSettings _jwtSettings;
        public AccountController(UniversityDBContext dbContext, JwtSettings jwtSettings)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings;
        }
        // Example user
        //To do: Change by real user DB
        private IEnumerable<User> Logins = new List<User>() { 
            new User()
            {
                Id = 1,
                Email = "Martin@gmail.com",
                Name = "Admin",
                Password = "Admin"
            },
            new User()
            {
                Id = 2,
                Email = "pepe@gmail.com",
                Name = "User1",
                Password = "Pepe"
            }
        };

        [HttpPost]
        public IActionResult GetToken(UserLogins userLogin)
        {
            try
            {
                var Token = new UserTokens();


                //Search User in context with LinQ
                var searchUser = (from user in _dbContext.Users
                                 where user.Name == userLogin.UserName && user.Password == userLogin.Password
                                 select user).FirstOrDefault();

                Console.WriteLine("User Found: ", searchUser);

                
                //var Valid = Logins.Any(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));

                //var Valid = searchUser != null;
                

                //Generate de Web Token by searhUser
                if (searchUser != null)
                {
                    //var user = Logins.FirstOrDefault(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));
                    Token = JwtHelper.GenTokenKey(new UserTokens()
                    {
                        UserName = searchUser.Name,
                        EmailId = searchUser.Email,
                        Id = searchUser.Id,
                        GuidId = Guid.NewGuid()

                    },_jwtSettings);
                }
                else
                {
                    return BadRequest("Wrong Credentials");
                }
                return Ok(Token);

            }catch(Exception ex)
            {
                throw new Exception("Get Token Error", ex);
            }
        }
        //Authorization to endpoint
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public IActionResult GetUserList()
        {
            return Ok(Logins);
        }

    }
}
