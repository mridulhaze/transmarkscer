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

Date: 2026-09-20 16:10:21
*/


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
-- Records of bedhons_courses
-- ----------------------------
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510301', N'History of The Emergence of Independent Bangladesh', N'1', N'4', N'100', N'T', N'681', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510303', N'Bangla-1', N'1', N'4', N'100', N'T', N'681', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510305', N'English-1', N'1', N'4', N'100', N'T', N'681', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510307', N'Introduction to Education', N'1', N'4', N'100', N'T', N'681', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510309', N'BANGLA-2', N'2', N'4', N'100', N'T', N'682', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510311', N'ENGLISH-2', N'2', N'4', N'100', N'T', N'682', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510313', N'EDUCATION IN BANGLADESH', N'2', N'4', N'100', N'T', N'682', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510315', N'FOUNDATIONS OF EDUCATION', N'2', N'4', N'100', N'T', N'682', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'510901', N'Comprehensive Viva (Satisfactory/non-satisfactory)', N'2', N'0', N'0', N'P', N'682', N'0', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520301', N'ICT in Education', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520303', N'Educational Psychology and Guidance', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520305', N'Bangla Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520307', N'English Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520309', N'ICT  Education Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520311', N'Economics Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520313', N'Political Science Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520315', N'Sociology Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520317', N'Geography and Environment Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520319', N'History Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520321', N'Islamic History and Culture Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520323', N'Physics Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520325', N'Chemistry Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520327', N'Mathematics Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520329', N'Botany Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520331', N'Zoology Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520333', N'Statistics Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520335', N'Accounting Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520337', N'Management Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520339', N'Marketing Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520341', N'Finance and Banking Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520343', N'Information Science and Library Management Paper-1', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520345', N'Home Economics Paper-1 Theory+Practical', N'3', N'4', N'100', N'T', N'683', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520347', N'Organization and Management of Educational Institutions', N'4', N'4', N'100', N'T', N'684', N'40', N'compulsory ', null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520349', N'Gender Education', N'4', N'4', N'100', N'T', N'684', N'40', N'compulsory ', null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520351', N'Bangla Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520353', N'English Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520355', N'ICT Education Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520357', N'Economics Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520359', N'Political Science Paper-2:Political Organizations and Systems', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520361', N'Sociology Paper-2:Sociological Theories', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520363', N'Geography and Environment Paper-2:Economic and Human Geography', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520365', N'History Paper-2:History of Bangladesh', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520367', N'Islamic History and Culture Paper-2:History of Muslim in India upto Impgence of Independent Bangladesh(712-1197 AD)', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520369', N'Physics Paper-2:Heat:Thermodynamics and Optics', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520371', N'Chemistry Paper-2:Organic Chemistry', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520373', N'Mathematics paper-2:Calculus and Coordinate Geometry', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520375', N'Botany Paper-2:Physiology:Limnology:Lichens:Cryptogams:Gymnosperms:Palaeobotany and Anatomy', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520377', N'Zoology Paper-2:Comparative Anatomy', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520379', N'Statistics Paper-2:Methods of Statistics', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520381', N'Accounting paper-2:Banking and Insurance', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520383', N'Management Paper-2:Fundamentals of Management', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520385', N'Marketing Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520387', N'Finance and Banking Paper-2:Banking and Insurance', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520389', N'Information Science and Library Management Paper-2:Organization of Knowledge(Classification & Cataloging)', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520391', N'Home Economics Paper-2', N'4', N'4', N'100', N'T', N'684', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'520392', N'Comprehensive Viva (Satisfactory/non-satisfactory)', N'4', N'0', N'0', N'P', N'684', N'0', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530301', N'Teaching Learning Methods and Strategies', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530303', N'Assessment and Evaluation and Education', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530305', N'Bangla:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530307', N'English:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530309', N'ICT Education :Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530311', N'Economics:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530313', N'Political Science:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530315', N'Sociology:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530317', N'Geography and Environment:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530319', N'History Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530321', N'Islamic History and Culture:paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530323', N'Physics:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530325', N'Chemistry:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530327', N'Mathematics:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530329', N'Botany :Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530331', N'Zoology:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530333', N'Statistics:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530335', N'Accounting:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530337', N'Management:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530339', N'Marketing:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530341', N'Finance and Banking:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530343', N'Information Science and Library Management Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530345', N'Home Ecomics:Paper-3', N'5', N'4', N'100', N'T', N'685', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530347', N'Teaching Methods:Micro Teaching and Simulation', N'6', N'4', N'100', N'T', N'686', N'40', N'compulsory ', null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530349', N'Introduction to Curriculum', N'6', N'4', N'100', N'T', N'686', N'40', N'compulsory ', null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530351', N'Bangla:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A1', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530353', N'English:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530355', N'ICT Education :Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A1:A2:A3:A4:A5', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530357', N'Economics:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530359', N'Political Science:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530361', N'Sociology:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530363', N'Geography and Environment:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2:A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530365', N'History Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530367', N'Islamic History and Culture:paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A2', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530369', N'Physics:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530371', N'Chemistry:Paper-4', N'6', N'4', N'100', N'TP', N'686', null, null, N'A3', N'25', N'75')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530373', N'Mathematics:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530375', N'Botany :Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530377', N'Zoology:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530379', N'Statistics:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A3', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530381', N'Accounting:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A4', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530383', N'Management:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A4', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530385', N'Marketing:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A4', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530387', N'Finance and Banking:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A4', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530389', N'Information Science and Library Management Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A5', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530391', N'Home Ecomics:Paper-4', N'6', N'4', N'100', N'T', N'686', N'40', null, N'A5', null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'530392', N'Comprehensive Viva (Satisfactory/non-satisfactory)', N'6', N'0', N'0', N'P', N'686', N'0', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540302', N'Teaching Subject-1', N'7', N'6', N'150', N'TP', N'687', N'150', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540304', N'Teaching Subject-2', N'7', N'6', N'150', N'TP', N'687', N'150', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540306', N'Report Writing', N'7', N'2', N'50', N'TP', N'687', N'50', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540308', N'Final Practice Teaching(Two Subjects)', N'7', N'4', N'100', N'TP', N'687', N'100', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540310', N'Viva Voice', N'7', N'2', N'50', N'TP', N'687', N'50', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540311', N'Introduction to Educational Research', N'8', N'4', N'100', N'T', N'688', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540313', N'Environment Education', N'8', N'4', N'100', N'T', N'688', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540315', N'Non-Formal and Continuing Education', N'8', N'4', N'100', N'T', N'688', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540317', N'Inclusive Education', N'8', N'4', N'100', N'T', N'688', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540319', N'Education and Development', N'8', N'4', N'100', N'T', N'688', N'40', null, null, null, N'60')
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540320', N'Comprehensive Final MCQ Exam', N'8', N'0', N'100', N'P', N'688', N'0', null, null, null, null)
GO
GO
INSERT INTO [dbo].[bedhons_courses] ([course_code], [course_name], [semester], [credits], [full_marks], [type], [exm_code], [inc_mark], [sub_type], [sub_area], [prac_mark], [theo_mark]) VALUES (N'540322', N'Viva Voce(Satisfactory/non-satisfactory)', N'8', N'0', N'0', N'P', N'688', N'0', null, null, null, null)
GO
GO

-- ----------------------------
-- Indexes structure for table bedhons_courses
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table bedhons_courses
-- ----------------------------
ALTER TABLE [dbo].[bedhons_courses] ADD PRIMARY KEY ([course_code])
GO
