namespace APICatalago.Pagination
{
    public class Parameters
    {
        const int maxPageSize = 1000;
        private int _pageSize = 100;
        public int PageNumber { get; set; } = 1;

        public int PageSize { get { return _pageSize; } set { _pageSize = (value > maxPageSize) ? maxPageSize: value; } }
    }
}
