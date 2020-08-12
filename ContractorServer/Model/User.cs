using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Pass { get; set; }
        public User(string login, string pass)
        {
            Login = login;
            Pass = pass;
        }
        public User()
        {

        }
    }
}
