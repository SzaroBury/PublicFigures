using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RespectCounter.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DomainUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DomainUsers_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DomainUsers_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DomainUsers_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Profession = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    DeathDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tags_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Happend = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PersonReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonReactions_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonReactions_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonReactions_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonTag",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTag", x => new { x.PersonId, x.TagId });
                    table.ForeignKey(
                        name: "FK_PersonTag_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonTag_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PersonTag_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonTag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFavoriteTags",
                columns: table => new
                {
                    FavoriteTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavoriteTags", x => new { x.FavoriteTagsId, x.User1Id });
                    table.ForeignKey(
                        name: "FK_UserFavoriteTags_DomainUsers_User1Id",
                        column: x => x.User1Id,
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavoriteTags_Tags_FavoriteTagsId",
                        column: x => x.FavoriteTagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRecentlyBrowsedTags",
                columns: table => new
                {
                    RecentlyBrowsedTagsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRecentlyBrowsedTags", x => new { x.RecentlyBrowsedTagsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRecentlyBrowsedTags_DomainUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "DomainUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRecentlyBrowsedTags_Tags_RecentlyBrowsedTagsId",
                        column: x => x.RecentlyBrowsedTagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ActivityReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActivityReactions_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityReactions_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityReactions_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ActivityTags",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityTags", x => new { x.ActivityId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ActivityTags_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityTags_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityTags_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ActivityTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DirectChildrenCount = table.Column<int>(type: "int", nullable: false),
                    AllChildrenCount = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CommentReactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReactions_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentReactions_DomainUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CommentReactions_DomainUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "DomainUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("b2c5e637-9d76-447f-a45e-9330d048b861"), null, "Admin", null },
                    { new Guid("c60d7ecd-2ead-4094-a536-83567e36052f"), null, "User", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 0, "5ed98c9c-fe85-4e45-8299-84d80f43eeea", null, false, false, null, null, "SYSTEM_USER", null, null, false, null, null, null, false, "system_user" },
                    { new Guid("324f50c4-b332-4199-9f10-5406a11119da"), 0, "33f1208a-ccf4-4f4a-aad9-36eedeaef95f", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAELqnVWjFf2QyDXfRahRgzd2m1r2ykHNEItaDou+YhAtEIhnJpG94snQJX9ld4jSykQ==", null, false, null, null, "4332a146-3e43-4c05-aafa-4a5f8b106ddc", false, "admin" },
                    { new Guid("59c900f0-d4e2-4e41-9811-5e929e476192"), 0, "37c5ae4b-b8a2-41be-bb38-a1014387a838", "user@example.com", true, false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAELZZbNb/nsTMZZDru/rRltH2ZUAw6ykmzy2ohOamRr8//+8tcWezGhHc4rkOhUakXQ==", null, false, null, null, "99b0ce45-b1ff-4df6-8b79-823547c5c00f", false, "user" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("b2c5e637-9d76-447f-a45e-9330d048b861"), new Guid("324f50c4-b332-4199-9f10-5406a11119da") },
                    { new Guid("c60d7ecd-2ead-4094-a536-83567e36052f"), new Guid("59c900f0-d4e2-4e41-9811-5e929e476192") }
                });

            migrationBuilder.InsertData(
                table: "DomainUsers",
                columns: new[] { "Id", "AvatarUrl", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "Username" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), "System" },
                    { new Guid("324f50c4-b332-4199-9f10-5406a11119da"), null, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), "admin" },
                    { new Guid("59c900f0-d4e2-4e41-9811-5e929e476192"), null, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7919), new Guid("00000000-0000-0000-0000-000000000001"), "user" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "AvatarUrl", "Birthday", "Created", "CreatedById", "DeathDate", "Deleted", "Description", "FirstName", "LastName", "LastUpdated", "LastUpdatedById", "Nationality", "NickName", "Profession", "Status" },
                values: new object[,]
                {
                    { new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), null, new DateOnly(1988, 8, 21), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4204), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Robert", "Lewandowski", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4204), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "Lewy", "Footballer", 1 },
                    { new Guid("85323f6e-bcd2-41e2-9e10-4a352c2f141f"), null, new DateOnly(1972, 5, 16), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4317), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Andrzej", "Duda", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4317), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Politician", 1 },
                    { new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), null, new DateOnly(1984, 12, 7), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4296), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Robert", "Kubica", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4296), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Racer", 0 },
                    { new Guid("dd06393a-7555-44e7-b9f2-bb3089f36eb0"), null, new DateOnly(1957, 4, 22), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4327), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Donald", "Tusk", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4327), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Politician", 1 }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "Description", "LastUpdated", "LastUpdatedById", "Name" },
                values: new object[,]
                {
                    { new Guid("01cf2487-4d3f-453c-bc71-29d8d645771e"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5262), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5262), new Guid("00000000-0000-0000-0000-000000000001"), "Football" },
                    { new Guid("16821f42-1144-4cf6-9f0e-77856aafad6c"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5241), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5241), new Guid("00000000-0000-0000-0000-000000000001"), "Sport" },
                    { new Guid("321c6ccd-a45f-4472-adee-59917aa7c727"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5272), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5272), new Guid("00000000-0000-0000-0000-000000000001"), "F1" },
                    { new Guid("469acbde-8d55-4f6b-a278-001ca4e4acce"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5298), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5298), new Guid("00000000-0000-0000-0000-000000000001"), "PO" },
                    { new Guid("5d61e0a7-6eba-4163-91ce-31748678493c"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5281), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5281), new Guid("00000000-0000-0000-0000-000000000001"), "WEC" },
                    { new Guid("7361172f-79d0-449f-891a-3d378ea43e5a"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5293), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5293), new Guid("00000000-0000-0000-0000-000000000001"), "PiS" },
                    { new Guid("7ce7c993-8a63-4484-97bf-98f2aa1ed8f7"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5267), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5267), new Guid("00000000-0000-0000-0000-000000000001"), "FC Barcelona" },
                    { new Guid("8c74fb36-121e-4e1f-b9ab-0cb151110a84"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5289), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5289), new Guid("00000000-0000-0000-0000-000000000001"), "Politics" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "Description", "Happend", "LastUpdated", "LastUpdatedById", "Location", "PersonId", "Source", "Status", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4599), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test description", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4599), new Guid("00000000-0000-0000-0000-000000000001"), "", new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), "Dude, just trust me", "NotVerified", "Quote", "Milik jest słaby" },
                    { new Guid("ae1e8651-e6a5-487b-823f-6f6f60c4cc6e"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4658), new Guid("00000000-0000-0000-0000-000000000001"), false, "Można utknąć w eeeee korku", new DateTime(2010, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4658), new Guid("00000000-0000-0000-0000-000000000001"), "Monaco, MC", new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), "https://www.youtube.com/watch?v=qbYMoKxif6I", "Verified", "Act", "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali" }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("2fc60632-d57c-4380-ad25-184fa4a6c5fc"), null, 0, "Nie lubiem go, bo Andrzej to dziwne imię", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4916), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4916), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("85323f6e-bcd2-41e2-9e10-4a352c2f141f"), 0 },
                    { new Guid("5bb0cb86-8a1d-42a8-8dbe-eb1873fd1cb8"), null, 3, "Bardzo memiczna osoba", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4877), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4877), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("85323f6e-bcd2-41e2-9e10-4a352c2f141f"), 0 },
                    { new Guid("adec7066-9bb6-44c7-9563-bfd50d8d1700"), null, 2, "Najlepszy zawodnik!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4773), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4773), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 0 },
                    { new Guid("eeea95f9-04ab-49ef-9928-ac74035c90c9"), null, 0, "Ja tam mu nei ufam", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4909), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4909), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("85323f6e-bcd2-41e2-9e10-4a352c2f141f"), 0 }
                });

            migrationBuilder.InsertData(
                table: "PersonReactions",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "PersonId", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("009edbd2-d688-4453-8b7c-ebb70fd1a6e7"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5800), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5800), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 2 },
                    { new Guid("0f06fbb3-56de-42ee-8310-3603e96ae887"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5758), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5758), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), -1 },
                    { new Guid("26af26ba-9d0f-4f25-af2d-900bd3bf23cc"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5729), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5729), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), -2 },
                    { new Guid("7c39ff90-c276-4d23-bdf3-233e2a89ab02"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5746), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5746), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 1 },
                    { new Guid("82e641c3-b8f3-4aa5-8407-bc4933739f71"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5763), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5763), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 1 },
                    { new Guid("a82e3f2a-e96a-4388-9ab4-3a09d9a4d076"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5783), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5783), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 2 },
                    { new Guid("a8d47161-be64-4ccb-a105-d84f57b1ba1d"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5769), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5769), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 2 },
                    { new Guid("d2bab206-8167-4bf0-94dd-475c00dcea61"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5795), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5795), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), 2 }
                });

            migrationBuilder.InsertData(
                table: "PersonTag",
                columns: new[] { "PersonId", "TagId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById" },
                values: new object[,]
                {
                    { new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), new Guid("01cf2487-4d3f-453c-bc71-29d8d645771e"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), new Guid("16821f42-1144-4cf6-9f0e-77856aafad6c"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("19888f5c-f999-4673-ab6d-f0a487194096"), new Guid("7ce7c993-8a63-4484-97bf-98f2aa1ed8f7"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("85323f6e-bcd2-41e2-9e10-4a352c2f141f"), new Guid("7361172f-79d0-449f-891a-3d378ea43e5a"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), new Guid("16821f42-1144-4cf6-9f0e-77856aafad6c"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), new Guid("321c6ccd-a45f-4472-adee-59917aa7c727"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), new Guid("5d61e0a7-6eba-4163-91ce-31748678493c"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("93eec7b8-5c84-424d-a455-f66a1ba72cca"), new Guid("8c74fb36-121e-4e1f-b9ab-0cb151110a84"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("dd06393a-7555-44e7-b9f2-bb3089f36eb0"), new Guid("469acbde-8d55-4f6b-a278-001ca4e4acce"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("dd06393a-7555-44e7-b9f2-bb3089f36eb0"), new Guid("8c74fb36-121e-4e1f-b9ab-0cb151110a84"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "ActivityReactions",
                columns: new[] { "Id", "ActivityId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("37f581f7-ce33-41f1-9897-d70ee6d8a718"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5924), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5924), new Guid("00000000-0000-0000-0000-000000000001"), 2 },
                    { new Guid("3cb5be11-8574-453b-bc61-e408bfdea3a7"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5906), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5906), new Guid("00000000-0000-0000-0000-000000000001"), -1 },
                    { new Guid("79055356-99cd-4832-ab36-221548883a9d"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5891), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5891), new Guid("00000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("b6ba349d-d686-45e3-aefa-328a8ea8eddd"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5918), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(5918), new Guid("00000000-0000-0000-0000-000000000001"), 1 }
                });

            migrationBuilder.InsertData(
                table: "ActivityTags",
                columns: new[] { "ActivityId", "TagId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById" },
                values: new object[,]
                {
                    { new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new Guid("01cf2487-4d3f-453c-bc71-29d8d645771e"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), new Guid("16821f42-1144-4cf6-9f0e-77856aafad6c"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("ae1e8651-e6a5-487b-823f-6f6f60c4cc6e"), new Guid("16821f42-1144-4cf6-9f0e-77856aafad6c"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") },
                    { new Guid("ae1e8651-e6a5-487b-823f-6f6f60c4cc6e"), new Guid("321c6ccd-a45f-4472-adee-59917aa7c727"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001"), new DateTime(2025, 11, 3, 16, 5, 53, 16, DateTimeKind.Utc).AddTicks(7315), new Guid("00000000-0000-0000-0000-000000000001") }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("088fb9dc-93d8-4895-9c9d-a86efd404598"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), 1, "Niefajność", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4853), new Guid("00000000-0000-0000-0000-000000000001"), false, 1, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4853), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 },
                    { new Guid("28fdf3e9-c247-4c27-97c8-e0248e78d6f5"), null, 0, "Hańba!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4885), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4885), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("5bb0cb86-8a1d-42a8-8dbe-eb1873fd1cb8"), null, 0 },
                    { new Guid("31bcfe1e-c564-4281-b0a4-2db1eda235bb"), null, 0, "Jest całkiem dobry faktycznie", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4820), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4820), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("adec7066-9bb6-44c7-9563-bfd50d8d1700"), null, 0 },
                    { new Guid("46e439f2-aca0-4c06-ba78-9667bf7ab9eb"), new Guid("8102a794-9d84-4c94-95d0-a2fe6545013b"), 2, "Fajność!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4832), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4832), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 },
                    { new Guid("56756984-fcee-4a8c-af76-13c64fabf98e"), null, 0, "Chyba ty", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4891), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4891), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("5bb0cb86-8a1d-42a8-8dbe-eb1873fd1cb8"), null, 0 },
                    { new Guid("bc002a4b-c5cc-4e91-be84-6b09478c2a11"), null, 0, "No nie wiem. Milik lepszy!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4813), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4813), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("adec7066-9bb6-44c7-9563-bfd50d8d1700"), null, 0 },
                    { new Guid("d3d8ada6-a342-4763-9ff5-38eb3e19b374"), new Guid("ae1e8651-e6a5-487b-823f-6f6f60c4cc6e"), 0, "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4872), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4872), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "CommentReactions",
                columns: new[] { "Id", "CommentId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[] { new Guid("56c0f115-285f-41ff-b545-58b95bd801b4"), new Guid("adec7066-9bb6-44c7-9563-bfd50d8d1700"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6029), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6029), new Guid("00000000-0000-0000-0000-000000000001"), 2 });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("3ed95dee-06e1-4fbe-a83a-1c80cdeaceae"), null, 0, "Zgadza się!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4839), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4839), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("46e439f2-aca0-4c06-ba78-9667bf7ab9eb"), null, 0 },
                    { new Guid("8c256ed6-b5c8-406e-afcd-99628bb10e8a"), null, 0, "Nie, bo ty", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4898), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4898), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("56756984-fcee-4a8c-af76-13c64fabf98e"), null, 0 },
                    { new Guid("abd73285-480d-49ac-adf4-9ec9cc8c6f51"), null, 0, "Nie zgadzam się. Fajność.", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4860), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4860), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("088fb9dc-93d8-4895-9c9d-a86efd404598"), null, 0 },
                    { new Guid("e2428ee9-4830-4a6e-9f75-1b72091d010f"), null, 0, "Też się zgadzam. Fajność!", new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4847), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(4847), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("46e439f2-aca0-4c06-ba78-9667bf7ab9eb"), null, 0 }
                });

            migrationBuilder.InsertData(
                table: "CommentReactions",
                columns: new[] { "Id", "CommentId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("343a47ac-90d1-4b0c-aa5d-6c4852e834fb"), new Guid("46e439f2-aca0-4c06-ba78-9667bf7ab9eb"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6155), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6155), new Guid("00000000-0000-0000-0000-000000000001"), 2 },
                    { new Guid("97de4189-134a-4db1-a4a0-fda6bea3f5b7"), new Guid("46e439f2-aca0-4c06-ba78-9667bf7ab9eb"), new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6143), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 11, 3, 16, 5, 53, 341, DateTimeKind.Utc).AddTicks(6143), new Guid("00000000-0000-0000-0000-000000000001"), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_LastUpdatedById",
                table: "Activities",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PersonId",
                table: "Activities",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityReactions_ActivityId",
                table: "ActivityReactions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityReactions_CreatedById",
                table: "ActivityReactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityReactions_LastUpdatedById",
                table: "ActivityReactions",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTags_CreatedById",
                table: "ActivityTags",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTags_LastUpdatedById",
                table: "ActivityTags",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActivityTags_TagId",
                table: "ActivityTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ActivityId",
                table: "Comment",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedById",
                table: "Comment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LastUpdatedById",
                table: "Comment",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PersonId",
                table: "Comment",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CommentId",
                table: "CommentReactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_CreatedById",
                table: "CommentReactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReactions_LastUpdatedById",
                table: "CommentReactions",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_CreatedById",
                table: "DomainUsers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_LastUpdatedById",
                table: "DomainUsers",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DomainUsers_Username",
                table: "DomainUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonReactions_CreatedById",
                table: "PersonReactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonReactions_LastUpdatedById",
                table: "PersonReactions",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonReactions_PersonId",
                table: "PersonReactions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CreatedById",
                table: "Persons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_LastUpdatedById",
                table: "Persons",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTag_CreatedById",
                table: "PersonTag",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTag_LastUpdatedById",
                table: "PersonTag",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTag_TagId",
                table: "PersonTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedById",
                table: "Tags",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_LastUpdatedById",
                table: "Tags",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteTags_User1Id",
                table: "UserFavoriteTags",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserRecentlyBrowsedTags_UserId",
                table: "UserRecentlyBrowsedTags",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityReactions");

            migrationBuilder.DropTable(
                name: "ActivityTags");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CommentReactions");

            migrationBuilder.DropTable(
                name: "PersonReactions");

            migrationBuilder.DropTable(
                name: "PersonTag");

            migrationBuilder.DropTable(
                name: "UserFavoriteTags");

            migrationBuilder.DropTable(
                name: "UserRecentlyBrowsedTags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "DomainUsers");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
