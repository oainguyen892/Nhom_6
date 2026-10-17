# Báo cáo bài tập lớn — dự thảo kỹ thuật

Môn Thực hành lập trình .NET · Đề tài 05: Xây dựng hệ thống quản lý khách sạn và đặt phòng. Project QuanLyKhachSan_UNETI06_TI17A2ND. Nhóm 06, lớp TI17A2ND.

| Thành viên | MSSV | Phân công |
|---|---|---|
| Nguyễn Thọ Oai, trưởng nhóm | 23274800001 | Module 1, kiến trúc/CSDL/migration, kiểm thử tích hợp |
| Đặng Tùng Dương | 23274800005 | Module 2, Dashboard/thống kê |
| Nguyễn Văn Phong | 23203100062 | Module 3, phiếu đặt và nhận/trả |

Nhóm có 03 người thay vì 04 theo đề mẫu; cần giảng viên xác nhận. Các thành viên chưa ký nhận trách nhiệm vào bản bàn giao này. Cần chuyển dự thảo sang đúng mẫu báo cáo của giảng viên khi được cung cấp.

## 1. Phân tích bài toán

Admin quản lý danh mục và vận hành lưu trú; khách tìm phòng, đặt và theo dõi dữ liệu cá nhân. Các vấn đề cần giải quyết gồm phòng trùng lịch, dữ liệu tính tiền bị sửa từ trình duyệt, chuyển trạng thái sai, truy cập trái quyền và lịch sử bị mất khi xóa danh mục. Scope gồm bốn module của đề; Module 4 được phân đôi theo bảng phân công.

## 2. Kiến trúc và thiết kế

MVC trên .NET 10, EF Core 10 Code First, SQL Server. Sáu Entity và ERD ở THIET_KE_CSDL.md; PK/FK/unique/check constraint tại HotelDbContext. Controller dùng ViewModel/Bind allowlist; nghiệp vụ đặt/nhận/trả nằm ở BookingService, giao dịch ở HotelTransaction. SessionGuard xác thực lại tài khoản và vai trò tại server. Không dùng ORM khác thay EF hoặc SQL viết tay thay CRUD.

## 3. Chức năng triển khai

Tài khoản login/logout/khóa, quản lý loại, phòng, khách và hồ sơ. RoomSearch áp dụng Query→Search→Filter→Sort→Page. BookingService tính số đêm/giá snapshot/tổng, kiểm trùng nửa mở và khóa đồng thời. PhieuDatPhongController điều phối xác nhận/hủy/nhận/trả. StatisticsService tạo 8 chỉ số Dashboard, số phòng/loại, phiếu/trạng thái, lượt/phòng, loại nhiều nhất, lượt/khách, doanh thu/tháng đã trả.

## 4. Mã và truy vấn tiêu biểu

Đọc Services/RoomSearch.cs để giải thích IQueryable, CountAsync, Skip/Take và sort ổn định. Đọc Services/BookingService.cs cùng BookingRules.cs và HotelTransaction.cs để trình bày race condition và rollback. Đọc Filters/SessionGuardAttribute.cs và DatPhongController.Details để giải thích Controller guard và ownership. Đọc StatisticsService.cs để đối soát Count/GroupBy/Sum; phiếu hủy không tính lượt đặt, tháng doanh thu là tháng ngày trả dự kiến.

## 5. Giao diện và dữ liệu

Razor/Tag Helpers, CSS riêng, tiếng Việt, định dạng VND và ngày Việt Nam; responsive cơ bản. Dữ liệu mẫu: 5 loại, 20 phòng, 15 khách + tài khoản, 1 admin, 18 phiếu và chi tiết. Dashboard ban đầu: 4 chờ, 4 xác nhận, 4 đang ở, 3 đã trả, 3 hủy; tổng tiền đã trả 6.900.000 VND. Ảnh thật từ trình duyệt tại docs/minh-chung, mô tả ở MINH_CHUNG_GIAO_DIEN.md. Không có video minh chứng trong phiên này.

## 6. Kiểm thử và hoàn thiện

Xem KET_QUA_KIEM_THU.md, raw TRX và output lệnh ở verification. Tests SQL Server dùng DB GUID độc lập, không đụng DB cá nhân. Phạm vi gồm auth/ownership/anti-forgery, CRUD/FK/validation, filter/sort/page, đặt nhiều phòng/giá giả/trùng/đồng thời, trạng thái/phòng, seed và đối soát báo cáo. Các lỗi thực tế và sửa chữa được ghi trong kết quả; không suy diễn kiểm thử triển khai Internet hoặc mọi trình duyệt.

## 7. Quá trình GitHub và phần việc cá nhân

Remote đã kiểm tra truy cập được nhưng chưa có refs ở thời điểm bàn giao. Chưa push, commit hoặc PR. Tài liệu GITHUB_HUONG_DAN_3_THANH_VIEN.md mô tả quy trình để mỗi người tự đọc/sửa/test/commit thật. Bảng file/chức năng ở BANG_PHAN_CONG_CONG_VIEC.md và docs/ban-giao. Trưởng nhóm và thành viên phải bổ sung quá trình, link PR/commit thật và header sau khi trực tiếp tham gia; không dùng ZIP làm bằng chứng cá nhân.

## 8. Đánh giá và hướng phát triển

Đã có bản kỹ thuật tích hợp và kiểm thử SQL Server. Cần nhóm review trực tiếp, giảng viên xác nhận quy mô nhóm, hoàn thiện báo cáo theo mẫu, lịch sử GitHub thực và video bảo vệ. Có thể mở rộng ngày nhận/trả thực tế, dịch vụ/thuế/thanh toán, ảnh phòng, distributed session và khóa theo từng phòng để tối ưu tải. Các mục nâng cao không phải điều kiện thay cho yêu cầu bắt buộc.

## 9. Phân công hoàn thiện báo cáo và minh chứng

| Nội dung | Người phụ trách | Người review |
|---|---|---|
| Mở đầu, phân tích, MVC, CSDL/migration, Module 1 | Nguyễn Thọ Oai — 23274800001 | Đặng Tùng Dương và Nguyễn Văn Phong |
| Module 2, LINQ search/filter/sort/page, Dashboard/thống kê | Đặng Tùng Dương — 23274800005 | Nguyễn Thọ Oai |
| Khách hàng, booking, snapshot/overlap/ownership/đồng thời, nhận/trả | Nguyễn Văn Phong — 23203100062 | Nguyễn Thọ Oai |
| Kiểm thử, cài Visual Studio, tổng hợp file và định dạng báo cáo | Nguyễn Thọ Oai — 23274800001 | Dương đối soát số liệu; Phong đối soát nghiệp vụ |
| Ảnh và lời giải thích phần phòng/thống kê | Đặng Tùng Dương — 23274800005 | Nguyễn Thọ Oai |
| Ảnh và lời giải thích phần booking/nhận trả | Nguyễn Văn Phong — 23203100062 | Nguyễn Thọ Oai |
| Liên hệ lấy mẫu báo cáo và xác nhận nhóm 3 người | Nguyễn Thọ Oai — 23274800001 | Dương và Phong |
| Video trình bày | Oai: mở đầu/Module 1/tổng kết; Dương: Module 2/stats; Phong: Module 3/nhận trả | Oai ghép, cả nhóm xác nhận đoạn của mình |

Đã ghi header phân công từng file và kiểm thử bổ sung Production cục bộ; xem PHAN_CONG_THEO_FILE.json và KIEM_TRA_PRODUCTION_CUC_BO.md. Source chạy bằng Visual Studio, không yêu cầu hosting để làm bài. Không có chữ ký, ngày xác nhận, link PR/video hoặc phản hồi giảng viên được tạo thay thành viên.

Kết quả kiểm thử cập nhật v2: toàn bộ 74/74 ca đạt, gồm 4 ca cấu hình Production cục bộ; build 0 warning/0 error. Bài chạy bằng Visual Studio theo HUONG_DAN_VISUAL_STUDIO.md; không có triển khai hosting Internet.

Giao diện v4: font Noto Sans cục bộ, tiêu đề tiếng Việt không còn dùng Georgia; có 5 ảnh minh họa theo hạng phòng ở trang chủ/danh sách/chi tiết/form đặt. Dương tiếp quản RoomIllustrations và tài sản ảnh, Oai điều phối font/CSS/trang chủ, Phong sử dụng ảnh ở form đặt. Regression sau thay đổi giao diện vẫn đạt 74/74; tham chiếu FONT_VA_ANH_MINH_HOA.md và screenshot cập nhật.

Cập nhật kỹ thuật 17/10/2026 (v5): 78/78 ca SQL/HTTP đạt; sửa ngày tạo phiếu mẫu trong tương lai; đối chiếu đủ 25 mục và dọn/đóng lại ba ZIP. Xem RA_SOAT_TOAN_BO_20261017.md. Trạng thái review cá nhân, GitHub, xác nhận nhóm, báo cáo mẫu và video vẫn chờ thực hiện thật.
