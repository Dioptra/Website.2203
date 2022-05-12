using Microsoft.AspNetCore.Components;

namespace Website.Lib.Shared;

public partial class MainBoundary : ComponentBase
{
    [Parameter] public string ColorClass { get; set; }
}
