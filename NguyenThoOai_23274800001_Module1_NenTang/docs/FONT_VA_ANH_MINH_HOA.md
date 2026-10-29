# Font tiếng Việt và ảnh minh họa phòng

Đã thay Georgia ở tiêu đề bằng Noto Sans dùng chung, lưu font cục bộ tại `QuanLyKhachSan_UNETI06_TI17A2ND/wwwroot/fonts/noto-sans-variable.ttf`, kèm giấy phép OFL.txt. Font lấy từ kho Google Fonts chính thức, không gọi Google Fonts/CDN khi người dùng mở trang. CSS có fallback Segoe UI/Tahoma/Arial; tiêu đề có line-height phù hợp dấu tiếng Việt. Chuỗi giao diện được kiểm và chuẩn hóa Unicode NFC. Kiểm tra ban đầu cho thấy văn bản nguồn đã là NFC, vì vậy thay font và metric hiển thị là sửa chính.

Nguồn font: https://github.com/google/fonts/tree/main/ofl/notosans. Giữ nguyên file font và thông tin giấy phép.

## Tài sản ảnh

Năm ảnh được tạo bằng imagegen (built-in), sau đó sao chép vào project; prompt đầy đủ theo từng hạng ở ROOM_IMAGE_PROMPTS.json. Ảnh là minh họa nội thất, không xác nhận ảnh chụp một khách sạn thực tế. Giao diện hiển thị nhãn “Ảnh minh họa”.

| Hạng phòng | File dưới wwwroot/images/rooms |
|---|---|
| Tiêu chuẩn | standard.png |
| Superior | superior.png |
| Deluxe | deluxe.png |
| Gia đình | family.png |
| Suite | suite.png |

RoomIllustrations ánh xạ tên loại phòng sang file cố định, xử lý NFC/hoa-thường và dùng standard.png khi gặp loại mới. Không lấy URL ảnh từ người dùng. Không thay schema hoặc yêu cầu reset database. Ảnh có alt, kích thước và object-fit; ảnh danh sách/form tải lazy, ảnh chính tải ưu tiên.

Ảnh xuất hiện trên trang chủ, danh sách phòng, chi tiết phòng và phần chọn phòng khi đặt. Ảnh/RoomIllustrations thuộc phạm vi Dương; font/CSS/trang chủ dùng chung do Oai điều phối; Phong sử dụng ảnh từ baseline trong form đặt. Giữ rõ đây là phân công, chưa thay xác nhận đóng góp cá nhân.

## Kiểm chứng

Đã mở trang chủ, danh sách và chi tiết phòng trên trình duyệt; dấu tiếng Việt ở tiêu đề hiển thị liền, các ảnh đúng hạng. Trang chủ, danh sách, chi tiết, font và cả 5 ảnh đều HTTP 200; log ở verification/font-room-images-http.txt. Ảnh chụp thật tại minh-chung/trang-chu-font-anh.png, danh-sach-phong-co-anh.png và chi-tiet-phong-co-anh.png. Kiểm thử hồi quy và build ghi tại KET_QUA_KIEM_THU.md.

Sau cập nhật, dừng/chạy lại ứng dụng trong Visual Studio và nhấn Ctrl+F5 trong trình duyệt để tải CSS mới nếu trang cũ còn trong cache. site.css dùng asp-append-version trong layout.
