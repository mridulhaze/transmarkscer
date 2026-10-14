/*
Navicat SQL Server Data Transfer

Source Server         : SQL
Source Server Version : 160000
Source Host           : DESKTOP-VF08655:1433
Source Database       : solving
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 160000
File Encoding         : 65001

Date: 2026-08-09 12:27:48
*/


-- ----------------------------
-- Table structure for H_file_list
-- ----------------------------
DROP TABLE [dbo].[H_file_list]
GO
CREATE TABLE [dbo].[H_file_list] (
[file_name] varchar(30) NOT NULL ,
[solver_name] varchar(20) NULL ,
[solve_status] varchar(15) NULL ,
[start_date] date NULL ,
[end_date] date NULL ,
[solver_comments] varchar(255) NULL ,
[pap_code] varchar(6) NULL ,
[insert_date] datetime NULL ,
[rows] int NULL ,
[corrected_rows] int NULL ,
[master_file] varchar(255) NULL 
)


GO

-- ----------------------------
-- Records of H_file_list
-- ----------------------------
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'1', null, null, null, null, null, null, null, null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B01111H1A.txt', N'giash', N'SOLVED', N'2026-08-03', N'2026-08-03', null, N'B01111', N'2026-08-03 09:50:00.670', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B01111H1B.txt', N'giash', N'SOLVED', N'2026-08-03', N'2026-08-03', null, N'B01111', N'2026-08-03 09:44:03.823', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B0139H1A.txt', N'giash', N'SOLVED', N'2026-08-02', N'2026-08-02', null, N'B0139H', N'2026-07-30 16:11:41.627', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B0141H1A.txt', N'giash', N'SOLVED', N'2026-08-02', N'2026-08-02', null, N'B0141H', N'2026-07-30 16:08:04.880', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B0143H1A.txt', N'giash', N'SOLVED', N'2026-08-02', N'2026-08-02', null, N'B0143H', N'2026-07-30 16:08:14.910', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B0145H1A.txt', N'giash', N'SOLVED', N'2026-08-02', N'2026-08-02', null, N'B0145H', N'2026-07-30 16:08:24.083', null, null, null)
GO
GO
INSERT INTO [dbo].[H_file_list] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows], [master_file]) VALUES (N'B0147H1A.txt', N'giash', N'SOLVED', N'2026-08-02', N'2026-08-02', null, N'B0147H', N'2026-07-30 16:08:35.193', null, null, null)
GO
GO

-- ----------------------------
-- Table structure for H_solving
-- ----------------------------
DROP TABLE [dbo].[H_solving]
GO
CREATE TABLE [dbo].[H_solving] (
[id] bigint NOT NULL IDENTITY(1,1) ,
[file_name] varchar(30) NOT NULL ,
[scanned_row] varchar(160) NOT NULL ,
[serial] int NULL ,
[err_dscr] varchar(100) NULL ,
[exam_code] char(4) NULL ,
[pap_code] varchar(6) NULL ,
[bdexcode] char(32) NULL ,
[bhexcode] char(32) NULL ,
[qr_code] varchar(28) NULL ,
[solver] varchar(15) NULL ,
[updates] varchar(1024) NOT NULL ,
[last_update] datetime NULL ,
[binary] varchar(32) NULL ,
[marks] varchar(3) NULL ,
[hmarks] varchar(3) NULL ,
[eb_no] varchar(6) NULL ,
[sl_no] varchar(3) NULL ,
[comments] varchar(50) NULL ,
[hexcodea] varchar(8) NULL ,
[hexcodeb] varchar(8) NULL ,
[reg_no] varchar(256) NULL ,
[Ehex_no] varchar(8) NULL ,
[qr1] varchar(18) NULL ,
[qr2] varchar(18) NULL ,
[qr3] varchar(18) NULL ,
[form_sl] varchar(11) NULL ,
[hex1] varchar(8) NULL ,
[hex2] varchar(8) NULL ,
[hex3] varchar(8) NULL ,
[LU_status] varchar(11) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[H_solving]', RESEED, 718198)
GO

-- ----------------------------
-- Table structure for file_list
-- ----------------------------
DROP TABLE [dbo].[file_list]
GO
CREATE TABLE [dbo].[file_list] (
[file_name] varchar(30) NOT NULL ,
[solver_name] varchar(20) NULL ,
[solve_status] varchar(15) NULL ,
[start_date] date NULL ,
[end_date] date NULL ,
[solver_comments] nchar(255) NULL ,
[pap_code] varchar(6) NULL ,
[insert_date] date NULL ,
[rows] int NULL ,
[corrected_rows] int NULL 
)


GO

-- ----------------------------
-- Records of file_list
-- ----------------------------

-- ----------------------------
-- Table structure for file_list_other
-- ----------------------------
DROP TABLE [dbo].[file_list_other]
GO
CREATE TABLE [dbo].[file_list_other] (
[file_name] varchar(30) NOT NULL ,
[solver_name] varchar(20) NULL ,
[solve_status] varchar(15) NULL ,
[start_date] date NULL ,
[end_date] date NULL ,
[solver_comments] nchar(255) NULL ,
[pap_code] varchar(6) NULL ,
[insert_date] date NULL ,
[rows] int NULL ,
[corrected_rows] int NULL 
)


GO

-- ----------------------------
-- Records of file_list_other
-- ----------------------------
INSERT INTO [dbo].[file_list_other] ([file_name], [solver_name], [solve_status], [start_date], [end_date], [solver_comments], [pap_code], [insert_date], [rows], [corrected_rows]) VALUES (N'820511.txt', null, null, null, null, null, N'820511', N'2022-01-26', null, null)
GO
GO

-- ----------------------------
-- Table structure for E_solving
-- ----------------------------
DROP TABLE [dbo].[E_solving]
GO
CREATE TABLE [dbo].[E_solving] (
[id] int NOT NULL IDENTITY(1,1) ,
[file_name] varchar(30) NOT NULL ,
[scanned_row] nvarchar(255) NOT NULL ,
[serial] nvarchar(255) NULL ,
[err_dscr] nvarchar(255) NULL ,
[exam_code] nvarchar(255) NULL ,
[pap_code] nvarchar(255) NULL ,
[regi] nvarchar(255) NULL ,
[litho_1] nvarchar(255) NULL ,
[litho_2] nvarchar(255) NULL ,
[qr_code] nvarchar(255) NULL ,
[solver] nvarchar(255) NULL ,
[updates] nvarchar(255) NULL ,
[last_update] nvarchar(255) NULL ,
[scrpt_no] nvarchar(255) NULL ,
[hexcodea] nvarchar(255) NULL ,
[hexcodeb] nvarchar(255) NULL ,
[htthex] nvarchar(255) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[E_solving]', RESEED, 120007)
GO

-- ----------------------------
-- Records of E_solving
-- ----------------------------
SET IDENTITY_INSERT [dbo].[E_solving] ON
GO
SET IDENTITY_INSERT [dbo].[E_solving] OFF
GO

-- ----------------------------
-- Table structure for p1syllabas1314_med
-- ----------------------------
DROP TABLE [dbo].[p1syllabas1314_med]
GO
CREATE TABLE [dbo].[p1syllabas1314_med] (
[PAP_CODE] varchar(10) NOT NULL ,
[type] varchar(2) NULL ,
[semester] varchar(1) NULL ,
[course_code] varchar(255) NULL 
)


GO

-- ----------------------------
-- Records of p1syllabas1314_med
-- ----------------------------
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000320', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000322', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000323', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000324', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000326', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'000327', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810301', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810303', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810305', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810307', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810309', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810311', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810313', null, N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810314', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810315', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810317', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810319', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810321', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810323', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810325', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810327', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810329', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810331', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810333', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810335', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810337', N'T', N'2', N'566')
GO
GO
INSERT INTO [dbo].[p1syllabas1314_med] ([PAP_CODE], [type], [semester], [course_code]) VALUES (N'810338', null, N'2', N'566')
GO
GO

-- ----------------------------
-- Table structure for p1syllabas1314
-- ----------------------------
DROP TABLE [dbo].[p1syllabas1314]
GO
CREATE TABLE [dbo].[p1syllabas1314] (
[PAP_CODE] varchar(10) NOT NULL ,
[type] varchar(2) NULL ,
[semester] varchar(1) NULL ,
[COURSE_CODE] varchar(255) NULL 
)


GO

-- ----------------------------
-- Records of p1syllabas1314
-- ----------------------------
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1101', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1102', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1103', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1104', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1105', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1201', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1202', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1203', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1204', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'1205', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2101', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2102', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2103', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2104', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2105', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'211501', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2201', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2202', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2203', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2204', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'2205', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3101', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3102', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3103', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3104', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3105', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3201', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3202', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3203', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3204', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'3205', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4101', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4102', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4103', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4104', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4105', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4231', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4232', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4233', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4234', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4235', N'V', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4241', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4242', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4243', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4244', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4245', N'V', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4251', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4252', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4253', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4254', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4255', N'V', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4261', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4262', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4263', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4264', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'4265', N'V', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510101', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510103', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510105', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510107', N'T', N'1', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510109', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510111', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510113', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510115', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510117', N'T', N'2', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510301', N'T', N'1', N'5681')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510303', N'T', N'1', N'5681')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510305', N'T', N'1', N'5681')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510307', N'T', N'1', N'5681')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510309', N'T', N'2', N'5682')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510311', N'T', N'2', N'5682')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510313', N'T', N'2', N'5682')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510315', N'T', N'2', N'5682')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510501', N'T', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510511', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510512', N'P', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510513', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510514', N'P', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510515', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510516', N'P', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510701', N'T', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510702', N'P', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510801', N'T', N'1', N'5621')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510803', N'T', N'1', N'5621')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510805', N'T', N'1', N'5621')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510807', N'T', N'1', N'5621')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'510809', N'T', N'1', N'5621')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'511101', N'T', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'511111', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'511501', N'T', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'512211', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'512711', N'T', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'512801', N'T', N'1', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'515312', N'P', N'2', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520119', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520121', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520123', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520125', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520127', N'T', N'3', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520129', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520131', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520133', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520135', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520137', N'T', N'4', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520301', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520303', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520305', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520307', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520309', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520311', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520313', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520315', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520317', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520319', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520321', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520323', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520325', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520327', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520329', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520331', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520333', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520335', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520337', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520339', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520341', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520343', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520345', N'T', N'3', N'5683')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520347', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520349', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520351', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520353', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520355', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520357', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520359', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520361', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520363', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520365', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520367', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520369', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520371', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520373', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520375', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520377', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520379', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520381', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520383', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520385', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520387', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520389', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520391', N'T', N'4', N'5684')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520525', N'T', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520526', N'P', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520528', N'P', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520530', N'P', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520532', N'P', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520543', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520544', N'P', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520545', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520547', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520551', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520724', N'P', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'520744', N'P', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'521121', N'T', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'521131', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'522301', N'T', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'522501', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'523601', N'T', N'4', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'523701', N'T', N'3', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530139', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530141', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530143', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530145', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530147', N'T', N'5', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530149', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530151', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530153', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530155', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530157', N'T', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530158', N'V', N'6', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530301', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530303', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530305', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530307', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530309', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530311', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530313', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530315', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530317', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530319', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530321', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530323', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530325', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530327', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530329', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530331', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530333', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530335', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530337', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530339', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530341', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530343', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530345', N'T', N'5', N'5685')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530347', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530349', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530351', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530353', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530355', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530357', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530359', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530361', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530363', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530365', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530367', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530369', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530371', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530373', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530375', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530377', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530379', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530381', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530383', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530385', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530387', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530389', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530391', N'T', N'6', N'5686')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530563', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530565', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530567', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530568', N'P', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530569', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530571', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530573', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530575', N'P', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530578', N'P', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530579', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530581', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530583', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530585', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530587', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530609', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530622', N'P', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530623', N'T', N'5', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530772', N'P', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'530774', N'T', N'6', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540311', N'T', N'8', N'5688')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540313', N'T', N'8', N'5688')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540315', N'T', N'8', N'5688')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540317', N'T', N'8', N'5688')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540319', N'T', N'8', N'5688')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540589', N'T', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540591', N'T', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540593', N'T', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540595', N'T', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540597', N'T', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540598', N'P', N'8', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540780', N'P', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540782', N'P', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'540784', N'P', N'7', N'505')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542301', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542303', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542305', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542307', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542309', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542311', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542313', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542315', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542317', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542319', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542320', N'I', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542401', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542403', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542405', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542407', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542409', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542411', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542413', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542415', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542417', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542419', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542420', N'I', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542501', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542503', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542505', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542507', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542509', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542511', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542513', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542515', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542517', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542519', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542520', N'I', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542601', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542603', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542605', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542607', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542609', N'T', N'7', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542611', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542613', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542615', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542617', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542619', N'T', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'542620', N'I', N'8', null)
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810301', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810303', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810305', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810307', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810309', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810311', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810313', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810315', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810317', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810319', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810321', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810323', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810325', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810327', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810329', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810331', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810333', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810335', N'T', N'1', N'8772')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810337', N'T', N'2', N'8773')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810501', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810503', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810505', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810507', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810509', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810511', N'T', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'810513', N'P', N'1', N'555')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'816601', N'T', N'1', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'816603', N'T', N'1', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'816605', N'T', N'1', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'816607', N'T', N'1', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'816609', N'T', N'1', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'820511', N'T', N'2', N'556')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'820513', N'T', N'2', N'556')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'820515', N'T', N'2', N'556')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'820517', N'T', N'2', N'556')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'826611', N'T', N'2', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'826613', N'T', N'2', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'826615', N'T', N'2', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'826617', N'T', N'2', N'8141')
GO
GO
INSERT INTO [dbo].[p1syllabas1314] ([PAP_CODE], [type], [semester], [COURSE_CODE]) VALUES (N'826618', N'T', N'2', N'8141')
GO
GO

-- ----------------------------
-- Table structure for sysdiagrams
-- ----------------------------
DROP TABLE [dbo].[sysdiagrams]
GO
CREATE TABLE [dbo].[sysdiagrams] (
[name] sysname NOT NULL ,
[principal_id] int NOT NULL ,
[diagram_id] int NOT NULL IDENTITY(1,1) ,
[version] int NULL ,
[definition] varbinary(MAX) NULL 
)


GO

-- ----------------------------
-- Records of sysdiagrams
-- ----------------------------
SET IDENTITY_INSERT [dbo].[sysdiagrams] ON
GO
SET IDENTITY_INSERT [dbo].[sysdiagrams] OFF
GO

-- ----------------------------
-- Table structure for solvers
-- ----------------------------
DROP TABLE [dbo].[solvers]
GO
CREATE TABLE [dbo].[solvers] (
[solver_name] varchar(20) NOT NULL ,
[passkey] varchar(50) NOT NULL ,
[status] varchar(10) NOT NULL ,
[full_name] varchar(60) NULL 
)


GO

-- ----------------------------
-- Records of solvers
-- ----------------------------
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'__NONE', N'__NONE', N'Unassigned', N'')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'amin', N'123', N'solver', null)
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'arshed', N'841433', N'solver', N'Arshed')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'biswas', N'p123', N'solver', N'Prodip Kumar Biswas')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'esrafil', N'esrafil', N'solver', N'H M Israfil')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'faruk', N'comilla', N'solver', N'Dr. Faruk')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'jakir', N'32044', N'solver', N'Md. Jakir Mukswd')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'kafil', N'cof', N'solver', N'Kafil Uddin Mahmud Mojumder')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'kazal', N'465481km', N'solver', N'Kazal Kumar Roy ')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'mgias', N'123', N'solver', N'Md. Giyas Uddin')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'mostafa', N'dulal', N'solver', N'Mohammad Mostafa')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'Najmul', N'3325', N'solver', N'Najmul  Huda')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'nasir', N'841432', N'solver', N'Nasir Mia')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'pbarai', N'Pbarai', N'solver', N'Prodip Barai')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'pro', N'456', N'solver', N'proholad roy')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'sazzad', N'`12', N'solver', N'')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'shamim', N'123', N'solver', null)
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'shipra', N'rani', N'solver', N'Shipra Rani Das')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'm', N'm', N'solver', N'mr')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'a', N'a', N'uploader', N'Mridul Roy')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'Sweety', N'Sweety', N'solver', N'Sweety Rani Das')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'giash', N'1234', N'solver', N'Gias sir')
GO
GO
INSERT INTO [dbo].[solvers] ([solver_name], [passkey], [status], [full_name]) VALUES (N'rakib', N'1234', N'solver', N'rakib')
GO
GO

-- ----------------------------
-- Indexes structure for table H_file_list
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table H_file_list
-- ----------------------------
ALTER TABLE [dbo].[H_file_list] ADD PRIMARY KEY ([file_name])
GO

-- ----------------------------
-- Indexes structure for table H_solving
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table H_solving
-- ----------------------------
ALTER TABLE [dbo].[H_solving] ADD PRIMARY KEY ([id])
GO

-- ----------------------------
-- Indexes structure for table reject_entry
-- ----------------------------
CREATE UNIQUE INDEX [UQ_reject_entry_qr] ON [dbo].[reject_entry]
([qr_bar_code] ASC) 
WHERE ([qr_bar_code] IS NOT NULL AND [qr_bar_code]<>'')
WITH (IGNORE_DUP_KEY = ON)
GO

-- ----------------------------
-- Indexes structure for table file_list
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table file_list
-- ----------------------------
ALTER TABLE [dbo].[file_list] ADD PRIMARY KEY ([file_name])
GO

-- ----------------------------
-- Indexes structure for table file_list_other
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table file_list_other
-- ----------------------------
ALTER TABLE [dbo].[file_list_other] ADD PRIMARY KEY ([file_name])
GO

-- ----------------------------
-- Indexes structure for table E_solving
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table E_solving
-- ----------------------------
ALTER TABLE [dbo].[E_solving] ADD PRIMARY KEY ([id])
GO

-- ----------------------------
-- Indexes structure for table p1syllabas1314_med
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table p1syllabas1314_med
-- ----------------------------
ALTER TABLE [dbo].[p1syllabas1314_med] ADD PRIMARY KEY ([PAP_CODE])
GO

-- ----------------------------
-- Indexes structure for table p1syllabas1314
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table p1syllabas1314
-- ----------------------------
ALTER TABLE [dbo].[p1syllabas1314] ADD PRIMARY KEY ([PAP_CODE])
GO

-- ----------------------------
-- Indexes structure for table sysdiagrams
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table sysdiagrams
-- ----------------------------
ALTER TABLE [dbo].[sysdiagrams] ADD PRIMARY KEY ([diagram_id])
GO

-- ----------------------------
-- Uniques structure for table sysdiagrams
-- ----------------------------
ALTER TABLE [dbo].[sysdiagrams] ADD UNIQUE ([principal_id] ASC, [name] ASC)
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[H_solving]
-- ----------------------------
ALTER TABLE [dbo].[H_solving] ADD FOREIGN KEY ([file_name]) REFERENCES [dbo].[H_file_list] ([file_name]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[E_solving]
-- ----------------------------
ALTER TABLE [dbo].[E_solving] ADD FOREIGN KEY ([file_name]) REFERENCES [dbo].[file_list] ([file_name]) ON DELETE CASCADE ON UPDATE CASCADE
GO
