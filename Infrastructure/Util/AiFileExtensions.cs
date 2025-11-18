using AICore.ValueTypes;
using Infrastructure.ValueTypes;

namespace Infrastructure.Util;

public static class AiFileExtensions
{
    public static string ToBase64MimeString(this AiFile file)
    {
        var contentBase64 = Convert.ToBase64String(file.Content);
        return $"data:{file.Info.MimeType};base64,{contentBase64}";
    }
}