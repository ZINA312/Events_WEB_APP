using AutoMapper;
using Events_WEB_APP.Persistence.Contracts.Category;
using Events_WEB_APP.Core.Entities;

namespace Events_WEB_APP.API.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<Category, CategoryResponse>();
        }
    }
}
