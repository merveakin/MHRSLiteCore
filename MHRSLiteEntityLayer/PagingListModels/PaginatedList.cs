using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHRSLiteEntityLayer.PagingListModels
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public List<T> ItemList { get; set; }

        public PaginatedList(List<T> items, int count, int pageindex, int pageSize)
        {
            PageIndex = pageindex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            ItemList = items;
        }
        public bool PreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool NextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
        public static PaginatedList<T> CreateAsync(List<T> sourcelist, int pageindex, int pageSize)
        {
            var count = sourcelist.Count();
            //Bulunduğu sayfadan bir azaltıp sayfada kaç eleman olacaksa o kadar veriyi atlar.
            var items = sourcelist.Skip((pageindex - 1) * pageSize)
                //sayfada kaç eleman olmasını istiyorsak o kadarını alır.
                .Take(pageSize)
                //listele
                .ToList();
            //oluşan listeyi yeni nesne yaratarak gönderir.
            return new PaginatedList<T>(items, count, pageindex, pageSize);
        }

    }
}
