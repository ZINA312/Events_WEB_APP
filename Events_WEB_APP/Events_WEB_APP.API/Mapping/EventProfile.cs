using AutoMapper;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Event;

namespace Events_WEB_APP.API.Mapping
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventCreateRequest, Event>()
                .ForMember(dest => dest.ImagePath, opt => opt.Ignore());

            CreateMap<EventUpdateRequest, Event>();

            CreateMap<Event, EventResponse>()
                .ForMember(dest => dest.ImageUrl,
                    opt => opt.MapFrom(src => $"/uploads/{src.ImagePath}"));
        }
    }
}
