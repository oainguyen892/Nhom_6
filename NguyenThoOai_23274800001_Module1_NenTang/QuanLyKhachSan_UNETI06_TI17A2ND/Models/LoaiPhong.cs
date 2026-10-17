// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: LoaiPhong, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class LoaiPhong
{
    [Key] public int MaLoaiPhong { get; set; }
    [Required(ErrorMessage = "Nhập tên loại phòng."), StringLength(80, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Tên loại phòng")] public string TenLoaiPhong { get; set; } = "";
    [Range(1, 50, ErrorMessage = "{0} phải từ {1} đến {2}."), Display(Name = "Sức chứa")] public int SoNguoiToiDa { get; set; } = 2;
    [Tien, Range(typeof(decimal), "1", "100000000", ErrorMessage = "{0} phải từ {1} đến {2}."), Display(Name = "Giá một đêm (VND)")] public decimal DonGiaDem { get; set; }
    [StringLength(1000, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Mô tả")] public string? MoTa { get; set; }
    [Display(Name = "Đang phục vụ")] public bool TrangThai { get; set; } = true;
    [ValidateNever] public ICollection<Phong> Phongs { get; set; } = [];
}
