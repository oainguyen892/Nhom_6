// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class HotelClock
{
    public virtual DateTime Now => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
    public DateTime Today => Now.Date;
}
