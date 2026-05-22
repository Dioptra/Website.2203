using Microsoft.AspNetCore.Mvc;
using Website.Client;

namespace Website.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class NotificationController(INotification notificationService) : ControllerBase
{
    [HttpPost("PostContactMessage")]
    public async Task PostContactMessage([FromBody] ContactMessage contactMessage)
    {
        await notificationService.Send(contactMessage).ConfigureAwait(false);
    }

    [HttpPost("PostRealEstateInvestorEnquiry")]
    public async Task PostRealEstateInvestorEnquiry([FromBody] RealEstateInvestorEnquiry realEstateInvestorEnquiry)
    {
        await notificationService.Send(realEstateInvestorEnquiry).ConfigureAwait(false);
    }

    [HttpPost("PostRecruitmentEnquiry")]
    public async Task PostRecruitmentEnquiry([FromBody] RecruitmentEnquiry recruitmentEnquiry)
    {
        await notificationService.Send(recruitmentEnquiry).ConfigureAwait(false);
    }

    [HttpPost("PostVentureCapitalEnquiry")]
    public async Task PostVentureCapitalEnquiry([FromBody] VentureCapitalEnquiry ventureCapitalEnquiry)
    {
        await notificationService.Send(ventureCapitalEnquiry).ConfigureAwait(false);
    }
}