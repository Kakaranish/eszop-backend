namespace NotificationService.Application
{
    public class NotificationSettings
    {
        public string CleanupJobCronPattern { get; init; }
        public int ExpirationInMinutes { get; init; }
    }
}
