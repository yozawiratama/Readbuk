using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Borrowings.Commands;
using Application.Features.Borrowings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Readbuk.Api.Controllers.v1
{
    [ApiVersion("1.0"), Authorize]
    public class BorrowingController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateBorrowingCommand command)
        {
            command.SignedInUserId = User.FindFirst("uid")?.Value;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllBorrowingQuery()));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReturn(Guid id, UpdateReturnBorrowingCommand command)
        {
            if (id != command.ID)
            {
                return BadRequest();
            }
            command.SignedInUserId = User.FindFirst("uid")?.Value;
            return Ok(await Mediator.Send(command));
        }

    }
}

