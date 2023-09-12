
namespace WebAPISever.Services.UserService
{
    public interface IUserService
    {
        Task<ResponseBody<ResultUser>> AddUser(RequestAddUser request);

        Task<ResponseBody<SessionData>> LogInUser(RequestLogIn request);

        Task<ResponseBody<SessionData>> LogOutUser(SessionData session);

        Task<ResponseBody<bool>> CheckAccount(RequestCheckAccount request);

        Task<ResponseBody<List<ResultUser>>> List(RequestList request);

        Task<ResponseBody<List<ResultUser>>> SearchUser(RequestSearch request);


        // Default CRUD
        Task<List<User>> GetAllUsers();

        // POST User DB
        Task<User> DefaultAddUser(User user);

        // Update User DB
        Task<User?> UpdateUser(int id, User request);

        // Drop User DB
        Task<List<User>?> DefaultDeleteUser(int id);
    }
}
