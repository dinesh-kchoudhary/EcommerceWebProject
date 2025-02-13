using BrainHubTest.Data.Model;
using BrainHubTest.Model;

namespace BrainHubTest.Services
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string email, string password);
        Task<ApiResponse<string>> RegisterAsync(string email, string password);
    }
}
