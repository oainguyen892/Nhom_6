// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Controllers;

public abstract class HotelController : Controller
{
    protected async Task<bool> Attempt(Func<Task> action)
    {
        try { await action(); TempData["Success"] = "Thao tác thành công."; return true; }
        catch (NghiepVuException e) { ModelState.AddModelError("", e.Message); }
        catch (DbUpdateException) { ModelState.AddModelError("", "Dữ liệu trùng hoặc đang được sử dụng. Tải lại trang và kiểm tra dữ liệu liên quan."); }
        catch (SqlException e) when (e.Number is 51000 or 1205 or -2) { ModelState.AddModelError("", "Hệ thống đang xử lý yêu cầu khác. Vui lòng thử lại."); }
        return false;
    }
    protected int AccountId => HttpContext.Session.GetInt32("MaTaiKhoan") ?? 0;
}
