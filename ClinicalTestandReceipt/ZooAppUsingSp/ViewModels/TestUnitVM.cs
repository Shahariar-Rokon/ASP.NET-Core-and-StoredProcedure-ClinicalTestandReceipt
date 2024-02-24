using System.ComponentModel.DataAnnotations.Schema;

namespace ZooAppUsingSp.ViewModels
{
    [NotMapped]
    public class TestUnitVM
    {

        public TestUnitVM() { }

        public TestUnitVM(string name) : this()
        {
            Name = name;
        }

        public TestUnitVM(int id, string name) : this(name)
        {
            Id = id;
        }

        public int Id { get; set; } = 0;

        public string Name { get; set; }
    }
}
