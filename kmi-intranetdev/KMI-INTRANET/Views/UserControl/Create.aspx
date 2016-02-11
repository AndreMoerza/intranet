<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.NewUser>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>New User</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.8.3.js")%>"></script>
<script type='text/javascript'>
    $(function () {
        var overlay = $('<div id="overlay"></div>');
        $('.close').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });

        $('.x').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('.x1').click(function () {
            $('.popup1').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('.click').click(function () {
            overlay.show();
            overlay.appendTo(document.body);
            $('.popup').show();

            return false;
        });
        $('.click1').click(function () {
            overlay.show();
            overlay.appendTo(document.body);
            $('.popup1').show();

            return false;
        });
        $('.clickx').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });

    });
    function SetTextBoxValue(data) {
        $('#someId').val(empid)
    };
</script> 
<div class='popup'>
<div class='content'>
<img src="<%=Url.Content("~/Content/img/close1.gif")%>" alt='Close' class='x' id='Img1' />
<h4><b>Employee Master</b></h4>
  <hr />
   <% Html.RenderAction("Popup", "UserControl"); %>
  </div>
</div>
<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>
<%= Html.ValidationSummary("") %>

    <% using (Html.BeginForm()) {%>
    <fieldset style="width:70%;">
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.User)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextBoxFor(m => m.User, new { id = "someId", style = "width: 100px;" })%>
               <a href='' class='click'><img src="<%=Url.Content("~/Content/img/search.png")%>" class="search"/></a>
               <%= @Html.ValidationMessageFor(m => m.User)%> 
            </div>
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.Pass)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.PasswordFor(m => m.Pass, new {value = Model.Pass, style = "width: 150px;" })%>
               <%= @Html.ValidationMessageFor(m => m.Pass)%> 
            </div>

            <div class="editor-label">
            <%= Html.LabelFor(m => m.Level)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
            <%= Html.DropDownListFor(m => m.Level, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="ADMIN", Value = "ADMIN" },
                    new SelectListItem{ Text="SECTION HEAD", Value = "SECTION HEAD" },
                    new SelectListItem{ Text="DEPARTMENT HEAD", Value = "DEPARTMENT HEAD" },
                    new SelectListItem{ Text="ADMIN BAGIAN", Value = "ADMIN BAGIAN" },
                    new SelectListItem{ Text="USER", Value = "USER" }
                 }, new { Style = "width: 150px;" })%>
                 <%= @Html.ValidationMessageFor(m => m.Level)%> 
            </div>
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.Autorized)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
             <%= Html.DropDownListFor(m => m.Autorized, new SelectList(Model.Items, "Value","Text"), new { @id = "chkveg", @multiple = "multiple", @class = "chosen-select", Style = "width: 400px;" })%>
                <%--<%= @Html.ValidationMessageFor(m => m.Autorized)%>--%>
            </div>

            <br /><br />
            <p>
                <input type="submit" value="Create" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>
<%}%>

</asp:Content>


