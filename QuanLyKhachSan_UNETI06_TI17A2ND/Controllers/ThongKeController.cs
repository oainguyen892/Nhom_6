// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: Phạm vi được giao - Module 4: Dashboard và các thống kê LINQ.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using QuanLyKhachSan_UNETI06_TI17A2ND.Filters;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

[SessionGuard(true)]
public class ThongKeController(StatisticsService stats) : Controller
{
    public async Task<IActionResult> Index() => View(await stats.Read());
}
