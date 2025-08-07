using Kargo_İlan.Data;
using Kargo_İlan.DTOs.Account;
using Kargo_İlan.DTOs.Company;
using Kargo_İlan.Interfaces;
using Kargo_İlan.Models;
using Microsoft.EntityFrameworkCore;


namespace Kargo_İlan.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly KargoDbContext _context;

        public CompanyService(KargoDbContext context)
        {
            _context = context;

        }
        public async Task<(bool Success, string? ErrorMessage)> RegisterCompanyAsync(RegisterCompany model, int userId)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.User_id == userId);
            if (user == null)
                return (false, "Kullanıcı bilgisi bulunamadı.");

            var userRole = await _context.UserRole.FirstOrDefaultAsync(ur => ur.User_id == userId && ur.Role_id == 30);
            if (userRole == null)
                return (false, "Şirket oluşturmak için gerekli role sahip değilsiniz.");

            // Kullanıcının daha önce şirketi var mı?
            var userCompanyCheck = await _context.UserCompany
                .FirstOrDefaultAsync(uc => uc.User_id == userId);

            if (userCompanyCheck != null && !userCompanyCheck.IsDeleted)
            {
                return (false, "Her kullanıcı yalnızca bir şirkete ait olabilir.");
            }

            // Şirket adı veya vergi numarasıyla daha önce kayıt yapılmış mı?
            var existingCompany = await _context.Company
                .FirstOrDefaultAsync(c => c.CompanyName == model.CompanyName || c.TaxNumber == model.TaxNumber);

            Company company;

            if (existingCompany != null)
            {
                if (existingCompany.IsDeleted)
                {
                    // Şirket daha önce silinmişse geri getir
                    existingCompany.IsDeleted = false;
                    existingCompany.UpdateAt = DateTime.UtcNow;
                    existingCompany.UpdatedBy = userId;
                    existingCompany.CompanyName = model.CompanyName;
                    existingCompany.CompanyPhone = model.CompanyPhone;
                    existingCompany.CompanyEmail = model.CompanyEmail;
                    existingCompany.CompanyAdress = model.CompanyAdress;
                    existingCompany.TaxNumber = model.TaxNumber;

                    _context.Company.Update(existingCompany);
                    await _context.SaveChangesAsync();

                    company = existingCompany;
                }
                else
                {
                    return (false, "Bu şirket adı veya vergi numarası ile bir şirket zaten kaydedilmiş.");
                }
            }
            else
            {
                company = new Company
                {
                    CompanyName = model.CompanyName,
                    CompanyPhone = model.CompanyPhone,
                    CompanyEmail = model.CompanyEmail,
                    CompanyAdress = model.CompanyAdress,
                    TaxNumber = model.TaxNumber,
                    CreatedBy = userId
                };

                await _context.Company.AddAsync(company);
                await _context.SaveChangesAsync();
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (userCompanyCheck != null && userCompanyCheck.IsDeleted)
                {
                    // Silinmiş kayıt varsa, geri getir ve yeni şirketle eşleştir
                    userCompanyCheck.IsDeleted = false;
                    userCompanyCheck.UpdateAt = DateTime.UtcNow;
                    userCompanyCheck.UpdatedBy = userId;
                    userCompanyCheck.Company_id = company.Company_id;

                    _context.UserCompany.Update(userCompanyCheck);
                    await _context.SaveChangesAsync();
                }
                else if (userCompanyCheck == null)
                {
                    // İlk kez ekleniyorsa
                    var userCompany = new UserCompany
                    {
                        Company_id = company.Company_id,
                        User_id = userId,
                        CreatedBy = userId
                    };

                    await _context.UserCompany.AddAsync(userCompany);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Bir hata oluştu: {ex.Message}");
            }
        }
        public async Task<CompanyProfile?> GetCompanyProfileByUserIdAsync(int userId)
        {
            var companyRelation = await _context.UserCompany
                .Include(uc => uc.Company)
                .Where(uc => uc.User_id == userId && !uc.IsDeleted)
                .FirstOrDefaultAsync();

            if (companyRelation == null)
                return null;

            var company = companyRelation.Company;

            // Şirkete bağlı aktif kullanıcıları al
            var companyUsers = await _context.UserCompany
                .Include(uc => uc.User)
                .ThenInclude(u => u.Person)
                .Where(uc => uc.Company_id == company.Company_id && !uc.IsDeleted)
                .ToListAsync();         

            // Kullanıcı Id'lerini çıkar
            var userIds = companyUsers.Select(uc => uc.User_id).ToList();

            // Aktif ilan sayısını al (Kullanıcı bazında)
            var activeFreight = await _context.Freight
                .CountAsync(f => userIds.Contains(f.User_id) && !f.IsDeleted);

            // Kullanıcılara ait verilen teklifler (her kullanıcı farklı ilana teklif vermiş olabilir)
            var offeredFreight = await _context.Offer
                .Where(o => userIds.Contains(o.User_id))
                .Select(o => o.Freight_id)
                .Distinct()
                .CountAsync();

            // Şirket bilgilerini ve aktif kullanıcı sayısını içeren DTO döndür
            return new CompanyProfile
            {
                CompanyName = company.CompanyName,
                CompanyPhone = company.CompanyPhone,
                CompanyEmail = company.CompanyEmail,
                CompanyAdress = company.CompanyAdress,
                TaxNumber = company.TaxNumber,
                ActiveFreight = activeFreight,
                OfferedFreight = offeredFreight,
                CompanyUsers = companyUsers.Select(uc => new CompanyUserDto
                {
                    User_id = uc.User_id,  // Burada kullanıcı ID'sini alıyoruz
                    Name = uc.User.Person.Name,
                    SurName = uc.User.Person.SurName,
                    IsDeleted = uc.IsDeleted,
                    Company_id = uc.Company_id
                }).ToList()
            };
        }



        public async Task<bool> UpdateCompanyProfileAsync(int userId, CompanyProfile model)
        {
            // Kullanıcı ile ilişkili şirketi bul
            var userCompany = await _context.UserCompany
                .Include(uc => uc.Company)
                .Include(uc => uc.User)
                    .ThenInclude(u => u.Person) // Ortak bilgiler (Email vb.)
                .FirstOrDefaultAsync(uc => uc.User_id == userId && !uc.IsDeleted);

            if (userCompany == null || userCompany.Company == null)
                return false;

            // Şirket bilgilerini güncelle
            var company = userCompany.Company;
            company.CompanyName = model.CompanyName;
            company.CompanyPhone = model.CompanyPhone;
            company.CompanyEmail = model.CompanyEmail;
            company.CompanyAdress = model.CompanyAdress;
            company.TaxNumber = model.TaxNumber;

            // Güncelleyen kişi ve zaman
            company.UpdateAt = DateTime.UtcNow;
            company.UpdatedBy = userId;

            _context.Company.Update(company);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<List<User>> GetCompanyUsersAsync()
        {
            // "CompanyUser" rolünü al
            var companyRoleId = await _context.Role
                .Where(r => r.RoleName == "CompanyUser")
                .Select(r => r.Role_id)
                .FirstOrDefaultAsync();

            // "CompanyUser" rolüne sahip kullanıcıları al
            var companyUsers = await _context.UserRole
                .Where(ru => ru.Role_id == companyRoleId)
                .Select(ru => ru.User)
                .AsNoTracking() // Okuma amacıyla, daha hızlı performans için
                .ToListAsync();

            return companyUsers;
        }
        public async Task<(bool Success, string ErrorMessage)> AddCompanyUserAsync(AddCompanyUser model, int loggedInUserId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 0. Benzersizlik kontrolleri (IsDeleted = false olanlar için)

                var emailExists = await _context.Person
                    .AnyAsync(p => p.Email == model.Email && !p.IsDeleted);

                var phoneExists = await _context.Person
                    .AnyAsync(p => p.PhoneNumber == model.PhoneNumber && !p.IsDeleted);

                var userNameExists = await _context.User
                    .AnyAsync(u => u.UserName == model.UserName && !u.IsDeleted);

                var person = await _context.Person.FirstOrDefaultAsync(p => p.Email == model.Email);

                // 0.1 Telefon numarası başka kişide varsa hata
                if (phoneExists && (person == null || person.PhoneNumber != model.PhoneNumber))
                    return (false, "Bu telefon numarası zaten kayıtlıdır.");

                // 0.2 E-posta başka kişide varsa hata
                if (emailExists && person == null)
                    return (false, "Bu e-posta adresi zaten kayıtlıdır.");

                // 0.3 Kullanıcı adı başka kullanıcıya aitse hata
                var existingUser = await _context.User
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && !u.IsDeleted);

                if (existingUser != null)
                    return (false, "Bu kullanıcı adı zaten alınmış.");

                // 1. Person kontrolü
                if (person == null)
                {
                    person = new Person
                    {
                        Name = model.Name,
                        SurName = model.Surname,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        Address = model.Address,
                        CreatedBy = loggedInUserId,
                        IsDeleted = false,
                    };
                    await _context.Person.AddAsync(person);
                }
                else
                {
                    if (person.IsDeleted)
                    {
                        person.IsDeleted = false;
                        person.DeletedAt = null;
                    }
                    person.Name = model.Name;
                    person.SurName = model.Surname;
                    person.Address = model.Address;
                    person.PhoneNumber = model.PhoneNumber;
                    person.UpdatedBy = loggedInUserId;
                    _context.Person.Update(person);
                }

                await _context.SaveChangesAsync();

                // 2. User kontrolü
                var user = await _context.User.FirstOrDefaultAsync(u => u.Person_id == person.Person_id);
                if (user == null)
                {
                    user = new User
                    {
                        UserName = model.UserName,
                        Password = HashPassword(model.Password),
                        Person_id = person.Person_id,
                        CreatedBy = loggedInUserId,
                        IsDeleted = false,
                        CreateAt = DateTime.Now
                    };
                    await _context.User.AddAsync(user);
                }
                else
                {
                    if (user.IsDeleted)
                    {
                        user.IsDeleted = false;
                        user.DeletedAt = null;
                    }

                    user.UserName = model.UserName;
                    if (!string.IsNullOrWhiteSpace(model.Password))
                        user.Password = HashPassword(model.Password);

                    user.UpdatedBy = loggedInUserId;
                    user.UpdateAt = DateTime.Now;

                    _context.User.Update(user);
                }

                await _context.SaveChangesAsync();

                // 3. Giriş yapan kullanıcının şirketini bul
                var creatorCompany = await _context.UserCompany
                    .FirstOrDefaultAsync(uc => uc.User_id == loggedInUserId && !uc.IsDeleted);

                if (creatorCompany == null)
                    return (false, "Giriş yapan kullanıcının şirket bilgisi bulunamadı.");

                // 4. Yeni kullanıcıyı şirkete ata
                var existingUserCompany = await _context.UserCompany
                    .FirstOrDefaultAsync(uc => uc.User_id == user.User_id);

                if (existingUserCompany == null)
                {
                    var userCompany = new UserCompany
                    {
                        User_id = user.User_id,
                        Company_id = creatorCompany.Company_id,
                        CreateAt = DateTime.UtcNow,
                        CreatedBy = loggedInUserId,
                        IsDeleted = false
                    };
                    await _context.UserCompany.AddAsync(userCompany);
                }
                else if (existingUserCompany.IsDeleted)
                {
                    existingUserCompany.IsDeleted = false;
                    existingUserCompany.UpdateAt = DateTime.UtcNow;
                    existingUserCompany.UpdatedBy = loggedInUserId;
                    _context.UserCompany.Update(existingUserCompany);
                }

                // 5. Kullanıcıya "CompanyUser" rolünü ver
                var companyUserRole = await _context.Role.FirstOrDefaultAsync(r => r.RoleName == "CompanyUser");

                if (companyUserRole == null)
                    return (false, "CompanyUser rolü tanımlı değil.");

                var hasRole = await _context.UserRole
                    .AnyAsync(ru => ru.User_id == user.User_id && ru.Role_id == companyUserRole.Role_id);

                if (!hasRole)
                {
                    var roleUser = new UserRole
                    {
                        User_id = user.User_id,
                        Role_id = companyUserRole.Role_id
                    };
                    await _context.UserRole.AddAsync(roleUser);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, string.Empty);
            }
            catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("UK_Person_PhoneNumber") == true)
            {
                await transaction.RollbackAsync();
                return (false, "Bu telefon numarası sistemde zaten kayıtlıdır.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"Bir hata oluştu: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            // BCrypt ile şifreyi hashleme
            return BCrypt.Net.BCrypt.HashPassword(password);
        }




        public async Task<CompanyUserDetail> GetUserDetailAsync(int userId)
        {
            var userCompany = await _context.UserCompany
                .Include(uc => uc.User)
                .ThenInclude(u => u.Person)
                .FirstOrDefaultAsync(uc => uc.User_id == userId && !uc.IsDeleted);

            if (userCompany == null)
                return null;

            var freights = await _context.Freight
                .Where(f => f.User_id == userId && !f.IsDeleted)
                .Include(f => f.Vehicle_Type)
                .Include(f => f.Cargo_Type)
                .Include(f => f.Listing_Status)
                .Include(f => f.Category_Type)
                .Include(f => f.Yukleme_il)
                .Include(f => f.Yukleme_ilce)
                .Include(f => f.Varis_il)
                .Include(f => f.Varis_ilce)
                .ToListAsync();

            var freightIds = freights.Select(f => f.Freight_id).ToList();

            // 1. Kullanıcının kendi ilanlarına gelen teklifler
            var offersToUserFreights = await _context.Offer
                .Where(o => freightIds.Contains(o.Freight_id) && !o.IsDeleted)
                .Include(o => o.User)
                    .ThenInclude(u => u.Person)
                .Include(o => o.User)
                    .ThenInclude(u => u.UserCompanies)
                        .ThenInclude(uc => uc.Company)
                .ToListAsync();

            // 2. Kullanıcının başka ilanlara verdiği teklifler
            var userOffers = await _context.Offer
                .Where(o => o.User_id == userId && !o.IsDeleted)
                .Include(o => o.Freight)
                .Include(o => o.User)
                    .ThenInclude(u => u.Person)
                .Include(o => o.User)
                    .ThenInclude(u => u.UserCompanies)
                        .ThenInclude(uc => uc.Company)
                .ToListAsync();

            var viewModel = new CompanyUserDetail
            {
                UserName = userCompany.User.UserName,
                Name = userCompany.User.Person.Name,
                Surname = userCompany.User.Person.SurName,

                Listings = freights.Select(f => new FreightIndex
                {
                    Freight_id = f.Freight_id,
                    Title = f.Title,
                    CargoType = f.Cargo_Type.CargoName,
                    ListingStatus = f.Listing_Status.Status,
                    VehicleType = f.Vehicle_Type.VehicleName,
                    Category = f.Category_Type.CategoryName,
                    OlusturulmaTarihi = f.CreateAt,
                    YuklemeIl = f.Yukleme_il.ProvinceName,
                    YuklemeIlce = f.Yukleme_ilce.DistrictName,
                    VarisIlce = f.Varis_ilce.DistrictName,
                    VarisIl = f.Varis_il.ProvinceName,
                    Offers = offersToUserFreights
                        .Where(o => o.Freight_id == f.Freight_id)
                        .Select(o => new OfferIndex
                        {
                            Offer_id = o.Offer_id,
                            Description = o.Description,
                            Price = o.Price,
                            Date = o.CreateAt.Date,
                            OfferUserName = $"{o.User.Person.Name} {o.User.Person.SurName}",
                            OfferUserCompany = o.User.UserCompanies
                                .FirstOrDefault(uc => !uc.IsDeleted)?.Company?.CompanyName ?? "-"
                        }).ToList()
                }).ToList(),

                // Kullanıcının verdiği teklifler
                Offers = userOffers.Select(o => new OfferIndex
                {
                    Offer_id = o.Offer_id,
                    Description = o.Description,
                    Price = o.Price,
                    Date = o.CreateAt.Date,
                    OfferUserName = $"{o.User.Person.Name} {o.User.Person.SurName}",
                    OfferUserCompany = o.User.UserCompanies
                    .FirstOrDefault(uc => !uc.IsDeleted).Company.CompanyName,
                    FreightTitle = o.Freight.Title,
                    Freight_id = o.Freight?.Freight_id ?? 0
                }).ToList()


            };

            return viewModel;
        }
        public async Task<bool> DeleteCompanyUserAsync(int userCompanyId, int companyUserId)
        {
            var companyUser = await _context.UserCompany
                .FirstOrDefaultAsync(u => u.User_id == companyUserId && u.Company_id == userCompanyId && !u.IsDeleted);

            if (companyUser == null)
            {
                return false;
            }

            companyUser.IsDeleted = true;
            companyUser.DeletedAt = DateTime.UtcNow;
            _context.UserCompany.Update(companyUser);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<CompanyUserDto> GetCompanyUserByIdAsync(int companyUserId)
        {
            var user = await _context.UserCompany
                .Where(u => u.User_id == companyUserId)
                .Select(u => new CompanyUserDto
                {
                    User_id = u.User_id,
                    Company_id = u.Company_id,
                    Name = u.User.Person.Name,
                    SurName = u.User.Person.SurName,
                    Email = u.User.Person.Email,
                    Phone = u.User.Person.PhoneNumber,
                    IsDeleted = u.IsDeleted
                })
                .FirstOrDefaultAsync();

            return user;
        }
        public async Task<(bool success, string message)> DeleteCompanyAsync(int userId, int updatedByUserId)
        {
            var userCompany = await _context.UserCompany
                .FirstOrDefaultAsync(uc => uc.User_id == userId && !uc.IsDeleted);

            if (userCompany == null)
            {
                return (false, "Kullanıcıya ait bir şirket bulunamadı veya şirket zaten silinmiş.");
            }

            int companyId = userCompany.Company_id;

            // Şirkete ait tüm aktif UserCompany kayıtlarını al ve işaretle
            var relatedUserCompanies = await _context.UserCompany
                .Where(uc => uc.Company_id == companyId && !uc.IsDeleted)
                .ToListAsync();

            foreach (var uc in relatedUserCompanies)
            {
                uc.IsDeleted = true;
                uc.DeletedAt = DateTime.UtcNow;
                uc.UpdatedBy = updatedByUserId;
            }

            // Şirketi soft-delete yap
            var company = await _context.Company
                .FirstOrDefaultAsync(c => c.Company_id == companyId && !c.IsDeleted);

            if (company == null)
            {
                // Şirket zaten silinmiş olabilir, ama bu bir hata değil
                await _context.SaveChangesAsync();
                return (true, "Şirket ve tüm kullanıcıları başarıyla silindi.");
            }

            company.IsDeleted = true;
            company.DeletedAt = DateTime.UtcNow;
            company.UpdatedBy = updatedByUserId;

            await _context.SaveChangesAsync();

            return (true, "Şirket ve tüm kullanıcıları başarıyla silindi.");
        }


    }
}
