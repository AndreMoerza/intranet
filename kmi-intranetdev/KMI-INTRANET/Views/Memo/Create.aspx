<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.CreateMemo>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Create New Memo</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>

    <% using (Html.BeginForm("Create", "Memo", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <fieldset style="width:70%;">
            
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.Theme)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextAreaFor(m => m.Theme, new { style = "width: 200px;" })%>
                <%= @Html.ValidationMessageFor(m => m.Theme)%> 
            </div>

            <div class="editor-label">
                <%=@Html.LabelFor(m => m.Autorize)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.RadioButtonFor(m => m.Autorize, "All", new { id = "All", Checked = "checked" })%>All 
                <%= @Html.RadioButtonFor(m => m.Autorize, "Section Above", new { id = "Section" })%>Section Above 
                <%= @Html.RadioButtonFor(m => m.Autorize, "Department", new { id = "Department" })%>Department 
<%--                
                <div id="ifCSV" style="display:none"><%= @Html.TextBoxFor(m => m.Autorize_detail)%></div>--%>
                <%= @Html.ValidationMessageFor(m => m.Autorize)%> 
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.MemoFile)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBoxFor(m => m.MemoFile, new { Style = "width:200px;", type = "file" })%>
                <%= @Html.ValidationMessageFor(m => m.MemoFile)%> 
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.ValidFrom)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
            <%= @Html.TextBoxFor(m => m.ValidFrom, new { style = "width: 100px;", @class = "datepicker" })%>
                <%= @Html.ValidationMessageFor(m => m.ValidFrom)%> 
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.ValidUntil)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
            <%= @Html.TextBoxFor(m => m.ValidUntil, new { style = "width: 100px;", @class = "datepicker" })%>
                <%= @Html.ValidationMessageFor(m => m.ValidUntil)%> 
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Create" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>

    
<%}%>
</asp:Content>


