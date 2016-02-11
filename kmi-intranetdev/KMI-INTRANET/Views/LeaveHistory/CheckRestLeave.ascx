<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<KMI_INTRANET.Models.RestLeave>>" %>


    <div class="box" >
      <div class="box-body table-responsive">

          <table class="table table-bordered table-striped" >
                  <thead>
                  <tr>
                    <th>TAHUN</th>
                    <th>HAK CUTI S/D TGL.PERMOHONAN</th>
                    <th>YANG SUDAH DIAMBIL</th>
                    <th>SISA CUTI</th>
                  </tr>
                  </thead>
                  <tbody>
                     <% foreach(var item in Model)
                        { %>
                        <tr> 
                           <td><%: item.yearleave%></td>
                           <td><%: item.leaveright%></td>
                           <td><%: item.hasgotten%></td>
                           <td><%: item.restleave%></td>
                        </tr>
                     <% } %>
                   </tbody>         
           </table>
      </div><!-- /.box-body -->
    </div>   