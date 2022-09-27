using Material.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

using Website.Client.Attributes;
using Website.Client.ServiceClients;
using Website.Client.Shared;

namespace Website.Client.Pages;

[Sitemap(SitemapAttribute.ChangeFreqType.Weekly, 0.8)]
public partial class Index : ComponentBase
{
    private class ImageData
    {
        public string Uri { get; set; } = "";
        public string Caption { get; set; } = "";
        public string Width { get; set; } = "";
        public string Height { get; set; } = "";
        public bool Preload { get; set; } = false;
        public string Rel => Preload ? "preload" : "prefetch";
    }


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private INotification Notifier { get; set; }



    private GeneralPageLayout GeneralPageLayout { get; set; }
    private MBDialog Dialog { get; set; }
    private RealEstateInvestorEnquiry RealEstateInvestorEnquiry { get; set; } = new();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.



    private static readonly ImageData[] CarouselImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Client/images/01-main-screen.webp", Caption = "Dioptra's main screen layout", Preload = true },
        new() { Uri = "_content/Website.Client/images/02-main-screen-search.webp", Caption = "Scheme search" },
        new() { Uri = "_content/Website.Client/images/03-march-costs-chart.webp", Caption = "Budget, actual and forecast development costs" },
        new() { Uri = "_content/Website.Client/images/04-march-accruals.webp", Caption = "Loan interest accrual details" },
        new() { Uri = "_content/Website.Client/images/05-march-cost-summary.webp", Caption = "Project cost summary" },
        new() { Uri = "_content/Website.Client/images/06-march-capital-flows.webp", Caption = "Actual/forecast capital structure" },
        new() { Uri = "_content/Website.Client/images/07-march-land-reg.webp", Caption = "UK Land Registry sold unit prices" },
        new() { Uri = "_content/Website.Client/images/08-march-version-graph.webp", Caption = "Scheme data versioning/audit" },
        new() { Uri = "_content/Website.Client/images/09-march-edit-budget-schedule.webp", Caption = "Editting cost budget schedules" },
    };

    private static readonly ImageData[] SkylineImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Client/images/new-york-640.webp", Caption = "New York skyline", Width = "640px", Height = "420px" },
        new() { Uri = "_content/Website.Client/images/new-york-420.webp", Caption = "New York skyline", Width = "420px", Height = "420px" },
        new() { Uri = "_content/Website.Client/images/new-york-320.webp", Caption = "New York skyline", Width = "320px", Height = "420px" },
    };

    private static readonly ImageData[] ProgrammerImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Client/images/programmer-640.webp", Caption = "Programmer working at a desk", Width = "640px", Height = "420px" },
        new() { Uri = "_content/Website.Client/images/programmer-420.webp", Caption = "Programmer working at a desk", Width = "420px", Height = "420px" },
        new() { Uri = "_content/Website.Client/images/programmer-320.webp", Caption = "Programmer working at a desk", Width = "320px", Height = "420px" },
    };

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            GeneralPageLayout.ShowHomeButton(false);
        }
    }


    private void WorkForUsClick()
    {
        NavigationManager.NavigateTo("/work-for-us");
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
        await Notifier.Send(RealEstateInvestorEnquiry);
    }
}

