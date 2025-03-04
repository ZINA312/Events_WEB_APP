using Microsoft.AspNetCore.Http;

namespace Events_WEB_APP.Persistence.Contracts
{
    public record ImageUploadRequest(IFormFile File);
}
