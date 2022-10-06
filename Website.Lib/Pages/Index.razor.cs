using Microsoft.AspNetCore.Components;

namespace Website.Lib;

/// <summary>
/// The website's index page
/// </summary>
public partial class Index : ComponentBase
{
    [CascadingParameter] private MainLayout MainLayout { get; set; } = default!;


    private class ImageData
    {
        public string Uri { get; set; } = "";
        public string Caption { get; set; } = "";
        public string Width { get; set; } = "";
        public string Height { get; set; } = "";
    }


    [Inject] private NavigationManager NavigationManager { get; set; } = default!;


    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            MainLayout.ShowHomeButton(false);
        }
    }
}

