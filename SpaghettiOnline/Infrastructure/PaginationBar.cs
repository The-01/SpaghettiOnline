using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaghettiOnline.Infrastructure
{
    public class PaginationBar
    {
        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }


        public PaginationBar()
        {

        }

        public PaginationBar(int totalItems, int page, int pageSize = 6)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int currentPage = page;

            int startPage = currentPage - 3;
            int endPage = currentPage + 3;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;

                if (endPage > 6)
                {
                    startPage = endPage - 6;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
