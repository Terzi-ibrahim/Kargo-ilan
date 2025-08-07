using MailKit.Net.Smtp;
using MimeKit;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Data;
using Microsoft.EntityFrameworkCore;


namespace Kargo_İlan.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly KargoDbContext _context;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _senderEmail;
        private readonly string _senderPassword;

        public EmailService(IConfiguration config, KargoDbContext context)
        {
            _config = config;
            _context = context;
            _smtpServer = _config["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            _senderEmail = _config["EmailSettings:SenderEmail"];
            _senderPassword = _config["EmailSettings:SenderPassword"];
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Kargo İlan", _senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();

                // Yandex için SSL kullanılıyor.
                await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_senderEmail, _senderPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine($"E-posta başarıyla gönderildi: {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"E-posta gönderilirken bir hata oluştu: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }


        public async Task SendPasswordResetEmail(string toEmail, string resetToken)
        {
            try
            {
                // E-posta adresine göre CommonUser'ı bul
                var person = await _context.Person
                    .FirstOrDefaultAsync(cu => cu.Email == toEmail);

                if (person == null)
                {
                    // Kullanıcı bulunamazsa logla
                    Console.WriteLine($"E-posta adresiyle eşleşen kullanıcı bulunamadı: {toEmail}");
                    return;
                }

                // CommonId üzerinden Users tablosuna ulaş
                var user = await _context.User
                    .FirstOrDefaultAsync(u => u.Person_id == person.Person_id);

                if (user == null)
                {
                    // Kullanıcı bulunamazsa logla
                    Console.WriteLine($"User verisi bulunamadı: {user.UserName}");
                    return;
                }

                // Token'ı ve süresini veritabanına kaydediyoruz
                user.ResetToken = resetToken;
                user.ResetTokenExpiry = DateTime.Now.AddMinutes(10); // 10 dakika geçerlilik süresi
                await _context.SaveChangesAsync();

                string fullName = $"{user.UserName}";
                string namesurname = $"{person.Name},{person.SurName}";
                var resetLink = $"https://localhost:7202/Account/ChangePassword?token={resetToken}";

                string subject = "Şifre Sıfırlama Talebi";
                string body = $@"
                 <p>Merhaba {namesurname},</p>
                 <p>Sitemizde kullanıcı adınız {fullName} Hesabınız için bir şifre sıfırlama talebi aldık.</p>
                 <p>Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:</p>
                 <p><a href='{resetLink}'>Şifreyi Sıfırla</a></p>
                 <p>Bu bağlantı sadece <strong>10 dakika</strong> boyunca geçerlidir.</p>
                 <p>Herhangi bir sorunuz olursa destek ekibimizle iletişime geçebilirsiniz.</p>
                 <br>
                 <p><strong>Kargo İlan Ekibi</strong></p>";

                // E-posta gönderimi
                await SendEmailAsync(toEmail, subject, body);
            }
            catch (Exception ex)
            {
                // Hata mesajlarını daha ayrıntılı yazdırıyoruz
                Console.WriteLine($"Şifre sıfırlama e-postası gönderilirken bir hata oluştu: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            
                throw;
            }
        }




    }
}
