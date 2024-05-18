using KnowledgeNest.Entities.Identity;
using KnowledgeNest.Models;
using KnowledgeNest.Repositories.Interfaces;
using KnowledgeNest.Services.Interfaces;

namespace KnowledgeNest.Services.Implementations
{
    public class IdentityService : IIdentityService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public IdentityService(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public BaseResponse AddRole(CreateRoleModel model)
        {
            //check role exist
            var roleExist = _roleRepository.GetRole(model.Name);
            if(roleExist is not null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = $"{model.Name} already exists."
                };
            }

            var role = new Role
            {
                Name = model.Name,
                Description = model.Description
            };

            _roleRepository.AddRole(role);
            _unitOfWork.SaveChanges();
            return new BaseResponse
            {
                Status = false,
                Message = $"{model.Name} already exists."
            };
        }

        public IEnumerable<RoleDTO> GetRoles()
        {
            var roles = _roleRepository.GetRoles();
            return roles.Select(r => new RoleDTO
            {
                Name = r.Name
            }).ToList();
        }

        public UserResponse GetUser(string userName)
        {
            var user = _userRepository.FindByName(userName);
            if(user is null)
            {
                return null;
            }
            return new UserResponse
            {
                UserName = user.UserName,
                Email = user.Email
            };
        }

        public UserResponse Login(LoginModel model)
        {
            // Check if user exists by username or email
            var user = _userRepository.FindByName(model.UserName) ?? _userRepository.FindByEmail(model.UserName);

            if (user != null)
            {
                // Verify the password
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash);

                if (isPasswordValid)
                {
                    return new UserResponse
                    {
                        Status = true,
                        Message = "Login successful.",
                        Email = user.Email,
                        UserName = user.UserName,
                        Roles = user.UserRoles.Select(x => x.Role.Name).ToList()
                    };
                }
                else
                {
                    return new UserResponse
                    {
                        Status = false,
                        Message = "Invalid credentials"
                    };
                }
            }
            else
            {
                return new UserResponse
                {
                    Status = false,
                    Message = "Invalid credentials"
                };
            }
        }

        public BaseResponse Register(RegisterModel model)
        {
            //check if user already exist
            var userNameExist = _userRepository.FindByName(model.UserName);
            if (userNameExist is not null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = $"{model.UserName} already exist. Choose different username"
                };
            }
            var userEmailExist = _userRepository.FindByEmail(model.Email);
            if(userEmailExist != null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = $"{model.Email} already exist. Choose different email"
                };
            }

            // Generate a salt
            string salt = BCrypt.Net.BCrypt.GenerateSalt();

            // Hash the password using the generated salt
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password, salt);

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Salt = salt
            };

            //check the role
            var role = _roleRepository.GetRole(model.Role);
            if(role is null)
            {
                return new BaseResponse
                {
                    Status = false,
                    Message = $"Invalid Role selected"
                };
            }

            //Assign role to the user
            var userRole = new UserRole
            {
                UserId = user.Id,
                RoleId = role.Id,
            };

            user.UserRoles.Add(userRole);

            _userRepository.AddUser(user);
            _unitOfWork.SaveChanges();
            return new BaseResponse
            {
                Status = true,
                Message = $"You have succesfully registered"
            };
        }
    }
}
