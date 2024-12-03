using BlogApp.Application.Interfaces;
using BlogApp.Application.Requests;
using BlogApp.Domain.Base.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registra um novo usuário no sistema.
        /// </summary>
        /// <response code="200">Usuário registrado com sucesso, retornando o token JWT gerado</response>
        /// <response code="400">Erro nos parâmetros da requisição (ex: usuário já cadastrado)</response>
        /// <response code="422">Erro de validação, como dados inválidos ou incompletos</response>
        /// <response code="500">Erro interno no servidor durante o processo de registro</response>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
        {
            var (response, token) = await _authService.RegisterAsync(request);

            if (!response.IsValid() || string.IsNullOrEmpty(token))
                return UnprocessableEntity(response.ToValidationErrors());

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Realiza o login de um usuário no sistema.
        /// </summary>
        /// <response code="200">Login bem-sucedido, retornando o token JWT gerado</response>
        /// <response code="401">Credenciais inválidas (nome de usuário ou senha incorretos)</response>
        /// <response code="422">Erro de validação, como dados inválidos ou incompletos</response>
        /// <response code="500">Erro interno no servidor durante o processo de login</response>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var (response, token) = await _authService.LoginAsync(request);

            if (!response.IsValid() || string.IsNullOrEmpty(token))
                return UnprocessableEntity(response.ToValidationErrors());

            return Ok(new { Token = token });
        }
    }
}