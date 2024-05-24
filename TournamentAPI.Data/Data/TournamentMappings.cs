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

            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.startdatum, opt => opt.MapFrom(src => src.Time));

            CreateMap<TournamentDto, Tournament>()
                .ForMember(dest => dest.Games, opt => opt.Ignore());

            CreateMap<GameDto, Game>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.startdatum))
                .ForMember(dest => dest.TournamentId, opt => opt.Ignore())
                .ForMember(dest => dest.Tournament, opt => opt.Ignore());
        }
    }
}
