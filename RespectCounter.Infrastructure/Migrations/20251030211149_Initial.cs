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
                    { new Guid("0da47907-7151-4366-8fb9-181765bc6918"), null, "User", null },
                    { new Guid("4b1f5bcf-ee2b-46c5-8d6e-662a61814493"), null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RefreshToken", "RefreshTokenExpiration", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), 0, "9bdbfe87-8e21-4bf5-a512-3cfb9f163332", null, false, false, null, null, "SYSTEM_USER", null, null, false, null, null, null, false, "system_user" },
                    { new Guid("959b176f-bb8a-4694-94f1-ac6ad072d0a6"), 0, "59367c57-3cab-47df-adcb-c9a72bfd1dfa", "user@example.com", true, false, null, "USER@EXAMPLE.COM", "USER", "AQAAAAIAAYagAAAAECh69bYd/Uk/dwOL567jKt9SWxzUFpJ3hDdZ9axGLxJTqSp2BCMa2ZQqzqOXAjG0XA==", null, false, null, null, "b57cd635-f6f6-4b59-98b5-699c1a14af52", false, "user" },
                    { new Guid("e037e996-395a-4a07-9336-3d1e7ac2b934"), 0, "f30297b7-6f4f-48d6-bfb6-c80ce59c4ae3", "admin@example.com", true, false, null, "ADMIN@EXAMPLE.COM", "ADMIN", "AQAAAAIAAYagAAAAEAO/iYzc255MLkEF24ZhmOHOF1XwVaNi0bb7XeXVgndlHknUDF+Nb3fdOt7i/i70DA==", null, false, null, null, "b35ece43-c49b-4b43-a951-8e0c2af18931", false, "admin" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("0da47907-7151-4366-8fb9-181765bc6918"), new Guid("959b176f-bb8a-4694-94f1-ac6ad072d0a6") },
                    { new Guid("4b1f5bcf-ee2b-46c5-8d6e-662a61814493"), new Guid("e037e996-395a-4a07-9336-3d1e7ac2b934") }
                });

            migrationBuilder.InsertData(
                table: "DomainUsers",
                columns: new[] { "Id", "AvatarUrl", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "Username" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), null, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), "System" },
                    { new Guid("959b176f-bb8a-4694-94f1-ac6ad072d0a6"), null, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), "user" },
                    { new Guid("e037e996-395a-4a07-9336-3d1e7ac2b934"), null, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 614, DateTimeKind.Utc).AddTicks(7350), new Guid("00000000-0000-0000-0000-000000000001"), "admin" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "AvatarUrl", "Birthday", "Created", "CreatedById", "DeathDate", "Deleted", "Description", "FirstName", "LastName", "LastUpdated", "LastUpdatedById", "Nationality", "NickName", "Profession", "Status" },
                values: new object[,]
                {
                    { new Guid("4b8055d3-427b-405b-9284-0ebdd70f7ec2"), null, new DateOnly(1957, 4, 22), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6129), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Donald", "Tusk", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6129), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Politician", 1 },
                    { new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), null, new DateOnly(1972, 5, 16), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6102), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Andrzej", "Duda", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6102), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Politician", 1 },
                    { new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), null, new DateOnly(1988, 8, 21), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(5272), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Robert", "Lewandowski", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(5272), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "Lewy", "Footballer", 1 },
                    { new Guid("bb683711-c610-4b50-8576-caee58437e52"), null, new DateOnly(1984, 12, 7), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6055), new Guid("00000000-0000-0000-0000-000000000001"), new DateOnly(1, 1, 1), false, "Test desc", "Robert", "Kubica", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6055), new Guid("00000000-0000-0000-0000-000000000001"), "Polish", "", "Racer", 0 }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "Description", "LastUpdated", "LastUpdatedById", "Name" },
                values: new object[,]
                {
                    { new Guid("0a1423bd-c6ee-42f3-90fc-9148e90a795a"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7684), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7684), new Guid("00000000-0000-0000-0000-000000000001"), "Politics" },
                    { new Guid("1cedff05-fea4-43a0-9ac1-937a0342ab47"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7661), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7661), new Guid("00000000-0000-0000-0000-000000000001"), "WEC" },
                    { new Guid("223f09a8-5690-4f78-ab80-c1f5ee4475cd"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7497), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7497), new Guid("00000000-0000-0000-0000-000000000001"), "FC Barcelona" },
                    { new Guid("8654b0d9-7d49-4ced-8797-8100583f4772"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7432), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7432), new Guid("00000000-0000-0000-0000-000000000001"), "Sport" },
                    { new Guid("913bd01f-3572-4d63-be37-6d18c2879f97"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7701), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7701), new Guid("00000000-0000-0000-0000-000000000001"), "PiS" },
                    { new Guid("d194bec8-ee58-49cf-87f2-96e070997dc9"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7479), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7479), new Guid("00000000-0000-0000-0000-000000000001"), "Football" },
                    { new Guid("d8f613fc-2d90-469b-a378-cac1cbc667a7"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7710), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7710), new Guid("00000000-0000-0000-0000-000000000001"), "PO" },
                    { new Guid("e55f0aab-3ef1-4b94-8700-0f929343c714"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7635), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test desc", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7635), new Guid("00000000-0000-0000-0000-000000000001"), "F1" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "Description", "Happend", "LastUpdated", "LastUpdatedById", "Location", "PersonId", "Source", "Status", "Type", "Value" },
                values: new object[,]
                {
                    { new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6478), new Guid("00000000-0000-0000-0000-000000000001"), false, "Test description", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6478), new Guid("00000000-0000-0000-0000-000000000001"), "", new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), "Dude, just trust me", "NotVerified", "Quote", "Milik jest słaby" },
                    { new Guid("960765aa-244d-474b-b969-3f5d2338e777"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6566), new Guid("00000000-0000-0000-0000-000000000001"), false, "Można utknąć w eeeee korku", new DateTime(2010, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6566), new Guid("00000000-0000-0000-0000-000000000001"), "Monaco, MC", new Guid("bb683711-c610-4b50-8576-caee58437e52"), "https://www.youtube.com/watch?v=qbYMoKxif6I", "Verified", "Act", "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali" }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("142fb2fb-64ef-4ab8-a8fa-1e1d0d8b2630"), null, 0, "Nie lubiem go, bo Andrzej to dziwne imię", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7112), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7112), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), 0 },
                    { new Guid("2d7291a0-f4fd-456c-9c16-0fed384f4ad2"), null, 0, "Ja tam mu nei ufam", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7103), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7103), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), 0 },
                    { new Guid("3791ec9d-3764-47e3-b08b-2fd97de93fe5"), null, 2, "Najlepszy zawodnik!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6804), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6804), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 0 },
                    { new Guid("684e8514-4717-4677-9cb8-5394c931cb10"), null, 3, "Bardzo memiczna osoba", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7040), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7040), new Guid("00000000-0000-0000-0000-000000000001"), null, new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), 0 }
                });

            migrationBuilder.InsertData(
                table: "PersonReactions",
                columns: new[] { "Id", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "PersonId", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("0f4929d8-2160-4934-aeed-ae5aa5b90bdc"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8529), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8529), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), -1 },
                    { new Guid("1ef89e76-56c9-4c91-aaca-e457866bed85"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8572), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8572), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 2 },
                    { new Guid("3edaddee-3531-49ec-8bfd-f3a68c35e667"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8481), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8481), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), -2 },
                    { new Guid("4765dba1-879a-4bfb-8fe1-d17023c74f6e"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8543), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8543), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 2 },
                    { new Guid("72be4ae8-726c-40a4-8b71-9ede53a89daf"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8553), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8553), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 2 },
                    { new Guid("b250bb3e-d705-4983-98d8-545530f131d9"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8565), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8565), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 2 },
                    { new Guid("b65b0951-601a-42ec-8b01-ecb2cfbbdd27"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8514), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8514), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 1 },
                    { new Guid("f93044ae-fe11-4929-8e50-69f710ab2775"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8536), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8536), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), 1 }
                });

            migrationBuilder.InsertData(
                table: "PersonTag",
                columns: new[] { "PersonId", "TagId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById" },
                values: new object[,]
                {
                    { new Guid("4b8055d3-427b-405b-9284-0ebdd70f7ec2"), new Guid("0a1423bd-c6ee-42f3-90fc-9148e90a795a"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8134), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8135), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("4b8055d3-427b-405b-9284-0ebdd70f7ec2"), new Guid("d8f613fc-2d90-469b-a378-cac1cbc667a7"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8139), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8140), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), new Guid("0a1423bd-c6ee-42f3-90fc-9148e90a795a"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8132), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8132), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("9ea590e1-0646-4255-a1f7-6b8b7f87a967"), new Guid("913bd01f-3572-4d63-be37-6d18c2879f97"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8137), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8137), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), new Guid("223f09a8-5690-4f78-ab80-c1f5ee4475cd"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8121), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8122), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), new Guid("8654b0d9-7d49-4ced-8797-8100583f4772"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8110), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8111), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("ad612e08-7e7b-4b51-a876-2cc78280cfe7"), new Guid("d194bec8-ee58-49cf-87f2-96e070997dc9"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8119), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8119), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("bb683711-c610-4b50-8576-caee58437e52"), new Guid("1cedff05-fea4-43a0-9ac1-937a0342ab47"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8127), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8128), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("bb683711-c610-4b50-8576-caee58437e52"), new Guid("8654b0d9-7d49-4ced-8797-8100583f4772"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8116), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8117), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("bb683711-c610-4b50-8576-caee58437e52"), new Guid("e55f0aab-3ef1-4b94-8700-0f929343c714"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8124), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8124), new Guid("00000000-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "ActivityReactions",
                columns: new[] { "Id", "ActivityId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("0baee526-2c86-4ec5-b7ff-b1085269325d"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8766), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8766), new Guid("00000000-0000-0000-0000-000000000001"), 2 },
                    { new Guid("7985c37a-a59f-425a-8cb3-5b759a727253"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8740), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8740), new Guid("00000000-0000-0000-0000-000000000001"), -1 },
                    { new Guid("88b45527-efe7-4942-b56f-1466b825a4ef"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8756), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8756), new Guid("00000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("93576565-0c14-4a0d-8f38-9f196f93d50a"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8715), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8715), new Guid("00000000-0000-0000-0000-000000000001"), 1 }
                });

            migrationBuilder.InsertData(
                table: "ActivityTags",
                columns: new[] { "ActivityId", "TagId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById" },
                values: new object[,]
                {
                    { new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new Guid("8654b0d9-7d49-4ced-8797-8100583f4772"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8350), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8351), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), new Guid("d194bec8-ee58-49cf-87f2-96e070997dc9"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8356), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8356), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("960765aa-244d-474b-b969-3f5d2338e777"), new Guid("8654b0d9-7d49-4ced-8797-8100583f4772"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8358), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8359), new Guid("00000000-0000-0000-0000-000000000000") },
                    { new Guid("960765aa-244d-474b-b969-3f5d2338e777"), new Guid("e55f0aab-3ef1-4b94-8700-0f929343c714"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8360), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8361), new Guid("00000000-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("2e71ed28-e86a-40f3-8856-8833ee3fea46"), new Guid("960765aa-244d-474b-b969-3f5d2338e777"), 0, "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7022), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7022), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 },
                    { new Guid("4368bcac-c6a5-41f3-b1b7-56fbccac97af"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), 2, "Fajność!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6928), new Guid("00000000-0000-0000-0000-000000000001"), false, 2, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6928), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 },
                    { new Guid("5f453278-1214-45bf-ad1d-f62ef540bce0"), null, 0, "Hańba!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7062), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7062), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("684e8514-4717-4677-9cb8-5394c931cb10"), null, 0 },
                    { new Guid("6e4c16c8-70e4-4a31-9e90-65e99d6ca77c"), null, 0, "Jest całkiem dobry faktycznie", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6902), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6902), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("3791ec9d-3764-47e3-b08b-2fd97de93fe5"), null, 0 },
                    { new Guid("75d8cffa-3335-4911-8749-ff9872249fad"), null, 0, "Chyba ty", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7076), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7076), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("684e8514-4717-4677-9cb8-5394c931cb10"), null, 0 },
                    { new Guid("89924f9e-d6bd-45b9-adfa-719993b38ae1"), null, 0, "No nie wiem. Milik lepszy!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6870), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6870), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("3791ec9d-3764-47e3-b08b-2fd97de93fe5"), null, 0 },
                    { new Guid("fe83f076-b60a-430c-9401-dee9422a31a3"), new Guid("5e57c257-def4-4532-9325-dc162c3bca46"), 1, "Niefajność", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6982), new Guid("00000000-0000-0000-0000-000000000001"), false, 1, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6982), new Guid("00000000-0000-0000-0000-000000000001"), null, null, 0 }
                });

            migrationBuilder.InsertData(
                table: "CommentReactions",
                columns: new[] { "Id", "CommentId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[] { new Guid("57fd04d4-5ab3-43a3-a0dd-29e290d47c41"), new Guid("3791ec9d-3764-47e3-b08b-2fd97de93fe5"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8986), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(8986), new Guid("00000000-0000-0000-0000-000000000001"), 2 });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "AllChildrenCount", "Content", "Created", "CreatedById", "Deleted", "DirectChildrenCount", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId", "Status" },
                values: new object[,]
                {
                    { new Guid("0f278b96-aa7c-45f4-976e-3b0b12ee513b"), null, 0, "Też się zgadzam. Fajność!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6974), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6974), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("4368bcac-c6a5-41f3-b1b7-56fbccac97af"), null, 0 },
                    { new Guid("17acc2c9-a090-4810-bda6-061192569a30"), null, 0, "Nie, bo ty", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7088), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(7088), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("75d8cffa-3335-4911-8749-ff9872249fad"), null, 0 },
                    { new Guid("898ee461-f014-4a85-b8c3-fd289b3e6dd0"), null, 0, "Nie zgadzam się. Fajność.", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6995), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6995), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("fe83f076-b60a-430c-9401-dee9422a31a3"), null, 0 },
                    { new Guid("eeda22f0-99bc-4af6-b973-47071738c89e"), null, 0, "Zgadza się!", new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6952), new Guid("00000000-0000-0000-0000-000000000001"), false, 0, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(6952), new Guid("00000000-0000-0000-0000-000000000001"), new Guid("4368bcac-c6a5-41f3-b1b7-56fbccac97af"), null, 0 }
                });

            migrationBuilder.InsertData(
                table: "CommentReactions",
                columns: new[] { "Id", "CommentId", "Created", "CreatedById", "Deleted", "LastUpdated", "LastUpdatedById", "ReactionType" },
                values: new object[,]
                {
                    { new Guid("0b24e92f-da5f-457e-81eb-ff41ee877f35"), new Guid("4368bcac-c6a5-41f3-b1b7-56fbccac97af"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(9013), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(9013), new Guid("00000000-0000-0000-0000-000000000001"), 1 },
                    { new Guid("dea0704b-3b6f-4ea7-bb93-e7f6f99b6aa3"), new Guid("4368bcac-c6a5-41f3-b1b7-56fbccac97af"), new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(9031), new Guid("00000000-0000-0000-0000-000000000001"), false, new DateTime(2025, 10, 30, 21, 11, 47, 889, DateTimeKind.Utc).AddTicks(9031), new Guid("00000000-0000-0000-0000-000000000001"), 2 }
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
