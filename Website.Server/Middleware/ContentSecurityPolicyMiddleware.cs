﻿using Serilog;

namespace Website.Server;


/// <summary>
/// Middleware that adds the content security policy to HTTP request headers. Also pump primes the Vectis server at startup to
/// ensure that the server populates its caches as early as possible.
/// </summary>
public class ContentSecurityPolicyMiddleware
{
    private readonly RequestDelegate _next;


    public ContentSecurityPolicyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ContentSecurityPolicyService cspService, IWebHostEnvironment env)
    {
        var cspValues = cspService.GetValues(context);

        Log.Information($"'{context.Request.Path}', '{cspValues.NonceValue}'");
        var source = (env.IsDevelopment() ? "'self' " : "") + $"'nonce-{cspValues.NonceValue}'";

        var baseUri = context.Request.Host.ToUriComponent();
        var baseDomain = context.Request.Host.Host;

        var csp =
            "base-uri 'self'; " +
            "block-all-mixed-content; " +
            "child-src 'self' ; " +
            $"connect-src 'self' wss://{baseDomain}:* www.google-analytics.com region1.google-analytics.com; " +
            "default-src 'self'; " +
            "font-src use.typekit.net fonts.googleapis.com fonts.gstatic.com; " +
            "frame-ancestors 'none'; " +
            "frame-src 'self'; " +
            "form-action 'none'; " +
            "img-src 'self' www.google-analytics.com *.openstreetmap.org data: w3.org/svg/2000; " +
            "manifest-src 'self'; " +
            "media-src 'self'; " +
            "prefetch-src 'self'; " +
            "object-src 'none'; " +
            $"report-to https://{baseUri}/api/CspReporting/UriReport; " +
            $"report-uri https://{baseUri}/api/CspReporting/UriReport; " +
            // The sha-256 hash relates to an inline script added by blazor's javascript
            $"script-src {cspValues.ScriptSrcPart} 'sha256-v8v3RKRPmN4odZ1CWM5gw80QKPCCWMcpNeOmimNL2AA=' 'strict-dynamic' 'unsafe-inline' 'report-sample' 'unsafe-eval' https://www.googletagmanager.com/gtag/js; " +
            "style-src 'self' 'unsafe-inline' 'report-sample' p.typekit.net use.typekit.net fonts.googleapis.com fonts.gstatic.com; " +
            "upgrade-insecure-requests; " +
            "worker-src 'self';";
    //"_content/GoogleAnalytics.Blazor/googleanalytics.blazor.js"
    //"_content/Material.Blazor/material.blazor.min.js"
    //"_content/Website.Lib/js/dioptra.min.js"
    //"_framework/blazor.webassembly.js"

        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
        context.Response.Headers.Add("X-ClientId", "dioptra");
        context.Response.Headers.Add("Referrer-Policy", "no-referrer");
        context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
        context.Response.Headers.Add("Permissions-Policy", "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

        if (cspValues.ApplyContentSecurityPolicy)
        {
            context.Response.Headers.Add("Content-Security-Policy", csp);
        }

        await _next(context);
    }
}