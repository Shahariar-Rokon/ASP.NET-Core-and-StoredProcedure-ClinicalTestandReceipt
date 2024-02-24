using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ZooAppUsingSp.Models
{
    public class Test
    {
        public Test() { }
        public Test(string name) : this()
        {
            Name = name;
        }

        public Test(string name, string imageUrl) : this(name)
        {
            ImageUrl = imageUrl;
        }

        public Test(int id, string name, string imageUrl) : this(name, imageUrl)
        {
            Id = id;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [AllowNull, DataType(DataType.ImageUrl)]
        public string ImageUrl { get; set; }

        [AllowNull]
        public ICollection<TestDetail> testDetails { get; set; }
    }
}
