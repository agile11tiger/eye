using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace EyE.Client.Components
{
    public partial class Pagination
    {
        [Parameter] public PaginationViewModel Model { get; set; }
        [Parameter] public EventCallback UpdateState { get; set; }

        public async Task GoToPage(int pageNumber)
        {
            if (pageNumber <= Model.TotalPages)
            {
                Model.PageNumber = pageNumber;
                await UpdateState.InvokeAsync(null);
            }
        }

        public async Task GetFirstPage()
        {
            if (Model.HasPreviousPages)
            {
                Model.PaginationFirstElementNumber = 1;
                await GoToPage(Model.PaginationFirstElementNumber);
            }
        }

        public async Task GetLastPage()
        {
            if (Model.HasNextPages)
            {
                Model.PaginationFirstElementNumber = Model.TotalPages - Model.PaginationSize;
                await GoToPage(Model.PaginationLastElementNumber);
            }
        }

        public async Task GetPreviousPages()
        {
            if (Model.HasPreviousPages)
            {
                Model.PaginationFirstElementNumber -= Model.PaginationSize;
                await GoToPage(Model.PaginationLastElementNumber);
            }
        }

        public async Task GetNextPages()
        {
            if (Model.HasNextPages)
            {
                Model.PaginationFirstElementNumber += Model.PaginationSize;
                await GoToPage(Model.PaginationFirstElementNumber);
            }
        }
    }

    public class PaginationViewModel
    {
        //Пагинация в данном контексте это панелька для перехода по страницам
        //Текущая пагинация - это например от 1 до 5, а следующая будет от 6 до 10.

        /// <summary>
        /// Всего элементов на всех страницах
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Разрешенное количество элементов на странице
        /// </summary>
        public int PageSize { get; set; } = 30;
        /// <summary>
        /// Текущий номер страницы
        /// </summary>
        public int PageNumber { get; set; } = 1;
        /// <summary>
        /// Количество страниц у пагинации
        /// </summary>
        public int PaginationSize { get; set; } = 10;
        /// <summary>
        /// Номер первого элемента ТЕКУЩЕЙ пагинации(увеличиваеться или уменьшаеться взависимости от PaginationSize)
        /// </summary>
        public int PaginationFirstElementNumber { get; set; } = 1;

        public int PageCountStart { get => 1; }
        public int PaginationCountStart { get => 1; }
        /// <summary>
        /// Номер последнего элемента ТЕКУЩЕЙ пагинации
        /// </summary>
        public int PaginationLastElementNumber { get => PaginationSize + PaginationFirstElementNumber - PaginationCountStart; }
        public int TotalPages { get => (int)Math.Ceiling(Count / (double)PageSize); }
        /// <summary>
        /// Номер первого элемента ТЕКУЩЕЙ страницы
        /// </summary>
        public int PageFirstElementNumber { get => (PageNumber - PaginationCountStart) * PageSize + PageCountStart; }
        /// <summary>
        /// Номер последнего элемента ТЕКУЩЕЙ страницы
        /// </summary>
        public int PageLastElementNumber { get => (PageNumber - PaginationCountStart) * PageSize + PageSize; }
        public bool HasPreviousPages { get => PaginationFirstElementNumber > PaginationCountStart; }
        public bool HasNextPages { get => PaginationFirstElementNumber + PaginationSize <= TotalPages; }
    }
}
