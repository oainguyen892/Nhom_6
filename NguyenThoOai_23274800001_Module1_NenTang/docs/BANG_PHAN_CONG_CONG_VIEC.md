# Phân công công việc — Nhóm 06, TI17A2ND

Đề tài 05. Nhóm gồm 03 sinh viên: Nguyễn Thọ Oai (23274800001, trưởng nhóm), Đặng Tùng Dương (23274800005), Nguyễn Văn Phong (23203100062). Oai phụ trách Module 1/nền tảng; Dương phụ trách Module 2 và Dashboard/thống kê; Phong phụ trách Module 3 và phiếu/nhận/trả thuộc Module 4.

Đã điền người được giao phụ trách từng công việc và từng file. Đây là phân công của nhóm, không phải xác nhận từng người đã trực tiếp hoàn thành. Chỉ thành viên mới xác nhận việc review/sửa/kiểm thử của mình; không ký nhận hoặc tạo lịch sử Git thay người khác.

## Công việc và sản phẩm

| STT | Công việc | Người phụ trách | Sản phẩm / tiêu chí nghiệm thu |
|---|---|---|---|
| 1 | Kiến trúc MVC, project/namespace và cấu hình | Nguyễn Thọ Oai — 23274800001 | Program, csproj, cấu hình chung; chạy được Visual Studio |
| 2 | 6 Entity, PK/FK, DbContext, Migration, seed | Nguyễn Thọ Oai — 23274800001 | Models/Data/Migrations; DB SQL Server mới, seed idempotent |
| 3 | Đăng nhập/đăng xuất, tài khoản khóa và Controller guard | Nguyễn Thọ Oai — 23274800001 | TaiKhoanController, SessionGuard, TaiKhoanForms, Views/TaiKhoan |
| 4 | CRUD/chi tiết/trạng thái loại phòng | Nguyễn Thọ Oai — 23274800001 | LoaiPhongController và Views/LoaiPhong; validation, unique và FK |
| 5 | CRUD/chi tiết/trạng thái phòng | Đặng Tùng Dương — 23274800005 | PhongController, Views/Phong; giữ giá và lịch sử phiếu cũ |
| 6 | Search/filter kết hợp, 6 sort, phân trang server | Đặng Tùng Dương — 23274800005 | RoomSearch, PhongQuery; Count/Skip/Take và giữ query |
| 7 | Dashboard 8 chỉ số và 6 thống kê LINQ | Đặng Tùng Dương — 23274800005 | ThongKeController, StatisticsService/Page, Views/ThongKe; đối soát DB |
| 8 | Quản lý/tìm kiếm/trạng thái khách hàng | Nguyễn Văn Phong — 23203100062 | KhachHangController, Views/KhachHang |
| 9 | Hồ sơ cá nhân và validation/ownership | Nguyễn Văn Phong — 23203100062 | HoSoForm; lấy chủ hồ sơ từ Session, chặn ID giả |
| 10 | Đặt nhiều phòng, số đêm, snapshot và tổng tiền | Nguyễn Văn Phong — 23203100062 | DatPhongController/Form, BookingService/Rules, Views/DatPhong |
| 11 | Trùng lịch, đồng thời và rollback nhiều phòng | Nguyễn Văn Phong — 23203100062 | BookingService phối hợp HotelTransaction do Oai giữ; test race condition |
| 12 | Phiếu cá nhân, chi tiết, lịch sử lưu trú | Nguyễn Văn Phong — 23203100062 | Ownership tại SQL; Views/DatPhong, Shared/_BookingList và _BookingDetails |
| 13 | Admin tìm/lọc phiếu, xác nhận/hủy/nhận/trả | Nguyễn Văn Phong — 23203100062 | PhieuDatPhongController/Views; chuyển trạng thái hợp lệ |
| 14 | Giao dịch, đồng hồ và decimal binder dùng chung | Nguyễn Thọ Oai — 23274800001 | HotelTransaction, HotelClock, DecimalBinder; phối hợp Dương/Phong |
| 15 | Review/sửa Module 1 và nền tảng | Nguyễn Thọ Oai — 23274800001 | Tự đọc/sửa và ghi thay đổi thực tế; chạy auth/CRUD/migration tests |
| 16 | Review/sửa Module 2 và thống kê | Đặng Tùng Dương — 23274800005 | Tự đọc/sửa và ghi thay đổi thực tế; chạy filter/page/decimal/stats tests |
| 17 | Review/sửa Module 3 và nhận/trả | Nguyễn Văn Phong — 23203100062 | Tự đọc/sửa và ghi thay đổi thực tế; chạy ownership/booking/state/concurrency tests |
| 18 | Header và xác nhận phần việc Oai | Nguyễn Thọ Oai — 23274800001 | Đã điền tên theo phân công; Oai tự xác nhận nội dung thực hiện sau review |
| 19 | Header và xác nhận phần việc Dương | Đặng Tùng Dương — 23274800005 | Đã điền tên theo phân công; Dương tự xác nhận nội dung thực hiện sau review |
| 20 | Header và xác nhận phần việc Phong | Nguyễn Văn Phong — 23203100062 | Đã điền tên theo phân công; Phong tự xác nhận nội dung thực hiện sau review |
| 21 | Automated tests, regression và xử lý tích hợp | Nguyễn Thọ Oai — 23274800001 | tests/QuanLyKhachSan.Tests; Dương đối soát số liệu, Phong đối soát booking |
| 22 | Branch/commit/PR Module 1 và tích hợp | Nguyễn Thọ Oai — 23274800001 | feature/oai-taikhoan-tichhop; docs/github/OAI_PR_DU_THAO.md |
| 23 | Branch/commit/PR Module 2 và thống kê | Đặng Tùng Dương — 23274800005 | feature/duong-phong-thongke; docs/github/DUONG_PR_DU_THAO.md |
| 24 | Branch/commit/PR Module 3 và nhận/trả | Nguyễn Văn Phong — 23203100062 | feature/phong-datphong; docs/github/PHONG_PR_DU_THAO.md |
| 25 | Liên hệ giảng viên xác nhận nhóm 03 người | Nguyễn Thọ Oai — 23274800001 | DE_NGHI_XAC_NHAN_NHOM_3_NGUOI.md; Dương/Phong cùng xác nhận thông tin |
| 26 | Báo cáo: mục tiêu, phân tích, MVC, CSDL, cài đặt và tổng hợp | Nguyễn Thọ Oai — 23274800001 | BAO_CAO_BAI_TAP_LON_DU_THAO.md; ghép theo mẫu giảng viên khi có |
| 27 | Báo cáo: quản lý phòng, LINQ, Dashboard/thống kê | Đặng Tùng Dương — 23274800005 | Bổ sung giải thích RoomSearch/StatisticsService và số liệu đối soát |
| 28 | Báo cáo: khách, đặt/nhận/trả, overlap/ownership/rollback | Nguyễn Văn Phong — 23203100062 | Bổ sung giải thích BookingService và các ca lỗi |
| 29 | Video: mở đầu, kiến trúc, Module 1, tổng kết | Nguyễn Thọ Oai — 23274800001 | KICH_BAN_VIDEO_VA_BAO_VE.md; tự ghi hình và thuyết minh thật |
| 30 | Video: Module 2, phân trang, Dashboard/thống kê | Đặng Tùng Dương — 23274800005 | Tự ghi hình thao tác và giải thích truy vấn thật |
| 31 | Video: Module 3, đặt/nhận/trả và ownership | Nguyễn Văn Phong — 23203100062 | Tự ghi hình nghiệp vụ và ca lỗi thật |
| 32 | Lắp ghép video, kiểm tra âm thanh và nộp sản phẩm | Nguyễn Thọ Oai — 23274800001 | Dương và Phong xác nhận đoạn của mình; không dựng lời phát biểu/lịch sử giả |
| 33 | Chạy Visual Studio, kiểm Development/Production cục bộ | Nguyễn Thọ Oai — 23274800001 | HUONG_DAN_VISUAL_STUDIO.md, KIEM_TRA_PRODUCTION_CUC_BO.md; Dương/Phong chạy phần mình |
| 34 | Review PR, merge tuần tự, cleanup và chốt 3 ZIP | Nguyễn Thọ Oai — 23274800001 | Dương nhận overlay Module 2; Phong nhận overlay Module 3; kiểm CRC/hash |

## Phụ trách từng file C# và Razor

Header đã có Họ và tên/Mã sinh viên/Nội dung thực hiện dưới dạng phạm vi được giao, kèm dòng chưa xác nhận tác giả thực hiện. Sau review thật, thành viên tự cập nhật trạng thái và nội dung đóng góp cụ thể. Không gắn trách nhiệm cho thư viện bên thứ ba. Bản JSON tương ứng: PHAN_CONG_THEO_FILE.json.

| File | Người phụ trách |
|---|---|
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/DatPhongController.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/HomeController.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/HotelController.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/KhachHangController.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/LoaiPhongController.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/PhieuDatPhongController.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/PhongController.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/TaiKhoanController.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/ThongKeController.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Data/DemoSeed.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Data/HotelDbContext.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Filters/SessionGuardAttribute.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Migrations/20261016074658_InitialHotel.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Migrations/20261016074658_InitialHotel.Designer.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Migrations/HotelDbContextModelSnapshot.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/ChiTietDatPhong.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/KhachHang.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/LoaiPhong.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/PhieuDatPhong.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/Phong.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/TaiKhoan.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/TienAttribute.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Models/TrangThai.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Program.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/BookingRules.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/BookingService.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/DecimalBinder.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/HotelClock.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/HotelTransaction.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/RoomSearch.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Services/StatisticsService.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/DatPhongForm.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/HoSoForm.cs` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/PhongQuery.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/RoomIllustrations.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/StatisticsPage.cs` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/ViewModels/TaiKhoanForms.cs` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/_ViewImports.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/_ViewStart.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/DatPhong/Create.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/DatPhong/Details.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/DatPhong/Index.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Home/Error.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Home/Index.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/KhachHang/Details.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/KhachHang/HoSo.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/KhachHang/Index.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/LoaiPhong/Details.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/LoaiPhong/Form.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/LoaiPhong/Index.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/PhieuDatPhong/Details.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/PhieuDatPhong/Index.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Phong/Details.cshtml` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Phong/Form.cshtml` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Phong/Index.cshtml` | Đặng Tùng Dương — 23274800005 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Shared/_BookingDetails.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Shared/_BookingList.cshtml` | Nguyễn Văn Phong — 23203100062 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Shared/_Layout.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/Shared/_ValidationScriptsPartial.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/TaiKhoan/Create.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/TaiKhoan/DangNhap.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/TaiKhoan/Edit.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/TaiKhoan/Index.cshtml` | Nguyễn Thọ Oai — 23274800001 |
| `QuanLyKhachSan_UNETI06_TI17A2ND/Views/ThongKe/Index.cshtml` | Đặng Tùng Dương — 23274800005 |
| `tests/QuanLyKhachSan.Tests/AdminWorkflowTests.cs` | Nguyễn Thọ Oai — 23274800001 |
| `tests/QuanLyKhachSan.Tests/BookingRulesTests.cs` | Nguyễn Thọ Oai — 23274800001 |
| `tests/QuanLyKhachSan.Tests/DecimalAndRollbackTests.cs` | Nguyễn Thọ Oai — 23274800001 |
| `tests/QuanLyKhachSan.Tests/ProductionSmokeTests.cs` | Nguyễn Thọ Oai — 23274800001 |
| `tests/QuanLyKhachSan.Tests/SqlWebTests.cs` | Nguyễn Thọ Oai — 23274800001 |

## Điểm giao nhau

Oai giữ hợp đồng Entity/DbContext/migration, HotelTransaction/Clock/DecimalBinder, SessionGuard và layout. Dương/Phong không tạo project hoặc DB riêng. Phong dùng giá/phòng do Dương quản lý; Dương thống kê phiếu do Phong xử lý. Shared/_BookingList và _BookingDetails thuộc Phong. Hai gói module không ghi đè source của nhau. Oai điều phối regression sau mỗi merge.

## Xác nhận cá nhân

- [ ] Nguyễn Thọ Oai — 23274800001: tự xác nhận đã review/sửa/chạy Module 1, nền tảng và tích hợp; ghi ngày, nội dung sửa và commit thật sau khi thực hiện.
- [ ] Đặng Tùng Dương — 23274800005: tự xác nhận đã review/sửa/chạy Module 2 và thống kê; ghi ngày, nội dung sửa và commit thật sau khi thực hiện.
- [ ] Nguyễn Văn Phong — 23203100062: tự xác nhận đã review/sửa/chạy Module 3 và nhận/trả; ghi ngày, nội dung sửa và commit thật sau khi thực hiện.

Nhóm 03 người khác đề mẫu 04 người; Nguyễn Thọ Oai phụ trách xin xác nhận giảng viên. Không tự đánh dấu phản hồi, video, commit/PR hoặc bảo vệ là hoàn thành khi chưa có sự kiện thực tế.

Bổ sung giao diện v4: Dương phụ trách ViewModels/RoomIllustrations và 5 ảnh minh họa trong wwwroot/images/rooms. Oai điều phối Noto Sans/CSS/trang chủ. Phong tích hợp thumbnail trong form đặt; không tạo bản sao ảnh trong gói Phong.
