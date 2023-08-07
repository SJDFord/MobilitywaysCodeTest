using Microsoft.AspNetCore.Mvc;
using MobilitywaysCodeTest.Authentication.Abstractions;
using MobilitywaysCodeTest.DataService.Abstractions;
using MobilitywaysCodeTest.Entities;
using MobilitywaysCodeTest.Views;

namespace MobilitywaysCodeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly IDataService<User> _userDataService;
        private readonly ITokenManager _tokenManager;

        public AuthenticationController(IDataService<User> userDataService, ITokenManager tokenManager) {
            _userDataService = userDataService;
            _tokenManager = tokenManager;
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request) {
            // MAINTAINABILITY/TESTABILITY/REUSABILITY: This logic should be done in a business logic class rather than directly in the controller
            // PERFORMANCE: In production software we'd look a user up via email rather than reading all users in (or we'd most likely use a dedicated auth provider)
            var users = await _userDataService.ReadMultiple();
            var user = users.Find(u => {
                // SECURITY: For production software this password would not be stored as plaintext like this.
                // It would instead be salted and hashed in order to verify.
                // We would most likely just use a dedicated auth provider however instead of trying to create our own auth
                return u.Email.ToLower() == request.Email.ToLower() && 
                    u.Password == request.Password;
            });
            if (user == null) {
                return Unauthorized();
            }
            var token = _tokenManager.GenerateToken(user.Id);
            var response = new LoginResponse
            {
                Jwt = token
            };

            return Ok(response);
        }
    }
}
