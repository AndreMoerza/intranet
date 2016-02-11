<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<KMI_INTRANET.Models.Popup>>" %>
<div class="box" style="width: 100%;">
                                <div class="box-body table-responsive">
                                
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
                                                    <%= @Html.ActionLink(item.Employeeid, "Index", "Popup", new { @Id = "someId" }, new { @class = "clickx", @onclick = "SetTextBoxValue(empid='" + item.Employeeid + "',empnm='" + item.Employeenm + "',department='" + item.Department + "',gender='" + item.gender + "',statuta='" + item.statuta + "',sisacuti='" + item.sisacuti + "')" })%>
                                                    </td>
                                                    <td><%: item.Employeenm %></td>
                                                    
                                                   </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                </div><!-- /.box-body -->
                            </div>
