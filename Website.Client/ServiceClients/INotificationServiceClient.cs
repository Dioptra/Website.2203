namespace Website.Client.ServiceClients;

using System.Threading.Tasks;

public interface INotificationServiceClient
{
    Task SendNotification(ContactMessage message);
    Task SendNotification(RecruitmentEnquiry message);
    Task SendNotification(RealEstateInvestorEnquiry message);
    Task SendNotification(VentureCapitalEnquiry message);
}
