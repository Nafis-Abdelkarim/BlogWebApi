using BlogWebApi.Models.Domain;
using BlogWebApi.Models.ModelMapping;

namespace BlogWebApi.Repositories
{
    public interface IAppAuthService
    {
        Task<Token> Authenticate(LoginUserDTO loginUser);
    }
}
