using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentManage.Data;
using StudentManage.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserService>();

builder.Services.AddDbContext<StudentManageContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.Cookie.Name = "UserLoginCookie";
        config.LoginPath = "/Auth/Login";

        config.ExpireTimeSpan = TimeSpan.FromMinutes(1);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");


// app.MapControllerRoute(...): là phương thức dùng để định nghĩa một route cho các controller trong ứng dụng ASP.NET Core.
// name: "student": là tên của route. Bạn có thể sử dụng tên này để tham chiếu đến route đến mã của bạn.
// {action=Index}: Đây là tham số động cho action. Nếu không có action được chỉ định trong URL, nó sẽ mặc định gọi action Index. Nếu được gọi sẽ ứng với tên hàm trong controller
app.MapControllerRoute(
    name: "student",
    pattern: "student/{action=Index}/{id?}",
    defaults: new { controller = "Student", action = "Index" });


app.Run();
