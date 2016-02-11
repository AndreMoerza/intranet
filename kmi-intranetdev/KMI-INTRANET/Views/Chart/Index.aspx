<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
<h1>Organization Chart</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box" style="width: 50%;padding-left:5px;">
                                <div class="box-body table-responsive">
                                <p>
                                 <%: @Html.ActionLink("Create New", "Create", null, new { @class = "btn btn-instagram" })%>
                                </p>
                                    <table id="example1" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Title</th>
                                               <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td><%: item.title%></td>
                                                    
                                                    <td>
                                                   
                                                        <%: @Html.ActionLink("Detail", "Show", new { id = item.idchart })%> |
                                                        <%: @Html.ActionLink("Edit", "Edit", new { id = item.idchart })%> |
                                                        <%: @Html.ActionLink("Delete", "Delete", new { id = item.idchart })%>
                                                   </td>
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>
</asp:Content>


