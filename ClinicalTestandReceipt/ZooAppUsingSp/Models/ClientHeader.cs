using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.Models
{
    public class ClientHeader
    {
        public ClientHeader() { }
        public ClientHeader(string receiptNumber, DateTime testDate, decimal totalBill, string clientName = "", string clientPhoneNumber = "", string clientEmailAddress = "") : this()
        {
            ReceiptNumber = receiptNumber;
            TestDate = testDate;
            TotalBill = totalBill;
            ClientName = clientName;
            ClientPhoneNumber = clientPhoneNumber;
            ClientEmailAddress = clientEmailAddress;
        }

        public ClientHeader(int id, string invoiceNumber, DateTime purchaseDate, decimal totalBill, string customerName = "", string customerPhoneNumber = "", string customerEmailAddress = "") : this(invoiceNumber, purchaseDate, totalBill, customerName, customerPhoneNumber, customerEmailAddress)
        {
            Id = id;
        }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [AllowNull]
        public string? ClientName { get; set; }

        [AllowNull]
        public string? ClientPhoneNumber { get; set; }

        [AllowNull]
        public string? ClientEmailAddress { get; set; }

        [Required]
        public string ReceiptNumber { get; set; }

        [Required, DataType(DataType.Date), Column(TypeName = "DATE")]
        public DateTime TestDate { get; set; }

        [Required]
        public decimal TotalBill { get; set; }

        public ICollection<TestDetail> testDetails { get; set; } = new List<TestDetail>();
    }
}
