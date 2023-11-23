using AllForRent.Data.Enum;
using AllForRent.Models;
using Microsoft.AspNetCore.Identity;

namespace AllForRent.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                //context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (!context.ProductCards.Any())
                {
                    context.ProductCards.AddRange(new List<ProductCard>()
            {
                new ProductCard()
                {
                    HeadTitle = "Название товара 1",
                    Description = "Описание 1",
                    Price = 111,
                    Image = new ProductCardImages { First = "https://avatars.mds.yandex.net/i?id=3bdee6dc6fedd9cd37fa9713ef498025fdfe6696-9229664-images-thumbs&n=13" },
                    RentTime = RentTime.OneDay,
                    Address = new Address()
                    {
                        City = "Smolensk",
                        State = "NC"
                    }
                 },
                new ProductCard()
                {
                    HeadTitle = "Название товара 2",
                    Description = "Описание 2",
                    Price = 222,
                    Image = new ProductCardImages { First = "https://avatars.mds.yandex.net/i?id=20c346d16c83817bdd1d9ad292813b7d1d6063a6-9848498-images-thumbs&n=13" },
                    RentTime = RentTime.OneWeek,
                    Address = new Address()
                    {
                        City = "Smolensk",
                        State = "WS"
                    }
                 },
                new ProductCard()
                {
                    HeadTitle = "Название товара 3",
                    Description = "Описание 3",
                    Price = 333,
                    Image = new ProductCardImages { First = "https://avatars.mds.yandex.net/i?id=fb47b1af14182670f4e52ed07407f99bb447b940-10448002-images-thumbs&n=13" },
                    RentTime = RentTime.OneMonth,
                    Address = new Address()
                    {
                        City = "Smolensk",
                        State = "GW"
                    }
                 },
                new ProductCard()
                {
                    HeadTitle = "Название товара 4",
                    Description = "Описание 4",
                    Price = 444,
                    Image = new ProductCardImages { First = "https://afftimes.com/wp-content/uploads/2021/02/arenda-akkauntov-fejsbuk-zarabotok.jpg" },
                    RentTime = RentTime.OneYear,
                    Address = new Address()
                    {
                        City = "Смоленск",
                        State = "Смоленская область "
                    }
                }
            });
                    context.SaveChanges();
                }
            }
        }


        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "artbazilix@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "artbazilix",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Smolensk",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}
