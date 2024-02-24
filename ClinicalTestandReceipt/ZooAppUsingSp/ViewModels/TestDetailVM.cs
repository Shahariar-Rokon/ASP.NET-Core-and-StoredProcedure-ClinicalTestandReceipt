using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using ZooAppUsingSp.Models;

namespace ZooAppUsingSp.ViewModels
{
    public class TestDetailVM
    {
        public TestDetailVM() { }
        public TestDetailVM(int testId, decimal quantity, int testUnitId, decimal unitPrice) : this()
        {
            TestId = testId;
            Quantity = quantity;
            TestUnitId = testUnitId;
            UnitPrice = unitPrice;
        }

        public TestDetailVM(int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice) : this(testId, quantity, testUnitId, unitPrice)
        {
            TotalPrice = totalPrice;
        }

        public TestDetailVM(int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice, int clientHeaderId) : this(testId, quantity, testUnitId, unitPrice, totalPrice)
        {
            ClientHeaderId = clientHeaderId;
        }
        public TestDetailVM(string testname,int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice, int clientHeaderId) : this(testId, quantity, testUnitId, unitPrice, totalPrice,clientHeaderId)
        {
            TestName = testname;
        }

        public TestDetailVM(int id, int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice, int clientHeaderId) : this(testId, quantity, testUnitId, unitPrice, totalPrice, clientHeaderId)
        {
            Id = id;
        }

        public TestDetailVM(int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice, int clientHeaderId, string testName, string testUnitName) : this(testId, quantity, testUnitId, unitPrice, totalPrice, clientHeaderId)
        {
            TestName = testName;
            TestUnitName = testUnitName;
        }

        public TestDetailVM(int id, int testId, decimal quantity, int testUnitId, decimal unitPrice, decimal totalPrice, int clientHeaderId, string productName, string measurementUnitName) : this(testId, quantity, testUnitId, unitPrice, totalPrice, clientHeaderId, productName, measurementUnitName)
        {
            Id = id;
        }

        public int Id { get; set; }

        [Required]
        public int TestId { get; set; }

        [AllowNull, Display(Name = "Tests")]
        public string TestName { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required]
        public int TestUnitId { get; set; }

        [AllowNull, Display(Name = "Test Unit")]
        public string TestUnitName { get; set; }

        [Required, Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }

        [AllowNull, Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        [Required, Display(Name = "Client Header")]
        public int ClientHeaderId { get; set; }

        [AllowNull]
        public TestUnitVM? TestUnit { get; set; }
        [AllowNull]
        public TestVM? Test { get; set; }
        [AllowNull]
        public ClientHeaderVM? ClientHeader { get; set; }
    }
}
