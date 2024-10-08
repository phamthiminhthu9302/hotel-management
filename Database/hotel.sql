USE [HotelManagerment]
GO
/****** Object:  UserDefinedFunction [dbo].[ConvertString]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConvertString] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END
GO
/****** Object:  Table [dbo].[Access]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Access](
	[IDStaffType] [int] NOT NULL,
	[IDJob] [int] NOT NULL,
 CONSTRAINT [PK_Access] PRIMARY KEY CLUSTERED 
(
	[IDStaffType] ASC,
	[IDJob] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bill]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bill](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDBookRoom] [int] NULL,
	[IDStaff] [int] NULL,
	[DateOfCreate] [smalldatetime] NULL,
	[RoomPrice] [int] NULL,
	[ServicePrice] [int] NULL,
	[TotalPrice] [int] NULL,
	[Discount] [int] NULL,
	[StatusBill] [nvarchar](100) NULL,
	[Surcharge] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillDetails]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillDetails](
	[IDBill] [int] NOT NULL,
	[IDService] [int] NOT NULL,
	[Count] [int] NOT NULL,
	[TotalPrice] [int] NOT NULL,
 CONSTRAINT [PK_BillInfo] PRIMARY KEY CLUSTERED 
(
	[IDService] ASC,
	[IDBill] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookRoom]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookRoom](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IDCustomer] [int] NOT NULL,
	[IDStaff] [int] NOT NULL,
	[IDRoom] [int] NOT NULL,
	[DateBookRoom] [smalldatetime] NOT NULL,
	[DateCheckIn] [date] NOT NULL,
	[DateCheckOut] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BookRoomDetails]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BookRoomDetails](
	[IDBookRoom] [int] NOT NULL,
	[IDCustomerOther] [int] NOT NULL,
 CONSTRAINT [PK_ReceiveRoomDetails] PRIMARY KEY CLUSTERED 
(
	[IDBookRoom] ASC,
	[IDCustomerOther] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[PhoneNumber] [varchar](100) NOT NULL,
	[Sex] [nvarchar](100) NOT NULL,
	[Nationality] [nvarchar](100) NOT NULL,
	[IDCard] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[NameForm] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[Name] [nvarchar](100) NOT NULL,
	[Value] [float] NULL,
	[Describe] [nvarchar](200) NULL,
	[DateModify] [smalldatetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[idRoomType] [int] NOT NULL,
	[value] [int] NOT NULL,
	[rate] [float] NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idRoomType] ASC,
	[Month] ASC,
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Room]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Room](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IDRoomType] [int] NOT NULL,
	[StatusRoom] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoomType]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoomType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [int] NOT NULL,
	[LimitPerson] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Service]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Service](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[IDServiceType] [int] NOT NULL,
	[Price] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ServiceType]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ServiceType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[PassWord] [nvarchar](100) NOT NULL,
	[IDStaffType] [int] NOT NULL,
	[IDCard] [varchar](100) NOT NULL,
	[DateOfBirth] [date] NOT NULL,
	[Sex] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[PhoneNumber] [varchar](100) NOT NULL,
	[StartDay] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StaffType]    Script Date: 12/8/2023 11:56:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StaffType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 1)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 2)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 3)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 4)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 5)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 6)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 7)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 8)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (1, 9)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (2, 1)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (2, 4)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (2, 6)
INSERT [dbo].[Access] ([IDStaffType], [IDJob]) VALUES (5, 4)
GO
SET IDENTITY_INSERT [dbo].[Bill] ON 

INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (7, 29, 1, CAST(N'2023-11-27T14:32:00' AS SmallDateTime), 20000000, 1900000, 26900000, 0, N'Đã thanh toán', 5000000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (8, 30, 1, CAST(N'2023-11-27T14:35:00' AS SmallDateTime), 2000000, 2900000, 5100000, 10, N'Đã thanh toán', 200000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (9, 31, 1, CAST(N'2023-11-30T22:41:00' AS SmallDateTime), 2000000, 800000, 3000000, 0, N'Đã thanh toán', 200000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (10, 33, 1, CAST(N'2023-11-30T23:22:00' AS SmallDateTime), 7000000, 1700000, 10100000, 0, N'Đã thanh toán', 1400000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (12, 35, 1, CAST(N'2023-12-01T10:53:00' AS SmallDateTime), 7000000, 450000, 8850000, 0, N'Đã thanh toán', 1400000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (13, 36, 1, CAST(N'2023-12-01T10:54:00' AS SmallDateTime), 12000000, 0, 13800000, 0, N'Đã thanh toán', 1800000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (14, 37, 1, CAST(N'2023-12-01T13:22:00' AS SmallDateTime), 10000000, 2000000, 14500000, 0, N'Đã thanh toán', 2500000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (15, 38, 1, CAST(N'2023-12-01T14:01:00' AS SmallDateTime), 10000000, 0, 12500000, 0, N'Đã thanh toán', 2500000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (16, 39, 1, CAST(N'2023-12-01T14:05:00' AS SmallDateTime), 4000000, 0, 4600000, 10, N'Đã thanh toán', 600000)
INSERT [dbo].[Bill] ([ID], [IDBookRoom], [IDStaff], [DateOfCreate], [RoomPrice], [ServicePrice], [TotalPrice], [Discount], [StatusBill], [Surcharge]) VALUES (17, 40, 1, CAST(N'2023-12-01T14:24:00' AS SmallDateTime), 10000000, 750000, 13250000, 0, N'Đã thanh toán', 2500000)
SET IDENTITY_INSERT [dbo].[Bill] OFF
GO
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (9, 2, 1, 200000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (10, 3, 1, 200000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (12, 3, 1, 200000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (17, 3, 2, 400000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (12, 6, 1, 200000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (17, 6, 1, 200000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (8, 8, 1, 400000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (8, 11, 2, 2000000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (10, 14, 3, 1500000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (9, 15, 1, 600000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (12, 17, 1, 50000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (17, 19, 1, 150000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (7, 21, 2, 100000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (7, 22, 1, 1800000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (8, 27, 1, 500000)
INSERT [dbo].[BillDetails] ([IDBill], [IDService], [Count], [TotalPrice]) VALUES (14, 30, 1, 2000000)
GO
SET IDENTITY_INSERT [dbo].[BookRoom] ON 

INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (29, 12, 1, 1, CAST(N'2023-11-27T14:30:00' AS SmallDateTime), CAST(N'2023-11-27' AS Date), CAST(N'2023-11-29' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (30, 13, 1, 4, CAST(N'2023-11-27T14:33:00' AS SmallDateTime), CAST(N'2023-11-27' AS Date), CAST(N'2023-11-29' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (31, 5, 1, 4, CAST(N'2023-11-27T15:11:00' AS SmallDateTime), CAST(N'2023-11-27' AS Date), CAST(N'2023-11-29' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (33, 13, 1, 2, CAST(N'2023-11-30T23:21:00' AS SmallDateTime), CAST(N'2023-11-30' AS Date), CAST(N'2023-12-01' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (35, 14, 1, 2, CAST(N'2023-12-01T10:52:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-02' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (36, 15, 1, 3, CAST(N'2023-12-01T10:54:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-04' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (37, 12, 1, 1, CAST(N'2023-12-01T13:20:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-02' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (38, 9, 1, 1, CAST(N'2023-12-01T13:25:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-02' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (39, 12, 1, 3, CAST(N'2023-12-01T14:04:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-02' AS Date))
INSERT [dbo].[BookRoom] ([ID], [IDCustomer], [IDStaff], [IDRoom], [DateBookRoom], [DateCheckIn], [DateCheckOut]) VALUES (40, 39, 11, 1, CAST(N'2023-12-01T14:18:00' AS SmallDateTime), CAST(N'2023-12-01' AS Date), CAST(N'2023-12-02' AS Date))
SET IDENTITY_INSERT [dbo].[BookRoom] OFF
GO
INSERT [dbo].[BookRoomDetails] ([IDBookRoom], [IDCustomerOther]) VALUES (29, 36)
INSERT [dbo].[BookRoomDetails] ([IDBookRoom], [IDCustomerOther]) VALUES (31, 13)
INSERT [dbo].[BookRoomDetails] ([IDBookRoom], [IDCustomerOther]) VALUES (36, 12)
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (5, N'Vi Chí Thiện', CAST(N'1998-04-06' AS Date), N'Đồng Nai', N'0142537569', N'Nam', N'Việt Nam', N'121345678')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (9, N'Nguyễn Văn Thịnh', CAST(N'1998-07-08' AS Date), N'Tam Kỳ, Quảng Nam', N'0123456789', N'Nam', N'Việt Nam', N'123456788')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (12, N'Trần Gia Nghĩa', CAST(N'1998-04-06' AS Date), N'Thái bình', N'0147258369', N'Nam', N'Việt Nam', N'123456789')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (13, N'Nguyễn Ngọc Dung', CAST(N'1998-03-31' AS Date), N'Nha Trang', N'0424245245', N'Nữ', N'Việt Nam', N'237221369')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (14, N'Nguyễn Văn Anh', CAST(N'1998-04-06' AS Date), N'Hồ Chí Minh', N'0195247365', N'Nam', N'Việt Nam', N'123458523')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (15, N'Lê Văn Dũng', CAST(N'2000-02-08' AS Date), N'Hà Nội', N'0348470870', N'Nam', N'Hàn Quốc', N'113305429')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (16, N'Phạm Như Thảo', CAST(N'1998-04-06' AS Date), N'Sài Gòn', N'0124535789', N'Nữ', N'Việt Nam', N'147258369')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (36, N'Võ Văn Sơn', CAST(N'1998-04-06' AS Date), N'Đà Nẵng', N'0348470836', N'Nam', N'Việt Nam', N'741852963')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (37, N'Đinh Mạnh Ninh', CAST(N'1998-04-06' AS Date), N'Nha Trang', N'0112451472', N'Nam', N'Việt Nam', N'112233445')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (38, N'Hồ Thị Loan Anh', CAST(N'1990-07-06' AS Date), N'Hồ Chí Minh', N'0174852963', N'Nữ', N'Việt Nam', N'987415263')
INSERT [dbo].[Customer] ([ID], [Name], [DateOfBirth], [Address], [PhoneNumber], [Sex], [Nationality], [IDCard]) VALUES (39, N'Phạm Anh Thư', CAST(N'1998-04-06' AS Date), N'Sài Gòn', N'0112356791', N'Nam', N'Việt Nam', N'147258123')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Job] ON 

INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (1, N'Đặt Phòng', N'fBookRoom')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (2, N'Sử dụng dịch vụ và Thanh toán', N'fUseService')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (3, N'Thống kê và doanh thu', N'fReport')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (4, N'Quản lí phòng', N'fRoom')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (5, N'Quản lí nhân viên', N'fStaff')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (6, N'Quản lí khách hàng', N'fCustomer')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (7, N'Quản lí hoá đơn', N'fBill')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (8, N'Quản lí dịch vụ', N'fService')
INSERT [dbo].[Job] ([ID], [Name], [NameForm]) VALUES (9, N'Quy định', N'fParameter')
SET IDENTITY_INSERT [dbo].[Job] OFF
GO
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ1.1', 0, N'Phòng Suite (SUT) có tối đa 9 người', CAST(N'2023-12-01T13:09:00' AS SmallDateTime))
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ1.2', 0, N'Phòng Deluxe (DLX) có tối đa 6 người', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ1.3', 0, N'Phòng Superior (SUP) có tối đa 4 người', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ1.4', 0, N'Phòng Standard (STD) có tối đa 3 người', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ2.1', 0.25, N'Đơn giá phòng cho 6 khách Phòng Suite (SUT)', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ2.2', 0.2, N'Đơn giá phòng cho 4 khách Phòng Deluxe (DLX)', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ2.3', 0.15, N'Đơn giá phòng cho 3 khách Phòng Superior (SUP)', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ2.4', 0.1, N'Đơn giá phòng cho 2 khách Phòng Phòng Standard (STD)', NULL)
INSERT [dbo].[Parameter] ([Name], [Value], [Describe], [DateModify]) VALUES (N'QĐ3', 0.25, N'Mỗi khách hàng vượt số lượng tiêu chuẩn phụ thu thêm 0.25', NULL)
GO
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (1, 26900000, 0, 11, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (1, 40250000, 0, 12, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (2, 10100000, 0, 11, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (2, 8850000, 0, 12, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (3, 0, 0, 11, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (3, 18400000, 0, 12, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (4, 8100000, 0, 11, 2023)
INSERT [dbo].[Report] ([idRoomType], [value], [rate], [Month], [Year]) VALUES (4, 0, 0, 12, 2023)
GO
SET IDENTITY_INSERT [dbo].[Room] ON 

INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (1, N'Phòng 101', 1, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (2, N'Phòng 102', 2, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (3, N'Phòng 103', 3, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (4, N'Phòng 104', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (5, N'Phòng 105', 1, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (6, N'Phòng 106', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (7, N'Phòng 107', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (8, N'Phòng 108', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (9, N'Phòng 109', 3, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (10, N'Phòng 201', 3, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (11, N'Phòng 202', 3, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (12, N'Phòng 203', 2, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (13, N'Phòng 204', 2, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (14, N'Phòng 205', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (15, N'Phòng 206', 3, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (16, N'Phòng 207', 2, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (17, N'Phòng 208', 1, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (18, N'Phòng 209', 2, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (19, N'Phòng 210', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (20, N'Phòng 211', 4, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (25, N'Phòng 301', 1, N'Trống')
INSERT [dbo].[Room] ([ID], [Name], [IDRoomType], [StatusRoom]) VALUES (26, N'Phòng 302', 2, N'Trống')
SET IDENTITY_INSERT [dbo].[Room] OFF
GO
SET IDENTITY_INSERT [dbo].[RoomType] ON 

INSERT [dbo].[RoomType] ([ID], [Name], [Price], [LimitPerson]) VALUES (1, N'Phòng Suite (SUT)', 10000000, 6)
INSERT [dbo].[RoomType] ([ID], [Name], [Price], [LimitPerson]) VALUES (2, N'Phòng Deluxe (DLX)', 7000000, 4)
INSERT [dbo].[RoomType] ([ID], [Name], [Price], [LimitPerson]) VALUES (3, N'Phòng Superior (SUP)', 4000000, 3)
INSERT [dbo].[RoomType] ([ID], [Name], [Price], [LimitPerson]) VALUES (4, N'Phòng Standard (STD)', 1000000, 2)
SET IDENTITY_INSERT [dbo].[RoomType] OFF
GO
SET IDENTITY_INSERT [dbo].[Service] ON 

INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (1, N'Spa', 2, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (2, N'Fitness', 2, 100000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (3, N'Tam dương trùng phùng với bông cải uyên ương', 1, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (4, N'Karaoke', 2, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (6, N'Giặt ủi quần áo', 3, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (8, N'Dịch vụ xe đưa đón sân bay', 3, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (9, N'Dịch vụ cho thuê tự lái', 3, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (10, N'Dịch vụ trông trẻ', 3, 800000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (11, N'Bể bơi 4 mùa', 2, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (13, N'Sân tennis', 2, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (14, N'Mực khoanh chiên vàng & Gỏi bắp bò rau mùi với bánh tráng mè kiểu Thái & Phượng Hoàng tảo biển', 1, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (15, N'Súp nấm hải vị với gà và thịt cua', 1, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (16, N'Heo sữa quay bánh bao nửa con', 1, 2000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (17, N'Chè bột báng trái cây kiểu Melaka', 1, 50000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (18, N'Gỏi bò Cung đình & Phượng hoàng tảo biển & Sườn heo nướng kiểu Hàn Quốc', 1, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (19, N'Súp kem bắp hải sản kiểu Pháp', 1, 150000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (20, N'Cơm chiên gà cá mặn kiểu Hongkong', 1, 75000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (21, N'Bánh tiramisu socola', 1, 50000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (22, N'Chivas 18', 4, 1800000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (23, N'Vodka 3 Zoka', 4, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (24, N'Chivas 12', 4, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (25, N'Cocktail Negroni', 4, 300000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (26, N'Cocktail Bellini', 4, 300000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (27, N'Cocktail The Bloody Mary', 4, 500000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (28, N'Cocktail Old Fashioned', 4, 400000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (29, N'Lẩu dê', 1, 100000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (30, N'Vịt quay Bắc Kinh', 1, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (31, N'Lẩu hải sản', 1, 1000000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (32, N'Cháo vịt', 1, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (33, N'Gà KFC', 1, 200000)
INSERT [dbo].[Service] ([ID], [Name], [IDServiceType], [Price]) VALUES (34, N'Rượu Chateau Corbin 2014', 4, 2000000)
SET IDENTITY_INSERT [dbo].[Service] OFF
GO
SET IDENTITY_INSERT [dbo].[ServiceType] ON 

INSERT [dbo].[ServiceType] ([ID], [Name]) VALUES (1, N'Ăn uống')
INSERT [dbo].[ServiceType] ([ID], [Name]) VALUES (2, N'Giải trí')
INSERT [dbo].[ServiceType] ([ID], [Name]) VALUES (3, N'Tiện ích')
INSERT [dbo].[ServiceType] ([ID], [Name]) VALUES (4, N'Đồ uống')
SET IDENTITY_INSERT [dbo].[ServiceType] OFF
GO
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([ID], [UserName], [Name], [PassWord], [IDStaffType], [IDCard], [DateOfBirth], [Sex], [Address], [PhoneNumber], [StartDay]) VALUES (1, N'admin', N'Nguyễn Văn Thiện', N'c4ca4238a0b923820dcc509a6f75849b', 1, N'124578033', CAST(N'1996-04-18' AS Date), N'Nam', N'Hà Nội', N'0147852581', CAST(N'2018-05-16' AS Date))
INSERT [dbo].[Staff] ([ID], [UserName], [Name], [PassWord], [IDStaffType], [IDCard], [DateOfBirth], [Sex], [Address], [PhoneNumber], [StartDay]) VALUES (2, N'meo', N'Trần Anh Hùng', N'e10adc3949ba59abbe56e057f20f883e', 5, N'271254334', CAST(N'1990-01-01' AS Date), N'Nam', N'Đồng Nai', N'0123433789', CAST(N'2018-05-16' AS Date))
INSERT [dbo].[Staff] ([ID], [UserName], [Name], [PassWord], [IDStaffType], [IDCard], [DateOfBirth], [Sex], [Address], [PhoneNumber], [StartDay]) VALUES (11, N'panda', N'Trương Thế Vinh', N'e10adc3949ba59abbe56e057f20f883e', 2, N'456123789', CAST(N'1998-04-06' AS Date), N'Nữ', N'Nha Trang', N'0258369147', CAST(N'2023-11-27' AS Date))
INSERT [dbo].[Staff] ([ID], [UserName], [Name], [PassWord], [IDStaffType], [IDCard], [DateOfBirth], [Sex], [Address], [PhoneNumber], [StartDay]) VALUES (12, N'momo', N'Phạm Lan Hương', N'e10adc3949ba59abbe56e057f20f883e', 2, N'333166751', CAST(N'1990-01-01' AS Date), N'Nữ', N'Hồ Chí MInh', N'0135798463', CAST(N'2018-05-16' AS Date))
INSERT [dbo].[Staff] ([ID], [UserName], [Name], [PassWord], [IDStaffType], [IDCard], [DateOfBirth], [Sex], [Address], [PhoneNumber], [StartDay]) VALUES (13, N'ndc', N'nguyễn như', N'e10adc3949ba59abbe56e057f20f883e', 2, N'123456321', CAST(N'1998-04-06' AS Date), N'Nữ', N'Hồ chí Minh', N'0147147123', CAST(N'2023-12-01' AS Date))
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO
SET IDENTITY_INSERT [dbo].[StaffType] ON 

INSERT [dbo].[StaffType] ([ID], [Name]) VALUES (1, N'Quản lí')
INSERT [dbo].[StaffType] ([ID], [Name]) VALUES (2, N'Lễ tân')
INSERT [dbo].[StaffType] ([ID], [Name]) VALUES (5, N'Dọn phòng')
SET IDENTITY_INSERT [dbo].[StaffType] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Paramete__737584F6987C30CE]    Script Date: 12/8/2023 11:56:34 PM ******/
ALTER TABLE [dbo].[Parameter] ADD UNIQUE NONCLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Staff__43A2A4E391E69921]    Script Date: 12/8/2023 11:56:34 PM ******/
ALTER TABLE [dbo].[Staff] ADD UNIQUE NONCLUSTERED 
(
	[IDCard] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BillDetails] ADD  DEFAULT ((0)) FOR [TotalPrice]
GO
ALTER TABLE [dbo].[Customer] ADD  DEFAULT (N'No Name') FOR [Name]
GO
ALTER TABLE [dbo].[Parameter] ADD  DEFAULT (N'No Name') FOR [Name]
GO
ALTER TABLE [dbo].[Parameter] ADD  DEFAULT (getdate()) FOR [DateModify]
GO
ALTER TABLE [dbo].[Report] ADD  DEFAULT ((0)) FOR [value]
GO
ALTER TABLE [dbo].[Report] ADD  DEFAULT ((0)) FOR [rate]
GO
ALTER TABLE [dbo].[Report] ADD  DEFAULT ((1)) FOR [Month]
GO
ALTER TABLE [dbo].[Report] ADD  DEFAULT ((1990)) FOR [Year]
GO
ALTER TABLE [dbo].[RoomType] ADD  DEFAULT (N'No Name') FOR [Name]
GO
ALTER TABLE [dbo].[Access]  WITH CHECK ADD  CONSTRAINT [FK_Access_Job] FOREIGN KEY([IDJob])
REFERENCES [dbo].[Job] ([ID])
GO
ALTER TABLE [dbo].[Access] CHECK CONSTRAINT [FK_Access_Job]
GO
ALTER TABLE [dbo].[Access]  WITH CHECK ADD  CONSTRAINT [FK_Access_StaffType] FOREIGN KEY([IDStaffType])
REFERENCES [dbo].[StaffType] ([ID])
GO
ALTER TABLE [dbo].[Access] CHECK CONSTRAINT [FK_Access_StaffType]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_BookRoom] FOREIGN KEY([IDBookRoom])
REFERENCES [dbo].[BookRoom] ([ID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_BookRoom]
GO
ALTER TABLE [dbo].[Bill]  WITH CHECK ADD  CONSTRAINT [FK_Bill_Staff] FOREIGN KEY([IDStaff])
REFERENCES [dbo].[Staff] ([ID])
GO
ALTER TABLE [dbo].[Bill] CHECK CONSTRAINT [FK_Bill_Staff]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_Bill] FOREIGN KEY([IDBill])
REFERENCES [dbo].[Bill] ([ID])
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_Bill]
GO
ALTER TABLE [dbo].[BillDetails]  WITH CHECK ADD  CONSTRAINT [FK_BillDetails_Service] FOREIGN KEY([IDService])
REFERENCES [dbo].[Service] ([ID])
GO
ALTER TABLE [dbo].[BillDetails] CHECK CONSTRAINT [FK_BillDetails_Service]
GO
ALTER TABLE [dbo].[BookRoom]  WITH CHECK ADD FOREIGN KEY([IDCustomer])
REFERENCES [dbo].[Customer] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BookRoom]  WITH CHECK ADD  CONSTRAINT [FK_BookRoom_Room] FOREIGN KEY([IDRoom])
REFERENCES [dbo].[Room] ([ID])
GO
ALTER TABLE [dbo].[BookRoom] CHECK CONSTRAINT [FK_BookRoom_Room]
GO
ALTER TABLE [dbo].[BookRoomDetails]  WITH CHECK ADD  CONSTRAINT [FK_BookRoomDetails_BookRoom] FOREIGN KEY([IDBookRoom])
REFERENCES [dbo].[BookRoom] ([ID])
GO
ALTER TABLE [dbo].[BookRoomDetails] CHECK CONSTRAINT [FK_BookRoomDetails_BookRoom]
GO
ALTER TABLE [dbo].[BookRoomDetails]  WITH CHECK ADD  CONSTRAINT [FK_BookRoomDetails_Customer] FOREIGN KEY([IDCustomerOther])
REFERENCES [dbo].[Customer] ([ID])
GO
ALTER TABLE [dbo].[BookRoomDetails] CHECK CONSTRAINT [FK_BookRoomDetails_Customer]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_RoomType] FOREIGN KEY([idRoomType])
REFERENCES [dbo].[RoomType] ([ID])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_RoomType]
GO
ALTER TABLE [dbo].[Room]  WITH CHECK ADD  CONSTRAINT [FK_Room_RoomType] FOREIGN KEY([IDRoomType])
REFERENCES [dbo].[RoomType] ([ID])
GO
ALTER TABLE [dbo].[Room] CHECK CONSTRAINT [FK_Room_RoomType]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_ServiceType] FOREIGN KEY([IDServiceType])
REFERENCES [dbo].[ServiceType] ([ID])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_ServiceType]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_StaffType] FOREIGN KEY([IDStaffType])
REFERENCES [dbo].[StaffType] ([ID])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_StaffType]
GO
/****** Object:  StoredProcedure [dbo].[GetIDReceiveRoomCurrent]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[GetIDReceiveRoomCurrent]
as
begin
	select MAX(id)
	from ReceiveRoom
end
GO
/****** Object:  StoredProcedure [dbo].[InsertReceiveRoomDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[InsertReceiveRoomDetails]
@idReceiveRoom int,@idCustomer int
as
begin
	insert into BookRoomDetails(IDBookRoom,IDCustomerOther)
	values(@idReceiveRoom,@idCustomer)
end
GO
/****** Object:  StoredProcedure [dbo].[IsIDCustomerBillExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[IsIDCustomerBillExists] 
@id int
AS
BEGIN
select *
    FROM BILL a, BookRoom c where c.IDCustomer=@id and 
	 a.IDBookRoom=c.ID
END
GO
/****** Object:  StoredProcedure [dbo].[IsIDCustomerOtherBillExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[IsIDCustomerOtherBillExists] 
@id int
AS
BEGIN
select *
    FROM BILL a, BookRoomDetails b, BookRoom c where b.IDCustomerOther=@id and 
	c.ID=b.IDBookRoom and a.IDBookRoom=c.ID
END
GO
/****** Object:  StoredProcedure [dbo].[USP_ChekcAccess]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_ChekcAccess] 
@username int, @formname NVARCHAR(100)
AS
BEGIN
	SELECT UserName FROM dbo.Staff INNER JOIN dbo.StaffType ON StaffType.ID = Staff.IDStaffType 
	INNER JOIN Access ON Access.Idstafftype = stafftype.ID INNER JOIN Job ON Job.id = Access.idJob
	WHERE Staff.ID = @username AND @formname LIKE NameForm
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteAccess]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_DeleteAccess]
@idJob INT, @idStaffType int
AS
BEGIN
	DELETE Access WHERE @idJob = idJob AND @idStaffType = idStaffType
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteAccount]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROCEDURE  [dbo].[USP_DeleteAccount]
@username nvarchar(100)
AS
begin

	delete from Staff where @username=UserName
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_DeleteBookRoom]
@id int
as
begin
	delete from BookRoom
	where ID=@id
	delete from BookRoomDetails 
	where IDBookRoom=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteCustomer]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_DeleteCustomer] 
@id nvarchar(50)
AS
BEGIN
delete
    FROM Customer where ID=@id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteReceiveRoomDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_DeleteReceiveRoomDetails]
@idReceiveRoom int,@idCustomer int
as
begin
	delete from BookRoomDetails
	where IDCustomerOther=@idCustomer and IDBookRoom=@idReceiveRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_DeleteRoom]
@id int
as
begin
	
	delete from Room
	where ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[USP_DeleteService]
@id int
AS
begin

	delete from Service where @id= ID
	
END
/****** Object:  StoredProcedure [dbo].[USP_DeleteAccount]    Script Date: 6/25/2018 2:42:22 AM ******/
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteServiceBillDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE PROCEDURE [dbo].[USP_DeleteServiceBillDetails]
@idBill int,@idService int,@count int

AS
declare @count_ int=0
BEGIN
	
	select @count_=B.Count from BillDetails B where B.IDBill=@idBill  and B.IDService=@idService
	if(@count=@count_)
	begin
	delete B
	from BillDetails B
	where  B.IDBill=@idBill  and B.IDService=@idService
	end
	if(@count<@count_)
	begin
	update BillDetails  set BillDetails.Count=(@count_-@count) where IDBill=@idBill  and IDService=@idService
	end
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteServiceByIdServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_DeleteServiceByIdServiceType]
@id INT
AS
BEGIN
	delete 
	Service 
	WHERE IDServiceType=@id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE PROCEDURE  [dbo].[USP_DeleteServiceType]
@id int
AS
begin
	
	delete from ServiceType where @id= ID
	delete from Service where @id= IDServiceType
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_DeleteStaffType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_DeleteStaffType]
@id int
AS
begin
	DECLARE @count int = 0
	SELECT @count = COUNT(*) FROM staff WHERE @id = staff.IDStaffType
	IF(@count = 0)
	begin
		delete Access where idstafftype = @id
		DELETE staffType WHERE @id = id
	end
END
GO
/****** Object:  StoredProcedure [dbo].[USP_FindIDService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
create proc [dbo].[USP_FindIDService]
@name nvarchar(200)
as
begin
select ID
from  Service 
where @name=Name 
end
GO
/****** Object:  StoredProcedure [dbo].[USP_FindService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE proc [dbo].[USP_FindService]
@name nvarchar(100)
as
begin
select t.Name
from ServiceType t, Service s
where @name=s.Name and t.ID=s.IDServiceType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCustomerTypeNameByIdCard]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetCustomerTypeNameByIdCard]
@idCard nvarchar(100)
as
begin
	select B.Name
	from Customer A, CustomerType B
	where A.IDCustomerType=B.ID and A.IDCard=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIdAccByUserName]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[USP_GetIdAccByUserName]
@idCard nvarchar(100)
as
begin
	select ID
	from  Staff 
	where  UserName=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIdBillFromIdRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetIdBillFromIdRoom]
@idRoom int
as
begin
	select B.ID
	from BookRoom A,Bill B
	where A.ID=B.IDBookRoom and B.StatusBill=N'Chưa thanh toán' and A.IDRoom=@idRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIdBillMax]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_GetIdBillMax]
as
select MAX(id)
from Bill
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIDCustomerFromBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetIDCustomerFromBookRoom]
@idReceiveRoom int
as
begin
	select B.IDCustomer
	from BookRoom B
	where B.ID=@idReceiveRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIdReceiRoomFromIdRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE   proc [dbo].[USP_GetIdReceiRoomFromIdRoom]--IdRoom đưa vào có trạng thái "Có người"
@idRoom int
as
begin
select *
from BookRoom
where IDRoom=@idRoom
order by ID desc
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetIDRoomFromReceiveRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_GetIDRoomFromReceiveRoom]
@idReceiveRoom int
as
begin
	select IDRoom
	from ReceiveRoom
	where ID=@idReceiveRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetInfoCustomerById]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

create PROCEDURE  [dbo].[USP_GetInfoCustomerById]
@id int
AS
begin

	select * from Customer where ID=@id
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_GetMaxPersonByRoomType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_GetMaxPersonByRoomType]
@idRoomType int
as
begin
	if(@idRoomType=1)
	select *
	from Parameter
	where Name=N'QĐ1.1'

	if(@idRoomType=2)
	select *
	from Parameter
	where Name=N'QĐ1.2'

	if(@idRoomType=3)
	select *
	from Parameter
	where Name=N'QĐ1.3'

	if(@idRoomType=4)
	select *
	from Parameter
	where Name=N'QĐ1.4'
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetNameStaffTypeByUserName]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetNameStaffTypeByUserName]
@username int
as
begin
	select B.*
	from Staff A, StaffType B
	where a.IDStaffType=B.ID and A.ID=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetPeoples]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetPeoples]
@idBill int
as
begin
	select COUNT(B.IDBookRoom)
	from BookRoom A,BookRoomDetails B,Bill C
	where A.ID=C.IDBookRoom and A.ID=B.IDBookRoom and C.ID=@idBill
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetRoomTypeByIdRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_GetRoomTypeByIdRoom]
@idRoom int
as
begin
	select B.*
	from Room A,RoomType B
	where A.IDRoomType=B.ID and A.ID=@idRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_GetStaffSetUp]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_GetStaffSetUp]
@idBill int
as
begin
	select B.*
	from Bill A, Staff B
	where A.ID=@idBill and A.IDStaff=B.ID
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IDLogin]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IDLogin]
@userName nvarchar(100),@passWord nvarchar(100)
as
Select * from Staff where UserName=@userName and PassWord=@passWord
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertAccess]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InsertAccess]
@idJob INT, @idStaffType int
AS
BEGIN
	INSERT INTO Access(idJob, idstafftype) VALUES(@idJob, @idStaffType)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_InsertBill]
@idRoom int,@staffSetUp int
as
begin
	insert into Bill(IDBookRoom,IDStaff,StatusBill)
	values(@idRoom,@staffSetUp,N'Chưa thanh toán')
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertBillDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_InsertBillDetails]
@idBill int,@idService int,@count int
as
begin
		declare @totalPrice int,@price int
		select @price=Price
		from Service
		where ID=@idService
		set @totalPrice=@price*@count
		insert into BillDetails(IDBill,IDService,Count,TotalPrice)
		values(@idBill,@idService,@count,@totalPrice)
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_InsertBookRoom]
@idCustomer int,@idRoom int,@datecheckin date,@datecheckout date,@datebookroom smalldatetime,@manv int
as
begin
	insert into BookRoom (IDCustomer,IDRoom,DateCheckIn,DateCheckOut,DateBookRoom,IDStaff)
	values(@idCustomer,@idRoom,@datecheckin,@datecheckout,@datebookroom,@manv)
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertCustomer]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InsertCustomer]
@customerName NVARCHAR(100), @idCard NVARCHAR(100),
@address NVARCHAR(200), @dateOfBirth date, @phoneNumber VARCHAR(100),
@sex NVARCHAR(100), @nationality NVARCHAR(100)
AS
BEGIN

INSERT INTO dbo.Customer(IDCard, Name, DateOfBirth, Address, PhoneNumber, Sex, Nationality)
	VALUES(@idCard, @customerName, @dateOfBirth, @address, @phoneNumber, @sex, @nationality)
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertCustomer_]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_InsertCustomer_]
@idCard nvarchar(100),@name nvarchar(100), @dateOfBirth Date,@address nvarchar(200),@phoneNumber varchar(100),@sex nvarchar(100),@nationality nvarchar(100)
as
begin
	insert into Customer(IDCard,Name,DateOfBirth,Address,PhoneNumber,Sex,Nationality)
	values(@idCard,@name,@dateOfBirth,@address,@phoneNumber,@sex,@nationality)
end
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertReport]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[USP_InsertReport]
@idBill int
AS
BEGIN
	DECLARE @month INT = 0
	DECLARE @year INT = 0
	DECLARE @id INT = 0
	DECLARE @price INT = 0
	SELECT @id = dbo.ROOM.IDRoomType, @month = MONTH(bill.DateOfCreate), @year = YEAR(bill.DateOfCreate), @price = bill.TotalPrice
	FROM bill INNER JOIN dbo.BookRoom ON BookRoom.ID = bill.IDBookRoom
		INNER JOIN dbo.ROOM ON ROOM.ID = BookRoom.IDRoom
	WHERE bill.ID = @idBill

	DECLARE @count INT = 0	
	SELECT @count = COUNT(*) FROM Report WHERE month = @month AND year = @year and idRoomType = @id
	IF(@count=0) -- khong ton tai roomtype
    BEGIN
		
		INSERT INTO Report(idRoomType, Month, Year) SELECT roomtype.ID, @month, @year FROM roomtype 
	END
    UPDATE dbo.Report SET value = value + @price WHERE Year = @year AND Month = @month AND idRoomType = @id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------

--Status Room
--------------------------------------------------------------

CREATE PROC [dbo].[USP_InsertRoom]
@nameRoom NVARCHAR(100), @idRoomType INT, @StatusRoom NVARCHAR(100)
AS
INSERT INTO dbo.Room(Name, IDRoomType, StatusRoom)
VALUES(@nameRoom, @idRoomType, @StatusRoom)
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE PROC [dbo].[USP_InsertService]
@name NVARCHAR(200), @idServiceType INT, @price int
AS
BEGIN
	INSERT INTO dbo.Service(Name,IDServiceType,Price)
	VALUES(@name, @idServiceType, @price)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InsertServiceType]
@name NVARCHAR(100)
AS
BEGIN
	INSERT INTO dbo.ServiceType(name)
	VALUES(@name)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertStaff]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROC [dbo].[USP_InsertStaff]
@user NVARCHAR(100), @name NVARCHAR(100), @pass NVARCHAR(100),
@idStaffType INT,@idCard NVARCHAR(100), @dateOfBirth DATE, @sex NVARCHAR(100),
@address NVARCHAR(200), @phoneNumber varchar(100), @startDay date
AS
BEGIN
	
	INSERT INTO dbo.Staff(UserName, Name, PassWord, IDStaffType, IDCard, DateOfBirth, Sex, Address, PhoneNumber, StartDay)
	VALUES (@user, @name, @pass, @idStaffType,@idCard, @dateOfBirth, @sex, @address, @phoneNumber, @startDay)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_InsertStaffType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_InsertStaffType] 
@name NVARCHAR(100)
AS
BEGIN
    INSERT INTO staffType(Name) VALUES(@name)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_IsAccExistByIdCard]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsAccExistByIdCard]
@idCard nvarchar(100)
as
begin
select *
from Staff
where IDCard=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsAccExistByUserName]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsAccExistByUserName]
@idCard nvarchar(100)
as
begin
select *
from Staff
where UserName=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsBillExistsAcc]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE  [dbo].[USP_IsBillExistsAcc]
@idCard int
as
begin
	select *
	from bill a, Staff b
	where a.IDStaff=b.ID and b.ID=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsBookRoomExistsAcc]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE  [dbo].[USP_IsBookRoomExistsAcc]
@id int
as
begin
	select *
	from BookRoom a, Staff b
	where a.IDCustomer=b.ID and b.ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsBookRoomHaveBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsBookRoomHaveBill]--Trả về count > 0: tức là đã tồn tại Bill
@id int
as
begin
	select *
	from Bill A,BookRoom B
	where  A.IDBookRoom=B.ID and B.ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsCusBookRoomDetailExistsByIdBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsCusBookRoomDetailExistsByIdBookRoom]
@idBookRoom int
as
begin
	select B.IDCustomerOther
	from BookRoom A, BookRoomDetails B
	where A.ID=@idBookRoom and B.IDBookRoom=A.ID
	
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsCusBookRoomExistsByIdBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsCusBookRoomExistsByIdBookRoom]
@idBookRoom int
as
begin
	select B.IDCard
	from BookRoom A, Customer B
	where A.ID=@idBookRoom and B.ID=A.IDCustomer
	
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsCusExistsByIdCard]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsCusExistsByIdCard]
@idCard nvarchar(100)
as
begin
select *
from Customer 
where IDCard=@idCard 
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsCustomerBookRoomDetailExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_IsCustomerBookRoomDetailExists] 
@idCard nvarchar(50)
AS
BEGIN
select *
    FROM BookRoomDetails a, BookRoom c where a.IDCustomerOther=@idCard  and 
	 a.IDBookRoom=c.ID
END
GO
/****** Object:  StoredProcedure [dbo].[USP_IsCustomerBookRoomExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_IsCustomerBookRoomExists] 
@idCard nvarchar(50)
AS
BEGIN
select *
    FROM BookRoom  where IDCustomer=@idCard
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_IsExistBillDetailsOfRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Proc [dbo].[USP_IsExistBillDetailsOfRoom]--Kq > 0 :TH3, ngược lại TH2. Tuy nhiên, trước khi kt đk này phải chắc chắn tồn tại Bill
@idRoom int,@idservice int
as
begin
	select *
	from Bill A,BillDetails B,BookRoom C
	where A.StatusBill=N'Chưa thanh toán' and A.ID=B.IDBill and C.ID=A.IDBookRoom and C.IDRoom=@idRoom and B.IDService=@idservice
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsExistBillOfRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsExistBillOfRoom]--Trả về count > 0: tức là đã tồn tại Bill
@idRoom int
as
begin
	select *
	from Bill A,BookRoom B
	where A.StatusBill=N'Chưa thanh toán' and A.IDBookRoom=B.ID and B.IDRoom=@idRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsExistBillPay]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsExistBillPay]--Trả về count > 0: tức là đã tồn tại Bill
@id int
as
begin
	select *
	from Bill A,BookRoom B
	where  A.IDBookRoom=B.ID and B.ID=@id and A.StatusBill=N'Đã thanh toán'
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsIdCardExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsIdCardExists]
@idCard nvarchar(100)
as
begin
select *
from Customer a, staff b
where a.IDCard=@idCard or b.IDCard=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsIdCardExistsAcc]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[USP_IsIdCardExistsAcc]
@idCard nvarchar(100)
as
begin
	select *
	from Staff
	where IDCard=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsIDServiceExist]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE proc [dbo].[USP_IsIDServiceExist]
@id nvarchar(50)
as
begin
	select * from BillDetails A, Service B
	where B.ID = @id and A.IDService=B.ID
END
GO
/****** Object:  StoredProcedure [dbo].[USP_IsPhoneAccExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsPhoneAccExists]
@idCard nvarchar(100)
as
begin
select *
from  staff 
where PhoneNumber=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsPhoneCusExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsPhoneCusExists]
@idCard nvarchar(100)
as
begin
select *
from Customer  
where PhoneNumber=@idCard 
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsPhoneExists]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_IsPhoneExists]
@idCard nvarchar(100)
as
begin
select *
from Customer a, staff b
where a.PhoneNumber=@idCard or b.PhoneNumber=@idCard
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsRoomHaveBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsRoomHaveBill]--Trả về count > 0: tức là đã tồn tại Bill
@id int
as
begin
	select *
	from Bill A,BookRoom B, Room C
	where  A.IDBookRoom=B.ID and B.IDRoom=C.ID and C.ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_IsRoomHaveBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_IsRoomHaveBookRoom]--Trả về count > 0: tức là đã tồn tại Bill
@id int
as
begin
	select *
	from BookRoom B, Room C
	where B.IDRoom=C.ID and C.ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadBookRoomsByDate]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_LoadBookRoomsByDate]
@date Date
as
begin
	select A.ID[Mã đặt phòng], b.Name[Họ và tên],b.IDCard[CMND],D.Name[Tên phòng],C.Name[Loại phòng],A.DateCheckIn[Ngày nhận],A.DateCheckOut[Ngày trả]
	from BookRoom A,Customer B, RoomType C,Room D
	where a.IDRoom=D.ID and A.IDCustomer=B.ID and A.DateBookRoom>=@date and D.IDRoomType=C.ID
	order by A.DateBookRoom desc
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadEmptyRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[InsertReceiveRoomDetails]    Script Date: 11/21/2023 4:26:23 AM ******/
CREATE proc [dbo].[USP_LoadEmptyRoom]
@idRoomType int
as
begin
	select ID,Name,idRoomType,StatusRoom as [nameStatusRoom]
	from Room
	where StatusRoom=N'Trống' and IDRoomType=@idRoomType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullAccessNow]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullAccessNow]
@idStaffType INT
AS
BEGIN
	SELECT Job.Name, Job.ID FROM Job INNER JOIN Access ON Job.Id = Access.IDJob
	WHERE @idStaffType = dbo.Access.IDStaffType
END
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullAccessRest]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_LoadFullAccessRest]
@idStaffType INT
AS
BEGIN
	SELECT j.Name, j.Id FROM Job j
	WHERE NOT EXISTS 
	(
		SELECT * FROM Job INNER JOIN Access ON Job.Id = Access.IdJob
		WHERE j.Id = Job.Id AND Access.idStaffType = @idStaffType
	)
END
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFUllBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFUllBill] 
AS
BEGIN
	SELECT bill.id, room.Name AS [roomName], Customer.Name as [customerName], Staff.Name, DateOfCreate, Bill.StatusBill, TotalPrice, (cast(Discount as nvarchar(4)) + '%') [Discount], cast(TotalPrice*( (100-Discount)/100.0) as int) [FinalPrice]
    FROM dbo.BILL INNER JOIN Staff on Staff.ID=Bill.IDStaff
					inner join BookRoom on BookRoom.id = Bill.IDBookRoom
					INNER JOIN dbo.ROOM ON ROOM.ID = BookRoom.IDRoom
					inner join Customer on Customer.ID = BookRoom.IDCustomer
	ORDER BY DateOfCreate DESC
END
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullCustomer]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullCustomer]
AS
SELECT *
FROM dbo.Customer 
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullCustomerType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROC [dbo].[USP_LoadFullCustomerType]
AS
SELECT * FROM dbo.CustomerType
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullParameter]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullParameter]
AS
SELECT * FROM dbo.PARAMETER
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullReport]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullReport]
@month INT, @year int
AS
BEGIN
	SELECT name, value, rate FROM dbo.Report INNER JOIN dbo.ROOMTYPE ON ROOMTYPE.ID = Report.idRoomType
	WHERE Month = @month AND Year = @year
END
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullRoom]
AS
SELECT Room.ID, Room.Name,RoomType.Name AS [nameRoomType], Price, LimitPerson,
StatusRoom  AS [nameStatusRoom], IDRoomType
FROM dbo.Room INNER JOIN dbo.RoomType 
ON roomtype.id = room.IDRoomType
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullRoomType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullRoomType]
AS
SELECT * FROM dbo.RoomType
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullService]
AS
SELECT Service.ID, Service.Name, Price, ServiceType.Name AS [nameServiceType], IDServiceType
FROM dbo.Service INNER JOIN dbo.ServiceType ON ServiceType.ID = Service.IDServiceType
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_LoadFullServiceType]
AS
SELECT * FROM ServiceType
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullStaff]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROC [dbo].[USP_LoadFullStaff]
AS
BEGIN
	SELECT Staff.ID,UserName, Staff.Name, StaffType.Name as[Loai], IDCard,
			DateOfBirth, Sex, PhoneNumber, StartDay, Address, IDStaffType
    FROM dbo.Staff INNER JOIN dbo.StaffType ON StaffType.ID = Staff.IDStaffType
END
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullStaffType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------------------------
--Staff type
--------------------------------------------------------------

CREATE PROC [dbo].[USP_LoadFullStaffType]
AS
begin
SELECT * FROM dbo.StaffType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadFullStatusRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------

--Status Room
--------------------------------------------------------------
CREATE PROC [dbo].[USP_LoadFullStatusRoom]
AS
SELECT distinct StatusRoom FROM dbo.Room
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadListFullRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_LoadListFullRoom]
@date Date
as
begin
	select distinct  A.ID, A.Name,A.IDRoomType, A.StatusRoom AS [nameStatusRoom]
	from Room A, BookRoom C
	where A.StatusRoom=N'Có người'  and C.IDRoom=A.ID and (C.DateCheckIn<=@date or C.DateCheckOut>=@date)
	order by A.ID asc
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadReceiveRoomsByDate]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_LoadReceiveRoomsByDate]
@date Date
as
begin
	select A.ID[Mã nhận phòng], b.Name[Họ và tên],b.IDCard[CMND],C.Name[Tên phòng],D.DateCheckIn[Ngày nhận],D.DateCheckOut[Ngày trả]
	from ReceiveRoom A,Customer B, Room C,BookRoom D
	where A.IDBookRoom=D.ID and D.IDCustomer=B.ID and A.IDRoom=C.ID and D.DateCheckIn>=@date
	order by A.ID desc
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadRoomTypeInfo]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_LoadRoomTypeInfo]
@id int
as
begin
	select b.* from RoomType a, Room b 
	where a.ID=@id and a.ID=b.IDRoomType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadServiceByServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
create proc [dbo].[USP_LoadServiceByServiceType]
@idServiceType int
as
begin
	select *
	from Service
	where IDServiceType=@idServiceType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadStaffInforByUserName]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_LoadStaffInforByUserName]
@username nvarchar(100)
as
begin
	select *
	from Staff
	where UserName=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_LoadTotalBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_LoadTotalBill] 
AS
BEGIN
	SELECT bill.id, (Bill.Surcharge+Bill.RoomPrice) [Total]
    FROM dbo.BILL INNER JOIN Staff on Staff.ID=Bill.IDStaff
					inner join BookRoom on BookRoom.id = Bill.IDBookRoom
					INNER JOIN dbo.ROOM ON ROOM.ID = BookRoom.IDRoom
					inner join Customer on Customer.ID = BookRoom.IDCustomer
	ORDER BY DateOfCreate DESC
END
GO
/****** Object:  StoredProcedure [dbo].[USP_Login]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_Login]
@userName nvarchar(100),@passWord nvarchar(100)
as
Select * from Staff where UserName=@userName and PassWord=@passWord
GO
/****** Object:  StoredProcedure [dbo].[USP_RoomTypeInfo]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_RoomTypeInfo]
@id int
as
begin
select * 
from RoomType
where ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_SearchBill]
@string NVARCHAR(100), @mode int
AS
BEGIN
	SELECT @string = '%' + [dbo].[ConvertString](@string) + '%'
	
	DECLARE @table TABLE(id int)
	IF(@mode = 0)
		INSERT INTO @table SELECT bill.id FROM bill WHERE CAST(id AS NVARCHAR(100)) LIKE @string
	ELSE IF(@mode = 1)
		INSERT INTO @table SELECT bill.id  FROM bill INNER JOIN dbo.BookRoom ON BookRoom.ID = Bill.IDBookRoom
		 INNER JOIN dbo.Customer ON Customer.ID = BookRoom.IDCustomer 
		WHERE [dbo].ConvertString(Customer.Name) LIKE @string
	ELSE IF(@mode = 2)
INSERT INTO @table SELECT bill.id  FROM bill INNER JOIN dbo.BookRoom ON BookRoom.ID = Bill.IDBookRoom INNER JOIN dbo.Customer ON Customer.ID = BookRoom.IDCustomer
		WHERE [dbo].ConvertString(Customer.IDCard) LIKE @string
	ELSE IF(@mode = 3)
		INSERT INTO @table SELECT bill.id  FROM bill INNER JOIN dbo.BookRoom ON BookRoom.ID = Bill.IDBookRoom INNER JOIN dbo.Customer ON Customer.ID = BookRoom.IDCustomer
		WHERE CAST(dbo.Customer.PhoneNumber AS NVARCHAR(100)) LIKE @string

	SELECT bill.id, room.Name AS [roomName], Customer.Name as [customerName], Staff.Name, bill.DateOfCreate, Bill.StatusBill, bill.TotalPrice, (cast(bill.Discount as nvarchar(4)) + '%') [Discount], cast(bill.TotalPrice*( (100-bill.Discount)/100.0) as int) [FinalPrice]
    FROM dbo.BILL INNER JOIN dbo.BookRoom ON BookRoom.ID = Bill.IDBookRoom
	INNER JOIN dbo.Staff ON Staff.ID=Bill.IDStaff
	INNER JOIN dbo.ROOM ON ROOM.ID = BookRoom.IDRoom
	INNER JOIN @table ON bill.id = [@table].id
	inner join Customer on Customer.ID = BookRoom.IDCustomer
	ORDER BY DateOfCreate DESC
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchCustomer]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
	CREATE PROC [dbo].[USP_SearchCustomer]
	@string NVARCHAR(100), @mode INT
	AS
	BEGIN
		SELECT @string = '%' + [dbo].[ConvertString](@string) + '%'
		DECLARE @table TABLE(id INT)

		IF(@mode = 0)
			INSERT INTO @table SELECT id FROM [dbo].customer WHERE CAST(id AS NVARCHAR(100)) LIKE @string;
		ELSE IF(@mode = 1)
			INSERT INTO @table SELECT id FROM [dbo].customer WHERE [dbo].[ConvertString](name) LIKE @string;
		ELSE IF(@mode = 2)
			INSERT INTO @table SELECT id FROM [dbo].customer WHERE [dbo].[ConvertString](IDCard) LIKE @string;
		ELSE IF(@mode = 3)
			INSERT INTO @table SELECT id FROM [dbo].customer WHERE CAST(PhoneNumber AS NVARCHAR(100)) LIKE @string;

	    SELECT CUSTOMER.ID, Customer.Name, IDCard, Sex, DateOfBirth, PhoneNumber, Address, Nationality
		FROM Customer INNER JOIN @table ON [@table].id = CUSTOMER.ID 
		end
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchParameter]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
CREATE PROC [dbo].[USP_SearchParameter]
@string NVARCHAR(200)
AS
BEGIN
	SELECT @string = '%' + [dbo].[convertstring](@string) + '%'
	SELECT * FROM dbo.PARAMETER
	WHERE [dbo].[convertstring](name) like @string
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------

--Room
--------------------------------------------------------------
CREATE PROC [dbo].[USP_SearchRoom]
@string NVARCHAR(100), @int INT
AS
BEGIN
	SELECT @string = '%' + [dbo].[convertString](@string) + '%'
	SELECT Room.ID, Room.Name,RoomType.Name AS [nameRoomType], Price, LimitPerson,
	Room.StatusRoom AS [nameStatusRoom], IDRoomType
	FROM dbo.Room INNER JOIN dbo.RoomType ON roomtype.id = room.IDRoomType 
	WHERE dbo.ConvertString(dbo.Room.name) LIKE @string OR dbo.Room.id = @int
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchRoomType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------

--Room Type
--------------------------------------------------------------
CREATE PROC [dbo].[USP_SearchRoomType]
@string NVARCHAR(100), @int INT
AS
BEGIN
	SELECT @string = '%' + [dbo].[convertstring](@string) + '%'
	SELECT * FROM dbo.ROOMTYPE
	WHERE [dbo].[convertstring](name) LIKE @string OR id = @int
end
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_SearchService]
@string NVARCHAR(100), @int int
AS
BEGIN
		DECLARE @table TABLE
		(
			id INT
		)
		SELECT @string = '%' + [dbo].[ConvertString](@string) + '%'
		INSERT INTO @table
			SELECT id FROM dbo.SERVICE WHERE [dbo].[ConvertString](name) like @string OR id = @int
		SELECT Service.ID, Service.Name, Price, ServiceType.Name AS [nameServiceType], IDServiceType
		FROM @table INNER JOIN dbo.SERVICE ON SERVICE.ID = [@table].id INNER JOIN dbo.ServiceType ON ServiceType.ID = Service.IDServiceType
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
---------------------------

--------------------------------------------------------------

--Service Type
--------------------------------------------------------------
CREATE PROC [dbo].[USP_SearchServiceType]
@string NVARCHAR(100), @int INT
AS
BEGIN
	DECLARE @table table( id int)
	SELECT @string ='%' + [dbo].[ConvertString](@string) + '%'
	INSERT INTO @table SELECT id FROM ServiceType WHERE [dbo].[ConvertString](name) LIKE @string OR id = @int
	SELECT dbo.SERVICETYPE.ID, Name FROM @table INNER JOIN servicetype ON  SERVICETYPE.ID = [@table].id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_SearchStaff]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_SearchStaff]
@string NVARCHAR(100), @int int
AS
BEGIN
	SELECT @string = '%' + [dbo].[ConvertString](@string) + '%'
	DECLARE @table TABLE( username NVARCHAR(100))
	IF(@int < 1)
	begin
		INSERT INTO @table SELECT username FROM staff 
		WHERE username LIKE @string OR [dbo].[ConvertString](Name) LIKE @string
		OR  idcard LIKE @string
	END
	ELSE
    BEGIN
		INSERT INTO @table SELECT username FROM staff 
		WHERE username LIKE @string OR [dbo].[ConvertString](Name) LIKE @string
		OR  idcard LIKE @string OR cast(PhoneNumber AS NVARCHAR(100)) LIKE @string
	END
	SELECT Staff.UserName, Staff.Name, StaffType.Name[Loai], IDCard, DateOfBirth, Sex, PhoneNumber, StartDay, Address, IDStaffType
    FROM dbo.Staff INNER JOIN  @table ON [@table].username = STAFF.UserName INNER JOIN dbo.StaffType ON StaffType.ID = Staff.IDStaffType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowBill]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowBill]
@idRoom int
as
begin
	
	

	select D.Name [Tên dịch vụ],D.Price[Đơn giá],B.Count[Số lượng],B.TotalPrice[Thành tiền]
	from Bill A, BillDetails B, BookRoom C, Service D
	where A.StatusBill=N'Chưa thanh toán' and A.ID=B.IDBill and A.IDBookRoom=C.ID and C.IDRoom=@idRoom and B.IDService=D.ID

end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowBillInfo]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowBillInfo]
@idBill int
as
begin
select D.Name[HoTen],D.IDCard[CMND],D.PhoneNumber[SDT],D.Address[DiaChi],D.Nationality[QuocTich],F.Name[TenPhong],G.Name[LoaiPhong],G.Price[DonGia],C.DateCheckIn[NgayDen],C.DateCheckOut[NgayDi],A.RoomPrice[TienPhong],A.ServicePrice[TienDichVu],(A.Surcharge)[PhuThu],A.TotalPrice[ThanhTien],A.Discount[GiamGia]
from Bill A, BookRoom C, Customer D,Room F,RoomType G
where   C.IDCustomer=D.ID and F.IDRoomType=G.ID and A.ID=@idBill and A.IDBookRoom=C.ID and C.IDRoom=F.ID
end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowBillPreView]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowBillPreView]
@idBill int
as
begin
	select D.Name [Tên dịch vụ],D.Price[Đơn giá],B.Count[Số lượng],B.TotalPrice[Thành tiền]
	from Bill A, BillDetails B, Service D
	where A.StatusBill=N'Đã thanh toán' and A.ID=b.IDBill and A.ID=@idBill and B.IDService=D.ID
end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowBillRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowBillRoom]--Muốn proc thực thi được thì phải thực thi USP_UpdateBill trước(nếu có thể)
@idRoom int
as
begin

	select A.Name [Tên phòng],D.Price[Đơn giá] ,B.DateCheckIn [Ngày nhận],B.DateCheckOut[Ngày trả] ,E.RoomPrice[Tiền phòng],E.Surcharge[Phụ thu]
	from Room A,BookRoom B,RoomType D,Bill E
	where E.IDBookRoom=B.ID and StatusRoom=N'Có người' and A.ID=B.IDRoom and A.IDRoomType=D.ID  and B.IDRoom=@idRoom and E.StatusBill=N'Chưa thanh toán'
end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowCustomerFromReceiveRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowCustomerFromReceiveRoom]
@idReceiveRoom int
as
begin
	select C.Name[Tên khách hàng],C.IDCard[CMND],C.Address[Địa chỉ],C.PhoneNumber[Số điện thoại],C.Nationality[Quốc tịch]
	from  BookRoom B, Customer C
	where B.ID=@idReceiveRoom  and B.IDCustomer=C.ID
	union
	select C.Name[Tên khách hàng],C.IDCard[CMND],C.Address[Địa chỉ],C.PhoneNumber[Số điện thoại],C.Nationality[Quốc tịch]
	from BookRoom  A,BookRoomDetails B,Customer C
	where A.ID=@idReceiveRoom and A.ID=B.IDBookRoom and B.IDCustomerOther=C.ID
end
GO
/****** Object:  StoredProcedure [dbo].[USP_ShowReceiveRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_ShowReceiveRoom]
@idReceiveRoom int
as
begin
	select B.ID[Mã đặt phòng], C.Name[Tên phòng],D.Name[Loại],B.DateCheckIn[Ngày nhận],B.DateCheckOut[Ngày trả]
	from BookRoom B,Room C, RoomType D
	where  B.IDRoom=C.ID and B.ID=@idReceiveRoom and D.ID=C.IDRoomType
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateAccount3]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_UpdateAccount3]
@username nvarchar(100),@address nvarchar(100),@phonenumber int
as
begin
	update Staff
	set Address=@address,PhoneNumber=@phonenumber
	where UserName=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateBill_Other]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_UpdateBill_Other]
@idBill int,@discount int
as
begin
	declare @totalPrice int=0,@idRoom int
	select @totalPrice=RoomPrice+ServicePrice+ Surcharge
	from Bill
	where ID=@idBill

	update Bill
	set DateOfCreate=GETDATE(), TotalPrice=@totalPrice,Discount=@discount,StatusBill=N'Đã thanh toán'
	where ID=@idBill

	select @idRoom=B.IDRoom
	from Bill A, BookRoom B
	where A.IDBookRoom=B.ID

	update Room
	set StatusRoom=N'Trống'
	where ID=@idRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateBill_RoomPrice]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_UpdateBill_RoomPrice]
@idBill int
as
begin
	declare @idBookRoom int,@roomPrice int =0,@price int,@days int,@countCustomer int,@limitPerson int,@totalService int,@surcharge int =0,
	@RoomType int=0

	select @days=DATEDIFF(day,B.DateCheckIn,B.DateCheckOut),@price=D.Price ,@limitPerson=D.LimitPerson,@idBookRoom=A.IDBookRoom,
	@RoomType=D.ID
	from Bill A,BookRoom B,RoomType D,Room E
	where A.ID=@idBill and A.IDBookRoom=B.ID and B.IDRoom=E.ID and E.IDRoomType=D.ID 

	select @countCustomer=COUNT(A.IDCustomer)
	from BookRoom A , BookRoomDetails B
	where A.ID=@idBookRoom  and B.IDBookRoom=A.ID
	set @countCustomer+=1;
	set @roomPrice=@price*@days;

	declare @QD2 float = 0 -- thue gioi han so luong
	select @QD2 = value from Parameter where Name = N'QĐ3'

	if((@countCustomer-@limitPerson)>0)
	set @surcharge+=@roomPrice*@QD2*(@countCustomer-@limitPerson) 

	declare @QD3 float = 0 -- thue phong
	if(@RoomType=1)
	begin
	select @QD3 = value from Parameter where Name = N'QĐ2.1'
	end
	if(@RoomType=2)
	begin
	select @QD3 = value from Parameter where Name = N'QĐ2.2'
	end
	if(@RoomType=3)
	begin
	select @QD3 = value from Parameter where Name = N'QĐ2.3'
	end
	if(@RoomType=4)
	begin
	select @QD3 = value from Parameter where Name = N'QĐ2.4'
	end
	set @surcharge+=@roomPrice*@QD3

	update Bill
	set RoomPrice=@roomPrice, Surcharge=@surcharge
	where id=@idBill
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateBill_ServicePrice]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_UpdateBill_ServicePrice]
@idBill int
as
begin
	declare @totalServicePrice int=0
	select @totalServicePrice=SUM(TotalPrice)
	from BillDetails
	where IDBill=@idBill
	if(@totalServicePrice is null)
	set @totalServicePrice=0
	update Bill 
	set ServicePrice=@totalServicePrice
	where ID=@idBill
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateBillDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[USP_UpdateBillDetails]
@idBill int,@idService int,@_count int
as
begin
	declare @totalPrice int,@price int,@count int

	select @price=Price
	from Service
	where ID=@idService

	select @count=Count
	from Bill A,BillDetails B
	where A.ID=B.IDBill and A.ID=@idBill and A.StatusBill=N'Chưa thanh toán' and B.IDService=@idService

	set @count=@count+@_count
	if(@count>0)
	begin
		set @totalPrice=@count*@price
		update BillDetails
		set Count=@count,TotalPrice=@totalPrice
		where IDBill=@idBill and IDService=@idService
	end
	else
	begin
		delete from BillDetails
		where IDBill=@idBill and IDService=@idService
	end
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateBookRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_UpdateBookRoom]
@id int,@dateCheckIn date,@datecheckOut date
as
begin
	update BookRoom
	set DateCheckIn=@dateCheckIn,DateCheckOut=@datecheckOut
	where ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateChangeBillDetails]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[USP_UpdateChangeBillDetails]
@idBill int,@idService int, @idServiceOld int,@_count int
AS
BEGIN
	
	SET NOCOUNT ON;

 declare @totalPrice int,@price int,@count int

	
	select @price=Price
	from Service
	where ID=@idService
	select @count=Count
	from Bill A,BillDetails B
	where A.ID=B.IDBill and A.ID=@idBill and A.StatusBill=N'Chưa thanh toán' and B.IDService=@idService  
	if(@count>0)
	begin
		set @totalPrice=@_count*@price
		update BillDetails
		set Count=@_count,TotalPrice=@totalPrice
		where IDBill=@idBill and IDService=@idService
	end
	else
	begin
		
		set @totalPrice=@_count*@price
		update BillDetails
		set Count=@_count,TotalPrice=@totalPrice,IDService=@idService
		where IDBill=@idBill and IDService=@idServiceOld
	end
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateCustomer]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateCustomer]
@id INT, @customerName NVARCHAR(100), @idCardNow NVARCHAR(100), @address NVARCHAR(200),
@dateOfBirth date, @phoneNumber NVARCHAR(100), @sex NVARCHAR(100), @nationality NVARCHAR(100), @idCardPre NVARCHAR(100)
AS
BEGIN
	IF(@idCardPre != @idCardNow)
	begin
		DECLARE @count INT=0
		SELECT @count=COUNT(*)
		FROM dbo.Customer
		WHERE IDCard = @idCardNow
		IF(@count=0)
		BEGIN
			UPDATE dbo.Customer 
			SET 
			Name =@customerName, IDCard =@idCardNow,
			Address = @address, DateOfBirth =@dateOfBirth, PhoneNumber =@phoneNumber,
			Nationality = @nationality, Sex = @sex
			WHERE ID = @id
		END
	END
	ELSE
	BEGIN
		UPDATE dbo.Customer 
			SET 
			Name =@customerName,Address = @address,
			DateOfBirth =@dateOfBirth, PhoneNumber =@phoneNumber,
			Nationality = @nationality, Sex = @sex
			WHERE ID = @id
	end
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateCustomer_]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROCEDURE  [dbo].[USP_UpdateCustomer_]
@id int, @name NVARCHAR(100),
@idCard NVARCHAR(100),@phoneNumber NVARCHAR(100), @dateOfBirth DATE, 
@address NVARCHAR(200),@sex NVARCHAR(100),@nationality NVARCHAR(100)
AS
begin

	begin
	update Customer
	set Name=@name,IDCard=@idCard,PhoneNumber=@phoneNumber,DateOfBirth=@dateOfBirth,Address=@address,Sex=@sex,Nationality=@nationality
	where ID=@id
end
	
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateDisplayName]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_UpdateDisplayName]
@username int,@displayname nvarchar(100)
as
begin
	update Staff
	set Name=@displayname
	where ID=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateInfo]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE proc [dbo].[USP_UpdateInfo]
@username nvarchar(100),@address nvarchar(100),@phonenumber nvarchar(100),@idcard nvarchar(100),@dateOfBirth date,@sex nvarchar(50)
as
begin
	update Staff
	set Address=@address,PhoneNumber=@phonenumber,IDCard=@idcard,Sex=@sex, DateOfBirth=@dateOfBirth
	where UserName=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateParameter]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateParameter]
@name NVARCHAR(200), @value float, @describe NVARCHAR(200)
AS
BEGIN
UPDATE dbo.PARAMETER
	SET
	Value = @value,
	Describe = @describe,
	datemodify = GETDATE()
	WHERE name = @name
	SELECT @name = [dbo].[ConvertString](@name)
	IF(@name = 'QD2.1')
		UPDATE dbo.ROOMTYPE SET LimitPerson = @value WHERE ID = 1
	ELSE IF(@name = 'QD2.2')
		UPDATE dbo.ROOMTYPE SET LimitPerson = @value WHERE ID = 2
	ELSE IF(@name = 'QD2.3')
		UPDATE dbo.ROOMTYPE SET LimitPerson = @value WHERE ID = 3
	ELSE IF(@name = 'QD2.4')
		UPDATE dbo.ROOMTYPE SET LimitPerson = @value WHERE ID = 4
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdatePassword]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE  proc [dbo].[USP_UpdatePassword]
@username nvarchar(100),@password nvarchar(100)
as
begin
	update Staff
	set PassWord=@password
	where UserName=@username
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateReceiveRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[InsertReceiveRoomDetails]    Script Date: 11/21/2023 4:26:23 AM ******/
CREATE proc [dbo].[USP_UpdateReceiveRoom]
@idRoom int,@id int
as

begin

	Update BookRoom
	set IDRoom=@id where ID=@idRoom
	Update Room
	set StatusRoom=N'Có người' where ID=@id 
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------

--Status Room
--------------------------------------------------------------

CREATE PROC [dbo].[USP_UpdateRoom]
@id INT, @nameRoom NVARCHAR(100), @idRoomType INT, @StatusRoom NVARCHAR(100)
AS
UPDATE dbo.Room
SET
	Name = @nameRoom, IDRoomType = @idRoomType, StatusRoom = StatusRoom
WHERE ID = @id
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateRoomType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateRoomType]
@id INT, @name NVARCHAR(100), @price int, @limitPerson int
AS
	UPDATE RoomType
	SET
    name = @name, Price = @price, LimitPerson = @limitPerson
	WHERE id =@id
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateService]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--parameter
--------------------------------------------------------------
create proc [dbo].[USP_UpdateService]
@id int, @name nvarchar(200), @idServiceType int, @price int
as
begin
	update service
	set
	name = @name,
	idservicetype = @idservicetype,
	price = @price
	where id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateServiceType]
@id INT, @name NVARCHAR(100)
AS
BEGIN
	UPDATE dbo.ServiceType
	SET
    name = @name
	WHERE id =@id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateStaff]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--------------------------------------------------------------
--Customer Type
--------------------------------------------------------------

CREATE PROC [dbo].[USP_UpdateStaff]
@user NVARCHAR(100), @name NVARCHAR(100),@idStaffType INT,
@idCard NVARCHAR(100), @dateOfBirth DATE, @sex NVARCHAR(100),
@address NVARCHAR(200), @phoneNumber nvarchar(100), @startDay DATE
AS
BEGIN
	DECLARE @count INT = 0
	SELECT @count=COUNT(*) FROM staff
	WHERE IDCard = @idCard AND UserName != @user
	IF(@count = 0)
	UPDATE dbo.STAFF
	SET
    Name = @name, idstafftype = @idstafftype,
	idcard= @idCard, DateOfBirth = @dateOfBirth, sex = @sex,
	Address = @address, PhoneNumber = @phoneNumber, StartDay = @startDay
	WHERE UserName = @user
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateStaffType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UpdateStaffType] 
@id int, @name NVARCHAR(100)
AS
BEGIN
	UPDATE dbo.StaffType
	SET
    Name = @name
	WHERE ID = @id
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateStatusRoom]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[USP_UpdateStatusRoom]
@idRoom int
as
begin
	update Room
	set StatusRoom=N'Có người'
	where ID=@idRoom
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UpdateStatusRoomOld]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[InsertReceiveRoomDetails]    Script Date: 11/21/2023 4:26:23 AM ******/
create proc [dbo].[USP_UpdateStatusRoomOld]
@idRoom int
as
declare @id int
begin
select @id=b.ID  from BookRoom a, Room b where a.ID=@idRoom and b.ID=a.IDRoom
	Update Room
	set StatusRoom=N'Trống' where ID=@id
end
GO
/****** Object:  StoredProcedure [dbo].[USP_UseServiceForServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[USP_UseServiceForServiceType]
@id INT
AS
BEGIN
	select * from
	ServiceType a, Service b
	WHERE a.ID =@id and b.IDServiceType=a.ID
END
GO
/****** Object:  StoredProcedure [dbo].[USP_UseServiceinBillForServiceType]    Script Date: 12/8/2023 11:56:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [dbo].[USP_UseServiceinBillForServiceType]
@id INT
AS
BEGIN
	select * from
	ServiceType a, Service b, BillDetails c
	WHERE a.ID =@id and b.IDServiceType=a.ID and c.IDService=a.ID
END
GO
