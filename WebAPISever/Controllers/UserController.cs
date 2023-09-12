using WebAPISever.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPISever.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService usersService)
        {
            _userService = usersService;
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<ResponseBody<ResultUser>>> AddUser(RequestAddUser request)
        {
            var result = await _userService.AddUser(request);
            return Ok(result);
        }


        [HttpPost("LogIn")]
        public async Task<ActionResult<ResponseBody<SessionData>>> LogInUser(RequestLogIn logInUser)
        {
            var result = await _userService.LogInUser(logInUser);
            return Ok(result);
        }

        [HttpPost("LogOut")]
        public async Task<ActionResult<ResponseBody<SessionData>>> LogOutUser(SessionData logInUser)
        {
            var result = await _userService.LogOutUser(logInUser);
            return Ok(result);
        }

        [HttpPost("Check")]
        public async Task<ActionResult<ResponseBody<bool>>> CheckAccount(RequestCheckAccount request)
        {
            var result = await _userService.CheckAccount(request);
            return Ok(result);
        }

        [HttpPost("List")]
        public async Task<ActionResult<ResponseBody<List<ResultUser>>>> List(RequestList request)
        {
            var result = await _userService.List(request);
            return Ok(result);
        }

        [HttpPost("Search")]
        public async Task<ActionResult<ResponseBody<List<ResultUser>>>> SearchStudentList(RequestSearch request)
        {
            var result = await _userService.SearchUser(request);
            return Ok(result);
        }

        #region Default CRUD
        //--------------------------------- Get All Users ---------------------------------
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }


        // ----------------------------------- Post ----------------------------------------------
        [HttpPost]
        public async Task<ActionResult<User>> DefaultAddUser(User user)
        {
            var result = await _userService.DefaultAddUser(user);
            if (result is null)
                return NotFound("Users not found.");

            return Ok(result);
        }


        // ----------------------------------- Put ----------------------------------------------
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, User request)
        {
            var result = await _userService.UpdateUser(id, request);
            if (result is null)
                return NotFound("Users not found.");

            return Ok(result);
        }

        // ----------------------------------- Delete -------------------------------------------
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DefaultDeleteUser(int id)
        {
            var result = await _userService.DefaultDeleteUser(id);
            if (result is null)
                return NotFound("Users not found.");

            return Ok(result);
        }

        #endregion

    }

}
