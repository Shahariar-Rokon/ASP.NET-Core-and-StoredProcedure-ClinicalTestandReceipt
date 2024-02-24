using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.ViewModels
{
    public class TestVM
    {
        public TestVM() { }
        public TestVM(string name) : this()
        {
            Name = name;
        }

        public TestVM(string name, string imageUrl) : this(name)
        {
            ImageUrl = imageUrl;
        }

        public TestVM(int id, string name, string imageUrl) : this(name, imageUrl)
        {
            Id = id;
        }

        public int Id { get; set; } = 0;

        [Required]
        public string Name { get; set; } = string.Empty;

        [AllowNull, DataType(DataType.ImageUrl), Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }
}
