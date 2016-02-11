<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.CreateForms>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Buat Form</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript">
    function onUpload(e) {
        var files = e.files;

        $.each(files, function () {

            if (this.extension.toLowerCase() != ".doc") {
                alert("Only .doc files can be uploaded!")
                e.preventDefault();
            }

            if (this.size / 1024 / 1024 > 5) {
                alert("Max 5Mb file size is allowed!")
                e.preventDefault();
            }
        });
    }

</script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>

    <% using (Html.BeginForm("Create", "Forms", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <fieldset style="width:70%;">
            
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.formname)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextAreaFor(m => m.formname, new { style = "width: 200px;" })%>
                <%= @Html.ValidationMessageFor(m => m.formname)%> 
            </div>

            <div class="editor-label">
                <%=@Html.LabelFor(m => m.group)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= Html.DropDownListFor(m => m.group, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="HR", Value = "HR" },
                    new SelectListItem{ Text="IS", Value = "IS" },
                    new SelectListItem{ Text="PR", Value = "PR" }
                 }, "---Pilih Group---", new { Style = "width: 250px;" })%>
                <%= @Html.ValidationMessageFor(m => m.group)%> 
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.formFile)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBoxFor(m => m.formFile, new { @onkeypress = "onUpload(this)", Style = "width:350px;", type = "file" })%>
                <%= @Html.ValidationMessageFor(m => m.formFile)%> 
            </div>
           
            <br /><br />
            <p>
                <input type="submit" value="Simpan" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Kembali Ke List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>

    
<%}%>
</asp:Content>



