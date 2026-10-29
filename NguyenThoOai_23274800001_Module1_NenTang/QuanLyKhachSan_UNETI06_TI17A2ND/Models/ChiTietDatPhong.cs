// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: ChiTietDatPhong, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public class ChiTietDatPhong
{
    public int MaPhieuDat { get; set; }
    public int MaPhong { get; set; }
    public decimal DonGiaDem { get; set; }
    public int SoDem { get; set; }
    public decimal ThanhTien { get; set; }
    [ValidateNever] public PhieuDatPhong PhieuDatPhong { get; set; } = null!;
    [ValidateNever] public Phong Phong { get; set; } = null!;
}
