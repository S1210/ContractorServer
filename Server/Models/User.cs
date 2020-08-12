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
        public User(int id, string login, string pass)
        {
            Id = id;
            Login = login;
            Pass = pass;
        }
        public User()
        {

        }
    }

    
}
