using System;
using AutoMapper;
using NetTopologySuite.Geometries;
using SCIRS.Dtos;
using SCIRS.Enums;
using SCIRS.Models;

namespace SCIRS.Profiles;

public class MappingProfile : Profile
    {
 
        public MappingProfile()
        {
            CreateMap<Report, ReportDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => src.ResolvedAt))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom(src => src.CreatedById))
                .ForMember(dest => dest.AssignedToId, opt => opt.MapFrom(src => src.AssignedToId))
                
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Y))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.X))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy.UserName))
                .ForMember(dest => dest.AssignedToName, opt => opt.MapFrom(src => 
                    src.AssignedTo != null ? src.AssignedTo.UserName : null));
                
            CreateMap<CreateReportDto, Report>()
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => 
                    new Point(src.Longitude, src.Latitude) { SRID = 4326 }))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ReportStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedById, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.City, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ResolvedAt, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedToId, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore())
                .ForMember(dest => dest.StatusHistories, opt => opt.Ignore());
        }
}
    
