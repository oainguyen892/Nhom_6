// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc dữ liệu dùng chung: TrangThai, ràng buộc và quan hệ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Models;

public enum VaiTro { [Display(Name = "Admin")] Admin, [Display(Name = "Khách hàng")] KhachHang }
public enum TrangThaiPhong { [Display(Name = "Sẵn sàng")] SanSang, [Display(Name = "Đang sử dụng")] DangSuDung, [Display(Name = "Tạm ngừng")] TamNgung }
public enum TrangThaiDat { [Display(Name = "Chờ xác nhận")] ChoXacNhan, [Display(Name = "Đã xác nhận")] DaXacNhan, [Display(Name = "Đã nhận phòng")] DaNhanPhong, [Display(Name = "Đã trả phòng")] DaTraPhong, [Display(Name = "Đã hủy")] DaHuy }
public static class HienThi
{
    public static string Ten(this Enum e) => e.GetType().GetMember(e.ToString())[0].GetCustomAttributes(typeof(DisplayAttribute), false).Cast<DisplayAttribute>().FirstOrDefault()?.Name ?? e.ToString();
    public static string Vnd(this decimal n) => n.ToString(n % 1 == 0 ? "N0" : "N2", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " ₫";
}
