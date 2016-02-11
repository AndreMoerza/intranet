<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<KMI_INTRANET.Models.UserControlling>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Details</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <fieldset style="width:70%;">
    <% foreach(var item in Model) { %>
    <div class="editor-label">Username</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.TextBoxFor(m => item.username, new { style = "width: 100px;color:red;" , disabled="true" })%>
    </div>
    <div class="editor-label">Password</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.PasswordFor(m => item.pass, new { style = "width: 150px;color:red;", disabled = "true" })%>
    </div>
    <div class="editor-label">Level</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.TextBoxFor(m => item.level, new { style = "width: 150px;color:red;", disabled = "true" })%>
    </div>
    <div class="editor-label">Autorized Hirarchy</div>
    <div class="input-group" style="margin-left:10px;">
    <%= Html.DropDownListFor(m => item.autorized, new SelectList(Items,"Value", "Text",true), new { @id = "chkveg", @multiple = "multiple", @class = "chosen-select", Style = "width: 400px;color:red;", disabled = "true" })%>
    
    </div>
    <br /><br />
    <p>
    <%--<%: @Html.ActionLink("Edit", "Edit", new { id = item.nik }, new { @class = "btn btn-instagram" })%> --%>
    <%: @Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
    
    </p>
</fieldset>

<% } %>
</asp:Content>


