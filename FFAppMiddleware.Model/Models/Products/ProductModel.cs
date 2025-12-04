namespace FFAppMiddleware.Model.Models.Products
{
    public class ProductModel
    {
        public int ProductCategoryId { get; set; }

        public long ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public int Status { get; set; }

        public decimal? StockSalePrice { get; set; }

        public decimal? MaxCalculationBonus { get; set; }

        public decimal? MaxCalculationBonusUnit { get; set; }

        public decimal? MaxPaymentBonus { get; set; }

        public decimal? MaxPaymentBonusUnit { get; set; }

        public string BarCodes { get; set; }

        public string ProducerName { get; set; }

        public long? ProducerId { get; set; }

        public string ProductCategoryName { get; set; }
        public List<PromotionResponse> Promotions { get; set; } = new();


    }
    public class PromotionResponse
    {
        public long PromotionId { get; set; }

        public string PromotionName { get; set; }

        public DateTime? PromotionBeginDate { get; set; }

        public DateTime? PromotionEndDate { get; set; }

        public bool PromotionUseSupplier { get; set; }

        public bool PromotionIsValabilityTermOriented { get; set; }

        public bool IsStockAgeOriented { get; set; }

        public bool PromotionUseProducers { get; set; }

        public bool PromotionUseProductCategories { get; set; }

        public bool PromotionUseProducts { get; set; }
    }
    public class Brands
    {
        public long Id { get; set; }

        public string Name { get; set; }

      
    }

}
