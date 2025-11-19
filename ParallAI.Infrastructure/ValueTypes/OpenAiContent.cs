using System.Text.Json.Serialization;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Util;

namespace ParallAI.Infrastructure.ValueTypes;

[JsonConverter(typeof(OpenAiContentConverter))]
public record OpenAiContent
{
    public ContentItem[] Items { get; }
    
    public OpenAiContent(ContentItem[] items)
    {
        if (items.Length == 0)
            throw new ArgumentOutOfRangeException(message: "OpenAiContent cannot be empty", paramName: nameof(items));
        Items = items;
    }

    public static OpenAiContent Create(string text) => new([new TextContentItem(text)]);

    public static OpenAiContent Create(string text, IEnumerable<AiFile> attachments)
    {
        var items = attachments
            .Select(ContentItem.CreateFromFile)
            .Append(new TextContentItem(text));

        return new OpenAiContent(items.ToArray());
    }
}

[JsonDerivedType(typeof(TextContentItem), typeDiscriminator: "text")]
[JsonDerivedType(typeof(ImageContentItem), typeDiscriminator: "image_url")]
[JsonDerivedType(typeof(FileContentItem), typeDiscriminator: "file")]
public abstract record ContentItem(string Type)
{
    public static ContentItem CreateFromFile(AiFile file)
    {
        var encodedContent = file.ToBase64MimeString();
        return file.IsImage
            ? new ImageContentItem(new ImageContentItem.ImageUrl(encodedContent))
            : new FileContentItem(new FileContentItem.FileDetails(file.Info.FileId, encodedContent));
    }
}

public record TextContentItem(string Text) : ContentItem("text");

public record ImageContentItem(
    [property: JsonPropertyName("image_url")]
    ImageContentItem.ImageUrl Url
) : ContentItem("image_url")
{
    public record ImageUrl(string Url);
}

public record FileContentItem(FileContentItem.FileDetails File) : ContentItem("file")
{
    public record FileDetails(
        string Filename,
        [property: JsonPropertyName("file_data")]
        string FileData
    );
}