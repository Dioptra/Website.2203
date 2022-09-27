namespace Website.Client.ServiceClients;

using System.Threading.Tasks;

public interface INotification
{
    Task Send(ContactMessage message);
    Task Send(RecruitmentEnquiry message);
    Task Send(RealEstateInvestorEnquiry message);
    Task Send(VentureCapitalEnquiry message);
}
