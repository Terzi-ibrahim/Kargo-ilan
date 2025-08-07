using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Account;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Kargo_İlan.Services
{
    public class AccountService : IAccountService
    {

        private readonly KargoDbContext _context;
        private readonly IEmailService _emailService;

        public AccountService( KargoDbContext context, IEmailService emailService)
        {

            _context = context;
            _emailService = emailService; 
        }
        public async Task<(bool Success, string? ErrorMessage, ClaimsPrincipal? Principal)> LoginAsync(Login model)
        {
            bool isEmail = IsEmail(model.EmailOrUsername);

            var userQuery = _context.User.Include(u => u.Person).AsQueryable();

            User? user = isEmail
                ? await userQuery.FirstOrDefaultAsync(u => u.Person.Email == model.EmailOrUsername)
                : await userQuery.FirstOrDefaultAsync(u => u.UserName == model.EmailOrUsername);

            if (user == null || user.Person == null)
                return (false, "Geçersiz e-posta veya kullanıcı adı.", null);

            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                return (false, "Şifre yanlış.", null);

            var roleUsers = await _context.UserRole
                .Include(r => r.Role)
                .Where(r => r.User_id == user.User_id)
                .ToListAsync();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.User_id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("LoginTime", DateTime.UtcNow.ToString("o")),             
            };

            var hasCompany = await _context.UserCompany
                .AnyAsync(cu => cu.User_id == user.User_id && !cu.IsDeleted);

            claims.Add(new Claim("HasCompany", hasCompany.ToString().ToLower()));

            foreach (var ru in roleUsers)
            {
                if (ru.Role != null)
                    claims.Add(new Claim(ClaimTypes.Role, ru.Role.RoleName));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            return (true, null, principal);
        }


        private bool IsEmail(string input)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(input);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool Success, string? ErrorMessage)> RegisterAsync(Register model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 0. Benzersizlik kontrolleri
                var emailExists = await _context.Person.AnyAsync(p => p.Email == model.Email && !p.IsDeleted);
                var phoneExists = await _context.Person.AnyAsync(p => p.PhoneNumber == model.PhoneNumber && !p.IsDeleted);
                var userNameExists = await _context.User.AnyAsync(u => u.UserName == model.UserName && !u.IsDeleted);

                if (emailExists)
                    return (false, "Bu e-posta adresi zaten kayıtlı.");

                if (phoneExists)
                    return (false, "Bu telefon numarası zaten kayıtlı.");

                if (userNameExists)
                    return (false, "Bu kullanıcı adı zaten alınmış.");

                // 1. Yeni Person oluştur
                var newPerson = new Person
                {
                    Name = model.Name,
                    SurName = model.Surname,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    CreatedBy = 0,
                    IsDeleted = false
                };

                await _context.Person.AddAsync(newPerson);
                await _context.SaveChangesAsync();

                // 2. Yeni User oluştur
                var newUser = new User
                {
                    UserName = model.UserName,
                    Password = HashPassword(model.Password),
                    Person_id = newPerson.Person_id,
                    CreatedBy = 0,
                    IsDeleted = false
                };

                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();

                // 3. Rol ata
                var userRole = new UserRole
                {
                    User_id = newUser.User_id,
                    Role_id = 10, // Bireysel kullanıcı rolü
                    CreatedBy = 0
                };

                await _context.UserRole.AddAsync(userRole);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Bir hata oluştu: {ex.Message}");
            }
        }


        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

       

        public async Task<bool> HandleForgotPasswordAsync(string email)
        {
            var commonUser = await _context.Person
                .FirstOrDefaultAsync(cu => cu.Email == email);

            if (commonUser == null)
            {
                return false; 
            }

            // Kullanıcı bulundu, reset token oluşturuluyor
            string resetToken = Guid.NewGuid().ToString();

            // Reset token'ı ve reset link'i e-posta ile gönderiyoruz
            await _emailService.SendPasswordResetEmail(email, resetToken);

            return true; 
        }
        public async Task<bool> ValidateResetTokenAsync(string token)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.ResetToken == token && u.ResetTokenExpiry > DateTime.Now);

            return user != null;  // Eğer kullanıcı varsa geçerli bir token
        }

        public async Task<bool> ChangePasswordAsync(ChangePassword model)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.ResetToken == model.Token && u.ResetTokenExpiry > DateTime.Now);

            if (user == null)
            {
                return false;  // Geçersiz veya süresi dolmuş token
            }

            // Yeni şifreyi hash'le
            user.Password = HashPassword(model.Password);
            user.ResetToken = null;  // Token'ı sıfırlayın
            user.ResetTokenExpiry = null;  // Token süresini sıfırlayın

            // Kullanıcıyı güncelle
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<ProfileViewModel> GetProfileAsync(int userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.User_id == userId);
            if (user == null)
            {
                return null;  // Kullanıcı bulunamadı
            }

            var person = await _context.Person.FirstOrDefaultAsync(p => p.Person_id == user.Person_id);
            if (person == null)
            {
                return null;  // İlgili kişi bulunamadı
            }

            var activeFreightCount = await _context.Freight
                .CountAsync(f => f.User_id == userId && !f.IsDeleted);

            var offeredFreightCount = await _context.Offer
                .Where(o => o.User_id == userId && !o.IsDeleted)
                .Select(o => o.Freight_id)
                .Distinct()
                .CountAsync();

            var profileViewModel = new ProfileViewModel
            {
                Profile = new Profile
                {
                    Name = person.Name,
                    Surname = person.SurName,
                    UserName = user.UserName,
                    PhoneNumber = person.PhoneNumber,
                    Email = person.Email,
                    Address = person.Address,
                    UserId = userId,
                    ActiveFreight = activeFreightCount,
                    OfferedFreight = offeredFreightCount
                },
                UserChangePassword = new UserChangePassword()
            };

            return profileViewModel;
        }      

        public async Task<bool> UpdateProfileAsync(int userId, Profile profileDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.User_id == userId);
            if (user == null)
            {
                return false;  // Kullanıcı bulunamadı
            }

            var person = await _context.Person.FirstOrDefaultAsync(p => p.Person_id == user.Person_id);
            if (person == null)
            {
                return false;  // Kişi bilgisi bulunamadı
            }

            // Güncelleme işlemi
            person.Name = profileDto.Name;
            person.SurName = profileDto.Surname;
            person.PhoneNumber = profileDto.PhoneNumber;
            person.Email = profileDto.Email;
            person.Address = profileDto.Address;
            user.UserName = profileDto.UserName;

            await _context.SaveChangesAsync();
            return true;
        }     
        public async Task<ProfileViewModel> BuildProfileViewModelAsync(int userId)
        {
            var user = await _context.User.FindAsync(userId);
            var person = await _context.Person.FindAsync(user.Person_id);

            return new ProfileViewModel
            {
                Profile = new Profile
                {
                    UserId = userId,
                    Name = person.Name,
                    Surname = person.SurName,
                    UserName = user.UserName,
                    PhoneNumber = person.PhoneNumber,
                    Email = person.Email,
                    Address = person.Address
                },
                UserChangePassword = new UserChangePassword()
            };
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _context.User.FindAsync(userId);

            // Mevcut şifreyi doğrula
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.Password))
                return false;

            // Yeni şifreyi hash'le
            user.Password = HashPassword(newPassword);
            await _context.SaveChangesAsync();

            return true;
        }

     


    }
}
