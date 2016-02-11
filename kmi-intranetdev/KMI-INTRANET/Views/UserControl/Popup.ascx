<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<KMI_INTRANET.Models.EmpidPopup>>" %>

<div class="box" style="width: 100%;">
                                <div class="box-body table-responsive">
                                <p>
                                 
                                </p>
                                    <table id="example2" class="table table-bordered table-striped" >
                                        <thead>
                                            <tr>
                                               <th>Employee ID</th>
                                               <th>Employee Name</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        <% foreach(var item in Model)
                                              { %>
                                                   <tr> 
                                                    <td>
                                                    <%= @Html.ActionLink(item.Employeeid, "Index", "Popup", new { @Id = "someId" }, new { @class = "clickx", @onclick = "SetTextBoxValue(empid='" + item.Employeeid + "')" })%>
                                                    </td>
                                                    <td><%: item.Employeenm %></td>
                                                    
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>