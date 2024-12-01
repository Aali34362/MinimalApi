using OcrApi.ImageProcessing;
using OcrApi.TesseractProcessing;
namespace OcrApi.ConfigurationSettings;

public static class DependencyInjectionSettings
{
    public static void AddDependencyInjection(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IImagePreprocessor, ImagePreprocessor>();
        builder.Services.AddSingleton<ITesseractOCR, TesseractOCR>();
        builder.Services.Configure<SuperResolutionOptions>(
            builder.Configuration.GetSection("SuperResolution"));
    }
    public static void AddRandomDependencyInjection(this IServiceCollection services)
    {
        services.AddSingleton<ISuperResolutionService, SuperResolutionService>();
        ////services.AddSingleton<SuperResolutionService>(provider =>
        ////{
        ////    // Resolve the configuration
        ////    var options = provider.GetRequiredService<IOptions<SuperResolutionOptions>>().Value;

        ////    // Pass the model path to the service
        ////    return new SuperResolutionService(options.ModelPath!);
        ////});
    }

}
