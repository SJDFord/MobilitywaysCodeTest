using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MobilitywaysCodeTest.DataService.Abstractions;
using MobilitywaysCodeTest.Entities;
using MobilitywaysCodeTest.Views;

namespace MobilitywaysCodeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IDataService<User> _userDataService;

        public UserController(IDataService<User> userDataService) {
            _userDataService = userDataService;
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] UserCreationRequest request) {
            // MAINTAINABILITY/TESTABILITY/REUSABILITY: This logic should be done in a business logic class rather than directly in the controller
            var user = new User
            {
                Email = request.Email,
                Name = request.Name,
                Password = request.Password
            };

            await _userDataService.Create(user);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UserView>>> GetUsers() {
            // MAINTAINABILITY/TESTABILITY/REUSABILITY: This logic should be done in a business logic class rather than directly in the controller
            var users = await _userDataService.ReadMultiple();
            var userViews = users.Select(u => new UserView
            {
                Email = u.Email,
                Name = u.Name
            }).ToList();
            return Ok(userViews);
        }
    }
}
