using System;

public partial class ForgotPassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Text = "";
        }
    }

    protected void btnSendResetLink_Click(
        object sender,
        EventArgs e)
    {
        lblMessage.Text = "";

        string role = ddlRole.SelectedValue;
        string email = txtEmail.Text.Trim();

        // -----------------------------------------------------
        // Basic validation
        // -----------------------------------------------------

        if (string.IsNullOrWhiteSpace(role))
        {
            ShowMessage(
                "Please select your account role.",
                false);

            return;
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            ShowMessage(
                "Please enter your registered email address.",
                false);

            return;
        }

        // -----------------------------------------------------
        // Find account
        // -----------------------------------------------------

        DALPasswordReset.PasswordResetUser user =
            DALPasswordReset.GetUserByEmail(
                role,
                email);

        // -----------------------------------------------------
        // SECURITY:
        // Do not reveal whether the account exists.
        //
        // We still return the same success message below.
        // -----------------------------------------------------

        if (user != null)
        {
            try
            {
                // -------------------------------------------------
                // Generate secure token
                // -------------------------------------------------

                string resetToken =
                    DALPasswordReset.GenerateResetToken();

                // -------------------------------------------------
                // Hash token before storing it
                // -------------------------------------------------

                string tokenHash =
                    DALPasswordReset.HashResetToken(
                        resetToken);

                // -------------------------------------------------
                // Save token
                // -------------------------------------------------

                bool tokenCreated =
                    DALPasswordReset.CreatePasswordResetToken(
                        user.UserID,
                        user.UserRole,
                        tokenHash);

                if (tokenCreated)
                {
                    // -------------------------------------------------
                    // Build reset URL
                    // -------------------------------------------------

                    string resetUrl =
                        BuildResetUrl(
                            user.UserRole,
                            resetToken);

                    // -------------------------------------------------
                    // Email reset link
                    // -------------------------------------------------

                    string emailBody =
                        BuildEmailBody(
                            user.UserRole,
                            resetUrl);

                    EmailService.SendEmail(
                        user.Email,
                        "Password Reset - USP Grievance Redressal System",
                        emailBody);
                }
            }
            catch
            {
                // -------------------------------------------------
                // Do not expose internal database/SMTP errors.
                //
                // The generic response below will still be shown.
                // -------------------------------------------------
            }
        }

        // -----------------------------------------------------
        // Generic response
        // -----------------------------------------------------

        ShowMessage(
            "If an account exists with the provided information, " +
            "a password reset link has been sent to your email. " +
            "Please check your inbox and spam folder.",
            true);
    }

    // =========================================================
    // Build password reset URL
    // =========================================================
    private string BuildResetUrl(
        string role,
        string token)
    {
        string baseUrl =
            Request.Url.GetLeftPart(
                UriPartial.Authority);

        string applicationPath =
            Request.ApplicationPath;

        if (applicationPath == "/")
        {
            applicationPath = "";
        }

        string resetUrl =
            baseUrl +
            applicationPath +
            "/ResetPassword.aspx" +
            "?role=" +
            Server.UrlEncode(role) +
            "&token=" +
            Server.UrlEncode(token);

        return resetUrl;
    }

    // =========================================================
    // Build HTML email
    // =========================================================
    private string BuildEmailBody(
        string role,
        string resetUrl)
    {
        return @"
<!DOCTYPE html>

<html>
<head>
    <meta charset='utf-8' />
</head>

<body style='margin:0; padding:0; background:#f5f7fb; font-family:Arial,sans-serif;'>

    <div style='padding:40px 20px;'>

        <div style='max-width:600px; margin:0 auto; background:#ffffff; border-radius:10px; padding:35px;'>

            <h2 style='margin-top:0; color:#123a7a;'>
                Password Reset
            </h2>

            <p style='color:#475467; line-height:1.6;'>
                We received a request to reset the password
                for your <strong>" + role + @"</strong> account
                in the Enhanced USP Online Grievance
                Redressal System.
            </p>

            <p style='color:#475467; line-height:1.6;'>
                Click the button below to create a new password.
            </p>

            <p style='margin:30px 0;'>

                <a href='" + resetUrl + @"'
                   style='display:inline-block;
                          padding:12px 24px;
                          background:#2f6fed;
                          color:#ffffff;
                          text-decoration:none;
                          border-radius:7px;
                          font-weight:bold;'>
                    Reset Password
                </a>

            </p>

            <p style='color:#667085; line-height:1.6; font-size:13px;'>
                This password reset link will expire after
                <strong>30 minutes</strong>.
            </p>

            <p style='color:#667085; line-height:1.6; font-size:13px;'>
                If you did not request a password reset,
                you can safely ignore this email.
            </p>

            <hr style='border:0; border-top:1px solid #eaecf0; margin:25px 0;' />

            <p style='color:#98a2b3; font-size:11px; margin-bottom:0;'>
                Enhanced USP Online Grievance Redressal System
            </p>

        </div>

    </div>

</body>
</html>";
    }

    // =========================================================
    // Display message
    // =========================================================
    private void ShowMessage(
        string message,
        bool success)
    {
        lblMessage.Text = message;

        if (success)
        {
            lblMessage.CssClass =
                "message-label text-success";
        }
        else
        {
            lblMessage.CssClass =
                "message-label text-danger";
        }
    }
}