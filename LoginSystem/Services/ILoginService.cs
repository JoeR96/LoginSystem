using LoginSystem.Entities;
using LoginSystem.Models;
using LoginSystem.Models.Register;

namespace LoginSystem.Services
{
    public interface ILoginService
    {
        User FindUserWithEmail(string email);
        User FindUserWithUserName(string username);
        IEnumerable<User> GetAll();
        User GetById(int id);
        AuthenticateResponse Login(LoginSignUpRequest request);
        void Register(LoginSignUpRequest model);
        bool VerifyDuplicateUser(LoginSignUpRequest model);
        bool AuthenticateUser(string passwordHash, string password);
        public void Update(int id, UpdateRequest model);
        public void Delete(int id);
    }
}