using Material.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Website.Lib;

/// <summary>
/// The standard Blazor MainLayout component.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;


    private bool HomeButtonExited { get; set; } = true;


    private async Task HomeClick()
    {
        NavigationManager.NavigateTo("/");
        await JSRuntime.InvokeVoidAsync("Website.General.scrollToTop").ConfigureAwait(false);
        HomeButtonExited = true;
        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }


    public void ShowHomeButton(bool show)
    {
        HomeButtonExited = !show;
        _ = InvokeAsync(StateHasChanged);
    }
}
