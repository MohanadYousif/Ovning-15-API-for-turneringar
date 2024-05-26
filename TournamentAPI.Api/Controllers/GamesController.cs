using AutoMapper;
using TournamentAPI.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using TournamentAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using TournamentAPI.Core.IRepositories;

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
        public async Task<ActionResult<IEnumerable<GameDto>>> GetAllGames()
        {
            var games = await uow.gameRepository.GetAllAsync();

            if (games == null)
            {
                return NotFound();
            }

            var gamesDto = mapper.Map<IEnumerable<GameDto>>(games);
            return Ok(gamesDto);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await uow.gameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            GameDto gameDto = mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        // GET: api/Games/5
        [HttpGet("title")]
        public async Task<ActionResult<GameDto>> GetGameByTitle(string title)
        {
            var game = await uow.gameRepository.GetAsync(title);
            if (game == null)
            {
                return NotFound();
            }
            GameDto gameDto = mapper.Map<GameDto>(game);
            return Ok(gameDto);
        }

        // PUT: api/Games/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, GameDto gameDto)
        {
            var game = mapper.Map<Game>(gameDto);
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
        public async Task<ActionResult<GameDto>> PostGame(GameDto gameDto)
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
