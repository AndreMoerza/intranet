<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<KMI_INTRANET.Models.P2K3>>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
    <h1>P2K3 Details</h1>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <fieldset style="width:70%;">
    <% foreach(var item in Model) { %>
    <div style="float:right;margin-right:5%;border: 5px solid #333333 ;">
       <img src='<%= Url.Action("Show", "P2K3", new { id = item.idP2K3}) %>' height="150px" alt=""/>
    </div>
    <div class="editor-label">Title</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.TextBoxFor(m => item.title, new { style = "width: 100px;color:red;" , disabled="true" })%>
    </div>
    <div class="editor-label">News</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.TextAreaFor(m => item.isi, new { style = "width: 250px;color:red;", disabled = "true" })%>
    </div>
    <div class="editor-label">Stat</div>
    <div class="input-group" style="margin-left:10px;">
        <%= @Html.RadioButtonFor(m => item.stat, "Active", checked(item.stat == "Active" ? true : false))%>Active 
        <%= @Html.RadioButtonFor(m => item.stat, "No Active", checked(item.stat == "No Active" ? true : false))%>No Active      
    </div>
    
    <br /><br />
    <p>
    <%--<%: @Html.ActionLink("Edit", "Edit", new { id = item.nik }, new { @class = "btn btn-instagram" })%> --%>
    <%: @Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
    
    </p>
</fieldset>

<% } %>
</asp:Content>
