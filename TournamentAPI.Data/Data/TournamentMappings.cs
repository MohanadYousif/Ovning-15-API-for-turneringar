using AutoMapper;
using TournamentAPI.Core.Dtos;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings()
        {
            CreateMap<Tournament, TournamentDto>()
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.StartDate.AddMonths(3)));

            CreateMap<Game, GameDto>();

            CreateMap<TournamentDto, Tournament>()
                .ForMember(dest => dest.Games, opt => opt.Ignore());

            CreateMap<GameDto, Game>();
        }
    }
}
