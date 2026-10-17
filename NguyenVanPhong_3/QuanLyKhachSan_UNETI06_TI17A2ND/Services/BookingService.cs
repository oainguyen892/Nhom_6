// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class BookingService(HotelDbContext db, HotelTransaction tx, HotelClock clock)
{
    public Task<int> Book(int accountId, DatPhongForm f) => tx.Run(async () =>
    {
        if (f.NgayNhan != f.NgayNhan.Date || f.NgayTra != f.NgayTra.Date || f.NgayNhan < clock.Today) throw new NghiepVuException("Ngày nhận không được trước hôm nay; chỉ nhập ngày, không nhập giờ.");
        var nights = BookingRules.Nights(f.NgayNhan, f.NgayTra);
        if (f.PhongIds.Length is < 1 or > 10 || f.PhongIds.Distinct().Count() != f.PhongIds.Length) throw new NghiepVuException("Chọn từ 1 đến 10 phòng khác nhau.");
        var kh = await db.KhachHangs.SingleOrDefaultAsync(x => x.MaTaiKhoan == accountId && x.TrangThai && x.TaiKhoan.TrangThai && x.TaiKhoan.VaiTro == VaiTro.KhachHang) ?? throw new NghiepVuException("Khách hàng không hoạt động.");
        var rooms = await db.Phongs.Include(x => x.LoaiPhong).Where(x => f.PhongIds.Contains(x.MaPhong)).ToListAsync();
        if (rooms.Count != f.PhongIds.Length || rooms.Any(x => x.TrangThai == TrangThaiPhong.TamNgung || !x.LoaiPhong.TrangThai)) throw new NghiepVuException("Phòng hoặc loại phòng đã ngừng phục vụ.");
        if (await db.ChiTiets.AnyAsync(x => f.PhongIds.Contains(x.MaPhong) && x.PhieuDatPhong.TrangThai != TrangThaiDat.DaHuy && x.PhieuDatPhong.TrangThai != TrangThaiDat.DaTraPhong && x.PhieuDatPhong.NgayNhanDuKien < f.NgayTra && x.PhieuDatPhong.NgayTraDuKien > f.NgayNhan)) throw new NghiepVuException("Phòng đã có lịch đặt trong khoảng ngày này.");
        var p = new PhieuDatPhong { MaKhachHang = kh.MaKhachHang, NgayDat = clock.Now, NgayNhanDuKien = f.NgayNhan, NgayTraDuKien = f.NgayTra, TrangThai = TrangThaiDat.ChoXacNhan };
        foreach (var r in rooms) p.ChiTiets.Add(new ChiTietDatPhong { MaPhong = r.MaPhong, DonGiaDem = r.DonGiaDem, SoDem = nights, ThanhTien = r.DonGiaDem * nights });
        p.TongTienDuKien = p.ChiTiets.Sum(x => x.ThanhTien); db.PhieuDats.Add(p); await db.SaveChangesAsync(); return p.MaPhieuDat;
    });
    public Task<int> Move(int id, TrangThaiDat target) => tx.Run(async () =>
    {
        var p = await db.PhieuDats.Include(x => x.ChiTiets).ThenInclude(x => x.Phong).ThenInclude(x => x.LoaiPhong).SingleOrDefaultAsync(x => x.MaPhieuDat == id) ?? throw new NghiepVuException("Không tìm thấy phiếu.");
        if (!BookingRules.CanMove(p.TrangThai, target)) throw new NghiepVuException("Không được chuyển trạng thái theo thứ tự này.");
        if (target == TrangThaiDat.DaNhanPhong)
        {
            if (clock.Today < p.NgayNhanDuKien || clock.Today >= p.NgayTraDuKien) throw new NghiepVuException("Chỉ nhận phòng trong khoảng ngày đã đặt.");
            foreach (var d in p.ChiTiets)
            {
                if (d.Phong.TrangThai != TrangThaiPhong.SanSang || !d.Phong.LoaiPhong.TrangThai) throw new NghiepVuException("Phòng chưa sẵn sàng để nhận.");
                if (await db.ChiTiets.AnyAsync(x => x.MaPhong == d.MaPhong && x.MaPhieuDat != id && x.PhieuDatPhong.TrangThai == TrangThaiDat.DaNhanPhong)) throw new NghiepVuException("Phòng đang có khách lưu trú.");
                d.Phong.TrangThai = TrangThaiPhong.DangSuDung;
            }
        }
        if (target == TrangThaiDat.DaTraPhong) foreach (var d in p.ChiTiets) d.Phong.TrangThai = TrangThaiPhong.SanSang;
        p.TrangThai = target; return id;
    });
}
