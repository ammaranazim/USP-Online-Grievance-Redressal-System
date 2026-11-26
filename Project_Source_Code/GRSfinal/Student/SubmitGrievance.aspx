<%@ Page Title="" Language="C#" MasterPageFile="~/Student/StudentMaster.master" AutoEventWireup="true" CodeFile="SubmitGrievance.aspx.cs" Inherits="Student_SubmitGrievance" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">

   <link href="<%= ResolveUrl("~/Student/css/SubmitGrievance.css?v=2") %>" rel="stylesheet" type="text/css" />

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="grievance-page">

        <!-- PAGE HEADER -->
        <div class="page-header">
            <h2>Submit New Grievance</h2>
        </div>

        <asp:Panel ID="pnlAlert" runat="server"
            CssClass="alert-box"
            Visible="false">
            <asp:Label ID="lblMessage" runat="server"></asp:Label>

</asp:Panel>
       
       

        <div class="row">

            <!-- =====================================================
                 MAIN FORM COLUMN
                 ===================================================== -->
            <div class="col-lg-9">
<!-- 1. Student Information -->
<div class="grievance-card mb-4">
    <h5 class="section-title">
        <span class="step-badge">
            <i class="fas fa-user"></i>
        </span>
        1. Student Information
        <small class="text-muted font-weight-normal">(Auto-Filled)</small>
    </h5>

    <!-- Row 1 -->
    <div class="row">

        <div class="col-md-3 mb-3">
            <label class="form-label">Full Name</label>
            <asp:TextBox ID="txtFullName" runat="server"
                CssClass="form-control"
                ReadOnly="true"
                TabIndex="-1">
            </asp:TextBox>
        </div>

        <div class="col-md-3 mb-3">
            <label class="form-label">Registration No.</label>
            <asp:TextBox ID="txtRegNo" runat="server"
                CssClass="form-control"
                ReadOnly="true"
                TabIndex="-1">
            </asp:TextBox>
        </div>

        <div class="col-md-3 mb-3">
            <label class="form-label">Program</label>
            <asp:TextBox ID="txtProgram" runat="server"
                CssClass="form-control"
                ReadOnly="true"
                TabIndex="-1">
            </asp:TextBox>
        </div>

        <div class="col-md-3 mb-3">
            <label class="form-label">Department</label>
            <asp:TextBox ID="txtStudentDept" runat="server"
                CssClass="form-control"
                ReadOnly="true"
                TabIndex="-1">
            </asp:TextBox>
        </div>

    </div>
  </div>


                <!-- 2. Grievance Details -->
<div class="grievance-card mb-4">

    <h5 class="section-title">
        <span class="step-badge">
           <i class="fas fa-pen"></i>
        </span>
        2. Grievance Details
    </h5>

    <!-- Category + Department -->
    <div class="row">

        <div class="col-md-6 mb-3">
            <label class="form-label">
                Category <span class="required-star">*</span>
            </label>

            <asp:DropDownList ID="ddlCategory"
                runat="server"
                CssClass="form-control custom-select"
                AutoPostBack="true"
                OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
            </asp:DropDownList>

            <asp:RequiredFieldValidator
                ID="rfvCategory"
                runat="server"
                ControlToValidate="ddlCategory"
                InitialValue="0"
                ErrorMessage="Please select a category"
                Display="Dynamic"
                CssClass="validation-msg">
            </asp:RequiredFieldValidator>
        </div>


        <div class="col-md-6 mb-3">
            <label class="form-label">
                Department <span class="required-star">*</span>
            </label>

            <asp:DropDownList ID="ddlDepartment"
                runat="server"
                CssClass="form-control custom-select">
            </asp:DropDownList>

            <asp:RequiredFieldValidator
                ID="rfvDepartment"
                runat="server"
                ControlToValidate="ddlDepartment"
                InitialValue="0"
                ErrorMessage="Please select a department"
                Display="Dynamic"
                CssClass="validation-msg">
            </asp:RequiredFieldValidator>
        </div>

    </div>


    <!-- Grievance Type + Subject -->
    <div class="row">

        <div class="col-md-6 mb-3">
            <label class="form-label">
                Grievance Type <span class="required-star">*</span>
            </label>

            <asp:DropDownList ID="ddlGrievanceType"
                runat="server"
                CssClass="form-control custom-select">
            </asp:DropDownList>

            <asp:RequiredFieldValidator
                ID="rfvGrievanceType"
                runat="server"
                ControlToValidate="ddlGrievanceType"
                InitialValue="0"
                ErrorMessage="Please select a grievance type"
                Display="Dynamic"
                CssClass="validation-msg">
            </asp:RequiredFieldValidator>
        </div>


        <div class="col-md-6 mb-3">
            <label class="form-label">
                Subject <span class="required-star">*</span>
            </label>

            <asp:TextBox ID="txtTitle"
                runat="server"
                CssClass="form-control"
                placeholder="Enter a short subject for your grievance"
                MaxLength="150">
            </asp:TextBox>

            <asp:RequiredFieldValidator
                ID="rfvTitle"
                runat="server"
                ControlToValidate="txtTitle"
                ErrorMessage="Subject is required"
                Display="Dynamic"
                CssClass="validation-msg">
            </asp:RequiredFieldValidator>
        </div>

    </div>


    <!-- Description -->
    <div class="mb-2">

        <label class="form-label">
            Description of Grievance
            <span class="required-star">*</span>
        </label>

        <asp:TextBox ID="txtDescription"
            runat="server"
            CssClass="form-control"
            TextMode="MultiLine"
            Rows="6"
            placeholder="Provide full details of your grievance..."
            MaxLength="1000"
            onkeyup="updateCharCount()">
        </asp:TextBox>

        <asp:RequiredFieldValidator
            ID="rfvDescription"
            runat="server"
            ControlToValidate="txtDescription"
            ErrorMessage="Description is required"
            Display="Dynamic"
            CssClass="validation-msg">
        </asp:RequiredFieldValidator>

        <small class="form-hint">
            <span id="charCount">0</span> / 1000 Characters
        </small>

    </div>

</div>

                <!-- =================================================
                     3. SUPPORTING DOCUMENTS
                     ================================================= -->
                <div class="grievance-card mb-4">

                    <h5 class="section-title">
                        <span class="step-badge">
                           <i class="fas fa-paperclip"></i>
                        </span>

                        3. Supporting Documents
                    </h5>

                    <p class="text-muted document-intro">
                        Attach any document that supports your grievance (if any).
                    </p>


                    <div class="dropzone"
                         onclick="document.getElementById('<%= fileUpload.ClientID %>').click();">

                        <i class="fas fa-cloud-upload-alt dropzone-icon"></i>

                       <p class="mb-1">
    Click below to select files
</p>

                        <p class="text-muted mb-2">
                            OR
                        </p>

                        <span class="btn btn-primary btn-sm">
                            Browse Files
                        </span>

                        <p class="text-muted mt-2 mb-0 file-info">
    Allowed Formats: DOC, DOCX, PDF, JPG, PNG
    &nbsp;|&nbsp;
    Maximum 2 files, 2 MB each
</p>

                    </div>


                    <asp:FileUpload
    ID="fileUpload"
    runat="server"
    AllowMultiple="true"
    CssClass="d-none"
    accept=".doc,.docx,.pdf,.png,.jpg"
    onchange="showSelectedFiles(this);" />


                    <div class="info-note mt-3">
    <i class="fas fa-info-circle mr-1"></i>
    You can upload up to 2 supporting documents, each 2 MB or smaller.
</div>

                </div>


                <!-- =================================================
                     4. DECLARATION
                     ================================================= -->
                <div class="grievance-card mb-4">

                    <h5 class="section-title">
                        <span class="step-badge">
                            <i class="fas fa-shield-alt"></i>
                        </span>

                        4. Declaration
                    </h5>


                    <div class="custom-control">

                        <asp:CheckBox
                            ID="chkDeclaration"
                            runat="server" />

                        <label
                            class="declaration-label"
                            for="<%= chkDeclaration.ClientID %>">

                            I hereby declare that the information provided above
                            is true and accurate to the best of my knowledge.

                        </label>

                    </div>

                </div>


                <!-- =================================================
                     ACTION BUTTONS
                     ================================================= -->
                <div class="form-actions mb-5">

                    <asp:Button
                        ID="btnCancel"
                        runat="server"
                        Text="Cancel"
                        CssClass="btn btn-outline-secondary"
                        CausesValidation="false"
                        OnClick="btnCancel_Click" />

                    <asp:Button
                        ID="btnReset"
                        runat="server"
                        Text="Reset"
                        CssClass="btn btn-light border"
                        CausesValidation="false"
                        OnClick="btnReset_Click" />

                    <asp:Button
                        ID="btnSubmit"
                        runat="server"
                        Text="Submit Grievance"
                        CssClass="btn btn-primary submit-btn"
                        OnClick="btnSubmit_Click" />

                </div>

            </div>


            <!-- =====================================================
                 RIGHT SIDE — NEED HELP
                 ===================================================== -->
            <div class="col-lg-3">

                <div class="help-card">

                    <h6>
                        <i class="fas fa-headset"></i>
                        Need Help?
                    </h6>

                    <p class="text-muted">
                        Contact Student Affairs
                    </p>


                    <div class="help-item">

                       <i class="fas fa-phone"></i>

                        <div>
                            <strong>Phone</strong>

                            <div>
                                061-6775101
                            </div>
                        </div>

                    </div>


                    <div class="help-item">

                       <i class="fas fa-envelope"></i>

                        <div>
                            <strong>Email</strong>

                            <div>
                                grievance@usp.edu.pk
                            </div>
                        </div>

                    </div>


                    <div class="help-item">

                       <i class="fas fa-clock"></i>

                        <div>
                            <strong>Office Hours</strong>

                            <div>
                                09:00 AM - 05:00 PM
                                <br />
                                (Monday to Friday)
                            </div>
                        </div>

                    </div>

                </div>

            </div>

        </div>

    </div>


    <!-- CHARACTER COUNT SCRIPT -->

    <script type="text/javascript">

        function updateCharCount() {

            var textBox =
                document.getElementById('<%= txtDescription.ClientID %>');

            var counter =
                document.getElementById('charCount');

            if (textBox && counter) {
                counter.innerText = textBox.value.length;
            }
        }

        window.addEventListener("load", function () {
            updateCharCount();
        });

    </script>
   <script type="text/javascript">

       var selectedFiles = [];

       function showSelectedFiles(input) {

           selectedFiles = Array.from(input.files);

           renderSelectedFiles();
       }

       function renderSelectedFiles() {

           var container = document.getElementById("selectedFiles");
           var input = document.getElementById("<%= fileUpload.ClientID %>");

        if (!container || !input) {
            return;
        }

        container.innerHTML = "";

        if (selectedFiles.length === 0) {
            input.value = "";
            return;
        }

        var title = document.createElement("div");
        title.className = "selected-files-title";
        title.innerHTML =
            '<i class="fas fa-paperclip"></i> Selected Files';

        container.appendChild(title);

        selectedFiles.forEach(function (file, index) {

            var item = document.createElement("div");
            item.className = "selected-file-item";

            var icon = document.createElement("span");
            icon.className = "selected-file-icon";
            icon.innerHTML = '<i class="fas fa-file"></i>';

            var details = document.createElement("div");
            details.className = "selected-file-details";

            var name = document.createElement("span");
            name.className = "selected-file-name";
            name.textContent = file.name;

            var type = document.createElement("span");
            type.className = "selected-file-type";
            type.textContent = formatFileSize(file.size);

            var removeButton = document.createElement("button");
            removeButton.type = "button";
            removeButton.className = "selected-file-remove";
            removeButton.innerHTML = '<i class="fas fa-times"></i>';
            removeButton.title = "Remove file";

            removeButton.onclick = function () {
                removeSelectedFile(index);
            };

            details.appendChild(name);
            details.appendChild(type);

            item.appendChild(icon);
            item.appendChild(details);
            item.appendChild(removeButton);

            container.appendChild(item);
        });
    }

    function removeSelectedFile(index) {

        selectedFiles.splice(index, 1);

        var input = document.getElementById("<%= fileUpload.ClientID %>");

        if (!input) {
            return;
        }

        var dataTransfer = new DataTransfer();

        selectedFiles.forEach(function (file) {
            dataTransfer.items.add(file);
        });

        input.files = dataTransfer.files;

        renderSelectedFiles();
    }

    function formatFileSize(bytes) {

        if (bytes < 1024) {
            return bytes + " B";
        }

        if (bytes < 1024 * 1024) {
            return (bytes / 1024).toFixed(1) + " KB";
        }

        return (bytes / (1024 * 1024)).toFixed(1) + " MB";
    }

</script>
</asp:Content>

    