# Kịch bản video và bảo vệ — 3 thành viên

Điều phối, ghép và kiểm tra video: Nguyễn Thọ Oai — 23274800001. Người trình bày Module 2/thống kê: Đặng Tùng Dương — 23274800005. Người trình bày Module 3/nhận trả: Nguyễn Văn Phong — 23203100062. Đây là kịch bản đã chuẩn bị; chưa có video ghi hình ba sinh viên. Thời lượng gợi ý khoảng 12–15 phút, điều chỉnh theo yêu cầu thật của giảng viên.

| Phân đoạn | Người trình bày | Thao tác thật cần ghi |
|---|---|---|
| 0:00–1:00 Mở đầu | Nguyễn Thọ Oai | Giới thiệu đề tài, đúng 3 tên/MSSV, phân công; mở solution Visual Studio |
| 1:00–4:00 Nền tảng và Module 1 | Nguyễn Thọ Oai | net10/EF10, 6 Entity/ERD/migration; đăng nhập đúng/sai, khách vào URL admin; CRUD loại phòng |
| 4:00–7:00 Module 2 và thống kê | Đặng Tùng Dương | Search/filter kết hợp, 6 sort, đổi trang giữ query; tạo/sửa phòng; mở RoomSearch và đối soát Dashboard |
| 7:00–11:00 Module 3 và nhận/trả | Nguyễn Văn Phong | Hồ sơ/ownership, đặt nhiều phòng, ngày sai/trùng lịch; xác nhận→nhận→trả; mở BookingService/HotelTransaction |
| 11:00–13:00 Test và tổng kết | Nguyễn Thọ Oai | Chạy test, mở TRX, giải thích kết quả; nêu hạn chế; mở commit/PR thật nếu đã có |
| Hỏi đáp | Cả 3 người | Tự giải thích file phụ trách và thực hiện thay đổi nhỏ theo yêu cầu |

## Lời dẫn cho Oai

“Em là Nguyễn Thọ Oai, mã sinh viên 23274800001, nhóm trưởng Nhóm 06 lớp TI17A2ND. Nhóm thực hiện Đề tài 05 trên ASP.NET Core 10 MVC và SQL Server. Phần được giao cho em gồm tài khoản, phân quyền, loại phòng, nền tảng dữ liệu và tích hợp. Sau đây em trình bày cấu trúc solution và cơ sở dữ liệu.”

Trong phần mã, giải thích Model–Controller–View, PK/FK, DbContext, Migration, PasswordHasher, SessionGuard và antiforgery. Khi giới thiệu việc đã làm cá nhân, chỉ nêu những phần Oai thực tế đã trực tiếp tham gia sau review.

## Lời dẫn cho Dương

“Em là Đặng Tùng Dương, mã sinh viên 23274800005. Phần được giao cho em là quản lý phòng, tìm kiếm, lọc, sắp xếp, phân trang và Dashboard/thống kê. Em sẽ kết hợp các điều kiện tìm phòng và chuyển trang để kiểm tra việc giữ lại bộ lọc.”

Mở RoomSearch: chỉ rõ IQueryable, Where, OrderBy, CountAsync, Skip/Take. Đối soát 20 phòng/5 loại; chỉ rõ doanh thu chỉ lấy phiếu đã trả và nhóm theo ngày trả dự kiến. Không nói đã tự viết/review trước khi thực tế đã tham gia.

## Lời dẫn cho Phong

“Em là Nguyễn Văn Phong, mã sinh viên 23203100062. Phần được giao cho em là khách hàng, hồ sơ, đặt phòng, quản lý phiếu và nhận/trả. Em sẽ trình diễn một lượt đặt hợp lệ, một trường hợp trùng lịch và quy trình chuyển trạng thái.”

Đăng nhập khách để đặt, chuyển sang admin để xác nhận/nhận/trả; giải thích snapshot giá, [ngày nhận, ngày trả), ownership và khóa DB trong giao dịch. Dùng ngày hiện tại và phòng sẵn sàng để trình diễn nhận phòng. Chỉ nêu đóng góp cá nhân có thật.

## Trước khi quay và sau khi quay

Oai chạy thử solution và DB demo, kiểm âm thanh/quay màn hình; Dương và Phong chuẩn bị đúng file của mình. Không đưa credential cá nhân hoặc màn hình tài khoản riêng vào video. Mỗi người dùng giọng và hình ảnh/thao tác của chính mình; không tạo giọng phát biểu giả hoặc video giả làm minh chứng. Sau quay, Oai ghép ba đoạn, cả nhóm xác nhận nội dung và đối chiếu thời lượng giảng viên yêu cầu. Link video thật chỉ ghi sau khi đã quay và người có quyền chấp thuận chia sẻ.
