// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiểm thử tự động, SQL Server/HTTP và tích hợp hệ thống.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan.Tests;

public sealed class HotelFactory : WebApplicationFactory<Program>
{
    public string EnvironmentName { get; init; } = "Development";
    public string SeedSetting { get; init; } = "false";
    public string Connection { get; } = CreateConnection();
    private static string CreateConnection()
    {
        var template = Environment.GetEnvironmentVariable("HOTEL_TEST_CONNECTION") ?? @"Server=(localdb)\Nhom6DeTai05;Database={database};Integrated Security=true;TrustServerCertificate=true";
        if (!template.Contains("{database}")) throw new InvalidOperationException("HOTEL_TEST_CONNECTION phải chứa {database}; không sử dụng database có sẵn để chạy test.");
        var connection = template.Replace("{database}", "Nhom6_Test_" + Guid.NewGuid().ToString("N"));
        if (!new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connection).InitialCatalog.StartsWith("Nhom6_Test_", StringComparison.Ordinal))
            throw new InvalidOperationException("Test chỉ được tạo và dọn database có tiền tố Nhom6_Test_.");
        return connection;
    }
    protected override void ConfigureWebHost(IWebHostBuilder b)
    {
        b.UseEnvironment(EnvironmentName);
        b.ConfigureAppConfiguration((_, c) => c.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["ConnectionStrings:Hotel"] = Connection,
            ["Demo:Seed"] = SeedSetting,
            ["https_port"] = "443",
            ["AllowedHosts"] = "localhost;hotel.test"
        }));
    }
}
public partial class SqlWebTests : IAsyncLifetime
{
    private readonly HotelFactory factory = new();
    private HttpClient client = null!;
    private IServiceScope Scope() => factory.Services.CreateScope();
    private static BookingService Booking(IServiceProvider p) => p.GetRequiredService<BookingService>();
    private DateTime Today => new HotelClock().Today;
    public async Task InitializeAsync() { client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false, BaseAddress = new Uri("http://localhost") }); using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); await db.Database.MigrateAsync(); await DemoSeed.Run(db, new HotelClock()); }
    public async Task DisposeAsync() { client.Dispose(); using (var s = Scope()) { var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); await db.Database.EnsureDeletedAsync(); } await factory.DisposeAsync(); }
    private async Task<string> Token(string url) { var html = await client.GetStringAsync(url); return WebUtility.HtmlDecode(Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value); }
    private async Task<HttpResponseMessage> Post(string path, string tokenPath, Dictionary<string, string> form) { form["__RequestVerificationToken"] = await Token(tokenPath); return await client.PostAsync(path, new FormUrlEncodedContent(form)); }
    private async Task Login(string user = "admin", string password = DemoSeed.Password) { var r = await Post("/TaiKhoan/DangNhap", "/TaiKhoan/DangNhap", new() { ["TenDangNhap"] = user, ["MatKhau"] = password }); Assert.Equal(HttpStatusCode.Redirect, r.StatusCode); }
    private async Task<int> RoomId() { using var s = Scope(); return await s.ServiceProvider.GetRequiredService<HotelDbContext>().Phongs.Where(x => x.SoPhong == "101").Select(x => x.MaPhong).SingleAsync(); }
    private async Task<int> AccountId() { using var s = Scope(); return await s.ServiceProvider.GetRequiredService<HotelDbContext>().TaiKhoans.Where(x => x.TenDangNhap == "khach01").Select(x => x.MaTaiKhoan).SingleAsync(); }
    private DatPhongForm Form(int room, int days = 60) => new() { PhongIds = [room], NgayNhan = Today.AddDays(days), NgayTra = Today.AddDays(days + 2) };
    [Fact]
    public async Task MigrationSeedAndStatisticsReconcile()
    {
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); Assert.Single(await db.Database.GetAppliedMigrationsAsync()); Assert.False(db.Database.HasPendingModelChanges());
        Assert.Equal(6, db.Model.GetEntityTypes().Count()); Assert.Equal(5, await db.LoaiPhongs.CountAsync()); Assert.Equal(20, await db.Phongs.CountAsync()); Assert.Equal(15, await db.KhachHangs.CountAsync()); Assert.Equal(16, await db.TaiKhoans.CountAsync()); Assert.Equal(18, await db.PhieuDats.CountAsync()); Assert.Equal(18, await db.ChiTiets.CountAsync());
        await DemoSeed.Run(db, new HotelClock()); Assert.Equal(18, await db.PhieuDats.CountAsync()); Assert.Equal(20, await db.Phongs.CountAsync()); Assert.Equal(16, await db.TaiKhoans.CountAsync());
        var p = await s.ServiceProvider.GetRequiredService<StatisticsService>().Read(); Assert.Equal(20, p.Totals["Phòng"]); Assert.Equal(4, p.Totals["Chờ xác nhận"]); Assert.Equal(4, p.Totals["Đã xác nhận"]); Assert.Equal(4, p.Totals["Phòng đang sử dụng"]); Assert.Equal(3, p.Totals["Đã trả phòng"]);
        Assert.Equal(20, p.RoomsByType.Sum(x => x.Count)); Assert.Equal(18, p.BookingsByState.Sum(x => x.Count)); Assert.Equal(15, p.BookingsByRoom.Sum(x => x.Count)); Assert.Equal(15, p.TopTypes.Sum(x => x.Count)); Assert.Equal(15, p.BookingsByCustomer.Sum(x => x.Count));
        Assert.Equal(await db.PhieuDats.Where(x => x.TrangThai == TrangThaiDat.DaTraPhong).SumAsync(x => x.TongTienDuKien), p.Revenue.Sum(x => x.Amount));
        Assert.All(await db.TaiKhoans.ToListAsync(), a => Assert.NotEqual(DemoSeed.Password, a.MatKhau));
        var now = new HotelClock().Now;
        Assert.All(await db.PhieuDats.ToListAsync(), p =>
        {
            Assert.True(p.NgayDat <= now, "Ngày tạo phiếu mẫu không được nằm trong tương lai.");
            Assert.True(p.NgayDat <= p.NgayNhanDuKien);
        });
    }
    [Fact]
    public async Task PublicAndAdminViewsHaveNoDeadRoutes()
    {
        foreach (var url in new[] { "/", "/Phong", "/Phong/Details/1", "/TaiKhoan/DangNhap" }) Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(url)).StatusCode);
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/ThongKe")).StatusCode); await Login();
        foreach (var url in new[] { "/ThongKe", "/Phong/Create", "/Phong/Edit/1", "/LoaiPhong", "/LoaiPhong/Create", "/LoaiPhong/Edit/1", "/LoaiPhong/Details/1", "/KhachHang", "/KhachHang/Details/1", "/PhieuDatPhong", "/PhieuDatPhong/Details/1", "/TaiKhoan", "/TaiKhoan/Create", "/TaiKhoan/Edit/1" }) Assert.True((await client.GetAsync(url)).StatusCode == HttpStatusCode.OK, url);
    }
    [Fact]
    public async Task LoginWrongLockedLogoutAndServerGuards()
    {
        var r = await Post("/TaiKhoan/DangNhap", "/TaiKhoan/DangNhap", new() { ["TenDangNhap"] = "admin", ["MatKhau"] = "wrong" }); Assert.Equal(HttpStatusCode.OK, r.StatusCode); Assert.Contains("không đúng", WebUtility.HtmlDecode(await r.Content.ReadAsStringAsync()));
        using (var s = Scope()) { var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var a = await db.TaiKhoans.SingleAsync(x => x.TenDangNhap == "khach01"); a.TrangThai = false; await db.SaveChangesAsync(); }
        r = await Post("/TaiKhoan/DangNhap", "/TaiKhoan/DangNhap", new() { ["TenDangNhap"] = "khach01", ["MatKhau"] = DemoSeed.Password }); Assert.Equal(HttpStatusCode.OK, r.StatusCode);
        await Login("khach02");
        foreach (var url in new[] { "/ThongKe", "/Phong/Create", "/Phong/Edit/1", "/LoaiPhong", "/KhachHang", "/PhieuDatPhong", "/TaiKhoan" }) Assert.Equal(HttpStatusCode.Forbidden, (await client.GetAsync(url)).StatusCode);
        r = await Post("/TaiKhoan/DangXuat", "/KhachHang/HoSo", new()); Assert.Equal(HttpStatusCode.Redirect, r.StatusCode); Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/DatPhong")).StatusCode);
    }
    [Fact]
    public async Task CustomerOwnershipAndAntiforgery()
    {
        await Login("khach01");
        Assert.Equal(HttpStatusCode.OK, (await client.GetAsync("/DatPhong/Details/1")).StatusCode); Assert.Equal(HttpStatusCode.NotFound, (await client.GetAsync("/DatPhong/Details/2")).StatusCode);
        foreach (var url in new[] { "/DatPhong", "/DatPhong?lichSu=true", "/DatPhong/Create", "/KhachHang/HoSo" }) Assert.Equal(HttpStatusCode.OK, (await client.GetAsync(url)).StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, (await client.PostAsync("/DatPhong/Create", new FormUrlEncodedContent(new Dictionary<string, string>()))).StatusCode);
        var r = await Post("/KhachHang/HoSo", "/KhachHang/HoSo", new() { ["MaTaiKhoan"] = "3", ["MaKhachHang"] = "2", ["HoTen"] = "Tên mới", ["Email"] = "new@example.com", ["SoDienThoai"] = "0901234567", ["CCCD"] = "001234567890", ["NgaySinh"] = "2000-01-01", ["GioiTinh"] = "Khác" }); Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); Assert.Equal("Tên mới", (await db.KhachHangs.SingleAsync(x => x.MaKhachHang == 1)).HoTen); Assert.Equal("Khách mẫu 02", (await db.KhachHangs.SingleAsync(x => x.MaKhachHang == 2)).HoTen);
    }
    [Fact]
    public async Task SearchFilterSortPageAndLinks()
    {
        using var s = Scope(); var search = s.ServiceProvider.GetRequiredService<RoomSearch>();
        var f = new PhongQuery { Q = "phòng không tồn tại", Page = 999 }; var p = await search.Find(f); Assert.Equal(0, p.Total); Assert.Equal(1, p.Query.Page);
        p = await search.Find(new() { Page = -5 }); Assert.Equal(8, p.Items.Count); Assert.Equal(1, p.Query.Page); p = await search.Find(new() { Page = 999 }); Assert.Equal(3, p.Query.Page); Assert.Equal(4, p.Items.Count);
        foreach (var sort in new[] { "so", "so_desc", "gia", "gia_desc", "tang", "tang_desc" }) { p = await search.Find(new() { Sort = sort }); var values = p.Items.Select(x => sort.StartsWith("gia") ? x.DonGiaDem : sort.StartsWith("tang") ? x.Tang : decimal.Parse(x.SoPhong)).ToList(); Assert.Equal(sort.EndsWith("desc") ? values.OrderDescending() : values.Order(), values); }
        p = await search.Find(new() { Q = "Tiêu chuẩn", Loai = 1, Tang = 1, TrangThai = TrangThaiPhong.SanSang, GiaTu = 1, GiaDen = 500000 }); Assert.Single(p.Items); Assert.Equal("101", p.Items[0].SoPhong);
        var html = await client.GetStringAsync("/Phong?Q=1&Sort=gia_desc&GiaTu=1&Tang=1&TrangThai=0"); Assert.Contains("gia_desc", html); Assert.Contains("GiaTu", html);
        html = await client.GetStringAsync("/Phong?Sort=gia_desc&GiaTu=1&GiaDen=9999999&TrangThai=0"); Assert.Matches("href=\"[^\"]*Page=2", html); Assert.Contains("Sort=gia_desc", html); Assert.Contains("GiaDen=9999999", html);
    }
    [Fact]
    public async Task BookingSnapshotAndTamperedPricesIgnored()
    {
        await Login("khach01"); var room = await RoomId();
        var r = await Post("/DatPhong/Create", "/DatPhong/Create", new() { ["PhongIds"] = room.ToString(), ["NgayNhan"] = Today.AddDays(60).ToString("yyyy-MM-dd"), ["NgayTra"] = Today.AddDays(62).ToString("yyyy-MM-dd"), ["DonGiaDem"] = "1", ["SoDem"] = "999", ["TongTienDuKien"] = "1", ["MaKhachHang"] = "2" }); Assert.Equal(HttpStatusCode.Redirect, r.StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var p = await db.PhieuDats.Include(x => x.ChiTiets).OrderByDescending(x => x.MaPhieuDat).FirstAsync(); Assert.Equal(1, p.MaKhachHang); Assert.Equal(800000, p.TongTienDuKien); Assert.Equal(2, p.ChiTiets.Single().SoDem); Assert.Equal(400000, p.ChiTiets.Single().DonGiaDem);
        var m = await db.Phongs.FindAsync(room); m!.DonGiaDem = 900000; await db.SaveChangesAsync(); Assert.Equal(400000, (await db.ChiTiets.AsNoTracking().SingleAsync(x => x.MaPhieuDat == p.MaPhieuDat)).DonGiaDem);
    }
    [Fact]
    public async Task InvalidBookingsNeverPersist()
    {
        var room = await RoomId(); var aid = await AccountId(); using var s = Scope(); var sp = s.ServiceProvider; var db = sp.GetRequiredService<HotelDbContext>(); var before = await db.PhieuDats.CountAsync();
        var invalid = new[] { new DatPhongForm { PhongIds = [room], NgayNhan = Today.AddDays(-1), NgayTra = Today.AddDays(2) }, new DatPhongForm { PhongIds = [room], NgayNhan = Today, NgayTra = Today }, new DatPhongForm { PhongIds = [], NgayNhan = Today, NgayTra = Today.AddDays(1) }, new DatPhongForm { PhongIds = [99999], NgayNhan = Today, NgayTra = Today.AddDays(1) }, new DatPhongForm { PhongIds = [room, room], NgayNhan = Today, NgayTra = Today.AddDays(1) } };
        foreach (var f in invalid) await Assert.ThrowsAsync<NghiepVuException>(() => Booking(sp).Book(aid, f));
        var stopped = await db.Phongs.Where(x => x.TrangThai == TrangThaiPhong.TamNgung).Select(x => x.MaPhong).FirstAsync(); await Assert.ThrowsAsync<NghiepVuException>(() => Booking(sp).Book(aid, Form(stopped)));
        await Assert.ThrowsAsync<NghiepVuException>(() => Booking(sp).Book(99999, Form(room)));
        Assert.Equal(before, await db.PhieuDats.CountAsync());
    }
    [Fact]
    public async Task OverlapAndConcurrentRequestsAreAtomic()
    {
        var aid = await AccountId(); var room = await RoomId();
        async Task<bool> Try() { using var s = Scope(); try { await Booking(s.ServiceProvider).Book(aid, Form(room)); return true; } catch (NghiepVuException) { return false; } }
        var results = await Task.WhenAll(Try(), Try()); Assert.Single(results, x => x);
        using var scope = Scope(); var service = Booking(scope.ServiceProvider); await Assert.ThrowsAsync<NghiepVuException>(() => service.Book(aid, Form(room)));
        await service.Book(aid, new() { PhongIds = [room], NgayNhan = Today.AddDays(62), NgayTra = Today.AddDays(63) });
        var db = scope.ServiceProvider.GetRequiredService<HotelDbContext>(); Assert.Equal(2, await db.PhieuDats.CountAsync(x => x.NgayNhanDuKien >= Today.AddDays(60)));
    }
    [Fact]
    public async Task MultiRoomRollbackAndMoney()
    {
        var aid = await AccountId(); using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var rs = await db.Phongs.OrderBy(x => x.MaPhong).Take(2).ToListAsync();
        var id = await Booking(s.ServiceProvider).Book(aid, new() { PhongIds = rs.Select(x => x.MaPhong).ToArray(), NgayNhan = Today.AddDays(60), NgayTra = Today.AddDays(63) });
        var p = await db.PhieuDats.Include(x => x.ChiTiets).SingleAsync(x => x.MaPhieuDat == id); Assert.Equal(2, p.ChiTiets.Count); Assert.Equal(rs.Sum(x => x.DonGiaDem) * 3, p.TongTienDuKien);
        await Assert.ThrowsAsync<NghiepVuException>(() => Booking(s.ServiceProvider).Book(aid, new() { PhongIds = [rs[0].MaPhong, 3], NgayNhan = Today.AddDays(61), NgayTra = Today.AddDays(63) })); Assert.Equal(19, await db.PhieuDats.CountAsync()); Assert.Equal(20, await db.ChiTiets.CountAsync());
    }
    [Fact]
    public async Task StateLifecycleUpdatesRoomAndRejectsInvalidMoves()
    {
        var aid = await AccountId(); var room = await RoomId(); using var s = Scope(); var sp = s.ServiceProvider; var db = sp.GetRequiredService<HotelDbContext>(); var service = Booking(sp);
        var id = await service.Book(aid, Form(room, 0)); await Assert.ThrowsAsync<NghiepVuException>(() => service.Move(id, TrangThaiDat.DaNhanPhong)); await service.Move(id, TrangThaiDat.DaXacNhan);
        Assert.Equal(TrangThaiPhong.SanSang, (await db.Phongs.FindAsync(room))!.TrangThai); await service.Move(id, TrangThaiDat.DaNhanPhong); Assert.Equal(TrangThaiPhong.DangSuDung, (await db.Phongs.FindAsync(room))!.TrangThai);
        await Assert.ThrowsAsync<NghiepVuException>(() => service.Move(id, TrangThaiDat.DaHuy)); await service.Move(id, TrangThaiDat.DaTraPhong); Assert.Equal(TrangThaiPhong.SanSang, (await db.Phongs.FindAsync(room))!.TrangThai); await Assert.ThrowsAsync<NghiepVuException>(() => service.Move(id, TrangThaiDat.DaXacNhan));
    }
    [Fact]
    public async Task CancellationAndFutureCheckinRules()
    {
        var room = await RoomId(); var aid = await AccountId(); using var s = Scope(); var service = Booking(s.ServiceProvider); var id = await service.Book(aid, Form(room)); await service.Move(id, TrangThaiDat.DaHuy); await service.Book(aid, Form(room));
        var next = await service.Book(aid, Form(room, 70)); await service.Move(next, TrangThaiDat.DaXacNhan); await Assert.ThrowsAsync<NghiepVuException>(() => service.Move(next, TrangThaiDat.DaNhanPhong)); await service.Move(next, TrangThaiDat.DaHuy);
    }
    [Fact]
    public async Task CrudValidationUniquenessAndForeignKeys()
    {
        await Login();
        var typeForm = new Dictionary<string, string> { ["TenLoaiPhong"] = "Loại kiểm thử", ["SoNguoiToiDa"] = "2", ["DonGiaDem"] = "600000", ["TrangThai"] = "true" };
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/LoaiPhong/Create", "/LoaiPhong/Create", typeForm)).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await Post("/LoaiPhong/Create", "/LoaiPhong/Create", typeForm)).StatusCode);
        using var s = Scope(); var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var type = await db.LoaiPhongs.SingleAsync(x => x.TenLoaiPhong == "Loại kiểm thử");
        var roomForm = new Dictionary<string, string> { ["SoPhong"] = "TEST-01", ["MaLoaiPhong"] = type.MaLoaiPhong.ToString(), ["Tang"] = "6", ["DonGiaDem"] = "600000", ["TrangThai"] = "0" };
        Assert.Equal(HttpStatusCode.Redirect, (await Post("/Phong/Create", "/Phong/Create", roomForm)).StatusCode); Assert.Equal(HttpStatusCode.OK, (await Post("/Phong/Create", "/Phong/Create", roomForm)).StatusCode);
        var room = await db.Phongs.AsNoTracking().SingleAsync(x => x.SoPhong == "TEST-01"); roomForm["Tang"] = "-1"; Assert.Equal(HttpStatusCode.OK, (await Post($"/Phong/Edit/{room.MaPhong}", $"/Phong/Edit/{room.MaPhong}", roomForm)).StatusCode);
        await Post($"/LoaiPhong/Delete/{type.MaLoaiPhong}", "/LoaiPhong", new()); Assert.True(await db.LoaiPhongs.AnyAsync(x => x.MaLoaiPhong == type.MaLoaiPhong));
        await Post($"/Phong/Delete/{room.MaPhong}", "/Phong", new()); Assert.False(await db.Phongs.AnyAsync(x => x.MaPhong == room.MaPhong)); await Post($"/LoaiPhong/Delete/{type.MaLoaiPhong}", "/LoaiPhong", new()); Assert.False(await db.LoaiPhongs.AnyAsync(x => x.MaLoaiPhong == type.MaLoaiPhong));
        await Post("/Phong/Delete/1", "/Phong", new()); Assert.True(await db.Phongs.AnyAsync(x => x.MaPhong == 1));
    }
    [Fact]
    public async Task AdminPostsAndLockedSessionRevalidated()
    {
        await Login("khach01"); var r = await Post("/Phong/Delete/1", "/KhachHang/HoSo", new()); Assert.Equal(HttpStatusCode.Forbidden, r.StatusCode);
        using (var s = Scope()) { var db = s.ServiceProvider.GetRequiredService<HotelDbContext>(); var a = await db.TaiKhoans.SingleAsync(x => x.TenDangNhap == "khach01"); a.TrangThai = false; await db.SaveChangesAsync(); }
        Assert.Equal(HttpStatusCode.Redirect, (await client.GetAsync("/DatPhong")).StatusCode);
    }
}
