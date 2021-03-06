<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccomMaster.aspx.cs" Inherits="MasterUI_AccomMaster" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register TagName="PageHeaderControl" TagPrefix="phc" Src="~/userControl/pageheader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" media="all" href="../css/pageheader.css" />
    <link rel="stylesheet" type="text/css" media="all" href="../style.css" />
    <title>Accomodation Master</title>

    <script language="javascript" type="text/javascript" src="../js/calendar/calendar.js"></script>

    <script language="javascript" type="text/javascript" src="../js/calendar/calendar-en.js"></script>

    <script language="javascript" type="text/javascript" src="../js/calendar/calendar-setup.js"></script>

    <script language="javascript" type="text/javascript" src="../js/master/accommaster.js"></script>

    <script language="javascript" type="text/javascript" src="../js/global.js"></script>

    <link rel="stylesheet" type="text/css" media="all" href="../css/calendar-blue2.css"
        title="win2k-cold-1" />
</head>
<body>
    <form id="form1" runat="server">
    <phc:PageHeaderControl ID="pageheader1" runat="server" PageTitle="Accomodation Master" />
    <div>
        <asp:ScriptManager ID="scmgrAccomMaster" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="pnlAccomMaster" runat="server">
            <ContentTemplate>
                <table id="filtersection" class="filtersection">
                    <tr>
                        <td class="labelcell">
                            Accomodation Type:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="select dropdown" ID="ddlAccomTypeId" runat="server" Width="215px">
                            </asp:DropDownList>
                        </td>
                        <td class="labelcell">
                            Region:
                        </td>
                        <td>
                            <asp:DropDownList CssClass="select dropdown" ID="ddlRegion" runat="server" Width="159px"
                                TabIndex="1">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 68px">
                            <asp:Button CssClass="appbutton" ID="btnSearch" runat="server" OnClick="btnSearch_Click"
                                Text="Show" Width="65px" TabIndex="2" />
                        </td>
                    </tr>
                </table>
                <div id="gridsection" class="gridsection">
                    <asp:DataGrid ID="dgAccomodations" runat="server" AutoGenerateColumns="False" BorderStyle="Ridge"
                        CellPadding="4" DataKeyField="AccomodationId" ForeColor="#333333" GridLines="None"
                        OnSelectedIndexChanged="dgAccomodations_SelectedIndexChanged" Width="650px" TabIndex="3">
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <EditItemStyle BackColor="#2461BF" />
                        <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <AlternatingItemStyle BackColor="White" />
                        <ItemStyle BackColor="#EFF3FB" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <Columns>
                            <asp:BoundColumn DataField="AccomodationId" HeaderText="Accomodation Id" Visible="False">
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="AccomodationName" HeaderText="Accomodation"></asp:BoundColumn>
                            <asp:BoundColumn DataField="AccomodationTypeId" HeaderText="Accomodation Type Id"
                                Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="AccomodationType" HeaderText="Accomodation Type"></asp:BoundColumn>
                            <asp:BoundColumn DataField="RegionId" HeaderText="Region Id" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Region" HeaderText="Region"></asp:BoundColumn>
                            <asp:BoundColumn DataField="AccomInitial" HeaderText="Initial"></asp:BoundColumn>
                            <asp:ButtonColumn CommandName="Select" HeaderText="[...]" Text="Edit"></asp:ButtonColumn>
                        </Columns>
                    </asp:DataGrid>
                </div>
                <table id="inputsection" class="inputsection">
                    <tr>
                        <td class="labelcell">
                            Accomodation:
                        </td>
                        <td class="inputcell">
                            <asp:TextBox CssClass="input" ID="txtAccomName" runat="server" MaxLength="100" Width="260px"
                                TabIndex="4"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="labelcell">
                            Accomodation Initial:
                        </td>
                        <td class="inputcell">
                            <asp:TextBox CssClass="input" ID="txtAccomInitial" runat="server" MaxLength="5" TabIndex="5"
                                Width="39px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table id="buttonsection" class="buttonsection">
                    <tr>
                        <td>
                            <asp:Button CssClass="appbutton" ID="btnEdit" runat="server" Height="24px" OnClick="btnEdit_Click"
                                Text="Update" Width="65px" TabIndex="10" />
                        </td>
                        <td>
                            <asp:Button CssClass="appbutton" ID="btnDelete" runat="server" Height="24px" OnClick="btnDelete_Click"
                                Text="Delete" Width="65px" TabIndex="11" />
                        </td>
                        <td>
                            <asp:Button CssClass="appbutton" ID="btnCancel" runat="server" OnClick="btnCancel_Click"
                                Text="Cancel" Width="65px" TabIndex="12" />
                        </td>
                    </tr>
                </table>
                <table id="statussection" class="statussection">
                    <tr>
                        <td>
                            <asp:Label ID="lblStatus" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <table id="hiddensection" class="hiddensection">
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfId" runat="server" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Button CssClass="appbutton" ID="btnAddNew" runat="server" Height="24px" OnClick="btnAddNew_Click"
                                Text="New" Visible="False" Width="65px" />
                        </td>
                        <td>
                            <asp:Button CssClass="appbutton" ID="btnSave" runat="server" Height="24px" OnClick="btnSave_Click"
                                Text="Save" Visible="False" Width="65px" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
