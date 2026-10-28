// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

[SessionGuard]
public class KhachHangController(HotelDbContext db, HotelTransaction tx) : HotelController
{
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy, VaiTro.LeTan)] public async Task<IActionResult> Index(string? q) { ViewBag.Q = q; var query = db.KhachHangs.AsNoTracking(); if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.HoTen.Contains(q) || x.SoDienThoai.Contains(q) || x.CCCD.Contains(q)); return View(await query.OrderBy(x => x.HoTen).ToListAsync()); }
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy, VaiTro.LeTan)] public async Task<IActionResult> Details(int id) { var k = await db.KhachHangs.AsNoTracking().Include(x => x.PhieuDats).SingleOrDefaultAsync(x => x.MaKhachHang == id); return k is null ? NotFound() : View(k); }
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpPost]
    public async Task<IActionResult> TrangThai(int id, bool active)
    {
        if (!await Attempt(async () => await tx.Run(async () => { var k = await db.KhachHangs.FindAsync(id) ?? throw new NghiepVuException("Khách hàng không tồn tại."); k.TrangThai = active; return 0; }))) TempData["Error"] = "Không cập nhật được khách hàng."; return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> HoSo()
    {
        var k = await db.KhachHangs.AsNoTracking().SingleOrDefaultAsync(x => x.MaTaiKhoan == AccountId); if (k is null) return NotFound();
        return View(new HoSoForm { HoTen = k.HoTen, Email = k.Email, NgaySinh = k.NgaySinh, GioiTinh = k.GioiTinh ?? "Khác", SoDienThoai = k.SoDienThoai, CCCD = k.CCCD, DiaChi = k.DiaChi });
    }
    [HttpPost]
    public async Task<IActionResult> HoSo(HoSoForm f)
    {
        if (ModelState.IsValid && await Attempt(async () => await tx.Run(async () =>
        {
            var k = await db.KhachHangs.Include(x => x.TaiKhoan).SingleOrDefaultAsync(x => x.MaTaiKhoan == AccountId) ?? throw new NghiepVuException("Không tìm thấy hồ sơ.");
            k.HoTen = f.HoTen.Trim(); k.Email = f.Email; k.NgaySinh = f.NgaySinh; k.GioiTinh = f.GioiTinh; k.SoDienThoai = f.SoDienThoai; k.CCCD = f.CCCD; k.DiaChi = f.DiaChi; k.TaiKhoan.HoTen = k.HoTen; k.TaiKhoan.Email = k.Email; return 0;
        }))) return RedirectToAction(nameof(HoSo)); return View(f);
    }
}
