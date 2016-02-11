<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.Forms>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
<h1>Edit</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {%>

        <fieldset style="width:70%;">
             <div><%= Html.HiddenFor(model => model.idform)%></div>
             <div class="editor-label" style="font-weight: 700;">Nama Form</div>
             <div class="input-group" style="margin-left:10px;">
                <%= Html.TextArea("formname", Model.formname, new { style = "width: 200px;" })%>
                <%= Html.ValidationMessage("formname", "*")%>
            </div>
            <div class="editor-label" style="font-weight: 700;">Group</div>
            <div class="input-group" style="margin-left:10px;">
                  <%= Html.DropDownListFor(m => m.group, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="HR", Value = "HR" },
                    new SelectListItem{ Text="IS", Value = "IS" },
                    new SelectListItem{ Text="PR", Value = "PR" }
                 }, "---Pilih Group---", new { Style = "width: 250px;" })%>
                 <%= @Html.ValidationMessageFor(m => m.group)%> 
            </div>
            <div class="editor-label" style="font-weight: 700;">Form (.Xls/.Pdf/.Docs)</div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBox("formFile", Model.formFile, new { style = "width: 350px;", type = "file" })%><p style="color:Blue;">Keep Empty If Not Change Nickname</p>
                <%= Html.ValidationMessage("formFile", "*")%>
            </div>
            
            <br /><br />
            <p>
                <input type="submit" value="Ubah" class="btn btn-instagram"/>
                <%=Html.ActionLink("Kembali Ke List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>
        <% } %>
</asp:Content>



