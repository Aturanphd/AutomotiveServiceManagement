using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.UI.Models;

// Money accepts a single decimal separator; grouping separators are intentionally rejected.
public class MoneyModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        var value = context.ValueProvider.GetValue(context.ModelName);
        if (value == ValueProviderResult.None) return Task.CompletedTask;
        context.ModelState.SetModelValue(context.ModelName, value);
        var text = value.FirstValue?.Trim() ?? "";
        if (Regex.IsMatch(text, @"^-?\d{1,7}([,.]\d{1,2})?$") &&
            decimal.TryParse(text.Replace(',', '.'), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var amount))
            context.Result = ModelBindingResult.Success(amount);
        else context.ModelState.TryAddModelError(context.ModelName, "Tutarı 1250,50 veya 1250.50 biçiminde, en fazla iki ondalık basamakla giriniz. Binlik ayırıcı kullanmayınız.");
        return Task.CompletedTask;
    }
}
public class MoneyModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context) => context.Metadata.ModelType == typeof(decimal) ? new MoneyModelBinder() : null;
}
