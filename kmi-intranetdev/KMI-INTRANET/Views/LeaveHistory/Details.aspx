<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<KMI_INTRANET.Models.LeaveModel>>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
    <h1>Leave Details</h1>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.8.3.js")%>"></script>
<script type="text/javascript">
    $(document).ready(function () {
        
        var typecuti = $("#type").val();
        if (typecuti == "CH") {
            document.getElementById("Ctahunan").style.display = "none";
            document.getElementById("Chaid").style.display = "block";
            document.getElementById("reason1").style.display = "none";
            FillHaid();
        }
        else if (typecuti == "CT") {
            document.getElementById("Ctahunan").style.display = "block";
            document.getElementById("Chaid").style.display = "none";
            document.getElementById("reason1").style.display = "block";
            FillRestLeave();
        }
       
    });

    function FillRestLeave() {
        $.getJSON("http://kmiapp/kmi-intranetdev/LeaveHistory/CheckRestLeave/" + $('#nik').val(), function (data) {
            var items = "";
            var sumrest = 0;
            $.each(data, function (i, leave) {
                items += "<tr><td>" + leave.yearleave + "</td><td>" + leave.leaveright + "</td><td>" + leave.massleave + "</td><td>" + leave.hasgotten + "</td><td style='color:red;'>" + leave.on_going + "</td><td>" + leave.restleave + "</td></tr>";
                sumrest = sumrest + leave.restleave;
            });
            items += "<tr><td colspan='5' align='right'><b>TOTAL SISA CUTI</b></td><td> " + sumrest + " </td></tr>";
            $('#rData').html(items);
        });
    };
    function FillHaid() {
        $.getJSON("http://kmiapp/kmi-intranetdev/LeaveHistory/HaidHistory/" + $('#nik').val(), function (data) {
            var items = "";
            $.each(data, function (i, haid) {
                items += "<table border=0><tr><td>1.</td><td> Haid Bulan sebelumnya</td><td>:</td><td>TANGGAL <b>" + haid.ONELAST + "</b></td></tr><tr><td>2.</td><td> Haid 2 Bulan sebelumnya</td><td>:</td><td>TANGGAL <b>" + haid.TWOLAST + "</b></td></table>";
            });
            items += "";
            $('#haid').html(items);
        });
    };
   
</script> 
<% foreach(var item in Model) { %>
   
    <fieldset style="width:80%;">
    <legend>FORM PERMOHONAN CUTI KARYAWAN / KARYAWATI</legend>
               <div class="col-md-6" style="width:30%;">NIK</div>
               <div class="col-md-6" style="width:70%;">
                <%= @Html.TextBoxFor(m => item.nik, new { id = "nik", style = "width: 100px;color:red;", disabled = "true" })%>
               </div>
               <div class="col-md-6" style="width:30%;">NAMA KARYAWAN</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.TextBoxFor(m => item.empnm, new { style = "width: 200px;color:red;", disabled = "true" })%>
               </div>
               <div class="col-md-6" style="width:30%;">BAGIAN / DEPT</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.TextBoxFor(m => item.dept, new { style = "width: 250px;color:red;", disabled = "true" })%>
               </div>
               <div class="col-md-6" style="width:30%;">TIPE CUTI</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextBoxFor(m => item.type, new { id = "type", style = "width: 50px;color:red;", disabled = "true" })%>
               </div>
               <%--<div id="DivSifatCuti" style="display:none;">
               <div class="col-md-6" style="width:30%;">SIFAT CUTI</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.RadioButtonFor(m => m.SifatCuti, "Normal", new { id = "Normal", @checked = true })%> Normal 
                <%=@Html.RadioButtonFor(m => m.SifatCuti, "Mendadak", new { id = "Mendadak" })%> Mendadak 
               </div>
               </div>--%>
			   <%--<div id="reason1" style="display:none;">
               <div class="col-md-6" style="width:30%;">SIFAT</div>
               <div class="col-md-6" style="width:70%;">
                <%= @Html.TextAreaFor(m => item.reason, new { style = "width: 200px;color:red;", disabled = "true" })%>
               </div>
			   </div>--%>
               <div class="col-md-6" style="width:30%;">DARI - SAMPAI TANGGAL</div>
               <div class="col-md-6" style="width:70%;">
                 <%= @Html.TextBoxFor(m => item.Detfrom, new { style = "width: 100px;color:red;", disabled = "true" })%>
                <%= @Html.TextBoxFor(m => item.Detto, new { style = "width: 100px;color:red;", disabled = "true" })%>
                 <%=@Html.TextBoxFor(m => item.totaldays, new { style = "width: 30px;color:red;", MaxLength = "2", disabled = "disabled" })%> Days
               </div>
               
               <div class="col-md-6" style="width:30%;">ALAMAT</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextAreaFor(m => item.alamat, new { Style = "width:250px;color:red;", disabled = "true" })%>
               </div>
               <div id="reason1" style="display:none;">
               <div class="col-md-6" style="width:30%;">ALASAN</div>
               <div class="col-md-6" style="width:70%;">
                 <%= @Html.TextAreaFor(m => item.reason, new { style = "width: 200px;color:red;", disabled = "true" })%>
               </div>
               </div>
               <div class="col-md-6" style="width:30%;">STATUS</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextAreaFor(m => item.status, new { Style = "width:250px;color:red;", disabled = "true" })%>
               </div>
            
        </fieldset>
 
<%}%> 

<fieldset id="Ctahunan" style="width:80%;display:none;">
     <div class="box" >
      <div class="box-body table-responsive">

          <table class="table table-bordered table-striped" >
                  <thead>
                  <tr>
                    <th>TAHUN</th>
                    <th>HAK CUTI S/D TGL.PERMOHONAN</th>
                    <th>CUTI MASAL</th>
                    <th>YANG SUDAH DIAMBIL</th>
                    <th>SEDANG BERJALAN</th>
                    <th>SISA CUTI</th>
                  </tr>
                  </thead>
                  <tbody id="rData">
                     
                  </tbody>         
           </table>
      </div><!-- /.box-body -->
    </div>      
</fieldset>      
<fieldset id="Chaid" style="width:80%;display:none;">
<h3>RIWAYAT CUTI HAID</h3>
<br />
<div id="haid">

</div>  
</fieldset>  
<div class="col-md-12">
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </div>
</asp:Content>


