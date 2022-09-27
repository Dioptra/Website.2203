using Microsoft.AspNetCore.Mvc;
using Website.Client;
using Website.Client.ServiceClients;

namespace Website.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class NotificationController : ControllerBase
{
    public readonly INotification _notificationService;


    public NotificationController(INotification notificationService)
    {
        _notificationService = notificationService;
    }


    [HttpPost("PostContactMessage")]
    public async Task PostContactMessage([FromBody] ContactMessage contactMessage)
    {
        await _notificationService.Send(contactMessage).ConfigureAwait(false);
    }


    [HttpPost("PostRealEstateInvestorEnquiry")]
    public async Task PostRealEstateInvestorEnquiry([FromBody] RealEstateInvestorEnquiry realEstateInvestorEnquiry)
    {
        await _notificationService.Send(realEstateInvestorEnquiry).ConfigureAwait(false);
    }


    [HttpPost("PostRecruitmentEnquiry")]
    public async Task PostRecruitmentEnquiry([FromBody] RecruitmentEnquiry recruitmentEnquiry)
    {
        await _notificationService.Send(recruitmentEnquiry).ConfigureAwait(false);
    }


    [HttpPost("PostVentureCapitalEnquiry")]
    public async Task PostVentureCapitalEnquiry([FromBody] VentureCapitalEnquiry ventureCapitalEnquiry)
    {
        await _notificationService.Send(ventureCapitalEnquiry).ConfigureAwait(false);
    }
}