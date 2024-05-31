using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YachtMarinaAPI.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Journeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Distance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarinaMarkers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarinaMarkers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ShippingAddress_FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_Address1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress_State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddress_Zip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Subtotal = table.Column<long>(type: "bigint", nullable: false),
                    OrderStatus = table.Column<int>(type: "int", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    LoanPricePerDay = table.Column<long>(type: "bigint", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YearOfProduction = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rolename = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendJourney",
                columns: table => new
                {
                    JourneyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    friendId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendJourney", x => new { x.JourneyId, x.Id });
                    table.ForeignKey(
                        name: "FK_FriendJourney_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineCoordinate",
                columns: table => new
                {
                    JourneyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineCoordinate", x => new { x.JourneyId, x.Id });
                    table.ForeignKey(
                        name: "FK_LineCoordinate_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Marker",
                columns: table => new
                {
                    JourneyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lng = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Marker", x => new { x.JourneyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Marker_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    JourneyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => new { x.JourneyId, x.Id });
                    table.ForeignKey(
                        name: "FK_Photo_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    isLoan = table.Column<bool>(type: "bit", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    BasketId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfRegister = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    ConfirmationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "ChatUser",
                columns: table => new
                {
                    ChatsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatUser", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ChatUser_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friend",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FriendUserId = table.Column<int>(type: "int", nullable: false),
                    Rolename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friend", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friend_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    FromUsername = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUserAvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToUserId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JourneyUser",
                columns: table => new
                {
                    JourneysId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyUser", x => new { x.JourneysId, x.UserId });
                    table.ForeignKey(
                        name: "FK_JourneyUser_Journeys_JourneysId",
                        column: x => x.JourneysId,
                        principalTable: "Journeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JourneyUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Yachts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isLoan = table.Column<bool>(type: "bit", nullable: false),
                    YearOfProduction = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yachts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Yachts_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "MarinaMarkers",
                columns: new[] { "Id", "Lat", "Lng", "Name" },
                values: new object[,]
                {
                    { 1, 53.656204624404204m, 14.619229583488101m, "Klub Żeglarski i Motorowodny LOK Stepnica" },
                    { 2, 53.66398675567115m, 14.514430078620219m, "Marina Trzebież" },
                    { 3, 53.72901360148618m, 14.280381204084115m, "Marina Nowe Warpno" },
                    { 4, 53.85099465073054m, 14.620618366810927m, "Marina Wolin" },
                    { 5, 54.134785612826235m, 13.743982320596313m, "MARINA KRÖSLIN GmbH im BALTIC SEA RESORT" },
                    { 6, 53.92484250491297m, 14.273954582044846m, "Marina Basen Północny" },
                    { 7, 53.89685373578024m, 14.361488472458847m, "Marina Łunowo" },
                    { 8, 53.903619917590575m, 14.436979667884126m, "Marina Wicko" },
                    { 9, 53.9759309937529m, 14.76896098382106m, "Marina Kamień Pomorski" },
                    { 10, 54.00633117994754m, 14.705488306339065m, "Przystań jachtowa w Międzywodziu" },
                    { 11, 54.039158902778766m, 14.801714273149273m, "Przystań jachtowa Dziwnówek" },
                    { 12, 53.88047470469847m, 14.426514107316752m, "Marina Wapnica Międzyzdroje" },
                    { 13, 53.72725538121933m, 14.282705024591502m, "Port Jachtowy" },
                    { 14, 53.52262711436701m, 14.625836954883425m, "Port Szczecin" },
                    { 15, 54.10418698975753m, 15.084198208319565m, "Port Niechorze" },
                    { 16, 53.73631291899811m, 14.050031728836158m, "Stadtanleger Ueckermünde" },
                    { 17, 54.06364878736257m, 13.78154357301695m, "Segel Club Wolgast e.V." },
                    { 18, 54.10838422590356m, 13.813029197715817m, "Marina and fishing port Karlshagen" },
                    { 19, 54.15049391054093m, 13.757882482802255m, "Baltic Sea Yacht Events, Segelkatamaran" },
                    { 20, 53.844872436662364m, 13.85785336911374m, "Gästehafen Karnin" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Description", "Length", "LoanPricePerDay", "Name", "PictureUrl", "Price", "PublicId", "QuantityInStock", "Type", "YearOfProduction" },
                values: new object[,]
                {
                    { 1, "Antila", "Jacht Antila 30 wyprodukowany w 2021 roku o długości 9.45m.", 9.45m, 1000L, "Antila 30", "/images/products/jacht1.jpg", 400000L, null, 5, "Żaglowo-motorowy", 2021 },
                    { 2, "Antila", "Jacht Antila 33 wyprodukowany w 2017 roku o długości 9.60m.", 9.60m, 900L, "Antila 33", "/images/products/jacht2.jpg", 300000L, null, 4, "Żaglowo-motorowy", 2017 },
                    { 3, "Antila", "Jacht Antila 28.2 wyprodukowany w 2023 roku o długości 8.85m.", 8.85m, 700L, "Antila 28.2", "/images/products/jacht3.jpg", 300000L, null, 3, "Żaglowo-motorowy", 2023 },
                    { 4, "Antila", "Jacht Antila 27 wyprodukowany w 2022 roku o długości 9.45m.", 9.45m, 600L, "Antila 27", "/images/products/jacht4.jpg", 130000L, null, 4, "Żaglowo-motorowy", 2022 },
                    { 5, "Delphia", "Jacht Delphia 33 MC wyprodukowany w 2014 roku o długości 9.95m.", 9.95m, 700L, "Delphia 33 MC", "/images/products/jacht5.jpg", 220000L, null, 2, "Żaglowo-motorowy", 2014 },
                    { 6, "Laguna", "Jacht Laguna 26 wyprodukowany w 2020 roku o długości 7.6m.", 7.6m, 400L, "Laguna 26", "/images/products/jacht6.jpg", 140000L, null, 3, "Żaglowo-motorowy", 2020 },
                    { 7, "Laguna", "Jacht Laguna 30 wyprodukowany w 2011 roku o długości 8.85m.", 8.85m, 500L, "Laguna 30", "/images/products/jacht7.jpg", 150000L, null, 5, "Żaglowo-motorowy", 2011 },
                    { 8, "Mariner", "Jacht Mariner 20 wyprodukowany w 2011 roku o długości 5.98m.", 5.98m, 200L, "Mariner 20", "/images/products/jacht8.jpg", 35000L, null, 6, "Żaglowo-motorowy", 2011 },
                    { 9, "Mariner", "Jacht Mariner 24 wyprodukowany w 2013 roku o długości 7.62m.", 7.62m, 2500L, "Mariner 24", "/images/products/jacht9.jpg", 50000L, null, 3, "Żaglowo-motorowy", 2013 },
                    { 10, "Mariner", "Jacht Mariner 31 wyprodukowany w 2019 roku o długości 9.45m.", 9.45m, 500L, "Mariner 31", "/images/products/jacht10.jpg", 160000L, null, 4, "Żaglowo-motorowy", 2019 },
                    { 11, "Phila", "Jacht Phila 880 wyprodukowany w 2014 roku o długości 8.80m.", 8.80m, 300L, "Phila 880", "/images/products/jacht11.jpg", 130000L, null, 2, "Żaglowo-motorowy", 2014 },
                    { 12, "Phila", "Jacht Phila 900 wyprodukowany w 2016 roky o długości 9.52m.", 9.52m, 400L, "Phila 900", "/images/products/jacht12.jpg", 200000L, null, 2, "Żaglowo-motorowy", 2016 },
                    { 13, "Phobos", "Jacht Phobos 21 wyprodukowany w 2012 roku o długości 6.18m.", 6.18m, 350L, "Phobos 21", "/images/products/jacht13.jpg", 100000L, null, 3, "Żaglowo-motorowy", 2012 },
                    { 14, "Phobos", "Jacht Phobos 25 wyprodukowany w 2010 roku o długości 7.72m", 7.72m, 400L, "Phobos 25", "/images/products/jacht14.jpg", 150000L, null, 4, "Żaglowo-motorowy", 2010 },
                    { 15, "Sasanka", "Jacht Sasanka 660 wyprodukowany w 2006 roku o długości 6.60m.", 6.60m, 300L, "Sasanka 660", "/images/products/jacht15.jpg", 35000L, null, 5, "Żaglowo-motorowy", 2006 },
                    { 16, "Scandinavia", "Jacht Scandinavia 27 wyprodukowany w 2022 roku o długości 7.85m.", 7.85m, 300L, "Scandinavia 27", "/images/products/jacht16.jpg", 95000L, null, 1, "Żaglowo-motorowy", 2011 },
                    { 17, "Sedna", "Jacht Sedna 26 wyprodukowany w 2016 roku o długości 7.90m.", 7.90m, 300L, "Sedna 26", "/images/products/jacht17.jpg", 165000L, null, 2, "Żaglowo-motorowy", 2016 },
                    { 18, "Sedna", "Jacht Sedna 30 wyprodukowany w 2020 roku o długości 9m", 9m, 400L, "Sedna 30", "/images/products/jacht18.jpg", 250000L, null, 3, "Żaglowo-motorowy", 2020 },
                    { 19, "TES", "Jacht TES 246 Versus wyprodukowany w 2018 roku o długości 7.49m", 7.49m, 310L, "TES 246 Versus", "/images/products/jacht19.jpg", 200000L, null, 1, "Żaglowo-motorowy", 2017 },
                    { 20, "TES", "Jacht TES 32 Dreamer wyprodukowany w 2024 roku o długości 9.69m.", 9.69m, 450L, "TES 32 Dreamer", "/images/products/jacht20.jpg", 325000L, null, 1, "Żaglowo-motorowy", 2024 },
                    { 21, "Sportina", "Jacht Sportina 680 wyprodukowany w 2006 roku o długości 6.80m.", 6.80m, 300L, "Sportina 680", "/images/products/jacht21.jpg", 30000L, null, 5, "Żaglowo-motorowy", 2006 },
                    { 22, "Sportina", "Jacht Sportina 760 wyprodukowany w 2003 roku o długości 7.90m.", 7.60m, 400L, "Sportina 760 ", "/images/products/jacht22.jpg", 55000L, null, 6, "Żaglowo-motorowy", 2003 },
                    { 23, "BALT", "Jacht BALT 818 Tytan wyprodukowany w 2020 roku o długości 7.30m.", 7.30m, 700L, "BALT 818 Tytan", "/images/products/jacht23.jpg", 350000L, null, 3, "Motorowy", 2020 },
                    { 24, "Futura", "Jacht Futura 900 wyprodukowany w 2017 roku o długości 9m.", 9.00m, 500L, "Futura 900", "/images/products/jacht24.jpg", 220000L, null, 1, "Motorowy", 2017 },
                    { 25, "Janmor", "Jacht Janmor 700 wyprodukowany w 2017 roku o długości 7.08m.", 7.08m, 450L, "Janmor 700", "/images/products/jacht25.jpg", 185000L, null, 2, "Motorowy", 2017 },
                    { 26, "Stillo", "Jacht Stillo 30 wyprodukowany w 2021 roku o długości 9.1m.", 9.1m, 1000L, "Stillo 30", "/images/products/jacht26.jpg", 450000L, null, 2, "Motorowy", 2021 },
                    { 27, "Sun camper", "Jacht Sun Camper 35 Flybridge wyprodukowany w 2018 roku o długości 9.05m.", 9.05m, 1200L, "Sun Camper 35 Flybridge", "/images/products/jacht27.jpg", 400000L, null, 1, "Motorowy", 2018 },
                    { 28, "Conrad", "Jacht Conrad 45 wyprodukowany w 1976 roku o długości 13.84m.", 13.84m, 500L, "Conrad 45", "/images/products/jacht28.jpg", 120000L, null, 1, "Żaglowo-motorowy", 1976 },
                    { 29, "Island Packet Yachts", "Jacht Island Packet 420 wyprodukowany w 2000 roku o długości 12.80m.", 12.80m, 1000L, "Island Packet 420", "/images/products/jacht29.jpg", 850000L, null, 1, "Żaglowo-motorowy", 2000 },
                    { 30, "Island Packet Yachts", "Jacht Island Packet 40 wyprodukowany w 1996 roky o długości 12.50m.", 12.50m, 900L, "Island Packet 40", "/images/products/jacht30.jpg", 580000L, null, 1, "Żaglowo-motorowy", 1996 },
                    { 31, "Irwin", "Jacht Irwin 52 wyprodukowany w 1986 roky o długości 15.84m.", 15.84m, 1000L, "Irwin 52", "/images/products/jacht31.jpg", 700000L, null, 2, "Żaglowo-motorowy", 1986 },
                    { 32, "Beneteau", "Jacht Beneteau Oceanis Clipper 411 wyprodukowany w 2001 roku o długości 12.77m.", 12.77m, 500L, "Beneteau Oceanis Clipper 411", "/images/products/jacht32.jpg", 320000L, null, 2, "Żaglowo-motorowy", 2001 },
                    { 33, "Farr", "Jacht Farr VOR 60 wyprodukowany w 1997 roku o długości 18.90m.", 18.90m, 1000L, "Farr VOR 60", "/images/products/jacht33.jpg", 700000L, null, 1, "Żaglowo-motorowy", 1997 },
                    { 34, "Atlantic", "Jacht Atlantic 60 wyprodukowany w 1996 roku o długości 18.59m.", 18.59m, 1000L, "Atlantic 60", "/images/products/jacht34.jpg", 820000L, null, 1, "Żaglowo-motorowy", 1996 },
                    { 35, "Formosa", "Jacht Formosa 51 wyprodukowany w 1976 roku o długości 19.50m.", 19.50m, 1000L, "Formosa 51", "/images/products/jacht35.jpg", 600000L, null, 1, "Żaglowo-motorowy", 1976 },
                    { 36, "Tecnomarine", "Jacht Tecnomarine 62 wyprodukowany w 1992 roku o długości 18.95m.", 18.95m, 1000L, "Tecnomarine 62", "/images/products/jacht36.jpg", 520000L, null, 1, "Motorowy", 1992 },
                    { 37, "Princess", "Jacht Princess 60 wyprodukowany w 1996 roku o długości 18.60m", 18.60m, 1500L, "Princess 60", "/images/products/jacht37.jpg", 910000L, null, 1, "Motorowy", 1996 },
                    { 38, "Hershine", "Jacht Hershine MIAMI 62 wyprodukowany w 1991 roku o długości 18.50m.", 18.50m, 2500L, "Hershine MIAMI 62", "/images/products/jacht38.jpg", 600000L, null, 1, "Motorowy", 1991 },
                    { 39, "Saxdor", "Jacht Saxdor 270 GTO wyprodukowany w 2023 roku o długości 8.4m.", 8.45m, 2500L, "Saxdor 270 GTO", "/images/products/jacht39.jpg", 590000L, null, 1, "Motorowy", 2023 },
                    { 40, "Quicksilver", "Jacht Quicksilver Activ 905 Weekend wyprodukowany w 2021 roku o długości 9m.", 9m, 2000L, "Quicksilver Activ 905 Weekend", "/images/products/jacht40.jpg", 560000L, null, 1, "Motorowy", 2021 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Rolename" },
                values: new object[,]
                {
                    { 1, "Turysta" },
                    { 2, "Żeglarz jachtowy" },
                    { 3, "Jachtowy sternik morski" },
                    { 4, "Kapitan jachtowy" },
                    { 5, "Sternik motorowodny" },
                    { 6, "Sternik motorowodny morski" },
                    { 7, "Bosman" },
                    { 8, "Właściciel" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketId",
                table: "BasketItem",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_ProductId",
                table: "BasketItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatUser_UsersId",
                table: "ChatUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_RoleId",
                table: "Documents",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                table: "Documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Friend_UserId",
                table: "Friend",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invites_UserId",
                table: "Invites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyUser_UserId",
                table: "JourneyUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatId",
                table: "Messages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_userId",
                table: "Yachts",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.DropTable(
                name: "ChatUser");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "Friend");

            migrationBuilder.DropTable(
                name: "FriendJourney");

            migrationBuilder.DropTable(
                name: "Invites");

            migrationBuilder.DropTable(
                name: "JourneyUser");

            migrationBuilder.DropTable(
                name: "LineCoordinate");

            migrationBuilder.DropTable(
                name: "MarinaMarkers");

            migrationBuilder.DropTable(
                name: "Marker");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Yachts");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Journeys");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
