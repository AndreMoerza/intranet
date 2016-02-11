<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/SiteMaster.Master" Inherits="System.Web.Mvc.ViewPage<KMI_INTRANET.Models.LeaveRequest>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="Header" runat="server">
    <h1>New Request</h1>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<script type="text/javascript" src="<%=Url.Content("~/Scripts/jquery-1.8.3.js")%>"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var username = '<%= Session["UserSecurity"] %>';
        if (username == "USER") {
            Fill();
            document.getElementById("pops").style.display = "none";
        }
    });
    $(function () {
        document.getElementById("reason").style.display = "none";
        document.getElementById("reason1").style.display = "none";
        document.getElementById("Ctahunan").style.display = "none";
        document.getElementById("Chaid").style.display = "none";
        document.getElementById("DivSifatCuti").style.display = "none";
        document.getElementById("SelectedItem").selectedIndex = "0";
        $('#empid').val("");
        $('#empnm').val("");
        $('#departement').val("");
        $('#gender').val("");
        $('#statuta').val("");
        $('#sisacuti').val("");
        $("#times").val("");
        $("#tglkej").val("");
        $("#hpl").val("");
        $("#fromleave").val("");
        $('#toleave').val("");
        $('#hari').val("");
        var overlay = $('<div id="overlay"></div>');
        var dateNow = new Date();
        $('.close').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });

        $('.x').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('.x1').click(function () {
            $('.popup1').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('.click').click(function () {
            overlay.show();
            overlay.appendTo(document.body);
            $('.popup').show();

            return false;
        });
        $('.click1').click(function () {
            overlay.show();
            overlay.appendTo(document.body);
            $('.popup1').show();

            return false;
        });
        $('.clickx').click(function () {
            $('.popup').hide();
            overlay.appendTo(document.body).remove();
            return false;
        });
        $('#SelectedItem').change(function () {
            var typecuti = $("#SelectedItem").val();
            var gender = $("#gender").val();
            var sisacuti = $("#sisacuti").val();
            document.getElementById("detail").selectedIndex = "0";
            document.getElementById("detail1").selectedIndex = "0";
            document.getElementById("reasonpost").selectedIndex = "0";
            if (typecuti == "CT") {
                if (sisacuti == 0 || sisacuti == null) {
                    alert("Sisa cuti anda " + sisacuti + ", Anda tidak dapat mengajukan cuti !");
                }
                else {
                    document.getElementById("toleave").readOnly = false;
                    $('#toleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true });
                    document.getElementById("toleave").style.background = "";
                    document.getElementById("reason").style.display = "block";
                    document.getElementById("reason1").style.display = "none";
                    document.getElementById("Ctahunan").style.display = "block";
                    document.getElementById("Chaid").style.display = "none";
                    document.getElementById("DivSifatCuti").style.display = "block";
                    document.getElementById("Message").style.display = "none";
                    document.getElementById("HPL").style.display = "none";
                    document.getElementById("TGLKEJ").style.display = "none";
                    FillRestLeave();
                }
            }
            else if (typecuti == "DI") {
                document.getElementById("toleave").readOnly = true;
                $('#toleave').datepicker('remove');
                document.getElementById("toleave").style.background = "#F0F0F0";
                document.getElementById("reason").style.display = "none";
                document.getElementById("reason1").style.display = "block";
                document.getElementById("Ctahunan").style.display = "none";
                document.getElementById("Chaid").style.display = "none";
                document.getElementById("DivSifatCuti").style.display = "none";
                document.getElementById("Message").style.display = "block";
                document.getElementById("HPL").style.display = "none";
                document.getElementById("TGLKEJ").style.display = "block";
            }
            else if (typecuti == "CH") {
                $('#toleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true });
                if (gender == "Female") {
                    document.getElementById("toleave").readOnly = false;
                    document.getElementById("toleave").style.background = "";
                    document.getElementById("reason").style.display = "none";
                    document.getElementById("reason1").style.display = "none";
                    document.getElementById("Ctahunan").style.display = "none";
                    document.getElementById("Chaid").style.display = "block";
                    document.getElementById("DivSifatCuti").style.display = "none";
                    document.getElementById("Message").style.display = "none";
                    document.getElementById("HPL").style.display = "none";
                    document.getElementById("TGLKEJ").style.display = "none";
                    FillHaid();
                }
                /*else {
                document.getElementById("detail").style.display = "none";
                document.getElementById("reason").style.display = "none";
                document.getElementById("reason1").style.display = "none";
                document.getElementById("Ctahunan").style.display = "none";
                document.getElementById("Chaid").style.display = "none";
                document.getElementById("DivSifatCuti").style.display = "none";
                document.getElementById("reasonpost").selectedIndex = "0";
                document.getElementById("SelectedItem").selectedIndex = "0";
                alert("You Dont Have Right Women Periodical Leave");
                    
                }*/
            }
            else if (typecuti == "I") {
                $('#toleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true });
                document.getElementById("toleave").readOnly = false;
                document.getElementById("toleave").style.background = "";
                document.getElementById("reason").style.display = "block";
                document.getElementById("reason1").style.display = "none";
                document.getElementById("Ctahunan").style.display = "none";
                document.getElementById("Chaid").style.display = "none";
                document.getElementById("DivSifatCuti").style.display = "none";
                document.getElementById("Message").style.display = "block";
                document.getElementById("HPL").style.display = "none";
                document.getElementById("TGLKEJ").style.display = "none";
            }
            else if (typecuti == "CI") {
                $('#toleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true });
                document.getElementById("toleave").readOnly = false;
                document.getElementById("toleave").style.background = "";
                document.getElementById("reason").style.display = "block";
                document.getElementById("reason1").style.display = "none";
                document.getElementById("Ctahunan").style.display = "none";
                document.getElementById("Chaid").style.display = "none";
                document.getElementById("DivSifatCuti").style.display = "none";
                document.getElementById("Message").style.display = "block";
                document.getElementById("HPL").style.display = "none";
                document.getElementById("TGLKEJ").style.display = "none";
            }
            else if (typecuti == "CM") {
                document.getElementById("toleave").readOnly = true;
                $('#toleave').datepicker('remove');
                document.getElementById("toleave").style.background = "#F0F0F0";
                document.getElementById("reason").style.display = "none";
                document.getElementById("reason1").style.display = "none";
                document.getElementById("Ctahunan").style.display = "none";
                document.getElementById("Chaid").style.display = "none";
                document.getElementById("DivSifatCuti").style.display = "none";
                document.getElementById("Message").style.display = "block";
                document.getElementById("HPL").style.display = "block";
                document.getElementById("TGLKEJ").style.display = "none";
            }
            $("#fromleave").val("");
            $("#toleave").val("");
            $("#hari").val("");
            $("#times").val("");
            $("#tglkej").val("");
            $("#hpl").val("");
        });
        $('#reasonpost').change(function () {
            $("#fromleave").val("");
            $("#toleave").val("");
            $("#hari").val("");
            var typeDi = $("#reasonpost").val();
            if (typeDi == "Perkawinan") {
                document.getElementById("detail").style.display = "block";
                document.getElementById("detail1").style.display = "none";
            }
            else if (typeDi == "Meninggal Dunia") {
                document.getElementById("detail1").style.display = "block";
                document.getElementById("detail").style.display = "none";
            }
            else {
                document.getElementById("detail").style.display = "none";
                document.getElementById("detail1").style.display = "none";
            }

        });
        $('#detail').change(function () {
            var det = $("#detail").val();
            var statuta = $('#statuta').val();
            if (det == "Pekerja") {
                if (statuta == "MENIKAH") {
                    document.getElementById("reason").style.display = "none";
                    document.getElementById("Ctahunan").style.display = "none";
                    document.getElementById("Chaid").style.display = "none";
                    document.getElementById("DivSifatCuti").style.display = "none";
                    document.getElementById("detail").selectedIndex = "0";
                    alert("You Have Used The Merried Permission !");
                }
            }
        });
        $('#tglkej').datepicker({ dateFormat: 'mm/dd/yy', autoclose: true }).on('changeDate', function (ev) {

        });
        $('#hpl').datepicker({ dateFormat: 'mm/dd/yy', autoclose: true }).on('changeDate', function (ev) {

        });
        $('#times').datetimepicker({
            format: 'HH:mm',
            defaultDate: dateNow
        });
        $('#fromleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true }).on('changeDate', function (ev) {
            var typecuti = $("#SelectedItem").val();
            var typeDi = $("#reasonpost").val();
            var det = $("#detail").val();
            var det1 = $("#detail1").val();
            var selectedDate = $("#toleave").val();
            var arr = selectedDate.split("/");
            var date1 = new Date(arr[2] + "-" + arr[1] + "-" + arr[0]);
            var time = $("#times").val();
            var tglkej = $("#tglkej").val();
            var aw = tglkej.split("/");
            var timeplan = "13:00";
            if (time <= timeplan) {
                var kej = new Date(aw[2] + "-" + aw[1] + "-" + aw[0]);
            }
            else {
                var kej1 = new Date(aw[2] + "-" + aw[1] + "-" + aw[0]);
                var kej2 = kej1.getDate() + 1
                var kej3 = ((kej1.getMonth() + 1) < 10 ? '0' : '') + (kej1.getMonth() + 1);
                var kej4 = kej1.getFullYear();
                var kej = new Date(kej4 + "-" + kej3 + "-" + kej2);
            }
            var fromdate = $("#fromleave").val();
            var arrr = fromdate.split("/");
            var date2 = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
            if (date1 < date2) {
                if (typecuti != "DI" && typecuti != "CM") {
                    //$("#fromleave").val("");
                    $('#toleave').val("");
                    $('#hari').val("");
                    alert("Tanggal Akhir Lebih Kecil Dari Tanggal Mulai");
                }
                else {
                    if (date2 > kej) {
                        var min = calcBusinessDays(kej, date2) + 1;
                    }
                    else {
                        var min = calcBusinessDays(date2, kej);
                    }

                    if (min > 1) {
                        alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Pernikahan !");
                        jmlhari = 0;
                        $("#fromleave").val("");
                        $('#toleave').val("");
                        $('#hari').val("");
                    }
                    else {
                        $('#toleave').val("");
                        $('#hari').val("");
                    }
                }
            }
            else {
                if (typecuti == "DI") {
                    if (typeDi == "Perkawinan") {
                        if (date2 == kej) {

                            if (det == "Pekerja") {
                                jmlhari = 3;
                            }
                            else if (det == "Anak") {
                                jmlhari = 2;
                            }
                            else if (det == "Saudara") {
                                jmlhari = 1;
                            }
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Pernikahan !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                if (det == "Pekerja") {
                                    jmlhari = 3;
                                }
                                else if (det == "Anak") {
                                    jmlhari = 2;
                                }
                                else if (det == "Saudara") {
                                    jmlhari = 1;
                                }
                            }
                        }
                    }
                    else if (typeDi == "Meninggal Dunia") {
                        if (date2 == kej) {
                            if (det1 == "Istri/Suami/Orangtua") {
                                jmlhari = 3;
                            }
                            else if (det1 == "Mertua/Menantu/Saudara Kandung") {
                                jmlhari = 2;
                            }
                            else if (det1 == "Nenek/Kakek") {
                                jmlhari = 1;
                            }
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Kejadian !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                if (det1 == "Istri/Suami/Orangtua") {
                                    jmlhari = 3;
                                }
                                else if (det1 == "Mertua/Menantu/Saudara Kandung") {
                                    jmlhari = 2;
                                }
                                else if (det1 == "Nenek/Kakek") {
                                    jmlhari = 1;
                                }
                            }
                        }
                    }
                    else if (typeDi == "Melahirkan/Keguguran") {
                        if (date2 == kej) {
                            jmlhari = 3;
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Kejadian !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                jmlhari = 3;
                            }
                        }
                    }
                    else if (typeDi == "Pengkhitanan/Baptisan") {
                        /*if (date2 == kej) {*/
                        jmlhari = 2;
                        /*}
                        else {
                        if (date2 > kej) {
                        var min = calcBusinessDays(kej, date2) + 1;
                        }
                        else {
                        var min = calcBusinessDays(date2, kej);
                        }

                        if (min > 1) {
                        alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Pernikahan !");
                        jmlhari = 0;
                        $("#fromleave").val("");
                        $('#toleave').val("");
                        $('#hari').val("");
                        }
                        else {
                        jmlhari = 2;
                        }
                        }*/
                    }
                    else if (typeDi == "Wali Nikah") {
                        if (date2 == kej) {
                            jmlhari = 1;
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Pernikahan !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                jmlhari = 1;
                            }
                        }
                    }
                    else if (typeDi == "Kebakaran/Kebanjiran") {
                        if (date2 == kej) {
                            jmlhari = 2;
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Kejadian !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                jmlhari = 2;
                            }
                        }
                    }
                    else {
                        if (date2 == kej) {
                            jmlhari = 1;
                        }
                        else {
                            if (date2 > kej) {
                                var min = calcBusinessDays(kej, date2) + 1;
                            }
                            else {
                                var min = calcBusinessDays(date2, kej);
                            }

                            if (min > 1) {
                                alert("Tanggal Izin Sekurang-kurangnya 1 Hari Sebelum/Sesudah Tgl Kejadian !");
                                jmlhari = 0;
                                $("#fromleave").val("");
                                $('#toleave').val("");
                                $('#hari').val("");
                            }
                            else {
                                jmlhari = 1;
                            }
                        }
                    }
                    var tt = $("#fromleave").val();
                    var arr = tt.split("/");
                    var date = new Date(arr[2] + "-" + arr[1] + "-" + arr[0]);
                    var newdate = "";
                    var count = 0;
                    if (jmlhari == 1) {
                        while (count <= (jmlhari - 1)) {
                            newdate = new Date(date.setDate(date.getDate() + count));
                            if (newdate.getDay() != 0 && newdate.getDay() != 6) {
                                count++;
                            }
                        }
                    }
                    else {
                        while (count < (jmlhari - 1)) {
                            newdate = new Date(date.setDate(date.getDate() + 1));
                            if (newdate.getDay() != 0 && newdate.getDay() != 6) {
                                count++;
                            }
                        }
                    }

                    var d = ((newdate.getDate()) < 10 ? '0' : '') + (newdate.getDate());
                    var m = ((newdate.getMonth() + 1) < 10 ? '0' : '') + (newdate.getMonth() + 1);
                    var y = newdate.getFullYear();

                    var someFormattedDate = d + '/' + m + '/' + y;
                    $("#toleave").val(someFormattedDate);

                    $("#hari").val(jmlhari);

                }
                else if (typecuti == "CT") {

                    if (document.getElementById('Normal').checked) {
                        var fromdate = $("#fromleave").val();
                        var todate = $("#toleave").val();
                        var arrr = fromdate.split("/");
                        var tode = new Date();
                        var from = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
                        var arrrr = todate.split("/");
                        var to = new Date(arrrr[2] + "-" + arrrr[1] + "-" + arrrr[0]);
                        //var datediff = Math.abs(from.getTime() - tode.getTime());
                        var tot = calcBusinessDays(tode, from) + 1;
                        //var tot = parseInt(datediff / (24 * 60 * 60 * 1000), 10) + 1;
                        if (tot < 7) {
                            if (from < tode) {
                                $("#fromleave").val("")
                                alert("Pengajuan Cuti Kurang Dari 7 Hari, Silahkan Pilih Sifat Cuti Mendadak !");
                            }
                            else {
                                alert("Pengajuan Cuti Kurang Dari 7 Hari, Silahkan Pilih Sifat Cuti Mendadak !");
                            }
                        }
                        if (todate != "") {
                            var minDate = calcAllDays(date2, date1) + 1;
                            $("#hari").val(minDate);
                        }

                    }
                }
                else if (typecuti == "CH") {
                    var fromdate = $("#fromleave").val();
                    var arrr = fromdate.split("/");
                    var tode = $("#toleave").val();
                    var arrr1 = tode.split("/");
                    var from = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
                    var to = new Date(arrr1[2] + "-" + arrr1[1] + "-" + arrr1[0]);
                    var tode = new Date();
                    //var datediff = Math.abs(from.getTime() - to.getTime());
                    var tot = calcBusinessDays(from, to) + 1;
                    //var tot = parseInt(datediff / (24 * 60 * 60 * 1000), 10) + 1;
                    if (from > tode) {
                        $("#fromleave").val("");
                        $('#toleave').val("");
                        $('#hari').val("");
                        alert("Tanggal Cuti Haid Tidak Boleh Melewati Tanggal Sekarang !");
                    }
                    else {
                        if (tot > 2) {
                            alert("Maksimal Cuti Haid 2 Hari");
                            $('#toleave').val("");
                            $("#hari").val("0");
                        }
                    }

                }
                else if (typecuti == "CM") {
                    var fromdate = $("#fromleave").val();
                    var arrr = fromdate.split("/");
                    var tode = $("#toleave").val();
                    var arrr1 = tode.split("/");
                    var hpl = $("#hpl").val();
                    var arrr2 = hpl.split("/");
                    var from = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
                    var to = new Date(arrr1[2] + "-" + arrr1[1] + "-" + arrr1[0]);
                    var hpll = new Date(arrr2[2] + "-" + arrr2[1] + "-" + arrr2[0]);
                    var tode = new Date();
                    //var datediff = Math.abs(from.getTime() - to.getTime());
                    var tot = calcBusinessDays(from, to) + 1;
                    //var tot = parseInt(datediff / (24 * 60 * 60 * 1000), 10) + 1;
                    var selisih = calcBusinessDays(from, hpll);
                    var arr = fromdate.split("/");
                    var date = new Date(arr[2] + "-" + arr[1] + "-" + arr[0]);
                    var newdate = "";
                    var count = 0;
                    jmlhari = 90;
                    if (selisih < 15) {
                        alert("Pengajuan cuti melahirkan Sekurang-kurangnya 15 hari dari tgl perakiraan lahir")
                    }
                    if (jmlhari == 1) {
                        while (count <= (jmlhari - 1)) {
                            newdate = new Date(date.setDate(date.getDate() + count));
                            //if (newdate.getDay() != 0 && newdate.getDay() != 6) {
                            count++;
                            //}
                        }
                    }
                    else {
                        while (count < (jmlhari - 1)) {
                            newdate = new Date(date.setDate(date.getDate() + 1));
                            //if (newdate.getDay() != 0 && newdate.getDay() != 6) {
                            count++;
                            //}
                        }
                    }
                    var d = ((newdate.getDate()) < 10 ? '0' : '') + (newdate.getDate());
                    var m = ((newdate.getMonth() + 1) < 10 ? '0' : '') + (newdate.getMonth() + 1);
                    var y = newdate.getFullYear();

                    var someFormattedDate = d + '/' + m + '/' + y;
                    $("#toleave").val(someFormattedDate);

                    $("#hari").val(jmlhari);

                }
            }

        });
        $('#toleave').datepicker({ dateFormat: 'mm/dd/yy', daysOfWeekDisabled: [0, 6], autoclose: true }).on('changeDate', function (ev) {
            var typecuti = $("#SelectedItem").val();
            var sisacuti = $("#sisacuti").val();
            var selectedDate = $("#toleave").val();
            var arr = selectedDate.split("/");
            var date1 = new Date(arr[2] + "-" + arr[1] + "-" + arr[0]);

            var fromdate = $("#fromleave").val();
            var arrr = fromdate.split("/");
            var date2 = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
            if (date1 < date2) {
                if (typecuti != "DI" || typecuti != "CM") {
                    //$("#fromleave").val("");
                    $('#toleave').val("");
                    $('#hari').val("");
                    alert("Tanggal Akhir Lebih Kecil Dari Tanggal Mulai");
                }
                else {
                    $('#toleave').val("");
                    $('#hari').val("");
                }
            }
            else {
                //var datediff = Math.abs(date1.getTime() - date2.getTime());
                var minDate = calcAllDays(date2, date1) + 1;
                //var minDate = parseInt(datediff / (24 * 60 * 60 * 1000), 10) + 1;

                if (minDate > sisacuti) {
                    if (typecuti != "CI" && typecuti != "CM" && typecuti != "CH") {
                        //$("#fromleave").val("");
                        $('#toleave').val("");
                        $('#hari').val("");
                        alert("Sisa cuti anda " + sisacuti + ", Pengajuan cuti melebihi sisa cuti anda !");
                    }

                    else {
                        $("#hari").val(minDate);
                    }
                }
                else {
                    $("#hari").val(minDate);
                }
                if (typecuti == "CH") {

                    var fromdate = $("#fromleave").val();
                    var arrr = fromdate.split("/");
                    var tode = $("#toleave").val();
                    var arrr1 = tode.split("/");
                    var from = new Date(arrr[2] + "-" + arrr[1] + "-" + arrr[0]);
                    var to = new Date(arrr1[2] + "-" + arrr1[1] + "-" + arrr1[0]);
                    var datediff = Math.abs(from.getTime() - to.getTime());
                    //var tot = parseInt(datediff / (24 * 60 * 60 * 1000), 10) + 1;
                    var tot = calcBusinessDays(from, to) + 1;
                    if (tot > 2) {
                        alert("Maksimal Cuti Haid 2 Hari");
                        $('#toleave').val("");
                        $("#hari").val("0");
                    }
                    else {
                        $("#hari").val(tot);
                    }
                }
                else if (typecuti == "CI") {
                    if (minDate > 40) {
                        $('#toleave').val("");
                        $('#hari').val("");
                        alert("Cuti ibadah anda melewati 40 Hari !");
                    }
                }

            }

        });

    });
    function SetTextBoxValue(data, data1, data2, data3, data4,data5) {
        
        $('#empid').val(data);
        $('#empnm').val(data1);
        $('#departement').val(data2);
        $('#gender').val(data3);
        $('#statuta').val(data4);
        $('#sisacuti').val(data5);
        var typecuti = $("#SelectedItem").val();
        var det = $("#detail").val();
        var statuta = $('#statuta').val();
        var gen = $('#gender').val();
        document.getElementById("SelectedItem")[0].selected = "selected";

        if (gen == "Male") {
            document.getElementById("SelectedItem")[1].disabled = "true";
            document.getElementById("SelectedItem")[1].style.color = "#FF0000";
            document.getElementById("SelectedItem")[3].disabled = "true";
            document.getElementById("SelectedItem")[3].style.color = "#FF0000";
        }
        else {
            document.getElementById("SelectedItem")[1].removeAttribute("disabled");
            document.getElementById("SelectedItem")[1].style.color = "#000000";
            document.getElementById("SelectedItem")[3].removeAttribute("disabled");
            document.getElementById("SelectedItem")[3].style.color = "#000000";
        }
       
        if (typecuti == "DI") {
            if (det == "Pekerja") {
                if (statuta == "MENIKAH") {
                    document.getElementById("reason").style.display = "none";
                    document.getElementById("Ctahunan").style.display = "none";
                    document.getElementById("Chaid").style.display = "none";
                    document.getElementById("DivSifatCuti").style.display = "none";
                    document.getElementById("detail").selectedIndex = "0";
                    alert("You Have Used The Merried Permission !");
                }
            }
        }
    };
    function timeDiff(time1, time2) {
        var t1 = new Date();
        var parts = time1.split(":");
        t1.setHours(parts[0], parts[1], 0);
        var t2 = new Date();
        partss = time2.split(":");
        t2.setHours(partss[0], partss[1], 0);

        return parseInt(Math.abs(t1.getTime() - t2.getTime()) / 1000);
    }
    function calcBusinessDays(start, end) {   
        var start = new Date(start);
        var end = new Date(end);

        var totalBusinessDays = 0;
        start.setHours(0, 0, 0, 0);
        end.setHours(0, 0, 0, 0);

        var current = new Date(start);
        current.setDate(current.getDate() + 1);
        var day;
        while (current <= end) {
            day = current.getDay();
            if (day >= 1 && day <= 5) {
                ++totalBusinessDays;
            }
            current.setDate(current.getDate() + 1);
        }
        return totalBusinessDays;
    };
    function calcAllDays(start, end) {
        var start = new Date(start);
        var end = new Date(end);

        var totalAllDays = 0;
        start.setHours(0, 0, 0, 0);
        end.setHours(0, 0, 0, 0);

        var current = new Date(start);
        current.setDate(current.getDate() + 1);
        var day;
        while (current <= end) {
            day = current.getDay();
            if (day >= 0 && day <= 6) {
                ++totalAllDays;
            }
            current.setDate(current.getDate() + 1);
        }
        return totalAllDays;
    };
    function getEndDate(start,total) {
        var startDate = start;
        startDate = new Date(startDate.replace(/-/g, "/"));
        var endDate = "", noOfDaysToAdd = 13, count = 0;
        while (count < noOfDaysToAdd) {
            endDate = new Date(startDate.setDate(startDate.getDate() + 1));
            if (endDate.getDay() != 0 && endDate.getDay() != 6) {
                count++;
            }
        }
        return endDate;
    };
    function FillRestLeave() {
        $.getJSON("http://kmiapp/kmi-intranetdev/LeaveHistory/CheckRestLeave/" + $('#empid').val(), function (data) {
            var items = "";
            var sumrest = 0;
            $.each(data, function (i, leave) {
                items += "<tr><td>" + leave.yearleave + "</td><td>" + leave.leaveright + "</td><td>" + leave.massleave + "</td><td>" + leave.hasgotten + "</td><td style='color:red;'>" + leave.on_going + "</td><td>" + leave.restleave + "</td></tr>";
                sumrest = sumrest + leave.restleave;
            });
            items += "<tr><td colspan='5' align='right'><b>TOTAL SISA CUTI</b></td><td> " + sumrest  + " </td></tr>";
            $('#rData').html(items);
        });
    };
    function FillHaid() {
        $.getJSON("http://kmiapp/kmi-intranetdev/LeaveHistory/HaidHistory/" + $('#empid').val(), function (data) {
            var items = "";
            $.each(data, function (i, haid) {
                items += "<table border=0><tr><td>1.</td><td> Haid Bulan sebelumnya</td><td>:</td><td>TANGGAL <b>" + haid.ONELAST + "</b></td></tr><tr><td>2.</td><td> Haid 2 Bulan sebelumnya</td><td>:</td><td>TANGGAL <b>" + haid.TWOLAST + "</b></td></table>";
            });
            items += "";
            $('#haid').html(items);
        });
    };
    function Fill() {
        $.getJSON("http://kmiapp/kmi-intranetdev/LeaveHistory/Popup1/", function (data) {
            $.each(data, function (i, datas) {
                $('#empid').val(datas.Employeeid);
                $('#empnm').val(datas.Employeenm);
                $('#departement').val(datas.Department);
                $('#gender').val(datas.gender);
                $('#statuta').val(datas.statuta);
                $('#sisacuti').val(datas.sisacuti);
                var gen = $('#gender').val();

                if (gen == "Male") {
                    document.getElementById("SelectedItem")[1].disabled = "true";
                    document.getElementById("SelectedItem")[1].style.color = "#FF0000";
                    document.getElementById("SelectedItem")[3].disabled = "true";
                    document.getElementById("SelectedItem")[3].style.color = "#FF0000";
                }
                else {
                    document.getElementById("SelectedItem")[1].removeAttribute("disabled");
                    document.getElementById("SelectedItem")[1].style.color = "#000000";
                    document.getElementById("SelectedItem")[3].removeAttribute("disabled");
                    document.getElementById("SelectedItem")[3].style.color = "#000000";
                }
            });
        });
    };
</script> 

<div class='popup'>
            <div class='content'>
            <img src="<%=@Url.Content("~/Content/img/close1.gif")%>" alt='Close' class='x' id='Img1' />
            <h4><b>Employee Master</b></h4>
            <hr />
                    <% Html.RenderAction("Popup", "LeaveHistory"); %>
            </div>
            </div>
<script src="<%=@Url.Content("~/Scripts/jquery.validate.min.js")%>" type="text/javascript"></script>
<script src="<%=@Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js")%>" type="text/javascript"></script>

<% using (Html.BeginForm()) {%>
   
    <fieldset style="width:80%;">
    <legend>FORM PERMOHONAN CUTI KARYAWAN / KARYAWATI</legend>
               <div class="col-md-6" style="width:30%;">NIK</div>
               <div class="col-md-6" style="width:70%;">
               <%=@Html.TextBoxFor(m => m.Empid, new { id = "empid", style = "width: 100px;background-color:#F0F0F0;" })%>
               <a href="" class="click"><img src="<%=@Url.Content("~/Content/img/search.png")%>" id="pops" class="search" alt="Search"/></a>
               <%=@Html.ValidationMessageFor(m => m.Empid)%> 
               </div>
               <div class="col-md-6" style="width:30%;">NAMA KARYAWAN</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.TextBox("empnm", null, new { style = "width: 200px;", disabled = "disabled" })%>
                <%=@Html.TextBox("gender", null, new { style = "width: 200px;display:none;" })%>
                <%=@Html.TextBox("statuta", null, new { style = "width: 200px;display:none;" })%>
                <%=@Html.TextBox("sisacuti", null, new { style = "width: 200px;display:none;" })%>
               </div>
               <div class="col-md-6" style="width:30%;">BAGIAN / DEPT</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.TextBox("departement", null, new { style = "width: 250px;", disabled = "disabled" })%>
               </div>
               <div class="col-md-6" style="width:30%;">TIPE CUTI</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.DropDownListFor(m => m.SelectedItem, new SelectList(Model.Items, "Value", "Text"), "---Pilih Tipe Cuti---")%>
                <%=@Html.ValidationMessageFor(m => m.SelectedItem)%> 
               </div>
               <div id="DivSifatCuti" style="display:none;">
               <div class="col-md-6" style="width:30%;">SIFAT CUTI</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.RadioButtonFor(m => m.SifatCuti, "Normal", new { id = "Normal", @checked = true })%> Normal 
                <%=@Html.RadioButtonFor(m => m.SifatCuti, "Mendadak", new { id = "Mendadak" })%> Mendadak 
               </div>
               </div>
			   <div id="reason1" style="display:none;">
               <div class="col-md-6" style="width:30%;">SIFAT</div>
               <div class="col-md-6" style="width:70%;">
                <%= Html.DropDownListFor(m => m.reasonpost, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="Perkawinan", Value = "Perkawinan" },
                    new SelectListItem{ Text="Meninggal Dunia", Value = "Meninggal Dunia" },
                    new SelectListItem{ Text="Istri Pekerja/Pekerja Melahirkan/Keguguran anak", Value = "Melahirkan/Keguguran" },
                    new SelectListItem{ Text="Pengkhitanan/Baptisan Anak Pekerja anak", Value = "Pengkhitanan/Baptisan" },
                    new SelectListItem{ Text="Menjadi Wali dalam Perkawinan", Value = "Wali Nikah" },
                    new SelectListItem{ Text="Rumah Pekerja Kebakaran/Kebanjiran", Value = "Kebakaran/Kebanjiran" }
                 }, "---Pilih Sifat Cuti---", new { Style = "width: 300px;" })%>
                  <%= Html.DropDownListFor(m => m.detail, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="Pekerja Sendiri", Value = "Pekerja" },
                    new SelectListItem{ Text="Anak Pekerja", Value = "Anak" },
                    new SelectListItem{ Text="Saudara Kandung Pekerja", Value = "Saudara" }
                 }, "---Pilih Detail---", new { Style = "width: 250px;display:none;" })%>
                <%= Html.DropDownListFor(m => m.detail1, new List<SelectListItem>
                 {
                    new SelectListItem{ Text="Istri/Suami/Orangtua Kandung", Value = "Istri/Suami/Orangtua" },
                    new SelectListItem{ Text="Mertua/Menantu/Saudara Kandung", Value = "Mertua/Menantu/Saudara Kandung" },
                    new SelectListItem{ Text="Nenek/Kakek", Value = "Nenek/Kakek" }
                 }, "---Pilih Detail---", new { Style = "width: 250px;display:none;" })%>
               </div>
			   </div>
               <div id="HPL" style="display:none;">
               <div class="col-md-6" style="width:30%;">PERAKIRAAN TANGGAL LAHIR</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextBoxFor(m => m.hpl, new { Style = "width:100px;" })%>
                 <%=@Html.ValidationMessageFor(m => m.hpl)%> 
               </div>
			   </div>
               <div id="TGLKEJ" style="display:none;">
               <div class="col-md-6" style="width:30%;">TANGGAL KEJADIAN</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextBoxFor(m => m.tglkej, new { Style = "width:100px;" })%>
                 Jam <%=@Html.TextBoxFor(m => m.times, new { style = "width: 50px;" })%> 
                 <%=@Html.ValidationMessageFor(m => m.tglkej)%> 
               </div>
			   </div>
               <div class="col-md-6" style="width:30%;">DARI - SAMPAI TANGGAL</div>
               <div class="col-md-6" style="width:70%;">
                <%=@Html.TextBoxFor(m => m.fromleave, new { style = "width: 100px;" })%> 
                <%=@Html.TextBoxFor(m => m.toleave, new { style = "width: 100px;"})%> 
                <%=@Html.TextBoxFor(m => m.hari, new { style = "width: 30px;background:#F0F0F0;", MaxLength = "2", readOnly = true })%> Hari
                <%=@Html.ValidationMessageFor(m => m.fromleave)%>
               </div>
               
               <div class="col-md-6" style="width:30%;">ALAMAT</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextAreaFor(m => m.Alamat, new { Style = "width:250px;" })%>
                 <%=@Html.ValidationMessageFor(m => m.Alamat)%> 
               </div>
			   <div id="reason" style="display:none;">
               <div class="col-md-6" style="width:30%;">ALASAN</div>
               <div class="col-md-6" style="width:70%;">
                 <%=@Html.TextAreaFor(m => m.reasonpost1, new { Style = "width:250px;" })%>
                 <%=@Html.ValidationMessageFor(m => m.reasonpost1)%> 
               </div>
			   </div>
               <div id="Message" style="display:none;color:red;">
               <br />
               <div class="col-md-12">
                Note : Lampirkan bukti dan diserahkan ke HRD(*)
			   </div>
               </div>
            <div class="col-md-12">
                <br />
                <input type="submit" value="Create" class="btn btn-instagram"/> 
                <%=Html.ActionLink("Back to List", "Index", null, new { @class = "btn btn-instagram" })%>
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
                    <th>HAK CUTI</th>
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
</asp:Content>


