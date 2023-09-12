using Microsoft.AspNetCore.Mvc;

namespace WebAPISever.Manager
{
    public class SessionData
    {
        [FromHeader]
        public string? Session { get; set; }
    }


    public class SessionManager : Singleton<SessionManager>
    {
        private Sessions _sessions = new Sessions();
        public Sessions Sessions
        {
            get { return _sessions; }
            set 
            {
                _sessions = value;
            }
        }

        protected override void OnInit()
        {

        }

        public void AddSession(SessionData key, User value)
        {
            Sessions.Add(key, value);
            WriteSessionCount(Sessions.Count);
        }

        public void RemoveSession(SessionData key)
        {
            Sessions.Remove(key);
            WriteSessionCount(Sessions.Count);
        }

        void WriteSessionCount(int count)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"Session Count : ");

            Console.ResetColor();
            Console.Write(count);

            Console.WriteLine();
        }


        public async Task<SessionData> LogOutUser(SessionData session)
        {
            var findSession = await Task.Run<SessionData?>(() => FindSession(session));
            return findSession;
        }


        User FindUserBySessionId(SessionData session)
        {
            User user = null;

            foreach (var item in Sessions)
            {
                if (item.Key.Session == session.Session)
                {
                    user = item.Value;
                }
            }

            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }

        SessionData FindSession(SessionData session)
        {
            SessionData resultLogIn = null;

            foreach (var item in Sessions)
            {
                if (item.Key.Session == session.Session)
                {
                    resultLogIn = item.Key;
                    break;
                }
                else
                {
                    resultLogIn = null;
                }
            }

            return resultLogIn;
        }

        public bool CheckSession(string? session)
        {
            bool isToken = false;

            foreach (var item in Sessions)
            {
                if (item.Key.Session == session)
                {
                    isToken = true;
                    break;
                }
            }

            return isToken;
        }

        public ResponseBody<T> NotFoundSession<T>()
        {
            var returnValue = new ResponseBody<T>();

            try
            {
                returnValue.ResultCode = (int)FailCode.SessionNotFound;
                returnValue.ResultMessage = "Token Not Found";

                return returnValue;
            }
            catch (Exception ex) 
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Worning! :");

                Console.ResetColor();
                Console.WriteLine(ex.ToString());

                returnValue.ResultCode = (int)HttpStatusCode.NotFound;
                returnValue.ResultMessage = $"Can't Find {nameof(T)}";

                return returnValue;
            }

        }
    }

}
