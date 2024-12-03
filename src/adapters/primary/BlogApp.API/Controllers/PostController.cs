using BlogApp.Application.Interfaces;
using BlogApp.Application.Requests;
using BlogApp.Application.Responses;
using BlogApp.Domain.Base.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Realiza o cadastro de uma nova postagem
        /// </summary>
        /// <returns>Retorna a postagem cadastrada</returns>
        /// <response code="201">Retorno padrão com dados</response>
        /// <response code="400">Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        /// <response code="500">Retorno padrão informando erros internos</response>
        [HttpPost("Create")]
        [Authorize]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest request)
        {
            request.UserId = GetUserIdFromClaims();

            var (response, createdPost) = await _postService.CreatePostAsync(request);

            if (!response.IsValid() || createdPost is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return Created($"/{createdPost.Id}", createdPost);
        }

        /// <summary>
        /// Realiza a atualização dos de uma postagem
        /// </summary>
        /// <response code="204">Retorno padrão sem dados</response>
        /// <response code="400">Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        /// <response code="500">Retorno padrão informando erros internos</response>
        [HttpPut("Update")]
        [Authorize]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost(PostUpdateRequest request)
        {
            request.UserId = GetUserIdFromClaims();

            var (response, updatePost) = await _postService.UpdatePostAsync(request);

            if (!response.IsValid() || updatePost is null)
                return UnprocessableEntity(response.ToValidationErrors());

            return NoContent();
        }

        /// <summary>
        /// Realiza a obtenção de todas as postagens cadastradas
        /// </summary>
        /// <returns>Retorno padrão que contém lista das postagens</returns>
        /// <response code="200">Retorno padrão com dados</response>
        /// <response code="404">Retorno padrão informando erros que aconteceram</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllPosts()
        {
            var (response, post) = await _postService.GetAllPostsAsync();

            if (!response.IsValid() || !post.Any())
                return NotFound(new Notification(HttpStatusCode.NotFound, "Post List Not Found"));

            return Ok(post);
        }

        /// <summary>
        /// Realiza a obtenção de uma postagem cadastrada tendo o identificador como parâmetro informado
        /// </summary>
        /// <returns>Retorno padrão que contém a postagem</returns>
        /// <response code = "200" > Retorno padrão com dados</response>
        /// <response code = "400" > Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code = "404" > Retorno padrão informando erros que aconteceram</response>
        [HttpGet("Find/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Post Id Invalid");

            var (response, post) = await _postService.GetPostByIdAsync(id);

            if (!response.IsValid() || post is null)
                return NotFound(new Notification(HttpStatusCode.NotFound, "Post Not Found"));

            return Ok(post);
        }

        /// <summary>
        /// Realiza a exclusão de uma postagem tendo o identificador como parâmetro informado
        /// </summary>
        /// <response code="204">Retorno padrão sem dados</response>
        /// <response code="400">Retorno padrão informando erros nos parâmetros da requisição</response>
        /// <response code="422">Retorno padrão informando erros que aconteceram</response>
        /// <response code="500">Retorno padrão informando erros internos</response>
        [HttpDelete("Delete/{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePostAsync(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest("Post Id Invalid");

            var response = await _postService.DeletePostAsync(id);

            if (!response.IsValid())
                return UnprocessableEntity(response.ToValidationErrors());

            return NoContent();
        }

        private Guid GetUserIdFromClaims()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            return userIdClaim == null
                ? throw new UnauthorizedAccessException("User ID claim not found in token.")
                : Guid.Parse(userIdClaim.Value);
        }
    }
}