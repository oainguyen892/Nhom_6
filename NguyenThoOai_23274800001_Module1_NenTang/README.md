# Nguyễn Thọ Oai — Module 1 và nền tảng dùng chung

MSSV: **23274800001**. Bản bàn giao kỹ thuật có hỗ trợ AI, công bố theo yêu cầu của chủ tài khoản `oainguyen892`. Đây là một commit bàn giao hiện tại; không khẳng định là lịch sử tự phát triển của sinh viên. Header phân công/chờ review được giữ nguyên.

Phần này chứa tài khoản/đăng nhập/Session/phân quyền, quản lý loại phòng, Entity/DbContext/migration/seed, cấu hình solution/project, giao diện và thư viện dùng chung, tests và tài liệu tích hợp. Không sao chép source Module 2/thống kê của Dương hoặc Module 3/nhận trả của Phong vào commit này.

## Ghép để chạy

Đây là phần dùng chung; cần ghép đủ ba phần vào **một thư mục mới**, cùng một project và SQL Server. Không chạy trực tiếp khi chưa có các controller/view/service của hai module còn lại.

1. Sao chép nội dung thư mục này vào thư mục tích hợp mới.
2. Sao chép nội dung `DangTungDuong_23274800005_Module2_Dashboard_ThongKe/` ở repo vào thư mục tích hợp đó.
3. Sao chép nội dung `NguyenVanPhong_3/` vào cùng thư mục tích hợp.
4. Mở `QuanLyKhachSan_UNETI06_TI17A2ND.sln` bằng Visual Studio 2026, chọn project MVC/profile `http`, chạy Ctrl+F5. Xem `docs/HUONG_DAN_VISUAL_STUDIO.md` và `docs/HUONG_DAN_CAI_DAT_CHAY.md` để cấu hình SQL Server.
5. Chạy `dotnet restore --locked-mode`, `dotnet build -c Release`, `dotnet test -c Release`. Test mặc định cần instance LocalDB `Nhom6DeTai05`, tạo một lần bằng `SqlLocalDB create Nhom6DeTai05 -s`. Test chỉ tạo/xóa DB tạm có tiền tố riêng; không dùng DB ứng dụng cho test.

Kết quả baseline tích hợp ngày 17/10/2026: 78/78 test đạt, build 0 lỗi/0 cảnh báo. Chi tiết/giới hạn ở `docs/RA_SOAT_TOAN_BO_20261017.md`; log có tiền tố `audit-20261017`. Manifest `MANIFEST_OAI.json` xác định nội dung gói này; `docs/PHAN_CONG_THEO_FILE.json` là bảng phân công toàn nhóm, gồm cả các file ở hai phần còn lại.

Font Noto Sans và thư viện dùng chung kèm giấy phép. Ảnh minh họa phòng thuộc gói Dương. Không có ZIP, bin/obj/.vs, database cá nhân hoặc thông tin xác thực GitHub.

Đây chưa phải xác nhận hoàn thành toàn bộ yêu cầu học phần: nhóm còn phải xác nhận quy mô 3 người, trực tiếp review/giải thích/nhận trách nhiệm, ghi đúng đóng góp, hoàn thiện báo cáo theo mẫu và video thật. Production máy chủ thật chưa được kiểm chứng.
