using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnglishMaster.Shared.Dto.Request
{
    public sealed class UserReqestDto
    {
        public UserReqestDto(string username, string password, string firstName, string lastName)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
        }
        public string Username { get; }
        public string Password { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
