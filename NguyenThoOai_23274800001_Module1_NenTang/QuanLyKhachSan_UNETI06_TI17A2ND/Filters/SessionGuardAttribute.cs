// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class SessionGuardAttribute(bool adminOnly = false) : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext c)
    {
        var id = c.HttpContext.Session.GetInt32("MaTaiKhoan");
        var db = c.HttpContext.RequestServices.GetRequiredService<HotelDbContext>();
        var a = id.HasValue ? await db.TaiKhoans.AsNoTracking().SingleOrDefaultAsync(x => x.MaTaiKhoan == id && x.TrangThai) : null;
        if (a is null) { c.HttpContext.Session.Clear(); c.Result = new RedirectToActionResult("DangNhap", "TaiKhoan", null); return; }
        if (adminOnly && a.VaiTro != VaiTro.Admin) { c.Result = new StatusCodeResult(403); return; }
        c.HttpContext.Items["Account"] = a;
        c.HttpContext.Session.SetString("VaiTro", a.VaiTro.ToString()); c.HttpContext.Session.SetString("HoTen", a.HoTen);
    }
}
