USE [FypProject]
GO
/****** Object:  Table [dbo].[AvailableProductCount]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AvailableProductCount](
	[ProductId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[CategoryId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Books]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Books](
	[BookId] [int] IDENTITY(1,1) NOT NULL,
	[BookName] [nvarchar](50) NOT NULL,
	[PublisherName] [nvarchar](50) NOT NULL,
	[WriterName] [nvarchar](50) NOT NULL,
	[Edition] [nvarchar](50) NOT NULL,
	[BookActualPrice] [nchar](10) NOT NULL,
	[BookSellingPrice] [nchar](10) NOT NULL,
	[BookBarCode] [nvarchar](50) NOT NULL,
	[RecordBy] [nvarchar](50) NULL,
	[RecordAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Franchise]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Franchise](
	[FranchiseId] [int] IDENTITY(1,1) NOT NULL,
	[FranchiseName] [nvarchar](50) NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[DisableDate] [datetime] NULL,
	[ContactPerson] [varchar](100) NULL,
	[Address] [varchar](100) NULL,
	[ContactNumber] [varchar](100) NULL,
 CONSTRAINT [PK_Franchise] PRIMARY KEY CLUSTERED 
(
	[FranchiseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Product]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[ProductActualPrice] [nvarchar](50) NOT NULL,
	[ProductSellingPrice] [nvarchar](50) NOT NULL,
	[Barcode] [nvarchar](50) NOT NULL,
	[SubCategoryId] [int] NOT NULL,
	[RecordAt] [datetime] NOT NULL,
	[RecordBy] [nvarchar](50) NOT NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalesDetail]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesDetail](
	[SalesDetailId] [int] IDENTITY(1,1) NOT NULL,
	[SalesMasterId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[Quantity] [nvarchar](50) NOT NULL,
	[SellingPrice] [nvarchar](50) NOT NULL,
	[SellingSubTotal] [nvarchar](50) NOT NULL,
	[ActualPrice] [nvarchar](50) NOT NULL,
	[ActualSubTotal] [nvarchar](50) NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SalesDetail] PRIMARY KEY CLUSTERED 
(
	[SalesDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SalesMaster]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SalesMaster](
	[SalesMasterId] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [nvarchar](50) NOT NULL,
	[NoOfQuantity] [nvarchar](50) NOT NULL,
	[TotalAmount] [nvarchar](50) NOT NULL,
	[Datetime] [datetime] NOT NULL,
	[UserId] [nvarchar](50) NOT NULL,
	[RecivedAmount] [nvarchar](50) NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SalesMaster] PRIMARY KEY CLUSTERED 
(
	[SalesMasterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StockDetail]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockDetail](
	[StockDetailId] [int] IDENTITY(1,1) NOT NULL,
	[StockMasterId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductQuantity] [nvarchar](50) NOT NULL,
	[ProductActualPrice] [nvarchar](50) NOT NULL,
	[SubTotalAmount] [nvarchar](50) NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_StockDetail] PRIMARY KEY CLUSTERED 
(
	[StockDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StockMaster]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockMaster](
	[StockMasterId] [int] IDENTITY(1,1) NOT NULL,
	[ProductTotalQuantity] [nvarchar](50) NOT NULL,
	[ProductTotalAmount] [nvarchar](50) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_StockMaster] PRIMARY KEY CLUSTERED 
(
	[StockMasterId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubCategory]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubCategory](
	[SubCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[SubCategoryName] [nvarchar](50) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_SubCategory] PRIMARY KEY CLUSTERED 
(
	[SubCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransferDetail]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferDetail](
	[TransferDetailId] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NOT NULL,
	[ProductQuantity] [int] NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_TransferDetail] PRIMARY KEY CLUSTERED 
(
	[TransferDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TransferMaster]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransferMaster](
	[TransferId] [int] IDENTITY(1,1) NOT NULL,
	[FranchiseId] [int] NOT NULL,
	[TotalProductQuantity] [int] NOT NULL,
	[Datetime] [datetime] NOT NULL,
	[RecordAt] [datetime] NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
 CONSTRAINT [PK_TransferMaster] PRIMARY KEY CLUSTERED 
(
	[TransferId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/11/2020 4:06:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NULL,
	[Cnic] [nvarchar](50) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[RecordAt] [datetime] NOT NULL,
	[RecordBy] [nvarchar](50) NULL,
	[UpdateAt] [datetime] NULL,
	[UpdateBy] [nvarchar](50) NULL,
	[DisableDate] [datetime] NULL,
	[FranchiseId] [int] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[AvailableProductCount] ([ProductId], [SubCategoryId], [Quantity], [CategoryId]) VALUES (1, 1, 25, 1)
INSERT [dbo].[AvailableProductCount] ([ProductId], [SubCategoryId], [Quantity], [CategoryId]) VALUES (2, 3, 50, 2)
SET IDENTITY_INSERT [dbo].[Books] ON 

INSERT [dbo].[Books] ([BookId], [BookName], [PublisherName], [WriterName], [Edition], [BookActualPrice], [BookSellingPrice], [BookBarCode], [RecordBy], [RecordAt], [UpdateBy], [UpdateAt]) VALUES (1, N'English Text Book', N'Ali Khan', N'Ali Zaidi', N'1st', N'250       ', N'300       ', N'12345678', N'27', CAST(0x0000AC6200C27F3F AS DateTime), N'27', CAST(0x0000AC6200C2AD15 AS DateTime))
INSERT [dbo].[Books] ([BookId], [BookName], [PublisherName], [WriterName], [Edition], [BookActualPrice], [BookSellingPrice], [BookBarCode], [RecordBy], [RecordAt], [UpdateBy], [UpdateAt]) VALUES (2, N'Urdu', N'ali', N'hamza', N'1st ', N'400       ', N'500       ', N'49504367', N'27', CAST(0x0000AC62010C2738 AS DateTime), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Books] OFF
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([CategoryId], [CategoryName], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (1, N'Vehicles', CAST(0x0000AC6700BDEB92 AS DateTime), N'27', CAST(0x0000AC6700BE130E AS DateTime), N'27')
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (2, N'Books', CAST(0x0000AC6700BE08FD AS DateTime), N'27', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Category] OFF
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductId], [ProductName], [Description], [ProductActualPrice], [ProductSellingPrice], [Barcode], [SubCategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (1, N'CD-70', N'Hondaa CD 70..', N'50000', N'55000', N'711670427', 1, CAST(0x0000AC6700D46024 AS DateTime), N'27', CAST(0x0000AC6700F19967 AS DateTime), N'27')
INSERT [dbo].[Product] ([ProductId], [ProductName], [Description], [ProductActualPrice], [ProductSellingPrice], [Barcode], [SubCategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (2, N'English textbook', N'English sindh textbook', N'500', N'600', N'145890190', 3, CAST(0x0000AC69010FEF0E AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[Product] ([ProductId], [ProductName], [Description], [ProductActualPrice], [ProductSellingPrice], [Barcode], [SubCategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (3, N'Maths O level', N'Olvelz maths book', N'400', N'500', N'960956618', 4, CAST(0x0000AC6901100BE7 AS DateTime), N'27', NULL, NULL)
SET IDENTITY_INSERT [dbo].[Product] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (4, N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (5, N'SuperAdmin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (6, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[StockDetail] ON 

INSERT [dbo].[StockDetail] ([StockDetailId], [StockMasterId], [ProductId], [ProductQuantity], [ProductActualPrice], [SubTotalAmount], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (2, 2, 1, N'25', N'50000', N'1250000', CAST(0x0000AC6E0103AD43 AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[StockDetail] ([StockDetailId], [StockMasterId], [ProductId], [ProductQuantity], [ProductActualPrice], [SubTotalAmount], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (3, 2, 2, N'18', N'500', N'9000', CAST(0x0000AC6E0103AD60 AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[StockDetail] ([StockDetailId], [StockMasterId], [ProductId], [ProductQuantity], [ProductActualPrice], [SubTotalAmount], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (4, 3, 2, N'32', N'500', N'16000', CAST(0x0000AC6E0103C8E1 AS DateTime), N'27', NULL, NULL)
SET IDENTITY_INSERT [dbo].[StockDetail] OFF
SET IDENTITY_INSERT [dbo].[StockMaster] ON 

INSERT [dbo].[StockMaster] ([StockMasterId], [ProductTotalQuantity], [ProductTotalAmount], [DateTime], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (2, N'43', N'50500', CAST(0x0000AC6E01039E96 AS DateTime), CAST(0x0000AC6E01039E96 AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[StockMaster] ([StockMasterId], [ProductTotalQuantity], [ProductTotalAmount], [DateTime], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (3, N'32', N'500', CAST(0x0000AC6E0103C8CF AS DateTime), CAST(0x0000AC6E0103C8CF AS DateTime), N'27', NULL, NULL)
SET IDENTITY_INSERT [dbo].[StockMaster] OFF
SET IDENTITY_INSERT [dbo].[SubCategory] ON 

INSERT [dbo].[SubCategory] ([SubCategoryId], [SubCategoryName], [CategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (1, N'Bikes', 1, CAST(0x0000AC6700BF9049 AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[SubCategory] ([SubCategoryId], [SubCategoryName], [CategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (2, N'Cars', 1, CAST(0x0000AC6700BFE90E AS DateTime), N'27', CAST(0x0000AC6700BFFC18 AS DateTime), N'27')
INSERT [dbo].[SubCategory] ([SubCategoryId], [SubCategoryName], [CategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (3, N'A-Levelz', 2, CAST(0x0000AC69010FC35F AS DateTime), N'27', NULL, NULL)
INSERT [dbo].[SubCategory] ([SubCategoryId], [SubCategoryName], [CategoryId], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy]) VALUES (4, N'O-Levelz', 2, CAST(0x0000AC69010FCC5F AS DateTime), N'27', NULL, NULL)
SET IDENTITY_INSERT [dbo].[SubCategory] OFF
INSERT [dbo].[UserRole] ([RoleId], [UserId]) VALUES (5, 27)
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([Id], [UserName], [FirstName], [LastName], [EmailAddress], [Address], [Cnic], [PhoneNumber], [Password], [RecordAt], [RecordBy], [UpdateAt], [UpdateBy], [DisableDate], [FranchiseId]) VALUES (27, N'superadmin', N'application', N'admin', N'admin@gmail.com', NULL, NULL, NULL, N'superAdmin', CAST(0x0000AC5700A6D889 AS DateTime), NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Users] OFF
