using Microsoft.AspNetCore.Components;

namespace Website.Lib.Pages;
[Sitemap(SitemapAttribute.ChangeFreqType.Monthly, 0.1)]
public partial class PrivacyPolicy : ComponentBase
{
    [CascadingParameter] private Action<bool> ShowHomeButton { get; set; }


    protected override void OnInitialized()
    {
        base.OnInitialized();

        ShowHomeButton(true);
    }
}
