<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.P2K3>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
<h1>Edit</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<%= Html.ValidationSummary("Edit was unsuccessful. Please correct the errors and try again.") %>

    <% using (Html.BeginForm())
       {%>

        <fieldset style="width:70%;">
             <div><%= Html.HiddenFor(model => model.idP2K3)%></div>
             <div class="editor-label">Title</div>
             <div class="input-group" style="margin-left:10px;">
                <%= Html.TextBox("title", Model.title, new { style = "width: 200px;" })%>
                <%= Html.ValidationMessage("Title", "*")%>
            </div>
             <div class="editor-label">News</div>
             <div class="input-group" style="margin-left:10px;">
                <%= Html.TextArea("isi", Model.isi, new { style = "width: 250px;" })%>
                <%= Html.ValidationMessage("isi", "*")%>
            </div>
            <div class="editor-label">Image (JPG/PNG*)</div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBox("NewsFile", Model.P2K3File, new { style = "width: 200px;", type = "file" })%><p style="color:Blue;">Keep Empty If Not Change Nickname</p>
                <%= Html.ValidationMessage("NewsFile", "*")%>
            </div>
            <div class="editor-label">Status</div>
            <div class="input-group" style="margin-left:10px;">
                 <%= @Html.RadioButton("stat", "Active", checked(Model.stat == "Active" ? true : false))%>Active 
                 <%= @Html.RadioButton("stat", "No Active", checked(Model.stat == "No Active" ? true : false))%>No Active 
                 
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Update" class="btn btn-instagram"/>
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>
        <% } %>
</asp:Content>

