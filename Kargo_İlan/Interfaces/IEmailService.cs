namespace Kargo_İlan.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task SendPasswordResetEmail(string toEmail, string resetToken);

    }
}
