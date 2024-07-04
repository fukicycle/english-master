using EnglishMaster.Server.Services.Interfaces;
using EnglishMaster.Shared.Dto.Response;
using EnglishMaster.Shared.Models;

namespace EnglishMaster.Server.Services
{
    public sealed class UserService : IUserService
    {
        private readonly DB _db;
        private readonly ILogger<UserService> _logger;

        public UserService(DB db, ILogger<UserService> logger)
        {
            _db = db;
            _logger = logger;
        }


        public void Register(string email, string password, string firstName, string lastName, string? nickname, string? iconUrl)
        {
            User user = new User();
            user.Username = email;
            user.Password = password;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Nickname = nickname;
            user.IconUrl = iconUrl;
            if (_db.Users.Any(a => a.Username == user.Username))
            {
                throw new Exception("Already in registed your email.");
            }
            _db.Users.Add(user);
            _db.SaveChanges();
        }
        public UserResponseDto GetUserResponseDto(long userId)
        {
            User? user = _db.Users.Find(userId);
            if (user == null)
            {
                throw new Exception($"No such user:{userId}");
            }
            return new UserResponseDto(user.FirstName, user.LastName, user.Nickname, user.IconUrl);
        }
    }
}
