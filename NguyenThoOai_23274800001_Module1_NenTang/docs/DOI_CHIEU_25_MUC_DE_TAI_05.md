# Đối chiếu đúng 25 mục Đề tài 05

Cập nhật rà soát **17/10/2026**, baseline **QLKS06-20261017-v5**: đọc lại toàn bộ 423 đoạn văn trong DOCX, chạy SQL Server/HTTP **78/78 ca đạt**. Chi tiết từng nhóm yêu cầu, lỗi dữ liệu mẫu đã sửa và các giới hạn ở [RA_SOAT_TOAN_BO_20261017.md](RA_SOAT_TOAN_BO_20261017.md). Các mục về minh chứng con người dưới đây vẫn chưa hoàn thành.

Đã trích đọc toàn bộ đoạn văn và bảng trong DOCX người dùng cung cấp, đối chiếu đủ 25 tiêu đề chính và các tiểu mục. Không suy ra số trang từ metadata: DOCX có metadata Pages=1 và không có lastRenderedPageBreak; số trang 16 theo mô tả chưa được xác minh bằng dàn trang. Việc đọc bao gồm toàn bộ nội dung, không chỉ 16 trang đầu. Bản ghi này phân biệt mức hoàn thành kỹ thuật với minh chứng trách nhiệm sinh viên.

Đường dẫn code dưới project QuanLyKhachSan_UNETI06_TI17A2ND; tên test nằm trong tests/QuanLyKhachSan.Tests.

| Mục | Tiêu đề nguyên bản | Trạng thái thực tế | File / minh chứng |
|---|---|---|---|
| 1 | MỤC TIÊU | Đạt kỹ thuật | Program.cs; Controllers, Views; test route và nghiệp vụ |
| 2 | CÔNG NGHỆ SỬ DỤNG | Đạt | csproj net10.0, EF 10.0.13; verification/dotnet-info.txt |
| 3 | QUY MÔ VÀ TỔ CHỨC NHÓM | Cần xác nhận | 3 người thay mẫu 4; BANG_PHAN_CONG_CONG_VIEC.md; tên project thống nhất |
| 4 | PHÂN CHIA CHỨC NĂNG | Đã lập, chờ thành viên xác nhận | BANG_PHAN_CONG_CONG_VIEC.md; Module 4 chia Dương/Phong |
| 5 | MODULE 1 – TÀI KHOẢN, ĐĂNG NHẬP, PHÂN QUYỀN VÀ QUẢN LÝ LOẠI PHÒNG | Đạt kiểm thử | TaiKhoanController, LoaiPhongController, SessionGuard; LoginWrongLockedLogoutAndServerGuards, AccountCreationAndSelfLockProtection |
| 6 | MODULE 2 – QUẢN LÝ VÀ TRA CỨU PHÒNG | Đạt kiểm thử | PhongController, RoomSearch, PhongQuery, Views/Phong; SearchFilterSortPageAndLinks, CrudValidationUniquenessAndForeignKeys |
| 7 | MODULE 3 – QUẢN LÝ KHÁCH HÀNG VÀ ĐẶT PHÒNG | Đạt kiểm thử | KhachHangController, DatPhongController, BookingService; CustomerOwnershipAndAntiforgery, BookingSnapshotAndTamperedPricesIgnored |
| 8 | MODULE 4 – QUẢN LÝ ĐẶT PHÒNG, NHẬN/TRẢ PHÒNG, DASHBOARD VÀ THỐNG KÊ | Đạt kiểm thử | PhieuDatPhongController, ThongKeController, StatisticsService; StateLifecycleUpdatesRoomAndRejectsInvalidMoves, MigrationSeedAndStatisticsReconcile |
| 9 | THIẾT KẾ CƠ SỞ DỮ LIỆU | Đạt | 6 Entity, HotelDbContext, THIET_KE_CSDL.md; SqlConstraintsEnforceUniquenessAndRestrictDeletes |
| 10 | ENTITY FRAMEWORK CORE VÀ MIGRATION | Đạt SQL Server thật | Migrations/InitialHotel; verification/migrations.txt; MigrationSeedAndStatisticsReconcile |
| 11 | YÊU CẦU VỀ ASP.NET CORE MVC | Đạt kiểm thử | Controllers/ViewModels/Views, model binding, Tag Helpers; PublicAndAdminViewsHaveNoDeadRoutes |
| 12 | YÊU CẦU VỀ VALIDATION VÀ XỬ LÝ NGHIỆP VỤ | Đạt phạm vi test | Models và ViewModels, BookingRules, BookingService, HotelTransaction; InvalidBookingsNeverPersist, OverlapAndConcurrentRequestsAreAtomic, MultiRoomRollbackAndMoney |
| 13 | YÊU CẦU VỀ RAZOR VIEW | Đạt kiểm tra mã/HTTP và trình duyệt mẫu | Views, wwwroot; date/textarea/select, message; ảnh minh-chung và test route |
| 14 | TÌM KIẾM, LỌC, SẮP XẾP, PHÂN TRANG VÀ LINQ | Đạt kiểm thử | RoomSearch CountAsync/Skip/Take; Views/Phong/Index Routes; SearchFilterSortPageAndLinks, BookingAdminHttpFiltersAndTransitions |
| 15 | DỮ LIỆU MẪU | Đạt kiểm thử | DemoSeed: 5 loại/20 phòng/15 khách/1 admin/18 phiếu/18 chi tiết; seed chạy lại không trùng |
| 16 | YÊU CẦU GIAO DIỆN | Đạt phạm vi kiểm tra | Trang chủ, khách và admin đủ route; CSS responsive; ảnh thật minh-chung; chưa kiểm mọi trình duyệt |
| 17 | QUY ĐỊNH GHI THÔNG TIN NGƯỜI THỰC HIỆN TRONG MÃ NGUỒN | Chờ thành viên thực hiện | Đã điền header phân công cho từng file; PHAN_CONG_THEO_FILE.json; thành viên còn phải trực tiếp review và xác nhận đóng góp |
| 18 | CHỨC NĂNG NÂNG CAO – KHÔNG BẮT BUỘC | Có hash mật khẩu; mục khác không triển khai | PasswordHasher; không upload/Excel/PDF/AJAX/email; không thay chức năng bắt buộc |
| 19 | QUY ĐỊNH SỬ DỤNG AI | Chờ trách nhiệm/bảo vệ cá nhân | HUONG_DAN_BAO_VE.md; yêu cầu sinh viên hiểu và chịu trách nhiệm, không tạo minh chứng giả |
| 20 | LÀM VIỆC NHÓM VỚI GITHUB | Chưa hoàn thành lịch sử thực tế | Remote đã kiểm tra; chỉ có hướng dẫn, chưa commit/push/PR; mỗi người phải tự thực hiện |
| 21 | KIỂM THỬ VÀ HOÀN THIỆN | Đạt 78/78 ca đã chạy | RA_SOAT_TOAN_BO_20261017.md; verification/audit-20261017/final.trx và audit-20261017-test.txt; nêu giới hạn |
| 22 | SẢN PHẨM CẦN NỘP | Đủ source/gói/docs; báo cáo và video cần hoàn thiện | 3 ZIP và migration/seed; BAO_CAO_BAI_TAP_LON_DU_THAO.md; chưa có video và báo cáo theo mẫu riêng của giảng viên |
| 23 | BẢO VỆ VÀ ĐÁNH GIÁ CÁ NHÂN | Chưa kiểm chứng | HUONG_DAN_BAO_VE.md; từng sinh viên phải trực tiếp bảo vệ |
| 24 | QUY TRÌNH NGHIỆP VỤ TỔNG QUÁT | Đạt kiểm thử nghiệp vụ | test booking/state/profile/SQL; workflow khách và admin trong README |
| 25 | KẾT QUẢ CẦN ĐẠT | Đạt kỹ thuật trong môi trường kiểm thử; chưa đủ minh chứng cá nhân | KET_QUA_KIEM_THU.md, baseline và 3 ZIP; mục 3/17/20/22/23 cần con người xác nhận |

Các tiểu mục bắt buộc đã kiểm kê: 5.1–5.5 (tài khoản, login/logout, Controller guard, loại phòng); 6.1–6.6 (CRUD/search/filter/sort/page); 7.1–7.5 (khách, phiếu, chi tiết, đặt, ownership); 8.1–8.4 (quản lý phiếu, nhận/trả, 8 chỉ số và 6 thống kê); 16.1–16.2 (hai phía); 21.1–21.6 (6 nhóm test); 24.1–24.3 (khách/admin/EF). File lệnh bổ sung 2 gói thành viên, fullsource, kiểm thử đồng thời, không giả tác giả và cleanup được thực hiện theo tài liệu bàn giao.
