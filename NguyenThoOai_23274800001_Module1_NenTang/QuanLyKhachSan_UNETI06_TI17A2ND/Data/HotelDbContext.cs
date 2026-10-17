// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - DbContext, quan hệ, Code First/Migration và dữ liệu mẫu dùng chung.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSan_UNETI06_TI17A2ND.Models;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Data;

public class HotelDbContext(DbContextOptions<HotelDbContext> options) : DbContext(options)
{
    public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
    public DbSet<LoaiPhong> LoaiPhongs => Set<LoaiPhong>();
    public DbSet<Phong> Phongs => Set<Phong>();
    public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
    public DbSet<PhieuDatPhong> PhieuDats => Set<PhieuDatPhong>();
    public DbSet<ChiTietDatPhong> ChiTiets => Set<ChiTietDatPhong>();
    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<TaiKhoan>().HasIndex(x => x.TenDangNhap).IsUnique();
        b.Entity<LoaiPhong>().HasIndex(x => x.TenLoaiPhong).IsUnique();
        b.Entity<Phong>().HasIndex(x => x.SoPhong).IsUnique();
        b.Entity<KhachHang>().HasOne(x => x.TaiKhoan).WithOne(x => x.KhachHang).HasForeignKey<KhachHang>(x => x.MaTaiKhoan).OnDelete(DeleteBehavior.Restrict);
        b.Entity<Phong>().HasOne(x => x.LoaiPhong).WithMany(x => x.Phongs).HasForeignKey(x => x.MaLoaiPhong).OnDelete(DeleteBehavior.Restrict);
        b.Entity<PhieuDatPhong>().HasOne(x => x.KhachHang).WithMany(x => x.PhieuDats).HasForeignKey(x => x.MaKhachHang).OnDelete(DeleteBehavior.Restrict);
        b.Entity<ChiTietDatPhong>().HasKey(x => new { x.MaPhieuDat, x.MaPhong });
        b.Entity<ChiTietDatPhong>().HasOne(x => x.PhieuDatPhong).WithMany(x => x.ChiTiets).HasForeignKey(x => x.MaPhieuDat).OnDelete(DeleteBehavior.Cascade);
        b.Entity<ChiTietDatPhong>().HasOne(x => x.Phong).WithMany(x => x.ChiTiets).HasForeignKey(x => x.MaPhong).OnDelete(DeleteBehavior.Restrict);
        b.Entity<ChiTietDatPhong>().HasIndex(x => x.MaPhong);
        foreach (var p in b.Model.GetEntityTypes().SelectMany(e => e.GetProperties()).Where(p => p.ClrType == typeof(decimal))) { p.SetPrecision(18); p.SetScale(2); }
        b.Entity<LoaiPhong>().ToTable(t => { t.HasCheckConstraint("CK_LoaiPhong_Gia", "[DonGiaDem]>0 AND [SoNguoiToiDa]>0"); });
        b.Entity<Phong>().ToTable(t => t.HasCheckConstraint("CK_Phong_GiaTang", "[DonGiaDem]>0 AND [Tang]>=0 AND [TrangThai] BETWEEN 0 AND 2"));
        b.Entity<PhieuDatPhong>().ToTable(t => t.HasCheckConstraint("CK_Phieu_NgayTien", "[NgayTraDuKien]>[NgayNhanDuKien] AND [TongTienDuKien]>0 AND [TrangThai] BETWEEN 0 AND 4"));
        b.Entity<ChiTietDatPhong>().ToTable(t => t.HasCheckConstraint("CK_ChiTiet_Tien", "[SoDem]>0 AND [DonGiaDem]>0 AND [ThanhTien]=[SoDem]*[DonGiaDem]"));
    }
}
