using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class PaginationDto
    {
        private readonly int MaxPageSize = 50;
        public int pageSize = 5;
        public int CurrentPage { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = (value > MaxPageSize) ? MaxPageSize : value;
            }
        }
    }
}
