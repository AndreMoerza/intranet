<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<KMI_INTRANET.Models.UserControlling>>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>User Control</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box" style="width: 80%;padding-left:5px;">
                                <div class="box-body table-responsive">
                                <p>
                                 <%: @Html.ActionLink("New User", "Create", null, new { @class = "btn btn-instagram" })%>
                                </p>
                                    <table id="example1" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Username</th>
                                               <th>Owner</th>
                                               <th>Level</th>
                                               <th>Autorized Hirarchy</th>
                                               <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                         <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td><%: item.username %></td>
                                                    <td><%: item.empnm %></td>
                                                    <td><%: item.level%></td>
                                                    <td><%: item.autorized%></td>
                                                    <td>
                                                        <%: @Html.ActionLink("Details", "Details", new { user = item.username })%> |
                                                        <%: @Html.ActionLink("Delete", "Delete", new { user = item.username})%>
                                                   </td>
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>
       


</asp:Content>


