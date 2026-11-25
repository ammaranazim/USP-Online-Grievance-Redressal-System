<%@ Page Title="Manage Students" Language="C#" MasterPageFile="~/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="ModifyStudent.aspx.cs" Inherits="Admin_Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <strong><span style="font-size: xx-large; color: #000099">Student Search</span></strong><br />
    <br />
    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control btn-sm mb-2" placeholder="Search by RegNo, Name, CNIC, Phone, City..." Height="34px" Width="380px"></asp:TextBox>

<asp:Button ID="btnSearch" runat="server" Text="Search"
    CssClass="btn btn-primary btn-sm mb-2" OnClick="btnSearch_Click" />

<asp:Button ID="btnClear" runat="server" Text="Clear"
    CssClass="btn btn-secondary btn-sm mb-2" OnClick="btnClear_Click" />
    
<asp:GridView

ID="GridView1"

runat="server"

CssClass="table table-bordered table-striped"

AutoGenerateColumns="False"

DataKeyNames="StdID"

OnRowEditing="GridView1_RowEditing"

 OnRowDeleting="GridView1_RowDeleting">

    <Columns>

        <asp:TemplateField HeaderText="Action">
            <ItemStyle Width="90px" />
  <ItemTemplate>

        <asp:LinkButton

            ID="btnEdit"

            runat="server"

            CommandName="Edit"

            Text="Edit"

            CssClass="btn btn-primary btn-sm mb-2">

        </asp:LinkButton>



        &nbsp;



        <asp:LinkButton

            ID="btnDelete"

            runat="server"

            CommandName="Delete"

            Text="Delete"

            CssClass="btn btn-danger btn-sm"

            OnClientClick="return confirm('Are you sure you want to delete this student?');">

        </asp:LinkButton>



    </ItemTemplate>



    <EditItemTemplate>



        <asp:LinkButton

            ID="btnUpdate"

            runat="server"

            CommandName="Update"

            Text="Update"

            CssClass="btn btn-success btn-sm">

        </asp:LinkButton>



        &nbsp;



        <asp:LinkButton

            ID="btnCancel"

            runat="server"

            CommandName="Cancel"

            Text="Cancel"

            CssClass="btn btn-secondary btn-sm">

        </asp:LinkButton>



    </EditItemTemplate>



</asp:TemplateField>

        <asp:BoundField DataField="StdID" HeaderText="StdID" ReadOnly="True" SortExpression="StdID" />

       <asp:TemplateField HeaderText="Registration No">

    <ItemTemplate>

        <%# Eval("StdRegNo") %>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtRegNo" runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdRegNo") %>'></asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>



        <asp:TemplateField HeaderText="First Name">



    <ItemTemplate>

        <%# Eval("StdFirstName") %>

    </ItemTemplate>



    <EditItemTemplate>

        <asp:TextBox

            ID="txtFirstName"

            runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdFirstName") %>'>

        </asp:TextBox>

    </EditItemTemplate>



</asp:TemplateField>

        <asp:TemplateField HeaderText="Last Name">

    <ItemTemplate>

        <%# Eval("StdLastName") %>

    </ItemTemplate>



    <EditItemTemplate>

        <asp:TextBox ID="txtLastName"

            runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdLastName") %>'>

        </asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>


        <asp:TemplateField HeaderText="Phone No">

    <ItemTemplate>

        <%# Eval("StdPhoneNo") %>

    </ItemTemplate>



    <EditItemTemplate>

        <asp:TextBox ID="txtPhoneNo"

            runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdPhoneNo") %>'>

        </asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>

        <asp:TemplateField HeaderText="Semester">

    <ItemTemplate>

        <%# Eval("StdSemester") %>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtSemester" runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdSemester") %>'></asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>

        <asp:TemplateField HeaderText="Program">

    <ItemTemplate>

        <%# Eval("StdProgram") %>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtProgram" runat="server"

            CssClass="form-control"

            Text='<%# Bind("StdProgram") %>'></asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>


        <asp:TemplateField HeaderText="Status">

    <ItemTemplate>

        <%# Eval("StdStatus") %>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control"

    SelectedValue='<%# Bind("StdStatus") %>'>



    <asp:ListItem Value="active">active</asp:ListItem>

    <asp:ListItem Value="inactive">inactive</asp:ListItem>



</asp:DropDownList>

    </EditItemTemplate>

</asp:TemplateField>

       

        <asp:TemplateField HeaderText="Email">

    <ItemTemplate>

        <%# Eval("StdEmail") %>

    </ItemTemplate>

    <EditItemTemplate>

        <asp:TextBox ID="txtEmail" runat="server"

            CssClass="form-control"

            TextMode="Email"

            Text='<%# Bind("StdEmail") %>'></asp:TextBox>

    </EditItemTemplate>

</asp:TemplateField>

    </Columns>

    </asp:GridView>
    
</asp:Content>

