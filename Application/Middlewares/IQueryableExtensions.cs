using Application.DTOs;

namespace API.Middlewares
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDto paginationDto)
        {
            return queryable.Skip((paginationDto.CurrentPage-1)*paginationDto.pageSize).Take(paginationDto.PageSize);
        }
    }
}
