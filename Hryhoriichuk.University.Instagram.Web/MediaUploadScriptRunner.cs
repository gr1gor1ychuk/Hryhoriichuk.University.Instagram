using Hryhoriichuk.University.Instagram.Web.Controllers;
using Hryhoriichuk.University.Instagram.Web.Data;
using Hryhoriichuk.University.Instagram.Web;

public class MediaUploadScriptRunner
{
    private readonly IServiceProvider _serviceProvider;

    public MediaUploadScriptRunner(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Run()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediaUploadScript = scope.ServiceProvider.GetRequiredService<MediaUploadScript>();
            await mediaUploadScript.UploadMediaForUsers();
        }
    }
}