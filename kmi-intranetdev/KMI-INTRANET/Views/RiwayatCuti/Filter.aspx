<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.SearchModel>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
    <h1>Riwayat Cuti</h1>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.8.3.js")%>"></script>
<script type="text/javascript">
    $(function () {
        var overlay = $("<div id=overlay></div>");
        $(".close").click(function () {
            $(".popup").hide();
            overlay.appendTo(document.body).remove();
            return false;
        });

        $(".x").click(function () {
            $(".popup").hide();
            overlay.appendTo(document.body).remove();
            return false;
        });

        $(".click").click(function () {
            overlay.show();
            overlay.appendTo(document.body);
            $(".popup").show();

            return false;
        });
        $(".clickx").click(function () {
            $(".popup").hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('#tgl').datepicker({ dateFormat: 'dd/mm/yy', daysOfWeekDisabled: [0, 6], autoclose: true }).on('changeDate', function (ev) {

        });
       
        $("#clear").click(function () {
            $('#empid').val("");
            $('#tgl').val("");
            $('#autorized').val("");
        });
    });
    function SetTextBoxValue(data) {
        $('#empid').val(data);
    };
</script>

<div class="popup">
   <div class="content">
   <img src="<%=@Url.Content("~/Content/img/close1.gif")%>" alt="Close" class="x" id="Img1" />
   <h4><b>Employee Master</b></h4>
   <hr />
       <% Html.RenderAction("Popup", "UserControl"); %>
   </div>
</div>
<% using (Html.BeginForm("Filter", "RiwayatCuti", FormMethod.Post))
   {%>
<div class="box" style="width: 100%;padding-left:5px;">
<fieldset id="filter" style="width:90%;">
 <div class="col-md-6" style="width:30%;">NIK</div>
 <div class="col-md-6" style="width:70%;">
  <%=@Html.TextBoxFor(m => m.empid, new { style = "width: 100px;" })%>
  <a href="" class="click"><img src="<%=@Url.Content("~/Content/img/search.png")%>" id="pops" class="search" alt="Search"/></a>
 </div> 
 <div class="col-md-6" style="width:30%;">Tanggal</div>
 <div class="col-md-6" style="width:70%;">
  <%=@Html.TextBoxFor(m => m.tgl, new { style = "width: 100px;" })%>
 </div> 
 <div class="col-md-6" style="width:30%;">Hirarki</div>
 <div class="col-md-6" style="width:70%;">
 <%= Html.DropDownListFor(m => m.autorized, new SelectList(Model.Items, "Value", "Text"), new { @id = "chkveg", @multiple = "multiple", @class = "chosen-select", Style = "width: 400px;height:" })%>
  </div> 
  <%--<div class="col-md-6" style="width:30%;">Bentuk</div>
  <div class="col-md-6" style="width:70%;">
    <%= @Html.RadioButton("bentuk", "Table", new { id = "Table", Checked = "checked" })%>Table 
    <%= @Html.RadioButton("bentuk", "Chart", new { id = "Chart" })%>Chart 
  </div>--%> 
 <div class="col-md-12">
   <br />
   <input type="submit" name="btn1" value="Tampilkan" class="btn btn-instagram"/> 
    <input type="submit" name="btn1" value="Bersihkan" id="clear" class="btn btn-instagram"/>
 </div>
</fieldset> 
<%}%> 
<fieldset id="datashow" style="width:90%;">
 <div class="box" >
      <div class="box-body table-responsive">
      <%  if (Model.FilterTable != null && Model.FilterTable.Count > 0)
     { %>
          <table class="table table-bordered table-striped" >
                  <thead>
                  <tr>
                    <th>Employee Id</th>
                    <th>Employee Name</th>
                    <th>Leave Type</th>
                    <th>From</th>
                    <th>To</th>
                    <th>Day(s)</th>
                  </tr>
                  </thead>
                  <tbody id="rData">
                    <% foreach (var item in Model.FilterTable)
                       { %>
                       <tr> 
                         <td><%: item.empid%></td>
                         <td><%: item.empnm %></td>
                         <td><%: item.leavetype%></td>
                         <td><%: item.fromleave%></td>
                         <td><%: item.toleave%></td>
                         <td><%: item.days%></td>
                       </tr>
                   <% } %>
                  </tbody>   
          </table>
           <% } %>
      </div>
      <div style="float:right;">
      <a href="<%= Url.Action("ExportToExcel","RiwayatCuti") %>" ><img src="<%=@Url.Content("~/Content/img/excel.png")%>" alt="exp" width="30px" height="30px"/></a>
      </div>
    </div>    
</fieldset>

</div>

</asp:Content>
