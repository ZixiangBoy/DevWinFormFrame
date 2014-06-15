USE [RdsSyncDb]
GO
/****** Object:  StoredProcedure [dbo].[P_ERP_AutoTakeRefundSyncByNick]    Script Date: 2014/6/15 10:24:41 ******/

/*

按店铺自动提取需要同步的退款数据

*/
CREATE proc [dbo].[P_ERP_AutoTakeRefundSyncByNick](@Nick nvarchar(200),@minjdp datetime output,@maxjdp datetime output) as

begin tran	
	;
	with tv as(
	select max(jdp_modified) jdp_modified,seller_nick from [sys_info]..jdp_tb_refund where seller_nick=@Nick group by seller_nick
	)
	update T_ERP_Sync_Refund
	set 
	T_ERP_Sync_Refund.MinJDPModified=T_ERP_Sync_Refund.MaxJDPModified,
	T_ERP_Sync_Refund.MaxJDPModified=tv.jdp_modified,
	T_ERP_Sync_Refund.LastUpdateTime=getdate()
	from tv
	where tv.seller_nick=T_ERP_Sync_Refund.SellerNick

	select @minjdp=MinJDPModified,@maxjdp=MaxJDPModified from T_ERP_Sync_Refund where SellerNick=@Nick

	select tid,seller_nick,jdp_response from [sys_info]..jdp_tb_refund a 
	join T_ERP_Sync_Refund d
	on a.seller_nick=d.SellerNick and d.SellerNick=@Nick and a.jdp_modified>d.MinJDPModified 
	and a.jdp_modified<=d.MaxJDPModified
commit

GO
/****** Object:  StoredProcedure [dbo].[P_ERP_AutoTakeSyncByNick]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
按店铺自动提取需要同步的数据
*/
create proc [dbo].[P_ERP_AutoTakeSyncByNick](@Nick nvarchar(200),@minjdp datetime ,@maxjdp datetime ) as
begin 	
		
	select tid,seller_nick,jdp_response from [sys_info]..jdp_tb_trade a 	
	where a.seller_nick=@Nick and a.jdp_modified>@minjdp and a.jdp_modified<=@maxjdp
	update [RdsSyncDb]..T_ERP_Sync_SysInfo
	set 
	[RdsSyncDb]..T_ERP_Sync_SysInfo.MinJDPModified=[RdsSyncDb]..T_ERP_Sync_SysInfo.MaxJDPModified,
	[RdsSyncDb]..T_ERP_Sync_SysInfo.MaxJDPModified=@maxjdp,--tv.jdp_modified,
	[RdsSyncDb]..T_ERP_Sync_SysInfo.LastUpdateTime=getdate()
	where [RdsSyncDb]..T_ERP_Sync_SysInfo.SellerNick=@Nick
end



GO
/****** Object:  StoredProcedure [dbo].[P_ERP_InitSync_RefundTime_Dync]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[P_ERP_InitSync_RefundTime_Dync](@tpdb nvarchar(20)='[sys_info]') as
begin
	declare @sql nvarchar(500)
	set @sql=
	N'insert into '+N'T_ERP_Sync_Refund(MinJDPModified,MaxJDPModified,SellerNick)
	select min(jdp_modified),max(jdp_modified),seller_nick from '+@tpdb+N'..jdp_tb_refund 
	where not exists(select 1 from '+N'T_ERP_Sync_Refund where SellerNick=seller_nick)
	group by seller_nick'
	exec sp_executesql @sql
	insert T_ERP_Sync_Refund(SellerNick,MinJDPModified,MaxJDPModified)
	select a.SellerNick,'2013-12-03 00:00:00.000','2013-12-03 00:00:00.000' from [T_ERP_Sync_SysInfo] a left join T_ERP_Sync_Refund b
	on a.SellerNick=b.SellerNick where b.SellerNick Is Null
end

GO
/****** Object:  StoredProcedure [dbo].[P_ERP_InitSync_SysInfoTime_Dync]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[P_ERP_InitSync_SysInfoTime_Dync](@erpdb nvarchar(20)='[erp_bt]',@tpdb nvarchar(20)='[sys_info]') as
begin
	declare @sql nvarchar(500)
	set @sql=
	'insert into '+@erpdb+'..T_ERP_Sync_SysInfo(MinJDPModified,MaxJDPModified,SellerNick)
	select min(jdp_modified),max(jdp_modified),seller_nick from '+@tpdb+'..jdp_tb_trade 
	where not exists(select 1 from '+@erpdb+'..T_ERP_Sync_SysInfo where SellerNick=seller_nick) group by seller_nick'
	exec sp_executesql @sql
end

GO
/****** Object:  StoredProcedure [dbo].[P_ERP_Job_SyncJdpItem]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
每日同步商品信息
*/
CREATE proc [dbo].[P_ERP_Job_SyncJdpItem] as
begin
	truncate table T_ERP_Sync_Item
	insert T_ERP_Sync_Item
	(num_iid,nick,approve_status,has_showcase,created,modified,cid,has_discount,jdp_hashcode,jdp_response
	,jdp_delete,jdp_created,jdp_modified
	)
	select num_iid,nick,approve_status,has_showcase,created,modified,cid,has_discount,jdp_hashcode,jdp_response
	,jdp_delete,jdp_created,jdp_modified from sys_info..jdp_tb_item
end
GO
/****** Object:  StoredProcedure [dbo].[P_ERP_RetryTakeSyncByNick]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
按店铺重试提取需要同步的数据
*/
create proc [dbo].[P_ERP_RetryTakeSyncByNick](@Nick nvarchar(200),@minjdp datetime ,@maxjdp datetime ) as
begin 	
		
	select tid,seller_nick,jdp_response from [sys_info]..jdp_tb_trade a 	
	where a.seller_nick=@Nick and a.jdp_modified>@minjdp and a.jdp_modified<=@maxjdp
	--update T_ERP_Sync_SysInfo
	--set 
	--T_ERP_Sync_SysInfo.MinJDPModified=T_ERP_Sync_SysInfo.MaxJDPModified,
	--T_ERP_Sync_SysInfo.MaxJDPModified=@maxjdp,--tv.jdp_modified,
	--T_ERP_Sync_SysInfo.LastUpdateTime=getdate()
	--where T_ERP_Sync_SysInfo.SellerNick=@Nick
end


GO
/****** Object:  StoredProcedure [dbo].[P_ERP_SyncInit_SyncShop]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
初始化分店分表名称
*/
CREATE proc [dbo].[P_ERP_SyncInit_SyncShop] as
begin
	insert T_ERP_Sync_Shop(SellerNick)
	select a.SellerNick from [T_ERP_Sync_SysInfo] a
	left join T_ERP_Sync_Shop b on a.SellerNick=b.SellerNick
	where b.SellerNick Is Null

	update T_ERP_Sync_Shop
	set RefundTableName='T_ERP_ShopRefund'+Cast(Id as nvarchar(20))
	,TradeTableName='T_ERP_ShopTrade'+Cast(Id as nvarchar(20))

	declare @mid int
	select @mid=max(Id) from T_ERP_Sync_Shop
	declare @crdtb nvarchar(500)
	set @crdtb=N'create table T_ERP_ShopRefund$
(
	Id bigint identity(1,1) not null primary key
	,Guid uniqueidentifier not null default newid()
	,[Begin] datetime not null ,[End] datetime not null
	,SellerNick nvarchar(50) not null default ''''
	,CreateTime datetime not null default getdate()
	,JdpCount int not null default 0
)'
	declare @crdtbIdx nvarchar(500)
	set @crdtbIdx=N'create unique index trdIdx on T_ERP_ShopRefund$(Guid)'
	declare @crdtrdIdx nvarchar(500)
	set @crdtrdIdx=N'create unique index trdIdx on T_ERP_ShopTrade$(Guid)'
	declare @crdtrd nvarchar(500)
	set @crdtrd=N'create table T_ERP_ShopTrade$
(
	Id bigint identity(1,1) not null primary key
	,Guid uniqueidentifier not null default newid()
	,[Begin] datetime not null ,[End] datetime not null
	,SellerNick nvarchar(50) not null default ''''
	,CreateTime datetime not null default getdate()
	,JdpCount int not null default 0
	,TrdStep int not null default 0
	,TrdPayed int not null default 0
	,TrdUnPay int not null default 0
)'
	declare @cursql nvarchar(500)
	declare @cursqltrd nvarchar(500)
	declare @cursqlIdx nvarchar(100)
	declare @cursqltrdIdx nvarchar(100)
	while @mid>0
	begin
		set @cursql=Replace(@crdtb,'$',cast(@mid as nvarchar(10)))
		set @cursqltrd=Replace(@crdtrd,'$',cast(@mid as nvarchar(10)))
		set @cursqlIdx=Replace(@crdtbIdx,'$',cast(@mid as nvarchar(10)))
		set @cursqltrdIdx=Replace(@crdtrdIdx,'$',cast(@mid as nvarchar(10)))
		exec sp_executesql @cursql
		exec sp_executesql @cursqltrd
		exec sp_executesql @cursqlIdx
		exec sp_executesql @cursqltrdIdx
		select @mid=@mid-1
	end
end
GO
/****** Object:  StoredProcedure [dbo].[P_ERP_TakeSyncRangeByNick]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
按店铺提取需要同步的数据的时间段范围
*/
create proc [dbo].[P_ERP_TakeSyncRangeByNick](@Nick nvarchar(200),@minjdp datetime output,@maxjdp datetime output) as
begin 	
	declare @mxjdp datetime
	declare @curmxjdp datetime
	declare @newmxjdp datetime
	declare @min int
	select @curmxjdp=MaxJDPModified from [RdsSyncDb]..T_ERP_Sync_SysInfo where SellerNick=@Nick 
	select @mxjdp=max(jdp_modified) from [sys_info]..jdp_tb_trade where seller_nick=@Nick 
	select @min=DATEDIFF(minute,@curmxjdp,@mxjdp)
	if @min<=3
	begin
		select @maxjdp=@mxjdp,@minjdp=MaxJDPModified from [RdsSyncDb]..T_ERP_Sync_SysInfo where SellerNick=@Nick		
	end
	else
	begin
		select @newmxjdp=dateadd(minute,3,@curmxjdp)		
		select @maxjdp=@newmxjdp,@minjdp=MaxJDPModified from [RdsSyncDb]..T_ERP_Sync_SysInfo where SellerNick=@Nick
		
	end
end



GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund1]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund1](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund2]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund2](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund3]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund3](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund4]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund4](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund5]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund5](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund6]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund6](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund7]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund7](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopRefund8]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopRefund8](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade1]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade1](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade2]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade2](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade3]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade3](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade4]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade4](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade5]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade5](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade6]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade6](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade7]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade7](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_ShopTrade8]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_ShopTrade8](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Begin] [datetime] NOT NULL,
	[End] [datetime] NOT NULL,
	[SellerNick] [nvarchar](50) NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[JdpCount] [int] NOT NULL,
	[TrdStep] [int] NOT NULL,
	[TrdPayed] [int] NOT NULL,
	[TrdUnPay] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_Sync_Item]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[T_ERP_Sync_Item](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[num_iid] [bigint] NOT NULL,
	[nick] [varchar](32) NULL,
	[approve_status] [varchar](32) NULL,
	[has_showcase] [varchar](32) NULL,
	[created] [datetime] NULL,
	[modified] [datetime] NULL,
	[cid] [varchar](256) NULL,
	[has_discount] [varchar](32) NULL,
	[jdp_hashcode] [varchar](128) NULL,
	[jdp_response] [text] NULL,
	[jdp_delete] [int] NULL,
	[jdp_created] [datetime] NULL,
	[jdp_modified] [datetime] NULL,
	[CreateDate] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[T_ERP_Sync_Refund]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_Sync_Refund](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[SellerNick] [nvarchar](200) NULL,
	[MinJDPModified] [datetime] NULL,
	[MaxJDPModified] [datetime] NULL,
	[DataCreateTime] [datetime] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_Sync_Shop]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_Sync_Shop](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellerNick] [nvarchar](200) NOT NULL,
	[RefundTableName] [nvarchar](100) NULL,
	[TradeTableName] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_Sync_SysInfo]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_Sync_SysInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[SellerNick] [nvarchar](200) NULL,
	[MinJDPModified] [datetime] NULL,
	[MaxJDPModified] [datetime] NULL,
	[DataCreateTime] [datetime] NOT NULL,
	[LastUpdateTime] [datetime] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_SyncLog]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_SyncLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellerNick] [nvarchar](200) NULL,
	[MinDateTime] [datetime] NULL,
	[MaxDateTime] [datetime] NULL,
	[CreateTime] [datetime] NOT NULL,
	[Remark] [nvarchar](200) NULL,
	[JdpCounts] [int] NULL,
	[JdpResCounts] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_SynSaveChartLog]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_SynSaveChartLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SellerNick] [nvarchar](200) NOT NULL,
	[SavePath] [nvarchar](4000) NOT NULL,
	[IsUsing] [bit] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_TaoBao_Refund]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_TaoBao_Refund](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[Address] [nvarchar](200) NULL,
	[AdvanceStatus] [bigint] NULL,
	[AlipayNo] [nvarchar](200) NULL,
	[BuyerNick] [nvarchar](200) NULL,
	[CompanyName] [nvarchar](200) NULL,
	[Created] [datetime] NULL,
	[CsStatus] [bigint] NULL,
	[Desc] [nvarchar](max) NULL,
	[GoodReturnTime] [datetime] NULL,
	[GoodStatus] [nvarchar](200) NULL,
	[HasGoodReturn] [bit] NULL,
	[Iid] [nvarchar](200) NULL,
	[Modified] [datetime] NULL,
	[Num] [bigint] NULL,
	[NumIid] [bigint] NULL,
	[Oid] [bigint] NULL,
	[OrderStatus] [nvarchar](200) NULL,
	[Payment] [decimal](18, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[Reason] [nvarchar](200) NULL,
	[RefundFee] [decimal](18, 2) NULL,
	[RefundId] [bigint] NULL,
	[Timeout] [datetime] NULL,
	[SellerNick] [nvarchar](200) NULL,
	[ShippingType] [nvarchar](200) NULL,
	[Sid] [nvarchar](200) NULL,
	[SplitSellerFee] [decimal](18, 2) NULL,
	[SplitTaobaoFee] [decimal](18, 2) NULL,
	[Status] [nvarchar](200) NULL,
	[Tid] [bigint] NULL,
	[Title] [nvarchar](200) NULL,
	[TotalFee] [decimal](18, 2) NULL,
	[RemindType] [bigint] NULL,
	[ExistTimeout] [bit] NULL,
	[IsMoved] [bit] NULL,
 CONSTRAINT [PK_T_ERP_TaoBao_Refund] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_TaoBaoATS_Order]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_TaoBaoATS_Order](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Tid] [decimal](18, 0) NULL,
	[AdjustFee] [decimal](18, 2) NULL,
	[BuyerNick] [nvarchar](200) NULL,
	[BuyerRate] [bit] NULL,
	[Cid] [decimal](18, 0) NULL,
	[DiscountFee] [decimal](18, 2) NULL,
	[EndTime] [datetime] NULL,
	[Iid] [nvarchar](200) NULL,
	[IsOversold] [bit] NULL,
	[IsServiceOrder] [bit] NULL,
	[ItemMealId] [decimal](18, 0) NULL,
	[ItemMealName] [nvarchar](300) NULL,
	[Modified] [datetime] NULL,
	[Num] [decimal](18, 0) NULL,
	[NumIid] [decimal](18, 0) NULL,
	[Oid] [decimal](18, 0) NULL,
	[OrderFrom] [nvarchar](50) NULL,
	[OuterIid] [nvarchar](200) NULL,
	[OuterSkuId] [nvarchar](200) NULL,
	[Payment] [decimal](18, 2) NULL,
	[PicPath] [nvarchar](200) NULL,
	[Price] [decimal](18, 2) NULL,
	[RefundId] [decimal](18, 0) NULL,
	[RefundStatus] [nvarchar](100) NULL,
	[SellerNick] [nvarchar](100) NULL,
	[SellerRate] [bit] NULL,
	[SellerType] [nvarchar](50) NULL,
	[SkuId] [nvarchar](100) NULL,
	[SkuPropertiesName] [nvarchar](1000) NULL,
	[Snapshot] [nvarchar](1000) NULL,
	[SnapshotUrl] [nvarchar](1000) NULL,
	[Status] [nvarchar](100) NULL,
	[TimeoutActionTime] [datetime] NULL,
	[Title] [nvarchar](1000) NULL,
	[TotalFee] [decimal](18, 2) NULL,
	[DataCreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK_T_ERP_TaoBaoATS_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_TaoBaoATS_PromotionDetail]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_TaoBaoATS_PromotionDetail](
	[PId] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Tid] [decimal](18, 0) NULL,
	[DiscountFee] [decimal](18, 2) NULL,
	[GiftItemId] [nvarchar](200) NULL,
	[GiftItemName] [nvarchar](200) NULL,
	[GiftItemNum] [decimal](18, 0) NULL,
	[Id] [decimal](18, 0) NULL,
	[PromotionDesc] [nvarchar](1000) NULL,
	[PromotionId] [nvarchar](200) NULL,
	[PromotionName] [nvarchar](200) NULL,
	[IsUsing] [bit] NULL,
	[DataCreateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_TaobaoATS_ServiceOrder]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_TaobaoATS_ServiceOrder](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Tid] [decimal](18, 0) NULL,
	[BuyerNick] [nvarchar](200) NULL,
	[ItemOid] [decimal](18, 0) NULL,
	[Num] [decimal](18, 0) NULL,
	[Oid] [decimal](18, 0) NULL,
	[Payment] [decimal](18, 4) NULL,
	[PicPath] [nvarchar](1000) NULL,
	[Price] [decimal](18, 4) NULL,
	[RefundId] [decimal](18, 0) NULL,
	[SellerNick] [nvarchar](200) NULL,
	[ServiceDetailUrl] [nvarchar](1000) NULL,
	[ServiceId] [decimal](18, 0) NULL,
	[Title] [nvarchar](500) NULL,
	[TotalFee] [decimal](18, 4) NULL,
	[CreateTime] [datetime] NOT NULL,
	[TimeStamp] [timestamp] NOT NULL,
 CONSTRAINT [PK_T_ERP_TaobaoATS_ServiceOrder] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_TaoBaoATS_Trade]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_TaoBaoATS_Trade](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[AdjustFee] [decimal](18, 2) NULL,
	[AlipayId] [decimal](18, 0) NULL,
	[AlipayNo] [nvarchar](500) NULL,
	[AlipayUrl] [nvarchar](1000) NULL,
	[AlipayWarnMsg] [nvarchar](1000) NULL,
	[AreaId] [nvarchar](300) NULL,
	[AvailableConfirmFee] [decimal](18, 2) NULL,
	[BuyerAlipayNo] [nvarchar](500) NULL,
	[BuyerArea] [nvarchar](1000) NULL,
	[BuyerCodFee] [decimal](18, 2) NULL,
	[BuyerEmail] [nvarchar](200) NULL,
	[BuyerFlag] [int] NULL,
	[BuyerMemo] [nvarchar](2000) NULL,
	[BuyerMessage] [nvarchar](2000) NULL,
	[BuyerNick] [nvarchar](500) NULL,
	[BuyerObtainPointFee] [decimal](18, 0) NULL,
	[BuyerRate] [bit] NULL,
	[CanRate] [bit] NULL,
	[CodFee] [decimal](18, 2) NULL,
	[CodStatus] [nvarchar](300) NULL,
	[CommissionFee] [decimal](18, 2) NULL,
	[ConsignTime] [datetime] NULL,
	[Created] [datetime] NULL,
	[CreditCardFee] [decimal](18, 2) NULL,
	[DiscountFee] [decimal](18, 2) NULL,
	[EndTime] [datetime] NULL,
	[EticketExt] [nvarchar](200) NULL,
	[ExpressAgencyFee] [decimal](18, 2) NULL,
	[HasBuyerMessage] [bit] NULL,
	[HasPostFee] [bit] NULL,
	[HasYfx] [bit] NULL,
	[Iid] [nvarchar](300) NULL,
	[InvoiceName] [nvarchar](500) NULL,
	[Is3D] [bit] NULL,
	[IsBrandSale] [bit] NULL,
	[IsForceWlb] [bit] NULL,
	[IsLgtype] [bit] NULL,
	[LgAging] [datetime] NULL,
	[LgAgingType] [nvarchar](300) NULL,
	[MarkDesc] [nvarchar](1000) NULL,
	[Modified] [datetime] NULL,
	[Num] [decimal](18, 0) NULL,
	[NumIid] [decimal](18, 0) NULL,
	[NutFeature] [nvarchar](1000) NULL,
	[Orders] [uniqueidentifier] NULL,
	[Payment] [decimal](18, 2) NULL,
	[PayTime] [datetime] NULL,
	[PicPath] [nvarchar](max) NULL,
	[PointFee] [decimal](18, 0) NULL,
	[PostFee] [decimal](18, 2) NULL,
	[Price] [decimal](18, 2) NULL,
	[Promotion] [nvarchar](max) NULL,
	[PromotionDetails] [uniqueidentifier] NULL,
	[RealPointFee] [decimal](18, 0) NULL,
	[ReceivedPayment] [decimal](18, 2) NULL,
	[ReceiverAddress] [nvarchar](1000) NULL,
	[ReceiverCity] [nvarchar](200) NULL,
	[ReceiverDistrict] [nvarchar](200) NULL,
	[ReceiverMobile] [nvarchar](50) NULL,
	[ReceiverName] [nvarchar](200) NULL,
	[ReceiverPhone] [nvarchar](50) NULL,
	[ReceiverState] [nvarchar](200) NULL,
	[ReceiverZip] [nvarchar](20) NULL,
	[SellerAlipayNo] [nvarchar](300) NULL,
	[SellerCodFee] [decimal](18, 2) NULL,
	[SellerEmail] [nvarchar](200) NULL,
	[SellerFlag] [int] NULL,
	[SellerMemo] [nvarchar](2000) NULL,
	[SellerMobile] [nvarchar](50) NULL,
	[SellerName] [nvarchar](100) NULL,
	[SellerNick] [nvarchar](100) NULL,
	[SellerPhone] [nvarchar](50) NULL,
	[SellerRate] [bit] NULL,
	[ServiceOrders] [uniqueidentifier] NULL,
	[ShippingType] [nvarchar](50) NULL,
	[Snapshot] [nvarchar](1000) NULL,
	[SnapshotUrl] [nvarchar](1000) NULL,
	[Status] [nvarchar](50) NULL,
	[StepPaidFee] [decimal](18, 2) NULL,
	[StepTradeStatus] [nvarchar](50) NULL,
	[Tid] [decimal](18, 0) NULL,
	[TimeoutActionTime] [datetime] NULL,
	[Title] [nvarchar](500) NULL,
	[TotalFee] [decimal](18, 2) NULL,
	[TradeFrom] [nvarchar](50) NULL,
	[TradeMemo] [nvarchar](1000) NULL,
	[Type] [nvarchar](50) NULL,
	[YfxFee] [decimal](18, 2) NULL,
	[YfxId] [nvarchar](100) NULL,
	[DataCreateTime] [datetime] NOT NULL,
 CONSTRAINT [PK__T_ERP_TaoBaoATS___4634284C] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[T_ERP_WriteFileErr]    Script Date: 2014/6/15 10:24:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_ERP_WriteFileErr](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[FailFileName] [nvarchar](1000) NULL,
	[TypeName] [nvarchar](20) NULL,
	[JsonText] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[T_ERP_ShopRefund1] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund1] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund1] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund1] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund2] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund2] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund2] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund2] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund3] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund3] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund3] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund3] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund4] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund4] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund4] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund4] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund5] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund5] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund5] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund5] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund6] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund6] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund6] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund6] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund7] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund7] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund7] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund7] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund8] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund8] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund8] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopRefund8] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade1] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade2] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade3] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade4] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade5] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade6] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade7] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT ('') FOR [SellerNick]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT ((0)) FOR [JdpCount]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT ((0)) FOR [TrdStep]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT ((0)) FOR [TrdPayed]
GO
ALTER TABLE [dbo].[T_ERP_ShopTrade8] ADD  DEFAULT ((0)) FOR [TrdUnPay]
GO
ALTER TABLE [dbo].[T_ERP_Sync_Item] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_Sync_Item] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[T_ERP_Sync_Refund] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_Sync_Refund] ADD  DEFAULT (getdate()) FOR [DataCreateTime]
GO
ALTER TABLE [dbo].[T_ERP_Sync_Refund] ADD  DEFAULT (getdate()) FOR [LastUpdateTime]
GO
ALTER TABLE [dbo].[T_ERP_Sync_SysInfo] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_Sync_SysInfo] ADD  DEFAULT (getdate()) FOR [DataCreateTime]
GO
ALTER TABLE [dbo].[T_ERP_Sync_SysInfo] ADD  DEFAULT (getdate()) FOR [LastUpdateTime]
GO
ALTER TABLE [dbo].[T_ERP_SyncLog] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_SynSaveChartLog] ADD  DEFAULT ((0)) FOR [IsUsing]
GO
ALTER TABLE [dbo].[T_ERP_SynSaveChartLog] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_TaoBao_Refund] ADD  CONSTRAINT [DF_T_ERP_TaoBao_Refund_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_TaoBao_Refund] ADD  CONSTRAINT [DF__T_ERP_Tao__Creat__0697FACD]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_Order] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_Order] ADD  DEFAULT (getdate()) FOR [DataCreateTime]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_PromotionDetail] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_PromotionDetail] ADD  DEFAULT (getdate()) FOR [DataCreateTime]
GO
ALTER TABLE [dbo].[T_ERP_TaobaoATS_ServiceOrder] ADD  CONSTRAINT [DF__T_ERP_Taob__Guid__51A5DAF8]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_TaobaoATS_ServiceOrder] ADD  CONSTRAINT [DF__T_ERP_Tao__Creat__5299FF31]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_Trade] ADD  CONSTRAINT [DF__T_ERP_TaoB__Guid__47284C85]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_TaoBaoATS_Trade] ADD  CONSTRAINT [DF__T_ERP_Tao__DataC__481C70BE]  DEFAULT (getdate()) FOR [DataCreateTime]
GO
ALTER TABLE [dbo].[T_ERP_WriteFileErr] ADD  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[T_ERP_WriteFileErr] ADD  DEFAULT (getdate()) FOR [CreateTime]
GO
