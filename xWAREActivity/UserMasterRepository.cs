using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xWAREActivity.Models;

namespace xWAREActivity
{
    public class UserMasterRepository:IDisposable
    {
        ActivityDBEntities context = new ActivityDBEntities();

        public User ValidateUser(string email, string password)
        {
            return context.Users.First(user =>
            user.email.Equals(email, StringComparison.OrdinalIgnoreCase)
            && user.password == password);
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}