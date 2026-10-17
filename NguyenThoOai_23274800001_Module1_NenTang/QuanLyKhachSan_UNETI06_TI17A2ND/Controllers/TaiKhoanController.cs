// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Module 1: tài khoản, đăng nhập, phân quyền và quản lý loại phòng.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

public class TaiKhoanController(HotelDbContext db, HotelTransaction tx, HotelClock clock) : HotelController
{
    [HttpGet] public IActionResult DangNhap() => View(new DangNhapForm());
    [HttpPost, EnableRateLimiting("login")]
    public async Task<IActionResult> DangNhap(DangNhapForm f)
    {
        if (!ModelState.IsValid) return View(f);
        var a = await db.TaiKhoans.SingleOrDefaultAsync(x => x.TenDangNhap == f.TenDangNhap.Trim());
        var h = new PasswordHasher<TaiKhoan>();
        if (a is null || !a.TrangThai || h.VerifyHashedPassword(a, a.MatKhau, f.MatKhau) == PasswordVerificationResult.Failed) { ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng, hoặc tài khoản đã khóa."); return View(f); }
        HttpContext.Session.Clear(); HttpContext.Session.SetInt32("MaTaiKhoan", a.MaTaiKhoan); HttpContext.Session.SetString("HoTen", a.HoTen); HttpContext.Session.SetString("VaiTro", a.VaiTro.ToString());
        return a.VaiTro == VaiTro.Admin ? RedirectToAction("Index", "ThongKe") : RedirectToAction("Index", "Phong");
    }
    [HttpPost] public IActionResult DangXuat() { HttpContext.Session.Clear(); return RedirectToAction("Index", "Home"); }
    [SessionGuard(true)] public async Task<IActionResult> Index() => View(await db.TaiKhoans.AsNoTracking().OrderBy(x => x.TenDangNhap).ToListAsync());
    [SessionGuard(true), HttpGet] public IActionResult Create() => View(new TaoTaiKhoanForm());
    [SessionGuard(true), HttpPost]
    public async Task<IActionResult> Create(TaoTaiKhoanForm f)
    {
        if (!ModelState.IsValid) return View(f);
        if (await Attempt(async () => await tx.Run(async () =>
        {
            var name = f.TenDangNhap.Trim(); if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == name)) throw new NghiepVuException("Tên đăng nhập đã tồn tại.");
            var a = new TaiKhoan { TenDangNhap = name, HoTen = f.HoTen.Trim(), Email = f.Email, VaiTro = f.VaiTro }; a.MatKhau = new PasswordHasher<TaiKhoan>().HashPassword(a, f.MatKhauMoi);
            if (f.VaiTro == VaiTro.KhachHang) a.KhachHang = new KhachHang { HoTen = a.HoTen, Email = a.Email, SoDienThoai = f.SoDienThoai, CCCD = f.CCCD, NgayDangKy = clock.Now };
            db.TaiKhoans.Add(a); return 0;
        }))) return RedirectToAction(nameof(Index)); return View(f);
    }
    [SessionGuard(true), HttpGet] public async Task<IActionResult> Edit(int id) { var a = await db.TaiKhoans.FindAsync(id); return a is null ? NotFound() : View(new SuaTaiKhoanForm { Id = id, VaiTro = a.VaiTro, TrangThai = a.TrangThai }); }
    [SessionGuard(true), HttpPost]
    public async Task<IActionResult> Edit(int id, SuaTaiKhoanForm f)
    {
        if (id != f.Id) return BadRequest();
        if (!ModelState.IsValid) return View(f);
        if (await Attempt(async () => await tx.Run(async () =>
        {
            var a = await db.TaiKhoans.Include(x => x.KhachHang).SingleOrDefaultAsync(x => x.MaTaiKhoan == id) ?? throw new NghiepVuException("Tài khoản không tồn tại.");
            if (id == AccountId && (!f.TrangThai || f.VaiTro != VaiTro.Admin)) throw new NghiepVuException("Không tự khóa hoặc hạ quyền tài khoản đang quản trị.");
            if (a.KhachHang != null && f.VaiTro != VaiTro.KhachHang) throw new NghiepVuException("Không đổi vai trò tài khoản đã gắn hồ sơ khách hàng.");
            if (a.KhachHang == null && f.VaiTro == VaiTro.KhachHang) throw new NghiepVuException("Hãy tạo tài khoản khách hàng kèm hồ sơ mới.");
            a.VaiTro = f.VaiTro; a.TrangThai = f.TrangThai; return 0;
        }))) return RedirectToAction(nameof(Index)); return View(f);
    }
}
