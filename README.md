# Dioptra Website

[![GitHub issues](https://img.shields.io/github/issues/Dioptra/Website.2203?logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/issues)
[![GitHub forks](https://img.shields.io/github/forks/Dioptra/Website.2203?logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/network/members)
[![GitHub stars](https://img.shields.io/github/stars/Dioptra/Website.2203?logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/stargazers)
[![GitHub stars](https://img.shields.io/github/watchers/Dioptra/Website.2203?logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/watchers)

[![GithubActionsMainPublish](https://img.shields.io/github/workflow/status/Dioptra/Website.2203/GithubActionsRelease?label=actions%20release&logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/actions?query=workflow%3AGithubActionsRelease)
[![GithubActionsDevelop](https://img.shields.io/github/workflow/status/Dioptra/Website.2203/GithubActionsWIP?label=actions%20wip&logo=github&style=flat-square)](https://github.com/Dioptra/Website.2203/actions?query=workflow%3AGithubActionsWIP)

Supplied with a [modified MIT license](https://github.com/Dioptra/Website.2203/blob/main/LICENSE.md). You are restricted from copying website written and visual content
as defined in the license file, however all other software is on an MIT equivalent basis.

Many thanks to [@MarkStega](https://github.com/MarkStega) of [Optimium Health](https://www.optimiumhealth.com/) for contributions to this project and for collaborating with
[@SimonZiegler](https://github.com/simonziegler) of [Dioptra](https://dioptra.tech) on [Material.Blazor](https://github.com/Material-Blazor) since 2020.

<br />

## About The Project
This is the Blazor project (both Blazor Server and Blazor WebAssembly) for the [public website of Dioptra Limited](https://dioptra.tech). This project is open sourced to
provide the developer community with an opinionated Blazor project reference.

The repo makes use of all of the [Material.Blazor nuget packages](https://github.com/Material-Blazor) that are jointly copyright to Dioptra Limited and Optimium Health:

- [Material.Blazor](https://github.com/Material-Blazor/Material.Blazor) is a Razor Class Library integrating Google's Material Design 2 to Blazor.
- [CompressedStaticFiles.AspNetCore](https://github.com/Material-Blazor/CompressedStaticFiles.AspNetCore) serves pre-compressed GZip and Brotli files for ASP.NET projects.
- [GoogleAnalytics.Blazor](https://github.com/Material-Blazor/GoogleAnalytics.Blazor) gives access to Google Analytics for Blazor projects.
- [HttpSecurity.AspNet](https://github.com/Material-Blazor/HttpSecurity.AspNet) manages HTTP security headers include Content Security Policies for ASP.NET projects.

The project does not have detailed documentation, and you are welcome to pick through the repo for inspriation. We're open to being asked questions or receiving comments,
but please don't expect a swift response.

Enjoy!

## Notes

### Dual Mode Blazor

The `Website.Server` project doubles up as both (i) a Blazor Server project and (ii) the server host for a Server Hosted Blazor WebAssembly project. You select between these
by setting the configuration to	`DEBUG_SERVER` or `DEBUG_WEBASSEMBLY`, or likewise `RELEASE_SERVER` or `RELEASE_WEBASSEMBLY`. `Host.cshtml` is coded to detect the selected mode
and to serve the requisite HTML.

### Security

The repo targets a relatively high level of web security by setting the tightest possible Content Security Policy, applying other HTTP security headers as advised resulting
from a different app's penetration test, and applying integrity hashes or nonces to all style sheets and scripts. Furthermore rate limiting is applied to controllers.

### Proprietary Information

This repo is Dioptra's public facing website. While we invite comment on the technologies employed, the website's content is copyright and represents how Dioptra wishes
to express itself. Note that images have been purchased and are subject to third party licenses, so are not available to be copied and re-used on other websites. Dioptra's logo
is copyright.

### Progressive Web App

Dioptra is using this repo to test PWA technology. A future aim is for the website to automatically reload if it detects a new version.