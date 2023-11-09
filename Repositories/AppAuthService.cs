using BlogWebApi.Models;
using BlogWebApi.Models.Domain;
using BlogWebApi.Models.ModelMapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogWebApi.Repositories
{
    public class AppAuthService : IAppAuthService
    {
        private readonly IConfiguration _configuration;

        private readonly BlogDbwebapiContext _context;

        public AppAuthService(IConfiguration configuration, BlogDbwebapiContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<Token> Authenticate(LoginUserDTO loginUser)
        {
            //We check the inputs if its correct
            if (loginUser == null || string.IsNullOrEmpty(loginUser.Username) || string.IsNullOrEmpty(loginUser.Password))
                throw new Exception($"Invalid inputs received!");

            //Checking if the informations of the user are correct in the database and join with the Role table to the role Name 
            var user = _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Username.Equals(loginUser.Username) && u.Password.Equals(loginUser.Password));

            if (user == null)
                throw new Exception($"Username with the name {loginUser.Username} is not found or Password is Invalide!");

            //User name and password are valid
            //Generate JSON Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name )
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { AuthToken = tokenHandler.WriteToken(token) };
        }
    }
}
