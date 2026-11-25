<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StudentLogin.aspx.cs" Inherits="Student_StudentLogin" %>

<!DOCTYPE html>

<html>
<head id="Head1" runat="server">

    <title>Student Login | USP Online Grievance Redressal System</title>

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

   <link href="../bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet"
href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css">

    <!-- Google Font -->
<link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet" />

    <!-- Custom CSS -->
    <link href="css/Student-Login.css" rel="stylesheet" />

</head>

<body>

<form id="form1" runat="server">

<div class="login-page">

    <div class="overlay"></div>

    <div class="container-fluid h-100">

        <div class="row no-gutters vh-100 align-items-center justify-content-center">

            <!-- Left Panel -->

            <div class="col-lg-5 d-none d-lg-flex">

                <div class="left-panel">
                    
                    <div class="left-content">

                       <img src="../Images/usp-logo.png" class="left-logo" />

                        <h2>University of Southern Punjab</h2>

                        <h4>Online Grievance Redressal System</h4>

                        <p>
                            Welcome to the Student Grievance Portal.
                            Submit, track and manage your grievances securely.
                        </p>

                        <ul class="feature-list">

                            <li>
                                <i class="fas fa-check-circle"></i>
                                Secure Grievance Submission
                            </li>

                            <li>
                                <i class="fas fa-check-circle"></i>
                                Track Grievance Status
                            </li>

                            <li>
                                <i class="fas fa-check-circle"></i>
                                Fast Department Response
                            </li>

                        </ul>

                    </div>

                </div>

            </div>

            <!-- Login Panel -->

            <div class="col-lg-5 col-md-8 col-sm-11">

                <div class="login-card">
                     
                    <div class="text-center mb-4">

                        <img src="../Images/usp-logo.png" class="logo" />

                        <h3 class="mt-3">
                            Student Login
                        </h3>

                        <p class="system-title">
                            Login using your Registration Number
                        </p>

                    </div>

                    <asp:Label ID="lblMessage"
                        runat="server"
                        CssClass="alert-message">
                    </asp:Label>

                    <!-- Registration -->

                    <div class="form-group">

                        <label>Registration Number</label>

                        <div class="input-group">

                            <div class="input-group-prepend">

                                <span class="input-group-text">

                                    <i class="fas fa-id-card"></i>

                                </span>

                            </div>

                            <asp:TextBox
                                ID="txtRegNo"
                                runat="server"
                                CssClass="form-control"
                                placeholder="BSCS-023R22-82">
                            </asp:TextBox>

                        </div>

                    </div>

                    <!-- Password -->

                    <div class="form-group">

                        <label>Password</label>

                        <div class="input-group">

                            <div class="input-group-prepend">

                                <span class="input-group-text">

                                    <i class="fas fa-lock"></i>

                                </span>

                            </div>

                            <asp:TextBox
                                ID="txtPassword"
                                runat="server"
                                ClientIDMode="Static"
                                CssClass="form-control"
                                TextMode="Password"
                                placeholder="Enter Password">
                            </asp:TextBox>

                            <div class="input-group-append">

                         <span class="input-group-text password-toggle"
                               onclick="togglePassword()">

                          <i id="eyeIcon" class="fas fa-eye"></i>

                        </span>

                        </div>

                        </div>

                    </div>

                    <!-- Remember -->

                    <div class="d-flex justify-content-between align-items-center mb-4">

    <div class="remember-section">
        <asp:CheckBox
            ID="chkRemember"
            runat="server"
            Text="Remember Me" />
    </div>

   <a href="../ForgotPassword.aspx" class="forgot-link">
    Forgot Password?
</a>

</div>
                    <!-- Login -->

                    <asp:Button
                        ID="btnLogin"
                        runat="server"
                        Text="LOGIN"
                        CssClass="btn login-btn btn-block"
                        OnClick="btnLogin_Click" />

                    <div class="login-footer">

                        © 2026 University of Southern Punjab

                    </div>

                </div>

            </div>

        </div>

    </div>



</form>

<script src="../bootstrap/js/jquery.min.js"></script>
<script src="../bootstrap/js/bootstrap.bundle.min.js"></script>
    <script>

        function togglePassword() {

            var password = document.getElementById("txtPassword");
            var eye = document.getElementById("eyeIcon");

            if (password.type === "password") {

                password.type = "text";
                eye.className = "fas fa-eye-slash";

            }
            else {

                password.type = "password";
                eye.className = "fas fa-eye";

            }

        }

</script>

</body>
</html>
