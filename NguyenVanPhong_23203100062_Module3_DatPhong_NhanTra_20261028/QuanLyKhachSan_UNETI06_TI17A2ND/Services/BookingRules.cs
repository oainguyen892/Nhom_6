// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public static class BookingRules
{
    public static bool Overlaps(DateTime a, DateTime b, DateTime c, DateTime d) => a < d && b > c;
    public static bool CanMove(TrangThaiDat from, TrangThaiDat to) => (from, to) is
     (TrangThaiDat.ChoXacNhan, TrangThaiDat.DaXacNhan) or
     (TrangThaiDat.ChoXacNhan, TrangThaiDat.DaHuy) or
     (TrangThaiDat.DaXacNhan, TrangThaiDat.DaHuy) or
     (TrangThaiDat.DaXacNhan, TrangThaiDat.DaNhanPhong) or
     (TrangThaiDat.DaNhanPhong, TrangThaiDat.DaTraPhong);
    public static int Nights(DateTime start, DateTime end) { var n = (end.Date - start.Date).Days; if (n <= 0 || n > 365) throw new NghiepVuException("Thời gian ở phải từ 1 đến 365 đêm."); return n; }
}
