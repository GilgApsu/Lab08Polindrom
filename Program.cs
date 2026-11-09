using Microsoft.EntityFrameworkCore;
using PalindromeLab.Components;
using PalindromeLab.Data;
using PalindromeLab.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Фабрика контекстов - рекомендуемый способ работы с EF Core в Blazor Server
builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlite("Data Source=palindromes.db"));

// Бизнес-логика через Dependency Injection
builder.Services.AddScoped<IPalindromeService, PalindromeService>();

var app = builder.Build();

// Создаём БД при первом запуске
using (var scope = app.Services.CreateScope())
{
    var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>();
    using var db = factory.CreateDbContext();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
