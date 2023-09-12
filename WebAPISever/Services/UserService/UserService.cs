using Microsoft.EntityFrameworkCore;

namespace WebAPISever.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ResponseBody<ResultUser>> AddUser(RequestAddUser request)
        {
            var responseBody = new ResponseBody<ResultUser>();

            // 계정 존재 여부 확인
            var checkUserAccount = await _context.Users.AnyAsync(x => x.Account == request.Account);
            if (checkUserAccount)
            {
                responseBody.ResultCode = (int)FailCode.AccountExist;
                responseBody.ResultMessage = "Account Already Exist";
                return responseBody;
            }

            // Make User DB Data
            var user = new User
            {
                Account = request.Account,
                Password = request.Password,
                Name = request.Name,
                Mail = request.Mail,
                PhoneNumber = request.PhoneNumber,
                CreateTime = DateTime.Now,
            };

            // Add User DB and Save
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Make Result Body
            var resultAddUser = new ResultUser
            {
                Account = request.Account,
                Password = request.Password,
                Name = request.Name,
                Mail = request.Mail,
                PhoneNumber= request.PhoneNumber,
            };

            responseBody.ResultCode = (int)HttpStatusCode.OK;
            responseBody.ResultMessage = "OK";
            responseBody.Data = resultAddUser;

            return responseBody;

        }


        public async Task<ResponseBody<SessionData>> LogInUser(RequestLogIn request)
        {
            var responseBody = new ResponseBody<SessionData>();

            // 계정 존재 여부 확인
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Account == request.Account);
            if (user == null)
            {
                responseBody.ResultCode = (int)FailCode.AccountNotFound;
                responseBody.ResultMessage = "Account not found";
                return responseBody;
            }

            // 패스워드 일치여부 확인
            var isPasswordMatched = await _context.Users.AnyAsync(x => x.Account == request.Account && x.Password == request.Password);
            if (!isPasswordMatched)
            {
                responseBody.ResultCode = (int)FailCode.PasswordMismatched;
                responseBody.ResultMessage = "Password mismatched";
                return responseBody;
            }

            // 계정 & 패스워드 있다면 세션ID 발행
            var session = GenerateSessionId();
            responseBody.ResultCode = (int)HttpStatusCode.OK;
            responseBody.ResultMessage = "Ok";
            responseBody.Data = session;

            // 발행한 세션을 Dictionary로 관리
            SessionManager.Instance.AddSession(session, user);

            // 로그인 시간 변경후 DB 저장
            user.LogInTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return responseBody;
        }

        SessionData GenerateSessionId()
        {
            var session = new SessionData();
            session.Session =  Guid.NewGuid().ToString();

            return session;
        }

        public async Task<ResponseBody<SessionData>> LogOutUser(SessionData session)
        {
            var responseBody = new ResponseBody<SessionData>();
            var sessionInfo = await SessionManager.Instance.LogOutUser(session);

            if (sessionInfo == null)
            {
                responseBody.ResultCode = (int)HttpStatusCode.NotFound;
                responseBody.ResultMessage = "Not Found SessionId";
                return responseBody;
            }

            responseBody.ResultCode = (int)HttpStatusCode.OK;
            responseBody.ResultMessage = "Succeess LogOut By SessionId";
            responseBody.Data = sessionInfo;

            // 발행한 세션 제거
            SessionManager.Instance.RemoveSession(sessionInfo);

            return responseBody;
        }

        public async Task<ResponseBody<bool>> CheckAccount(RequestCheckAccount request)
        {
            var responseBody = new ResponseBody<bool>();
            responseBody.ResultCode = (int)HttpStatusCode.OK;

            // 계정 존재 여부 확인
            var isUser = await _context.Users.AnyAsync(x => x.Account == request.Account);
            if (isUser)
            {
                responseBody.ResultMessage = "Account Exist";
                responseBody.Data = isUser;
                return responseBody;
            }

            responseBody.ResultMessage = "Not Found Account";
            responseBody.Data = isUser;

            return responseBody;
        }

        public async Task<ResponseBody<List<ResultUser>>> List(RequestList request)
        {
            var responseBody = new ResponseBody<List<ResultUser>>();

            // Check Session
            var isSessionValid = SessionManager.Instance.CheckSession(request.SessionId);
            if (!isSessionValid)
            {
                return SessionManager.Instance.NotFoundSession<List<ResultUser>>();
            }

            var users = new List<User>();
            // 최신 저장된 데이터가 가장 먼저 보이도록 내림차순 정렬
            users = await _context.Users.OrderByDescending(x => x.Id).ToListAsync();
            
            var result = new List<ResultUser>();

            foreach (var filteredUser in users)
            {
                var resultUser = new ResultUser
                {
                    Account = filteredUser.Account,
                    Password = filteredUser.Password,
                    Name = filteredUser.Name,
                    Mail = filteredUser.Mail,
                    PhoneNumber = filteredUser.PhoneNumber,
                };

                result.Add(resultUser);
            }

            responseBody.ResultCode = (int)HttpStatusCode.OK;
            responseBody.ResultMessage = "Ok";
            responseBody.Data = result;

            return responseBody;
        }

        public async Task<ResponseBody<List<ResultUser>>> SearchUser(RequestSearch request)
        {
            var responseBody = new ResponseBody<List<ResultUser>>();

            // Check Session
            var isSessionValid = SessionManager.Instance.CheckSession(request.SessionId);
            if (!isSessionValid)
            {
                return SessionManager.Instance.NotFoundSession<List<ResultUser>>();
            }

            // Search User By Name
            var searchedUser = await _context.Users.OrderByDescending(x => x.Id)
                .Where(user => user.Name.Contains(request.SearchData)).ToListAsync();

            // Check Searched User List is Empty
            if (searchedUser.Count <= 0)
            {
                responseBody.ResultCode = (int)FailCode.SearchDataEmpty;
                responseBody.ResultMessage = "Search Data Empty";
                return responseBody;
            }

            var result = new List<ResultUser>();
            foreach (var user in searchedUser)
            {
                var resultUser = new ResultUser
                {
                    Account = user.Account,
                    Password = user.Password,
                    Name = user.Name,
                    Mail = user.Mail,
                    PhoneNumber = user.PhoneNumber,
                };

                result.Add(resultUser);
            }

            responseBody.ResultCode = (int)HttpStatusCode.OK;
            responseBody.ResultMessage = "Ok";
            responseBody.Data = result;

            return responseBody;
        }


        #region Default CRUD
        //--------------------------------- Get All Users ---------------------------------
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> DefaultAddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> UpdateUser(int id, User request)
        {
            // Get Databases
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;

            // Change Databases
            user.Account = request.Account;
            user.Password = request.Password;
            user.Mail = request.Mail;
            user.Name = request.Name;
            user.CreateTime = request.CreateTime;
            user.LogInTime = request.LogInTime;
            user.LogOutTime = request.LogOutTime;

            // Save Changes async
            await _context.SaveChangesAsync();

            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>?> DefaultDeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return await _context.Users.ToListAsync();
        }

    }
    #endregion
}
