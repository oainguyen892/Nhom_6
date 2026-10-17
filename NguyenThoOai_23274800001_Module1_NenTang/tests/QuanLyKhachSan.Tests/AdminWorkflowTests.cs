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
    [Fact]
    public async Task CustomerToAdminToStayHistoryWorksThroughHttp()
    {
        await Login("khach01");
        var response = await Post("/DatPhong/Create", "/DatPhong/Create", new()
        {
            ["PhongIds[0]"] = "1", ["PhongIds[1]"] = "2",
            ["NgayNhan"] = Today.ToString("yyyy-MM-dd"), ["NgayTra"] = Today.AddDays(2).ToString("yyyy-MM-dd")
        });
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        var booking = await db.PhieuDats.AsNoTracking().OrderByDescending(x => x.MaPhieuDat).FirstAsync();
        var id = booking.MaPhieuDat;
        Assert.Equal(2100000m, booking.TongTienDuKien);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync($"/DatPhong/Details/{id}")).StatusCode);
        await Post("/TaiKhoan/DangXuat", "/DatPhong", new());
        await Login();
        foreach (var state in new[] { TrangThaiDat.DaXacNhan, TrangThaiDat.DaNhanPhong, TrangThaiDat.DaTraPhong })
        {
            response = await Post("/PhieuDatPhong/ChuyenTrangThai", $"/PhieuDatPhong/Details/{id}",
                new() { ["id"] = id.ToString(), ["target"] = ((int)state).ToString() });
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal(state, (await db.PhieuDats.AsNoTracking().SingleAsync(x => x.MaPhieuDat == id)).TrangThai);
            var expected = state == TrangThaiDat.DaNhanPhong ? TrangThaiPhong.DangSuDung : TrangThaiPhong.SanSang;
            Assert.All(await db.Phongs.AsNoTracking().Where(x => x.MaPhong == 1 || x.MaPhong == 2).ToListAsync(), r => Assert.Equal(expected, r.TrangThai));
            var html = WebUtility.HtmlDecode(await client.GetStringAsync($"/PhieuDatPhong/Details/{id}"));
            Assert.Contains("Thao tác thành công", html);
        }
        var report = await s.ServiceProvider.GetRequiredService<StatisticsService>().Read();
        Assert.Equal(4, report.Totals["Đã trả phòng"]);
        Assert.Contains(report.Revenue, x => x.Label == $"{Today.Month:00}/{Today.Year}" && x.Amount == 2100000m);
        await Post("/TaiKhoan/DangXuat", "/ThongKe", new());
        await Login("khach01");
        var history = WebUtility.HtmlDecode(await client.GetStringAsync("/DatPhong?lichSu=true"));
        Assert.Contains($"/DatPhong/Details/{id}", history);
        Assert.Contains("Đã trả phòng", history);
    }

    [Fact]
    public async Task CustomerSearchAndStatusChangeArePersisted()
    {
        await Login();
        foreach (var q in new[] { "Khách mẫu 01", "0900000001", "001000000001" })
        {
            var html = WebUtility.HtmlDecode(await client.GetStringAsync("/KhachHang?q=" + Uri.EscapeDataString(q)));
            Assert.Contains("Khách mẫu 01", html);
            Assert.DoesNotContain("Khách mẫu 02", html);
        }
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/KhachHang/TrangThai/1", "/KhachHang", new() { ["active"] = "false" })).StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.False((await db.KhachHangs.AsNoTracking().SingleAsync(x => x.MaKhachHang == 1)).TrangThai);
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/KhachHang/TrangThai/1", "/KhachHang", new() { ["active"] = "true" })).StatusCode);
        Assert.True((await db.KhachHangs.AsNoTracking().SingleAsync(x => x.MaKhachHang == 1)).TrangThai);
    }

    [Fact]
    public async Task InactiveTypeAndLockedAccountCannotCreateBookings()
    {
        var account = await AccountId();
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        var type = await db.LoaiPhongs.FindAsync(1);
        type!.TrangThai = false; await db.SaveChangesAsync();
        await Assert.ThrowsAsync<NghiepVuException>(() => Booking(s.ServiceProvider).Book(account, Form(1)));
        type.TrangThai = true;
        (await db.TaiKhoans.FindAsync(account))!.TrangThai = false; await db.SaveChangesAsync();
        await Assert.ThrowsAsync<NghiepVuException>(() => Booking(s.ServiceProvider).Book(account, Form(1)));
        Assert.Equal(18, await db.PhieuDats.CountAsync());
    }

    [Fact]
    public async Task LocalRoomIllustrationsAndVietnameseFontAreServed()
    {
        foreach (var name in new[] { "standard", "superior", "deluxe", "family", "suite" })
        {
            var response = await client.GetAsync($"/images/rooms/{name}.png");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("image/png", response.Content.Headers.ContentType?.MediaType);
            Assert.True((await response.Content.ReadAsByteArrayAsync()).Length > 1000);
        }
        var css = await client.GetStringAsync("/css/site.css");
        Assert.Contains("Noto Sans", css);
        var fontUrl = System.Text.RegularExpressions.Regex.Match(css, @"url\(['"" ]*([^)'"" ]+\.ttf)").Groups[1].Value;
        Assert.NotEmpty(fontUrl);
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(new Uri(new Uri("http://localhost/css/site.css"), fontUrl))).StatusCode);
        var home = WebUtility.HtmlDecode(await client.GetStringAsync("/"));
        Assert.Contains("Một kỳ nghỉ trọn vẹn", home);
        Assert.Contains("Ảnh minh họa", home);
    }

    [Fact]
    public async Task AccountCreationAndSelfLockProtection()
    {
        await Login();
        var f = new Dictionary<string, string> { ["TenDangNhap"] = "khachmoi", ["HoTen"] = "Khách mới", ["Email"] = "moi@example.com", ["MatKhauMoi"] = "Test@Password_2026", ["SoDienThoai"] = "0901234567", ["CCCD"] = "001234567890", ["VaiTro"] = "1" };
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/TaiKhoan/Create", "/TaiKhoan/Create", f)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Post("/TaiKhoan/Create", "/TaiKhoan/Create", f)).StatusCode);
        using var s = Scope();
        var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        var a = await db.TaiKhoans.Include(x => x.KhachHang).SingleAsync(x => x.TenDangNhap == "khachmoi");
        Assert.NotNull(a.KhachHang); Assert.NotEqual(f["MatKhauMoi"], a.MatKhau);
        var admin = await db.TaiKhoans.SingleAsync(x => x.TenDangNhap == "admin");
        Assert.Equal(HttpStatusCode.OK, (await Post($"/TaiKhoan/Edit/{admin.MaTaiKhoan}", $"/TaiKhoan/Edit/{admin.MaTaiKhoan}", new() { ["Id"] = admin.MaTaiKhoan.ToString(), ["VaiTro"] = "0", ["TrangThai"] = "false" })).StatusCode);
        Assert.True((await db.TaiKhoans.AsNoTracking().SingleAsync(x => x.MaTaiKhoan == admin.MaTaiKhoan)).TrangThai);
    }

    [Fact]
    public async Task BookingAdminHttpFiltersAndTransitions()
    {
        await Login();
        var html = WebUtility.HtmlDecode(await client.GetStringAsync("/PhieuDatPhong?q=Khách%20mẫu%2001&trangThai=0"));
        Assert.Contains("Khách mẫu 01", html); Assert.Contains("#1", html); Assert.DoesNotContain("#2</a>", html);
        html = WebUtility.HtmlDecode(await client.GetStringAsync($"/PhieuDatPhong?q=Khách%20mẫu%2001&trangThai=0&ngayNhan={Today.AddDays(7):yyyy-MM-dd}"));
        Assert.Contains("#1</a>", html); Assert.DoesNotContain("#16</a>", html);
        html = WebUtility.HtmlDecode(await client.GetStringAsync($"/PhieuDatPhong?ngayNhan={Today.AddDays(100):yyyy-MM-dd}"));
        Assert.DoesNotContain("#1</a>", html);
        var r = await Post("/PhieuDatPhong/ChuyenTrangThai", "/PhieuDatPhong/Details/1", new() { ["id"] = "1", ["target"] = "1" });
        Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.Equal(TrangThaiDat.DaXacNhan, (await db.PhieuDats.AsNoTracking().SingleAsync(x => x.MaPhieuDat == 1)).TrangThai);
        await Post("/PhieuDatPhong/ChuyenTrangThai", "/PhieuDatPhong/Details/1", new() { ["id"] = "1", ["target"] = "3" });
        Assert.Equal(TrangThaiDat.DaXacNhan, (await db.PhieuDats.AsNoTracking().SingleAsync(x => x.MaPhieuDat == 1)).TrangThai);
        html = WebUtility.HtmlDecode(await client.GetStringAsync("/PhieuDatPhong/Details/1")); Assert.Contains("Không được chuyển trạng thái", html);
    }

    [Fact]
    public async Task RoomEditPreservesHistoryAndPriceSnapshot()
    {
        await Login();
        var f = new Dictionary<string, string> { ["SoPhong"] = "101", ["MaLoaiPhong"] = "1", ["Tang"] = "1", ["DonGiaDem"] = "700000", ["TrangThai"] = "0", ["MoTa"] = "Mô tả mới" };
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/Phong/Edit/1", "/Phong/Edit/1", f)).StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.Equal(700000, (await db.Phongs.AsNoTracking().SingleAsync(x => x.MaPhong == 1)).DonGiaDem);
        Assert.Equal(400000, (await db.ChiTiets.AsNoTracking().SingleAsync(x => x.MaPhong == 1)).DonGiaDem);
        f["MaLoaiPhong"] = "2";
        var r = await Post("/Phong/Edit/1", "/Phong/Edit/1", f);
        Assert.Equal(HttpStatusCode.OK, r.StatusCode); Assert.Contains("bảo toàn lịch sử", WebUtility.HtmlDecode(await r.Content.ReadAsStringAsync()));
        f["MaLoaiPhong"] = "1"; f["TrangThai"] = "2";
        Assert.Equal(HttpStatusCode.OK, (await Post("/Phong/Edit/1", "/Phong/Edit/1", f)).StatusCode);
    }

    [Fact]
    public async Task CustomerDisabledCannotBookAndValidationShowsMessages()
    {
        await Login("khach01");
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        var k = await db.KhachHangs.SingleAsync(x => x.MaKhachHang == 1); k.TrangThai = false; await db.SaveChangesAsync();
        await Assert.ThrowsAsync<NghiepVuException>(() => Booking(s.ServiceProvider).Book(k.MaTaiKhoan, Form(1)));
        var r = await Post("/DatPhong/Create", "/DatPhong/Create", new() { ["PhongIds"] = "1", ["NgayNhan"] = Today.AddDays(-1).ToString("yyyy-MM-dd"), ["NgayTra"] = Today.AddDays(1).ToString("yyyy-MM-dd") });
        Assert.Equal(HttpStatusCode.OK, r.StatusCode); Assert.Contains("không được trước hôm nay", WebUtility.HtmlDecode(await r.Content.ReadAsStringAsync()));
        r = await Post("/KhachHang/HoSo", "/KhachHang/HoSo", new() { ["HoTen"] = "", ["Email"] = "bad", ["CCCD"] = "1", ["SoDienThoai"] = "2", ["NgaySinh"] = "2999-01-01" });
        Assert.Equal(HttpStatusCode.OK, r.StatusCode); Assert.Contains("validation-summary-errors", await r.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task TypeStatusValidationAndTypeEdit()
    {
        await Login();
        var f = new Dictionary<string, string> { ["TenLoaiPhong"] = "Tiêu chuẩn", ["SoNguoiToiDa"] = "2", ["DonGiaDem"] = "420000", ["TrangThai"] = "false" };
        var r = await Post("/LoaiPhong/Edit/1", "/LoaiPhong/Edit/1", f); Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        f["TrangThai"] = "true"; Assert.Equal(HttpStatusCode.Redirect, (await Post("/LoaiPhong/Edit/1", "/LoaiPhong/Edit/1", f)).StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        Assert.Equal(2, (await db.LoaiPhongs.AsNoTracking().SingleAsync(x => x.MaLoaiPhong == 1)).SoNguoiToiDa);
        f["SoNguoiToiDa"] = "0"; Assert.Equal(HttpStatusCode.OK, (await Post("/LoaiPhong/Edit/1", "/LoaiPhong/Edit/1", f)).StatusCode);
    }

    [Fact]
    public async Task SqlConstraintsEnforceUniquenessAndRestrictDeletes()
    {
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>();
        var room = await db.Phongs.AsNoTracking().FirstAsync();
        db.Phongs.Add(new Phong { SoPhong = room.SoPhong, MaLoaiPhong = room.MaLoaiPhong, Tang = 1, DonGiaDem = 1 });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync()); db.ChangeTracker.Clear();
        var type = await db.LoaiPhongs.FirstAsync(); db.LoaiPhongs.Remove(type);
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync()); db.ChangeTracker.Clear();
        db.ChiTiets.Add(new ChiTietDatPhong { MaPhong = 99999, MaPhieuDat = 1, SoDem = 1, DonGiaDem = 10, ThanhTien = 10 });
        await Assert.ThrowsAsync<DbUpdateException>(() => db.SaveChangesAsync());
    }

    [Theory]
    [InlineData("1.001", false)]
    [InlineData("1.25", true)]
    public void MoneyScaleValidation(string price, bool expected)
    {
        var value = decimal.Parse(price, System.Globalization.CultureInfo.InvariantCulture);
        Assert.Equal(expected, new TienAttribute().IsValid(value));
    }
}
