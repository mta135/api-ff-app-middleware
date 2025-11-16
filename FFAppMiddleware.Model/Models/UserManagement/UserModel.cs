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
            public int CityId { get; set; }
            public int CityName { get; set; }
            public string Address { get; set; }
            public bool Blocked { get; set; }

            //public int WorkStatus { get; set; }
          //  public bool HasSmartphone { get; set; }
         //   public DateTime? MarriageDate { get; set; }
           // public string BirthPlace { get; set; }

            public long Account { get; set; }
            public string Currency { get; set; }
            public decimal Balance { get; set; }
            public decimal Available { get; set; }
            
            public List<Card> Cards { get; set; } = new List<Card>();

            public decimal TotalAmountSpent { get; set; }

            public List<Coupons> Coupons { get; set; } = new List<Coupons>();
    }

        public class Card
        {
            public string Id { get; set; }
            public string Number { get; set; }
            public bool Status { get; set; }
            public DateTime Created { get; set; }
            public DateTime Activated { get; set; }
            public DateTime? IssueDate { get; set; }
            public string IssueBranch { get; set; }
            public DateTime? Blocked { get; set; }
            public int Type { get; set; }
        }

        public class UserRegistrationResult
        {
            public bool Success { get; set; }
            public string Message { get; set; } = string.Empty;
            public LoyaltyUser? User { get; set; }
        }

        public class Coupons
        {
            public string Coupon { get; set; } = "TESTCOUPON2025";
            public string Description { get; set; } = "Test Coupon 2025 APLICAT LA PRODUSE BIODERMA";
            public DateTime ValidTo { get; set; } = DateTime.Now.AddMonths(1);
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
    }
}
