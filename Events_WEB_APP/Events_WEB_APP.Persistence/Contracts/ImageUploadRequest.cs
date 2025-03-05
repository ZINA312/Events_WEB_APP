using Microsoft.AspNetCore.Http;

namespace Events_WEB_APP.Persistence.Contracts
{
    /// <summary>
    /// Представляет запрос на загрузку изображения.
    /// </summary>
    /// <param name="File">Файл изображения, который необходимо загрузить.</param>
    public record ImageUploadRequest(IFormFile File);
}
