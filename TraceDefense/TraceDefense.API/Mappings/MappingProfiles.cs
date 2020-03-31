using AutoMapper;
using System.Collections.Generic;
using TraceDefense.API.Models.Protos;
using TraceDefense.Entities.Interactions;

namespace TraceDefense.API.Mappings
{
    /// <summary>
    /// Mapping profiles used to translate Protos to internal Entity classes
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Creates a new <see cref="MappingProfiles"/> instance
        /// </summary>
        public MappingProfiles()
        {
            // Location
            CreateMap<Models.Protos.Location, Entities.Interactions.Location>()
                .ForMember(
                    l => l.Latitude,
                    opts => opts.MapFrom(l => l.Lattitude)
                )
                .ForMember(
                    l => l.Radius,
                    opts => opts.MapFrom(l => l.RadiusMeters)
                )
                .ReverseMap();

            // LocationTime
            CreateMap<Models.Protos.LocationTime, Entities.Interactions.LocationTime>()
                .ReverseMap();

            // GeoProximity -> GeoProximityMatch
            CreateMap<GeoProximity, GeoProximityMatch>()
                .ForMember(
                    gm => gm.DurationTolerance,
                    opts => opts.MapFrom(g => g.DurationToleranceSecs)
                )
                .ForMember(
                    gm => gm.ProximityRadius,
                    opts => opts.MapFrom(g => g.ProximityRadiusMeters)
                )
                .ReverseMap();

            // IDMatch -> BluetoothIdMatch
            CreateMap<IDMatch, BluetoothIdMatch>()
                .ReverseMap();

            // QueryInfo
            CreateMap<Models.Protos.QueryInfo, Entities.Interactions.QueryInfo>()
                .ForMember(
                    qi => qi.Timestamp,
                    opts => opts.MapFrom(q => q.QueryTimestamp)
                )
                .ReverseMap();

            // ProximityQuery -> Query
            CreateMap<ProximityQuery, Query>()
                .ForMember(
                    q => q.GeoProximities,
                    opts => opts.MapFrom(p => p.GeoProximity)
                )
                .ReverseMap();
        }
    }
}