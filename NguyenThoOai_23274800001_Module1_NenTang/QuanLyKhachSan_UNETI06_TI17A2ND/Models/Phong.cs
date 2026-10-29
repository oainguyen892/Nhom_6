// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: Phong, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class Phong
{
    [Key] public int MaPhong { get; set; }
    [Required(ErrorMessage = "Nhập số phòng."), StringLength(20, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Số phòng")] public string SoPhong { get; set; } = "";
    [Range(1, int.MaxValue, ErrorMessage = "{0} phải từ {1} đến {2}."), Display(Name = "Loại phòng")] public int MaLoaiPhong { get; set; }
    [Range(0, 100, ErrorMessage = "{0} phải từ {1} đến {2}."), Display(Name = "Tầng")] public int Tang { get; set; } = 1;
    [Tien, Range(typeof(decimal), "1", "100000000", ErrorMessage = "{0} phải từ {1} đến {2}."), Display(Name = "Giá một đêm (VND)")] public decimal DonGiaDem { get; set; }
    [StringLength(1000, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự)."), Display(Name = "Mô tả")] public string? MoTa { get; set; }
    [EnumDataType(typeof(TrangThaiPhong), ErrorMessage = "Lựa chọn {0} không hợp lệ."), Display(Name = "Trạng thái")] public TrangThaiPhong TrangThai { get; set; }
    [ValidateNever] public LoaiPhong LoaiPhong { get; set; } = null!;
    [ValidateNever] public ICollection<ChiTietDatPhong> ChiTiets { get; set; } = [];
}
