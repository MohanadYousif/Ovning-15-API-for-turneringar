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
                .ForMember(dto => dto.EndDate, opt => opt.MapFrom(src => src.StartDate.AddMonths(3)))
                .ForMember(dto => dto.GamesDto, opt => opt.MapFrom(src => src.Games));

            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.Startdatum, opt => opt.MapFrom(src => src.Time));

            CreateMap<TournamentDto, Tournament>()
                .ForMember(dest => dest.Games, opt => opt.MapFrom(src => src.GamesDto));

            CreateMap<GameDto, Game>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Startdatum))
                .ForMember(dest => dest.Tournament, opt => opt.Ignore());
        }
    }
}
