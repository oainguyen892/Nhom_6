// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 4: Dashboard và các thống kê LINQ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class StatisticsService(HotelDbContext db)
{
    public async Task<StatisticsPage> Read()
    {
        // A snapshot of all six reports comes from one serializable read transaction.
        await using var tx = await db.Database.BeginTransactionAsync(System.Data.IsolationLevel.Serializable);
        var p = new StatisticsPage();
        p.Totals["Loại phòng"] = await db.LoaiPhongs.CountAsync(); p.Totals["Phòng"] = await db.Phongs.CountAsync(); p.Totals["Khách hàng"] = await db.KhachHangs.CountAsync(); p.Totals["Phiếu đặt"] = await db.PhieuDats.CountAsync();
        p.Totals["Chờ xác nhận"] = await db.PhieuDats.CountAsync(x => x.TrangThai == TrangThaiDat.ChoXacNhan); p.Totals["Đã xác nhận"] = await db.PhieuDats.CountAsync(x => x.TrangThai == TrangThaiDat.DaXacNhan); p.Totals["Phòng đang sử dụng"] = await db.Phongs.CountAsync(x => x.TrangThai == TrangThaiPhong.DangSuDung); p.Totals["Đã trả phòng"] = await db.PhieuDats.CountAsync(x => x.TrangThai == TrangThaiDat.DaTraPhong);
        p.RoomsByType = await db.LoaiPhongs.OrderBy(x => x.TenLoaiPhong).Select(x => new StatisticRow(x.TenLoaiPhong, x.Phongs.Count)).ToListAsync();
        var states = await db.PhieuDats.GroupBy(x => x.TrangThai).Select(g => new { State = g.Key, Count = g.Count() }).ToListAsync(); p.BookingsByState = Enum.GetValues<TrangThaiDat>().Select(s => new StatisticRow(s.Ten(), states.FirstOrDefault(x => x.State == s)?.Count ?? 0)).ToList();
        p.BookingsByRoom = await db.Phongs.OrderBy(x => x.SoPhong).Select(x => new StatisticRow(x.SoPhong, x.ChiTiets.Count(c => c.PhieuDatPhong.TrangThai != TrangThaiDat.DaHuy))).ToListAsync();
        p.TopTypes = await db.LoaiPhongs.Select(x => new StatisticRow(x.TenLoaiPhong, x.Phongs.SelectMany(r => r.ChiTiets).Count(c => c.PhieuDatPhong.TrangThai != TrangThaiDat.DaHuy))).ToListAsync(); p.TopTypes = p.TopTypes.OrderByDescending(x => x.Count).ThenBy(x => x.Label).ToList();
        p.BookingsByCustomer = await db.KhachHangs.OrderBy(x => x.HoTen).Select(x => new StatisticRow(x.HoTen, x.PhieuDats.Count(c => c.TrangThai != TrangThaiDat.DaHuy))).ToListAsync();
        var revenue = await db.PhieuDats.Where(x => x.TrangThai == TrangThaiDat.DaTraPhong).GroupBy(x => new { x.NgayTraDuKien.Year, x.NgayTraDuKien.Month }).Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count(), Amount = g.Sum(x => x.TongTienDuKien) }).OrderBy(x => x.Year).ThenBy(x => x.Month).ToListAsync();
        p.Revenue = revenue.Select(x => new StatisticRow($"{x.Month:00}/{x.Year}", x.Count, x.Amount)).ToList(); await tx.CommitAsync(); return p;
    }
}
