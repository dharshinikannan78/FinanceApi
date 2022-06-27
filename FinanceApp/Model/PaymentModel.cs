using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Model
{
    public class PaymentModel
    {
        [Key]
        public int PaymentId { get; set; }
        [ForeignKey("ProductCustomerId")]
        public int ProductCustomerId { get; set; }
          public string PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaidAmount { get; set; }
        public string CollectedBy { get; set; }
        
    }
}
