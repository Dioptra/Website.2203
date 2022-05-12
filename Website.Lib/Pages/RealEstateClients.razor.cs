using Material.Blazor;
using Microsoft.AspNetCore.Components;
using Website.Lib.Shared;

namespace Website.Lib.Pages;
[Sitemap(SitemapAttribute.ChangeFreqType.Weekly, 0.8)]
public partial class RealEstateClients : ComponentBase
{
    [Inject] private ITeamsNotificationService TeamsNotificationService { get; set; }



    private GeneralPageLayout GeneralPageLayout { get; set; }
    private MBDialog Dialog { get; set; } = new();
    private RealEstateInvestorEnquiry RealEstateInvestorEnquiry { get; set; } = new();
    private string HiringDialogTitle { get; set; } = "";
    private bool ShowProspectiveRole { get; set; } = false;



    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            GeneralPageLayout.ShowHomeButton(true);
        }
    }


    private async Task OpenDialogAsync()
    {
        RealEstateInvestorEnquiry = new();

        await Dialog.ShowAsync();
    }

    private async Task CloseDialogAsync()
    {
        await Dialog.HideAsync();
    }

    private async Task DialogSubmittedAsync()
    {
        await Dialog.HideAsync();
        await TeamsNotificationService.SendNotification(RealEstateInvestorEnquiry);
    }

}
