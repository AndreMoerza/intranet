<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.ChangePasswordModel>" %>


<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
<h1>Change Password</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>
<%= Html.ValidationSummary("Change Password was unsuccessful. Please correct the errors and try again.") %>

<% using (Html.BeginForm()) {%>
        <fieldset style="width:80%;">
            <div >
            <div style="float:right;margin-right:5%;border: 5px solid #333333 ;">
            <img src='<%= Url.Action("Show", "Account", new { id = Session["USER"] }) %>' height="250px" alt="User Image"/>
            </div>
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.Nickname)%> 
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextBoxFor(m => m.Nickname)%> <p style="color:Blue;">Keep Empty If Not Change Nickname</p>
               <%= @Html.ValidationMessageFor(m => m.Nickname)%>
            </div>
            <%--<div class="editor-label">
                <%= @Html.LabelFor(m => m.OldPassword)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.PasswordFor(m => m.OldPassword) %>
               <%= @Html.ValidationMessageFor(m => m.OldPassword)%>
            </div>--%>

            <div class="editor-label">
               <%= @Html.LabelFor(m => m.NewPassword)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%=@Html.PasswordFor(m => m.NewPassword)%> <p style="color:Blue;">Keep Empty If Not Change Password</p>
                <%=@Html.ValidationMessageFor(m => m.NewPassword)%>
            </div>

            <div class="editor-label">
                <%=@Html.LabelFor(m => m.ConfirmPassword)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%=@Html.PasswordFor(m => m.ConfirmPassword)%>
                <%=@Html.ValidationMessageFor(m => m.ConfirmPassword)%>
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Change Password" class="btn btn-instagram" />
            </p>
            </div>
        </fieldset>

<% } %>
</asp:Content>


