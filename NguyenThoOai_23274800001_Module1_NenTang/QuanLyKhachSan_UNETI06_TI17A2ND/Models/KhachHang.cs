// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: KhachHang, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class KhachHang
{
    [Key] public int MaKhachHang { get; set; }
    public int MaTaiKhoan { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập {0}."), StringLength(100, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string HoTen { get; set; } = "";
    public DateTime? NgaySinh { get; set; }
    [StringLength(20, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string? GioiTinh { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^0[0-9]{9}$")] public string SoDienThoai { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), EmailAddress(ErrorMessage = "Email không hợp lệ."), StringLength(150, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string Email { get; set; } = "";
    [Required(ErrorMessage = "Vui lòng nhập {0}."), RegularExpression(@"^[0-9]{12}$")] public string CCCD { get; set; } = "";
    [StringLength(250, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string? DiaChi { get; set; }
    public DateTime NgayDangKy { get; set; }
    public bool TrangThai { get; set; } = true;
    [ValidateNever] public TaiKhoan TaiKhoan { get; set; } = null!;
    [ValidateNever] public ICollection<PhieuDatPhong> PhieuDats { get; set; } = [];
}
