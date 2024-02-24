using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.Models
{
    public class TestDetail
    {
        public TestDetail() { }

        public TestDetail(int testId, decimal quantity, int testUnitId, decimal testUnitPrice, decimal testTotalPrice, int clientHeaderId) : this()
        {
            TestId = testId;
            Quantity = quantity;
            TestUnitId = testUnitId;
            TestUnitPrice = testUnitPrice;
            TestTotalPrice = testTotalPrice;
            ClientHeaderId = clientHeaderId;
        }

        public TestDetail(int id, int productId, decimal quantity, int measurementUnitId, decimal productUnitPrice, decimal productTotalPrice, int purchaseHeaderId) : this(productId, quantity, measurementUnitId, productUnitPrice, productTotalPrice, purchaseHeaderId)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Test))]
        public int TestId { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [ForeignKey(nameof(TestUnit))]
        public int TestUnitId { get; set; }

        [Required]
        public decimal TestUnitPrice { get; set; }

        [AllowNull]
        public decimal TestTotalPrice { get; set; }

        [ForeignKey(nameof(ClientHeader))]
        public int ClientHeaderId { get; set; }

        [AllowNull]
        public TestUnit? TestUnit { get; set; }
        [AllowNull]
        public Test? Test { get; set; }
        [AllowNull]
        public ClientHeader? ClientHeader { get; set; }
    }
}
