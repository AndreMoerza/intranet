<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.CreateP2K3>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="Header" runat="server">
    <h1>Create P2K3</h1>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<script src="<%=Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>

    <% using (Html.BeginForm("Create", "P2K3", FormMethod.Post, new { enctype = "multipart/form-data" }))
       {%>
    <fieldset style="width:70%;">
            
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.title)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
               <%= @Html.TextBoxFor(m => m.title, new { style = "width: 200px;" })%>
                <%= @Html.ValidationMessageFor(m => m.title)%> 
            </div>
                        
            <div class="editor-label">
                <%= @Html.LabelFor(m => m.isi)%>
            </div>
            <div class="row">
            <div class="col-md-12">
                            <div class="box box-info">
                                
                                <div class="box-body pad">
                                 <%--<textarea id="editor1" name="editor1" rows="10" cols="80">
                                        </textarea>   --%> 
                                    <%= @Html.TextAreaFor(m => m.isi, new { style = "width: 250px;" })%>
                                    <%= @Html.ValidationMessageFor(m => m.isi)%> 
                                </div>
                            </div><!-- /.box -->

                            
            </div><!-- /.col-->
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.P2K3File)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.TextBoxFor(m => m.P2K3File, new { Style = "width:200px;", type = "file" })%>
                <%= @Html.ValidationMessageFor(m => m.P2K3File)%> 
            </div>
            <div class="editor-label">
                <%=@Html.LabelFor(m => m.stat)%>
            </div>
            <div class="input-group" style="margin-left:10px;">
                <%= @Html.RadioButtonFor(m => m.stat, "Active", new { id = "Active", Checked = "checked" })%>Active 
                <%= @Html.RadioButtonFor(m => m.stat, "No Active", new { id = "No Active" })%>No Active 
                <%= @Html.ValidationMessageFor(m => m.stat)%> 
            </div>
            <br /><br />
            <p>
                <input type="submit" value="Create" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
            </p>
        </fieldset>

    
<%}%>
</asp:Content>

