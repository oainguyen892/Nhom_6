// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 2: quản lý phòng, tìm kiếm, lọc, sắp xếp và phân trang.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Text;

namespace QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;

public static class RoomIllustrations
{
    public static string For(string? typeName)
    {
        var file = typeName?.Normalize(NormalizationForm.FormC).Trim().ToUpperInvariant() switch
        {
            "SUPERIOR" => "superior",
            "DELUXE" => "deluxe",
            "GIA ĐÌNH" => "family",
            "SUITE" => "suite",
            _ => "standard"
        };
        return $"/images/rooms/{file}.png";
    }
}
