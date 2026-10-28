// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 2: quản lý phòng, tìm kiếm, lọc, sắp xếp và phân trang.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public class PhongQuery
{
    [StringLength(100, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string? Q { get; set; }
    public int? Loai { get; set; }
    [StringLength(30, ErrorMessage = "Độ dài {0} không hợp lệ (tối đa {1} ký tự).")] public string? Toa { get; set; }
    [Range(1, 15, ErrorMessage = "Tầng phải từ 1 đến 15.")] public int? Tang { get; set; }
    [EnumDataType(typeof(TrangThaiPhong), ErrorMessage = "Lựa chọn {0} không hợp lệ.")] public TrangThaiPhong? TrangThai { get; set; }
    [Range(typeof(decimal), "0", "100000000", ErrorMessage = "{0} phải từ {1} đến {2}.")] public decimal? GiaTu { get; set; }
    [Range(typeof(decimal), "0", "100000000", ErrorMessage = "{0} phải từ {1} đến {2}.")] public decimal? GiaDen { get; set; }
    public string Sort { get; set; } = "so";
    public int Page { get; set; } = 1;
}
public class PhongPage
{
    public PhongQuery Query { get; set; } = new();
    public List<Phong> Items { get; set; } = [];
    public List<LoaiPhong> Loais { get; set; } = [];
    public int Total { get; set; }
    public int Pages => Math.Max(1, (Total + 7) / 8);
}
