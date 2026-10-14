/*
Navicat SQL Server Data Transfer

Source Server         : SQL
Source Server Version : 160000
Source Host           : DESKTOP-VF08655:1433
Source Database       : unipro
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 160000
File Encoding         : 65001

Date: 2026-09-21 11:07:07
*/


-- ----------------------------
-- Table structure for mcq
-- ----------------------------
DROP TABLE [dbo].[mcq]
GO
CREATE TABLE [dbo].[mcq] (
[sl] nvarchar(255) NULL ,
[filename] nvarchar(255) NULL ,
[col_code] nvarchar(255) NULL ,
[reg_no] nvarchar(255) NULL ,
[year] nvarchar(255) NULL ,
[semester] nvarchar(255) NULL ,
[exm_year] nvarchar(255) NULL ,
[course_code] nvarchar(255) NULL ,
[mca1] nvarchar(255) NULL ,
[mca2] nvarchar(255) NULL ,
[mca3] nvarchar(255) NULL ,
[mca4] nvarchar(255) NULL ,
[mca5] nvarchar(255) NULL ,
[mca6] nvarchar(255) NULL ,
[mca7] nvarchar(255) NULL ,
[mca8] nvarchar(255) NULL ,
[mca9] nvarchar(255) NULL ,
[mca10] nvarchar(255) NULL ,
[mca11] nvarchar(255) NULL ,
[mca12] nvarchar(255) NULL ,
[mca13] nvarchar(255) NULL ,
[mca14] nvarchar(255) NULL ,
[mca15] nvarchar(255) NULL ,
[mca16] nvarchar(255) NULL ,
[mca17] nvarchar(255) NULL ,
[mca18] nvarchar(255) NULL ,
[mca19] nvarchar(255) NULL ,
[mca20] nvarchar(255) NULL ,
[mark] nvarchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for H_solving_bedhons_5s_24_lot2
-- ----------------------------
DROP TABLE [dbo].[H_solving_bedhons_5s_24_lot2]
GO
CREATE TABLE [dbo].[H_solving_bedhons_5s_24_lot2] (
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
DBCC CHECKIDENT(N'[dbo].[H_solving_bedhons_5s_24_lot2]', RESEED, 842096)
GO

-- ----------------------------
-- Table structure for mtemp
-- ----------------------------
DROP TABLE [dbo].[mtemp]
GO
CREATE TABLE [dbo].[mtemp] (
[reg_no] varchar(255) NULL ,
[exm_roll] varchar(255) NULL ,
[session] varchar(513) NULL ,
[std_type] varchar(255) NULL ,
[semester] varchar(1) NULL ,
[year] varchar(4) NULL ,
[sub_code] varchar(5) NULL ,
[c_code] varchar(255) NULL ,
[center] varchar(255) NULL ,
[pap_code] varchar(255) NULL ,
[col_name] varchar(255) NULL ,
[col_code] varchar(255) NULL ,
[mcq_marks] smallint NULL ,
[incourse_marks] smallint NULL ,
[marks1] smallint NULL ,
[marks2] varchar(3) NULL ,
[mcq_inc_total] smallint NULL ,
[final_marks] smallint NULL ,
[grade] varchar(2) NULL ,
[grade_point] real NULL ,
[sgpa] varchar(4) NULL ,
[status] varchar(20) NULL ,
[total_marks] varchar(10) NULL ,
[eb_no] varchar(10) NULL ,
[sl_no] varchar(10) NULL ,
[hexcodea] varchar(8) NULL ,
[hexcodeb] varchar(8) NULL ,
[script_no] varchar(32) NULL ,
[exm_code] varchar(10) NULL ,
[id] int NULL ,
[inc_mark] varchar(255) NULL ,
[total_sub] varchar(255) NULL ,
[ems_status] varchar(255) NULL ,
[Ehex_no] varchar(8) NULL ,
[qr1] varchar(255) NULL ,
[qr2] varchar(255) NULL ,
[qr3] varchar(255) NULL ,
[form_sl] varchar(11) NULL ,
[sub_type] varchar(20) NULL ,
[ems_present_status] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for BEDHONS_5685_ff_split_data_24_NEW
-- ----------------------------
DROP TABLE [dbo].[BEDHONS_5685_ff_split_data_24_NEW]
GO
CREATE TABLE [dbo].[BEDHONS_5685_ff_split_data_24_NEW] (
[reg_no] varchar(255) NULL ,
[inc_mark] varchar(255) NULL ,
[sif_sub] varchar(255) NULL ,
[std_type] varchar(1) NULL 
)


GO

-- ----------------------------
-- Table structure for examinee_course
-- ----------------------------
DROP TABLE [dbo].[examinee_course]
GO
CREATE TABLE [dbo].[examinee_course] (
[exm_roll] varchar(11) NOT NULL ,
[semester] varchar(1) NOT NULL ,
[type] varchar(1) NOT NULL ,
[exm_year] varchar(10) NOT NULL ,
[course_code] varchar(6) NOT NULL ,
[incourse_marks] smallint NULL ,
[final_marks] smallint NULL ,
[mcq_marks] smallint NULL ,
[total_marks] varchar(50) NULL ,
[grade] varchar(2) NULL ,
[grade_point] float(53) NULL ,
[credits] float(53) NULL ,
[course_point] float(53) NULL ,
[status] varchar(100) NULL ,
[date] datetime2(7) NULL ,
[satisfactory] varchar(2) NULL ,
[reg_no] varchar(11) NULL ,
[session] varchar(20) NULL ,
[col_code] varchar(20) NULL ,
[ff_status] varchar(20) NULL ,
[gender] varchar(255) NULL ,
[course_name] varchar(255) NULL ,
[total_check] varchar(255) NULL ,
[missmatch] varchar(255) NULL ,
[update_to_EC] int NULL ,
[ready_to_publish] varchar(1) NULL 
)


GO

-- ----------------------------
-- Table structure for mtemp_format
-- ----------------------------
DROP TABLE [dbo].[mtemp_format]
GO
CREATE TABLE [dbo].[mtemp_format] (
[reg_no] varchar(255) NULL ,
[exm_roll] varchar(255) NULL ,
[session] varchar(513) NULL ,
[std_type] varchar(255) NULL ,
[semester] varchar(1) NULL ,
[year] varchar(4) NULL ,
[sub_code] varchar(5) NULL ,
[c_code] varchar(255) NULL ,
[center] varchar(255) NULL ,
[pap_code] varchar(255) NULL ,
[col_name] varchar(255) NULL ,
[col_code] varchar(255) NULL ,
[mcq_marks] smallint NULL ,
[incourse_marks] smallint NULL ,
[marks1] smallint NULL ,
[marks2] varchar(3) NULL ,
[mcq_inc_total] smallint NULL ,
[final_marks] smallint NULL ,
[grade] varchar(2) NULL ,
[grade_point] real NULL ,
[sgpa] varchar(4) NULL ,
[status] varchar(20) NULL ,
[total_marks] varchar(10) NULL ,
[eb_no] varchar(10) NULL ,
[sl_no] varchar(10) NULL ,
[hexcodea] varchar(8) NULL ,
[hexcodeb] varchar(8) NULL ,
[script_no] varchar(32) NULL ,
[exm_code] varchar(10) NULL ,
[id] int NULL ,
[inc_mark] varchar(255) NULL ,
[total_sub] varchar(255) NULL ,
[ems_status] varchar(255) NULL ,
[Ehex_no] varchar(8) NULL ,
[qr1] varchar(255) NULL ,
[qr2] varchar(255) NULL ,
[qr3] varchar(255) NULL ,
[form_sl] varchar(11) NULL ,
[sub_type] varchar(20) NULL ,
[ems_present_status] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for bedhons_courses
-- ----------------------------
DROP TABLE [dbo].[bedhons_courses]
GO
CREATE TABLE [dbo].[bedhons_courses] (
[course_code] varchar(6) NOT NULL ,
[course_name] varchar(200) NULL ,
[semester] varchar(1) NULL ,
[credits] float(53) NULL ,
[full_marks] float(53) NULL ,
[type] varchar(4) NULL ,
[exm_code] varchar(5) NULL ,
[inc_mark] varchar(4) NULL ,
[sub_type] varchar(20) NULL ,
[sub_area] varchar(50) NULL ,
[prac_mark] varchar(4) NULL ,
[theo_mark] varchar(4) NULL 
)


GO

-- ----------------------------
-- Table structure for FF
-- ----------------------------
DROP TABLE [dbo].[FF]
GO
CREATE TABLE [dbo].[FF] (
[exm_code] nvarchar(255) NULL ,
[exm_year] nvarchar(255) NULL ,
[exam_roll_no] nvarchar(255) NULL ,
[degree] nvarchar(255) NULL ,
[course] nvarchar(255) NULL ,
[course_code] nvarchar(255) NULL ,
[sub_name] nvarchar(255) NULL ,
[sub_code] nvarchar(255) NULL ,
[reg_no] nvarchar(255) NULL ,
[adm_roll] nvarchar(255) NULL ,
[exm_roll] nvarchar(255) NULL ,
[sess1] nvarchar(255) NULL ,
[sess2] nvarchar(255) NULL ,
[ac_session] nvarchar(255) NULL ,
[reg_type] nvarchar(255) NULL ,
[std_type] nvarchar(255) NULL ,
[col_code] nvarchar(255) NULL ,
[col_name] nvarchar(255) NULL ,
[cen_code] nvarchar(255) NULL ,
[cen_col_code] nvarchar(255) NULL ,
[cen_name] nvarchar(255) NULL ,
[std_name] nvarchar(255) NULL ,
[fname] nvarchar(255) NULL ,
[mname] nvarchar(255) NULL ,
[pap_code] nvarchar(255) NULL ,
[inc_marks] nvarchar(255) NULL ,
[ict_sub] nvarchar(255) NULL ,
[thesis] nvarchar(255) NULL ,
[nu_fee] nvarchar(255) NULL ,
[con_status] nvarchar(255) NULL ,
[app_date] nvarchar(255) NULL ,
[con_date] nvarchar(255) NULL ,
[inc_mark] varchar(255) NULL ,
[total_sub] varchar(255) NULL 
)


GO

-- ----------------------------
-- Table structure for attandance
-- ----------------------------
DROP TABLE [dbo].[attandance]
GO
CREATE TABLE [dbo].[attandance] (
[exam_code] nvarchar(255) NULL ,
[exam_year] nvarchar(255) NULL ,
[att_date] nvarchar(255) NULL ,
[reg_no] nvarchar(255) NULL ,
[exm_roll] nvarchar(255) NULL ,
[cen_code] nvarchar(255) NULL ,
[paper_code] nvarchar(255) NULL ,
[script_qr] nvarchar(255) NULL ,
[script_sl] nvarchar(255) NULL 
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
DBCC CHECKIDENT(N'[dbo].[H_solving]', RESEED, 128)
GO

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
-- Indexes structure for table H_solving_bedhons_5s_24_lot2
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table H_solving_bedhons_5s_24_lot2
-- ----------------------------
ALTER TABLE [dbo].[H_solving_bedhons_5s_24_lot2] ADD PRIMARY KEY ([id])
GO

-- ----------------------------
-- Indexes structure for table mtemp
-- ----------------------------
CREATE INDEX [IX_mtemp_reg_pap] ON [dbo].[mtemp]
([reg_no] ASC, [pap_code] ASC) 
GO
CREATE INDEX [IX_mtemp_qr1] ON [dbo].[mtemp]
([qr1] ASC) 
GO

-- ----------------------------
-- Indexes structure for table examinee_course
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table examinee_course
-- ----------------------------
ALTER TABLE [dbo].[examinee_course] ADD PRIMARY KEY ([exm_roll], [semester], [type], [exm_year], [course_code])
GO

-- ----------------------------
-- Indexes structure for table mtemp_format
-- ----------------------------
CREATE INDEX [IX_mtemp_reg_pap] ON [dbo].[mtemp_format]
([reg_no] ASC, [pap_code] ASC) 
GO
CREATE INDEX [IX_mtemp_qr1] ON [dbo].[mtemp_format]
([qr1] ASC) 
GO

-- ----------------------------
-- Indexes structure for table bedhons_courses
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table bedhons_courses
-- ----------------------------
ALTER TABLE [dbo].[bedhons_courses] ADD PRIMARY KEY ([course_code])
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
-- Indexes structure for table E_solving
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table E_solving
-- ----------------------------
ALTER TABLE [dbo].[E_solving] ADD PRIMARY KEY ([id])
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
-- Indexes structure for table solvers
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table solvers
-- ----------------------------
ALTER TABLE [dbo].[solvers] ADD PRIMARY KEY ([solver_name])
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
-- Indexes structure for table H_solving
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table H_solving
-- ----------------------------
ALTER TABLE [dbo].[H_solving] ADD PRIMARY KEY ([id])
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
-- Indexes structure for table file_list_other
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table file_list_other
-- ----------------------------
ALTER TABLE [dbo].[file_list_other] ADD PRIMARY KEY ([file_name])
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[E_solving]
-- ----------------------------
ALTER TABLE [dbo].[E_solving] ADD FOREIGN KEY ([file_name]) REFERENCES [dbo].[file_list] ([file_name]) ON DELETE CASCADE ON UPDATE CASCADE
GO

-- ----------------------------
-- Foreign Key structure for table [dbo].[H_solving]
-- ----------------------------
ALTER TABLE [dbo].[H_solving] ADD FOREIGN KEY ([file_name]) REFERENCES [dbo].[H_file_list] ([file_name]) ON DELETE CASCADE ON UPDATE NO ACTION
GO
