// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 4: Dashboard và các thống kê LINQ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public record StatisticRow(string Label, int Count, decimal Amount = 0);
public class StatisticsPage
{
    public Dictionary<string, int> Totals { get; set; } = [];
    public List<StatisticRow> RoomsByType { get; set; } = [];
    public List<StatisticRow> BookingsByState { get; set; } = [];
    public List<StatisticRow> BookingsByRoom { get; set; } = [];
    public List<StatisticRow> TopTypes { get; set; } = [];
    public List<StatisticRow> BookingsByCustomer { get; set; } = [];
    public List<StatisticRow> Revenue { get; set; } = [];
}
