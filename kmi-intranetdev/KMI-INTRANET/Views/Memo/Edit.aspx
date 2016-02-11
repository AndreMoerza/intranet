<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.Memo>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
<h1>Edit</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {%>

        <fieldset style="width:70%;">
             <div><%= Html.HiddenFor(model => model.idmemo)%></div>
             <div class="editor-label">Theme</div>
             <div class="input-group" style="margin-left:10px;">
                <%= Html.TextArea("Theme", Model.Theme, new { style = "width: 200px;" })%>
                <%= Html.ValidationMessage("Theme", "*")%>
            </div>
            <div class="editor-label">Autorize</div>
            <div class="input-group" style="margin-left:10px;">
                 <%= @Html.RadioButton("Autorize", "All", checked(Model.Autorize == "All" ? true : false))%>All 
                 <%= @Html.RadioButton("Autorize", "Section Above", checked(Model.Autorize == "Section Above" ? true : false))%>Section Above 
                 <%= @Html.RadioButton("Autorize", "Department", checked(Model.Autorize == "Department" ? true : false))%>Department 
            </div>
            <div class="editor-label">Memo (Pdf*)</div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBox("MemoFile", Model.MemoFile, new { style = "width: 200px;", type = "file" })%><p style="color:Blue;">Keep Empty If Not Change Nickname</p>
                <%= Html.ValidationMessage("MemoFile", "*")%>
            </div>
            <div class="editor-label">Valid From</div>
            <div class="input-group" style="margin-left:10px;">
                 <%= @Html.TextBox("ValidFrom", Model.ValidFrom, new { style = "width: 100px;", @class = "datepicker" })%>
                 <%= Html.ValidationMessage("ValidFrom", "*")%>
            </div>
            <div class="editor-label">Valid Until</div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBox("ValidUntil", Model.ValidUntil, new { style = "width: 100px;", @class = "datepicker" })%>
                <%= Html.ValidationMessage("ValidUntil", "*")%>
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Update" class="btn btn-instagram"/>
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>
        <% } %>
</asp:Content>


