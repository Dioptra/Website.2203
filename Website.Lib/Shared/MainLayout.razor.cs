﻿namespace Website.Lib.Shared;

using Material.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] private ITeamsNotificationService TeamsNotificationService { get; set; }
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; }

    private MBDialog ContactDialog { get; set; } = new();
    private bool HomeButtonExited { get; set; } = true;
    private ContactMessage ContactMessage { get; set; } = new();

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await JSRuntime.InvokeVoidAsync("Website.General.instantiateErrorDialog");
        }
    }

    private async Task OpenContactDialogAsync()
    {
        ContactMessage = new();

        await ContactDialog.ShowAsync();
    }

    private async Task CloseContactDialogAsync()
    {
        await ContactDialog.HideAsync();
    }


    private async Task ContactDialogSubmittedAsync()
    {
        await ContactDialog.HideAsync();
        await TeamsNotificationService.SendNotification(ContactMessage);
    }


    private async Task HomeClick()
    {
        NavigationManager.NavigateTo("/#dw-main-top");
        await JSRuntime.InvokeVoidAsync("Website.General.scrollToTop").ConfigureAwait(false);
        HomeButtonExited = true;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }


    private void ShowHomeButton(bool show)
    {
        HomeButtonExited = !show;
        _ = InvokeAsync(StateHasChanged);
    }


}

