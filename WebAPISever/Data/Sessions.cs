namespace WebAPISever.Data
{
    public class Sessions : Dictionary<SessionData, User>
    {
        public Sessions()
        {

        }

        SessionData _selectedKey = new SessionData();
        public SessionData SelectedKey
        {
            get { return _selectedKey; }
        }

        User _selectedValue = new User();
        public User SelectedValue
        {
            get { return _selectedValue; }
        }


        public void SelectWithKey(SessionData key)
        {
            if (ContainsKey(key))
            {
                _selectedKey = key;
                _selectedValue = this[key];
            }
        }

        public void SelectWithValue(User value)
        {
            foreach (var item in this)
            {
                if (Equals(item.Value, value))
                {
                    SelectWithKey(item.Key);
                    break;
                }
            }
        }

        public User GetKeyValue(SessionData key)
        {
            if (ContainsKey(key))
            {
                return this[key];
            }
            else
            {
                return null;
            }
        }

        public User GetSessionValue(string session)
        {
            User user = null;

            foreach (var item in this)
            {
                if(SessionData.Equals(item.Key.Session, session))
                {
                    user = item.Value;
                    break;
                }
            }

            return user;
        }
    }
}
