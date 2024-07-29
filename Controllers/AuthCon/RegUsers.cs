using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using eBook_Library.services.AuthService;
using eBook_Library.Models.Authentication;


namespace eBook_Library.Controllers.AuthCon;


[ApiController]
[Route("/api")]

public class RegUser(UserManager<User> userManager, IUserService userService, GenToken genToken) : ControllerBase {

    private readonly IUserService _userService = userService;
    private readonly UserManager<User> userManager;
    private readonly GenToken _genToken = genToken;

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserModel model, URoles role) {
        if(ModelState.IsValid) {
            var exUser = await _userService.GetUserByEmail(model.Email);

            if(exUser != null) {
                ModelState.AddModelError("", "Email exists already");

                return BadRequest(ModelState);
            }
        }

        var result = await _userService.CreateNewUser(model);
        var roleResult = await _userService.AssignRole(role);

        if (result.Succeeded && roleResult.Succeeded) {

            var token = _genToken.GenerateToken(model.Email);
            return Ok(new { token });
        }
        foreach (var err in result.Errors) {
            ModelState.AddModelError(" ", err.Description);
        }
        return BadRequest(ModelState);
    }
}