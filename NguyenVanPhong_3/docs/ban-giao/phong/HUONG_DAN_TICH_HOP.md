# Hướng dẫn tích hợp

Phiên bản QLKS06-20261016-v4; sourceFingerprint `29c4011e38a8defe5f0f6b1e7d6cd6c5352e3f1fba41b88e060ad53d65201018`. Cùng một project và DB; đây là overlay kỹ thuật, không phải ứng dụng độc lập hay bằng chứng đóng góp cá nhân. Người được giao phụ trách: Nguyễn Văn Phong, MSSV 23203100062, Module 3 và phiếu đặt/nhận/trả. Header đã điền tên theo phân công, kèm trạng thái chờ review cá nhân.

1. Lấy baseline chung do trưởng nhóm review, đối chiếu docs/BASELINE.json trước khi giải nén. Chưa có commit remote để làm ancestor nên không tạo patch diff giả. Hash ở DANH_SACH_FILE xác định chính xác nội dung nhận.
2. Tạo branch cá nhân theo docs/GITHUB_HUONG_DAN_3_THANH_VIEN.md; giải nén tại root chứa .sln, giữ nguyên tiền tố thư mục `QuanLyKhachSan_UNETI06_TI17A2ND/`. Không giải nén thẳng vào Controllers.
3. Dependencies do Oai giữ: 6 Entity và enum/TienAttribute; Data/HotelDbContext và DemoSeed; Migrations; Services/HotelTransaction, HotelClock, DecimalBinder; Controllers/HotelController; Filters/SessionGuardAttribute; Program, csproj, appsettings, layout/_ViewImports/_ViewStart/_ValidationScriptsPartial/CSS/JS; tests và solution. Gói không chứa/sửa các file chung này.
4. Hai ZIP module có tập file source không giao nhau. Shared/_BookingList và _BookingDetails thuộc gói Phong, được hai controller phiếu dùng chung; không có trong gói Dương. Nếu cần sửa file dùng chung, tự tạo thay đổi trên branch riêng, mô tả diff thật và nhờ Oai review, không copy bản chung cũ đè người khác.
   RoomIllustrations và 5 ảnh phòng thuộc gói Dương. Biểu mẫu đặt phòng của Phong dùng ảnh từ baseline đầy đủ này. Font Noto Sans/giấy phép OFL, CSS và trang chủ dùng chung do Oai điều phối, nằm trong FullSource; không chép trùng vào hai overlay.
5. `dotnet restore`, `dotnet build -c Release`, `dotnet test -c Release` trên baseline đầy đủ có SQL Server. Chạy unit riêng khi máy chưa có SQL; ghi rõ integration chưa chạy trên máy đó.
6. Đọc, sửa/bổ sung thật, kiểm thử và nhận trách nhiệm; chỉ lúc đó bổ sung header đúng đóng góp và commit bằng tài khoản thật. Baseline đã chứa cùng code thì git diff có thể rỗng: không tạo commit rỗng hoặc chia lại lịch sử để giả công việc.

Gói không có .git, bin/obj/.vs, DB, credential riêng hoặc ZIP khác. Không tự push main; thành viên chủ động push branch/mở PR sau chấp thuận của nhóm.
