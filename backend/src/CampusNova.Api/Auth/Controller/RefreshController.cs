using CampusNova.Application.Auth.Queries.RefreshToken;
using CampusNova.Application.Common;
using CampusNova.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CampusNova.Api.Auth.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    public class RefreshController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RefreshController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> Refresh()
        {

            var refreshToken = Request.Cookies["refresh_token"];

            Console.WriteLine($"[Controller RAW COMPLETO]: {refreshToken}");

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }

            var result = await _mediator.Send(new RefreshQuery
            {
                RefreshToken = refreshToken
            });

            if (result is null) return Unauthorized();

            Response.Cookies.Append("access_token", result.Token, CookiesServices.AccessTokenOptions);
            Response.Cookies.Append("refresh_token", result.RefreshToken, CookiesServices.RefreshTokenOptions);

            return Ok(ApiResponse<string>.Success("ok", "Token refrescado exitosamente."));

        } 
    }
}
