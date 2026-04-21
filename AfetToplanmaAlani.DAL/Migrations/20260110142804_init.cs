using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AfetToplanmaAlani.DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaceCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Brand = table.Column<string>(type: "TEXT", nullable: true),
                    Model = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    Plate = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkGroupStaff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    WorkGroup = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Crew = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Duty = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkGroupStaff", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Places",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    District = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Neighborhood = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Latitude = table.Column<double>(type: "REAL", nullable: false),
                    Longitude = table.Column<double>(type: "REAL", nullable: false),
                    ContactNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    PlaceCategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Places", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Places_PlaceCategories_PlaceCategoryId",
                        column: x => x.PlaceCategoryId,
                        principalTable: "PlaceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "VehicleStaff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleStaff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleStaff_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Surname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    PlaceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Staff_Places_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "Places",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlaceCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Toplanma Noktaları" },
                    { 2, "Çadır Alanları" },
                    { 3, "Su Tesisleri" },
                    { 4, "Fırın" },
                    { 5, "Eczane" }
                });

            migrationBuilder.InsertData(
                table: "WorkGroupStaff",
                columns: new[] { "Id", "Crew", "Duty", "Name", "PhoneNumber", "Surname", "WorkGroup" },
                values: new object[,]
                {
                    { 1, "AFAD", "Lider", "Ali", "5551110001", "Yılmaz", "Bilgi Yönetimi Değerlendirme Ve Haberleşme Çalışma Grubu" },
                    { 2, "Kızılay", "Üye", "Ayşe", "5551110002", "Demir", "Arama Kurtarma Çalışma Grubu" },
                    { 3, "AFAD", "Üye", "Mehmet", "5551110003", "Kaya", "Barınma Çalışma Grubu" },
                    { 4, "Kızılay", "Üye", "Fatma", "5551110004", "Öztürk", "Hasar Tespit Çalışma Grubu" },
                    { 5, "AFAD", "Üye", "Ahmet", "5551110005", "Çelik", "Enkaz Kaldırma Çalışma Grubu" },
                    { 6, "Kızılay", "Üye", "Elif", "5551110006", "Ak", "Yangın Çalışma Grubu" },
                    { 7, "AFAD", "Üye", "Can", "5551110007", "Yıldız", "Beslenme Çalışma Grubu" },
                    { 8, "Kızılay", "Üye", "Selin", "5551110008", "Arslan", "Sağlık Çalışma Grubu" },
                    { 9, "AFAD", "Üye", "Deniz", "5551110009", "Kurt", "Ayni Bağış Ve Depo Yönetimi Çalışma Grubu" },
                    { 10, "Kızılay", "Üye", "Burak", "5551110010", "Demirtaş", "Psikososyal Çalışma Grubu" },
                    { 11, "AFAD", "Üye", "Derya", "5551110011", "Polat", "Güvenlik Ve Trafik Çalışma Grubu" },
                    { 12, "Kızılay", "Üye", "Ege", "5551110012", "Kara", "Tahliye Yerleştirme Çalışma Grubu" },
                    { 13, "AFAD", "Üye", "Merve", "5551110013", "Yalçın", "Afet Zarar Tespit Çalışma Grubu" },
                    { 14, "Kızılay", "Üye", "Ozan", "5551110014", "Güneş", "Afet Kimliklendirme Ve Defin Çalışma Grubu" },
                    { 15, "AFAD", "Üye", "Seda", "5551110015", "Erdoğan", "Afet Finans Ve Kaynak Yönetimi Çalışma Grubu" },
                    { 16, "Kızılay", "Üye", "Kaan", "5551110016", "Öz", "Afet Tarım Orman Gıda Su Ve Hayvancılık Çalışma Grubu" },
                    { 17, "AFAD", "Üye", "Nazlı", "5551110017", "Uçar", "Afet Enerji Çalışma Grubu" },
                    { 18, "Kızılay", "Üye", "Emre", "5551110018", "Şahin", "Afet Haberleşme Çalışma Grubu" },
                    { 19, "AFAD", "Üye", "Büşra", "5551110019", "Aydın", "Afet Ulaşım Alt Yapı Çalışma Grubu" },
                    { 20, "Kızılay", "Üye", "Serkan", "5551110020", "Topal", "Afet Nakliye Çalışma Grubu" }
                });

            migrationBuilder.InsertData(
                table: "Places",
                columns: new[] { "Id", "City", "ContactNumber", "District", "Latitude", "Longitude", "Name", "Neighborhood", "PlaceCategoryId" },
                values: new object[,]
                {
                    { 1, "İstanbul", "02120000000", "Kadıköy", 40.988999999999997, 29.027999999999999, "Merkez Park Toplanma Alanı", "Fenerbahçe", 1 },
                    { 2, "İstanbul", "02120000001", "Kadıköy", 40.991199999999999, 29.0321, "Moda Çadır Alanı", "Moda", 2 },
                    { 3, "İstanbul", "02120000002", "Avcılar", 40.979100000000003, 28.722100000000001, "Avcılar Su Tesisi", "Cihangir", 3 },
                    { 4, "İstanbul", "02120000003", "Bakırköy", 40.980499999999999, 28.874199999999998, "Bakırköy Fırını", "Kartaltepe", 4 },
                    { 5, "İstanbul", "02120000004", "Beşiktaş", 41.043799999999997, 29.009399999999999, "Beşiktaş Eczanesi", "Levent", 5 },
                    { 6, "Ankara", "03120000000", "Çankaya", 39.9208, 32.854100000000003, "Kızılay Toplanma Alanı", "Kızılay", 1 },
                    { 7, "Ankara", "03120000001", "Çankaya", 39.933399999999999, 32.859699999999997, "Bahçelievler Çadır Alanı", "Bahçelievler", 2 },
                    { 8, "Ankara", "03120000002", "Altındağ", 39.927199999999999, 32.863999999999997, "Ulus Su Tesisi", "Ulus", 3 },
                    { 9, "Ankara", "03120000003", "Mamak", 39.945999999999998, 32.857999999999997, "Mamak Fırını", "OSB", 4 },
                    { 10, "Ankara", "03120000004", "Yenimahalle", 39.9512, 32.753399999999999, "Yenimahalle Eczanesi", "Batıkent", 5 },
                    { 11, "İzmir", "02320000000", "Konak", 38.419199999999996, 27.128699999999998, "Alsancak Toplanma Alanı", "Alsancak", 1 },
                    { 12, "İzmir", "02320000001", "Konak", 38.414900000000003, 27.1282, "Göztepe Çadır Alanı", "Göztepe", 2 },
                    { 13, "İzmir", "02320000002", "Bornova", 38.466000000000001, 27.221299999999999, "Bornova Su Tesisi", "Kazım Dirik", 3 },
                    { 14, "İzmir", "02320000003", "Buca", 38.408499999999997, 27.111499999999999, "Buca Fırını", "Evka 3", 4 },
                    { 15, "İzmir", "02320000004", "Karşıyaka", 38.451999999999998, 27.051100000000002, "Karşıyaka Eczanesi", "Alaybey", 5 },
                    { 16, "Bursa", "02240000000", "Osmangazi", 40.189700000000002, 29.061699999999998, "Nilüfer Toplanma Alanı", "Nilüfer", 1 },
                    { 17, "Bursa", "02240000001", "Osmangazi", 40.200200000000002, 29.072299999999998, "Beşevler Çadır Alanı", "Beşevler", 2 },
                    { 18, "Bursa", "02240000002", "Yıldırım", 40.197000000000003, 29.074999999999999, "Hüdavendigar Su Tesisi", "Hüdavendigar", 3 },
                    { 19, "Bursa", "02240000003", "Nilüfer", 40.185000000000002, 28.989999999999998, "Görükle Fırını", "Görükle", 4 },
                    { 20, "Bursa", "02240000004", "Osmangazi", 40.203000000000003, 29.055, "Altınova Eczanesi", "Altınova", 5 },
                    { 21, "Ankara", "03120000005", "Çankaya", 39.933100000000003, 32.859000000000002, "Sıhhiye Toplanma Alanı", "Sıhhiye", 1 },
                    { 22, "Ankara", "03120000006", "Çankaya", 39.933999999999997, 32.859999999999999, "Bahçelievler Çadır Alanı", "Bahçelievler", 2 },
                    { 23, "Ankara", "03120000007", "Çankaya", 39.932000000000002, 32.854999999999997, "Kızılay Su Tesisi", "Kızılay", 3 },
                    { 24, "Ankara", "03120000008", "Çankaya", 39.935000000000002, 32.857999999999997, "Çankaya Fırını", "Çankaya", 4 },
                    { 25, "Ankara", "03120000009", "Yenimahalle", 39.951000000000001, 32.753, "Batıkent Eczanesi", "Batıkent", 5 },
                    { 26, "İzmir", "02320000005", "Karşıyaka", 38.451000000000001, 27.052, "Karşıyaka Toplanma Alanı", "Karşıyaka", 1 },
                    { 27, "İzmir", "02320000006", "Karşıyaka", 38.454999999999998, 27.050000000000001, "Bostanlı Çadır Alanı", "Bostanlı", 2 },
                    { 28, "İzmir", "02320000007", "Bornova", 33.466000000000001, 27.221, "Bornova Su Tesisi2", "Kazım Dirik", 3 },
                    { 29, "İzmir", "02320000008", "Konak", 38.417000000000002, 27.128, "Konak Fırını", "Konak", 4 },
                    { 30, "İzmir", "02320000009", "Konak", 38.414999999999999, 27.129000000000001, "Göztepe Eczanesi", "Göztepe", 5 }
                });

            migrationBuilder.InsertData(
                table: "Staff",
                columns: new[] { "Id", "Name", "PhoneNumber", "PlaceId", "Surname" },
                values: new object[,]
                {
                    { 1, "Ali", "5551112233", 1, "Yılmaz" },
                    { 2, "Ayşe", "5552223344", 1, "Demir" },
                    { 3, "Mehmet", "5553334455", 5, "Kaya" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_PlaceCategoryId",
                table: "Places",
                column: "PlaceCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Staff_PlaceId",
                table: "Staff",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleStaff_VehicleId",
                table: "VehicleStaff",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "VehicleStaff");

            migrationBuilder.DropTable(
                name: "WorkGroupStaff");

            migrationBuilder.DropTable(
                name: "Places");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "PlaceCategories");
        }
    }
}
