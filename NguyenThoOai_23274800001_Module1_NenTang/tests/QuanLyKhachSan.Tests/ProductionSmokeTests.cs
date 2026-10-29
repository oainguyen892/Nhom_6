// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiểm thử tự động, SQL Server/HTTP và tích hợp hệ thống.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;

namespace QuanLyKhachSan.Tests;

public sealed class ProductionSmokeTests : IAsyncLifetime
{
    private readonly HotelFactory factory = new() { EnvironmentName = "Production", SeedSetting = "true" };
    private HttpClient client = null!;

    public Task InitializeAsync()
    {
        client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new Uri("https://hotel.test"),
            AllowAutoRedirect = false
        });
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        client.Dispose();
        using var scope = factory.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<HotelDbContext>().Database.EnsureDeletedAsync();
        await factory.DisposeAsync();
    }

    [Fact]
    public async Task ProductionDoesNotMigrateOrSeedEvenWhenDemoSettingIsTrue()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.False(await db.Database.CanConnectAsync());
        var response = await client.GetAsync("/");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var html = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain(DemoSeed.Password, html);
        Assert.DoesNotContain("khach01", html);
    }

    [Fact]
    public async Task ProductionRedirectsHttpAndAddsHstsOnHttps()
    {
        var redirect = await client.GetAsync("http://hotel.test/");
        Assert.Equal(HttpStatusCode.TemporaryRedirect, redirect.StatusCode);
        Assert.Equal("https://hotel.test/", redirect.Headers.Location?.AbsoluteUri);
        var response = await client.GetAsync("/");
        Assert.True(response.Headers.Contains("Strict-Transport-Security"));
    }

    [Fact]
    public async Task ProductionLoginUsesSecureCookiesAndGuardsAdmin()
    {
        using (var scope = factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
            await db.Database.MigrateAsync();
            var account = new TaiKhoan { TenDangNhap = "production-test-admin", HoTen = "Quản trị kiểm thử", Email = "admin@example.com", VaiTro = VaiTro.Admin };
            account.MatKhau = new PasswordHasher<TaiKhoan>().HashPassword(account, "Synthetic@Test2026");
            db.TaiKhoans.Add(account);
            await db.SaveChangesAsync();
            Assert.Equal(1, await db.TaiKhoans.CountAsync());
        }
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/ThongKe")).StatusCode);
        var login = await client.GetAsync("/TaiKhoan/DangNhap");
        Assert.All(login.Headers.GetValues("Set-Cookie"), cookie => Assert.Contains("secure", cookie.ToLowerInvariant()));
        var html = await login.Content.ReadAsStringAsync();
        var token = WebUtility.HtmlDecode(Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value);
        var response = await client.PostAsync("/TaiKhoan/DangNhap", new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["TenDangNhap"] = "production-test-admin",
            ["MatKhau"] = "Synthetic@Test2026",
            ["__RequestVerificationToken"] = token
        }));
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        var sessionCookie = Assert.Single(response.Headers.GetValues("Set-Cookie"), cookie => cookie.Contains(".AspNetCore.Session"));
        Assert.Contains("secure", sessionCookie.ToLowerInvariant());
        Assert.Contains("httponly", sessionCookie.ToLowerInvariant());
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/ThongKe")).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsync("/TaiKhoan/DangXuat", new StringContent(""))).StatusCode);
    }

    [Fact]
    public async Task ProductionErrorPageDoesNotExposeDatabaseException()
    {
        var response = await client.GetAsync("/Phong");
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        var html = WebUtility.HtmlDecode(await response.Content.ReadAsStringAsync());
        Assert.Contains("Không thể hoàn tất yêu cầu", html);
        Assert.DoesNotContain("SqlException", html);
        Assert.DoesNotContain("StackTrace", html);
        Assert.DoesNotContain("Nhom6_Test_", html);
    }
}
