
<%@ Page Title="HOD Escalations" Language="C#" MasterPageFile="~/DepartmentHead/HODMaster.master" AutoEventWireup="true" CodeFile="HODEscalations.aspx.cs" Inherits="DepartmentHead_HODEscalations" %>

<asp:Content ID="TitleContent1" ContentPlaceHolderID="TitleContent" runat="server">
    HOD Escalations
</asp:Content>

<asp:Content ID="HeadContent1" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/DepartmentHead/css/HODEscalations.css?v=1") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="hod-escalations-page">

        <div class="hod-page-heading">
            <div>
                <h2>Escalations</h2>
                <p>Review escalation requests submitted for grievances in your department.</p>
            </div>
        </div>

        <asp:Panel ID="pnlMessage" runat="server"
            CssClass="hod-alert" Visible="false">
            <asp:Label ID="lblMessage" runat="server" />
        </asp:Panel>

        <section class="hod-panel">

            <div class="hod-panel-heading">
                <div>
                    <h3>Escalation Requests</h3>
                    <p>Only requests belonging to your department are shown.</p>
                </div>

                <span class="hod-record-count">
                    <asp:Label ID="lblRecordCount" runat="server" Text="0 records" />
                </span>
            </div>

            <div class="hod-toolbar">
                <div class="hod-search-group">

                    <asp:TextBox ID="txtSearch" runat="server"
                        CssClass="form-control"
                        placeholder="Search escalation, grievance, student..." />

                    <asp:Button ID="btnSearch" runat="server"
                        Text="Search"
                        CssClass="btn hod-btn-primary"
                        OnClick="btnSearch_Click" />

                    <asp:Button ID="btnClear" runat="server"
                        Text="Clear"
                        CssClass="btn hod-btn-outline"
                        CausesValidation="false"
                        OnClick="btnClear_Click" />

                    <button type="button"
                        id="btnToggleFilters"
                        class="btn hod-btn-outline"
                        aria-expanded="false"
                        aria-controls="hodEscalationFilters">
                        <i class="fa-solid fa-filter" aria-hidden="true"></i>
                        Filters
                    </button>

                </div>
            </div>

            <div id="hodEscalationFilters"
                class="hod-filter-panel"
                hidden="hidden">

                <div class="hod-filter-field">

                    <label for="<%= ddlStatus.ClientID %>">Status</label>

                    <asp:DropDownList ID="ddlStatus" runat="server"
                        CssClass="form-control">

                        <asp:ListItem Text="All statuses" Value="" />
                        <asp:ListItem Text="Pending HOD Review" Value="Pending HOD Review" />
                        <asp:ListItem Text="Pending Admin Review" Value="Pending Admin Review" />
                        <asp:ListItem Text="Rejected by HOD" Value="Rejected by HOD" />
                        <asp:ListItem Text="Action Required" Value="Action Required" />
                        <asp:ListItem Text="Action Taken" Value="Action Taken" />
                        <asp:ListItem Text="Closed" Value="Closed" />

                    </asp:DropDownList>

                </div>

                <p class="hod-filter-hint">
                    Choose a status, then select Search to apply it.
                </p>

            </div>

            <div class="table-responsive hod-table-wrap">

                <asp:GridView ID="gvEscalations" runat="server"
                    AutoGenerateColumns="False"
                    CssClass="table hod-data-table"
                    GridLines="None"
                    DataKeyNames="escalationID"
                    EmptyDataText="No escalation requests were found."
                    OnRowCommand="gvEscalations_RowCommand">

                    <Columns>

                        <asp:TemplateField HeaderText="Escalation ID">
                            <ItemTemplate>
                                <span class="hod-id">
                                    ESC-<%# Eval("escalationID") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Grievance">
                            <ItemTemplate>
                                <div class="hod-primary-cell">
                                    GRV-<%# Eval("grievanceID") %>
                                </div>
                                <div class="hod-secondary-cell">
                                    <%#: Eval("grievanceTitle") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Student">
                            <ItemTemplate>
                                <div class="hod-primary-cell">
                                    <%#: Eval("StudentName") %>
                                </div>
                                <div class="hod-secondary-cell">
                                    <%#: Eval("stdRegNo") %>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:BoundField
                            DataField="escalationReason"
                            HeaderText="Escalation Reason" />

                        <asp:TemplateField HeaderText="Requested Date">
                            <ItemTemplate>
                                <%# Eval("requestedDate", "{0:dd MMM yyyy, hh:mm tt}") %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <span class='<%# GetEscalationStatusClass(Convert.ToString(Eval("requestStatus"))) %>'>
                                    <%#: Eval("requestStatus") %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnViewDetails" runat="server"
                                    CssClass="hod-escalation-view-details"
                                    CommandName="ViewDetails"
                                    CommandArgument='<%# Eval("escalationID") %>'
                                    CausesValidation="false">
                                    View Details
                                    
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>
        </section>
    </div>

    <script>
        (function () {
            var toggle = document.getElementById("btnToggleFilters");
            var panel = document.getElementById("hodEscalationFilters");

            if (!toggle || !panel) {
                return;
            }

            toggle.addEventListener("click", function () {
                var willOpen = panel.hasAttribute("hidden");

                if (willOpen) {
                    panel.removeAttribute("hidden");
                } else {
                    panel.setAttribute("hidden", "hidden");
                }

                toggle.setAttribute(
                    "aria-expanded",
                    willOpen ? "true" : "false"
                );
            });
        })();
    </script>

</asp:Content>
