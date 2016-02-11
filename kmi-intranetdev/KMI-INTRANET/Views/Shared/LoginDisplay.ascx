<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
         <%: Html.ActionLink("Sign Out", "Logout", "Account",null , new {@class = "btn btn-default btn-flat" })%> 
<%
    }
    else {
%> 
         <%: Html.ActionLink("LogIn", "Login", "Account") %> 
<%
    }
%>
