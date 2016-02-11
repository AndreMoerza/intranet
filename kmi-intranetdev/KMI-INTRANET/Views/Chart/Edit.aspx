<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.Chart>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
<h1>Edit</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {%>

        <fieldset style="width:70%;">
             <div><%= Html.HiddenFor(model => model.idchart)%></div>
             <div class="editor-label">Theme</div>
             <div class="input-group" style="margin-left:10px;">
                <%= Html.TextArea("title", Model.title, new { style = "width: 200px;" })%>
                <%= Html.ValidationMessage("Theme", "*")%>
            </div>
            
            <div class="editor-label">Memo (Pdf*)</div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBox("ChartFile", Model.ChartFile, new { style = "width: 200px;", type = "file" })%><p style="color:Blue;">Keep Empty If Not Change Nickname</p>
                <%= Html.ValidationMessage("ChartFile", "*")%>
            </div>
            
            <br /><br />
            <p>
                <input type="submit" value="Update" class="btn btn-instagram"/>
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>
        <% } %>
</asp:Content>



