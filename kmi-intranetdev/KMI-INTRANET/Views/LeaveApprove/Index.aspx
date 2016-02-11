<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<KMI_INTRANET.Models.LeaveApprove>>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
    <h1>Leave Approval</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="box" style="width: 100%;padding-left:5px;">
                                <div class="box-body table-responsive">
                                
                                    <table id="example1" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Employee Id</th>
                                               <th>Employee Name</th>
                                               <th>Leave Type</th>
                                               <th>From Date</th>
                                               <th>To Date</th>
                                               <th>Total Days</th>
                                               <th>Status</th>
                                               <th>Action</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td><%: item.nik %></td>
                                                    <td><%: item.empnm %></td>
                                                    <td><%: item.namacuti %></td>
                                                    <td><%: item.fromleave%></td>
                                                    <td><%: item.toleave%></td>
                                                    <td><%: item.totaldays%></td>
                                                    <td><%: item.deskripsi%></td>
                                                    <td>
                                                     <%: @Html.ActionLink("Details", "Details", new { nik = item.nik, fromdate = DateTime.Parse(item.fromleave.ToString()), todate = DateTime.Parse(item.toleave.ToString())})%> |
                                                     <%: @Html.ActionLink("Approve", "Approve", new { nik = item.nik, fromdate = DateTime.Parse(item.fromleave.ToString()), todate = DateTime.Parse(item.toleave.ToString()), reason = item.reason, selectitem = item.namacuti, alamat = item.alamat })%> |
                                                     <%: @Html.ActionLink("Reject", "Reject", new { nik = item.nik, fromdate = DateTime.Parse(item.fromleave.ToString()), todate = DateTime.Parse(item.toleave.ToString()), reason = item.reason, selectitem = item.namacuti, alamat = item.alamat })%> 
                                                   </td>
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>


</asp:Content>


