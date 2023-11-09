using AllForRent.Data.Enum;
using AllForRent.Models;

namespace AllForRent.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.ProductCards.Any())
                {
                    context.ProductCards.AddRange(new List<ProductCard>()
                    {
                        new ProductCard()
                        {
                            Name = "Название товара 1",
                            Description = "Описание 1",
                            Seller = "Вася",
                            ProductPrice = "111",
                            Image = "https://avatars.mds.yandex.net/i?id=3bdee6dc6fedd9cd37fa9713ef498025fdfe6696-9229664-images-thumbs&n=13"
                         },
                        new ProductCard()
                        {
                            Name = "Название товара 2",
                            Description = "Описание 2",
                            Seller = "Дэнис",
                            ProductPrice = "222",
                            Image = "https://avatars.mds.yandex.net/i?id=20c346d16c83817bdd1d9ad292813b7d1d6063a6-9848498-images-thumbs&n=13"
                         },
                        new ProductCard()
                        {
                            Name = "Название товара 3",
                            Description = "Описание 3",
                            Seller = "Миха",
                            ProductPrice = "333",
                            Image = "https://avatars.mds.yandex.net/i?id=fb47b1af14182670f4e52ed07407f99bb447b940-10448002-images-thumbs&n=13"
                         },
                        new ProductCard()
                        {
                            Name = "Название товара 4",
                            Description = "Описание 4",
                            Seller = "Лёха",
                            ProductPrice = "444",
                            Image = "https://afftimes.com/wp-content/uploads/2021/02/arenda-akkauntov-fejsbuk-zarabotok.jpg"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.AppUsers.Any())
                {
                    context.AppUsers.AddRange(new List<AppUser>()
                    {
                        new AppUser()
                        {
                            Name = "Пользователь Гена"
                        },
                        new AppUser()
                        {
                            Name = "Пользователь Лена"
                        },
                        new AppUser()
                        {
                            Name = "Пользователь Маша"
                        },
                        new AppUser()
                        {
                            Name = "Пользователь Саша"
                        }
                    });
                    context.SaveChanges();
                }
                if (!context.AppSellers.Any())
                {
                    context.AppSellers.AddRange(new List<AppSeller>()
                    {
                        new AppSeller()
                        {
                            Name = "Пользователь 1",
                            Description = "Описание продавца 1",
                            RentTime = RentTime.OneWeek
                        },
                        new AppSeller()
                        {
                            Name = "Пользователь 2",
                            Description = "Описание продавца 2",
                            RentTime = RentTime.TwoWeek
                        },
                        new AppSeller()
                        {
                            Name = "Пользователь 3",
                            Description = "Описание продавца 3",
                            RentTime = RentTime.ThreeWeek
                        },
                        new AppSeller()
                        {
                            Name = "Пользователь 4",
                            Description = "Описание продавца 4",
                            RentTime = RentTime.FourWeek
                        }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}

