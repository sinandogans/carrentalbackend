namespace Core.Utilities.EmailService;

public interface IEmailHelper
{
    public void SendEmail(EmailMessage message);
}