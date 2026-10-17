# Các điểm cần trực tiếp review

Phiên bản QLKS06-20261016-v4; sourceFingerprint `29c4011e38a8defe5f0f6b1e7d6cd6c5352e3f1fba41b88e060ad53d65201018`. Cùng một project và DB; đây là overlay kỹ thuật, không phải ứng dụng độc lập hay bằng chứng đóng góp cá nhân. Người được giao phụ trách: Nguyễn Văn Phong, MSSV 23203100062, Module 3 và phiếu đặt/nhận/trả. Header đã điền tên theo phân công, kèm trạng thái chờ review cá nhân.

Giải thích ownership qua Session, giá DB/snapshot, các khoảng nửa mở, Transaction/app lock và rollback nhiều phòng. Test cả ngày sai, phòng/khách khóa, hủy/nhận/trả sai thứ tự và hai request đồng thời. Không đổi resource lock hoặc thêm writer bỏ qua HotelTransaction.

- [ ] Đọc và giải thích từng file trong danh sách; ghi lại ít nhất những vấn đề thực tế cần sửa/bổ sung theo quyết định nhóm, không tạo sửa đổi hình thức để làm lịch sử.
- [ ] Tự chạy app, test, đối chiếu DB và xác nhận các quy ước nghiệp vụ trong README.
- [ ] Có thay đổi và test thật thì ghi header C# `// Họ và tên: Nguyễn Văn Phong` / `// Mã sinh viên: 23203100062` / `// Nội dung thực hiện: ...` với nội dung cụ thể đã trực tiếp làm. Razor dùng @* ... *@. Nếu cùng sửa, ghi đóng góp từng người; không gán tác giả chưa tham gia.
- [ ] Tự commit bằng tài khoản cá nhân, mở PR và trả lời review; không nhận ZIP làm bằng chứng quá trình phát triển.
- [ ] Đọc HUONG_DAN_BAO_VE và trình diễn thay đổi nhỏ trực tiếp.

Trạng thái hiện tại: đã giao Nguyễn Văn Phong — 23203100062 phụ trách và điền header phân công; chưa có xác nhận trực tiếp về phần việc đã thực hiện. Kịch bản trình bày của Nguyễn Văn Phong ở docs/KICH_BAN_VIDEO_VA_BAO_VE.md; bản nháp PR của người phụ trách ở docs/github. Chính Nguyễn Văn Phong ghi ngày và xác nhận sau review, không ký thay.
