using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.Models
{
    public class TestUnit
    {
        public TestUnit() { }

        public TestUnit(string name) : this()
        {
            Name = name;
        }

        public TestUnit(int id, string name) : this(name)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [AllowNull]
        public ICollection<TestDetail>? TestDetails { get; set; }
    }
}
