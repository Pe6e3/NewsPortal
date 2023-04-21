using System.ComponentModel.DataAnnotations;

namespace News.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Не указано Имя")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
}
