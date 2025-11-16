using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Azure.Core.HttpHeader;

namespace FFAppMiddleware.Model.Models.SaleChart
{
  public  class PreCheckModel
    {
        public class Cart
        {
            public Guid CartId { get; set; }
            public Guid ClientId { get; set; }
            public long CardId { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public List<CartItem> Items { get; set; } = new();
            public string Message { get; set; }
        }

        public class CartItem
        {
            [JsonIgnore]
            public Guid CartId { get; set; }
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
            public decimal Discount { get; set; }
            public long? PromotionId { get; set; }
            public int PointsEarned { get; set; }
        }
        //public class ResponsePreCheck
        //{
        //    public bool success { get; set; }
        //    public int status { get; set; }
        //    public PreCheckData data { get; set; }
        //}
        //public class PreCheckData
        //{
        //    public PreCheck pre_check { get; set; }
        //    public List<string> field { get; set; }
        //    public string currency { get; set; }
        //    public string send_channel { get; set; }
        //    public string sms_id { get; set; }


        //    public string message { get; set; }
        //    // Add other properties as needed
        //}
        //public class PreCheck
        //{

        //    public string PreCheckId { get; set; }
        //    public Payment Payment { get; set; }
        //    public string BranchId { get; set; }
        //    public string OperatorId { get; set; }
        //    public string SessionId { get; set; }
        //    public string TerminalId { get; set; }
        //    public List<Coupon> Coupon { get; set; }
        //    public decimal ReceiptAmount { get; set; }
        //    public decimal PaymentBonus { get; set; }
        //    public object BaseBonus { get; set; }
        //    public string ReceiptDescription { get; set; }
        //    public decimal BirthdayBonus { get; set; }
        //    public decimal UserStatusFixBonus { get; set; }


        //    public List<ReceiptDetail> ReceiptDetails { get; set; }
        //    public string Currency { get; set; }
        //    public object SendChannel { get; set; }
        //    public object SmsId { get; set; }
        //    public decimal MaxPaymentBonusCheck { get; set; }
        //    public decimal MaxPaymentMoneyCheck { get; set; }
        //    public decimal BalanceAvailable { get; set; }

        //}
        //public class ReceiptDetail
        //{
        //   public int Position { get; set; }
        //   public string ProdCode { get; set; }
        //   public decimal ProdPrice { get; set; }
        //   public int ProdAmount { get; set; }
        //   public decimal ProdSum { get; set; } 
        //   public object BonusRestrict { get; set; }
        //   public string BonusAccrualRestrict { get; set; }
        //   public object DiscountRestrict { get; set; }
        //   public decimal ExternalDiscount { get; set; }
        //   public object DiscountApplied { get; set; }
        //   public string ProdName { get; set; }
        //   public string ProdCat { get; set; } 
        //   public decimal DiscountLimit { get; set; }
        //   public decimal Discount { get; set; }
        //   public List<DiscountSuccess> DiscountSuccess { get; set; } 
        //   public decimal DiscountBonus { get; set; }
        //   public decimal Bonus { get; set; }
        //   public List<object> BonusSuccess { get; set; }
        //}
        //public class Payment
        //{
        //    public decimal Money { get; set; }
        //    public decimal BonusRedeemed { get; set; }
        //    public decimal BonusHold { get; set; }
        //    public decimal Discount { get; set; }
        //    public decimal DiscountBonus { get; set; }
        //}
        //public class DiscountSuccess
        //{
        //    public int PromotionId { get; set; }

        //    public string PromotionTitle { get; set; }

        //    public string Rule { get; set; }

        //    public string Info { get; set; }
        //    public decimal Discount { get; set; }
        //    public int Priority { get; set; }
        //}
        //public class Coupon
        //{
        //    public string Code { get; set; }
        //    public bool Success { get; set; }
        //    public string Message { get; set; }
        //}



    }
}
