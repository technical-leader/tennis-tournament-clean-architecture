using AutoMapper;
using TennisTournament.Application.DTOs;
using TennisTournament.Domain.Entities;
using TennisTournament.Domain.Enums;
using System.Linq;

namespace TennisTournament.Application.Mappings
{
    /// <summary>
    /// Perfil de AutoMapper para configurar los mapeos entre entidades y DTOs.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Constructor que configura todos los mapeos.
        /// </summary>
        public MappingProfile()
        {
            // Mapeos de Player
            CreateMap<MalePlayer, MalePlayerDto>()
                .ForMember(dest => dest.PlayerType, opt => opt.MapFrom(src => PlayerType.Male));
            CreateMap<MalePlayerDto, MalePlayer>();
            CreateMap<FemalePlayer, FemalePlayerDto>()
                .ForMember(dest => dest.PlayerType, opt => opt.MapFrom(src => PlayerType.Female));
            CreateMap<FemalePlayerDto, FemalePlayer>();

            // Mapeo de Match
            CreateMap<Match, MatchDto>()
                .ForMember(dest => dest.Player1Id, opt => opt.MapFrom(src => src.Player1.Id))
                .ForMember(dest => dest.Player1Display, opt => opt.MapFrom(src => src.Player1.Name))
                .ForMember(dest => dest.Player2Id, opt => opt.MapFrom(src => src.Player2.Id))
                .ForMember(dest => dest.Player2Display, opt => opt.MapFrom(src => src.Player2.Name))
                .ForMember(dest => dest.WinnerId, opt => opt.MapFrom(src => src.Winner != null ? src.Winner.Id : (Guid?)null))
                .ForMember(dest => dest.WinnerDisplay, opt => opt.MapFrom(src => src.Winner != null ? src.Winner.Name : string.Empty))
                .AfterMap((src, dest) =>
                {
                    if (src.Winner == null)
                        throw new InvalidOperationException($"El partido con Id {src.Id} no tiene un ganador asignado tras la simulación.");
                });

            // Mapeo de Tournament
            CreateMap<Tournament, TournamentDto>()
                .ForMember(dest => dest.PlayerIds, opt => opt.MapFrom(src => src.Players.Select(p => p.Id).ToList()))
                .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Players.Select(p => p.Name).ToList()));

            // Mapeo de TournamentShortDto para el listado general (sin Matches)
            CreateMap<Tournament, TournamentShortDto>()
                .ForMember(dest => dest.PlayerIds, opt => opt.MapFrom(src => src.Players.Select(p => p.Id).ToList()))
                .ForMember(dest => dest.PlayerNames, opt => opt.MapFrom(src => src.Players.Select(p => p.Name).ToList()));

            // Mapeo de Result
            CreateMap<Result, ResultDto>()
                .ForMember(dest => dest.TournamentType, opt => opt.MapFrom(src => src.Tournament.Type))
                .ForMember(dest => dest.WinnerId, opt => opt.MapFrom(src => src.Winner.Id))
                .ForMember(dest => dest.WinnerDisplay, opt => opt.MapFrom(src => src.Winner != null && !string.IsNullOrEmpty(src.Winner.Name) ? src.Winner.Name : string.Empty));
        }
    }
}