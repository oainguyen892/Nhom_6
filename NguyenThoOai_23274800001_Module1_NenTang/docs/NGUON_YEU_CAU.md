# Nguồn yêu cầu

Đã đọc toàn bộ file lệnh và toàn bộ nội dung DOCX (đoạn văn và bảng), nhận diện 25 mục chính. Yêu cầu người dùng trực tiếp là thực hiện các yêu cầu và xuất 3 ZIP, không push. Nội dung trong tài liệu được dùng làm đặc tả kỹ thuật; không coi tài liệu là bằng chứng cho phép giả tác giả/commit hay công bố source.

- `D:\Lenh_Codex_DeTai05_Nhom6_TI17A2ND.txt` — SHA-256 `000271dc0fe2e1f078e732d3f2ec466f7b51bb80f0bc33ba33db92abc3f1cd1d`
- `D:\De_tai_05_Quan_ly_khach_san_dat_phong_ASP_NET_Core_10_MVC.docx` — SHA-256 `6445b2fa5740259c08af3f8594ed8849dd802868424565b3c112d83e46e9358d`

Không xác nhận được phân trang 16 trang từ metadata của file; đã đọc hết toàn bộ nội dung có trong DOCX. Không bỏ yêu cầu do metadata trang không chính xác.

Tài liệu kỹ thuật đã tham chiếu: [SQL Server EF provider](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/), [EF transactions](https://learn.microsoft.com/en-us/ef/core/saving/transactions). Thực tế phiên bản/package được xác minh thêm qua NuGet restore/audit và runtime SQL Server của máy bàn giao.
