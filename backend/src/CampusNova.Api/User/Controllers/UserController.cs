using CampusNova.Application.Common;
using CampusNova.Application.Common.Interfaces;
using CampusNova.Application.User.Queries.Login;
using CampusNova.Application.User.Queries.Login.DTO;
using CampusNova.Domain.Common.Interfaces;
using CampusNova.Domain.User.Enum;
using CampusNova.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CampusNova.Api.User.Controllers;

[ApiController]
[Route("v1/api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserServices _context;
    private readonly INotificationService _hub;

    public UserController(IMediator mediator,ICurrentUserServices context, INotificationService hub)
    {
        _mediator = mediator;
        _context = context;
        _hub = hub;
    }

    [HttpPost("login-access")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginQuery request)
    {
        var result = await _mediator.Send(request);

        if (result == null) return BadRequest(ApiResponse<string>.Fail("Usuario y contraseña no autorizados."));

        Response.Cookies.Append("access_token", result.Token, CookiesServices.AccessTokenOptions);
        Response.Cookies.Append("refresh_token", result.RefreshToken, CookiesServices.RefreshTokenOptions);

        result.Token = null;
        result.RefreshToken = null;
        result.Rol = null;
        return Ok(ApiResponse<UserInformationDTO>.Success(result, "Sucess: se encontro informacion relacionada."));
    }

    [HttpGet("info-session")]
    [Authorize]
    public IActionResult Session()
    {
        return Ok(ApiResponse<string>.Success(null,"User connected for session"));
    }

    [HttpGet("logout-session")]
    public async Task<IActionResult> LogoutSession()
    {
        var deleteCookieOptions = new CookieOptions { Path = "/", Secure = true, SameSite = SameSiteMode.None };
        Response.Cookies.Delete("access_token", deleteCookieOptions);
        Response.Cookies.Delete("refresh_token", deleteCookieOptions);
        return Ok(ApiResponse<string>.Success(null, "Sesión cerrada exitosamente."));
    }


    [HttpGet("force-logout-signalr")]
    [Authorize(Roles = $"{RolUser.Admin},{RolUser.Student}")]
    public async Task<IActionResult> ForceLogoutSignal([FromQuery] string id)
    {
        await _hub.SendToUserAsync(id, "ForceLogout");
        return Ok(ApiResponse<string>.Success(null, "Session cerrada en signalr."));
    }

}
