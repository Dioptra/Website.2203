using System.Threading.Tasks;

using Website.Client;

namespace Website.Server.Services;

public interface INotificationService
{
    Task SendNotification(ContactMessage message);
    Task SendNotification(RecruitmentEnquiry message);
    Task SendNotification(RealEstateInvestorEnquiry message);
    Task SendNotification(VentureCapitalEnquiry message);
}
