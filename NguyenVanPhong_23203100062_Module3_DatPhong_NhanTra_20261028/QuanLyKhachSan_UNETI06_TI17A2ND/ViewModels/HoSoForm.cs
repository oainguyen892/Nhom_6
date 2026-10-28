// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Văn Phong
// Mã sinh viên: 23203100062
// Nội dung thực hiện: Phạm vi được giao - Module 3: quản lý khách hàng, hồ sơ, đặt phòng và chi tiết.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public class HoSoForm : IValidatableObject
{
    [Required(ErrorMessage = "Nhập họ tên."), StringLength(100, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Họ tên")] public string HoTen { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), EmailAddress(ErrorMessage = "Email không hợp lệ."), StringLength(150, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại gồm 10 chữ số, bắt đầu bằng 0."), Display(Name = "Số điện thoại")] public string SoDienThoai { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^[0-9]{12}$", ErrorMessage = "CCCD gồm đúng 12 chữ số."), Display(Name = "CCCD")] public string CCCD { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), DataType(DataType.Date), Display(Name = "Ngày sinh")] public DateTime? NgaySinh { get; set; }
    [RegularExpression("^(Nam|Nữ|Khác)$"), Display(Name = "Giới tính")] public string GioiTinh { get; set; } = "Khác";
    [StringLength(250, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Địa chỉ")] public string? DiaChi { get; set; }
    public IEnumerable<ValidationResult> Validate(ValidationContext c)
    {
        var clock = c.GetService(typeof(QuanLyKhachSan_UNETI06_TI17A2ND.Services.HotelClock)) as QuanLyKhachSan_UNETI06_TI17A2ND.Services.HotelClock ?? new QuanLyKhachSan_UNETI06_TI17A2ND.Services.HotelClock();
        if (NgaySinh.HasValue && (NgaySinh > clock.Today || NgaySinh < clock.Today.AddYears(-120))) yield return new ValidationResult("Ngày sinh phải trong 120 năm qua.", [nameof(NgaySinh)]);
    }
}
