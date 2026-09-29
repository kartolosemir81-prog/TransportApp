using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TransportApp.Data;
using TransportApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=transport.db"));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Transport API",
        Version = "v1",
        Description = "Düzce Akçakoca Servis Takip API'si"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Veritabanı + Gerçek Veriler
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    
    context.Database.EnsureCreated();
    
    // Roller
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    if (!await roleManager.RoleExistsAsync("User"))
        await roleManager.CreateAsync(new IdentityRole("User"));
    
    // Admin kullanıcısı
    var adminEmail = "admin@transport.com";
    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var admin = new ApplicationUser 
        { 
            UserName = adminEmail, 
            Email = adminEmail,
            FullName = "Admin",
            IsAdmin = true
        };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    // Test kullanıcısı
    var userEmail = "user@transport.com";
    if (await userManager.FindByEmailAsync(userEmail) == null)
    {
        var user = new ApplicationUser 
        { 
            UserName = userEmail, 
            Email = userEmail,
            FullName = "Test Kullanıcı"
        };
        await userManager.CreateAsync(user, "User123!");
        await userManager.AddToRoleAsync(user, "User");
    }

    // Gerçek Düzce-Akçakoca servis verileri
    if (!context.Routes.Any())
    {
        context.Routes.AddRange(
            // TAKSİLER
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Taksi",
                VehicleType = "Taksi",
                PlateNumber = "41 T 1234",
                DriverName = "Ahmet Yılmaz",
                DriverPhone = "0532 111 22 33",
                StartPoint = "Akçakoca Taksi Durağı (Merkez)",
                EndPoint = "Düzce Taksi Durağı (Merkez)",
                DepartureTime = "06:00",
                ArrivalTime = "07:15",
                Frequency = "Her 15 dakika",
                Stops = "Akçakoca Merkez, Akçakoca Otogar, Çaylı, Düzce Otogar, Düzce Merkez",
                Price = 180,
                IsActive = true,
                Notes = "Gece 00:00'dan sonra %25 gece tarifesi uygulanır. Yaklaşık 45-60 dakika sürer.",
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Taksi",
                VehicleType = "Taksi",
                PlateNumber = "41 T 2345",
                DriverName = "Mehmet Demir",
                DriverPhone = "0533 222 33 44",
                StartPoint = "Akçakoca Taksi Durağı (Merkez)",
                EndPoint = "Düzce Taksi Durağı (Merkez)",
                DepartureTime = "06:15",
                ArrivalTime = "07:30",
                Frequency = "Her 15 dakika",
                Stops = "Akçakoca Merkez, Akçakoca Otogar, Çaylı, Düzce Otogar, Düzce Merkez",
                Price = 180,
                IsActive = true,
                Notes = "Havalimanı transferi için ek ücret alınır.",
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Taksi",
                VehicleType = "Taksi",
                PlateNumber = "41 T 3456",
                DriverName = "Hasan Kaya",
                DriverPhone = "0534 333 44 55",
                StartPoint = "Akçakoca Taksi Durağı (Merkez)",
                EndPoint = "Düzce Taksi Durağı (Merkez)",
                DepartureTime = "06:30",
                ArrivalTime = "07:45",
                Frequency = "Her 15 dakika",
                Stops = "Akçakoca Merkez, Akçakoca Otogar, Çaylı, Düzce Otogar, Düzce Merkez",
                Price = 180,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            // BELEDİYE SERVİSLERİ
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Belediye Servisi",
                VehicleType = "Belediye Servisi",
                PlateNumber = "41 B 1001",
                DriverName = "Ali Çelik",
                DriverPhone = "0535 444 55 66",
                StartPoint = "Akçakoca Otogar",
                EndPoint = "Düzce Otogar",
                DepartureTime = "07:00",
                ArrivalTime = "08:30",
                Frequency = "Her 30 dakika",
                Stops = "Akçakoca Otogar, Akçakoca Merkez, Çaylı, Düzce Otogar",
                Price = 45,
                IsActive = true,
                Notes = "Hafta sonu seferleri 1 saat sonra başlar. Öğrenci indirimi uygulanır.",
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Belediye Servisi",
                VehicleType = "Belediye Servisi",
                PlateNumber = "41 B 1002",
                DriverName = "Mustafa Şahin",
                DriverPhone = "0536 555 66 77",
                StartPoint = "Akçakoca Otogar",
                EndPoint = "Düzce Otogar",
                DepartureTime = "07:30",
                ArrivalTime = "09:00",
                Frequency = "Her 30 dakika",
                Stops = "Akçakoca Otogar, Akçakoca Merkez, Çaylı, Düzce Otogar",
                Price = 45,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca - Düzce Belediye Servisi",
                VehicleType = "Belediye Servisi",
                PlateNumber = "41 B 1003",
                DriverName = "Emre Aydın",
                DriverPhone = "0537 666 77 88",
                StartPoint = "Akçakoca Otogar",
                EndPoint = "Düzce Otogar",
                DepartureTime = "08:00",
                ArrivalTime = "09:30",
                Frequency = "Her 30 dakika",
                Stops = "Akçakoca Otogar, Akçakoca Merkez, Çaylı, Düzce Otogar",
                Price = 45,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            // MİNİBÜSLER
            new RouteInfo
            {
                RouteName = "Akçakoca Merkez - Düzce Merkez Minibüs",
                VehicleType = "Minibüs",
                PlateNumber = "41 M 2001",
                DriverName = "Fatma Öztürk",
                DriverPhone = "0538 777 88 99",
                StartPoint = "Akçakoca Merkez",
                EndPoint = "Düzce Merkez",
                DepartureTime = "08:00",
                ArrivalTime = "09:30",
                Frequency = "Her 20 dakika",
                Stops = "Akçakoca Merkez, Çaylı, Düzce Merkez",
                Price = 60,
                IsActive = true,
                Notes = "Minibüsler 18 kişiliktir. Dolu dolu gider.",
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca Merkez - Düzce Merkez Minibüs",
                VehicleType = "Minibüs",
                PlateNumber = "41 M 2002",
                DriverName = "Ayşe Yıldız",
                DriverPhone = "0539 888 99 00",
                StartPoint = "Akçakoca Merkez",
                EndPoint = "Düzce Merkez",
                DepartureTime = "08:20",
                ArrivalTime = "09:50",
                Frequency = "Her 20 dakika",
                Stops = "Akçakoca Merkez, Çaylı, Düzce Merkez",
                Price = 60,
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new RouteInfo
            {
                RouteName = "Akçakoca Merkez - Düzce Merkez Minibüs",
                VehicleType = "Minibüs",
                PlateNumber = "41 M 2003",
                DriverName = "Ali Koç",
                DriverPhone = "0530 999 00 11",
                StartPoint = "Akçakoca Merkez",
                EndPoint = "Düzce Merkez",
                DepartureTime = "08:40",
                ArrivalTime = "10:10",
                Frequency = "Her 20 dakika",
                Stops = "Akçakoca Merkez, Çaylı, Düzce Merkez",
                Price = 60,
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        );
        await context.SaveChangesAsync();
    }

    // Örnek mesajlar
    if (!context.ContactMessages.Any())
    {
        context.ContactMessages.AddRange(
            new ContactMessage
            {
                Name = "Zeynep Arslan",
                Email = "zeynep@email.com",
                Subject = "Servi Saati Hakkında",
                Message = "Akçakoca'dan Düzce'ye son servi saati kaçta? Gece geç saatte servis var mı?",
                SentAt = DateTime.Now.AddDays(-2),
                IsRead = false
            },
            new ContactMessage
            {
                Name = "Burak Doğan",
                Email = "burak@email.com",
                Subject = "Ücret Bilgisi",
                Message = "Öğrenci indirimi var mı? Öğrenci belgesiyle ücret indirimi uyguluyor musunuz?",
                SentAt = DateTime.Now.AddDays(-1),
                IsRead = false
            },
            new ContactMessage
            {
                Name = "Selin Yılmaz",
                Email = "selin@email.com",
                Subject = "Teşekkürler",
                Message = "Servisleriniz çok düzenli ve zamanında. Teşekkür ederim.",
                SentAt = DateTime.Now.AddHours(-5),
                IsRead = true
            }
        );
        await context.SaveChangesAsync();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transport API v1");
    options.RoutePrefix = "swagger";
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
