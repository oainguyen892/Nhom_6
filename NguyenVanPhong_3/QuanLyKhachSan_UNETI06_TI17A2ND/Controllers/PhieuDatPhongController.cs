// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 4: quản lý phiếu, xác nhận/hủy và nhận/trả phòng.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

[SessionGuard(true)]
public class PhieuDatPhongController(HotelDbContext db, BookingService service) : HotelController
{
    public async Task<IActionResult> Index(string? q, TrangThaiDat? trangThai, DateTime? ngayNhan)
    {
        ViewBag.Q = q; ViewBag.TrangThai = trangThai; ViewBag.NgayNhan = ngayNhan;
        var query = db.PhieuDats.AsNoTracking().Include(x => x.KhachHang).AsQueryable();
        if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.KhachHang.HoTen.Contains(q));
        if (trangThai.HasValue) query = query.Where(x => x.TrangThai == trangThai);
        if (ngayNhan.HasValue) query = query.Where(x => x.NgayNhanDuKien == ngayNhan.Value.Date);
        return View(await query.OrderByDescending(x => x.NgayDat).ToListAsync());
    }
    public async Task<IActionResult> Details(int id) { var p = await db.PhieuDats.AsNoTracking().Include(x => x.KhachHang).Include(x => x.ChiTiets).ThenInclude(x => x.Phong).SingleOrDefaultAsync(x => x.MaPhieuDat == id); return p is null ? NotFound() : View(p); }
    [HttpPost]
    public async Task<IActionResult> ChuyenTrangThai(int id, TrangThaiDat target)
    {
        if (!await Attempt(async () => await service.Move(id, target))) TempData["Error"] = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)); return RedirectToAction(nameof(Details), new { id });
    }
}
