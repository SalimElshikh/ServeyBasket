namespace ApplicationLayer.Reposatories;

public interface INotificationService
{
    Task SendNewPollsNotifications(int? pollId = null);

}
