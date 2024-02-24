using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.ViewModels
{
    public class ClientHeaderVM
    {
        public ClientHeaderVM() { }
        public ClientHeaderVM(string clientName, string clientPhoneNumber, string clientEmailAddress, string receiptNumber, DateTime testDate, decimal totalAmount) : this()
        {
            ClientName = clientName;
            ClientPhoneNumber = clientPhoneNumber;
            ClientEmailAddress = clientEmailAddress;
            ReceiptNumber = receiptNumber;
            TestDate = testDate;
            TotalAmount = totalAmount;
        }

        public ClientHeaderVM(int id, string clientName, string clientPhoneNumber, string clientEmailAddress, string receiptNumber, DateTime testDate, decimal totalAmount) : this(clientName, clientPhoneNumber, clientEmailAddress, receiptNumber, testDate, totalAmount)
        {
            Id = id;
        }

        public int Id { get; set; }

        [AllowNull, Display(Name = "Client Name")]
        public string ClientName { get; set; } = string.Empty;

        [AllowNull, DataType(DataType.PhoneNumber), Display(Name = "Client Phone Number")]
        public string ClientPhoneNumber { get; set; }

        [AllowNull, DataType(DataType.EmailAddress), Display(Name = "Client Email Address")]
        public string ClientEmailAddress { get; set; }

        [AllowNull, Display(Name = "Receipt Number")]
        public string ReceiptNumber { get; set; }

        [AllowNull, DataType(DataType.Date), Column(TypeName = "DATE"), Display(Name = "Test Date")]
        public DateTime TestDate { get; set; }

        [AllowNull, Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [AllowNull]
        public ICollection<TestDetailVM> TestDetails { get; set; } = new List<TestDetailVM>();
    }
}
