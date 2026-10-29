// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Module 1: tài khoản, đăng nhập, phân quyền và quản lý loại phòng.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

[SessionGuard(true)]
public class LoaiPhongController(HotelDbContext db, HotelTransaction tx) : HotelController
{
    public async Task<IActionResult> Index() => View(await db.LoaiPhongs.AsNoTracking().OrderBy(x => x.TenLoaiPhong).ToListAsync());
    public async Task<IActionResult> Details(int id) { var m = await db.LoaiPhongs.AsNoTracking().SingleOrDefaultAsync(x => x.MaLoaiPhong == id); return m is null ? NotFound() : View(m); }
    [HttpGet] public IActionResult Create() => View("Form", new LoaiPhong());
    [HttpGet] public async Task<IActionResult> Edit(int id) { var m = await db.LoaiPhongs.AsNoTracking().SingleOrDefaultAsync(x => x.MaLoaiPhong == id); return m is null ? NotFound() : View("Form", m); }
    [HttpPost] public Task<IActionResult> Create([Bind("TenLoaiPhong,SoNguoiToiDa,DonGiaDem,MoTa,TrangThai")] LoaiPhong f) => Save(0, f);
    [HttpPost] public Task<IActionResult> Edit(int id, [Bind("TenLoaiPhong,SoNguoiToiDa,DonGiaDem,MoTa,TrangThai")] LoaiPhong f) => Save(id, f);
    private async Task<IActionResult> Save(int id, LoaiPhong f)
    {
        f.MaLoaiPhong = id;
        if (ModelState.IsValid && await Attempt(async () => await tx.Run(async () =>
        {
            if (await db.LoaiPhongs.AnyAsync(x => x.TenLoaiPhong == f.TenLoaiPhong.Trim() && x.MaLoaiPhong != id)) throw new NghiepVuException("Tên loại phòng đã tồn tại.");
            var m = id == 0 ? new LoaiPhong() : await db.LoaiPhongs.FindAsync(id) ?? throw new NghiepVuException("Loại phòng không tồn tại.");
            if (!f.TrangThai && await db.Phongs.AnyAsync(x => x.MaLoaiPhong == id && x.ChiTiets.Any(d => d.PhieuDatPhong.TrangThai == TrangThaiDat.ChoXacNhan || d.PhieuDatPhong.TrangThai == TrangThaiDat.DaXacNhan || d.PhieuDatPhong.TrangThai == TrangThaiDat.DaNhanPhong))) throw new NghiepVuException("Loại phòng còn lịch đặt hoạt động, chưa thể ngừng phục vụ.");
            m.TenLoaiPhong = f.TenLoaiPhong.Trim(); m.SoNguoiToiDa = f.SoNguoiToiDa; m.DonGiaDem = f.DonGiaDem; m.MoTa = f.MoTa; m.TrangThai = f.TrangThai; if (id == 0) db.LoaiPhongs.Add(m); return 0;
        }))) return RedirectToAction(nameof(Index)); return View("Form", f);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await Attempt(async () => await tx.Run(async () => { var m = await db.LoaiPhongs.FindAsync(id) ?? throw new NghiepVuException("Không tìm thấy loại phòng."); if (await db.Phongs.AnyAsync(x => x.MaLoaiPhong == id)) throw new NghiepVuException("Không xóa loại phòng đã có phòng liên quan."); db.LoaiPhongs.Remove(m); return 0; }))) TempData["Error"] = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
        return RedirectToAction(nameof(Index));
    }
}
