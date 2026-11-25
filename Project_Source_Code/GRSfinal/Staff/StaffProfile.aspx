<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Staff/StaffMaster.master" AutoEventWireup="true" CodeFile="StaffProfile.aspx.cs" Inherits="Staff_StaffProfile" %>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Staff/css/StaffProfile.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="staff-profile-page">

        <div class="staff-profile-header">
            <h1>My Profile</h1>
            <p>View your personal and professional information.</p>
        </div>

        <asp:Panel ID="pnlProfile" runat="server" CssClass="staff-profile-card">

            <div class="staff-profile-banner">
                <div class="staff-profile-avatar">
                    <asp:Label ID="lblProfileInitials" runat="server" Text="S" />
                </div>

                <div class="staff-profile-identity">
                    <h2>
                        <asp:Label ID="lblProfileName" runat="server" />
                    </h2>
                    <p>
                        <asp:Label ID="lblProfileDesignation" runat="server" />
                    </p>
                    <span class="staff-profile-status">
                        <asp:Label ID="lblProfileStatus" runat="server" />
                    </span>
                </div>
            </div>

            <div class="staff-profile-section">
                <h3>Personal Information</h3>

                <div class="staff-profile-grid">

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Staff ID</span>
                        <asp:Label ID="lblStaffID" runat="server" CssClass="staff-profile-value" />
                    </div>

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Full Name</span>
                        <asp:Label ID="lblStaffFullName" runat="server" CssClass="staff-profile-value" />
                    </div>

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Username</span>
                        <asp:Label ID="lblStaffUsername" runat="server" CssClass="staff-profile-value" />
                    </div>

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Email Address</span>
                        <asp:Label ID="lblStaffEmail" runat="server" CssClass="staff-profile-value" />
                    </div>

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Phone Number</span>
                        <asp:Label ID="lblStaffPhone" runat="server" CssClass="staff-profile-value" />
                    </div>

                    <div class="staff-profile-field">
                        <span class="staff-profile-label">Designation</span>
                        <asp:Label ID="lblStaffDesignation" runat="server" CssClass="staff-profile-value" />
                    </div>

                </div>
            </div>

            <div class="staff-profile-section staff-profile-department">
                <h3>Department Information</h3>

                <div class="staff-profile-field">
                    <span class="staff-profile-label">Department</span>
                    <asp:Label ID="lblDepartmentName" runat="server" CssClass="staff-profile-value" />
                </div>
            </div>

        </asp:Panel>

    </div>

</asp:Content>
