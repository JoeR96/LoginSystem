using AutoMapper;
using LoginSystem.Entities;
using LoginSystem.Models;
using LoginSystem.Models.Register;
using LoginSystem.Persistence;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LoginSystemTests")]
namespace LoginSystem.Services
{
    public class LoginService : ILoginService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IJwtUtilities _jwtUtils;

        public LoginService(DataContext context, IMapper mapper,
                    IJwtUtilities jwtUtils)
        {
            _jwtUtils = jwtUtils;
            _dataContext = context;
            _mapper = mapper;
        }

        public void Register(LoginSignUpRequest model)
        {
            if (VerifyDuplicateUser(model))
                throw new AppException("Username '" + model.Username + "' is already taken");

            var user = _mapper.Map<User>(model);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();
        }

        public bool VerifyDuplicateUser(LoginSignUpRequest model)
        => _dataContext.Users
                .Any(u =>
                u.Username == model.Username ||
                u.Email == model.Email);


        public User FindUserWithUserName(string username) =>
            _dataContext.Users.Where(x => x.Username == username).FirstOrDefault();

        public User FindUserWithEmail(string email) =>
            _dataContext.Users.Where(x => x.Email == email).FirstOrDefault();

        public AuthenticateResponse Login(LoginSignUpRequest request)
        {
            var user = request.Username == null ? FindUserWithUserName(request.Username)
                : FindUserWithEmail(request.Email);

            if (user == null)
                throw new AppException("Invalid Username or Email");

            if (!AuthenticateUser(user.PasswordHash, request.Password))
                throw new AppException("Invalid Password");

            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public bool AuthenticateUser(string passwordHash, string password)
            => BCrypt.Net.BCrypt.Verify(password, passwordHash);
        public IEnumerable<User> GetAll() => _dataContext.Users;

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.Username != user.Username && _dataContext.Users.Any(x => x.Username == model.Username))
                throw new AppException("Username '" + model.Username + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            // copy model to user and save
            _mapper.Map(model, user);
            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
        }


        public User GetById(int id) => getUser(id);
        private User getUser(int id)
        {
            var user = _dataContext.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
