using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using AutoMapper;
using TournamentAPI.Core.Dtos;
using TournamentAPI.Data.Repositories;
using Microsoft.AspNetCore.JsonPatch;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public GamesController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games = await uow.gameRepository.GetAllAsync();
            var gameDtos = mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gameDtos);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await uow.gameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            var gameDto = mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            if (id != gameDto.Id)
            {
                return BadRequest();
            }

            var game = mapper.Map<Game>(gameDto);
            uow.gameRepository.Update(game);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto gameDto)
        {
            var game = mapper.Map<Game>(gameDto);
            uow.gameRepository.Add(game);
            await uow.CompleteAsync();

            var createdGameDto = mapper.Map<GameDto>(game);

            return CreatedAtAction(nameof(GetGame), new { id = createdGameDto.Id }, createdGameDto);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await uow.gameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            uow.gameRepository.Remove(game);
            await uow.CompleteAsync();

            return NoContent();
        }

        [HttpPatch("{gameId}")]
        public async Task<ActionResult<GameDto>> PatchGame(int gameId, [FromBody] JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var game = await uow.gameRepository.GetAsync(gameId);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = mapper.Map<GameDto>(game);
            patchDocument.ApplyTo(gameDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(gameDto, game);
            uow.gameRepository.Update(game);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(gameId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(mapper.Map<GameDto>(game));
        }


        private bool GameExists(int id)
        {
            return uow.gameRepository.AnyAsync(id).Result;
        }
    }
}
