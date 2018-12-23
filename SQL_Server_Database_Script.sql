USE [advert_site]
GO
/****** Object:  Schema [advert_site]    Script Date: 2018-12-23 23:29:10 ******/
CREATE SCHEMA [advert_site]
GO
/****** Object:  Table [advert_site].[Category]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Category](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](45) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Comments]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Comments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[text] [varchar](max) NULL,
	[userid] [nvarchar](450) NOT NULL,
	[listingid] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[userid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Listing_pictures]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Listing_pictures](
	[picture_id] [int] NOT NULL,
	[Listing_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[picture_id] ASC,
	[Listing_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Listings]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Listings](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[userid] [nvarchar](450) NOT NULL,
	[subcategoryid] [int] NOT NULL,
	[name] [varchar](45) NULL,
	[description] [varchar](max) NULL,
	[price] [float] NOT NULL,
	[quantity] [int] NOT NULL,
	[date] [datetime2](0) NOT NULL,
	[verified] [smallint] NULL,
	[display] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Messages]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Messages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[subject] [varchar](max) NULL,
	[text] [varchar](max) NULL,
	[sender_id] [nvarchar](450) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC,
	[sender_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Reviews]    Script Date: 2018-12-23 23:29:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Reviews](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[sellerid] [nvarchar](450) NOT NULL,
	[buyerid] [nvarchar](450) NOT NULL,
	[date] [datetime2](0) NULL,
	[comment] [varchar](max) NULL,
	[evaluation] [smallint] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Subcategory]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Subcategory](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](45) NULL,
	[categoryid] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [advert_site].[Users_has_Messages]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [advert_site].[Users_has_Messages](
	[recipient_id] [nvarchar](450) NOT NULL,
	[Messages_id] [int] NOT NULL,
	[Messages_sender_id] [nvarchar](450) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[recipient_id] ASC,
	[Messages_id] ASC,
	[Messages_sender_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 2018-12-23 23:29:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [advert_site].[Listings] ADD  DEFAULT ((1)) FOR [quantity]
GO
ALTER TABLE [advert_site].[Comments]  WITH CHECK ADD  CONSTRAINT [fk_Comments_Listings11] FOREIGN KEY([listingid])
REFERENCES [advert_site].[Listings] ([id])
GO
ALTER TABLE [advert_site].[Comments] CHECK CONSTRAINT [fk_Comments_Listings11]
GO
ALTER TABLE [advert_site].[Comments]  WITH CHECK ADD  CONSTRAINT [fk_Comments_Users11] FOREIGN KEY([userid])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Comments] CHECK CONSTRAINT [fk_Comments_Users11]
GO
ALTER TABLE [advert_site].[Listing_pictures]  WITH CHECK ADD  CONSTRAINT [fk_Listing_pictures_Listings1] FOREIGN KEY([Listing_id])
REFERENCES [advert_site].[Listings] ([id])
GO
ALTER TABLE [advert_site].[Listing_pictures] CHECK CONSTRAINT [fk_Listing_pictures_Listings1]
GO
ALTER TABLE [advert_site].[Listings]  WITH CHECK ADD  CONSTRAINT [fk_Listings_Subcategory1] FOREIGN KEY([subcategoryid])
REFERENCES [advert_site].[Subcategory] ([id])
GO
ALTER TABLE [advert_site].[Listings] CHECK CONSTRAINT [fk_Listings_Subcategory1]
GO
ALTER TABLE [advert_site].[Listings]  WITH CHECK ADD  CONSTRAINT [fk_Listings_Users1] FOREIGN KEY([userid])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Listings] CHECK CONSTRAINT [fk_Listings_Users1]
GO
ALTER TABLE [advert_site].[Messages]  WITH CHECK ADD  CONSTRAINT [fk_Messages_Users1] FOREIGN KEY([sender_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Messages] CHECK CONSTRAINT [fk_Messages_Users1]
GO
ALTER TABLE [advert_site].[Reviews]  WITH CHECK ADD  CONSTRAINT [id] FOREIGN KEY([buyerid])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Reviews] CHECK CONSTRAINT [id]
GO
ALTER TABLE [advert_site].[Reviews]  WITH CHECK ADD  CONSTRAINT [id1] FOREIGN KEY([sellerid])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Reviews] CHECK CONSTRAINT [id1]
GO
ALTER TABLE [advert_site].[Subcategory]  WITH CHECK ADD  CONSTRAINT [fk_Subcategory_Category1] FOREIGN KEY([categoryid])
REFERENCES [advert_site].[Category] ([id])
GO
ALTER TABLE [advert_site].[Subcategory] CHECK CONSTRAINT [fk_Subcategory_Category1]
GO
ALTER TABLE [advert_site].[Users_has_Messages]  WITH CHECK ADD  CONSTRAINT [fk_Users_has_Messages_Messages1] FOREIGN KEY([Messages_id], [Messages_sender_id])
REFERENCES [advert_site].[Messages] ([id], [sender_id])
GO
ALTER TABLE [advert_site].[Users_has_Messages] CHECK CONSTRAINT [fk_Users_has_Messages_Messages1]
GO
ALTER TABLE [advert_site].[Users_has_Messages]  WITH CHECK ADD  CONSTRAINT [fk_Users_has_Messages_Users1] FOREIGN KEY([recipient_id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [advert_site].[Users_has_Messages] CHECK CONSTRAINT [fk_Users_has_Messages_Users1]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
