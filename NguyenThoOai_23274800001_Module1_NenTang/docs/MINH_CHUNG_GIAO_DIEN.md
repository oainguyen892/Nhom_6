# Minh chứng giao diện thực tế

Ảnh chụp thật trong trình duyệt của ứng dụng tại http://localhost:5066 ngày 16/10/2026, dùng SQL Server LocalDB Nhom6DeTai05 và DB demo riêng. Khung trình duyệt bên ứng dụng hẹp (khoảng 304 px); đây là kiểm tra responsive cơ bản, không phải ma trận mọi thiết bị.

- `minh-chung/dashboard.png`: đăng nhập admin, Dashboard hiển thị loại 5/phòng 20/khách 15/phiếu 18. Phần dưới có 6 báo cáo đối soát trong test.
- `minh-chung/phong.png`: trang danh sách phòng, bộ lọc, trạng thái và giá VND.
- `minh-chung/validation-gia.png`: biểu mẫu sửa phòng hiển thị lỗi giá âm bằng tiếng Việt; dữ liệu âm không được lưu.

Ảnh phản ánh thời điểm kiểm tra trực tiếp. HTTP integration kiểm tra thêm đủ các route GET/POST chính; không suy ra ảnh cho các trang chưa chụp. Chưa có video hoặc ảnh bảo vệ của sinh viên.

## Cập nhật giao diện v4

Các ảnh mới được chụp trên trình duyệt desktop khoảng 1264 px sau khi sửa font và thêm ảnh phòng:

- `minh-chung/trang-chu-font-anh.png`: tiêu đề Noto Sans hiển thị dấu tiếng Việt liền, hero có ảnh Suite.
- `minh-chung/danh-sach-phong-co-anh.png`: các thẻ phòng có ảnh riêng theo hạng, giá, trạng thái và nút xem phòng.
- `minh-chung/chi-tiet-phong-co-anh.png`: ảnh lớn bên cạnh thông tin phòng 101.

Đây là screenshot ứng dụng đang chạy thật; các hình nội thất bên trong giao diện là ảnh minh họa, không phải minh chứng ảnh chụp một khách sạn thực tế.
