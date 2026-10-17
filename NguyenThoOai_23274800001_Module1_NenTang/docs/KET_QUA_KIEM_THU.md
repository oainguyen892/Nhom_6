# Kết quả kiểm thử thực tế

**Kết quả mới nhất 17/10/2026: 78 passed, 0 failed, 0 skipped**, baseline QLKS06-20261017-v5. Restore locked mode thành công, build Release 0 warning/0 error; audit NuGet không phát hiện package có lỗ hổng theo feed đang dùng. Đã sửa ngày tạo phiếu seed nằm trong tương lai, thêm 4 ca SQL/HTTP và kiểm lại luồng khách đặt hai phòng → Admin xác nhận/nhận/trả → doanh thu/lịch sử. Chi tiết ở [báo cáo rà soát](RA_SOAT_TOAN_BO_20261017.md); log mới `verification/audit-20261017-test.txt`, `verification/audit-20261017/final.trx`, `verification/audit-20261017-build.txt`. Các phần bên dưới giữ lịch sử ngày 16/10, không thay cho kết quả mới nhất.

Ngày thực hiện: 16/10/2026 (Asia/Saigon). Máy Windows x64 10.0.26300; SDK .NET 10.0.401, runtime 10.0.12; EF Core/SQL Server provider/Design/dotnet-ef 10.0.13. SQL Server thật: SQL Server 2025 Express LocalDB 17.0.4025.3, instance riêng `Nhom6DeTai05`. Không dùng InMemory hoặc SQLite để báo kết quả SQL Server.

## Kết quả đã chạy

| Kiểm tra | Kết quả |
|---|---|
| Remote GitHub | `git ls-remote --symref` thành công, không có refs; không xác định nhánh mặc định remote vì repo chưa có commit |
| .NET 10 / restore / Release build | Thành công; lần build hoàn thiện 0 warning, 0 error |
| EF migration tạo DB mới | InitialHotel đã áp dụng SQL Server thật |
| Model so với migration | Không có thay đổi model chưa tạo migration |
| Automated tests mở rộng | 74 passed, 0 failed, 0 skipped (70 ca hiện có + 4 Production cục bộ) |
| NuGet audit sau sửa dependency | Không phát hiện package có lỗ hổng theo feed tại thời điểm chạy |
| Chạy HTTP thật | Kestrel http://localhost:5066, Development, migrate/seed và các trang chính hoạt động |
| Trình duyệt | Đã kiểm trang chủ/login/Dashboard/danh sách/sửa phòng và validation giá âm; ảnh thật trong minh-chung |
| Đóng gói và kiểm tra cuối | Raw output của lượt cuối, smoke ZIP và kiểm tra hash ở verification; đọc các file này để đối chiếu |

## Lệnh và bằng chứng

```powershell
dotnet --info
dotnet tool restore
dotnet restore --locked-mode
dotnet build -c Release --no-restore
dotnet test -c Release --no-build --logger 'trx;LogFileName=final.trx' --results-directory docs/verification
dotnet ef migrations list --project .\QuanLyKhachSan_UNETI06_TI17A2ND
dotnet ef migrations has-pending-model-changes --project .\QuanLyKhachSan_UNETI06_TI17A2ND
dotnet list package --vulnerable --include-transitive
git diff --check
```

Mỗi test HTTP/SQL dùng DB `Nhom6_Test_<GUID>` độc lập, migrate và seed từ đầu, dọn đúng DB của test sau khi xong. Biến `HOTEL_TEST_CONNECTION` bắt buộc placeholder `{database}` và prefix bảo vệ để tránh thao tác DB cá nhân. Kiểm tra trực tiếp sys.databases sau lượt test không còn DB test tạm.

Output đầy đủ trong `verification/dotnet-info.txt`, `restore.txt`, `build.txt`, `test.txt`, `final.trx`, `migrations.txt`, `packages.txt`, `package-smoke.txt`, `git-check.txt`, `zip-audit.txt`. Đây là bằng chứng kiểm thử được chủ động giữ lại, không phải log chạy ứng dụng cần loại khỏi source.

## Phạm vi 74 ca

| Nhóm | Test / tình huống thực thi |
|---|---|
| Quy tắc unit | 6 khoảng nửa mở; 3 số đêm hợp lệ; 3 số đêm sai; 25 cặp trạng thái; 4 validation loại; 4 validation hồ sơ; ngày sinh tương lai |
| Auth/Session | Login đúng/sai/khóa; logout; session bị khóa sau đăng nhập; khách GET/POST URL admin trả 403 |
| Ownership | Khách xem phiếu người khác 404; POST ID giả hồ sơ vẫn chỉ sửa người đăng nhập; phiếu mới bỏ qua MaKhachHang giả |
| CSRF/overposting | POST không token 400; request giả giá/số đêm/tổng bị bỏ qua; Bind/ViewModel giới hạn field |
| CRUD | Create hợp lệ/trùng, Edit hợp lệ/invalid, Details và Delete; FK restrict, unique SQL, FK sai bị DB từ chối |
| Phòng/loại | Giá/sức chứa/tầng sai; không đổi số/loại có lịch sử; không ngừng phục vụ khi còn lịch; giá mới giữ nguyên giá phiếu cũ |
| Tìm/lọc/sort/page | Có/không kết quả; điều kiện loại+tầng+trạng thái+giá+search; 6 sort; page đầu/cuối/âm/vượt; link giữ query; 8 phần tử/trang |
| Booking | Ngày quá khứ/ngày trả sai, không phòng/phòng không tồn tại/lặp/tạm ngừng; khách khóa/không tồn tại; hợp lệ một/nhiều phòng |
| Tiền | SoDem/ThanhTien/TongTien server; giá snapshot, client tamper; dấu chấm/phẩy không làm sai giá; >2 chữ số thập phân bị từ chối |
| Đồng thời | Hai request độc lập/DbContext riêng cùng đặt một phòng: chính xác một thành công; đặt sát ngày trả hợp lệ |
| Giao dịch | Trùng một phòng làm cả lượt nhiều phòng không được lưu; nhận nhiều phòng lỗi giữa chừng rollback và clear tracker, không rò trạng thái ở thao tác kế tiếp |
| Trạng thái | Chờ→xác nhận→nhận→trả; hủy chờ/xác nhận; chặn nhận sớm/tùy tiện/hủy đang ở; phòng chỉ đổi đang dùng khi nhận, sẵn sàng khi trả |
| Dashboard | Đúng 5 loại/20 phòng/15 khách/18 phiếu; chờ4/xác nhận4/đangở4/đãtrả3; các group/count/lượt và Sum revenue đối soát DB |
| Seed/migration/routes | 6 Entity, migration áp dụng, model không drift; seed chạy lại không trùng; tất cả GET chính khách/admin không lỗi Razor/route |

Test SQL/HTTP nằm trong SqlWebTests.cs và các partial AdminWorkflowTests/DecimalAndRollbackTests; unit trong BookingRulesTests.cs. Không coi số lượng test là bằng chứng bao phủ mọi trường hợp thực tế.

## Lỗi phát hiện và đã sửa

1. Build đầu có 5 lỗi Razor RZ2005 do section directive không ở đầu dòng; sửa vị trí, build sạch.
2. NuGet cảnh báo Pkcs gián tiếp 9.0.x có advisory NU1902/NU1903; nâng EF/tool/testing lên 10.0.13, pin Pkcs 10.0.13 và audit lại không còn cảnh báo.
3. Test lần đầu 54/59 passed: 4 ca tạo ValidationContext khác instance và 1 ca so sánh HTML chưa decode; sửa mã test, 59/59 pass. Đây là lỗi test, không ghi thành lỗi nghiệp vụ ứng dụng.
4. Rà soát lịch sử: chặn đổi số/loại của phòng đã có chi tiết; thêm test kiểm giá mới không làm thay đổi phiếu cũ.
5. Rà soát form tiền: culture vi-VN và client validator có thể hiểu khác dấu thập phân; thêm DecimalBinder, number input và validator giới hạn 2 chữ số, tải site.js sau validation libraries; test dấu chấm/phẩy đạt. Ô lọc giá được render theo invariant culture với step 0.01 để giữ nguyên phần thập phân khi nạp lại trang/chuyển trang, có assertion hồi quy HTTP.
6. Rà soát rollback: tracker có thể giữ thay đổi trong scope sau exception; clear tracker khi giao dịch lỗi; thêm test lỗi phòng thứ hai rồi thao tác tiếp theo không làm lưu nhầm phòng đầu.
7. Các lượt tiếp theo: 67/67, 70/70 đạt. Đã cập nhật package kiểm thử còn cũ, bỏ collector không dùng và format source.
8. Chạy thật scripts/Start-Demo.ps1 bằng Windows PowerShell phát hiện setter SqlConnectionStringBuilder bị adapter IDictionary hiểu sai tên khóa DataSource; sửa dùng indexer với tên khóa chuẩn. Chạy lại script đã restore/migrate/start Kestrel thành công và /Phong trả HTTP 200, xem start-script.txt và startup-smoke.txt. Chuẩn hóa UTF-8 (BOM cho script PowerShell 5.1), LF và bỏ khoảng trắng thừa trong output SQL để git diff --check đạt.

## Giới hạn và phần chưa kiểm chứng

- Đã kiểm 4 ca Production cục bộ qua TestHost/SQL Server; CHƯA KIỂM CHỨNG triển khai Internet, IIS/server public, máy nhiều app instance, tải lớn, HA, distributed Session hoặc mọi trình duyệt/thiết bị. Theo thông tin người dùng, nhóm chạy bằng Visual Studio; không tuyên bố đã triển khai hosting.
- Khóa đồng thời là khóa toàn bộ mutation tồn phòng trong cùng DB; test hai request độc lập đạt, chưa benchmark tải cao. Writer mới phải tuân thủ cùng HotelTransaction.
- Doanh thu dùng ngày trả dự kiến, không có ngày trả thực tế/thuế/thanh toán phát sinh trong schema đề.
- Video, báo cáo theo mẫu riêng, header trách nhiệm thực, commit/PR và bảo vệ của sinh viên CHƯA KIỂM CHỨNG/chưa thực hiện; nhóm cần tự hoàn thiện. Nhóm 3 người cần xác nhận với giảng viên.
- Đã đọc toàn bộ nội dung DOCX; chưa xác minh được phân trang 16 trang vì metadata tài liệu không phản ánh dàn trang.

## Cập nhật bàn giao v2 — phân công và Visual Studio

Đã điền 34 công việc và header phân công cho 68 file C#/Razor, đối chiếu đúng tên/MSSV Oai/Dương/Phong; chưa ký xác nhận hoặc khẳng định tác giả thực hiện thay thành viên. Có bảng file JSON, bản nháp PR cho từng người, đề nghị giảng viên và kịch bản video.

Thêm ProductionSmokeTests: kiểm không seed/migrate, HTTP→HTTPS/HSTS, cookie Secure/login/guard/antiforgery, trang lỗi chung. Lượt đầu 3/4 đạt, phát hiện cookie TempData thiếu Secure; cấu hình Secure ngoài Development và chạy lại 4/4 đạt. Toàn bộ regression sau sửa và header đạt **74/74**, build **0 warning/0 error**. Raw output hiện tại ở verification/test.txt, final.trx và production-local.txt/trx. Đây là kết quả kỹ thuật tại máy bàn giao, không thay xác nhận cá nhân của sinh viên.

## Cập nhật v3 — khởi chạy Visual Studio

Ảnh người dùng cho thấy Folder View chạy executable với Production/content root bin/cổng 5000. Đã bật launchBrowser=true trong profile http và bổ sung cách mở đúng .sln. Chạy thử `dotnet run --project .\QuanLyKhachSan_UNETI06_TI17A2ND --launch-profile http` dùng cấu hình LocalDB mặc định: port 5066, Development, content root thư mục csproj; trang chủ, CSS và login đều HTTP 200. Đã dừng tiến trình kiểm tra để không chiếm cổng khi người dùng F5. Không sửa logic nghiệp vụ, không chạy lại toàn bộ regression cho thay đổi cấu hình trình duyệt; 74/74 là kết quả regression v2. Log mới ở verification/visual-studio-profile-smoke.txt.

## Cập nhật v4 — font tiếng Việt và ảnh phòng

Đã thay font Georgia ở hero bằng Noto Sans variable được lưu trong wwwroot/fonts, kèm OFL; chuẩn hóa/kiểm tra văn bản Razor NFC. Thêm 5 PNG minh họa trong wwwroot/images/rooms, RoomIllustrations ánh xạ tên hạng phòng theo allowlist và fallback khi có loại mới. Trang chủ, danh sách, chi tiết và form chọn phòng hiển thị ảnh/alt/nhãn minh họa. Không đổi schema, migration hay dữ liệu đặt phòng.

Đã kiểm trực tiếp trang chủ, danh sách và chi tiết trên trình duyệt desktop; ảnh chụp thật ở minh-chung/trang-chu-font-anh.png, danh-sach-phong-co-anh.png, chi-tiet-phong-co-anh.png. Các trang chính, font và cả 5 ảnh đều HTTP 200, log font-room-images-http.txt. Build Release 0 warning/0 error; regression sau sửa **74/74 passed, 0 failed, 0 skipped**, output mới trong build.txt/test.txt/final.trx. Không suy diễn đã kiểm mọi trình duyệt hoặc mobile cho giao diện v4.

File RoomIllustrations được giao Dương phụ trách, nâng tổng header phân công lên 69 file; không thay xác nhận tác giả cá nhân. Gói Dương gồm thêm helper và 5 ảnh (16 file source/asset + 4 tài liệu); Phong dùng tài sản ảnh từ baseline tích hợp.
