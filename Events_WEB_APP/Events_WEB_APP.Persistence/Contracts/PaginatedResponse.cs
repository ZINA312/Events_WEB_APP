using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_WEB_APP.Persistence.Contracts
{
    public record PaginatedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
}
