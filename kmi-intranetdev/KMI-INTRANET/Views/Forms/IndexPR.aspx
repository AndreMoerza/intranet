<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>PR Forms</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box" style="width: 90%;padding-left:5px;">
                                <div class="box-body table-responsive">
                                
                                    <table id="example1" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Group</th>
                                               <th>Nama Form</th>
                                               <th>Aksi</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td><%: item.group %></td>
                                                    <td><%: item.formname %></td>
                                                    <td>
                                                        <%: @Html.ActionLink("Download", "Show", new { id = item.idform })%> 
                                                   </td>
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>


</asp:Content>
