using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Publishers.Commands;
using Application.Features.Publishers.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Readbuk.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class PublisherController : BaseApiController
    {
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CreatePublisherCommand command)
        {
            command.SignedInUserId = User.FindFirst("uid")?.Value;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllPublishersQuery()));
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetById(Guid id)
        //{
        //    return Ok(await Mediator.Send(new GetCityByIdQuery { Id = id }));
        //}

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeletePublisherByIdCommand { ID = id }));
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> Update(Guid id, UpdatePublisherCommand command)
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

