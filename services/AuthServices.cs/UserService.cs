using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using eBook_Library.Models.Authentication;

namespace eBook_Library.services.AuthService;

public interface IUserService {

    Task<IdentityResult> CreateNewUser(UserModel model);
    Task<IdentityRole> AssignRole(URoles role, UserModel model);
    Task<User> GetUserByEmail(string email);
}

public class UserService : IUserService {

    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager) {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateNewUser(UserModel model) {

        var newUser = new User {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);
        return result;
    }

    public async Task<IdentityRole> AssignRole(URoles role, UserModel model) {
        
        var user = new User {
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString()
        }

        var result = await _userManager.AddRolesAsync(user, role.User);
        return result;
    }

    public async Task<User> GetUserByEmail(string email) {

            var exUser = await _userManager.FindByEmailAsync(email);
            return exUser;
    }
}




