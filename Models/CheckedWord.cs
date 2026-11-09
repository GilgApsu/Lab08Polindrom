using System.ComponentModel.DataAnnotations;

namespace PalindromeLab.Models;

public class CheckedWord
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите слово или фразу")]
    [StringLength(200, ErrorMessage = "Не более 200 символов")]
    public string Text { get; set; } = "";

    public bool IsPalindrome { get; set; }

    public DateTime CheckedAt { get; set; } = DateTime.Now;
}
