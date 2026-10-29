// Phân công phụ trách (chưa xác nhận tác giả thực hiện).
// Họ và tên: Nguyễn Thọ Oai
// Mã sinh viên: 23274800001
// Nội dung thực hiện: Phạm vi được giao - Kiến trúc MVC, cấu hình, dịch vụ và giao diện dùng chung; tích hợp.
// Trạng thái: chờ thành viên trực tiếp review, chỉnh sửa và xác nhận.
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
namespace QuanLyKhachSan_UNETI06_TI17A2ND.Services;

public sealed class DecimalBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var value = context.ValueProvider.GetValue(context.ModelName);
        if (value == ValueProviderResult.None) return Task.CompletedTask;
        context.ModelState.SetModelValue(context.ModelName, value);
        var raw = value.FirstValue?.Trim();
        if (string.IsNullOrEmpty(raw) && context.ModelMetadata.IsNullableValueType)
            context.Result = ModelBindingResult.Success(null);
        else if (decimal.TryParse(raw?.Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var number))
            context.Result = ModelBindingResult.Success(number);
        else context.ModelState.TryAddModelError(context.ModelName, "Giá phải là số, tối đa một dấu thập phân; không nhập dấu phân nhóm hàng nghìn.");
        return Task.CompletedTask;
    }
}
public sealed class DecimalBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context) =>
        context.Metadata.ModelType == typeof(decimal) || context.Metadata.ModelType == typeof(decimal?) ? new DecimalBinder() : null;
}
