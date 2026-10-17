# Hướng dẫn bảo vệ và tự kiểm tra

## Kiến trúc và dữ liệu

Request đi qua routing, SessionGuard, Controller bind ViewModel và kiểm ModelState, service áp dụng nghiệp vụ, DbContext chuyển LINQ sang SQL, Razor render View. Model là dữ liệu/quan hệ; View hiển thị; Controller điều phối, không đặt toàn bộ nghiệp vụ vào action. Ví dụ DatPhongController chỉ nhận ngày/IDs; BookingService kiểm giá/lịch/giao dịch.

Sáu bảng có PK int; ChiTiet dùng PK ghép phiếu/phòng; FK và navigation thể hiện quan hệ ở THIET_KE_CSDL. DbContext theo DI scoped một request; migration mô tả thay đổi schema có lịch sử; không sửa migration đã triển khai để né lỗi. Model binding có thể nhận dữ liệu giả nên dùng form/Bind allowlist. DataAnnotation kiểm required/range/email; ModelState và asp-validation-summary/message đưa lỗi về View. Razor @ encode output; Tag Helper sinh action/href/select/anti-forgery.

## Xác thực và quyền

PasswordHasher tạo hash có salt, VerifyHashedPassword kiểm password. Session lưu MaTaiKhoan/HoTen/VaiTro; đăng xuất Clear. SessionGuard đọc lại tài khoản từ DB để khóa/đổi quyền có hiệu lực; admin URL bị 403 dù menu có bị tự sửa. Ownership phiếu lọc `KhachHang.MaTaiKhoan == AccountId` ngay trong SQL; hồ sơ lấy AccountId từ Session, không từ POST. AutoValidateAntiforgeryToken áp dụng POST toàn MVC, token được Tag Helper sinh; thiếu token trả 400.

## LINQ và phân trang

RoomSearch dựng IQueryable AsNoTracking → Contains số phòng/tên loại → các Where kết hợp → OrderBy ổn định → CountAsync → clamp page → Skip/Take(8) → ToListAsync. Không ToList trước phân trang. View giữ Q/Loai/Tang/TrangThai/GiaTu/GiaDen/Sort ở liên kết trang. Hãy chứng minh lọc rỗng, page=-1/999, sáu sort và chuyển trang giữ điều kiện.

## Đặt phòng và giao dịch

Số đêm = (trả.Date − nhận.Date).Days, từ 1–365. Giá do DB quyết định, lưu snapshot vào ChiTiet; tổng = Sum(giá × đêm), client không có trường giá đáng tin. Hai khoảng giao nhau khi startA < endB và endA > startB; ngày trả bằng ngày nhận tiếp theo hợp lệ. Giữ lịch của chờ/xác nhận/đang ở; phiếu hủy và đã trả giải phóng lịch.

HotelTransaction bắt đầu transaction rồi lấy sp_getapplock Exclusive trên resource DB chung, chạy check và SaveChanges trước commit, exception dispose rollback. Hai process cùng DB dùng cùng lock nên request thứ hai kiểm lại sau khi request đầu commit. Raw SQL chỉ dùng khóa; CRUD vẫn EF. Khóa thô phù hợp đồ án, throughput cao cần khóa theo phòng theo thứ tự hoặc schema tồn phòng theo ngày. Mọi writer mới bắt buộc theo cùng protocol.

BookingRules cho phép chờ→xác nhận→nhận→trả và hủy ở chờ/xác nhận. Nhận phải đúng khoảng ngày, phòng sẵn sàng, loại hoạt động và không có lượt đang ở khác. Chỉ nhận mới đổi phòng đang sử dụng; trả đổi sẵn sàng. Không đánh đang sử dụng cho phiếu tương lai.

## Báo cáo

Count tổng 8 chỉ số. GroupBy trạng thái và tháng, Sum tiền từ phiếu đã trả; report phòng/loại/khách dùng navigation Count (loại phiếu hủy). Hiểu vì sao số phiếu khác số chi tiết khi đặt nhiều phòng. TopTypes hiển thị thứ tự giảm dần và đồng hạng. Doanh thu dựa trên ngày trả dự kiến vì schema không có ngày trả thật; có thể đề xuất mở rộng nhưng phải nêu thay đổi Entity/migration/UI/tests.

## Câu hỏi riêng

- Oai: vì sao admin có thể không có KhachHang trong quan hệ 1-1? Vì sao hash không so sánh chuỗi trực tiếp? Khóa phiên đang đăng nhập kiểm ở đâu? Thêm ràng buộc unique qua migration thế nào? Chứng minh FK restrict và seed idempotent bằng test nào?
- Dương: đoạn nào vẫn là IQueryable? Nếu chuyển ToList trước Skip thì sai gì? Vì sao cần tie-breaker khi sort giá? Cách giữ giá có phần thập phân qua link trang? Đối soát doanh thu 3 phiếu đã trả với 6.900.000 VND thế nào? Thêm sort mới và kiểm thử ra sao?
- Phong: sửa MaKhachHang trong request có đổi chủ phiếu không? Vì sao dùng khoảng nửa mở? Vì sao transaction đơn thuần ReadCommitted chưa đủ ngăn hai request? App lock được nhả khi nào? Multi-room lỗi phòng thứ hai có để lại phiếu nửa chừng không? Giải thích 25 tổ hợp chuyển trạng thái.

Mỗi người tự trình diễn trên máy sạch: migrate, seed, test, một thay đổi nhỏ thật trong phần phụ trách; cập nhật header đúng đóng góp, commit bằng tài khoản thật, ghi các quyết định nghiệp vụ mình hiểu. Không học thuộc kết quả test mà không đọc truy vấn/service.
