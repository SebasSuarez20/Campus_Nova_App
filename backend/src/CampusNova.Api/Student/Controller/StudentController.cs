using CampusNova.Application.Common;
using CampusNova.Application.Student.Command.CreateStudent;
using CampusNova.Application.Student.Command.DeleteStudent;
using CampusNova.Application.Student.Command.UpdateStudent;
using CampusNova.Application.Student.Queries.ConsultStudent;
using CampusNova.Domain.Student.Record;
using CampusNova.Domain.User.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CampusNova.Api.Student.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    [Authorize(Roles = $"{RolUser.Admin},{RolUser.Student}")]
    public class StudentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("get-student-full")]
        public async Task<IActionResult> GetStudent()
        {
            var resp = await _mediator.Send(new ConsultStudentQuery());
            return Ok(ApiResponse<IEnumerable<GetStudentRecord>>.Success(resp, null));
        }

        [AllowAnonymous]
        [HttpPost("create-student")]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentCommand request)
        {
            var resp = await _mediator.Send(request);
            return Created(string.Empty, ApiResponse<DBNull>.Success(null, resp));
        }

        [HttpPut("update-student")]

        public async Task<IActionResult> UpdateStudent([FromBody] UpdateStudentCommand request)
        {
            var resp = await _mediator.Send(request);
            return Ok(ApiResponse<DBNull>.Success(null, resp));
        }

        [HttpDelete("delete-student")]
        public async Task<IActionResult> DeleteStudent()
        {
            var resp = await _mediator.Send(new DeleteStudentCommand());

            var deleteCookieOptions = new CookieOptions { Path = "/", Secure = true, SameSite = SameSiteMode.None };
            Response.Cookies.Delete("access_token", deleteCookieOptions);
            Response.Cookies.Delete("refresh_token", deleteCookieOptions);

            return Ok(ApiResponse<DBNull>.Success(null, resp));
        }

    }
}
