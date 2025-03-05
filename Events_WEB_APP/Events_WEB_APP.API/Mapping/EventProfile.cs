using AutoMapper;
using Events_WEB_APP.Core.Entities;
using Events_WEB_APP.Persistence.Contracts.Event;

namespace Events_WEB_APP.API.Mapping
{
    /// <summary>
    /// Профиль для маппинга событий.
    /// </summary>
    public class EventProfile : Profile
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="EventProfile"/>.
        /// </summary>
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
