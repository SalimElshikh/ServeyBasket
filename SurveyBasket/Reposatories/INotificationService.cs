namespace SurveyBasket.Reposatories;

public interface INotificationService
{
    Task SendNewPollsNotifications(int? pollId = null);

}
