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

Date: 2026-09-20 15:34:10
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
[master_file] varchar(255) NULL ,
[exm_code] varchar(6) NULL ,
[exm_year] varchar(4) NULL 
)


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
[LU_status] varchar(11) NULL ,
[exm_year] varchar(4) NULL ,
[exm_code] varchar(10) NULL 
)


GO
DBCC CHECKIDENT(N'[dbo].[H_solving]', RESEED, 842096)
GO

-- ----------------------------
-- Table structure for reject_entry
-- ----------------------------
DROP TABLE [dbo].[reject_entry]
GO
CREATE TABLE [dbo].[reject_entry] (
[qr_bar_code] varchar(255) NULL ,
[course_code] varchar(255) NULL ,
[TIMS_EB_CODE] varchar(255) NULL ,
[Serial_no] int NULL ,
[mark1] varchar(255) NULL ,
[mark2] varchar(255) NULL ,
[litho1] varchar(255) NULL ,
[litho2] varchar(255) NULL ,
[id] int NULL ,
[created_at] varchar(255) NULL ,
[saved_by] varchar(255) NULL ,
[course_name] varchar(255) NULL ,
[year] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for reject_entry_courses
-- ----------------------------
DROP TABLE [dbo].[reject_entry_courses]
GO
CREATE TABLE [dbo].[reject_entry_courses] (
[course_name] varchar(255) NULL ,
[user] varchar(255) NULL ,
[year] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for absent
-- ----------------------------
DROP TABLE [dbo].[absent]
GO
CREATE TABLE [dbo].[absent] (
[reg_no] nvarchar(255) NULL ,
[col_code] nvarchar(255) NULL ,
[pap_code] nvarchar(255) NULL ,
[script_no] nvarchar(255) NULL ,
[hexcodea] varchar(255) NULL ,
[hexcodeb] varchar(255) NULL ,
[mark] varchar(10) NULL ,
[status] varchar(10) NULL ,
[KEY] varchar(256) NULL ,
[SEMESTER] varchar(255) NULL ,
[YEAR] varchar(255) NULL ,
[EXM_NAME] varchar(255) NULL ,
[qr1] varchar(18) NULL ,
[qr2] varchar(18) NULL ,
[qr3] varchar(18) NULL ,
[hex1] varchar(8) NULL ,
[hex2] varchar(8) NULL ,
[hex3] varchar(8) NULL ,
[form_sl] varchar(11) NULL ,
[LU_status] varchar(11) NULL ,
[e_hex] varchar(255) NULL ,
[h_hex] varchar(255) NULL ,
[pstatus] varchar(255) NULL ,
[hmark] varchar(10) NULL ,
[sl_no] int NULL ,
[eb_no] int NULL ,
[c_code] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for absent_Entry
-- ----------------------------
DROP TABLE [dbo].[absent_Entry]
GO
CREATE TABLE [dbo].[absent_Entry] (
[reg_no] nvarchar(255) NULL ,
[col_code] nvarchar(255) NULL ,
[pap_code] nvarchar(255) NULL ,
[script_no] nvarchar(255) NULL ,
[hexcodea] varchar(255) NULL ,
[hexcodeb] varchar(255) NULL ,
[mark] varchar(10) NULL ,
[status] varchar(10) NULL ,
[KEY] varchar(256) NULL ,
[SEMESTER] varchar(255) NULL ,
[YEAR] varchar(255) NULL ,
[EXM_NAME] varchar(255) NULL ,
[qr1] varchar(18) NULL ,
[qr2] varchar(18) NULL ,
[qr3] varchar(18) NULL ,
[hex1] varchar(8) NULL ,
[hex2] varchar(8) NULL ,
[hex3] varchar(8) NULL ,
[form_sl] varchar(11) NULL ,
[LU_status] varchar(11) NULL ,
[e_hex] varchar(255) NULL ,
[h_hex] varchar(255) NULL ,
[pstatus] varchar(255) NULL ,
[hmark] varchar(10) NULL ,
[sl_no] int NULL ,
[eb_no] int NULL ,
[c_code] varchar(255) NULL 
)


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
