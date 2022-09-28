using Microsoft.AspNetCore.Components;

namespace Website.Client;

/// <summary>
/// Terms and conditions page.
/// </summary>
[Sitemap(SitemapAttribute.ChangeFreqType.Monthly, 0.1)]
public partial class TermsAndConditions : ComponentBase
{
    private GeneralPageLayout GeneralPageLayout { get; set; } = default!;


    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            GeneralPageLayout.ShowHomeButton(true);
        }
    }
}
