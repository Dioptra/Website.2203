﻿using Material.Blazor;
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
        public bool Preload { get; set; } = false;
        public string Rel => Preload ? "preload" : "prefetch";
    }


    [Inject] private NavigationManager NavigationManager { get; set; } = default!;


    private static readonly ImageData[] CarouselImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Lib/images/01-main-screen.webp", Caption = "Dioptra's main screen layout", Preload = true },
        new() { Uri = "_content/Website.Lib/images/02-main-screen-search.webp", Caption = "Scheme search" },
        new() { Uri = "_content/Website.Lib/images/03-march-costs-chart.webp", Caption = "Budget, actual and forecast development costs" },
        new() { Uri = "_content/Website.Lib/images/04-march-accruals.webp", Caption = "Loan interest accrual details" },
        new() { Uri = "_content/Website.Lib/images/05-march-cost-summary.webp", Caption = "Project cost summary" },
        new() { Uri = "_content/Website.Lib/images/06-march-capital-flows.webp", Caption = "Actual/forecast capital structure" },
        new() { Uri = "_content/Website.Lib/images/07-march-land-reg.webp", Caption = "UK Land Registry sold unit prices" },
        new() { Uri = "_content/Website.Lib/images/08-march-version-graph.webp", Caption = "Scheme data versioning/audit" },
        new() { Uri = "_content/Website.Lib/images/09-march-edit-budget-schedule.webp", Caption = "Editting cost budget schedules" },
    };


    private static readonly ImageData[] SkylineImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Lib/images/new-york-640.webp", Caption = "New York skyline", Width = "640px", Height = "420px" },
        new() { Uri = "_content/Website.Lib/images/new-york-420.webp", Caption = "New York skyline", Width = "420px", Height = "420px" },
        new() { Uri = "_content/Website.Lib/images/new-york-320.webp", Caption = "New York skyline", Width = "320px", Height = "420px" },
    };


    private static readonly ImageData[] ProgrammerImages = new ImageData[]
    {
        new() { Uri = "_content/Website.Lib/images/programmer-640.webp", Caption = "Programmer working at a desk", Width = "640px", Height = "420px" },
        new() { Uri = "_content/Website.Lib/images/programmer-420.webp", Caption = "Programmer working at a desk", Width = "420px", Height = "420px" },
        new() { Uri = "_content/Website.Lib/images/programmer-320.webp", Caption = "Programmer working at a desk", Width = "320px", Height = "420px" },
    };


    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            MainLayout.ShowHomeButton(false);
        }
    }
}

