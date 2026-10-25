using CampusNova.Application.Common;
using CampusNova.Application.Subject.Command.DeleteSubject;
using CampusNova.Application.Subject.Command.EnrollSubject;
using CampusNova.Application.Subject.Queries.ConsultStudentClasses;
using CampusNova.Domain.Subject.Model;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusNova.Api.Subject.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create-subject")]
        public async Task<IActionResult> EnrollSubject([FromBody] EnrollSubjectCommand request)
        {
            var resp = await _mediator.Send(request);
            return Created(string.Empty, ApiResponse<DBNull>.Success(null, resp));
        }

        [HttpPut("delete-subject")]
        public async Task<IActionResult> DeleteSubject([FromBody] DeleteSubjectCommand request)
        {
            var resp = await _mediator.Send(request);
            return Created(string.Empty, ApiResponse<DBNull>.Success(null, resp));
        }

        [HttpGet("get-student-classes")]
        public async Task<IActionResult> GetStudentClasses()
        {
            var resp = await _mediator.Send(new ConsultStudentClassesQuery());
            return Ok(ApiResponse<StudentClassesResultRecord>.Success(resp, null));
        }
    }
}
