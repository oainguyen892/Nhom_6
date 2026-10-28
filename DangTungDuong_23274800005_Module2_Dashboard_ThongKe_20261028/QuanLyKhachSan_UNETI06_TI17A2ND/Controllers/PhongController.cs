// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 2: quản lý phòng, tìm kiếm, lọc, sắp xếp và phân trang.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

public class PhongController(HotelDbContext db, RoomSearch search, HotelTransaction tx) : HotelController
{
    public async Task<IActionResult> Index([FromQuery] PhongQuery f) { if (f.GiaTu > f.GiaDen) ModelState.AddModelError("", "Giá từ không được lớn hơn giá đến."); return View(await search.Find(f)); }
    public async Task<IActionResult> Details(int id) { var m = await db.Phongs.AsNoTracking().Include(x => x.LoaiPhong).SingleOrDefaultAsync(x => x.MaPhong == id); return m is null ? NotFound() : View(m); }
    private async Task Options() => ViewBag.Loais = new SelectList(await db.LoaiPhongs.OrderBy(x => x.TenLoaiPhong).ToListAsync(), "MaLoaiPhong", "TenLoaiPhong");
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpGet] public async Task<IActionResult> Create() { await Options(); return View("Form", new Phong()); }
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpGet] public async Task<IActionResult> Edit(int id) { var m = await db.Phongs.AsNoTracking().SingleOrDefaultAsync(x => x.MaPhong == id); if (m is null) return NotFound(); await Options(); return View("Form", m); }
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpPost] public Task<IActionResult> Create([Bind("SoPhong,MaLoaiPhong,Toa,Tang,DonGiaDem,MoTa,TrangThai")] Phong f) => Save(0, f);
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpPost] public Task<IActionResult> Edit(int id, [Bind("SoPhong,MaLoaiPhong,Toa,Tang,DonGiaDem,MoTa,TrangThai")] Phong f) => Save(id, f);
    private async Task<IActionResult> Save(int id, Phong f)
    {
        f.SoPhong = f.SoPhong?.Trim() ?? "";
        f.Toa = string.IsNullOrWhiteSpace(f.Toa) ? "A" : f.Toa.Trim().ToUpperInvariant();
        f.MoTa = f.MoTa?.Trim() ?? "";
        f.MaPhong = id;
        if (f.Tang is < 1 or > 15) ModelState.AddModelError(nameof(f.Tang), "Tầng phải từ 1 đến 15.");
        if (ModelState.IsValid && await Attempt(async () => await tx.Run(async () =>
        {
            if (await db.Phongs.AnyAsync(x => x.Toa == f.Toa && x.SoPhong == f.SoPhong && x.MaPhong != id)) throw new NghiepVuException("Số phòng đã tồn tại trong tòa này.");
            var loai = await db.LoaiPhongs.FindAsync(f.MaLoaiPhong) ?? throw new NghiepVuException("Loại phòng không tồn tại.");
            var m = id == 0 ? new Phong() : await db.Phongs.FindAsync(id) ?? throw new NghiepVuException("Phòng không tồn tại.");
            if (id != 0 && (f.MaLoaiPhong != m.MaLoaiPhong || f.SoPhong != m.SoPhong || f.Toa != m.Toa) && await db.ChiTiets.AnyAsync(d => d.MaPhong == id)) throw new NghiepVuException("Phòng có lịch sử đặt; giữ nguyên số phòng và loại để bảo toàn lịch sử.");
            var active = id != 0 && await db.ChiTiets.AnyAsync(d => d.MaPhong == id && (d.PhieuDatPhong.TrangThai == TrangThaiDat.ChoXacNhan || d.PhieuDatPhong.TrangThai == TrangThaiDat.DaXacNhan || d.PhieuDatPhong.TrangThai == TrangThaiDat.DaNhanPhong));
            if (active && (f.MaLoaiPhong != m.MaLoaiPhong || f.TrangThai != m.TrangThai)) throw new NghiepVuException("Phòng còn lịch đặt; không đổi loại/trạng thái thủ công.");
            if (f.TrangThai == TrangThaiPhong.DangSuDung && m.TrangThai != TrangThaiPhong.DangSuDung) throw new NghiepVuException("Trạng thái đang sử dụng chỉ được cập nhật khi nhận phòng.");
            if (!loai.TrangThai && f.TrangThai != TrangThaiPhong.TamNgung) throw new NghiepVuException("Loại phòng ngừng phục vụ; chọn trạng thái tạm ngừng.");
            m.SoPhong = f.SoPhong; m.Toa = f.Toa; m.MaLoaiPhong = f.MaLoaiPhong; m.Tang = f.Tang; m.DonGiaDem = f.DonGiaDem; m.MoTa = f.MoTa; m.TrangThai = f.TrangThai; if (id == 0) db.Phongs.Add(m); return 0;
        }))) return RedirectToAction(nameof(Index)); await Options(); return View("Form", f);
    }
    [SessionGuard(false, VaiTro.Admin, VaiTro.QuanLy), HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await Attempt(async () => await tx.Run(async () => { var m = await db.Phongs.FindAsync(id) ?? throw new NghiepVuException("Không tìm thấy phòng."); if (await db.ChiTiets.AnyAsync(x => x.MaPhong == id)) throw new NghiepVuException("Không xóa phòng có lịch sử đặt; hãy dùng trạng thái tạm ngừng khi hết lịch hoạt động."); db.Phongs.Remove(m); return 0; }))) TempData["Error"] = string.Join(" ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)); return RedirectToAction(nameof(Index));
    }
}
