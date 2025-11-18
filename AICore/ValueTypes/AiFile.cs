namespace AICore.ValueTypes;


public record AiFile(byte[] Content, AiFileInfo Info)
{
    public bool IsImage => Info.IsImage;
}

public record AiFileInfo(string FileId, string MimeType)
{
    public bool IsImage => MimeType.StartsWith("image");
}