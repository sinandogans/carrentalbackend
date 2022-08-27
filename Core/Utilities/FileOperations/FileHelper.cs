using Microsoft.AspNetCore.Http;

namespace Core.Utilities.FileOperations
{
    public class FileHelper
    {
        public string AddImageToProject(IFormFile image)
        {
            Guid fileName = Guid.NewGuid();
            string localPath = "C:\\dotnetprojects\\carrentalbackend\\Core\\Utilities\\FileOperations\\CarImages\\" + fileName.ToString() + ".png";

            FileStream fs = new(localPath, FileMode.CreateNew);
            image.CopyTo(fs);
            fs.Close();

            return localPath;
        }
    }
}
