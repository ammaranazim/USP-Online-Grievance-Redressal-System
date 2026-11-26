# USP Online Grievance Redressal System

**Final Year Project (FYP)**: BS Computer Science
**Institution:** University of Southern Punjab, Multan

A web-based application for managing student grievances digitally. Students submit grievances with supporting evidence and track their progress. Department Heads assign grievances to staff, staff propose resolutions, and the Admin oversees the whole system, including escalations, users, departments, and reports. Email notifications are sent at key workflow steps.

## Modules

| Module | Main features |
|---|---|
| **Student** | Login, dashboard, submit grievance, upload evidence, track status, change password |
| **Admin** | Dashboard, all grievances, escalated grievances, manage students / staff / department heads / departments, notifications, reports |
| **Department Head (HOD)** | View department grievances, search & filter, reject or assign to staff, review and approve/return resolutions, escalations |
| **Staff** | View assigned grievances, submit proposed resolution with remarks and evidence, resubmit returned resolutions, escalation requests |

Shared features: role selection page, forgot / reset password via email, secure password hashing (PBKDF2 with salt).

## Technology Stack

- ASP.NET Web Forms (C#), .NET Framework 4.5
- Microsoft SQL Server, ADO.NET
- HTML, CSS, JavaScript, Bootstrap 4.4.1
- Gmail SMTP for email notifications

## Repository Structure

| Folder / File | Contents |
|---|---|
| `Project_Source_Code/GRSfinal/` | Website source code |
| `Database/GRS_Testing.sql` | Ready-to-run database with demo accounts and sample data |

## How to Run (about 5 minutes)

### Requirements
- Windows with Visual Studio (ASP.NET / web development workload)
- Microsoft SQL Server 2012 or newer (Express is fine) and SQL Server Management Studio (SSMS)

### Step 1: Download
On GitHub click **Code > Download ZIP**, then extract it.

### Step 2: Create the database
1. Open SSMS and connect to your SQL Server instance.
2. Open `Database/GRS_Testing.sql` and execute the **entire** file.
3. This creates a database named `GRS` with all tables, demo accounts, and one sample grievance.

> Use an instance that does not already have a database called `GRS`. The script will not overwrite an existing one.


### Step 3: Configure the connection
1. Open `Project_Source_Code/GRSfinal/Web.config` in any text editor.
2. Find the connection string named `GRS` and replace `YOUR_SQL_SERVER_NAME` with your SQL Server name:

| Your SQL Server | Replace `YOUR_SQL_SERVER_NAME` with |
|---|---|
| SQL Server Express | `.\SQLEXPRESS` |
| Default instance | `.` |

Example: `Data Source=.\SQLEXPRESS;Initial Catalog=GRS;Integrated Security=True`

Email settings (`SmtpEmail`, `SmtpAppPassword`) can stay as placeholders. They are only needed to test password-reset emails. Normal login does not use email.

### Step 4: Run
1. In Visual Studio choose **File > Open > Web Site**.
2. Select the `Project_Source_Code/GRSfinal` folder (the project has no `.sln` file).
3. Right-click `Default.aspx`, choose **Set As Start Page**, then press **Ctrl+F5**.
4. If Visual Studio offers to upgrade or retarget the .NET Framework version, accept it (for example to 4.8).

## Demo Login Credentials (testing accounts only)

| Role | Login | Password | Login page |
|---|---|---|---|
| Admin | `ammara` | `12345678` | `Admin/AdminLogin.aspx` |
| Student | `BSCS-023R22-82` | `ii8899` | `Student/StudentLogin.aspx` |
| HOD | `mubashir123` | `UspTest@2026` | `DepartmentHead/HODLogin.aspx` |
| Staff | `muneeb1234` | `Staff123@` | `Staff/StaffLogin.aspx` |
| Staff | `zeeshan123` | `Zeeshan123@` | `Staff/StaffLogin.aspx` |

## Suggested Test Flow

1. **Student:** log in, view the sample grievance, submit a new one.
2. **HOD:** log in, view the department's grievances, assign one to staff.
3. **Staff (`muneeb1234`):** open the assigned grievance and submit a resolution.
4. **HOD:** review the proposed resolution (approve or return).
5. **Admin:** view users, departments, grievances, and reports.

## Troubleshooting

- **Cannot connect to database:** check the server name in the `Web.config` connection string and make sure SQL Server is running.
- **Login fails:** make sure you ran the whole `GRS_Testing.sql` file and used the exact credentials above, without spaces.
- **Upload errors:** make sure the `UploadedFiles` and `EvidenceFiles` folders exist inside `GRSfinal` and are writable.

## Group Members

| Name | Registration No. |
|---|---|
| Ammara Nazam | BSCS-023R22-82 |
| Mifra Ameer | BSCS-023R22-97 |
| Misbah Rabbani | BSCS-023R22P-46 |

**Supervisor:** Sir Imran Ali
**Session:** 2022-2026