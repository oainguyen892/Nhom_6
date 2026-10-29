# Rà soát và dọn project ngày 17/10/2026

Phiên bản: **QLKS06-20261017-v5**. Kết luận: các chức năng bắt buộc đã được triển khai và vượt qua bộ kiểm thử hiện có; **chưa đủ điều kiện kết luận đáp ứng toàn bộ bài nộp**, vì còn yêu cầu về nhóm, đóng góp cá nhân, GitHub, báo cáo và video. Không có commit, push hoặc PR được tạo trong đợt này.

Đã đọc lại toàn bộ 423 đoạn văn, kể cả nội dung trong bảng của `D:\De_tai_05_Quan_ly_khach_san_dat_phong_ASP_NET_Core_10_MVC.docx`, đối chiếu 25 mục chính. Tài liệu là đặc tả của bài; trạng thái đạt được xác định từ source và kiểm thử thực tế. Bảng đối chiếu từng mục ở [DOI_CHIEU_25_MUC_DE_TAI_05.md](DOI_CHIEU_25_MUC_DE_TAI_05.md).

## Kết quả thực thi

| Kiểm tra | Kết quả / bằng chứng |
|---|---|
| Restore theo lock file | Thành công; verification/audit-20261017-restore.txt |
| Build Release | 0 lỗi, 0 cảnh báo; verification/audit-20261017-build.txt |
| Kiểm thử tự động sau sửa | **78 đạt, 0 lỗi, 0 bỏ qua**; verification/audit-20261017/final.trx và audit-20261017-test.txt |
| SQL Server | Test tạo database GUID riêng trên LocalDB Nhom6DeTai05, áp migration thật, dọn database test sau mỗi ca; không dùng InMemory/SQLite |
| Model/migration và seed | Không có model lệch migration; 6 entity, 5 loại, 20 phòng, 15 khách, 16 tài khoản, 18 phiếu/chi tiết; seed lặp không nhân bản |
| Dependency audit | Không phát hiện package có lỗ hổng theo nguồn NuGet đang cấu hình tại thời điểm chạy; verification/audit-20261017-packages.txt |
| Gói bàn giao | Kiểm CRC, đường dẫn, hash baseline, file module và thành phần cấm bằng verify_delivery.py; verification/zip-audit.txt |
| Giải nén độc lập | Áp FullSource và cả hai overlay vào thư mục mới ngoài workspace; restore locked mode, build 0 lỗi/0 cảnh báo, **78/78 ca đạt**; verification/audit-20261017-package-smoke.txt/trx |

Máy: Windows, .NET SDK 10.0.401, ASP.NET Core runtime 10.0.12, EF Core 10.0.13. Bằng chứng trước đây ngày 16/10 được giữ nguyên; kết quả đợt này có tiền tố/thư mục `audit-20261017`.

## Đối chiếu chức năng bắt buộc

| Phạm vi đề bài | Đã kiểm tra | Minh chứng mã/test tiêu biểu |
|---|---|---|
| 5.1–5.4 Tài khoản, Session, phân quyền | Đăng nhập đúng/sai/khóa, đăng xuất, xác thực lại tài khoản đã khóa sau đăng nhập; URL và POST Admin bị chặn cho khách; chống giả mạo POST | TaiKhoanController, SessionGuardAttribute; LoginWrongLockedLogoutAndServerGuards, AdminPostsAndLockedSessionRevalidated |
| 5.5 Loại phòng | Danh sách/chi tiết/thêm/sửa/xóa/trạng thái; trùng tên, sức chứa/giá sai; không xóa loại có phòng; không ngừng loại còn lịch hoạt động | LoaiPhongController; CrudValidationUniquenessAndForeignKeys, TypeStatusValidationAndTypeEdit |
| 6.1–6.2 Phòng | CRUD, select loại từ DB, số phòng duy nhất, tầng/giá/trạng thái; bảo toàn số/loại và giá lịch sử; không xóa phòng có chi tiết | PhongController; RoomEditPreservesHistoryAndPriceSnapshot, DecimalPriceBindsWithoutCultureDrift |
| 6.3–6.6 Tra cứu phòng | Tìm theo số/tên loại; kết hợp loại/tầng/trạng thái/khoảng giá; đủ 6 cách sắp xếp; 8 phòng/trang; trang đầu/cuối/ngoài giới hạn, không kết quả, giữ query khi chuyển trang | RoomSearch, PhongQuery, Views/Phong/Index; SearchFilterSortPageAndLinks |
| 7.1 Khách hàng | Admin xem/chi tiết/tìm theo tên, điện thoại, CCCD và cập nhật trạng thái; khách sửa hồ sơ của mình; từ chối dữ liệu sai, bỏ qua ID người khác gửi lên | KhachHangController, HoSoForm; CustomerSearchAndStatusChangeArePersisted, CustomerOwnershipAndAntiforgery |
| 7.2–7.4 Đặt phòng/chi tiết | Đặt một/nhiều phòng; ngày hợp lệ, khoảng nửa mở, khách/tài khoản/loại/phòng hoạt động; giá snapshot, số đêm/tiền do server tính; không lưu phiếu lỗi | BookingService; InvalidBookingsNeverPersist, InactiveTypeAndLockedAccountCannotCreateBookings, BookingSnapshotAndTamperedPricesIgnored |
| 7.5 Quyền dữ liệu | Chỉ xem phiếu/chi tiết/lịch sử của mình; phiếu của khách khác trả 404 | DatPhongController; CustomerOwnershipAndAntiforgery |
| 8.1 Phiếu Admin | Danh sách/chi tiết, tên khách, trạng thái/ngày nhận; xác nhận/hủy, từ chối chuyển trạng thái sai | PhieuDatPhongController; BookingAdminHttpFiltersAndTransitions, CancellationAndFutureCheckinRules |
| 8.2 Nhận/trả | Đúng thứ tự/ngày, phòng sẵn sàng; cập nhật phiếu/phòng, rollback nhiều phòng nếu một phòng lỗi; hai request trùng chỉ một thành công | BookingRules/HotelTransaction; StateLifecycleUpdatesRoomAndRejectsInvalidMoves, FailedMultiRoomCheckinRollsBackAndClearsTrackedChanges, OverlapAndConcurrentRequestsAreAtomic |
| 8.3–8.4 Dashboard/thống kê | Đủ 8 KPI và 6 thống kê; đối soát tổng với SQL, loại trừ phiếu hủy khỏi lượt đặt; doanh thu phiếu đã trả | StatisticsService; MigrationSeedAndStatisticsReconcile |
| 9–12 Kiến trúc/dữ liệu/validation | 6 bảng, PK/FK/navigation, một-một tài khoản/khách, nhiều-nhiều qua chi tiết; EF Code First SQL Server, MVC/ViewModel/binding/Tag Helper; DB bảo vệ unique/FK/tiền/ngày | HotelDbContext, Migrations; SqlConstraintsEnforceUniquenessAndRestrictDeletes, unit BookingRulesTests |
| 13–16 Razor/giao diện/seed | Route khách/Admin hoạt động; date/textarea/select, thông báo, tên thay FK, VND/ngày; font Noto Sans và 5 PNG được phục vụ tại chỗ | PublicAndAdminViewsHaveNoDeadRoutes, LocalRoomIllustrationsAndVietnameseFontAreServed; ảnh chụp ngày 16/10 trong minh-chung |
| 21 và 24 Luồng xuyên suốt | Khách đặt **2 phòng qua HTTP**, Admin xác nhận/nhận/trả qua HTTP, kiểm phiếu/phòng/tiền/thông báo và doanh thu, khách đăng nhập lại thấy lịch sử | CustomerToAdminToStayHistoryWorksThroughHttp |

Không thu thập số người dự kiến trong phiếu, nên điều kiện sức chứa có điều kiện ở 7.4 không áp dụng; sức chứa vẫn hiển thị. Tài khoản Admin không cần hồ sơ khách; quan hệ một-một được thực hiện bằng FK unique ở khách, phía tài khoản có thể không có hồ sơ. Doanh thu dùng tổng tiền dự kiến của phiếu đã trả, nhóm theo tháng ngày trả dự kiến; chưa có thanh toán/ngày trả thực tế trong mô hình của đề. Các quy ước này đã ghi trong README.

## Lỗi tìm thấy và sửa

Dữ liệu mẫu tính NgayDat = ngày nhận - 3 cho cả đặt phòng tương lai, khiến một số phiếu có ngày tạo sau hôm nay. Đã bổ sung assertion về thời gian trong MigrationSeedAndStatisticsReconcile; test thất bại trước sửa (verification/audit-20261017-seed-before.txt). DemoSeed nay đặt ngày tạo trong quá khứ, đồng thời trước ngày nhận, cho cả lịch sử và đặt tương lai. Test và toàn bộ regression đã đạt sau sửa.

Sửa này áp dụng khi tạo dữ liệu mẫu mới. Không tự ghi đè ngày/phiếu trong database đang dùng của người dùng; không reset database để làm đẹp dữ liệu kiểm thử. Không đổi schema, không cần migration mới.

Bổ sung 4 ca SQL/HTTP cho tìm kiếm/trạng thái khách, loại/tài khoản khóa, asset font/ảnh và luồng khách–Admin–lịch sử. Từ 74 lên 78 ca; assertion seed nằm trong test cũ.

## Phần còn thiếu để nộp đúng toàn bộ đề

| Mục | Việc còn phải thực hiện | Người được giao |
|---|---|---|
| 3–4 | Xin xác nhận nhóm 3 người thay quy định 4 người và cách chia Module 4 | Nguyễn Thọ Oai chủ trì; Dương/Phong phối hợp |
| 17, 19, 23, 25 | Trực tiếp đọc, sửa/bổ sung thật, chịu trách nhiệm, xác nhận header đúng đóng góp và bảo vệ | Oai: Module 1/nền tảng; Đặng Tùng Dương: Module 2/thống kê; Nguyễn Văn Phong: Module 3/nhận trả |
| 20 | Commit/PR/lịch sử phát triển thật qua tài khoản mỗi người | Cả ba người; hiện chỉ có tài liệu hướng dẫn, không có lịch sử thay thế do AI tạo |
| 22 | Báo cáo theo mẫu giảng viên, link GitHub và video minh chứng thật | Oai tổng hợp; Dương/Phong viết, quay và xác nhận phần mình |

Họ tên/MSSV đầy đủ và bảng file có tại BANG_PHAN_CONG_CONG_VIEC.md; header hiện tại ghi **phân công chờ review**, không phải xác nhận các sinh viên đã viết code. Chức năng nâng cao mục 18 là tùy chọn: có hash mật khẩu và ảnh minh họa tĩnh; chưa có upload, biểu đồ, Excel/PDF, AJAX hay email.

## Giới hạn kiểm tra

Không khẳng định không còn mọi lỗi. Bộ test bao phủ các nhóm chức năng trong đề, không phải mọi tổ hợp dữ liệu/tải. Không kiểm tải lớn, mọi trình duyệt/thiết bị hay thao tác F5 trực tiếp trong Visual Studio ở đợt này. Các ca Production kiểm cấu hình trong test host cục bộ; **chưa xác minh triển khai Production trên máy chủ thật**. Không có hosting/tên miền theo thông tin hiện tại của nhóm.

## Dọn dẹp và bàn giao

Đã chuyển bin/obj và script sinh tài liệu cũ complete_handover_docs.py (có thể ghi đè nội dung mới) sang vùng tạm khôi phục ngoài workspace; đường dẫn thực tế ở verification/audit-20261017-cleanup.txt. Giữ source, tests, migration, lock file, tài sản font/ảnh/giấy phép, công cụ chạy/đóng gói và bằng chứng kiểm thử. Không đụng database ứng dụng hoặc .git. Visual Studio đang mở nên giữ .vs để bảo toàn trạng thái IDE; .vs bị ignore và không nằm trong ba ZIP.

FullSource và hai overlay thành viên được đóng lại cùng baseline v5; kiểm thử từ thư mục giải nén độc lập ghi ở verification/audit-20261017-package-smoke.txt/trx. SHA-256 cuối ở deliverables/SHA256SUMS.txt. Mở file .sln, chọn profile http, Ctrl+F5 theo HUONG_DAN_VISUAL_STUDIO.md; build sẽ tự tạo lại bin/obj.

Khi kiểm tra cuối, Visual Studio đang mở đã tự tạo lại bin/obj Debug cho xử lý thiết kế. Đây là cache phát sinh của IDE, đã bị ignore và loại khỏi gói; không liên tục xóa khi IDE đang sử dụng. Dọn source và gói bàn giao không đòi hỏi đóng phiên làm việc Visual Studio của người dùng.
