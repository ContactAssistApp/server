using System.Collections.Generic;
using System.Linq;

using AutoMapper;
using CovidSafe.API.v20200505.Protos;
using CovidSafe.Entities.Geospatial;
using CovidSafe.Entities.Messages;

namespace CovidSafe.API.v20200505
{
    /// <summary>
    /// Maps proto types to their internal database representations
    /// </summary>
    public class MappingProfiles : Profile
    {
        /// <summary>
        /// Creates a new <see cref="MappingProfiles"/> instance
        /// </summary>
        public MappingProfiles()
        {
            // Location -> Coordinates
            CreateMap<Location, Coordinates>()
                // Properties have the same name+type
                .ReverseMap();

            // Region -> Region
            CreateMap<Protos.Region, Entities.Geospatial.Region>()
                // Properties have the same name+type
                .ReverseMap();

            // MessageInfo -> InfectionReportMetadata
            CreateMap<MessageInfo, MessageContainerMetadata>()
                .ForMember(
                    im => im.Id,
                    op => op.MapFrom(mi => mi.MessageId)
                )
                .ForMember(
                    im => im.Timestamp,
                    op => op.MapFrom(mi => mi.MessageTimestamp)
                )
                .ReverseMap();

            // IEnumerable<InfectionReportMetadata> -> MessageListResponse
            // This is a one-way response so no ReverseMap is necessary
            CreateMap<IEnumerable<MessageContainerMetadata>, MessageListResponse>()
                .ForMember(
                    mr => mr.MessageInfoes,
                    op => op.MapFrom(im => im)
                )
                .ForMember(
                    mr => mr.MaxResponseTimestamp,
                    op => op.MapFrom(im => im.Count() > 0 ? im.Max(o => o.Timestamp) : 0)
                );

            // Area -> InfectionArea
            CreateMap<Area, NarrowcastArea>()
                .ForMember(
                    ia => ia.BeginTimestamp,
                    op => op.MapFrom(a => a.BeginTime)
                )
                .ForMember(
                    ia => ia.EndTimestamp,
                    op => op.MapFrom(a => a.EndTime)
                )
                .ForMember(
                    ia => ia.Location,
                    op => op.MapFrom(a => a.Location)
                )
                .ForMember(
                    ia => ia.RadiusMeters,
                    op => op.MapFrom(a => a.RadiusMeters)
                )
                .ReverseMap();

            // BlueToothSeed -> BluetoothSeedMessage
            CreateMap<BlueToothSeed, BluetoothSeedMessage>()
                .ForMember(
                    bs => bs.BeginTimestamp,
                    op => op.MapFrom(s => s.SequenceStartTime)
                )
                .ForMember(
                    bs => bs.EndTimestamp,
                    op => op.MapFrom(s => s.SequenceEndTime)
                )
                .ForMember(
                    bs => bs.Seed,
                    op => op.MapFrom(s => s.Seed)
                )
                .ReverseMap();

            // AreaMatch -> AreaReport
            CreateMap<AreaMatch, NarrowcastMessage>()
                .ForMember(
                    ar => ar.Areas,
                    op => op.MapFrom(am => am.Areas)
                )
                .ForMember(
                    ar => ar.UserMessage,
                    op => op.MapFrom(am => am.UserMessage)
                )
                .ReverseMap();

            // SelfReportRequest -> InfectionReport
            // This is only a request object so no ReverseMap is necessary
            CreateMap<SelfReportRequest, MessageContainer>()
                .ForMember(
                    ir => ir.BluetoothSeeds,
                    op => op.MapFrom(sr => sr.Seeds)
                )
                // Currently no NarrowcastMessages in a SelfReportRequest
                .ForMember(
                    ir => ir.Narrowcasts,
                    op => op.Ignore()
                )
                // Not specified by users
                .ForMember(
                    ir => ir.BluetoothMatchMessage,
                    op => op.Ignore()
                )
                // Not specified by users
                .ForMember(
                    ir => ir.BooleanExpression,
                    op => op.Ignore()
                );

            // MatchMessage -> InfectionReport
            CreateMap<MatchMessage, MessageContainer>()
                .ForMember(
                    ir => ir.Narrowcasts,
                    op => op.MapFrom(mm => mm.AreaMatches)
                )
                .ForMember(
                    ir => ir.BluetoothSeeds,
                    op => op.MapFrom(mm => mm.BluetoothSeeds)
                )
                .ForMember(
                    ir => ir.BooleanExpression,
                    op => op.MapFrom(mm => mm.BoolExpression)
                )
                .ForMember(
                    // Not supported in v20200415
                    ir => ir.BluetoothMatchMessage,
                    op => op.Ignore()
                )
                // Other properties have the same name+type
                .ReverseMap();
        }
    }
}
