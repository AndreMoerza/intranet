<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Memo</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box" style="width: 90%;padding-left:5px;">
                                <div class="box-body table-responsive">
                                <p>
                                 <%: @Html.ActionLink("Create New Memo", "Create", null, new { @class = "btn btn-instagram" })%>
                                </p>
                                    <table id="example1" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Memo Theme</th>
                                               <th>Autorized</th>
                                               <th>Valid From</th>
                                               <th>Valid Until</th>
                                               <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td><%: item.Theme%></td>
                                                    <td><%: item.Autorize%></td>
                                                    <td><%: item.ValidFrom%></td>
                                                    <td><%: item.ValidUntil%></td>
                                                    <td>
                                                   
                                                        <%: @Html.ActionLink("Detail", "Show", new { id = item.idmemo })%> |
                                                        <%: @Html.ActionLink("Edit", "Edit", new { id = item.idmemo })%> |
                                                        <%: @Html.ActionLink("Delete", "Delete", new { id = item.idmemo })%>
                                                   </td>
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>


</asp:Content>


