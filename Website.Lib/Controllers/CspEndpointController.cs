using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Website.Lib.Controllers;

/// <summary>
/// And endpoint for CSP reporting.
/// </summary>
public class CspEndpointController : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    public IActionResult UriReport([FromBody] string request)
    {
        Log.Error("CSP violation: " + request);
        return Ok();
    }
}
