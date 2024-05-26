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
    public class TournamentsController : ControllerBase
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public TournamentsController(IUoW uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includeGames)
        {
            var tournaments = await uow.tournamentRepository.GetAllAsync(includeGames);

            if (tournaments == null)
            {
                return NotFound();
            }
            var tournamentsDto = mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentsDto);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await uow.tournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }
            var tournamentDto = mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentDto tournamentDto)
        {
            var tournament = mapper.Map<Tournament>(tournamentDto);
            uow.tournamentRepository.Update(tournament);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return (!TournamentExists(id)) ? NotFound() : StatusCode(500);
            }

            return NoContent();
        }

        // POST: api/Tournaments
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(TournamentDto tournamentDto)
        {
            var tournament = mapper.Map<Tournament>(tournamentDto);

            uow.tournamentRepository.Add(tournament);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500);
            }

            var createdTournamentDto = mapper.Map<TournamentDto>(tournament);

            return CreatedAtAction(nameof(GetTournament), new {id = createdTournamentDto.Id } , createdTournamentDto);
        }

        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await uow.tournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            uow.tournamentRepository.Remove(tournament);
            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return (!TournamentExists(id)) ? NotFound() : StatusCode(500);
            }

            return NoContent();
        }

        // PATCH: api/Tournaments/5
        [HttpPatch("{tournamentId}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int tournamentId, JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var tournament = await uow.tournamentRepository.GetAsync(tournamentId);
            if (tournament == null)
            {
                return NotFound();
            }

            var tournamentDto = mapper.Map<TournamentDto>(tournament);
            patchDocument.ApplyTo(tournamentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(tournamentDto, tournament);
            uow.tournamentRepository.Update(tournament);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return (!TournamentExists(tournamentId)) ? NotFound() : StatusCode(500);
            }

            return Ok(mapper.Map<TournamentDto>(tournament));
        }

        private bool TournamentExists(int id)
        {
            return uow.tournamentRepository.AnyAsync(id).Result;
        }
    }
}
