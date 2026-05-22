using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace Website.Server;

/// <summary>
/// And endpoint for CSP reporting.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CspReportingController : Controller
{
    /// <summary>
    /// Receives CSP reports.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("UriReport")]
    [AllowAnonymous]
    public async Task<IActionResult> UriReport([FromForm] string request)
    {
        await Task.CompletedTask;
        Log.Warning("CSP violation: " + request);
        return Ok();
    }


    /// <summary>
    /// Receives CSP report-to payloads.
    /// </summary>
    /// <param name="report"></param>
    /// <returns></returns>
    [HttpPost("ToReport")]
    [AllowAnonymous]
    public async Task<IActionResult> ToReport([FromBody] JsonElement report)
    {
        await Task.CompletedTask;
        Log.Warning("CSP report-to violation: {Report}", report.GetRawText());
        return Ok();
    }
}
