namespace FFAppMiddleware.Model.Models.Products
{
    public class ProductRequest
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ProductFilter Filter { get; set; } = new ProductFilter();

        public SortItem Sort { get; set; } = new SortItem();
    }


    public class SortItem
    {       
        public string Column { get; set; }
        public string Direction { get; set; }
    }

    public class ProductFilter
    {
        public long? ProductId { get; set; }      
        public string ProductName { get; set; }  
        public string ProducerName { get; set; }  
    }

    public enum ProductSortBy
    {
        NameAsc,
        NameDesc,
        PriceAsc,
        PriceDesc
    }
}
