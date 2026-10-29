// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: TaiKhoan, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class TaiKhoan
{
    [Key] public int MaTaiKhoan { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(50, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string TenDangNhap { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}.")] public string MatKhau { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(100, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string HoTen { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), EmailAddress(ErrorMessage = "Email không hợp lệ."), StringLength(150, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string Email { get; set; } = "";
    public VaiTro VaiTro { get; set; } = VaiTro.KhachHang;
    public bool TrangThai { get; set; } = true;
    [ValidateNever] public KhachHang? KhachHang { get; set; }
}
