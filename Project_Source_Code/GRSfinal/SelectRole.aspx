
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectRole.aspx.cs" Inherits="SelectRole" %>

<!DOCTYPE html>
<html lang="en">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Select Your Portal |  USP Online Grievance Redressal System</title>

    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@4.4.1/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/SelectRole.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">

        <header class="role-header">
            <div class="container role-header-inner">

                <a href="Default.aspx" class="brand">
                    <img src="Images/usp-logo.png"
                         alt="University of Southern Punjab Logo"
                         class="brand-logo" />

                    <span class="brand-text">
                        <strong>University of Southern Punjab</strong>
                        <small>Multan, Pakistan</small>
                    </span>
                </a>

                <a href="Default.aspx" class="back-home">
                    <span aria-hidden="true"></span> Back to Home
                </a>

            </div>
        </header>

        <main class="role-main">

            <section class="role-intro">
                <span class="section-eyebrow"> USP ONLINE GRIEVANCE REDRESSAL SYSTEM</span>

                <h1>Select Your Portal</h1>

                <p>
                    Welcome to the  USP Online Grievance Redressal System.
                    Choose your designated portal to continue.
                </p>
            </section>

            <section class="container role-grid" aria-label="Available portals">

                <!-- Student Portal -->
                <article class="role-card student-card">
                    <div class="role-icon" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M12 3L2.5 8L12 13L21.5 8L12 3Z" />
                            <path d="M6 10V15.5C9.5 18.5 14.5 18.5 18 15.5V10" />
                            <path d="M21.5 8V14" />
                        </svg>
                    </div>

                    <span class="role-label">STUDENT ACCESS</span>
                    <h2>Student Portal</h2>

                    <p>
                        Submit Grievances, track their progress, receive
                        notifications and view resolution details.
                    </p>

                    <a href="Student/StudentLogin.aspx" class="role-button">
                        Student Login <span>→</span>
                    </a>
                </article>

                <!-- Department Head Portal -->
                <article class="role-card hod-card">
                    <div class="role-icon" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M3 21H21" />
                            <path d="M5 21V7L12 3L19 7V21" />
                            <path d="M9 10H9.01M15 10H15.01M9 14H9.01M15 14H15.01" />
                            <path d="M10 21V17H14V21" />
                        </svg>
                    </div>

                    <span class="role-label">DEPARTMENT MANAGEMENT</span>
                    <h2>Department Head</h2>

                    <p>
                        Review departmental Grievances, monitor progress
                        and oversee handling and resolution.
                    </p>

                    <a href="DepartmentHead/HODLogin.aspx" class="role-button">
                        HOD Login <span>→</span>
                    </a>
                </article>

                <!-- Staff Portal -->
                <article class="role-card staff-card">
                    <div class="role-icon" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <rect x="3" y="7" width="18" height="14" rx="2" />
                            <path d="M8 7V4H16V7" />
                            <path d="M3 12H21" />
                            <path d="M10 12V14H14V12" />
                        </svg>
                    </div>

                    <span class="role-label">AUTHORIZED PERSONNEL</span>
                    <h2>Staff Portal</h2>

                    <p>
                        Access assigned Grievances, update progress
                        and record relevant actions.
                    </p>

                    <a href="Staff/StaffLogin.aspx" class="role-button">
                        Staff Login <span>→</span>
                    </a>
                </article>

                <!-- Admin Portal -->
                <article class="role-card admin-card">
                    <div class="role-icon" aria-hidden="true">
                        <svg viewBox="0 0 24 24" fill="none">
                            <path d="M12 3L20 6V11C20 16 16.5 19.5 12 21C7.5 19.5 4 16 4 11V6L12 3Z" />
                            <path d="M9 12L11 14L15.5 9.5" />
                        </svg>
                    </div>

                    <span class="role-label">SYSTEM MANAGEMENT</span>
                    <h2>Admin Portal</h2>

                    <p>
                        Manage the system, oversee Grievances and access
                        administrative functions.
                    </p>

                    <a href="Admin/AdminLogin.aspx" class="role-button">
                        Admin Login <span>→</span>
                    </a>
                </article>

            </section>

            

        </main>

        <footer class="role-footer">
            <div class="container role-footer-inner">
                <div>
                    <strong>University of Southern Punjab, Multan</strong>
                    <p> USP Online Grievance Redressal System</p>
                </div>

               
            </div>
        </footer>

    </form>
</body>
</html>
