<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.CreateChart>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Create Organization Chart</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>

    <% using (Html.BeginForm("Create", "Chart", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <fieldset style="width:70%;">
            
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.title)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextAreaFor(m => m.title, new { style = "width: 200px;" })%>
                <%= @Html.ValidationMessageFor(m => m.title)%> 
            </div>

            <div class="editor-label">
                <%=@Html.LabelFor(m => m.ChartFile)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBoxFor(m => m.ChartFile, new { Style = "width:200px;", type = "file" })%>
                <%= @Html.ValidationMessageFor(m => m.ChartFile)%> 
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Create" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>

    
<%}%>
</asp:Content>

