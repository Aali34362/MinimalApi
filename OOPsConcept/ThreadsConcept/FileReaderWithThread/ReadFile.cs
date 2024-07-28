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
        string[] files = { "file1.txt", "file2.txt", "file3.txt" };

        // Create and start a thread for each file
        foreach (var file in files)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, file);
            Thread thread = new Thread(() => ReadFileFunction(filePath));
            thread.Start();
        }

        // Wait for all threads to finish
        Console.WriteLine("All threads started. Press Enter to exit.");
        Console.ReadLine();
    }
}
