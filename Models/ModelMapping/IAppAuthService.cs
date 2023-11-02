using BlogWebApi.Models.Domain;
namespace BlogWebApi.Models.ModelMapping
{
    public interface IAppAuthService
    {
        Task<Token> Authenticate(LoginUser loginUser);
    }
}
