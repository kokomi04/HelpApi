using HelpApi.Authorization;
using HelpApi.EF;
using HelpApi.Models;

namespace HelpApi.Services
{
    public interface IUserService
    {
        string Authenticate(UserModel model);
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User? GetByName(string userName);
    }

    public class UserService : IUserService
    {

        private readonly IJwtUtils _jwtUtils;
        private readonly MasterDBContext _masterDBContext;

        public UserService(MasterDBContext masterDBContext, IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
            _masterDBContext = masterDBContext;
        }

        public string Authenticate(UserModel model)
        {
            var user = _masterDBContext.User.SingleOrDefault(x => x.UserName == model.UserName); // && x.PasswordSalt == model.Password);

            // return null if user not found
            if (user == null) return "Not Found";

            // authentication successful so generate jwt token
            var token = _jwtUtils.GenerateJwtToken(user);

            return token;
        }

        public IEnumerable<User> GetAll()
        {
            return _masterDBContext.User.ToList();
        }

        public User? GetById(int id)
        {
            return _masterDBContext.User.FirstOrDefault(x => x.UserId == id);
        }
        public User? GetByName(string userName)
        {
            return _masterDBContext.User.FirstOrDefault(x => x.UserName == userName);
        }
    }
}