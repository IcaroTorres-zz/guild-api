using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
	public partial class Netcoreapp31 : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				"Guilds",
				table => new
				{
					Id = table.Column<Guid>(),
					CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					Disabled = table.Column<bool>(),
					Name = table.Column<string>(nullable: true)
				},
				constraints: table => { table.PrimaryKey("PK_Guilds", x => x.Id); });

			migrationBuilder.CreateTable(
				"Members",
				table => new
				{
					Id = table.Column<Guid>(),
					CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					Disabled = table.Column<bool>(),
					Name = table.Column<string>(nullable: true),
					IsGuildMaster = table.Column<bool>(),
					GuildId = table.Column<Guid>(nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Members", x => x.Id);
					table.ForeignKey(
						"FK_Members_Guilds_GuildId",
						x => x.GuildId,
						"Guilds",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				"Invites",
				table => new
				{
					Id = table.Column<Guid>(),
					CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					Disabled = table.Column<bool>(),
					Status = table.Column<short>(),
					GuildId = table.Column<Guid>(),
					MemberId = table.Column<Guid>()
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Invites", x => x.Id);
					table.ForeignKey(
						"FK_Invites_Guilds_GuildId",
						x => x.GuildId,
						"Guilds",
						"Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						"FK_Invites_Members_MemberId",
						x => x.MemberId,
						"Members",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				"Memberships",
				table => new
				{
					Id = table.Column<Guid>(),
					CreatedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					ModifiedDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
					Disabled = table.Column<bool>(),
					Since = table.Column<DateTime>(),
					Until = table.Column<DateTime>(nullable: true),
					MemberId = table.Column<Guid>(),
					GuildId = table.Column<Guid>()
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Memberships", x => x.Id);
					table.ForeignKey(
						"FK_Memberships_Guilds_GuildId",
						x => x.GuildId,
						"Guilds",
						"Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						"FK_Memberships_Members_MemberId",
						x => x.MemberId,
						"Members",
						"Id",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.InsertData(
				"Guilds",
				new[] {"Id", "CreatedDate", "Disabled", "Name"},
				new object[]
				{
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"),
					new DateTime(2020, 3, 31, 2, 32, 50, 186, DateTimeKind.Utc).AddTicks(4056), false, "NERV"
				});

			migrationBuilder.InsertData(
				"Guilds",
				new[] {"Id", "CreatedDate", "Disabled", "Name"},
				new object[]
				{
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"),
					new DateTime(2020, 3, 31, 2, 32, 50, 215, DateTimeKind.Utc).AddTicks(8222), false,
					"WILE: EVA Pilots"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("3cb0ba50-03d9-48db-93ca-692cb9d68131"),
					new DateTime(2020, 3, 31, 2, 32, 50, 225, DateTimeKind.Utc).AddTicks(8971), false, null, false,
					"Rei Ayanami, EVA 00 Pilot"
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("8cac0c89-b5b4-4b1a-8395-525395ba3d3b"),
					new DateTime(2020, 3, 31, 2, 32, 50, 231, DateTimeKind.Utc).AddTicks(9700), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("3cb0ba50-03d9-48db-93ca-692cb9d68131"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("18234e65-8cb1-42e8-9cf4-e4d37980752a"),
					new DateTime(2020, 3, 31, 2, 32, 50, 226, DateTimeKind.Utc).AddTicks(322), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), false, "Shinji Ikari, EVA 01 Pilot"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("e3f46444-78f3-4b94-b703-bc056121fc16"),
					new DateTime(2020, 3, 31, 2, 32, 50, 226, DateTimeKind.Utc).AddTicks(473), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), true, "Gendo Ikari, NERV Comander"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("84a7896c-264e-44de-9d36-e2edc3d201e7"),
					new DateTime(2020, 3, 31, 2, 32, 50, 226, DateTimeKind.Utc).AddTicks(1664), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), false, "Kozo Fuyutsuk, NERV Deputy Comander"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("bf449ca7-5183-46ad-a2eb-edf804e20a0e"),
					new DateTime(2020, 3, 31, 2, 32, 50, 220, DateTimeKind.Utc).AddTicks(1000), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), false, "Mari Makinami, EVA 05 Pilot"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 31, 2, 32, 50, 225, DateTimeKind.Utc).AddTicks(8739), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), false, "Asuka Langley Sohryu, EVA 02 Pilot"
				});

			migrationBuilder.InsertData(
				"Members",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "IsGuildMaster", "Name"},
				new object[]
				{
					new Guid("9fad438a-3555-4583-9621-2c065ad93084"),
					new DateTime(2020, 3, 31, 2, 32, 50, 226, DateTimeKind.Utc).AddTicks(1848), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), true, "Misato Katsuragi, WILE Comander"
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("8907eb3f-b192-42f1-9fe7-a91c4dd799e0"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7330), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("3cb0ba50-03d9-48db-93ca-692cb9d68131"),
					new DateTime(2020, 3, 18, 0, 14, 49, 870, DateTimeKind.Unspecified).AddTicks(9813),
					new DateTime(2020, 3, 18, 15, 25, 21, 374, DateTimeKind.Unspecified).AddTicks(2161)
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("bedfa422-317d-4640-a4c7-84f47e2fcd6d"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(63), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("18234e65-8cb1-42e8-9cf4-e4d37980752a"),
					(short) 3
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("b2b4625b-d424-4d8e-aee7-19efa63eb062"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(619), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("18234e65-8cb1-42e8-9cf4-e4d37980752a"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("684737ff-ab4b-4b68-b7f1-a8739e0b1840"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(908), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("e3f46444-78f3-4b94-b703-bc056121fc16"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("ab532a05-606d-4931-b81b-8ce522dd4496"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(1346), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("84a7896c-264e-44de-9d36-e2edc3d201e7"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("700dad75-3364-4d75-ab44-cec40dcfdeca"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(416), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("bf449ca7-5183-46ad-a2eb-edf804e20a0e"),
					(short) 4
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("ae609940-4505-4977-813d-d26428672315"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(549), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("bf449ca7-5183-46ad-a2eb-edf804e20a0e"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("f0dc8d5a-89eb-4496-a14c-bd041b36448f"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(1492), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("9fad438a-3555-4583-9621-2c065ad93084"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("89dd1816-dd87-4e8e-97f5-fc5b32f900bd"),
					new DateTime(2020, 3, 31, 2, 32, 50, 228, DateTimeKind.Utc).AddTicks(1445), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("c06ed199-312c-42f0-a582-b471129e41af"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(479), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Invites",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Status"},
				new object[]
				{
					new Guid("1d350c02-d364-41cf-b38c-fa2c242af4c6"),
					new DateTime(2020, 3, 31, 2, 32, 50, 232, DateTimeKind.Utc).AddTicks(1420), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					(short) 2
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("39823673-5f77-4b54-9f2b-ea2818d9c490"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7769), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 18, 15, 27, 11, 115, DateTimeKind.Unspecified).AddTicks(8971), null
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("c9aa80ec-229f-4763-ac30-0b1584ac2943"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7415), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 18, 0, 22, 31, 665, DateTimeKind.Unspecified).AddTicks(6800),
					new DateTime(2020, 3, 18, 15, 17, 3, 492, DateTimeKind.Unspecified).AddTicks(4124)
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("ef2fbc73-2d66-4f81-bb42-1e5dad58c276"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7245), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 18, 0, 4, 35, 630, DateTimeKind.Unspecified).AddTicks(3147),
					new DateTime(2020, 3, 18, 0, 22, 31, 664, DateTimeKind.Unspecified).AddTicks(5485)
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("048fca04-54c9-41fd-af36-a56e01426159"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7162), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 17, 23, 9, 49, 47, DateTimeKind.Unspecified).AddTicks(6435),
					new DateTime(2020, 3, 17, 23, 10, 59, 525, DateTimeKind.Unspecified).AddTicks(1100)
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("d1307584-b524-48e2-91fa-1882c9819ac8"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7485), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("bf449ca7-5183-46ad-a2eb-edf804e20a0e"),
					new DateTime(2020, 3, 18, 1, 3, 38, 622, DateTimeKind.Unspecified).AddTicks(9484), null
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("785e1d4f-99dd-4152-ade0-bce244c0eec1"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(6878), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("bf449ca7-5183-46ad-a2eb-edf804e20a0e"),
					new DateTime(2020, 3, 17, 19, 20, 47, 23, DateTimeKind.Unspecified).AddTicks(5603),
					new DateTime(2020, 3, 17, 19, 54, 32, 580, DateTimeKind.Unspecified).AddTicks(284)
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("408b0fcf-7a0e-46d3-aa2a-8cc41bd8c492"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7711), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("84a7896c-264e-44de-9d36-e2edc3d201e7"),
					new DateTime(2020, 3, 18, 15, 4, 32, 920, DateTimeKind.Unspecified).AddTicks(9975), null
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("63b7cac2-07b5-4bf1-a3ae-90e4b2f65c2e"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7651), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("e3f46444-78f3-4b94-b703-bc056121fc16"),
					new DateTime(2020, 3, 18, 7, 20, 19, 667, DateTimeKind.Unspecified).AddTicks(5361), null
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("970a0fea-aa33-4898-932b-e713045c9fce"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7588), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("18234e65-8cb1-42e8-9cf4-e4d37980752a"),
					new DateTime(2020, 3, 18, 2, 38, 44, 175, DateTimeKind.Unspecified).AddTicks(8894), null
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("63c3c07a-7d10-4585-879e-60ff90f30cfa"),
					new DateTime(2020, 3, 31, 2, 32, 50, 235, DateTimeKind.Utc).AddTicks(2481), false,
					new Guid("96685bc4-dcb7-4b22-90cc-ca83baff8186"), new Guid("5cd3f816-6b0f-4ba6-aeb5-26d70281f7e1"),
					new DateTime(2020, 3, 17, 19, 18, 42, 619, DateTimeKind.Unspecified).AddTicks(29),
					new DateTime(2020, 3, 17, 21, 24, 25, 62, DateTimeKind.Unspecified).AddTicks(6216)
				});

			migrationBuilder.InsertData(
				"Memberships",
				new[] {"Id", "CreatedDate", "Disabled", "GuildId", "MemberId", "Since", "Until"},
				new object[]
				{
					new Guid("016a61d1-dd66-47fe-8404-195ce90be07f"),
					new DateTime(2020, 3, 31, 2, 32, 50, 240, DateTimeKind.Utc).AddTicks(7828), false,
					new Guid("0bf46ff3-42a8-4dbb-a037-009d831b3263"), new Guid("9fad438a-3555-4583-9621-2c065ad93084"),
					new DateTime(2020, 3, 18, 15, 28, 14, 381, DateTimeKind.Unspecified).AddTicks(5450), null
				});

			migrationBuilder.CreateIndex(
				"IX_Guilds_Name",
				"Guilds",
				"Name",
				unique: true);

			migrationBuilder.CreateIndex(
				"IX_Invites_GuildId",
				"Invites",
				"GuildId");

			migrationBuilder.CreateIndex(
				"IX_Invites_MemberId",
				"Invites",
				"MemberId");

			migrationBuilder.CreateIndex(
				"IX_Members_GuildId",
				"Members",
				"GuildId");

			migrationBuilder.CreateIndex(
				"IX_Members_Name",
				"Members",
				"Name",
				unique: true);

			migrationBuilder.CreateIndex(
				"IX_Memberships_GuildId",
				"Memberships",
				"GuildId");

			migrationBuilder.CreateIndex(
				"IX_Memberships_MemberId",
				"Memberships",
				"MemberId");
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				"Invites");

			migrationBuilder.DropTable(
				"Memberships");

			migrationBuilder.DropTable(
				"Members");

			migrationBuilder.DropTable(
				"Guilds");
		}
	}
}