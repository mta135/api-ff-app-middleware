using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFAppMiddleware.Model.Models.UserManagement
{
    public class UserModel
    {
        public class LoyaltyUser
        {
            public Guid Id { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
            public DateTime Created { get; set; }
            public DateTime BirthDay { get; set; }
            public int Gender { get; set; }
            public bool SmsNotify { get; set; }
            public bool EmailNotify { get; set; }
            public int RegionId { get; set; }
            public string RegionName { get; set; }
            //public int CityId { get; set; }
           // public int CityName { get; set; }
            public string Address { get; set; }
           // public bool Blocked { get; set; }  
         //   public long Account { get; set; }
          //  public string Currency { get; set; }
            public List<Balance> Balance { get; set; }
            public decimal Available { get; set; }            
            public List<Card> Cards { get; set; } = new List<Card>();
            public decimal TotalAmountSpent { get; set; }
            public List<Coupons> Coupons { get; set; } = new List<Coupons>();
    }

        public class LoyaltyUserEdit
        {
            public Guid Id { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
           // public DateTime Created { get; set; }
            public DateTime BirthDay { get; set; }
            public int Gender { get; set; }
            public bool SmsNotify { get; set; }
            public bool EmailNotify { get; set; }
            public int RegionId { get; set; }
            public string RegionName { get; set; }
           // public int CityId { get; set; }
           // public int CityName { get; set; }
            public string Address { get; set; }
          //  public bool Blocked { get; set; }
         //   public long Account { get; set; }
          //  public string Currency { get; set; }
          //  public List<Balance> Balance { get; set; }
           // public decimal Available { get; set; }
          //  public List<Card> Cards { get; set; } = new List<Card>();
          //  public decimal TotalAmountSpent { get; set; }
          //  public List<Coupons> Coupons { get; set; } = new List<Coupons>();
        }

        public class Balance
        {
            public decimal CurrentBalance { get; set; }
            public decimal UsedBalance { get; set; }
            public decimal TotalBalance { get; set; }
        }
        public class Card
        {
            public long Id { get; set; }
            public string Number { get; set; }
            //public bool Status { get; set; }
           // public DateTime Created { get; set; }
          //  public DateTime Activated { get; set; }
          //  public DateTime? IssueDate { get; set; }
            //public string IssueBranch { get; set; }
            //public DateTime? Blocked { get; set; }
            //public int Type { get; set; }
            public bool IsDefault { get; set; }
            public bool IsActive { get; set; }
        }
        public class CardEdit
        {
            public long Id { get; set; }
          
            //public bool Status { get; set; }
            // public DateTime Created { get; set; }
            //  public DateTime Activated { get; set; }
            //  public DateTime? IssueDate { get; set; }
            //public string IssueBranch { get; set; }
            //public DateTime? Blocked { get; set; }
            //public int Type { get; set; }
            public bool IsDefault { get; set; }
            public bool IsActive { get; set; }
        }

        public class UserRegistrationResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public LoyaltyUserEdit? User { get; set; }
        }

        public class Coupons
        {
            public string Coupon { get; set; } 
            public string Description { get; set; } 
            public DateTime ValidTo { get; set; } 
            public int MaxUsageLimit { get; set; }
            public int UsageCount { get; set; }
            public string RemunerationType { get; set; }
            public string Status { get; set; }

        }
        public class Localites
        {
            public int Id { get; set; }
            public string Name { get; set; } 
            
        }
        public class GenderTypes
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }
        

        public class UserTransaction
        {
            public Guid TransactionId { get; set; }
            public string TransactionNumber { get; set; }  
            public DateTime TransactionDate { get; set; }  
            public int StatusId { get; set; }
            public  string StatusName { get; set; }
            public decimal TotalAmount { get; set; }       
            public decimal DiscountAmount { get; set; }
            public decimal PointsUsed { get; set; }        
            public decimal PointsReceived { get; set; }              
            public string StoreName { get; set; }          
            public long StoreId { get; set; }         

            public List<ProductDetail> Products { get; set; } = new(); 
        }

        public class ProductDetail
        {
            public long ProductId { get; set; }            
            public string ProductName { get; set; }   
            public decimal UnitPrice { get; set; }         
            public decimal Quantity { get; set; }     
            public decimal Discount { get; set; }          
            public decimal TotalPrice => (UnitPrice * Quantity) - Discount; 
        }


        public class UserRequest
        {
            public Guid UserId { get; set; }
            public string LanguageKey { get; set; } = "";
        }

        public class PhoneNumberRequest
        {
            public string PhoneNumber { get; set; }
        }
        public class DiscountCardNumberRequest
        {
            public string DiscountCardNumber { get; set; }
        }

    }
}
