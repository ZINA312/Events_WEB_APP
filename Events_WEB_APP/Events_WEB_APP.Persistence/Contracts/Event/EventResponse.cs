using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_WEB_APP.Persistence.Contracts.Event
{
    public record EventResponse(
    Guid Id,
    string Name,
    string Description,
    DateTime Date,
    string Location,
    Guid CategoryId,
    int MaxNumOfParticipants,
    string ImageUrl)
    {
        public EventResponse() : this(
            default,
            string.Empty,
            string.Empty,
            default,
            string.Empty,
            default,
            default,
            string.Empty)
        {
        }
    }
}
