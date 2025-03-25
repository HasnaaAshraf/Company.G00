namespace Company.G00.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1. Upload 

        public static string UploadFile(IFormFile file , string folderName)
        {

            // 1. Get Folder Location

            // Static Data :
            //string FolderPath = "C:\\Users\\ZONE\\source\\repos\\Company.G00\\Company.G00.PL\\wwwroot\\Files\\" + folderName;

            // Dynamic data : 
            //var FolderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\Files\\" + folderName; // Get Your Dynamic path => {PL}


            // Syntax Sugar :

            var FolderPath = Path.Combine(Directory.GetCurrentDirectory() , "wwwroot", "Files", folderName);

            // 2. File Name : (File Name That Appear When We Upload Image And This name Not Unique  )
            // Get File Name And Make It Unique

            Directory.CreateDirectory(FolderPath);

            // Create unique file name
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";

            // File Path

            var FilePath = Path.Combine (FolderPath, fileName);
       
            using var fileStream = new FileStream(FilePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName; 

        }

        // 2. Delete 

        //public static void Delete( string fileName , string folderName)
        //{
        //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Files", folderName, fileName);

        //    if (File.Exists(filePath))
        //        File.Delete(filePath);

        //}

        public static void Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName)) return;

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

    }
}
