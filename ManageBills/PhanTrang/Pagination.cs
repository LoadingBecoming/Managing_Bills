using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestWebAPI.PhanTrang
{
    public class Pagination
    {
        public int PageSize { set; get; }
        public int PageNumber { set; get; }
        public int TotalCount { set; get; }
        public int TotalPage
        {
            get
            {
                if (PageSize == 0) return 0;
                var total = TotalCount / PageSize;
                if (TotalCount % PageSize != 0)
                    total++;
                return total;

            }

        }

        public Pagination()
        {
            PageSize = -1;
            PageNumber = 1;
        }
    }
}
