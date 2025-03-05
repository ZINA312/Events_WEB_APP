namespace Events_WEB_APP.Persistence.Contracts
{
    /// <summary>
    /// Представляет пагинированный ответ с элементами и информацией о пагинации.
    /// </summary>
    /// <typeparam name="T">Тип элементов в списке.</typeparam>
    /// <param name="Items">Список элементов текущей страницы.</param>
    /// <param name="PageNumber">Номер текущей страницы.</param>
    /// <param name="PageSize">Количество элементов на странице.</param>
    /// <param name="TotalCount">Общее количество элементов.</param>
    public record PaginatedResponse<T>(
    List<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount);
}
