using System;

public partial class ResetPassword : System.Web.UI.Page
{
    private string ResetToken
    {
        get
        {
            return Request.QueryString["token"];
        }
    }

    private string UserRole
    {
        get
        {
            return Request.QueryString["role"];
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ValidateResetLink();
        }
    }

    // =========================================================
    // Validate reset link when page opens
    // =========================================================
    private void ValidateResetLink()
    {
        string token = ResetToken;
        string role = UserRole;

        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(role))
        {
            DisableResetForm(
                "This password reset link is invalid.");
            return;
        }

        if (!IsValidRole(role))
        {
            DisableResetForm(
                "This password reset link is invalid.");
            return;
        }

        DALPasswordReset.PasswordResetTokenInfo tokenInfo =
            DALPasswordReset.ValidateResetToken(
                token,
                role);

        if (tokenInfo == null)
        {
            DisableResetForm(
                "This password reset link is invalid, expired, " +
                "or has already been used.");
            return;
        }

        // Valid token.
        lblMessage.Text = "";
        btnResetPassword.Enabled = true;
    }

    // =========================================================
    // Reset Password button
    // =========================================================
    protected void btnResetPassword_Click(
        object sender,
        EventArgs e)
    {
        lblMessage.Text = "";

        string token = ResetToken;
        string role = UserRole;

        // -----------------------------------------------------
        // Validate URL values again.
        //
        // We do NOT rely on the validation performed during
        // Page_Load because the request must be checked again
        // when the password is actually changed.
        // -----------------------------------------------------

        if (string.IsNullOrWhiteSpace(token) ||
            string.IsNullOrWhiteSpace(role))
        {
            DisableResetForm(
                "This password reset link is invalid.");

            return;
        }

        if (!IsValidRole(role))
        {
            DisableResetForm(
                "This password reset link is invalid.");

            return;
        }

        // -----------------------------------------------------
        // Validate password
        // -----------------------------------------------------

        string newPassword =
            txtNewPassword.Text;

        string confirmPassword =
            txtConfirmPassword.Text;

        if (string.IsNullOrEmpty(newPassword))
        {
            ShowMessage(
                "Please enter a new password.",
                false);

            return;
        }

        if (newPassword.Length < 8)
        {
            ShowMessage(
                "Password must be at least 8 characters long.",
                false);

            return;
        }

        if (string.IsNullOrEmpty(confirmPassword))
        {
            ShowMessage(
                "Please confirm your new password.",
                false);

            return;
        }

        if (!string.Equals(
                newPassword,
                confirmPassword,
                StringComparison.Ordinal))
        {
            ShowMessage(
                "The passwords do not match.",
                false);

            return;
        }

        // -----------------------------------------------------
        // Validate token again immediately before changing
        // password.
        // -----------------------------------------------------

        DALPasswordReset.PasswordResetTokenInfo tokenInfo =
            DALPasswordReset.ValidateResetToken(
                token,
                role);

        if (tokenInfo == null)
        {
            DisableResetForm(
                "This password reset link is invalid, expired, " +
                "or has already been used.");

            return;
        }

        // -----------------------------------------------------
        // Reset password.
        //
        // DALPasswordReset:
        // 1. Hashes password using PasswordHelper
        // 2. Updates correct account
        // 3. Marks token as used
        // 4. Performs both operations in one transaction
        // -----------------------------------------------------

        bool passwordReset =
            DALPasswordReset.ResetPassword(
                tokenInfo.ResetID,
                tokenInfo.UserID,
                tokenInfo.UserRole,
                newPassword);

        if (!passwordReset)
        {
            ShowMessage(
                "Your password could not be reset. " +
                "Please request a new reset link.",
                false);

            return;
        }

        // -----------------------------------------------------
        // Success
        // -----------------------------------------------------

        txtNewPassword.Enabled = false;
        txtConfirmPassword.Enabled = false;
        btnResetPassword.Enabled = false;

        ShowMessage(
            "Your password has been reset successfully. " +
            "You can now log in using your new password.",
            true);
    }

    // =========================================================
    // Check allowed roles
    // =========================================================
    private bool IsValidRole(string role)
    {
        return role.Equals(
                   "Student",
                   StringComparison.OrdinalIgnoreCase)
            ||
               role.Equals(
                   "Admin",
                   StringComparison.OrdinalIgnoreCase)
            ||
               role.Equals(
                   "HOD",
                   StringComparison.OrdinalIgnoreCase)
            ||
               role.Equals(
                   "Staff",
                   StringComparison.OrdinalIgnoreCase);
    }

    // =========================================================
    // Disable reset form
    // =========================================================
    private void DisableResetForm(
        string message)
    {
        txtNewPassword.Enabled = false;
        txtConfirmPassword.Enabled = false;
        btnResetPassword.Enabled = false;

        ShowMessage(
            message,
            false);
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