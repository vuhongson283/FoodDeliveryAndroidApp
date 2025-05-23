USE [FoodDelivery]
GO
/****** Object:  Table [dbo].[ChatMessages]    Script Date: 22/03/2025 11:10:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [nvarchar](100) NULL,
	[Text] [nvarchar](max) NULL,
	[UserID] [int] NULL,
	[SentAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Foods]    Script Date: 22/03/2025 11:10:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Foods](
	[FoodID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[Calories] [int] NULL,
	[Image] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[FoodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 22/03/2025 11:10:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[FoodID] [int] NOT NULL,
	[Quantity] [int] NULL,
	[Price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 22/03/2025 11:10:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerID] [int] NOT NULL,
	[CustomerName] [nvarchar](500) NULL,
	[PhoneNumber] [nchar](20) NULL,
	[Address] [nvarchar](50) NULL,
	[TotalPrice] [decimal](10, 2) NOT NULL,
	[OrderStatus] [nvarchar](50) NULL,
	[CreatedAt] [datetime] NULL,
	[UpdateAt] [datetime] NULL,
	[IsDeleted] [bit] NULL,
	[PaymentMethod] [nvarchar](50) NULL,
 CONSTRAINT [PK__Orders__C3905BAF661B9CF0] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 22/03/2025 11:10:21 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](20) NULL,
	[Address] [nvarchar](max) NULL,
	[Role] [nvarchar](50) NULL,
	[Password] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ChatMessages] ON 

INSERT [dbo].[ChatMessages] ([MessageID], [Sender], [Text], [UserID], [SentAt]) VALUES (1, N'Nguyen Van A', N'Tôi có th? d?t hàng qua dây không?', NULL, CAST(N'2025-03-05T11:05:06.237' AS DateTime))
INSERT [dbo].[ChatMessages] ([MessageID], [Sender], [Text], [UserID], [SentAt]) VALUES (2, N'Tran Thi B', N'Vâng, b?n có th? d?t hàng t?i website.', NULL, CAST(N'2025-03-05T11:05:06.237' AS DateTime))
SET IDENTITY_INSERT [dbo].[ChatMessages] OFF
GO
SET IDENTITY_INSERT [dbo].[Foods] ON 

INSERT [dbo].[Foods] ([FoodID], [Name], [Description], [Price], [Calories], [Image]) VALUES (1, N'Pizza Margherita', N'Pizza v?i s?t cà chua, phô mai mozzarella', CAST(150000.00 AS Decimal(10, 2)), 300, N'pizza.jpg')
INSERT [dbo].[Foods] ([FoodID], [Name], [Description], [Price], [Calories], [Image]) VALUES (2, N'Burger Bò', N'Burger bò v?i rau, phô mai cheddar', CAST(80000.00 AS Decimal(10, 2)), 450, N'burger.jpg')
SET IDENTITY_INSERT [dbo].[Foods] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderDetails] ON 

INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (1, 5, 1, 1, CAST(150000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (2, 6, 1, 1, CAST(150000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (3, 7, 1, 2, CAST(150000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (1002, 1006, 1, 1, CAST(150000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (1003, 1006, 2, 1, CAST(80000.00 AS Decimal(10, 2)))
INSERT [dbo].[OrderDetails] ([OrderDetailID], [OrderID], [FoodID], [Quantity], [Price]) VALUES (1004, 1007, 2, 1, CAST(80000.00 AS Decimal(10, 2)))
SET IDENTITY_INSERT [dbo].[OrderDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON 

INSERT [dbo].[Orders] ([OrderID], [CustomerID], [CustomerName], [PhoneNumber], [Address], [TotalPrice], [OrderStatus], [CreatedAt], [UpdateAt], [IsDeleted], [PaymentMethod]) VALUES (5, 7, NULL, NULL, NULL, CAST(150000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-15T12:25:55.573' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [CustomerName], [PhoneNumber], [Address], [TotalPrice], [OrderStatus], [CreatedAt], [UpdateAt], [IsDeleted], [PaymentMethod]) VALUES (6, 7, N'Lam', N'0869228695          ', N'HN', CAST(150000.00 AS Decimal(10, 2)), N'Pending', CAST(N'2025-03-16T05:50:27.593' AS DateTime), NULL, NULL, N'QR')
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [CustomerName], [PhoneNumber], [Address], [TotalPrice], [OrderStatus], [CreatedAt], [UpdateAt], [IsDeleted], [PaymentMethod]) VALUES (7, 7, N'Lam', N'0869228695          ', N'HN', CAST(300000.00 AS Decimal(10, 2)), N'Canceled', CAST(N'2025-03-16T05:52:40.723' AS DateTime), NULL, NULL, N'QR')
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [CustomerName], [PhoneNumber], [Address], [TotalPrice], [OrderStatus], [CreatedAt], [UpdateAt], [IsDeleted], [PaymentMethod]) VALUES (1006, 7, N'son', N'098765              ', N'ND', CAST(230000.00 AS Decimal(10, 2)), N'Delivered', CAST(N'2025-03-18T14:40:33.637' AS DateTime), NULL, NULL, N'COD')
INSERT [dbo].[Orders] ([OrderID], [CustomerID], [CustomerName], [PhoneNumber], [Address], [TotalPrice], [OrderStatus], [CreatedAt], [UpdateAt], [IsDeleted], [PaymentMethod]) VALUES (1007, 8, N'thao', N'098765221           ', N'TB', CAST(80000.00 AS Decimal(10, 2)), N'OutForDelivery', CAST(N'2025-03-19T03:55:05.183' AS DateTime), NULL, NULL, N'COD')
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserID], [Username], [Email], [PhoneNumber], [Address], [Role], [Password]) VALUES (5, N'JohnDoe', N'john@example.com', N'123456789', N'123 Main St', N'User', N'123456')
INSERT [dbo].[Users] ([UserID], [Username], [Email], [PhoneNumber], [Address], [Role], [Password]) VALUES (6, N'JaneSmith', N'admin@gmail.com', N'987654321', N'456 Elm St', N'Admin', N'123')
INSERT [dbo].[Users] ([UserID], [Username], [Email], [PhoneNumber], [Address], [Role], [Password]) VALUES (7, N'son', N'vuson@gmail.com', N'098765', N'ND', N'User', N'123')
INSERT [dbo].[Users] ([UserID], [Username], [Email], [PhoneNumber], [Address], [Role], [Password]) VALUES (8, N'thao', N'thaobp@gmail.com', N'098765221', N'TB', N'User', N'12345')
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Users__A9D105347FADA6DA]    Script Date: 22/03/2025 11:10:21 CH ******/
ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ChatMessages] ADD  DEFAULT (getdate()) FOR [SentAt]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF__Orders__CreatedA__412EB0B6]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD FOREIGN KEY([FoodID])
REFERENCES [dbo].[Foods] ([FoodID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK__OrderDeta__Order__440B1D61] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK__OrderDeta__Order__440B1D61]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK__Orders__UserID__44FF419A] FOREIGN KEY([CustomerID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK__Orders__UserID__44FF419A]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [CK_OrderStatus] CHECK  (([OrderStatus]='Canceled' OR [OrderStatus]='Delivered' OR [OrderStatus]='OutForDelivery' OR [OrderStatus]='Pending'))
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [CK_OrderStatus]
GO
