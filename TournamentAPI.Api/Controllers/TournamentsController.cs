using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Core.Dtos;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.IRepositories;
using TournamentAPI.Data.Repositories;

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
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
        {
            var tournaments = await uow.tournamentRepository.GetAllAsync();
            var tournamentDtos = mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentDtos);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
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
            if (id != tournamentDto.Id)
            {
                return BadRequest();
            }

            var tournament = mapper.Map<Tournament>(tournamentDto);
            uow.tournamentRepository.Update(tournament);

            try
            {
                await uow.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(Tournament tournamentDto)
        {
            var tournament = mapper.Map<Tournament>(tournamentDto);
            uow.tournamentRepository.Add(tournament);
            await uow.CompleteAsync();

            var createdTournamentDto = mapper.Map<TournamentDto>(tournament);

            return CreatedAtAction(nameof(GetTournament), new { id = createdTournamentDto.Id }, createdTournamentDto);
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
            await uow.CompleteAsync();

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
                if (!TournamentExists(tournamentId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(mapper.Map<TournamentDto>(tournament));
        }

        private bool TournamentExists(int id)
        {
            return uow.tournamentRepository.AnyAsync(id).Result;
        }
    }
}
