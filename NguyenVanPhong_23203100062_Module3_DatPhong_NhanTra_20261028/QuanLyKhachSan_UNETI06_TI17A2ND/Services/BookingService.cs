using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class BookingService(HotelDbContext db, HotelTransaction tx, HotelClock clock, GiaPhongService giaPhong, EmailQueueService email)
{
    public Task<int> Book(int accountId, DatPhongForm f) => tx.Run(async () =>
    {
        if (f.NgayNhan != f.NgayNhan.Date || f.NgayTra != f.NgayTra.Date || f.NgayNhan < clock.Today) throw new NghiepVuException("Ngày nhận không được trước hôm nay; chỉ nhập ngày, không nhập giờ.");
        var nights = BookingRules.Nights(f.NgayNhan, f.NgayTra);
        if (f.SoNguoi is < 1 or > 20) throw new NghiepVuException("Số người từ 1 đến 20.");
        if (f.PhongIds.Length is < 1 or > 10 || f.PhongIds.Distinct().Count() != f.PhongIds.Length) throw new NghiepVuException("Chọn từ 1 đến 10 phòng khác nhau.");
        var kh = await db.KhachHangs.Include(x => x.TaiKhoan).SingleOrDefaultAsync(x => x.MaTaiKhoan == accountId && x.TrangThai && x.TaiKhoan.TrangThai && x.TaiKhoan.VaiTro == VaiTro.KhachHang) ?? throw new NghiepVuException("Khách hàng không hoạt động.");
        var rooms = await db.Phongs.Include(x => x.LoaiPhong).Where(x => f.PhongIds.Contains(x.MaPhong)).ToListAsync();
        if (rooms.Count != f.PhongIds.Length || rooms.Any(x => x.TrangThai == TrangThaiPhong.TamNgung || !x.LoaiPhong.TrangThai)) throw new NghiepVuException("Phòng hoặc loại phòng đã ngừng phục vụ.");
        if (rooms.Any(x => x.LoaiPhong.SoNguoiToiDa < f.SoNguoi)) throw new NghiepVuException("Số người vượt quá sức chứa của phòng đã chọn.");
        if (await db.ChiTiets.AnyAsync(x => f.PhongIds.Contains(x.MaPhong) && x.PhieuDatPhong.TrangThai != TrangThaiDat.DaHuy && x.PhieuDatPhong.TrangThai != TrangThaiDat.DaTraPhong && x.PhieuDatPhong.NgayNhanDuKien < f.NgayTra && x.PhieuDatPhong.NgayTraDuKien > f.NgayNhan)) throw new NghiepVuException("Phòng đã có lịch đặt trong khoảng ngày này.");
        var p = new PhieuDatPhong { MaDatPhong = $"BK{clock.Now:yyyyMMddHHmmss}{Guid.NewGuid():N}"[..24].ToUpperInvariant(), MaKhachHang = kh.MaKhachHang, KhachHang = kh, NgayDat = clock.Now, NgayNhanDuKien = f.NgayNhan, NgayTraDuKien = f.NgayTra, SoNguoi = f.SoNguoi, TrangThai = TrangThaiDat.ChoXacNhan };
        foreach (var r in rooms) p.ChiTiets.Add(new ChiTietDatPhong { MaPhong = r.MaPhong, DonGiaDem = r.DonGiaDem, SoDem = nights, ThanhTien = giaPhong.TinhTong(r.DonGiaDem, f.NgayNhan, f.NgayTra) });
        p.TongTienDuKien = p.ChiTiets.Sum(x => x.ThanhTien); p.TienCoc = Math.Round(p.TongTienDuKien * 0.20m, 2); db.PhieuDats.Add(p); await db.SaveChangesAsync(); email.DatLich(p, LoaiEmailThongBao.DatPhongMoi); return p.MaPhieuDat;
    });
    public Task<int> Move(int id, TrangThaiDat target) => tx.Run(async () =>
    {
        var p = await db.PhieuDats.Include(x => x.KhachHang).Include(x => x.ChiTiets).ThenInclude(x => x.Phong).ThenInclude(x => x.LoaiPhong).SingleOrDefaultAsync(x => x.MaPhieuDat == id) ?? throw new NghiepVuException("Không tìm thấy phiếu.");
        if (!BookingRules.CanMove(p.TrangThai, target)) throw new NghiepVuException("Không được chuyển trạng thái theo thứ tự này.");
        if (target == TrangThaiDat.DaNhanPhong) { if (clock.Today < p.NgayNhanDuKien || clock.Today >= p.NgayTraDuKien) throw new NghiepVuException("Chỉ nhận phòng trong khoảng ngày đã đặt."); foreach (var d in p.ChiTiets) { if (d.Phong.TrangThai != TrangThaiPhong.SanSang || !d.Phong.LoaiPhong.TrangThai) throw new NghiepVuException("Phòng chưa sẵn sàng để nhận."); d.Phong.TrangThai = TrangThaiPhong.DangSuDung; } p.NgayNhanPhongThucTe = clock.Now; }
        if (target == TrangThaiDat.DaTraPhong) { foreach (var d in p.ChiTiets) d.Phong.TrangThai = TrangThaiPhong.SanSang; p.NgayTraPhongThucTe = clock.Now; var kh = p.KhachHang; var completed = await db.PhieuDats.CountAsync(x => x.MaKhachHang == p.MaKhachHang && x.TrangThai == TrangThaiDat.DaTraPhong) + 1; kh.HangThanhVien = completed >= 10 ? HangKhachHang.KimCuong : completed >= 6 ? HangKhachHang.Vang : completed >= 3 ? HangKhachHang.Bac : HangKhachHang.Dong; }
        p.TrangThai = target; if (target == TrangThaiDat.DaXacNhan) email.DatLich(p, LoaiEmailThongBao.XacNhanDatPhong); if (target == TrangThaiDat.DaHuy) email.DatLich(p, LoaiEmailThongBao.HuyDatPhong); return id;
    });
}
