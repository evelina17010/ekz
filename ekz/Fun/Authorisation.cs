using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ekz.DBconn;

namespace ekz.Fun
{
    internal class Authorisation
    {
        public static ObservableCollection<User> sotrudniks { get; set; }
        public static User AuthorisationSotr(string login, string password)
        {
            sotrudniks = new ObservableCollection<User>(DB.storeEntities.User.ToList());
            var userExists = sotrudniks.Where(sotrudniks => sotrudniks.Email == login && sotrudniks.PasswordHash == password).FirstOrDefault();
            if (userExists != null)
            {
                return userExists;
            }
            else
            {
                return userExists;
            }
        }
    }
}
