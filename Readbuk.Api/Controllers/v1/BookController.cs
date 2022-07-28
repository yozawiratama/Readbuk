using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Features.Books.Commands;
using Application.Features.Books.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Readbuk.Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class BookController : BaseApiController
    {
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            command.SignedInUserId = User.FindFirst("uid")?.Value;
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAllBooksQuery()));
        }

        [HttpGet("title/{search}")]
        public async Task<IActionResult> GetByTitle(string search)
        {
            return Ok(await Mediator.Send(new GetAllBooksByTitleQuery { Search = search }));
        }

        [HttpGet("author/{search}")]
        public async Task<IActionResult> GetByAuthorName(string search)
        {
            return Ok(await Mediator.Send(new GetAllBooksByAuthorNameQuery { Search = search }));
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteBookByIdCommand { ID = id }));
        }

        [HttpPut("{id}/author/add"), Authorize]
        public async Task<IActionResult> AddAuthor(Guid id, AddAuthorBookCommand command)
        {
            if (id != command.BookID)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("/author/{id}"), Authorize]
        public async Task<IActionResult> RemoveAuthor(Guid id)
        {
            return Ok(await Mediator.Send(new RemoveAuthorBookByIdCommand { ID = id }));
        }

        [HttpPut("{id}/variant/add"), Authorize]
        public async Task<IActionResult> AddVariant(Guid id, AddVariantBookCommand command)
        {
            if (id != command.BookID)
            {
                return BadRequest();
            }
            command.SignedInUserId = User.FindFirst("uid")?.Value;
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("variant/{id}"), Authorize]
        public async Task<IActionResult> RemoveVariant(Guid id)
        {
            return Ok(await Mediator.Send(new RemoveVariantBookByIdCommand { ID = id }));
        }
    }
}

