// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiểm thử tự động, SQL Server/HTTP và tích hợp hệ thống.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan.Tests;

public partial class SqlWebTests
{
    [Theory]
    [InlineData("700000.25")]
    [InlineData("700000,25")]
    public async Task DecimalPriceBindsWithoutCultureDrift(string price)
    {
        await Login();
        var f = new Dictionary<string, string> { ["SoPhong"] = "101", ["MaLoaiPhong"] = "1", ["Tang"] = "1", ["DonGiaDem"] = price, ["TrangThai"] = "0" };
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/Phong/Edit/1", "/Phong/Edit/1", f)).StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.Equal(700000.25m, (await db.Phongs.AsNoTracking().SingleAsync(x => x.MaPhong == 1)).DonGiaDem);
        var html = WebUtility.HtmlDecode(await client.GetStringAsync("/Phong?GiaTu=400000.25&GiaDen=9999999.75"));
        Assert.Contains("value=\"400000.25\"", html);
        Assert.Contains("value=\"9999999.75\"", html);
        Assert.Contains("GiaTu=400000.25", html);
        f["DonGiaDem"] = "700000.251";
        Assert.Equal(HttpStatusCode.OK, (await Post("/Phong/Edit/1", "/Phong/Edit/1", f)).StatusCode);
        Assert.Equal(700000.25m, (await db.Phongs.AsNoTracking().SingleAsync(x => x.MaPhong == 1)).DonGiaDem);
    }

    [Fact]
    public async Task FailedMultiRoomCheckinRollsBackAndClearsTrackedChanges()
    {
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var service = Booking(s.ServiceProvider);
        var account = await AccountId();
        var id = await service.Book(account, new() { PhongIds = [1, 2], NgayNhan = Today, NgayTra = Today.AddDays(2) });
        await service.Move(id, TrangThaiDat.DaXacNhan);
        (await db.Phongs.FindAsync(2))!.TrangThai = TrangThaiPhong.TamNgung;
        await db.SaveChangesAsync();
        await Assert.ThrowsAsync<NghiepVuException>(() => service.Move(id, TrangThaiDat.DaNhanPhong));
        Assert.Equal(TrangThaiPhong.SanSang, (await db.Phongs.FindAsync(1))!.TrangThai);
        Assert.Equal(TrangThaiDat.DaXacNhan, (await db.PhieuDats.FindAsync(id))!.TrangThai);
        await service.Book(account, Form(1, 60));
        using var verify = Scope();
        Assert.Equal(TrangThaiPhong.SanSang, (await verify.ServiceProvider.GetRequiredService<HotelDbContext>().Phongs.FindAsync(1))!.TrangThai);
    }
}
