<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminStudents.aspx.cs" Inherits="Admin_Students" %>

<asp:Content ID="StudentsHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%= ResolveUrl("~/Admin/css/AdminStudents.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="StudentsContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="students-page">

        <!-- PAGE HEADER -->
        <div class="students-page-header">
            <div>
                <h2>Students</h2>
                <p>View and manage registered student records.</p>
            </div>

            <asp:HyperLink
                ID="lnkRegisterStudent"
                runat="server"
                NavigateUrl="~/Admin/RegisterStudent.aspx"
                CssClass="student-primary-btn">
                <span class="student-btn-symbol">+</span>
                Register Student
            </asp:HyperLink>
        </div>

        <!-- MESSAGE -->
        <asp:Label
            ID="lblMessage"
            runat="server"
            CssClass="students-message"
            EnableViewState="false" />

        <!-- SEARCH AND FILTERS -->
        <div class="students-filter-card">

            <div class="students-filter-toolbar">

                <!-- SEARCH INPUT -->
                <div class="student-search-field">
                    <asp:TextBox
                        ID="txtSearch"
                        runat="server"
                        CssClass="student-form-control"
                        MaxLength="100"
                        placeholder="Search by registration no., name, email or CNIC..." />
                </div>

                <!-- SEARCH BUTTON -->
                <asp:Button
                    ID="btnSearch"
                    runat="server"
                    Text="Search"
                    CssClass="student-toolbar-btn student-search-btn"
                    CausesValidation="false"
                    OnClick="btnApplyFilters_Click" />

                <!-- CLEAR BUTTON -->
                <asp:Button
                    ID="btnClear"
                    runat="server"
                    Text="Clear"
                    CssClass="student-toolbar-btn student-clear-btn"
                    CausesValidation="false"
                    OnClick="btnReset_Click" />

                <!-- FILTER TOGGLE -->
                <button
                    type="button"
                    id="btnToggleStudentFilters"
                    class="student-toolbar-btn student-filter-toggle"
                    onclick="return toggleStudentFilters();"
                    aria-expanded="false"
                    aria-controls="studentAdvancedFilters">

                    <span class="student-filter-icon">&#9881;</span>
                    <span>Filters</span>

                </button>

            </div>

            <!-- COLLAPSIBLE ADVANCED FILTERS -->
            <div id="studentAdvancedFilters" class="student-advanced-filters">

                <div class="student-advanced-filter-grid">

                    <!-- DEPARTMENT -->
                    <div class="student-filter-field">

                        <asp:Label
                            ID="lblDepartment"
                            runat="server"
                            AssociatedControlID="ddlDepartment">
                            Department
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlDepartment"
                            runat="server"
                            CssClass="student-form-control" />

                    </div>

                    <!-- STATUS -->
                    <div class="student-filter-field">

                        <asp:Label
                            ID="lblStatus"
                            runat="server"
                            AssociatedControlID="ddlStatus">
                            Status
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlStatus"
                            runat="server"
                            CssClass="student-form-control">

                            <asp:ListItem Text="All Statuses" Value="" />
                            <asp:ListItem Text="Active" Value="Active" />
                            <asp:ListItem Text="Inactive" Value="Inactive" />

                        </asp:DropDownList>

                    </div>

                    <!-- PROGRAM -->
                    <div class="student-filter-field">

                        <asp:Label
                            ID="lblProgram"
                            runat="server"
                            AssociatedControlID="ddlProgram">
                            Program
                        </asp:Label>

                        <asp:DropDownList
                            ID="ddlProgram"
                            runat="server"
                            CssClass="student-form-control" />

                    </div>

                    <!-- SEMESTER -->
                    <div class="student-filter-field">

                        <asp:Label
                            ID="lblSemester"
                            runat="server"
                            AssociatedControlID="ddlSemester">
                            Semester
                        </asp:Label>

                       <asp:DropDownList
        ID="ddlSemester"
        runat="server"
        CssClass="student-form-control">
    </asp:DropDownList>

                    </div>

                </div>

            </div>

        </div>

        <!-- STUDENT RECORDS TABLE -->
        <div class="students-table-card">

            <div class="students-table-header">

                <div>
                    <h3>Student Records</h3>
                    <p>Review student information and open a record to edit it.</p>
                </div>

                <div class="students-record-count">
                    <asp:Label
                        ID="lblRecordCount"
                        runat="server"
                        Text="0 students" />
                </div>

            </div>

            <div class="students-table-wrapper">

                <asp:GridView
                    ID="gvStudents"
                    runat="server"
                    AutoGenerateColumns="False"
                    DataKeyNames="stdID,stdStatus"
                    CssClass="students-table"
                    GridLines="None"
                    EmptyDataText="No students found for the selected criteria."
                    OnRowCommand="gvStudents_RowCommand">

                    <Columns>

                        <asp:BoundField
                            DataField="stdRegNo"
                            HeaderText="Registration No." />

                      
                        <asp:TemplateField HeaderText="Student Name">
                            <ItemTemplate>
                                <span class="student-name">
                                    <%#: (Convert.ToString(Eval("stdFirstName")) + " " +
                                          Convert.ToString(Eval("stdLastName"))).Trim() %>
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>

                      
                        <asp:BoundField
                            DataField="stdEmail"
                            HeaderText="Email" />

                     
                        <asp:BoundField
                            DataField="deptName"
                            HeaderText="Department"
                            NullDisplayText="—" />

                
                        <asp:BoundField
                            DataField="stdProgram"
                            HeaderText="Program"
                            NullDisplayText="—" />

                   
                        <asp:BoundField
                            DataField="stdSemester"
                            HeaderText="Semester"
                            NullDisplayText="—" />

                    
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>

                                <span class='<%# "student-status " +
                                    (String.Equals(
                                        Convert.ToString(Eval("stdStatus")),
                                        "Active",
                                        StringComparison.OrdinalIgnoreCase)
                                        ? "status-active"
                                        : "status-inactive") %>'>

                                    <%#: Eval("stdStatus") %>

                                </span>

                            </ItemTemplate>
                        </asp:TemplateField>

                     
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>

                                <div class="student-row-actions">

                                    <asp:LinkButton
                                        ID="btnEditStudent"
                                        runat="server"
                                        Text="Edit"
                                        CssClass="student-edit-link"
                                        CommandName="EditStudent"
                                        CommandArgument='<%# Eval("stdID") %>'
                                        CausesValidation="false" />

                            
                                    <asp:LinkButton
                                        ID="btnChangeStudentStatus"
                                        runat="server"
                                        Text='<%# String.Equals(
                                            Convert.ToString(Eval("stdStatus")),
                                            "Active",
                                            StringComparison.OrdinalIgnoreCase)
                                            ? "Deactivate" : "Activate" %>'
                                        CssClass='<%# String.Equals(
                                            Convert.ToString(Eval("stdStatus")),
                                            "Active",
                                            StringComparison.OrdinalIgnoreCase)
                                            ? "student-delete-link" : "student-edit-link" %>'
                                        CommandName="ChangeStudentStatus"
                                        CommandArgument='<%# Eval("stdID") %>'
                                        CausesValidation="false"
                                        OnClientClick="return confirm('Are you sure you want to change this student status?');" />

                                </div>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>

                </asp:GridView>

            </div>

        </div>

    </div>

    <!-- FILTER TOGGLE SCRIPT -->
    <script>
        function toggleStudentFilters() {

            var panel = document.getElementById("studentAdvancedFilters");
            var button = document.getElementById("btnToggleStudentFilters");

            if (!panel || !button) {
                return false;
            }

            var isOpen = panel.classList.contains("show");

            panel.classList.toggle("show", !isOpen);
            button.classList.toggle("active", !isOpen);

            button.setAttribute("aria-expanded", String(!isOpen));

            return false;
        }
    </script>

</asp:Content>