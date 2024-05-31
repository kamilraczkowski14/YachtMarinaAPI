using Microsoft.EntityFrameworkCore;
using YachtMarinaAPI.Entities;
using YachtMarinaAPI.Models;
using YachtMarinaAPI.Models.Order;

namespace YachtMarinaAPI.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Yacht> Yachts { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MarinaMarker> MarinaMarkers { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Journey>()
               .OwnsMany(j => j.LineCoordinates);

            modelBuilder.Entity<Journey>()
              .OwnsMany(j => j.Markers);

            modelBuilder.Entity<Journey>()
            .OwnsMany(j => j.PhotosUrls);

            modelBuilder.Entity<Journey>()
              .OwnsMany(j => j.FriendsIds);



            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    RoleId = 1,
                    Rolename = "Turysta"
                },
                new Role()
                {
                    RoleId = 2,
                    Rolename = "Żeglarz jachtowy"
                },
                new Role()
                {
                    RoleId = 3,
                    Rolename = "Jachtowy sternik morski"
                },
                new Role()
                {
                    RoleId = 4,
                    Rolename = "Kapitan jachtowy"
                },
                new Role()
                {
                    RoleId = 5,
                    Rolename = "Sternik motorowodny"
                },
                new Role()
                {
                    RoleId = 6,
                    Rolename = "Sternik motorowodny morski",
                },
                new Role()
                {
                    RoleId = 7,
                    Rolename = "Bosman"
                },
                new Role()
                {
                    RoleId = 8,
                    Rolename = "Właściciel"
                }
            );


            modelBuilder.Entity<Product>().HasData(

                new Product
                {
                    Id = 1,
                    Name = "Antila 30",
                    Description = "Jacht Antila 30 wyprodukowany w 2021 roku o długości 9.45m.",
                    Price = 400000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht1.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.45m,
                    YearOfProduction = 2021,
                    Brand = "Antila",
                    QuantityInStock = 5
                },
                new Product
                {
                    Id = 2,
                    Name = "Antila 33",
                    Description = "Jacht Antila 33 wyprodukowany w 2017 roku o długości 9.60m.",
                    Price = 300000,
                    LoanPricePerDay = 900,
                    PictureUrl = "/images/products/jacht2.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.60m,
                    YearOfProduction = 2017,
                    Brand = "Antila",
                    QuantityInStock = 4
                },
                new Product
                {
                    Id = 3,
                    Name = "Antila 28.2",
                    Description = "Jacht Antila 28.2 wyprodukowany w 2023 roku o długości 8.85m.",
                    Price = 300000,
                    LoanPricePerDay = 700,
                    PictureUrl = "/images/products/jacht3.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 8.85m,
                    YearOfProduction = 2023,
                    Brand = "Antila",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 4,
                    Name = "Antila 27",
                    Description = "Jacht Antila 27 wyprodukowany w 2022 roku o długości 9.45m.",
                    Price = 130000,
                    LoanPricePerDay = 600,
                    PictureUrl = "/images/products/jacht4.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.45m,
                    YearOfProduction = 2022,
                    Brand = "Antila",
                    QuantityInStock = 4
                },
                new Product
                {
                    Id = 5,
                    Name = "Delphia 33 MC",
                    Description = "Jacht Delphia 33 MC wyprodukowany w 2014 roku o długości 9.95m.",
                    Price = 220000,
                    LoanPricePerDay = 700,
                    PictureUrl = "/images/products/jacht5.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.95m,
                    YearOfProduction = 2014,
                    Brand = "Delphia",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 6,
                    Name = "Laguna 26",
                    Description = "Jacht Laguna 26 wyprodukowany w 2020 roku o długości 7.6m.",
                    Price = 140000,
                    LoanPricePerDay = 400,
                    PictureUrl = "/images/products/jacht6.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.6m,
                    YearOfProduction = 2020,
                    Brand = "Laguna",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 7,
                    Name = "Laguna 30",
                    Description = "Jacht Laguna 30 wyprodukowany w 2011 roku o długości 8.85m.",
                    Price = 150000,
                    LoanPricePerDay = 500,
                    PictureUrl = "/images/products/jacht7.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 8.85m,
                    YearOfProduction = 2011,
                    Brand = "Laguna",
                    QuantityInStock = 5
                },
                new Product
                {
                    Id = 8,
                    Name = "Mariner 20",
                    Description = "Jacht Mariner 20 wyprodukowany w 2011 roku o długości 5.98m.",
                    Price = 35000,
                    LoanPricePerDay = 200,
                    PictureUrl = "/images/products/jacht8.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 5.98m,
                    YearOfProduction = 2011,
                    Brand = "Mariner",
                    QuantityInStock = 6
                },
                new Product
                {
                    Id = 9,
                    Name = "Mariner 24",
                    Description = "Jacht Mariner 24 wyprodukowany w 2013 roku o długości 7.62m.",
                    Price = 50000,
                    LoanPricePerDay = 2500,
                    PictureUrl = "/images/products/jacht9.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.62m,
                    YearOfProduction = 2013,
                    Brand = "Mariner",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 10,
                    Name = "Mariner 31",
                    Description = "Jacht Mariner 31 wyprodukowany w 2019 roku o długości 9.45m.",
                    Price = 160000,
                    LoanPricePerDay = 500,
                    PictureUrl = "/images/products/jacht10.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.45m,
                    YearOfProduction = 2019,
                    Brand = "Mariner",
                    QuantityInStock = 4
                },
                new Product
                {
                    Id = 11,
                    Name = "Phila 880",
                    Description = "Jacht Phila 880 wyprodukowany w 2014 roku o długości 8.80m.",
                    Price = 130000,
                    LoanPricePerDay = 300,
                    PictureUrl = "/images/products/jacht11.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 8.80m,
                    YearOfProduction = 2014,
                    Brand = "Phila",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 12,
                    Name = "Phila 900",
                    Description = "Jacht Phila 900 wyprodukowany w 2016 roky o długości 9.52m.",
                    Price = 200000,
                    LoanPricePerDay = 400,
                    PictureUrl = "/images/products/jacht12.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.52m,
                    YearOfProduction = 2016,
                    Brand = "Phila",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 13,
                    Name = "Phobos 21",
                    Description = "Jacht Phobos 21 wyprodukowany w 2012 roku o długości 6.18m.",
                    Price = 100000,
                    LoanPricePerDay = 350,
                    PictureUrl = "/images/products/jacht13.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 6.18m,
                    YearOfProduction = 2012,
                    Brand = "Phobos",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 14,
                    Name = "Phobos 25",
                    Description = "Jacht Phobos 25 wyprodukowany w 2010 roku o długości 7.72m",
                    Price = 150000,
                    LoanPricePerDay = 400,
                    PictureUrl = "/images/products/jacht14.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.72m,
                    YearOfProduction = 2010,
                    Brand = "Phobos",
                    QuantityInStock = 4
                },
                new Product
                {
                    Id = 15,
                    Name = "Sasanka 660",
                    Description = "Jacht Sasanka 660 wyprodukowany w 2006 roku o długości 6.60m.",
                    Price = 35000,
                    LoanPricePerDay = 300,
                    PictureUrl = "/images/products/jacht15.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 6.60m,
                    YearOfProduction = 2006,
                    Brand = "Sasanka",
                    QuantityInStock = 5
                },
                new Product
                {
                    Id = 16,
                    Name = "Scandinavia 27",
                    Description = "Jacht Scandinavia 27 wyprodukowany w 2022 roku o długości 7.85m.",
                    Price = 95000,
                    LoanPricePerDay = 300,
                    PictureUrl = "/images/products/jacht16.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.85m,
                    YearOfProduction = 2011,
                    Brand = "Scandinavia",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 17,
                    Name = "Sedna 26",
                    Description = "Jacht Sedna 26 wyprodukowany w 2016 roku o długości 7.90m.",
                    Price = 165000,
                    LoanPricePerDay = 300,
                    PictureUrl = "/images/products/jacht17.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.90m,
                    YearOfProduction = 2016,
                    Brand = "Sedna",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 18,
                    Name = "Sedna 30",
                    Description = "Jacht Sedna 30 wyprodukowany w 2020 roku o długości 9m",
                    Price = 250000,
                    LoanPricePerDay = 400,
                    PictureUrl = "/images/products/jacht18.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9m,
                    YearOfProduction = 2020,
                    Brand = "Sedna",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 19,
                    Name = "TES 246 Versus",
                    Description = "Jacht TES 246 Versus wyprodukowany w 2018 roku o długości 7.49m",
                    Price = 200000,
                    LoanPricePerDay = 310,
                    PictureUrl = "/images/products/jacht19.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.49m,
                    YearOfProduction = 2017,
                    Brand = "TES",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 20,
                    Name = "TES 32 Dreamer",
                    Description = "Jacht TES 32 Dreamer wyprodukowany w 2024 roku o długości 9.69m.",
                    Price = 325000,
                    LoanPricePerDay = 450,
                    PictureUrl = "/images/products/jacht20.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 9.69m,
                    YearOfProduction = 2024,
                    Brand = "TES",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 21,
                    Name = "Sportina 680",
                    Description = "Jacht Sportina 680 wyprodukowany w 2006 roku o długości 6.80m.",
                    Price = 30000,
                    LoanPricePerDay = 300,
                    PictureUrl = "/images/products/jacht21.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 6.80m,
                    YearOfProduction = 2006,
                    Brand = "Sportina",
                    QuantityInStock = 5
                },
                new Product
                {
                    Id = 22,
                    Name = "Sportina 760 ",
                    Description = "Jacht Sportina 760 wyprodukowany w 2003 roku o długości 7.90m.",
                    Price = 55000,
                    LoanPricePerDay = 400,
                    PictureUrl = "/images/products/jacht22.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 7.60m,
                    YearOfProduction = 2003,
                    Brand = "Sportina",
                    QuantityInStock = 6
                },
                new Product
                {
                    Id = 23,
                    Name = "BALT 818 Tytan",
                    Description = "Jacht BALT 818 Tytan wyprodukowany w 2020 roku o długości 7.30m.",
                    Price = 350000,
                    LoanPricePerDay = 700,
                    PictureUrl = "/images/products/jacht23.jpg",
                    Type = "Motorowy",
                    Length = 7.30m,
                    YearOfProduction = 2020,
                    Brand = "BALT",
                    QuantityInStock = 3
                },
                new Product
                {
                    Id = 24,
                    Name = "Futura 900",
                    Description = "Jacht Futura 900 wyprodukowany w 2017 roku o długości 9m.",
                    Price = 220000,
                    LoanPricePerDay = 500,
                    PictureUrl = "/images/products/jacht24.jpg",
                    Type = "Motorowy",
                    Length = 9.00m,
                    YearOfProduction = 2017,
                    Brand = "Futura",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 25,
                    Name = "Janmor 700",
                    Description = "Jacht Janmor 700 wyprodukowany w 2017 roku o długości 7.08m.",
                    Price = 185000,
                    LoanPricePerDay = 450,
                    PictureUrl = "/images/products/jacht25.jpg",
                    Type = "Motorowy",
                    Length = 7.08m,
                    YearOfProduction = 2017,
                    Brand = "Janmor",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 26,
                    Name = "Stillo 30",
                    Description = "Jacht Stillo 30 wyprodukowany w 2021 roku o długości 9.1m.",
                    Price = 450000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht26.jpg",
                    Type = "Motorowy",
                    Length = 9.1m,
                    YearOfProduction = 2021,
                    Brand = "Stillo",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 27,
                    Name = "Sun Camper 35 Flybridge",
                    Description = "Jacht Sun Camper 35 Flybridge wyprodukowany w 2018 roku o długości 9.05m.",
                    Price = 400000,
                    LoanPricePerDay = 1200,
                    PictureUrl = "/images/products/jacht27.jpg",
                    Type = "Motorowy",
                    Length = 9.05m,
                    YearOfProduction = 2018,
                    Brand = "Sun camper",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 28,
                    Name = "Conrad 45",
                    Description = "Jacht Conrad 45 wyprodukowany w 1976 roku o długości 13.84m.",
                    Price = 120000,
                    LoanPricePerDay = 500,
                    PictureUrl = "/images/products/jacht28.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 13.84m,
                    YearOfProduction = 1976,
                    Brand = "Conrad",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 29,
                    Name = "Island Packet 420",
                    Description = "Jacht Island Packet 420 wyprodukowany w 2000 roku o długości 12.80m.",
                    Price = 850000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht29.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 12.80m,
                    YearOfProduction = 2000,
                    Brand = "Island Packet Yachts",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 30,
                    Name = "Island Packet 40",
                    Description = "Jacht Island Packet 40 wyprodukowany w 1996 roky o długości 12.50m.",
                    Price = 580000,
                    LoanPricePerDay = 900,
                    PictureUrl = "/images/products/jacht30.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 12.50m,
                    YearOfProduction = 1996,
                    Brand = "Island Packet Yachts",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 31,
                    Name = "Irwin 52",
                    Description = "Jacht Irwin 52 wyprodukowany w 1986 roky o długości 15.84m.",
                    Price = 700000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht31.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 15.84m,
                    YearOfProduction = 1986,
                    Brand = "Irwin",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 32,
                    Name = "Beneteau Oceanis Clipper 411",
                    Description = "Jacht Beneteau Oceanis Clipper 411 wyprodukowany w 2001 roku o długości 12.77m.",
                    Price = 320000,
                    LoanPricePerDay = 500,
                    PictureUrl = "/images/products/jacht32.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 12.77m,
                    YearOfProduction = 2001,
                    Brand = "Beneteau",
                    QuantityInStock = 2
                },
                new Product
                {
                    Id = 33,
                    Name = "Farr VOR 60",
                    Description = "Jacht Farr VOR 60 wyprodukowany w 1997 roku o długości 18.90m.",
                    Price = 700000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht33.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 18.90m,
                    YearOfProduction = 1997,
                    Brand = "Farr",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 34,
                    Name = "Atlantic 60",
                    Description = "Jacht Atlantic 60 wyprodukowany w 1996 roku o długości 18.59m.",
                    Price = 820000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht34.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 18.59m,
                    YearOfProduction = 1996,
                    Brand = "Atlantic",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 35,
                    Name = "Formosa 51",
                    Description = "Jacht Formosa 51 wyprodukowany w 1976 roku o długości 19.50m.",
                    Price = 600000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht35.jpg",
                    Type = "Żaglowo-motorowy",
                    Length = 19.50m,
                    YearOfProduction = 1976,
                    Brand = "Formosa",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 36,
                    Name = "Tecnomarine 62",
                    Description = "Jacht Tecnomarine 62 wyprodukowany w 1992 roku o długości 18.95m.",
                    Price = 520000,
                    LoanPricePerDay = 1000,
                    PictureUrl = "/images/products/jacht36.jpg",
                    Type = "Motorowy",
                    Length = 18.95m,
                    YearOfProduction = 1992,
                    Brand = "Tecnomarine",
                    QuantityInStock = 1
                },
                new Product
                {
                    Id = 37,
                    Name = "Princess 60",
                    Description = "Jacht Princess 60 wyprodukowany w 1996 roku o długości 18.60m",
                    Price = 910000,
                    LoanPricePerDay = 1500,
                    PictureUrl = "/images/products/jacht37.jpg",
                    Type = "Motorowy",
                    Length = 18.60m,
                    YearOfProduction = 1996,
                    Brand = "Princess",
                    QuantityInStock = 1
                },
                 new Product
                 {
                     Id = 38,
                     Name = "Hershine MIAMI 62",
                     Description = "Jacht Hershine MIAMI 62 wyprodukowany w 1991 roku o długości 18.50m.",
                     Price = 600000,
                     LoanPricePerDay = 2500,
                     PictureUrl = "/images/products/jacht38.jpg",
                     Type = "Motorowy",
                     Length = 18.50m,
                     YearOfProduction = 1991,
                     Brand = "Hershine",
                     QuantityInStock = 1
                 },
                 new Product
                 {
                     Id = 39,
                     Name = "Saxdor 270 GTO",
                     Description = "Jacht Saxdor 270 GTO wyprodukowany w 2023 roku o długości 8.4m.",
                     Price = 590000,
                     LoanPricePerDay = 2500,
                     PictureUrl = "/images/products/jacht39.jpg",
                     Type = "Motorowy",
                     Length = 8.45m,
                     YearOfProduction = 2023,
                     Brand = "Saxdor",
                     QuantityInStock = 1
                 },
                 new Product
                 {
                     Id = 40,
                     Name = "Quicksilver Activ 905 Weekend",
                     Description = "Jacht Quicksilver Activ 905 Weekend wyprodukowany w 2021 roku o długości 9m.",
                     Price = 560000,
                     LoanPricePerDay = 2000,
                     PictureUrl = "/images/products/jacht40.jpg",
                     Type = "Motorowy",
                     Length = 9m,
                     YearOfProduction = 2021,
                     Brand = "Quicksilver",
                     QuantityInStock = 1
                 }
           );

            modelBuilder.Entity<MarinaMarker>().HasData(
                new MarinaMarker()
                {
                    Id = 1,
                    Name = "Klub Żeglarski i Motorowodny LOK Stepnica",
                    Lat = 53.656204624404204m,
                    Lng = 14.619229583488101m
                },
                 new MarinaMarker()
                 {
                     Id = 2,
                     Name = "Marina Trzebież",
                     Lat = 53.66398675567115m,
                     Lng = 14.514430078620219m
                 },
                  new MarinaMarker()
                  {
                      Id = 3,
                      Name = "Marina Nowe Warpno",
                      Lat = 53.72901360148618m,
                      Lng = 14.280381204084115m
                  },
                   new MarinaMarker()
                   {
                       Id = 4,
                       Name = "Marina Wolin",
                       Lat = 53.85099465073054m,
                       Lng = 14.620618366810927m
                   },
                    new MarinaMarker()
                    {
                        Id = 5,
                        Name = "MARINA KRÖSLIN GmbH im BALTIC SEA RESORT",
                        Lat = 54.134785612826235m,
                        Lng = 13.743982320596313m
                    },
                     new MarinaMarker()
                     {
                         Id = 6,
                         Name = "Marina Basen Północny",
                         Lat = 53.92484250491297m,
                         Lng = 14.273954582044846m
                     },
                      new MarinaMarker()
                      {
                          Id = 7,
                          Name = "Marina Łunowo",
                          Lat = 53.89685373578024m,
                          Lng = 14.361488472458847m
                      },
                       new MarinaMarker()
                       {
                           Id = 8,
                           Name = "Marina Wicko",
                           Lat = 53.903619917590575m,
                           Lng = 14.436979667884126m
                       },
                        new MarinaMarker()
                        {
                            Id = 9,
                            Name = "Marina Kamień Pomorski",
                            Lat = 53.9759309937529m,
                            Lng = 14.76896098382106m,
                        },
                         new MarinaMarker()
                         {
                             Id = 10,
                             Name = "Przystań jachtowa w Międzywodziu",
                             Lat = 54.00633117994754m,
                             Lng = 14.705488306339065m
                         },
                          new MarinaMarker()
                          {
                              Id = 11,
                              Name = "Przystań jachtowa Dziwnówek",
                              Lat = 54.039158902778766m,
                              Lng = 14.801714273149273m
                          },
                           new MarinaMarker()
                           {
                               Id = 12,
                               Name = "Marina Wapnica Międzyzdroje",
                               Lat = 53.88047470469847m,
                               Lng = 14.426514107316752m
                           },
                            new MarinaMarker()
                            {
                                Id = 13,
                                Name = "Port Jachtowy",
                                Lat = 53.72725538121933m,
                                Lng = 14.282705024591502m,
                            },
                             new MarinaMarker()
                             {
                                 Id = 14,
                                 Name = "Port Szczecin",
                                 Lat = 53.52262711436701m,
                                 Lng = 14.625836954883425m,
                             },
                              new MarinaMarker()
                              {
                                  Id = 15,
                                  Name = "Port Niechorze",
                                  Lat = 54.10418698975753m,
                                  Lng = 15.084198208319565m
                              },
                               new MarinaMarker()
                               {
                                   Id = 16,
                                   Name = "Stadtanleger Ueckermünde",
                                   Lat = 53.73631291899811m,
                                   Lng = 14.050031728836158m
                               },
                                new MarinaMarker()
                                {
                                    Id = 17,
                                    Name = "Segel Club Wolgast e.V.",
                                    Lat = 54.06364878736257m,
                                    Lng = 13.78154357301695m
                                },
                                 new MarinaMarker()
                                 {
                                     Id = 18,
                                     Name = "Marina and fishing port Karlshagen",
                                     Lat = 54.10838422590356m,
                                     Lng = 13.813029197715817m
                                 },
                                  new MarinaMarker()
                                  {
                                      Id = 19,
                                      Name = "Baltic Sea Yacht Events, Segelkatamaran",
                                      Lat = 54.15049391054093m,
                                      Lng = 13.757882482802255m
                                  },
                                   new MarinaMarker()
                                   {
                                       Id = 20,
                                       Name = "Gästehafen Karnin",
                                       Lat = 53.844872436662364m,
                                       Lng = 13.85785336911374m
                                   }
            );
        }
    }
}
