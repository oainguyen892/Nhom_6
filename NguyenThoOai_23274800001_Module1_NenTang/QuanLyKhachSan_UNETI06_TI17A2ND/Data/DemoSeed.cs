// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - DbContext, quan hệ, Code First/Migration và dữ liệu mẫu dùng chung.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Data;

public static class DemoSeed
{
    public const string Password = "Demo@Nhom6_2026";
    public static async Task Run(HotelDbContext db, HotelClock clock)
    {
        await new HotelTransaction(db).Run(async () =>
        {
            var hasher = new PasswordHasher<TaiKhoan>();
            for (var i = 0; i <= 15; i++)
            {
                var name = i == 0 ? "admin" : $"khach{i:00}";
                if (await db.TaiKhoans.AnyAsync(x => x.TenDangNhap == name)) continue;
                var a = new TaiKhoan { TenDangNhap = name, HoTen = i == 0 ? "Quản trị demo" : $"Khách mẫu {i:00}", Email = $"{name}@example.com", VaiTro = i == 0 ? VaiTro.Admin : VaiTro.KhachHang }; a.MatKhau = hasher.HashPassword(a, Password);
                if (i > 0) a.KhachHang = new KhachHang { HoTen = a.HoTen, Email = a.Email, SoDienThoai = $"090{i:0000000}", CCCD = $"001{i:000000000}", NgaySinh = new DateTime(1995, 1, Math.Min(i, 28)), GioiTinh = i % 2 == 0 ? "Nam" : "Nữ", DiaChi = "Hà Nội", NgayDangKy = clock.Now.AddMonths(-6) };
                db.TaiKhoans.Add(a);
            }
            await db.SaveChangesAsync();
            string[] types = ["Tiêu chuẩn", "Superior", "Deluxe", "Gia đình", "Suite"];
            for (var i = 0; i < 5; i++) if (!await db.LoaiPhongs.AnyAsync(x => x.TenLoaiPhong == types[i])) db.LoaiPhongs.Add(new LoaiPhong { TenLoaiPhong = types[i], SoNguoiToiDa = i + 1, DonGiaDem = 400000 + i * 250000, MoTa = "Không gian tiện nghi, Wi-Fi và bữa sáng." });
            await db.SaveChangesAsync();
            var ls = await db.LoaiPhongs.OrderBy(x => x.MaLoaiPhong).Take(5).ToListAsync();
            for (var i = 0; i < 20; i++) { var no = $"{i / 4 + 1}{i % 4 + 1:00}"; if (!await db.Phongs.AnyAsync(x => x.SoPhong == no)) db.Phongs.Add(new Phong { SoPhong = no, Tang = i / 4 + 1, MaLoaiPhong = ls[i % 5].MaLoaiPhong, DonGiaDem = ls[i % 5].DonGiaDem, MoTa = "Phòng thoáng, đầy đủ tiện nghi.", TrangThai = i == 19 ? TrangThaiPhong.TamNgung : TrangThaiPhong.SanSang }); }
            await db.SaveChangesAsync();
            // Only the initial, empty booking dataset receives demo history.
            if (!await db.PhieuDats.AnyAsync())
            {
                var rooms = await db.Phongs.OrderBy(x => x.MaPhong).Take(18).ToListAsync(); var customers = await db.KhachHangs.OrderBy(x => x.MaKhachHang).Take(15).ToListAsync();
                for (var i = 0; i < 18; i++)
                {
                    var state = (TrangThaiDat)(i % 5); var start = state == TrangThaiDat.DaTraPhong ? clock.Today.AddMonths(-1 - i % 3) : state == TrangThaiDat.DaNhanPhong ? clock.Today : clock.Today.AddDays(7 + i);
                    var bookedAt = start < clock.Today ? start.AddDays(-3) : clock.Today.AddDays(-1 - i % 3);
                    var r = rooms[i]; var p = new PhieuDatPhong { MaKhachHang = customers[i % 15].MaKhachHang, NgayDat = bookedAt, NgayNhanDuKien = start, NgayTraDuKien = start.AddDays(2), TongTienDuKien = r.DonGiaDem * 2, TrangThai = state };
                    p.ChiTiets.Add(new ChiTietDatPhong { MaPhong = r.MaPhong, DonGiaDem = r.DonGiaDem, SoDem = 2, ThanhTien = r.DonGiaDem * 2 }); db.PhieuDats.Add(p);
                    if (state == TrangThaiDat.DaNhanPhong) r.TrangThai = TrangThaiPhong.DangSuDung;
                }
            }
            return 0;
        });
    }
}
