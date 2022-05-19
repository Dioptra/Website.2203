namespace Website.Lib;

using System.Threading.Tasks;

public interface ITeamsNotificationService
{
    Task SendNotification(IMessage message);
}
