namespace RpgCalendar.Commands;

public class ImageService
{
    public string GetImageUrl(Guid? groupProfilePicture)
    {
        return groupProfilePicture is null ? "" : $"/img/{groupProfilePicture}.png";
    }

    public bool Contains(Guid imageId)
    {
        return true;
    }
}