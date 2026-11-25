-- USP GRS: fresh testing database with the user-guide demo logins.
-- Run the entire file in SSMS. Requires SQL Server 2012 or newer.
-- Uses SQL Server's default data paths. Does not overwrite an existing GRS.
USE [master]
GO
IF DB_ID(N'GRS') IS NOT NULL
BEGIN
    RAISERROR('GRS already exists. Use a fresh SQL Server instance or rename the demo database consistently before importing.', 16, 1);
    SET NOEXEC ON;
END
GO
CREATE DATABASE [GRS]
GO
USE [GRS]
GO
/****** Object:  Table [dbo].[AdminLogin]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminLogin](
	[adminLoginID] [int] IDENTITY(1,1) NOT NULL,
	[adminLoginName] [nvarchar](50) NOT NULL,
	[adminLoginEmail] [nvarchar](50) NOT NULL,
	[adminLoginPassword] [nvarchar](255) NOT NULL,
	[adminLoginPhoneNo] [nvarchar](50) NOT NULL,
	[adminLoginDescription] [nvarchar](500) NULL,
 CONSTRAINT [PK_adminLogin] PRIMARY KEY CLUSTERED 
(
	[adminLoginID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Department]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Department](
	[deptID] [int] IDENTITY(1,1) NOT NULL,
	[deptName] [nvarchar](50) NOT NULL,
	[deptCode] [nvarchar](50) NOT NULL,
	[deptStatus] [nvarchar](50) NOT NULL,
	[deptDescription] [nvarchar](50) NULL,
	[deptEmail] [nvarchar](50) NOT NULL,
	[deptContactNo] [nvarchar](50) NOT NULL,
	[deptWH] [int] NOT NULL,
	[deptLocation] [nvarchar](50) NOT NULL,
	[categoryID] [int] NOT NULL,
 CONSTRAINT [PK_Departmenttable] PRIMARY KEY CLUSTERED 
(
	[deptID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DepartmentHead]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DepartmentHead](
	[deptHeadID] [int] IDENTITY(1,1) NOT NULL,
	[deptHeadName] [nvarchar](50) NOT NULL,
	[deptHeadUserName] [nvarchar](50) NOT NULL,
	[deptHeadPassword] [nvarchar](255) NOT NULL,
	[deptHeadEmail] [nvarchar](50) NOT NULL,
	[deptHeadPhoneNo] [nvarchar](50) NOT NULL,
	[deptHeadDesignation] [nvarchar](50) NOT NULL,
	[deptHeadStatus] [nvarchar](50) NOT NULL,
	[deptID] [int] NOT NULL,
	[mustChangePassword] [bit] NOT NULL,
 CONSTRAINT [PK_DepartmentHead table] PRIMARY KEY CLUSTERED 
(
	[deptHeadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EscalatedGrievance]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EscalatedGrievance](
	[escalationID] [int] IDENTITY(1,1) NOT NULL,
	[grievanceID] [int] NOT NULL,
	[escalationReason] [nvarchar](500) NOT NULL,
	[escalationDescription] [nvarchar](max) NULL,
	[requestedDate] [datetime] NOT NULL,
	[requestStatus] [nvarchar](40) NOT NULL,
	[hodRemarks] [nvarchar](1000) NULL,
	[hodDecisionDate] [datetime] NULL,
	[reviewedByDeptHeadID] [int] NULL,
	[adminRemarks] [nvarchar](1000) NULL,
	[adminDecisionDate] [datetime] NULL,
	[decidedByAdminID] [int] NULL,
 CONSTRAINT [PK_EscalatedGrievance] PRIMARY KEY CLUSTERED 
(
	[escalationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Grievance]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Grievance](
	[grievanceID] [int] IDENTITY(1,1) NOT NULL,
	[stdID] [int] NOT NULL,
	[categoryID] [int] NOT NULL,
	[deptID] [int] NOT NULL,
	[grievanceTitle] [nvarchar](250) NOT NULL,
	[grievanceDescription] [nvarchar](1000) NOT NULL,
	[grievancePriority] [nvarchar](50) NOT NULL,
	[grievanceStatus] [nvarchar](50) NOT NULL,
	[grievanceSubmissionDate] [datetime] NOT NULL,
	[grievanceResolutionDate] [datetime] NULL,
	[gvTypeID] [int] NULL,
	[grievanceDueDate] [date] NULL,
 CONSTRAINT [PK_GrievanceTable] PRIMARY KEY CLUSTERED 
(
	[grievanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrievanceAssignment]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrievanceAssignment](
	[assignmentID] [int] IDENTITY(1,1) NOT NULL,
	[grievanceID] [int] NOT NULL,
	[staffID] [int] NOT NULL,
	[assignedBy] [int] NOT NULL,
	[assignedDate] [datetime] NOT NULL,
	[assignmentStatus] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_GrievanceAssignment] PRIMARY KEY CLUSTERED 
(
	[assignmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrievanceCategory]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrievanceCategory](
	[categoryID] [int] IDENTITY(1,1) NOT NULL,
	[categoryName] [nvarchar](50) NOT NULL,
	[categoryDescription] [nvarchar](50) NULL,
	[categoryStatus] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Grievance Category] PRIMARY KEY CLUSTERED 
(
	[categoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Grievanceevidence]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Grievanceevidence](
	[evidenceID] [int] IDENTITY(1,1) NOT NULL,
	[evidenceName] [varchar](255) NULL,
	[evidenceDescription] [varchar](250) NULL,
	[evidenceDatetime] [nvarchar](50) NOT NULL,
	[evidenceUploadedby] [varchar](100) NULL,
	[evidenceFilepath] [varchar](500) NULL,
	[grievanceID] [int] NOT NULL,
 CONSTRAINT [PK_evidence] PRIMARY KEY CLUSTERED 
(
	[evidenceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GrievanceFeedback]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrievanceFeedback](
	[feedbackID] [int] IDENTITY(1,1) NOT NULL,
	[grievanceID] [int] NOT NULL,
	[stdID] [int] NOT NULL,
	[rating] [int] NOT NULL,
	[feedbackComments] [nvarchar](1000) NULL,
	[feedbackDate] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[feedbackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_GrievanceFeedback_Grievance] UNIQUE NONCLUSTERED 
(
	[grievanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrievanceResolution]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrievanceResolution](
	[resolutionID] [int] IDENTITY(1,1) NOT NULL,
	[grievanceID] [int] NOT NULL,
	[staffID] [int] NOT NULL,
	[proposedResolution] [nvarchar](max) NOT NULL,
	[staffRemarks] [nvarchar](2000) NULL,
	[submittedDate] [datetime] NOT NULL,
	[reviewStatus] [nvarchar](30) NOT NULL,
	[hodRemarks] [nvarchar](2000) NULL,
	[reviewedByDeptHeadID] [int] NULL,
	[reviewedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[resolutionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GrievanceType]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrievanceType](
	[gvTypeID] [int] IDENTITY(1,1) NOT NULL,
	[gvTypeName] [nvarchar](50) NOT NULL,
	[gvTypeDescription] [nvarchar](50) NULL,
	[categoryID] [int] NOT NULL,
 CONSTRAINT [PK_GrievanceSubtype] PRIMARY KEY CLUSTERED 
(
	[gvTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PasswordResetToken]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordResetToken](
	[resetID] [int] IDENTITY(1,1) NOT NULL,
	[userID] [int] NOT NULL,
	[userRole] [nvarchar](20) NOT NULL,
	[tokenHash] [nvarchar](256) NOT NULL,
	[expiryDate] [datetime2](7) NOT NULL,
	[isUsed] [bit] NOT NULL,
	[createdDate] [datetime2](7) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[resetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Staff]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Staff](
	[staffID] [int] IDENTITY(1,1) NOT NULL,
	[staffName] [nvarchar](100) NOT NULL,
	[staffUserName] [nvarchar](50) NOT NULL,
	[staffPasswordHash] [nvarchar](255) NOT NULL,
	[staffEmail] [nvarchar](100) NOT NULL,
	[staffPhoneNo] [nvarchar](50) NOT NULL,
	[staffDesignation] [nvarchar](50) NOT NULL,
	[staffStatus] [nvarchar](20) NOT NULL,
	[deptID] [int] NOT NULL,
	[mustChangePassword] [bit] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[staffID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Staff_UserName] UNIQUE NONCLUSTERED 
(
	[staffUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StudentInfo]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentInfo](
	[stdID] [int] IDENTITY(1,1) NOT NULL,
	[stdRegNo] [nvarchar](50) NOT NULL,
	[stdFirstName] [nvarchar](50) NOT NULL,
	[stdLastName] [nvarchar](50) NOT NULL,
	[stdFName] [nvarchar](50) NOT NULL,
	[stdCnic] [nvarchar](50) NOT NULL,
	[stdPhoneNo] [nvarchar](50) NOT NULL,
	[stdSemester] [nvarchar](50) NOT NULL,
	[stdProgram] [nvarchar](50) NOT NULL,
	[stdSession] [nvarchar](50) NOT NULL,
	[stdDescription] [nvarchar](50) NOT NULL,
	[stdGender] [nvarchar](50) NOT NULL,
	[stdAddress] [nvarchar](50) NOT NULL,
	[stdDateOfBirth] [date] NOT NULL,
	[stdEnrollmentDate] [date] NOT NULL,
	[stdCity] [nvarchar](50) NOT NULL,
	[stdStatus] [nvarchar](50) NOT NULL,
	[deptID] [int] NOT NULL,
	[stdEmail] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Studenttable] PRIMARY KEY CLUSTERED 
(
	[stdID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[studentLogin]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[studentLogin](
	[stdLoginID] [int] IDENTITY(1,1) NOT NULL,
	[stdLoginPassword] [nvarchar](255) NOT NULL,
	[stdID] [int] NOT NULL,
	[mustChangePassword] [bit] NOT NULL,
 CONSTRAINT [PK_studentLogin] PRIMARY KEY CLUSTERED 
(
	[stdLoginID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemNotification]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemNotification](
	[notificationID] [int] IDENTITY(1,1) NOT NULL,
	[recipientRole] [nvarchar](20) NOT NULL,
	[recipientID] [int] NOT NULL,
	[grievanceID] [int] NULL,
	[notificationType] [nvarchar](50) NOT NULL,
	[message] [nvarchar](255) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[isRead] [bit] NOT NULL,
 CONSTRAINT [PK_SystemNotification] PRIMARY KEY CLUSTERED 
(
	[notificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TrackGrievance]    Script Date: 10/25/2026 10:07:52 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrackGrievance](
	[trackID] [int] IDENTITY(1,1) NOT NULL,
	[grievanceID] [int] NOT NULL,
	[loginID] [int] NOT NULL,
	[trackingactivityDescription] [nvarchar](500) NOT NULL,
	[trackingactivitydatetime] [datetime] NOT NULL,
	[escalationID] [int] NULL,
 CONSTRAINT [PK_TrackGrievance] PRIMARY KEY CLUSTERED 
(
	[trackID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_PasswordResetToken_user]    Script Date: 10/25/2026 10:07:52 PM ******/
CREATE NONCLUSTERED INDEX [IX_PasswordResetToken_user] ON [dbo].[PasswordResetToken]
(
	[userRole] ASC,
	[userID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [UX_PasswordResetToken_tokenHash]    Script Date: 10/25/2026 10:07:52 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_PasswordResetToken_tokenHash] ON [dbo].[PasswordResetToken]
(
	[tokenHash] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[DepartmentHead] ADD  CONSTRAINT [DF_DepartmentHead_mustChangePassword]  DEFAULT ((0)) FOR [mustChangePassword]
GO
ALTER TABLE [dbo].[EscalatedGrievance] ADD  CONSTRAINT [DF_EscalatedGrievance_RequestedDate]  DEFAULT (getdate()) FOR [requestedDate]
GO
ALTER TABLE [dbo].[EscalatedGrievance] ADD  CONSTRAINT [DF_EscalatedGrievance_RequestStatus]  DEFAULT (N'Pending HOD Review') FOR [requestStatus]
GO
ALTER TABLE [dbo].[GrievanceAssignment] ADD  CONSTRAINT [DF_GrievanceAssignment_Date]  DEFAULT (getdate()) FOR [assignedDate]
GO
ALTER TABLE [dbo].[GrievanceAssignment] ADD  CONSTRAINT [DF_GrievanceAssignment_Status]  DEFAULT ('Assigned') FOR [assignmentStatus]
GO
ALTER TABLE [dbo].[GrievanceFeedback] ADD  CONSTRAINT [DF_GrievanceFeedback_feedbackDate]  DEFAULT (getdate()) FOR [feedbackDate]
GO
ALTER TABLE [dbo].[GrievanceResolution] ADD  CONSTRAINT [DF_GrievanceResolution_SubmittedDate]  DEFAULT (getdate()) FOR [submittedDate]
GO
ALTER TABLE [dbo].[GrievanceResolution] ADD  CONSTRAINT [DF_GrievanceResolution_ReviewStatus]  DEFAULT ('Pending') FOR [reviewStatus]
GO
ALTER TABLE [dbo].[PasswordResetToken] ADD  CONSTRAINT [DF_PasswordResetToken_isUsed]  DEFAULT ((0)) FOR [isUsed]
GO
ALTER TABLE [dbo].[PasswordResetToken] ADD  CONSTRAINT [DF_PasswordResetToken_createdDate]  DEFAULT (getdate()) FOR [createdDate]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_Status]  DEFAULT ('Active') FOR [staffStatus]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_MustChangePassword]  DEFAULT ((1)) FOR [mustChangePassword]
GO
ALTER TABLE [dbo].[studentLogin] ADD  CONSTRAINT [DF_studentLogin_mustChangePassword]  DEFAULT ((0)) FOR [mustChangePassword]
GO
ALTER TABLE [dbo].[SystemNotification] ADD  CONSTRAINT [DF_SystemNotification_createdDate]  DEFAULT (getdate()) FOR [createdDate]
GO
ALTER TABLE [dbo].[SystemNotification] ADD  CONSTRAINT [DF_SystemNotification_isRead]  DEFAULT ((0)) FOR [isRead]
GO
ALTER TABLE [dbo].[Department]  WITH CHECK ADD  CONSTRAINT [FK_Department_Category] FOREIGN KEY([categoryID])
REFERENCES [dbo].[GrievanceCategory] ([categoryID])
GO
ALTER TABLE [dbo].[Department] CHECK CONSTRAINT [FK_Department_Category]
GO
ALTER TABLE [dbo].[DepartmentHead]  WITH CHECK ADD  CONSTRAINT [FK_DepartmentHead_Department] FOREIGN KEY([deptID])
REFERENCES [dbo].[Department] ([deptID])
GO
ALTER TABLE [dbo].[DepartmentHead] CHECK CONSTRAINT [FK_DepartmentHead_Department]
GO
ALTER TABLE [dbo].[EscalatedGrievance]  WITH CHECK ADD  CONSTRAINT [FK_EscalatedGrievance_Admin] FOREIGN KEY([decidedByAdminID])
REFERENCES [dbo].[AdminLogin] ([adminLoginID])
GO
ALTER TABLE [dbo].[EscalatedGrievance] CHECK CONSTRAINT [FK_EscalatedGrievance_Admin]
GO
ALTER TABLE [dbo].[EscalatedGrievance]  WITH CHECK ADD  CONSTRAINT [FK_EscalatedGrievance_DepartmentHead] FOREIGN KEY([reviewedByDeptHeadID])
REFERENCES [dbo].[DepartmentHead] ([deptHeadID])
GO
ALTER TABLE [dbo].[EscalatedGrievance] CHECK CONSTRAINT [FK_EscalatedGrievance_DepartmentHead]
GO
ALTER TABLE [dbo].[EscalatedGrievance]  WITH CHECK ADD  CONSTRAINT [FK_EscalatedGrievance_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[EscalatedGrievance] CHECK CONSTRAINT [FK_EscalatedGrievance_Grievance]
GO
ALTER TABLE [dbo].[Grievance]  WITH CHECK ADD  CONSTRAINT [FK_Grievance_Category] FOREIGN KEY([categoryID])
REFERENCES [dbo].[GrievanceCategory] ([categoryID])
GO
ALTER TABLE [dbo].[Grievance] CHECK CONSTRAINT [FK_Grievance_Category]
GO
ALTER TABLE [dbo].[Grievance]  WITH CHECK ADD  CONSTRAINT [FK_Grievance_Department] FOREIGN KEY([deptID])
REFERENCES [dbo].[Department] ([deptID])
GO
ALTER TABLE [dbo].[Grievance] CHECK CONSTRAINT [FK_Grievance_Department]
GO
ALTER TABLE [dbo].[Grievance]  WITH CHECK ADD  CONSTRAINT [FK_Grievance_Student] FOREIGN KEY([stdID])
REFERENCES [dbo].[StudentInfo] ([stdID])
GO
ALTER TABLE [dbo].[Grievance] CHECK CONSTRAINT [FK_Grievance_Student]
GO
ALTER TABLE [dbo].[Grievance]  WITH CHECK ADD  CONSTRAINT [FK_Grievance_Subtype] FOREIGN KEY([gvTypeID])
REFERENCES [dbo].[GrievanceType] ([gvTypeID])
GO
ALTER TABLE [dbo].[Grievance] CHECK CONSTRAINT [FK_Grievance_Subtype]
GO
ALTER TABLE [dbo].[GrievanceAssignment]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceAssignment_DepartmentHead] FOREIGN KEY([assignedBy])
REFERENCES [dbo].[DepartmentHead] ([deptHeadID])
GO
ALTER TABLE [dbo].[GrievanceAssignment] CHECK CONSTRAINT [FK_GrievanceAssignment_DepartmentHead]
GO
ALTER TABLE [dbo].[GrievanceAssignment]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceAssignment_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[GrievanceAssignment] CHECK CONSTRAINT [FK_GrievanceAssignment_Grievance]
GO
ALTER TABLE [dbo].[GrievanceAssignment]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceAssignment_Staff] FOREIGN KEY([staffID])
REFERENCES [dbo].[Staff] ([staffID])
GO
ALTER TABLE [dbo].[GrievanceAssignment] CHECK CONSTRAINT [FK_GrievanceAssignment_Staff]
GO
ALTER TABLE [dbo].[Grievanceevidence]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceEvidence_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[Grievanceevidence] CHECK CONSTRAINT [FK_GrievanceEvidence_Grievance]
GO
ALTER TABLE [dbo].[GrievanceFeedback]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceFeedback_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[GrievanceFeedback] CHECK CONSTRAINT [FK_GrievanceFeedback_Grievance]
GO
ALTER TABLE [dbo].[GrievanceFeedback]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceFeedback_Student] FOREIGN KEY([stdID])
REFERENCES [dbo].[StudentInfo] ([stdID])
GO
ALTER TABLE [dbo].[GrievanceFeedback] CHECK CONSTRAINT [FK_GrievanceFeedback_Student]
GO
ALTER TABLE [dbo].[GrievanceResolution]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceResolution_DepartmentHead] FOREIGN KEY([reviewedByDeptHeadID])
REFERENCES [dbo].[DepartmentHead] ([deptHeadID])
GO
ALTER TABLE [dbo].[GrievanceResolution] CHECK CONSTRAINT [FK_GrievanceResolution_DepartmentHead]
GO
ALTER TABLE [dbo].[GrievanceResolution]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceResolution_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[GrievanceResolution] CHECK CONSTRAINT [FK_GrievanceResolution_Grievance]
GO
ALTER TABLE [dbo].[GrievanceResolution]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceResolution_Staff] FOREIGN KEY([staffID])
REFERENCES [dbo].[Staff] ([staffID])
GO
ALTER TABLE [dbo].[GrievanceResolution] CHECK CONSTRAINT [FK_GrievanceResolution_Staff]
GO
ALTER TABLE [dbo].[GrievanceType]  WITH CHECK ADD  CONSTRAINT [FK_GrievanceType_Category] FOREIGN KEY([categoryID])
REFERENCES [dbo].[GrievanceCategory] ([categoryID])
GO
ALTER TABLE [dbo].[GrievanceType] CHECK CONSTRAINT [FK_GrievanceType_Category]
GO
ALTER TABLE [dbo].[Staff]  WITH CHECK ADD  CONSTRAINT [FK_Staff_Department] FOREIGN KEY([deptID])
REFERENCES [dbo].[Department] ([deptID])
GO
ALTER TABLE [dbo].[Staff] CHECK CONSTRAINT [FK_Staff_Department]
GO
ALTER TABLE [dbo].[StudentInfo]  WITH CHECK ADD  CONSTRAINT [FK_StudentInfo_Department] FOREIGN KEY([deptID])
REFERENCES [dbo].[Department] ([deptID])
GO
ALTER TABLE [dbo].[StudentInfo] CHECK CONSTRAINT [FK_StudentInfo_Department]
GO
ALTER TABLE [dbo].[studentLogin]  WITH CHECK ADD  CONSTRAINT [FK_StudentLogin_StudentInfo] FOREIGN KEY([stdID])
REFERENCES [dbo].[StudentInfo] ([stdID])
GO
ALTER TABLE [dbo].[studentLogin] CHECK CONSTRAINT [FK_StudentLogin_StudentInfo]
GO
ALTER TABLE [dbo].[SystemNotification]  WITH CHECK ADD  CONSTRAINT [FK_SystemNotification_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[SystemNotification] CHECK CONSTRAINT [FK_SystemNotification_Grievance]
GO
ALTER TABLE [dbo].[TrackGrievance]  WITH CHECK ADD  CONSTRAINT [FK_TrackGrievance_EscalatedGrievance] FOREIGN KEY([escalationID])
REFERENCES [dbo].[EscalatedGrievance] ([escalationID])
GO
ALTER TABLE [dbo].[TrackGrievance] CHECK CONSTRAINT [FK_TrackGrievance_EscalatedGrievance]
GO
ALTER TABLE [dbo].[TrackGrievance]  WITH CHECK ADD  CONSTRAINT [FK_TrackGrievance_Grievance] FOREIGN KEY([grievanceID])
REFERENCES [dbo].[Grievance] ([grievanceID])
GO
ALTER TABLE [dbo].[TrackGrievance] CHECK CONSTRAINT [FK_TrackGrievance_Grievance]
GO
ALTER TABLE [dbo].[EscalatedGrievance]  WITH CHECK ADD  CONSTRAINT [CK_EscalatedGrievance_RequestStatus] CHECK  (([requestStatus]=N'Closed' OR [requestStatus]=N'Action Taken' OR [requestStatus]=N'Action Required' OR [requestStatus]=N'Pending Admin Review' OR [requestStatus]=N'Pending HOD Review'))
GO
ALTER TABLE [dbo].[EscalatedGrievance] CHECK CONSTRAINT [CK_EscalatedGrievance_RequestStatus]
GO
ALTER TABLE [dbo].[GrievanceFeedback]  WITH CHECK ADD  CONSTRAINT [CK_GrievanceFeedback_Rating] CHECK  (([rating]>=(1) AND [rating]<=(5)))
GO
ALTER TABLE [dbo].[GrievanceFeedback] CHECK CONSTRAINT [CK_GrievanceFeedback_Rating]
GO
ALTER TABLE [dbo].[GrievanceResolution]  WITH CHECK ADD  CONSTRAINT [CK_GrievanceResolution_ReviewStatus] CHECK  (([reviewStatus]='Returned' OR [reviewStatus]='Approved' OR [reviewStatus]='Pending'))
GO
ALTER TABLE [dbo].[GrievanceResolution] CHECK CONSTRAINT [CK_GrievanceResolution_ReviewStatus]
GO
ALTER TABLE [dbo].[PasswordResetToken]  WITH CHECK ADD  CONSTRAINT [CK_PasswordResetToken_userRole] CHECK  (([userRole]='Staff' OR [userRole]='HOD' OR [userRole]='Admin' OR [userRole]='Student'))
GO
ALTER TABLE [dbo].[PasswordResetToken] CHECK CONSTRAINT [CK_PasswordResetToken_userRole]
GO
ALTER TABLE [dbo].[SystemNotification]  WITH CHECK ADD  CONSTRAINT [CK_SystemNotification_recipientRole] CHECK  (([recipientRole]=N'Staff' OR [recipientRole]=N'HOD' OR [recipientRole]=N'Admin' OR [recipientRole]=N'Student'))
GO
ALTER TABLE [dbo].[SystemNotification] CHECK CONSTRAINT [CK_SystemNotification_recipientRole]
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;
INSERT dbo.AdminLogin (adminLoginName, adminLoginEmail, adminLoginPassword, adminLoginPhoneNo, adminLoginDescription)
VALUES (N'ammara', N'ammaranazim5@gmail.com', N'100000.NSnm5ONbjFSDGzbdVgPEZQ==.L0w/Th5xsCQFVpiDAv/r6j5EO6u0fuoVkSSaCorH3TY=', N'00000000000', N'Public demonstration account');

INSERT dbo.GrievanceCategory (categoryName, categoryDescription, categoryStatus)
VALUES (N'Academic', N'Demo academic requests', N'Active');
DECLARE @categoryID int = SCOPE_IDENTITY();

INSERT dbo.Department (deptName, deptCode, deptStatus, deptDescription, deptEmail, deptContactNo, deptWH, deptLocation, categoryID)
VALUES (N'Computer Science', N'CS', N'Active', N'Demo department', N'department@example.com', N'00000000000', 48, N'Demo campus', @categoryID);
DECLARE @deptID int = SCOPE_IDENTITY();

INSERT dbo.GrievanceType (gvTypeName, gvTypeDescription, categoryID)
VALUES (N'Course Support', N'Demo course support request', @categoryID);
DECLARE @typeID int = SCOPE_IDENTITY();

INSERT dbo.DepartmentHead (deptHeadName, deptHeadUserName, deptHeadPassword, deptHeadEmail, deptHeadPhoneNo, deptHeadDesignation, deptHeadStatus, deptID, mustChangePassword)
VALUES (N'Demo HOD', N'mubashir123', N'100000.tqezyMLZzY05UwKVNpBC5Q==.ra82YkBTEZypXMjb/in2jV9FsSGvnJqLN6LUXyncDF8=', N'hod@example.com', N'00000000000', N'Head of Department', N'Active', @deptID, 0);
DECLARE @hodID int = SCOPE_IDENTITY();

INSERT dbo.Staff (staffName, staffUserName, staffPasswordHash, staffEmail, staffPhoneNo, staffDesignation, staffStatus, deptID, mustChangePassword)
VALUES (N'Demo Staff One', N'muneeb1234', N'100000.vYUspwNVjSnc9xOP20tZpg==.08QTG2ZO/hdwZuLjQw1ManK2fjTO/skNh2k/U6gLQMo=', N'staff1@example.com', N'00000000000', N'Lecturer', N'Active', @deptID, 0);
DECLARE @staffID int = SCOPE_IDENTITY();
INSERT dbo.Staff (staffName, staffUserName, staffPasswordHash, staffEmail, staffPhoneNo, staffDesignation, staffStatus, deptID, mustChangePassword)
VALUES (N'Demo Staff Two', N'zeeshan123', N'100000.rUP2+Cpl73qLbX9ZgQh7aQ==.SfoxDMldaeAXZqbLC5Z2IWprZLNgRYO0UFKkiqi7KR8=', N'staff2@example.com', N'00000000000', N'Lecturer', N'Active', @deptID, 0);

INSERT dbo.StudentInfo
(stdRegNo, stdFirstName, stdLastName, stdFName, stdCnic, stdPhoneNo, stdSemester, stdProgram, stdSession, stdDescription, stdGender, stdAddress, stdDateOfBirth, stdEnrollmentDate, stdCity, stdStatus, deptID, stdEmail)
VALUES (N'BSCS-023R22-82', N'Demo', N'Student', N'Demo Parent', N'00000-0000000-0', N'00000000000', N'8', N'BSCS', N'2022-2026', N'Public demonstration student', N'Female', N'Demo address', '20030101', '20220901', N'Multan', N'Active', @deptID, N'student@example.com');
DECLARE @studentID int = SCOPE_IDENTITY();

INSERT dbo.studentLogin (stdLoginPassword, stdID, mustChangePassword)
VALUES (N'100000.IV6IFOPOnTLvjU8CccYGsw==.wxwzSVbc69W8QPLzdgEjg8etOJP+3sR1R/f6KE8SIJ4=', @studentID, 0);

INSERT dbo.Grievance (stdID, categoryID, deptID, grievanceTitle, grievanceDescription, grievancePriority, grievanceStatus, grievanceSubmissionDate, gvTypeID, grievanceDueDate)
VALUES (@studentID, @categoryID, @deptID, N'Demo: request for course support', N'Synthetic sample grievance for testing the student, HOD and staff workflow.', N'Medium', N'Assigned', GETDATE(), @typeID, DATEADD(day, 2, GETDATE()));
DECLARE @grievanceID int = SCOPE_IDENTITY();

INSERT dbo.GrievanceAssignment (grievanceID, staffID, assignedBy, assignedDate, assignmentStatus)
VALUES (@grievanceID, @staffID, @hodID, GETDATE(), N'Assigned');

INSERT dbo.SystemNotification (recipientRole, recipientID, grievanceID, notificationType, message)
VALUES (N'Staff', @staffID, @grievanceID, N'Assignment', N'A demo grievance has been assigned to you.');
COMMIT TRANSACTION;
GO
PRINT 'GRS testing database created. See README.txt for demo credentials.';
GO
SET NOEXEC OFF;
GO
