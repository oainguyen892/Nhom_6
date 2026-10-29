# Quy trình GitHub cho 3 thành viên

Remote: https://github.com/oainguyen892/Nhom_6. Kiểm tra lúc tạo gói: remote truy cập được nhưng `git ls-remote --symref ...` không có refs. Vì vậy chưa xác định được nhánh mặc định/commit chung trên remote. Source local chưa có commit, không có author được tạo hộ và chưa push.

Phiên bản tham chiếu: `QLKS06-20261016-v4`, manifest SHA-256 ở docs/BASELINE.json. FullSource là bản tích hợp dùng kiểm thử/tham chiếu, không phải bằng chứng đóng góp cá nhân. Mỗi người cần có công việc sửa/hoàn thiện thực tế phù hợp quy định đề; chỉ giải nén rồi commit lại không chứng minh quá trình thực hiện.

1. Trưởng nhóm tự kiểm tra source, tài liệu, tests và chính sách của giảng viên; xác nhận nhóm 3 người. Sau khi người có quyền chấp thuận công bố, trưởng nhóm dùng tài khoản thật đưa baseline đã review lên repo. Không làm bước push trong phiên bàn giao này. Ghi commit SHA thật vào bảng xác nhận baseline chung sau khi nó tồn tại.
2. Mỗi thành viên clone repo bằng tài khoản GitHub cá nhân; xác nhận `git remote -v`, `git status`, nhánh mặc định hiện tại và commit baseline do nhóm thống nhất. Không tự giả định remote vẫn trống tại thời điểm nhận.

```powershell
git clone https://github.com/oainguyen892/Nhom_6.git
cd Nhom_6
git fetch origin
git status
git log -5 --oneline
git switch -c feature/duong-phong-thongke  # Dương
# Phong dùng: git switch -c feature/phong-datphong
```

3. Nhận ZIP tương ứng, đọc 4 tài liệu review trong docs/ban-giao/<tên>. Giải nén tại root có `.sln`; đối chiếu manifest với baseline trước khi áp dụng. Gói là overlay, không phải project chạy độc lập. Nếu baseline đã có code giống gói, `git diff` có thể rỗng; không tạo commit rỗng để giả đóng góp.
4. Thành viên tự đọc, chạy test, tìm vấn đề, sửa/bổ sung thật, bổ sung test có ý nghĩa và ghi header cho đúng phần trực tiếp thực hiện. Trao đổi file dùng chung với Oai; migration chỉ tạo sau khi thống nhất schema. Không copy đè bản mới của người khác.
5. `git diff`, chạy build/test, `git diff --check`. `git add` có chọn lọc đúng file mình sửa. Không add ZIP, bin/obj, DB, secrets. Commit bằng tài khoản đã cấu hình thật của bản thân; không dùng cấu hình author giả hay tạo commit hộ.

```powershell
git add QuanLyKhachSan_UNETI06_TI17A2ND/Controllers/PhongController.cs
git commit -m "[23274800005] [Phong] Mo ta thay doi thuc te da thuc hien"
# Phong: [23203100062] [DatPhong] ...
# Oai: [23274800001] [TaiKhoan] ... hoặc [TichHop] ...
git push -u origin feature/duong-phong-thongke
```

Lệnh push là hướng dẫn cho chính thành viên sau khi được chấp thuận, chưa được thực hiện trong gói bàn giao. Phong dùng branch của mình. Tách commit theo tiến độ thực tế; không bịa ngày giờ hay tách code người khác để tạo lịch sử giả.

6. Thành viên mở PR vào nhánh tích hợp/default thực tế. PR nêu lỗi/chức năng vừa thay đổi, cách test và file chung có ảnh hưởng. Trưởng nhóm review theo checklist; merge lần lượt, xử lý conflict bằng cách đọc cả hai thay đổi, không chọn toàn bộ ours/theirs mù quáng. Fetch/pull baseline mới trước module kế tiếp và chạy lại toàn bộ regression SQL Server.
7. Sau merge, mỗi người chuẩn bị bảo vệ và ghi minh chứng thật (PR/commit/video); cập nhật bảng phân công theo đóng góp thực, không ký hộ.

Header C# sau khi thực hiện thật:

```csharp
// Họ và tên: Đặng Tùng Dương
// Mã sinh viên: 23274800005
// Nội dung thực hiện: ghi cụ thể phần đã đọc, chỉnh sửa, kiểm thử và nhận trách nhiệm
```

Header Razor dùng `@* Họ và tên: ...; Mã sinh viên: ...; Nội dung thực hiện: ... *@`. Với Phong dùng Nguyễn Văn Phong / 23203100062; với Oai dùng Nguyễn Thọ Oai / 23274800001. Không ghi header khẳng định tác giả trước khi thành viên thực hiện.

Baseline commit thật do trưởng nhóm điền sau review/công bố: __________. Dương xác nhận: ________. Phong xác nhận: ________. Không có link PR hoặc lịch sử commit minh chứng trong phiên bàn giao này.

## Người phụ trách từng branch và PR

| Người phụ trách | Branch | Bản nháp nội dung PR |
|---|---|---|
| Nguyễn Thọ Oai — 23274800001 | feature/oai-taikhoan-tichhop | github/OAI_PR_DU_THAO.md |
| Đặng Tùng Dương — 23274800005 | feature/duong-phong-thongke | github/DUONG_PR_DU_THAO.md |
| Nguyễn Văn Phong — 23203100062 | feature/phong-datphong | github/PHONG_PR_DU_THAO.md |

Header hiện tại đã điền tên theo phân công, kèm trạng thái chưa xác nhận tác giả thực hiện. Khi mỗi người thực sự review/sửa/test, chính người đó thay nội dung phạm vi được giao bằng đóng góp thực tế và tự xác nhận. Không xóa dòng trạng thái để tạo ấn tượng đã thực hiện trước khi có đóng góp thật. Oai review PR của Dương/Phong; Dương và Phong review các thay đổi liên quan trong PR của Oai. Không tạo commit hộ, không push trong phiên bàn giao này.
