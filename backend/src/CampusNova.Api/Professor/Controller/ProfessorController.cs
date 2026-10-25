using CampusNova.Application.Common;
using CampusNova.Application.Professor.Queries.ConsultProfessor;
using CampusNova.Domain.Professor.Record;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusNova.Api.Professor.Controller
{
    [Route("v1/api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfessorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProfessorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-subjects")]
        public async Task<IActionResult> GetSubjects()
        {
            var resp = await _mediator.Send(new ConsultProfessorQuery());
            return Ok(ApiResponse<SubjectsResultRecord>.Success(resp, null));
        }
    }
}
