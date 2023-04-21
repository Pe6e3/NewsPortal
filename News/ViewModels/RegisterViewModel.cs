using System.ComponentModel.DataAnnotations;

namespace News.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Не указано Имя")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Не указан пароль")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string? ConfirmPassword { get; set; }
}
