using PalindromeLab.Models;

namespace PalindromeLab.Services;

public interface IPalindromeService
{
    bool IsPalindrome(string text);
    string Normalize(string text);

    Task<CheckedWord> CheckAndSaveAsync(string text);
    Task<List<CheckedWord>> GetAllAsync();
    Task DeleteAsync(int id);
}
