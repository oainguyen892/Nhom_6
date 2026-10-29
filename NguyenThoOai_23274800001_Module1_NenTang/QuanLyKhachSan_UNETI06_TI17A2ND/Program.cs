// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Threading.RateLimiting;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews(o =>
{
    o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
    o.ModelBinderProviders.Insert(0, new DecimalBinderProvider());
    o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor(_ => "Vui lòng nhập số hợp lệ.");
    o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((_, field) => $"Giá trị {field} không hợp lệ.");
});
builder.Services.AddDbContext<HotelDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Hotel") ?? throw new InvalidOperationException("Thiếu ConnectionStrings:Hotel.")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(30);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.SameSite = SameSiteMode.Lax;
    o.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
});
builder.Services.AddAntiforgery(o => o.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always);
builder.Services.Configure<CookieTempDataProviderOptions>(o => o.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always);
builder.Services.AddSingleton<HotelClock>();
builder.Services.AddScoped<HotelTransaction>(); builder.Services.AddScoped<BookingService>(); builder.Services.AddScoped<RoomSearch>(); builder.Services.AddScoped<StatisticsService>();
builder.Services.AddRateLimiter(o => { o.RejectionStatusCode = 429; o.AddPolicy("login", c => RateLimitPartition.GetFixedWindowLimiter(c.Connection.RemoteIpAddress?.ToString() ?? "local", _ => new FixedWindowRateLimiterOptions { PermitLimit = 15, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 })); });
var app = builder.Build();
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("vi-VN"); CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("vi-VN");
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); app.UseHttpsRedirection(); }
app.UseStaticFiles(); app.UseRouting(); app.UseRateLimiter(); app.UseSession();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
if (app.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Demo:Seed"))
{
    using var scope = app.Services.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
    await db.Database.MigrateAsync(); await DemoSeed.Run(db, scope.ServiceProvider.GetRequiredService<HotelClock>());
}
app.Run();
public partial class Program;
