using System;
using AutoMapper;
using SCIRS.Dtos;
using SCIRS.Models;

namespace SCIRS.Profiles;

public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.X))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => 
                    src.AssignedTo != null ? src.AssignedTo.UserName : null));
                
        }
    }
