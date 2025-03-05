using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.Category;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    /// <summary>
    /// Профиль для маппинга категорий.
    /// </summary>
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="CategoryProfile"/>.
        /// </summary>
        public CategoryProfile()
        {
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
