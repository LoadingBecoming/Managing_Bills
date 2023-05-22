using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.PhanTrang
{
    public class PageResult<T>
    {
        public Pagination Pagination { set; get; }
        public IEnumerable<T> Data { set; get; }

        public PageResult(Pagination pagination, IEnumerable<T> data)
        {
            Pagination = pagination;
            Data = data;
        }

        public static IEnumerable<T> ToPageRusult(Pagination pagination, IEnumerable<T> data)
        {
            pagination.PageNumber = pagination.PageNumber < 1 ? 1 : pagination.PageNumber;
            data = data.Skip(pagination.PageSize * (pagination.PageNumber - 1)).Take(pagination.PageSize);
            return data;
        }
    }
}
