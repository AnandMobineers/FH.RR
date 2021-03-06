﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CruiseOpenDatesMaster.aspx.cs" Inherits="Cruise_Masters_CruiseOpenDatesMaster" %>

<%@ Register TagName="PageHeaderControl" TagPrefix="phc" Src="~/userControl/pageheader.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../../css/pageheader.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../../style.css" />
    <link href="../../css/roleRights.css" rel="stylesheet" type="text/css" />
    <title>Open Dates Master</title>

    <script type="text/javascript">
        function dateSelectionChanged(sender, args) {
            selectedDate = sender.get_selectedDate();
            var hdnfNoOfRooms = document.getElementById("hdnfNoOfRooms").value;
            convertdate(selectedDate, hdnfNoOfRooms)
        }

        function convertdate(selecteddate, nofnights) {

            var today = new Date();
            var seld = new Date(selecteddate);
            var a = seld.getDate();
            var b=nofnights;
            var dd = parseInt(a)+parseInt(b);
            var result = new Date();
            result.setDate(result.getDate(selecteddate) + nofnights);
            var mm = selecteddate.getMonth() + 1;
            var yyyy = selecteddate.getFullYear();
            var today = mm + '/' +dd+ '/' + yyyy;
            document.getElementById("txtDeBordingDate").value = result.getDates;
        }

       

</script>
    <style>
        .controlsCss
        {
            Width: 161px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <phc:PageHeaderControl ID="pageheader1" runat="server" PageTitle="Cruise Open Departure Dates" />
        <div>
            <asp:ScriptManager ID="scmgrLocation" runat="server">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="updatepanel1" runat="server">
                <ContentTemplate>
                    <div id="gridsection" class="gridsection">
                        <asp:GridView ID="GridOpenDates" ForeColor="#333333" AutoGenerateColumns="false" BorderStyle="Ridge" GridLines="Both" runat="server" Width="950px">
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <AlternatingRowStyle BackColor="White" />
                            <RowStyle BackColor="#EFF3FB" HorizontalAlign="Left" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Sn.">
                                    <ItemTemplate><%#Container.DataItemIndex+1 %> </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="AccomName">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfnOpenDateId" runat="server" Value='<%#Eval("Id") %>' />
                                        <asp:HiddenField ID="hdnfAccomId" runat="server" Value='<%#Eval("AccomId") %>' />
                                        <asp:Label runat="server" ID="lbAccomName" Text='<%#Eval("AccomName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="River">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnfRiverId" runat="server" Value='<%#Eval("RiverId") %>' />
                                        <asp:Label runat="server" ID="lbRiverName" Text='<%#Eval("RiverName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Country">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnfCountryName" runat="server" Value='<%#Eval("CountryId") %>' />
                                        <asp:Label runat="server" ID="lbCountryName" Text='<%#Eval("CountryName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Boarding Loc.">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnfBoardingFrom" runat="server" Value='<%#Eval("BoardingFrom") %>' />
                                        <asp:Label runat="server" ID="lbBoardingFrom" Text='<%#Eval("BoardingFrom1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deboarding Loc.">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnfBoardingTo" runat="server" Value='<%#Eval("BoardingTo") %>' />
                                        <asp:Label runat="server" ID="lbBoardingTo" Text='<%#Eval("BoardingTo1") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Boarding Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbBoardingDate" Text='<%#Eval("BoardingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Deboarding Date">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbDeboardingDate" Text='<%#Eval("DeboardingDate") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cruise Direction">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbCruiseDirection" Text='<%#Eval("CruiseDirection") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <table>
                        <tr>
                            <td>Select Accom.</td>
                            <td>
                                <asp:DropDownList ID="ddlAccom" CssClass="controlsCss" runat="server"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqfddlAccom" runat="server" ErrorMessage="*" ControlToValidate="ddlAccom" InitialValue="-Select-" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>

                        </tr>
                        <tr>
                            <td>Country</td>
                            <td>
                                <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCountry_SelectedIndexChanged" CssClass="controlsCss">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqfddlCountry" runat="server" ControlToValidate="ddlCountry" ErrorMessage="*" ForeColor="Red" InitialValue="-Select-"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>River</td>
                            <td>
                                <asp:DropDownList ID="ddlRiver" CssClass="controlsCss" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqfddlRiver" runat="server" ControlToValidate="ddlRiver" ErrorMessage="*" ForeColor="Red" InitialValue="-Select-"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Select package</td>
                            <td>
                                <asp:DropDownList ID="ddlpackage" CssClass="controlsCss" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlpackage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqfddlpackage" runat="server" ErrorMessage="*" ControlToValidate="ddlpackage" InitialValue="-Select-" ForeColor="Red"></asp:RequiredFieldValidator>
                            
                            <asp:HiddenField  runat="server" ID="hdnfNoOfRooms"/>
                            
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>CheckIn Date</td>
                            <td>
                                <asp:TextBox ID="txtBoardingDate" CssClass="controlsCss" runat="server" Width="83px" AutoPostBack="true"  OnTextChanged="txtBoardingDate_TextChanged"></asp:TextBox>
                                <asp:CalendarExtender ID="txtBoardingDate_CalendarExtender"  runat="server" TargetControlID="txtBoardingDate">
                                </asp:CalendarExtender>
                                <br />

                                <asp:RegularExpressionValidator ID="regtxtBoardingDate"
                                    ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$"
                                    ControlToValidate="txtBoardingDate" ErrorMessage="Invalid Format. Use MM/DD/YYYY" runat="server"
                                    CssClass="colorred">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqftxtBoardingDate" runat="server" ErrorMessage="*" ControlToValidate="txtBoardingDate" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>CheckOut Date</td>
                            <td>
                                <asp:TextBox ID="txtDeBordingDate" CssClass="controlsCss" runat="server" Width="83px" Height="19px" Enabled="False"></asp:TextBox>
                                <asp:CalendarExtender ID="txtDeBordingDate_CalendarExtender" runat="server" TargetControlID="txtDeBordingDate">
                                </asp:CalendarExtender>
                                <br />
                                <asp:RegularExpressionValidator ID="regtxtDeBordingDate"
                                    ValidationExpression="^([0-9]|0[1-9]|1[012])\/([0-9]|0[1-9]|[12][0-9]|3[01])\/(19|20)\d\d$"
                                    ControlToValidate="txtDeBordingDate" ErrorMessage="Invalid Format. Use MM/DD/YYYY" runat="server"
                                    CssClass="colorred">
                                </asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqftxtDeBordingDate" runat="server" ErrorMessage="*" ControlToValidate="txtDeBordingDate" ForeColor="Red"></asp:RequiredFieldValidator>
                                <br />
                        <%-- <asp:CompareValidator ID="cmprtxtDeBordingDate" runat="server" ControlToCompare="txtBoardingDate" ControlToValidate="txtDeBordingDate" ForeColor="Red" ErrorMessage="*Invalid Date Range(End Date should be greater)" Operator="GreaterThan" Type="Date" />--%>

                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Button ID="btnAdd" runat="server" CssClass="appbutton" Height="24px" OnClick="btnAdd_Click" Text="Add" Width="65px" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnAdd0" runat="server" CssClass="appbutton" CausesValidation="false" Height="24px" Text="Reload" Width="65px" OnClick="btnAdd0_Click" />
                            </td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>

                </ContentTemplate>

            </asp:UpdatePanel>

        </div>

    </form>
</body>
</html>
