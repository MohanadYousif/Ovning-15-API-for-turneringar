using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using AutoMapper;
using TournamentAPI.Core.Dtos;
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
        public async Task<ActionResult<IEnumerable<Game>>> GetAllGames()
        {
            var games = await uow.gameRepository.GetAllAsync();

            if (games == null)
            {
                return NotFound();
            }

            return Ok(games);
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

            return Ok(game);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            var game = mapper.Map<Game>(gameDto);
            game.Id = id;
            uow.gameRepository.Update(game);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return (!GameExists(id)) ? NotFound() : StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Games
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(GameDto gameDto)
        {
            var game = mapper.Map<Game>(gameDto);
            uow.gameRepository.Add(game);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, game);
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

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return (!GameExists(id)) ? NotFound() : StatusCode(500);
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<GameDto>> PatchGame(int id, [FromBody] JsonPatchDocument<GameDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var game = await uow.gameRepository.GetAsync(id);
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

                return (!GameExists(id)) ? NotFound() : StatusCode(500);
            }

            return Ok(mapper.Map<GameDto>(game));
        }


        private bool GameExists(int id)
        {
            return uow.gameRepository.AnyAsync(id).Result;
        }
    }
}
