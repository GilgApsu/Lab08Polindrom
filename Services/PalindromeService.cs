using Microsoft.EntityFrameworkCore;
using PalindromeLab.Data;
using PalindromeLab.Models;

namespace PalindromeLab.Services;

public class PalindromeService : IPalindromeService
{
    private readonly IDbContextFactory<AppDbContext> _factory;

    public PalindromeService(IDbContextFactory<AppDbContext> factory)
    {
        _factory = factory;
    }

    // Оставляем только буквы и цифры, приводим к нижнему регистру:
    // "А роза упала на лапу Азора" -> "арозаупаланалапуазора"
    public string Normalize(string text) =>
        new string(text.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();

    public bool IsPalindrome(string text)
    {
        var s = Normalize(text);
        if (s.Length == 0) return false;

        for (int i = 0, j = s.Length - 1; i < j; i++, j--)
            if (s[i] != s[j]) return false;

        return true;
    }

    public async Task<CheckedWord> CheckAndSaveAsync(string text)
    {
        text = text.Trim();
        var entity = new CheckedWord
        {
            Text = text,
            IsPalindrome = IsPalindrome(text),
            CheckedAt = DateTime.Now
        };

        await using var db = await _factory.CreateDbContextAsync();
        db.CheckedWords.Add(entity);
        await db.SaveChangesAsync();
        return entity;
    }

    public async Task<List<CheckedWord>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.CheckedWords
            .AsNoTracking()
            .OrderByDescending(w => w.CheckedAt)
            .ToListAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await using var db = await _factory.CreateDbContextAsync();
        await db.CheckedWords.Where(w => w.Id == id).ExecuteDeleteAsync();
    }
}
