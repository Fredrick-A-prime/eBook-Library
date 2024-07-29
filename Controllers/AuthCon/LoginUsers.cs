using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eBook_Library.services.AuthService;
using eBook_Library.Models.Authentication;

namespace eBook_Library.Controllers.AuthCon;

[ApiController]
[Route("/api")]
public class UserLog : ControllerBase {

    private readonly UserManager<User> _userManager;
    private readonly IUserService _userService;
    private readonly GenToken _genToken;

    public UserLog(UserManager<User> userManager, IUserService userService, GenToken genToken) {
        _userManager = userManager;
        _userService = userService;
        _genToken = genToken;
    }

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin(LoginUser model) {

        var user = await _userService.GetUserByEmail(model.Email);
        Console.WriteLine(user);
        if (user != null) {
            var userPwd = await _userManager.CheckPasswordAsync(user, model.Password);
            if (userPwd) {
                var token = _genToken.GenerateToken(model.Email);
                return Ok(new { token });
            }
            return BadRequest("Invalid email or password");
        }
        return BadRequest(ModelState);
    }
}
