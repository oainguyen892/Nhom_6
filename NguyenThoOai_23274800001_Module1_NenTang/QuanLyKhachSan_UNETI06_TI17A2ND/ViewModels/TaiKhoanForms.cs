// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Module 1: tài khoản, đăng nhập, phân quyền và quản lý loại phòng.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public class DangNhapForm
{
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(50, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Tên đăng nhập")] public string TenDangNhap { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(128, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), DataType(DataType.Password), Display(Name = "Mật khẩu")] public string MatKhau { get; set; } = "";
}
public class TaoTaiKhoanForm
{
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(50, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Tên đăng nhập")] public string TenDangNhap { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(100, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Họ tên")] public string HoTen { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), EmailAddress(ErrorMessage = "Email không hợp lệ."), StringLength(150, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(128, MinimumLength = 10, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), DataType(DataType.Password), Display(Name = "Mật khẩu ban đầu (ít nhất 10 ký tự)")] public string MatKhauMoi { get; set; } = "";
    [EnumDataType(typeof(VaiTro), ErrorMessage = "Lựa chọn {0} không hợp lệ."), Display(Name = "Vai trò")] public VaiTro VaiTro { get; set; } = VaiTro.KhachHang;
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^0[0-9]{9}$"), Display(Name = "Điện thoại khách hàng")] public string SoDienThoai { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^[0-9]{12}$"), Display(Name = "CCCD khách hàng")] public string CCCD { get; set; } = "";
}
public class SuaTaiKhoanForm
{
    public int Id { get; set; }
    [EnumDataType(typeof(VaiTro), ErrorMessage = "Lựa chọn {0} không hợp lệ."), Display(Name = "Vai trò")] public VaiTro VaiTro { get; set; }
    [Display(Name = "Hoạt động")] public bool TrangThai { get; set; }
}
