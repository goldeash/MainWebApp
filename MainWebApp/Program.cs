using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Data;
using UserManagementApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контекст базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавляем Identity с настройками для пароля
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.Password.RequiredLength = 1; // Минимальная длина пароля — 1 символ
    options.Password.RequireDigit = false; // Не требовать цифры
    options.Password.RequireLowercase = false; // Не требовать строчные буквы
    options.Password.RequireUppercase = false; // Не требовать заглавные буквы
    options.Password.RequireNonAlphanumeric = false; // Не требовать специальные символы
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Настройка middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Middleware для перенаправления залогиненного пользователя на страницу управления пользователями
app.Use(async (context, next) =>
{
    if (context.User.Identity.IsAuthenticated && context.Request.Path == "/")
    {
        context.Response.Redirect("/Users/Index");
        return;
    }
    await next();
});

// Настройка маршрутов
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();