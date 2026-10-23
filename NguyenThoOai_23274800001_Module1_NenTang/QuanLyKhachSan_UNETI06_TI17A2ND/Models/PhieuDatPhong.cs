// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: PhieuDatPhong, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class PhieuDatPhong
{
    [Key] public int MaPhieuDat { get; set; }
    public int MaKhachHang { get; set; }
    public DateTime NgayDat { get; set; }
    public DateTime NgayNhanDuKien { get; set; }
    public DateTime NgayTraDuKien { get; set; }
    public decimal TongTienDuKien { get; set; }
    public TrangThaiDat TrangThai { get; set; }
    [ValidateNever] public KhachHang KhachHang { get; set; } = null!;
    [ValidateNever] public ICollection<ChiTietDatPhong> ChiTiets { get; set; } = [];
}
