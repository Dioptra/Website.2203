using Microsoft.AspNetCore.Components;

namespace Website.Client;

/// <summary>
/// Terms and conditions page.
/// </summary>
[Sitemap(SitemapAttribute.ChangeFreqType.Monthly, 0.1)]
public partial class TermsAndConditions : ComponentBase
{
    private ElementReference GeneralPageLayoutRef { get; set; } = default!;


    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            GeneralPageLayoutRef.ShowHomeButton(true);
        }
    }
}
