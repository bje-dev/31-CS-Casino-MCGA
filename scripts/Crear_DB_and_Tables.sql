/****** Object:  Database [CASINOS]    Script Date: 11/28/2023 12:28:32 AM ******/
CREATE DATABASE [CASINOS]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_1', MAXSIZE = 2 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [CASINOS] SET COMPATIBILITY_LEVEL = 150
GO
ALTER DATABASE [CASINOS] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CASINOS] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CASINOS] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CASINOS] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CASINOS] SET ARITHABORT OFF 
GO
ALTER DATABASE [CASINOS] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CASINOS] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CASINOS] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CASINOS] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CASINOS] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CASINOS] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CASINOS] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CASINOS] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CASINOS] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [CASINOS] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CASINOS] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CASINOS] SET  MULTI_USER 
GO
ALTER DATABASE [CASINOS] SET ENCRYPTION ON
GO
ALTER DATABASE [CASINOS] SET QUERY_STORE = ON
GO
ALTER DATABASE [CASINOS] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[Apuestas]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Apuestas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_terminal] [int] NULL,
	[id_casino] [int] NULL,
	[monto] [decimal](18, 2) NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK_Apuestas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Apuestasmaxmin]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Apuestasmaxmin](
	[id_casino] [int] NULL,
	[apuesta_min] [decimal](18, 2) NULL,
	[apuesta_max] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Bitacora]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bitacora](
	[idBitacora] [int] IDENTITY(1000,1) NOT NULL,
	[dateTime] [datetime] NOT NULL,
	[operacion] [nvarchar](50) NOT NULL,
	[componente] [nvarchar](50) NOT NULL,
	[casino] [int] NULL,
	[descripcion] [nvarchar](max) NULL,
	[resultado] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idBitacora] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Casinos]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Casinos](
	[id] [int] NOT NULL,
	[nombre] [varchar](50) NULL,
 CONSTRAINT [PK_Casinos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Depositos]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Depositos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_terminal] [int] NULL,
	[id_casino] [int] NULL,
	[monto] [decimal](18, 2) NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK_Depositos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pagos]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pagos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_terminal] [int] NULL,
	[id_casino] [int] NULL,
	[monto] [decimal](18, 2) NULL,
	[fecha] [datetime] NULL,
 CONSTRAINT [PK_Pagos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Premiosmaxmin]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Premiosmaxmin](
	[id_casino] [int] NULL,
	[apuesta_desde] [decimal](18, 2) NULL,
	[apuesta_hasta] [decimal](18, 2) NULL,
	[premio_min] [decimal](18, 2) NULL,
	[premio_max] [decimal](18, 2) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Terminales]    Script Date: 11/28/2023 12:28:32 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Terminales](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_casino] [int] NULL,
	[licencia] [bit] NULL,
	[sesion] [bit] NULL,
 CONSTRAINT [PK_Terminales] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Apuestas]  WITH CHECK ADD  CONSTRAINT [FK_Apuestas_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Apuestas] CHECK CONSTRAINT [FK_Apuestas_Casinos]
GO
ALTER TABLE [dbo].[Apuestas]  WITH CHECK ADD  CONSTRAINT [FK_Apuestas_Terminales] FOREIGN KEY([id_terminal])
REFERENCES [dbo].[Terminales] ([id])
GO
ALTER TABLE [dbo].[Apuestas] CHECK CONSTRAINT [FK_Apuestas_Terminales]
GO
ALTER TABLE [dbo].[Apuestasmaxmin]  WITH CHECK ADD  CONSTRAINT [FK_Apuestasmaxmin_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Apuestasmaxmin] CHECK CONSTRAINT [FK_Apuestasmaxmin_Casinos]
GO
ALTER TABLE [dbo].[Depositos]  WITH CHECK ADD  CONSTRAINT [FK_Depositos_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Depositos] CHECK CONSTRAINT [FK_Depositos_Casinos]
GO
ALTER TABLE [dbo].[Depositos]  WITH CHECK ADD  CONSTRAINT [FK_Depositos_Terminales] FOREIGN KEY([id_terminal])
REFERENCES [dbo].[Terminales] ([id])
GO
ALTER TABLE [dbo].[Depositos] CHECK CONSTRAINT [FK_Depositos_Terminales]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_Casinos]
GO
ALTER TABLE [dbo].[Pagos]  WITH CHECK ADD  CONSTRAINT [FK_Pagos_Terminales] FOREIGN KEY([id_terminal])
REFERENCES [dbo].[Terminales] ([id])
GO
ALTER TABLE [dbo].[Pagos] CHECK CONSTRAINT [FK_Pagos_Terminales]
GO
ALTER TABLE [dbo].[Premiosmaxmin]  WITH CHECK ADD  CONSTRAINT [FK_Premiosmaxmin_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Premiosmaxmin] CHECK CONSTRAINT [FK_Premiosmaxmin_Casinos]
GO
ALTER TABLE [dbo].[Terminales]  WITH CHECK ADD  CONSTRAINT [FK_Terminales_Casinos] FOREIGN KEY([id_casino])
REFERENCES [dbo].[Casinos] ([id])
GO
ALTER TABLE [dbo].[Terminales] CHECK CONSTRAINT [FK_Terminales_Casinos]
GO
ALTER DATABASE [CASINOS] SET  READ_WRITE 
GO
