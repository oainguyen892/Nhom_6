// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public class DatPhongForm
{
    [MinLength(1, ErrorMessage = "Chọn ít nhất một phòng."), MaxLength(10)] public int[] PhongIds { get; set; } = [];
    [DataType(DataType.Date), Display(Name = "Ngày nhận")] public DateTime NgayNhan { get; set; }
    [DataType(DataType.Date), Display(Name = "Ngày trả")] public DateTime NgayTra { get; set; }
}
