# Dọn dẹp và bàn giao

Đợt mới nhất **17/10/2026 — v5**: xem [báo cáo rà soát](RA_SOAT_TOAN_BO_20261017.md) và `verification/audit-20261017-cleanup.txt`. Đã dọn output build và script sinh tài liệu cũ bằng cách chuyển ra ngoài workspace; giữ .vs vì Visual Studio đang mở, loại .vs khỏi toàn bộ ZIP. Không đổi/xóa database hoặc lịch sử Git. Kết quả mới nhất 78/78; gói v5 được giải nén và kiểm lại độc lập. Phần dưới là lịch sử đợt dọn 16/10.

Đã loại khỏi cây project các bản nháp tạo source, bin/obj, Bootstrap/scaffolding không dùng, trang Privacy/Error mẫu không có route, model ErrorViewModel thừa và bản JS không dùng. Giữ nguyên các thư viện jQuery minified được tham chiếu, source map cần thiết và giấy phép. Giữ source, migration, tests, cấu hình mẫu an toàn và tài liệu kiểm thử.

Cơ chế kiểm duyệt tự động từ chối lệnh xóa đệ quy với thông báo `blocked by policy`, không nêu lý do chi tiết. Để dọn theo cách có thể khôi phục, các file/thư mục thừa được chuyển ra ngoài workspace vào `C:\Users\LEGION 5\AppData\Local\Temp\Nhom6Cleanup_20261016`; chúng không nằm trong source hoặc ZIP cuối. Các build output tạo lại sau kiểm thử cuối cũng được chuyển vào vùng tạm ngoài workspace. Không thay đổi/xóa dữ liệu cá nhân hoặc repo remote.

Sau đợt dọn source đã chạy lại restore ở locked mode, build Release và toàn bộ tests; raw output ở verification. Các ZIP được tạo lại từ cây sạch bằng scripts/package_source.py và kiểm bằng scripts/verify_delivery.py. FullSource được giải nén vào thư mục kiểm thử mới, áp dụng cả hai overlay và build/test lại; kết quả nằm trong verification/package-smoke.txt.

Ba ZIP nằm tại deliverables, được .gitignore loại khỏi source Git. SHA-256 của ba ZIP ở deliverables/SHA256SUMS.txt. docs/BASELINE.json nhận diện phiên bản code chung, không ghi commit chưa tồn tại. Mỗi gói thành viên chỉ có source tương ứng và 4 tài liệu review trong docs/ban-giao/<tên>; không chứa project/database riêng. Các file chung do trưởng nhóm giữ được liệt kê rõ ở HUONG_DAN_TICH_HOP.

Git được khởi tạo local, remote origin trỏ repo chung, nhánh local main chưa có commit. `git add -N` chỉ đánh dấu ý định thêm để kiểm tra toàn bộ diff; chưa stage nội dung, commit, push hoặc tạo PR. Không có thông tin tác giả sinh viên giả. Git status/diff check thật lưu trong verification/git-check.txt.

Phần còn cần nhóm thực hiện: từng thành viên trực tiếp review, sửa/bổ sung thật và chạy lại test; xác nhận nhóm 3 người với giảng viên; ghi header đúng đóng góp; commit/push branch/PR thật; hoàn thiện báo cáo theo mẫu riêng và quay video minh chứng. Không dùng bản tích hợp làm bằng chứng mỗi người đã tự viết module.
