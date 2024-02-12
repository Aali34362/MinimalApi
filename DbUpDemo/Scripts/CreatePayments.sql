Create Table Payments
(
	Id Int Primary Key Not Null,
	User_Nm Varchar(30) Not Null,
	Payment_Amt Varchar(10) Not Null,
	Payment_Type Varchar(10) Null,
	Payment_Status Varchar(20) Null,
	Act_Ind Int Null,
	Dact_Ind Int Null,
	Crtd_User Varchar(30) Null,
	Last_Crtd_User Varchar(30) Null,
	Crtd_Dt DateTime Null,
	Last_Crtd_Dt DateTime Null,
	Ip_address Varchar(10) Null
);