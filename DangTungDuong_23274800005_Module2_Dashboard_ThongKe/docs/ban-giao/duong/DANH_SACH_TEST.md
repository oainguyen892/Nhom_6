# Checklist kiểm thử thành viên

Phiên bản QLKS06-20261016-v4; sourceFingerprint `29c4011e38a8defe5f0f6b1e7d6cd6c5352e3f1fba41b88e060ad53d65201018`. Cùng một project và DB; đây là overlay kỹ thuật, không phải ứng dụng độc lập hay bằng chứng đóng góp cá nhân. Người được giao phụ trách: Đặng Tùng Dương, MSSV 23274800005, Module 2 và Dashboard/Thống kê. Header đã điền tên theo phân công, kèm trạng thái chờ review cá nhân.

Các test tương ứng trong baseline tích hợp đã chạy SQL Server thật; xem docs/KET_QUA_KIEM_THU.md và verification/final.trx. Thành viên phải chạy lại trên máy mình sau sửa, không ký nhận kết quả thay người khác.

- [ ] `SearchFilterSortPageAndLinks`
- [ ] `CrudValidationUniquenessAndForeignKeys`
- [ ] `RoomEditPreservesHistoryAndPriceSnapshot`
- [ ] `MigrationSeedAndStatisticsReconcile`
- [ ] `DecimalPriceBindsWithoutCultureDrift`
- [ ] Chạy đầy đủ regression: `dotnet test -c Release`.
- [ ] Review Controller guard, POST antiforgery, validation và encoding tiếng Việt.

Người được giao kiểm tra: Đặng Tùng Dương — 23274800005. Người review tích hợp: Nguyễn Thọ Oai — 23274800001. Ngày/kết quả/nội dung sửa do chính thành viên ghi sau khi kiểm tra thật.
