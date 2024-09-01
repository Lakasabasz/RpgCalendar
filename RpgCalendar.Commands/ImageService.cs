using RpgCalendar.Tools;
using SkiaSharp;

namespace RpgCalendar.Commands;

public class ImageService
{
    public string GetImageUrl(Guid? groupProfilePicture) 
        => groupProfilePicture is null ? "" : $"/img/{groupProfilePicture}.png";

    private string GetFilesystemPath(Guid imageId)
        => Path.Join(EnvironmentData.StaticFilesRoot, $"/{imageId}.png");

    public bool Contains(Guid imageId) => File.Exists(GetFilesystemPath(imageId));

    public void Save(Guid imageId, byte[] rawImage)
    {
        using var imageStream = new MemoryStream(rawImage);
        using var img = SKBitmap.Decode(imageStream);
        using var file = new FileStream(GetFilesystemPath(imageId), FileMode.Create, FileAccess.Write);
        using var encoded = img.Encode(SKEncodedImageFormat.Png, 80);
        encoded.SaveTo(file);
        file.Close();
    }
}