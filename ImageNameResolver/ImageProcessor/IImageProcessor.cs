public interface IImageProcessor
{
    Task<string> ProcessImageAsync(string imagePath);
}