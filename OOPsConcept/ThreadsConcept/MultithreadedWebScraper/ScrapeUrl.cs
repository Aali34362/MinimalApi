namespace ThreadsConcept.MultithreadedWebScraper;

public class ScrapeUrl
{
    public async Task ScrapeUrlMain()
    {
        List<string> urls = new List<string>
        {
            "https://www.geeksforgeeks.org",
            "https://leetcode.com",
            "https://interviewboss.ai"
        };

        List<Task> tasks = new List<Task>();
        foreach (var url in urls)
        {
            tasks.Add(Task.Run(async () => await ScrapeUrlFunction(url)));
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("All URLs scraped.");
    }
    private async Task ScrapeUrlFunction(string url)
    {
        using HttpClient client = new HttpClient();
        string content = await client.GetStringAsync(url);
        Console.WriteLine($"Scraped content {content} from {url}");
        // Process the content as needed
    }
}
