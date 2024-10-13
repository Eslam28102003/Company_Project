namespace Company.G06.PL.Helpers
{
    public class DocumentSettings
    {

        // 1.Upload
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Location Folder Path

            //string folderPath = Directory.GetCurrentDirectory()+@"wwwroot\files"+folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2. Get File Name Make It Unique

            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path --> FolderPath + FileName

            string filePath = Path.Combine(folderPath, fileName);

            // 4. Save File As Stream : data Per Time

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;
        }




        // 2.Delete

        public static void DeleteFile(string fileName , string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName , fileName);

            if(File.Exists(filePath))
                File.Delete(filePath);

        }
    }
}
