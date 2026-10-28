# Gói source — Nguyễn Văn Phong (23203100062)

Phạm vi theo phân công: **Module 3 — khách hàng, hồ sơ, đặt phòng; phiếu và nhận/trả phòng**.

Đây là gói overlay gồm các file source trong phạm vi được giao, giữ nguyên đường dẫn tương đối của solution. Gói không phải project độc lập và không bao gồm `.git`, `bin/obj`, database, credential hoặc file cấu hình riêng. Các header hiện có thể hiện phân công và trạng thái chờ thành viên review; gói này không xác nhận ai đã trực tiếp viết code.

## Cách đưa lên GitHub

1. Clone repo nhóm và kiểm tra nhánh/commit baseline mà nhóm đã thống nhất. Không dùng ZIP này để thay toàn bộ repo.
2. Tạo nhánh riêng: `git switch -c feature/phong-datphong`.
3. Giải nén ZIP vào **root repo** (cùng cấp với file `.sln`) để các thư mục `QuanLyKhachSan_UNETI06_TI17A2ND/...` được ghép đúng chỗ.
4. Chạy `git status --short` và xem `git diff`; kiểm tra không ghi đè thay đổi của người khác. Chỉ `git add` các file mình đã review và thực sự muốn gửi.
5. Build/test trên full solution, ghi lại kết quả thật; commit bằng tài khoản GitHub cá nhân rồi push branch và mở PR. Không commit ZIP, không tạo commit rỗng và không push thẳng `main`.

Lệnh kiểm tra đề xuất: `dotnet restore --locked-mode`, `dotnet build -c Release`, `dotnet test -c Release` (test tích hợp cần SQL Server). Các ca liên quan trong test suite: `CustomerOwnershipAndAntiforgery`, `BookingSnapshotAndTamperedPricesIgnored`, `InvalidBookingsNeverPersist`, `OverlapAndConcurrentRequestsAreAtomic`, `MultiRoomRollbackAndMoney`, `StateLifecycleUpdatesRoomAndRejectsInvalidMoves`, `CancellationAndFutureCheckinRules`, `FailedMultiRoomCheckinRollsBackAndClearsTrackedChanges`, `BookingAdminHttpFiltersAndTransitions`, `CustomerSearchAndStatusChangeArePersisted`, `InactiveTypeAndLockedAccountCannotCreateBookings`, `CustomerToAdminToStayHistoryWorksThroughHttp`.

## Phụ thuộc baseline chung

- Gói là overlay, không phải project chạy độc lập. Cần áp dụng lên baseline đầy đủ mới nhất của nhóm.
- BookingService phụ thuộc HotelTransaction, HotelClock, EmailQueueService và GiaPhongService; các dịch vụ nền dùng chung không nằm trong gói này.
- Các Razor view dùng các model, enum, layout, validation partial và RoomIllustrations của baseline tích hợp.
- Không bao gồm phần thanh toán/QR và chăm sóc khách hàng mới vì các file này chưa được giao rõ cho một trong hai phạm vi trong yêu cầu hiện tại.
