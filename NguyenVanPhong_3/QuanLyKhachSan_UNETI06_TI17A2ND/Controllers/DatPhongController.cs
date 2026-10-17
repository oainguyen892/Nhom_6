// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

[SessionGuard]
public class DatPhongController(HotelDbContext db, BookingService service, HotelClock clock) : HotelController
{
    private async Task Options() => ViewBag.Phongs = await db.Phongs.AsNoTracking().Include(x => x.LoaiPhong).Where(x => x.TrangThai != TrangThaiPhong.TamNgung && x.LoaiPhong.TrangThai).OrderBy(x => x.SoPhong).ToListAsync();
    [HttpGet] public async Task<IActionResult> Create(int? phongId) { await Options(); return View(new DatPhongForm { PhongIds = phongId.HasValue ? [phongId.Value] : [], NgayNhan = clock.Today, NgayTra = clock.Today.AddDays(1) }); }
    [HttpPost]
    public async Task<IActionResult> Create(DatPhongForm f)
    {
        int id = 0; if (ModelState.IsValid && await Attempt(async () => { id = await service.Book(AccountId, f); })) return RedirectToAction(nameof(Details), new { id }); await Options(); return View(f);
    }
    public async Task<IActionResult> Index(bool lichSu = false) { ViewBag.LichSu = lichSu; var q = db.PhieuDats.AsNoTracking().Where(x => x.KhachHang.MaTaiKhoan == AccountId); if (lichSu) q = q.Where(x => x.TrangThai == TrangThaiDat.DaTraPhong); return View(await q.OrderByDescending(x => x.NgayDat).ToListAsync()); }
    public async Task<IActionResult> Details(int id) { var p = await db.PhieuDats.AsNoTracking().Include(x => x.ChiTiets).ThenInclude(x => x.Phong).SingleOrDefaultAsync(x => x.MaPhieuDat == id && x.KhachHang.MaTaiKhoan == AccountId); return p is null ? NotFound() : View(p); }
}
