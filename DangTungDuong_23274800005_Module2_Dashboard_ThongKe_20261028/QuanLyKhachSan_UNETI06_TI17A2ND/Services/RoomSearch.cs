// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 2: quản lý phòng, tìm kiếm, lọc, sắp xếp và phân trang.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class RoomSearch(HotelDbContext db)
{
    public async Task<PhongPage> Find(PhongQuery f)
    {
        var q = db.Phongs.AsNoTracking().Include(x => x.LoaiPhong).AsQueryable();
        if (!string.IsNullOrWhiteSpace(f.Q)) { var s = f.Q.Trim(); q = q.Where(x => x.SoPhong.Contains(s) || x.LoaiPhong.TenLoaiPhong.Contains(s)); }
        if (f.Loai.HasValue) q = q.Where(x => x.MaLoaiPhong == f.Loai);
        if (!string.IsNullOrWhiteSpace(f.Toa)) { var toa = f.Toa.Trim().ToUpper(); q = q.Where(x => x.Toa == toa); }
        if (f.Tang.HasValue) q = q.Where(x => x.Tang == f.Tang);
        if (f.TrangThai.HasValue) q = q.Where(x => x.TrangThai == f.TrangThai);
        if (f.GiaTu.HasValue) q = q.Where(x => x.DonGiaDem >= f.GiaTu);
        if (f.GiaDen.HasValue) q = q.Where(x => x.DonGiaDem <= f.GiaDen);
        var count = await q.CountAsync();
        q = f.Sort switch
        {
            "so_desc" => q.OrderByDescending(x => x.SoPhong),
            "gia" => q.OrderBy(x => x.DonGiaDem).ThenBy(x => x.MaPhong),
            "gia_desc" => q.OrderByDescending(x => x.DonGiaDem).ThenBy(x => x.MaPhong),
            "tang" => q.OrderBy(x => x.Tang).ThenBy(x => x.SoPhong),
            "tang_desc" => q.OrderByDescending(x => x.Tang).ThenBy(x => x.SoPhong),
            _ => q.OrderBy(x => x.Toa).ThenBy(x => x.SoPhong)
        };
        f.Page = Math.Clamp(f.Page, 1, Math.Max(1, (count + 7) / 8));
        return new PhongPage { Query = f, Total = count, Items = await q.Skip((f.Page - 1) * 8).Take(8).ToListAsync(), Loais = await db.LoaiPhongs.AsNoTracking().OrderBy(x => x.TenLoaiPhong).ToListAsync() };
    }
}
