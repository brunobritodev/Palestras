namespace DevWeek.Notification
{
    public interface IDomainNotificationMediatorService
    {
        void Notify(DomainNotification notify);
    }
}
