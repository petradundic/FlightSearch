using AutoMapper;
using FlightSearch.Server.Dtos;
using FlightSearch.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlightSearch.Models.Mapping
{
    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile()
        {
            CreateMap<FlightDto, Flight>()
         .ForMember(dest => dest.OutboundStops, opt => opt.MapFrom(src => src.OutboundStops))
         .ForMember(dest => dest.ReturnStops, opt => opt.MapFrom(src => src.ReturnStops))
         .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
         .ForMember(dest => dest.Id, opt => opt.Ignore())
         .ForMember(dest => dest.SearchParameterId, opt => opt.Ignore())
         .ForMember(dest => dest.Origin, opt => opt.Ignore())
         .ForMember(dest => dest.Destination, opt => opt.Ignore())
         .ForMember(dest => dest.OutboundDepartureTime, opt => opt.MapFrom(src => src.OutboundDepartureTime))
         .ForMember(dest => dest.ReturnArrivalTime, opt => opt.MapFrom(src => src.ReturnArrivalTime))
         .ForMember(dest => dest.OutboundArrivalTime, opt => opt.MapFrom(src => src.OutboundArrivalTime))
         .ForMember(dest => dest.ReturnDepartureTime, opt => opt.MapFrom(src => src.ReturnDepartureTime))
         .ForMember(dest => dest.Passengers, opt => opt.Ignore())
         .ForMember(dest => dest.Currency, opt => opt.Ignore());
        }
    }
}