// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Data;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Data;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public class NghiepVuException(string message) : Exception(message);
public class HotelTransaction(HotelDbContext db)
{
    public async Task<T> Run<T>(Func<Task<T>> work)
    {
        await using var tx = await db.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        // Transaction-owned SQL Server lock serializes inventory mutations across processes.
        await db.Database.ExecuteSqlRawAsync("DECLARE @r int; EXEC @r = sys.sp_getapplock @Resource=N'Nhom6.Hotel.Inventory', @LockMode='Exclusive', @LockOwner='Transaction', @LockTimeout=15000; IF @r < 0 THROW 51000, N'Không lấy được khóa đặt phòng. Vui lòng thử lại.', 1;");
        try
        {
            var result = await work();
            await db.SaveChangesAsync();
            await tx.CommitAsync();
            return result;
        }
        catch
        {
            db.ChangeTracker.Clear();
            throw;
        }
    }
}
