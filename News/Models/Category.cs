using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class Category
    {

        [Key]
        public int CatehoryId { get; set; }

        [Display(Name = "Название категории")]
        public string? CategoryName { get; set; }
        public List<Post>? Posts { get; set; }
    }
}
