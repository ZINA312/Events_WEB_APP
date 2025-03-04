using AutoMapper;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Participant;

namespace Events_WEB_APP.API.Mapping
{
    public class ParticipantProfile : Profile
    {
        public ParticipantProfile()
        {
            CreateMap<ParticipantCreateRequest, Participant>()
                .ForMember(dest => dest.DateTimeOfRegistration, opt => opt.MapFrom(src => DateTime.UtcNow)); ;
            CreateMap<Participant, ParticipantResponse>();
        }
    }
}
