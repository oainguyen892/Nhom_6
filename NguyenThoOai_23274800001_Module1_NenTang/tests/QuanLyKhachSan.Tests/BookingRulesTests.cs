// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiểm thử tự động, SQL Server/HTTP và tích hợp hệ thống.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.ComponentModel.DataAnnotations;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
using QuanLyKhachSan_UNETI06_TI17A2ND.Services;
using QuanLyKhachSan_UNETI06_TI17A2ND.ViewModels;
namespace QuanLyKhachSan.Tests;

public class BookingRulesTests
{
    [Theory]
    [InlineData(0, 2, 1, 3, true)]
    [InlineData(0, 2, 2, 4, false)]
    [InlineData(0, 5, 1, 2, true)]
    [InlineData(2, 4, 0, 2, false)]
    [InlineData(2, 4, 0, 5, true)]
    [InlineData(0, 1, 3, 4, false)]
    public void HalfOpenIntervals(int a, int b, int c, int d, bool expected) { var today = new DateTime(2026, 10, 16); Assert.Equal(expected, BookingRules.Overlaps(today.AddDays(a), today.AddDays(b), today.AddDays(c), today.AddDays(d))); }
    [Theory][InlineData(1, 1)][InlineData(365, 365)][InlineData(2, 2)] public void Nights(int days, int expected) => Assert.Equal(expected, BookingRules.Nights(new(2026, 1, 1), new DateTime(2026, 1, 1).AddDays(days)));
    [Theory][InlineData(0)][InlineData(-1)][InlineData(366)] public void InvalidNights(int days) => Assert.Throws<NghiepVuException>(() => BookingRules.Nights(new(2026, 1, 1), new DateTime(2026, 1, 1).AddDays(days)));
    public static IEnumerable<object[]> Transitions() { for (var a = 0; a < 5; a++) for (var b = 0; b < 5; b++) yield return new object[] { a, b, (a, b) is (0, 1) or (0, 4) or (1, 4) or (1, 2) or (2, 3) }; }
    [Theory, MemberData(nameof(Transitions))] public void AllTransitions(int from, int to, bool expected) => Assert.Equal(expected, BookingRules.CanMove((TrangThaiDat)from, (TrangThaiDat)to));
    [Theory]
    [InlineData("", 0, 0, false)]
    [InlineData("Deluxe", 2, 100, true)]
    [InlineData("Deluxe", -1, 100, false)]
    [InlineData("Deluxe", 2, -1, false)]
    public void RoomTypeValidation(string name, int people, int price, bool expected) { var f = new LoaiPhong { TenLoaiPhong = name, SoNguoiToiDa = people, DonGiaDem = price }; Assert.Equal(expected, Validator.TryValidateObject(f, new ValidationContext(f), [], true)); }
    [Theory]
    [InlineData("a@b.com", "0901234567", "001234567890", true)]
    [InlineData("bad", "0901234567", "001234567890", false)]
    [InlineData("a@b.com", "1234", "001234567890", false)]
    [InlineData("a@b.com", "0901234567", "123", false)]
    public void ProfileValidation(string email, string phone, string cccd, bool expected) { var f = new HoSoForm { HoTen = "Khách", Email = email, SoDienThoai = phone, CCCD = cccd, NgaySinh = new(2000, 1, 1) }; Assert.Equal(expected, Validator.TryValidateObject(f, new(f), [], true)); }
    [Fact] public void FutureBirthDateRejected() { var f = new HoSoForm { HoTen = "Khách", Email = "a@b.com", SoDienThoai = "0901234567", CCCD = "001234567890", NgaySinh = DateTime.MaxValue }; Assert.False(Validator.TryValidateObject(f, new(f), [], true)); }
}
