using System.ComponentModel.DataAnnotations;

namespace News.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }
        [Display(Name="Заголовок новости")]
        public string? PostTitle { get; set; }


        [Display(Name = "Текст новости")]
        public string? PostBody { get; set; }


        [Display(Name = "Категория новости")]
        public int CategoryId { get; set; }
        public virtual Category?  Category { get; set; }



        [Display(Name = "Дата публикации")]
        [DisplayFormat(DataFormatString = "yyyy-MM-ddTHH:mm", ApplyFormatInEditMode = true)]
        public DateTime PostDate { get; set; }
    }
}
