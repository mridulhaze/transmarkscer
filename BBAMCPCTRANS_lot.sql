USE [m_ed]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[BBAMCPCTRANS_lot]
@reg_no   VARCHAR(11) = NULL,
@EXM_YEAR VARCHAR(4)
AS

BEGIN
--INSERT INTO MCPCMAIN
SELECT DISTINCT
	'8773' exm_code,
	--examinee.exm_year,
	dbo.GETMAXYEAR(examinee.reg_no) exm_year,
	examinee.reg_no,
	'' RESULT,
	cast(dbo.GETCGPA (examinee.reg_no)  as decimal(10,2)) as CGPA,
	SUBSTRING(std_rif.SESSION,3,2) SESS1,
	SUBSTRING(std_rif.SESSION,6,2) SESS2,
	--examinee.exm_roll,
EMS.exm_roll,
	'813' SUB_CODE,
	examinee.c_code,
	examinee.col_code,
	'' COURSE,
	dbo.GETMAXTYPE(examinee.reg_no) STD_TYPE,
	'44' T_CREDIT,
	dbo.GETTOTALCREDITS(examinee.reg_no,1,8) C_CREDIT,
-- need to write function
	'12' T_PAPER,
	dbo.GETTOTALPAP(examinee.reg_no,1,8)	C_PAPER,

-------------------------------------------------------

	'' S1_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '1')   as decimal(10,2))	S1_GPA,
	'' S1_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,1,1) S1_CR_PAS,

	dbo.GETTOTALPAP(examinee.reg_no,1,1) S1_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,1,1) S1_PAP_PA,
	
	--------------------------------------------------------

	'' S2_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '2')   as decimal(10,2))	S2_GPA,
	'' S2_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,2,2)  S2_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,2,2)  S2_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,2,2) S2_PAP_PA,

----------------------------------------------------------------
	'' S3_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '3')   as decimal(10,2))	S3_GPA,
	'' S3_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,3,3) S3_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,3,3) S3_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,3,3)	S3_PAP_PA,

------------------------------------------------------------

	'' S4_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '4')   as decimal(10,2))	S4_GPA,
	'' S4_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,4,4) S4_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,4,4) S4_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,4,4) S4_PAP_PA,

---------------------------------------------------------------------

	'' S5_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '5')   as decimal(10,2))	S5_GPA,
	'' S5_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,5,5) S5_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,5,5) S5_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,5,5) S5_PAP_PA,

------------------------------------------------------------------------

	'' S6_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '6')   as decimal(10,2))	S6_GPA,
	'' S6_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,6,6) S6_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,6,6) S6_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,6,6)	S6_PAP_PA,

---------------------------------------------------------------------------

	'' S7_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '7')   as decimal(10,2))	S7_GPA,
	'' S7_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,7,7) S7_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,7,7) S7_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,7,7)	S7_PAP_PA,

----------------------------------------------------------------------

	'' S8_RES,
	cast(dbo.GETSGPA_bba (examinee.reg_no, '8')   as decimal(10,2))	S8_GPA,
	'' S8_CR,
	dbo.GETTOTALCREDITS(examinee.reg_no,8,8) S8_CR_PAS,
	dbo.GETTOTALPAP(examinee.reg_no,8,8) S8_PAP,
	dbo.GETTOTALPAP(examinee.reg_no,8,8)	S8_PAP_PA,

---------------------------------------------------------------------

	cast(dbo.GETYGPA1 (examinee.reg_no)   as decimal(10,2))	Y1_GPA,
	cast(dbo.GETYGPA2 (examinee.reg_no)   as decimal(10,2))	Y2_GPA,
	cast(dbo.GETYGPA3 (examinee.reg_no)   as decimal(10,2))	Y3_GPA,
	cast(dbo.GETYGPA4 (examinee.reg_no)   as decimal(10,2))	Y4_GPA,

	std_rif.std_name,

	dbo.GETP11TONN(examinee.reg_no,1,1) P11_S1,
	dbo.GETP11TONN(examinee.reg_no,1,2) P12_S1,
	dbo.GETP11TONN(examinee.reg_no,1,3) P13_S1,
	dbo.GETP11TONN(examinee.reg_no,1,4) P14_S1,
	dbo.GETP11TONN(examinee.reg_no,1,5) P15_S1,
	dbo.GETP11TONN(examinee.reg_no,1,6) P16_S1,
	dbo.GETP11TONN(examinee.reg_no,1,7) P17_S1,
	dbo.GETP11TONN(examinee.reg_no,1,8) P18_S1,
	dbo.GETP11TONN(examinee.reg_no,1,9) P19_S1,
	dbo.GETP11TONN(examinee.reg_no,1,10) P20_S1,

	dbo.GETP11TONN(examinee.reg_no,2,1) P11_S2,
	dbo.GETP11TONN(examinee.reg_no,2,2) P12_S2,
	dbo.GETP11TONN(examinee.reg_no,2,3) P13_S2,
	dbo.GETP11TONN(examinee.reg_no,2,4) P14_S2,
	dbo.GETP11TONN(examinee.reg_no,2,5) P15_S2,
	dbo.GETP11TONN(examinee.reg_no,2,6) P16_S2,
	dbo.GETP11TONN(examinee.reg_no,2,7) P17_S2,
	dbo.GETP11TONN(examinee.reg_no,2,8) P18_S2,
	dbo.GETP11TONN(examinee.reg_no,2,9) P19_S2,
	dbo.GETP11TONN(examinee.reg_no,2,10) P20_S2,

	dbo.GETP11TONN(examinee.reg_no,3,1) P11_S3,
	dbo.GETP11TONN(examinee.reg_no,3,2) P12_S3,
	dbo.GETP11TONN(examinee.reg_no,3,3) P13_S3,
	dbo.GETP11TONN(examinee.reg_no,3,4) P14_S3,
	dbo.GETP11TONN(examinee.reg_no,3,5) P15_S3,
	dbo.GETP11TONN(examinee.reg_no,3,6) P16_S3,
	dbo.GETP11TONN(examinee.reg_no,3,7) P17_S3,
	dbo.GETP11TONN(examinee.reg_no,3,8) P18_S3,
	dbo.GETP11TONN(examinee.reg_no,3,9) P19_S3,
	dbo.GETP11TONN(examinee.reg_no,3,10) P20_S3,

	dbo.GETP11TONN(examinee.reg_no,4,1) P11_S4,
	dbo.GETP11TONN(examinee.reg_no,4,2) P12_S4,
	dbo.GETP11TONN(examinee.reg_no,4,3) P13_S4,
	dbo.GETP11TONN(examinee.reg_no,4,4) P14_S4,
	dbo.GETP11TONN(examinee.reg_no,4,5) P15_S4,
	dbo.GETP11TONN(examinee.reg_no,4,6) P16_S4,
	dbo.GETP11TONN(examinee.reg_no,4,7) P17_S4,
	dbo.GETP11TONN(examinee.reg_no,4,8) P18_S4,
	dbo.GETP11TONN(examinee.reg_no,4,9) P19_S4,
	dbo.GETP11TONN(examinee.reg_no,4,10) P20_S4,

	dbo.GETP11TONN(examinee.reg_no,5,1) P11_S5,
	dbo.GETP11TONN(examinee.reg_no,5,2) P12_S5,
	dbo.GETP11TONN(examinee.reg_no,5,3) P13_S5,
	dbo.GETP11TONN(examinee.reg_no,5,4) P14_S5,
	dbo.GETP11TONN(examinee.reg_no,5,5) P15_S5,
	dbo.GETP11TONN(examinee.reg_no,5,6) P16_S5,
	dbo.GETP11TONN(examinee.reg_no,5,7) P17_S5,
	dbo.GETP11TONN(examinee.reg_no,5,8) P18_S5,
	dbo.GETP11TONN(examinee.reg_no,5,9) P19_S5,
	dbo.GETP11TONN(examinee.reg_no,5,10) P20_S5,

	dbo.GETP11TONN(examinee.reg_no,6,1) P11_S6,
	dbo.GETP11TONN(examinee.reg_no,6,2) P12_S6,
	dbo.GETP11TONN(examinee.reg_no,6,3) P13_S6,
	dbo.GETP11TONN(examinee.reg_no,6,4) P14_S6,
	dbo.GETP11TONN(examinee.reg_no,6,5) P15_S6,
	dbo.GETP11TONN(examinee.reg_no,6,6) P16_S6,
	dbo.GETP11TONN(examinee.reg_no,6,7) P17_S6,
	dbo.GETP11TONN(examinee.reg_no,6,8) P18_S6,
	dbo.GETP11TONN(examinee.reg_no,6,9) P19_S6,
	dbo.GETP11TONN(examinee.reg_no,6,10) P20_S6,

	dbo.GETP11TONN(examinee.reg_no,7,1) P11_S7,
	dbo.GETP11TONN(examinee.reg_no,7,2) P12_S7,
	dbo.GETP11TONN(examinee.reg_no,7,3) P13_S7,
	dbo.GETP11TONN(examinee.reg_no,7,4) P14_S7,
	dbo.GETP11TONN(examinee.reg_no,7,5) P15_S7,
	dbo.GETP11TONN(examinee.reg_no,7,6) P16_S7,
	dbo.GETP11TONN(examinee.reg_no,7,7) P17_S7,
	dbo.GETP11TONN(examinee.reg_no,7,8) P18_S7,
	dbo.GETP11TONN(examinee.reg_no,7,9) P19_S7,
	dbo.GETP11TONN(examinee.reg_no,7,10) P20_S7,

	dbo.GETP11TONN(examinee.reg_no,8,1) P11_S8,
	dbo.GETP11TONN(examinee.reg_no,8,2) P12_S8,
	dbo.GETP11TONN(examinee.reg_no,8,3) P13_S8,
	dbo.GETP11TONN(examinee.reg_no,8,4) P14_S8,
	dbo.GETP11TONN(examinee.reg_no,8,5) P15_S8,
	dbo.GETP11TONN(examinee.reg_no,8,6) P16_S8,
	dbo.GETP11TONN(examinee.reg_no,8,7) P17_S8,
	dbo.GETP11TONN(examinee.reg_no,8,8) P18_S8,
	dbo.GETP11TONN(examinee.reg_no,8,9) P19_S8,
	dbo.GETP11TONN(examinee.reg_no,8,10) P20_S8,

	'' WITH_STAT,

	std_rif.SESSION,
	college.col_name,
	std_rif.fname,
	std_rif.mname,
	std_rif.sex,
	examinee.cert_sl CRT_SL,
	'' INST,
	'' COMMENTS,
	'' STATUS,
	'' REMARKS,
	'' VIEW_STATUS,
	'' PRINT_STATUS,
	'' UPDATE_STATUS,
	'' REF_ID,
	'' SPE_ST,

	result_publish_date.[Date] RES_DATE

FROM
	examinee,
	regroll,
	std_rif,
	college,EMS,
	result_publish_date

WHERE
	examinee.semester = '2'
AND examinee.exm_year = CASE WHEN @EXM_YEAR ='' THEN examinee.exm_year ELSE @EXM_YEAR END
AND (@reg_no IS NULL OR examinee.reg_no = @reg_no) -- FOR ALL STUDENT
AND examinee.exm_year=result_publish_date.[Year]
AND examinee.reg_no = regroll.reg_no AND examinee.reg_no = EMS.reg_no and EMS.exm_year=examinee.exm_year AND EMS.exm_year = examinee.exm_year
AND EMS.exm_code = examinee.exm_code
and examinee.exm_year=result_publish_date.year
AND regroll.reg_no = std_rif.reg_no
AND examinee.reg_no =std_rif.reg_no
AND examinee.col_code = college.col_code
 AND dbo.GETCGPA (examinee.reg_no) >= 2.00
 AND dbo.GETGRADESALL (examinee.reg_no) NOT like '%F%' 

AND (
    dbo.GETGRADESALL_SEMESTER(examinee.reg_no,1) NOT LIKE '%F%'
    AND dbo.GETGRADESALL_SEMESTER(examinee.reg_no,1) NOT LIKE '%NA%'
)
AND (
    dbo.GETGRADESALL_SEMESTER(examinee.reg_no,2) NOT LIKE '%F%'
    AND dbo.GETGRADESALL_SEMESTER(examinee.reg_no,2) NOT LIKE '%NA%'
)

 AND dbo.GETTOTALCREDITS(examinee.reg_no,1,8) =44
 AND dbo.GETSEMESTERALL(examinee.reg_no) = '12'

END
GO
