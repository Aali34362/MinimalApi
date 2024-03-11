namespace FormatDemo.TypesofLifeTimeServices;

public class WeatherForecast
{
    private readonly DependencyService dependencyService;
    private readonly DependencyService1 dependencyService1;
    public WeatherForecast(DependencyService dependencyService, DependencyService1 dependencyService1)
    {
        this.dependencyService = dependencyService;
        this.dependencyService1 = dependencyService1;
    }
    public void Get()
    {
        dependencyService.Write();
        dependencyService1.Write();
    }
}
