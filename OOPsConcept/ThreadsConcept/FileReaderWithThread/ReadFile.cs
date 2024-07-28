namespace ThreadsConcept.FileReaderWithThread;

 public class ReadFile
{
    private void ReadFileFunction(string filePath)
    {
        try
        {
            // Simulate a delay to show threading effect
            Thread.Sleep(1000);

            using (StreamReader reader = new StreamReader(filePath))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine($"Content of {Path.GetFileName(filePath)}:");
                Console.WriteLine(content);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading {filePath}: {ex.Message}");
        }
    }

    public void GetFileFunction()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        string[] files = { "file1.txt", "file2.txt", "file3.txt" };
        ////string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "FileReaderWithThread");
        ////string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileReaderWithThread");
        //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "FileReaderWithThread");

        // Get the base directory
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;

        // Navigate up to the project directory
        DirectoryInfo projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!;

        // Combine with the folder name
        string folderPath = Path.Combine(projectDir.FullName, "FileReaderWithThread");

        // Create and start a thread for each file
        foreach (var file in files)
        {
            ///var path = Path.GetFullPath(file);
            string filePath = Path.Combine(folderPath, file);
            Thread thread = new Thread(() => ReadFileFunction(filePath));
            thread.Start();
        }

        // Wait for all threads to finish
        Console.WriteLine("All threads started. Press Enter to exit.");
        Console.ReadLine();
    }
}
