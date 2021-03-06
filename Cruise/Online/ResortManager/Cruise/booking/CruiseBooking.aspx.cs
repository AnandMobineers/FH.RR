﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

public partial class Cruise_booking_CruiseBooking : System.Web.UI.Page
{
    BALBooking blsr = new BALBooking();
    DALBooking dlsr = new DALBooking();
    public string packageid = string.Empty;
    DataView dv;
    DataTable dt;
    int totpax = 0;
    int roomCatId = 0;
    int irpax = 0;
    double Totamt = 0;
    public int BookedId = 0;
    DataTable dtGetReturnedData;
    BALPackageRateCard blRate = new BALPackageRateCard();
    DALPackageRateCard dlRate = new DALPackageRateCard();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindCruiseRoomRates();
        for (int k = 0; k < GridRoomPaxDetail.Rows.Count; k++)
        {
            try
            {
              
                ImageButton imgbtn = (ImageButton)GridRoomPaxDetail.Rows[k].FindControl("imgbtnDelete");
                ScriptManager.GetCurrent(this).RegisterPostBackControl(imgbtn);
            
            }
            catch
            {
            }
        }

        if (!IsPostBack)
        {


            Session.Add("PackageId", Request.QueryString["PackId"].ToString());
            ddlConvert.SelectedIndex = 1;
            ddlpax1rm.SelectedIndex = 2;
            ButtonsDiv.Style.Add("display", "none");
            div1.Style.Add("display", "none");
            if (Session["BookedRooms"] != null)
            {
                ViewState["VsRoomDetails"] = Session["BookedRooms"];
               
                bindRoomRates();

                GridRoomPaxDetail.DataSource = (DataTable)Session["BookedRooms"];
                GridRoomPaxDetail.DataBind();

                if (GridRoomPaxDetail.Rows.Count > 0)
                {
                    ButtonsDiv.Style.Remove("display");
                }
                else
                {
                    ButtonsDiv.Style.Add("display", "none");
                }

                calculateTotal();

            }


         
           
          
   
            if (Session["UserCode"] != null||Session["CustomerCode"]!=null)
            {
                lnkLogout.Visible = true;
            }
            else
            {
                lnkLogout.Visible = false;
            }

        }
ScriptManager.RegisterStartupScript(this, this.GetType(), "blockArea", "blockArea();", true);
    }

  


    private void setImageMap()
    {
        try
        {
            DataTable dtRoomsdata;

            dtRoomsdata = bindroomddl();
            PolygonHotSpot hotSpot;
           
            foreach (DataRow dr in dtRoomsdata.Rows)
            {

                hotSpot = new PolygonHotSpot();
                if (dr["BookedStatus"].ToString() == "Available")
                {
                    hotSpot.HotSpotMode = HotSpotMode.PostBack;
                }
                else
                {
                    hotSpot.HotSpotMode = HotSpotMode.Inactive;
                   
                   
                }
      
                hotSpot.AlternateText = dr["BookedStatus"].ToString();
                hotSpot.Coordinates = dr["Coordinates"].ToString();
              
                hotSpot.PostBackValue = dr["RoomNo"].ToString();
                ImageMap1.HotSpots.Add(hotSpot);
            }
        }
        catch
        {
        }
    }

  
    #region UDF
    public void bindRoomRates()
    {
        try
        {

            if (Session["UserCode"] != null)
            {

                blsr.action = "RoomRates";
                blsr.AgentId = Convert.ToInt32(Session["UserCode"].ToString());
            }
            else
            {

                blsr.action = "RoomRatesCustAgent";
            }
            blsr._dtStartDate = Convert.ToDateTime(Session["checkin"]);

            blsr.PackageId = Session["PackId"].ToString();
           
            blsr.totpax = Int32.TryParse(Session["totpax"].ToString(), out totpax) ? totpax : 0;

            dt = new DataTable();


            dt = dlsr.GetRoomCategoryWiseRates(blsr);


            DataView dv = dt.DefaultView;
            dv.Sort = "PPRoomRate asc";
            DataTable sortedDT = dv.ToTable();

            if (sortedDT != null)
            {
                if (sortedDT.Rows.Count > 0)
                {
                    Session["Rrate"] = sortedDT;
                    gdvRoomCategories.DataSource = sortedDT;
                    gdvRoomCategories.DataBind();
              
                    div1.Style.Remove("display");


                    roundoff(); 
                }
                else
                {
                    gdvRoomCategories.DataSource = null;
                    gdvRoomCategories.DataBind();
                    div1.Style.Add("display", "none");
                 
                }
            }
            else
            {
                gdvRoomCategories.DataSource = null;
                gdvRoomCategories.DataBind();
                div1.Style.Add("display", "none");

            }

        }
        catch
        {
            div1.Style.Add("display", "none");
        }
    }

    public void roundoff()
    {
        foreach(GridViewRow row in gdvRoomCategories.Rows)
        {

           
            for (int k = 0; k < row.Cells.Count; k++)
            {
                try
                {
                    Label lblptws = (Label)row.FindControl("lbltws");
                    Label lblss = (Label)row.FindControl("lblsc");

                    string[] arrtws = lblptws.Text.ToString().Split(' ');
                    string[] arrss = lblss.Text.ToString().Split(' ');

                    lblptws.Text = arrtws[0].ToString() + " " + Convert.ToDecimal(arrtws[1]).ToString("#.##");

                    lblss.Text = arrss[0].ToString() + " " + Convert.ToDecimal(arrss[1]).ToString("#.##");
                    row.Cells[k].HorizontalAlign = HorizontalAlign.Center;
                }
                catch
                {

                }
            }
        }
    }


    private void GetRoomLimit()
    {
        try
        {

        }
        catch (Exception sqe)
        {

        }
    }
    public void hidecolumn(GridView grv, int num)
    {
        try
        {
            grv.HeaderRow.Cells[num].Visible = false;
            foreach (GridViewRow grow in grv.Rows)
            {
                grow.Cells[num].Visible = false;

            }

        }
        catch
        {

        }
    }
    public void initializetable()
    {

        try
        {
            dt = new DataTable();
            dt.Columns.Add("RoomCategory");
            dt.Columns.Add("NoofRooms");
            dt.Columns.Add("Total");
            dt.Columns.Add("roomcategoryid");
            dt.Columns.Add("Pax");
        }
        catch
        {
        }
    }
    public void addrows(DataView view, int roomcateId)
    {
        try
        {

            string[] arr = view.ToTable().Rows[0][2].ToString().Split(' ');
            string[] arr1 = view.ToTable().Rows[0][1].ToString().Split(' ');

            int count = 0;
            dt = new DataTable();
            dt = ViewState["dt"] as DataTable;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    if (ddlpax1rm.SelectedItem.Text == "2")
                        dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Sharing";
                    else
                        dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Single Use";
                    dr["NoofRooms"] = 1;
                    if (Convert.ToInt32(ddlpax1rm.SelectedValue) < 2)
                        dr["Total"] = (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr[1]));
                    else
                        dr["Total"] = (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr1[1]));
                    dr["roomcategoryid"] = view.ToTable().Rows[0][3].ToString();

                    foreach (DataRow dr1 in dt.Rows)
                    {
                        if (dr1["roomcategoryid"].ToString() == (view.ToTable().Rows[0][3].ToString()))
                        {
                            if (Convert.ToInt32(ddlpax1rm.SelectedValue) < 2)
                            {
                                if (Convert.ToInt32(dr1["Pax"]) == Convert.ToInt32(ddlpax1rm.SelectedValue))
                                {
                                    dr1["Total"] = Convert.ToDouble(dr1["Total"]) + (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr[1]));
                                    dr1["NoofRooms"] = Convert.ToInt32(dr1["NoofRooms"]) + 1;
                                    dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Single Use";
                                    count++;
                                }
                            }
                            else
                            {
                                if (Convert.ToInt32(dr1["Pax"]) == Convert.ToInt32(ddlpax1rm.SelectedValue))
                                {
                                    dr1["Total"] = Convert.ToDouble(dr1["Total"]) + (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr1[1]));
                                    dr1["NoofRooms"] = Convert.ToInt32(dr1["NoofRooms"]) + 1;
                                    dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Sharing";
                                    count++;
                                }
                            }
                        }
                    }
                    dr["Pax"] = ddlpax1rm.SelectedItem.Text;

                    if (count == 0)
                    {
                        dt.Rows.Add(dr);
                    }

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    hidecolumn(GridView1, 3);
                    ViewState["dt"] = dt;
                    this.RoomNumberWiseDetail(dv, roomcateId);





                }
            }
            else
            {
                initializetable();
                DataRow dr = dt.NewRow();
                if (ddlpax1rm.SelectedItem.Text == "2")
                {

                    dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Sharing";

                }
                else
                {
                    dr["RoomCategory"] = view.ToTable().Rows[0][0].ToString() + " Single Use";
                }
                dr["NoofRooms"] = 1;
                if (Convert.ToInt32(ddlpax1rm.SelectedValue) < 2)
                {

                    dr["Total"] = (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr[1]));
                }
                else
                {
                    

                    dr["Total"] = (Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(arr1[1]));
                }
                dr["roomcategoryid"] = view.ToTable().Rows[0][3].ToString();
                dr["Pax"] = ddlpax1rm.SelectedItem.Text;
                dt.Rows.Add(dr);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                hidecolumn(GridView1, 3);
                ViewState["dt"] = dt;

                this.RoomNumberWiseDetail(dv, roomcateId); // calling Insertable RoomDetail Function 
            }

            calculateTotal();

        }

        catch
        {

        }
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("SearchProperty.aspx");
    }

    public void calculateTotal()
    {
        try
        {
            Totamt = 0;
            DataTable dt1 = new DataTable();
            dt1 = ViewState["VsRoomDetails"] as DataTable;
            if (dt1 != null)
            {
                for (int k = 0; k < dt1.Rows.Count; k++)
                {
                    Totamt = Totamt + Convert.ToDouble(dt1.Rows[k]["Price"]);
                }

                lblTotal.Text = "Total: ";
                lblTotAmt.Text = Totamt.ToString();
                lblTotalCabins.Text = dt1.Rows.Count.ToString();
                TotalCabins.Text = "Cabins Selected";
                GridRoomPaxDetail.FooterRow.Cells[3].Text = "<strong style='font-weight: bolder; color: Black;'>Total :</strong>";
                GridRoomPaxDetail.FooterRow.Cells[5].Text = "<strong style='font-weight: bolder; color: Black;'> INR" + " " + Totamt.ToString()+" </strong>";


            }
            else
            {
                lblTotal.Text = "";
                lblTotAmt.Text = "";
                lblTotalCabins.Text = "";
                TotalCabins.Text = "";
            }
        }

        catch
        {
        }
    }
    public DataTable bindroomddl()
    {
        try
        {
            blsr.action = "GetcruiseRooms";
            blsr.PackageId = Session["PackageId"].ToString();
            if (Request.QueryString["DepartureId"] != null)
            {
                Session["DepartureId"] = Request.QueryString["DepartureId"].ToString();
            }
            blsr.DepartureId = Convert.ToInt32(Session["DepartureId"]);
          
            if (Session["UserCode"] != null)
            {
                blsr._iAgentId = Convert.ToInt32(Session["UserCode"].ToString());
            }
            dt = new DataTable();
            dt = dlsr.GetCruiseRooms(blsr);
            if (dt != null)
            {

                return dt;
            }
            else
            {
                return null;
             
            }
        }
        catch
        {
            return null;
        }
    }
    private int InsertParentTableData()
    {
        try
        {
            blsr.action = "GetDepartureDetails";
            blsr.PackageId = Request.QueryString["PackId"].ToString();
            dtGetReturnedData = dlsr.GetDepartureDetails(blsr);
            blsr._sBookingRef = txtBookingRef.Text.Trim().ToString();
            blsr._dtStartDate = Convert.ToDateTime(dtGetReturnedData.Rows[0]["CheckInDate"]);
            blsr._dtEndDate = Convert.ToDateTime(dtGetReturnedData.Rows[0]["CheckOutDate"]);
            blsr._iAccomTypeId = Convert.ToInt32(dtGetReturnedData.Rows[0]["AccomTypeId"]);
            blsr._iAccomId = Convert.ToInt32(dtGetReturnedData.Rows[0]["AccomId"]);
            blsr._iAgentId = Convert.ToInt32(Session["UserCode"].ToString());
            blsr._iNights = Convert.ToInt32(dtGetReturnedData.Rows[0]["NoOfNights"]);
            DataTable dtRoomBookingDetails = ViewState["VsRoomDetails"] as DataTable;
            blsr._iPersons = Convert.ToInt32(dtRoomBookingDetails.Compute("SUM(Pax)", string.Empty));
            blsr._BookingStatusId = 1;
            blsr._SeriesId = 0;
            blsr._proposedBooking = false;
            blsr._chartered = false;
            Session.Add("tblBookingBAL", blsr);
            int GetQueryResponse = dlsr.AddParentBookingDetail(blsr);
            if (GetQueryResponse > 0)
                return 1;
            else
                return 0;
        }
        catch
        {
            return 0;
        }
    }
    private int InsertChildTableData()
    {
        #region Fetching Departure Details
        blsr.action = "GetDepartureDetails";
        blsr.PackageId = Request.QueryString["PackId"].ToString();
        dtGetReturnedData = dlsr.GetDepartureDetails(blsr);
        blsr._iAccomId = Convert.ToInt32(dtGetReturnedData.Rows[0]["AccomId"]); ;
        #endregion
        blsr.action = "getMaxBookId";
        DataTable dtmaxId = dlsr.GetMaxBookingId(blsr);
        if (dtGetReturnedData != null)
        {
            int MaxBookingId = Convert.ToInt32(dtmaxId.Rows[0].ItemArray[0].ToString());
            BookedId = MaxBookingId;
            blsr._iBookingId = MaxBookingId;
            int LoopInsertStatus = 0;
            try
            {
                for (int LoopCounter = 0; LoopCounter < GridRoomPaxDetail.Rows.Count - 1; LoopCounter++)
                {
                    Label lbRoomNo = (Label)GridRoomPaxDetail.Rows[LoopCounter].Cells[0].FindControl("RoomId");
                    Label bRoomCategoryId = (Label)GridRoomPaxDetail.Rows[LoopCounter].Cells[1].FindControl("RoomCategoryId");
                    Label lbPax = (Label)GridRoomPaxDetail.Rows[LoopCounter].Cells[2].FindControl("Pax");
                    Label lbPrice = (Label)GridRoomPaxDetail.Rows[LoopCounter].Cells[3].FindControl("Price");
                    blsr._dtStartDate = Convert.ToDateTime(dtGetReturnedData.Rows[0]["CheckInDate"]);
                    blsr._dtEndDate = Convert.ToDateTime(dtGetReturnedData.Rows[0]["CheckOutDate"]);
                    blsr._iPaxStaying = Convert.ToInt32(lbPax.Text.Trim().ToString());
                    blsr._bConvertTo_Double_Twin = false;
                    blsr._cRoomStatus = "B";
                    blsr._sRoomNo = lbRoomNo.Text.Trim().ToString();
                    blsr.action = "AddPriceDetailsToo";
                    blsr._Amt = Convert.ToDecimal(lbPrice.Text.ToString());
                    int GetQueryResponse = dlsr.AddRoomBookingDetails(blsr);
                    if (GetQueryResponse > 0)
                        LoopInsertStatus++;
                    else
                    {
                        //do nothing
                    }


                }
                if (LoopInsertStatus == GridRoomPaxDetail.Rows.Count - 1)
                    return 1;
                else
                    return
                        0;

            }
            catch
            {
                return 0;
            }
        }
        else
            return 0;
    }

    #endregion

    #region Control Events
    private void RoomNumberWiseDetail(DataView view, int RoomcateId)
    {
        try
        {

            string[] arrWZ;
            string[] arr = view.ToTable().Rows[0][2].ToString().Split(' ');
            string[] arr1 = view.ToTable().Rows[0][1].ToString().Split(' ');

            string[] arrtx = view.ToTable().Rows[0][5].ToString().Split(' ');
            string[] arr1tx = view.ToTable().Rows[0][4].ToString().Split(' ');

            DataTable dtInsertable = new DataTable();
            dtInsertable.Columns.Add("RoomNumber", typeof(string));
            dtInsertable.Columns.Add("RoomCategoryId", typeof(int));
            dtInsertable.Columns.Add("categoryName", typeof(string));
            dtInsertable.Columns.Add("Pax", typeof(int));
            dtInsertable.Columns.Add("Bed Configuration", typeof(string));
            dtInsertable.Columns.Add("Price", typeof(decimal));
           
            dtInsertable.Columns.Add("Tax", typeof(decimal));
            dtInsertable.Columns.Add("Currency", typeof(string));
            dtInsertable.Columns.Add("Convertable", typeof(string));
            dtInsertable.Columns.Add("CRPrice", typeof(string));
       
            if (ViewState["VsRoomDetails"] == null)
            {
                DataRow dr = dtInsertable.NewRow();

               
                dr["RoomNumber"] = hfRoomId.Value;
                dr["RoomCategoryId"] = RoomcateId;
                dr["Pax"] = Convert.ToInt32(ddlpax1rm.SelectedItem.Text.ToString());
                dr["Currency"] = view.ToTable().Rows[0][8].ToString();

                if ((ddlConvert.SelectedValue == "1" && ddlpax1rm.SelectedItem.Text == "2"))
                {
                    dr["Bed Configuration"] = "Double Bed";
                }
                else if (ddlpax1rm.SelectedItem.Text == "1")
                {
                    dr["Bed Configuration"] = "Single Bed";
                }
                else
                {
                    dr["Bed Configuration"] = view.ToTable().Rows[0][9].ToString()+" Beds";
                }

              
                dr["Convertable"] = ddlConvert.SelectedValue.ToString();
            
                //dr["Price"] = Convert.ToDecimal(Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(view.ToTable().Rows[0][2]));
                if (Convert.ToInt32(ddlpax1rm.SelectedValue) >= 2)
                {
                    string TaxStatus = (view.ToTable().Rows[0][7].ToString());
                    string TaxValue = (view.ToTable().Rows[0][6].ToString());
                    //if (TaxStatus == "Tax Applied")
                    //{
                        arrWZ = view.ToTable().Rows[0][4].ToString().Split(' ');
                        dr["CRPrice"] = arrWZ[0].ToString() + " " + (Convert.ToDouble(arr1tx[1]) * Convert.ToInt32(ddlpax1rm.SelectedValue)).ToString("#.##");
                        dr["Price"] = (Convert.ToDouble(arr1tx[1]) * Convert.ToInt32(ddlpax1rm.SelectedValue)).ToString("#.##");
                        dr["Tax"] = 0;// Convert.ToDecimal(TaxValue.ToString());

                    //}
                    //else if (TaxStatus == "Not Applied")
                    //{
                    //    arrWZ = view.ToTable().Rows[0][2].ToString().Split(' ');
                    //    dr["CRPrice"] = arrWZ[0].ToString()+" " + Convert.ToDouble(arrWZ[1]).ToString("#.##");
                    //    dr["Tax"] = 0;
                    //    dr["Price"] =  Convert.ToDouble(arr[1]);
                    //}
                }
                else
                {
                    string TaxStatus = (view.ToTable().Rows[0][7].ToString());
                    string TaxValue = (view.ToTable().Rows[0][6].ToString());
                    //if (TaxStatus == "Tax Applied")
                    //{
                        arrWZ = view.ToTable().Rows[0][5].ToString().Split(' ');
                        dr["CRPrice"] = arrWZ[0].ToString() +" "+ Convert.ToDouble(arrWZ[1]).ToString("#.##");
                        dr["Price"] =  Convert.ToDouble(arrtx[1]);
                        dr["Tax"] = 0;// Convert.ToDecimal(TaxValue.ToString());

                    //}
                    //else if (TaxStatus == "Not Applied")
                    //{
                    //    arrWZ = view.ToTable().Rows[0][1].ToString().Split(' ');
                    //    dr["CRPrice"] = arrWZ[0].ToString() +" "+ Convert.ToDouble(arrWZ[1]).ToString("#.##");
                    //    dr["Tax"] = 0;
                    //    dr["Price"] =   Convert.ToDouble(arr1[1]);
                    //}
                }
                if (ddlpax1rm.SelectedItem.Text == "2")
                    dr["categoryName"] = view.ToTable().Rows[0][0].ToString() ;
                else
                    dr["categoryName"] = view.ToTable().Rows[0][0].ToString() ;
                dtInsertable.Rows.Add(dr);
                ViewState["VsRoomDetails"] = dtInsertable;

                GridRoomPaxDetail.DataSource = dtInsertable;
                GridRoomPaxDetail.DataBind();
            }
            else
            {
                dtInsertable = ViewState["VsRoomDetails"] as DataTable;
                DataRow dr = dtInsertable.NewRow();
                dr["Convertable"] = ddlConvert.SelectedValue.ToString();
                dr["RoomNumber"] = hfRoomId.Value;
                dr["RoomCategoryId"] = RoomcateId;
                if ((ddlConvert.SelectedValue == "1" && ddlpax1rm.SelectedItem.Text == "2"))
                {
                    dr["Bed Configuration"] = "Double Bed";
                }
                else if (ddlpax1rm.SelectedItem.Text == "1")
                {
                    dr["Bed Configuration"] = "Single Bed";
                }
                else
                {
                     dr["Bed Configuration"]=view.ToTable().Rows[0][9].ToString()+" Beds";
                }
                dr["Currency"] = view.ToTable().Rows[0][8].ToString();
                if (ddlpax1rm.SelectedItem.Text == "2")
                    dr["categoryName"] = view.ToTable().Rows[0][0].ToString();
                else
                    dr["categoryName"] = view.ToTable().Rows[0][0].ToString();
                dr["Pax"] = Convert.ToInt32(ddlpax1rm.SelectedItem.Text.ToString());
                //dr["Price"] = Convert.ToDecimal(Convert.ToInt32(ddlpax1rm.SelectedValue) * Convert.ToDouble(view.ToTable().Rows[0][2]));
                if (Convert.ToInt32(ddlpax1rm.SelectedValue) >= 2)
                {
                    string TaxStatus = (view.ToTable().Rows[0][7].ToString());
                    string TaxValue = (view.ToTable().Rows[0][6].ToString());
                    //if (TaxStatus == "Tax Applied")
                    //{
                        arrWZ = view.ToTable().Rows[0][4].ToString().Split(' ');
                        dr["CRPrice"] = arrWZ[0].ToString() + " " + (Convert.ToDouble(arr1tx[1]) * Convert.ToInt32(ddlpax1rm.SelectedValue)).ToString("#.##");
                        dr["Price"] = (Convert.ToDouble(arr1tx[1]) * Convert.ToInt32(ddlpax1rm.SelectedValue)).ToString("#.##");
                        dr["Tax"] = 0;// Convert.ToDecimal(TaxValue.ToString());

                    //}
                    //else if (TaxStatus == "Not Applied")
                    //{
                    //    arrWZ = view.ToTable().Rows[0][2].ToString().Split(' ');

                    //    dr["CRPrice"] = arrWZ[0].ToString() +" "+ Convert.ToDouble(arrWZ[1]).ToString("#.##");
                    //    dr["Tax"] = 0;
                    //    dr["Price"] =  Convert.ToDouble(arr[1]);
                    //}
                }
                else
                {
                    string TaxStatus = (view.ToTable().Rows[0][7].ToString());
                    string TaxValue = (view.ToTable().Rows[0][6].ToString());
                    //if (TaxStatus == "Tax Applied")
                    //{

                        arrWZ = view.ToTable().Rows[0][5].ToString().Split(' ');
                        dr["CRPrice"] = arrWZ[0].ToString() +" "+ Convert.ToDouble(arrWZ[1]).ToString("#.##");
                        dr["Price"] =  Convert.ToDouble(arrtx[1]);
                        dr["Tax"] = 0;// Convert.ToDecimal(TaxValue.ToString());

                    //}
                    //else if (TaxStatus == "Not Applied")
                    //{
                    //    arrWZ = view.ToTable().Rows[0][1].ToString().Split(' ');

                    //    dr["CRPrice"] = arrWZ[0].ToString() +" "+ Convert.ToDouble(arrWZ[1]).ToString("#.##");
                    //    dr["Tax"] = 0;
                    //    dr["Price"] =  Convert.ToDouble(arr1[1]);
                    //}
                }
                int Counter = 0;
                foreach (DataRow dr1 in dtInsertable.Rows)
                {
                    if (dr1["RoomNumber"].ToString() == hfRoomId.Value)
                    {
                        dr1.Delete();
                        Counter++;
                        break;
                    }
                    else
                    {
                        //do nothing
                    }

                }
                if (Counter > 0)
                {
                    dtInsertable.AcceptChanges();
                    dtInsertable.Rows.Add(dr);
                }
                else
                    dtInsertable.Rows.Add(dr);

                ViewState["VsRoomDetails"] = dtInsertable;
                GridRoomPaxDetail.DataSource = dtInsertable;
                GridRoomPaxDetail.DataBind();

              
            }


            if (GridRoomPaxDetail.Rows.Count > 0)
            {
                ButtonsDiv.Style.Remove("display");
            }
            else
            {
                ButtonsDiv.Style.Add("display", "none");
            }
        }
        catch
        {

        }
    }
    protected void ddlpax_SelectedIndexChanged(object sender, EventArgs e)
    {
       

    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Totamt = Totamt + Convert.ToDouble(e.Row.Cells[2].Text);

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[2].Text = Totamt.ToString();

            }
        }

        catch
        {
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        Session["Rrate"] = null;
        Session["totpax"] = null;
        ViewState["VsRoomDetails"] = null;
        Session["BookedRooms"] = null;
        Response.Redirect(Request.RawUrl);
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            if (GridRoomPaxDetail.Rows.Count > 0)
            {
                #region Inserting Booking Data
                Session["cruiseBookingUrl"] = Request.Url.ToString();

                DataTable RoomDetails = ViewState["VsRoomDetails"] as DataTable;

               


                Session.Add("BookedRooms", RoomDetails);
                ///    
              
                //Response.Redirect("sendtoairpay.aspx?BookedId=" + BookedId + "&PackName=" + Request.QueryString["PackageName"].ToString() + "&NoOfNights=" + Request.QueryString["NoOfNights"].ToString() + "&CheckinDate=" + Request.QueryString["CheckinDate"].ToString());
                if (Session["Redirecturl"] == null)
                {

                    if (Convert.ToInt32(RoomDetails.Compute("SUM(Pax)", string.Empty))>= Convert.ToInt32(Session["totpax"]))
                    {
                        string Redirecturl = "SummarizedDetails.aspx?BookedId=" + BookedId + "&PackName=" + Request.QueryString["PackageName"].ToString() + "&NoOfNights=" + Request.QueryString["NoOfNights"].ToString() + "&CheckinDate=" + Request.QueryString["CheckinDate"].ToString() + "&PackId=" + Session["PackageId"].ToString();
                        Session["Redirecturl"] = Redirecturl;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('You have not selected enough Rooms to Accomodate all Guests ')", true);
                    }
                }
               
                Response.Redirect(Session["Redirecturl"].ToString());
                
                #endregion
            }

            else
            {


                pMessages.InnerText = "No rooms selected.";
            }

        }
        catch (Exception sqe)
        {

        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
       
    }
    #endregion



    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("SearchProperty.aspx");
    }
  
    protected void GridRoomPaxDetail_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
       
    }
    protected void GridRoomPaxDetail_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remove")
        {
            try
            {
                ImageButton imgbtn = (ImageButton)e.CommandSource;
                GridViewRow grow = (GridViewRow)imgbtn.NamingContainer;
                DataTable dtnew = ViewState["VsRoomDetails"] as DataTable;
                dtnew.Rows.RemoveAt(grow.RowIndex);
                dtnew.AcceptChanges();
                ViewState["VsRoomDetails"] = dtnew;
                Session["BookedRooms"] = dtnew;
                GridRoomPaxDetail.DataSource = dtnew;
                GridRoomPaxDetail.DataBind();
                calculateTotal();

            }
            catch
            {
            }
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Redirect(Session["DepartureSearchUrl"].ToString());

    }
    protected void GridRoomPaxDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
          
        }

        catch
        {
        }
    }
    protected void txtPassengers_TextChanged(object sender, EventArgs e)
    {
        
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        Session["totpax"] = txtPassengers.Text;

        Session["checkin"] = Request.QueryString["CheckIndate"];
        BindCruiseRoomRates();
    }


    public void BindCruiseRoomRates()
    {

        try
        {

         
            //  bindroomddl();
            bindRoomRates();
            setImageMap();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "blockArea", "blockArea();", true);
        }

        catch
        {
        }
    }
    protected void ImageMap1_Click(object sender, ImageMapEventArgs e)
    {
        try
        {

            //DataTable dtRoomsdata;

            //dtRoomsdata = bindroomddl();

            //DataView dvr = new DataView(dtRoomsdata);
            //dvr.RowFilter="RoomNo="+e.PostBackValue.ToString()+"";

            //if (dvr.ToTable().Rows.Count > 0)
            //{
            if (gdvRoomCategories.Rows.Count > 1)
            {
                dt = new DataTable();
                dt = Session["Rrate"] as DataTable;
                #region getRoomCategory
                blsr.action = "GetRoomCateId";

                hfRoomId.Value = e.PostBackValue.ToString();
                blsr.RoomId = hfRoomId.Value;
                blsr.PackageId = Session["PackageId"].ToString();

                roomCatId = dlsr.getRoomCategory(blsr);
                #endregion
                Int32.TryParse(ddlpax1rm.SelectedValue, out irpax);
                dv = new DataView();
                dv = new DataView(dt, "roomcategoryid='" + roomCatId + "'", "roomcategoryid", DataViewRowState.CurrentRows);
                if (ddlpax1rm.SelectedIndex > 0)
                {
                    if (Session["UserCode"] != null)
                    {
                        blsr._iAgentId = Convert.ToInt32(Session["UserCode"].ToString());

                        blsr.action = "Getmaxrooms";
                        blsr._dtStartDate = Convert.ToDateTime(Session["checkin"]);
                        dtGetReturnedData = new DataTable();
                        dtGetReturnedData = dlsr.getMaxRoomsBookable(blsr);
                        if (dtGetReturnedData != null)
                        {
                            if (GridRoomPaxDetail.Rows.Count < Convert.ToInt32(dtGetReturnedData.Rows[0][0]))
                            {
                                lblCurr.Text = dv.ToTable().Rows[0]["Currency"].ToString();
                                addrows(dv, roomCatId);
                            }


                        }
                    }
                    else
                    {
                        blsr._iAgentId = 247;

                        blsr.action = "Getmaxrooms";
                        blsr._dtStartDate = Convert.ToDateTime(Session["checkin"]);
                        dtGetReturnedData = new DataTable();
                        dtGetReturnedData = dlsr.getMaxRoomsBookable(blsr);
                        if (dtGetReturnedData != null)
                        {
                            if (GridRoomPaxDetail.Rows.Count < Convert.ToInt32(dtGetReturnedData.Rows[0][0]))
                            {
                                lblCurr.Text = dv.ToTable().Rows[0]["Currency"].ToString();
                                addrows(dv, roomCatId);
                            }


                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('No data found for this room,try selecting Passengers first')", true);
            }

            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('this room has already been booked .')", true);
            //}


        }
        catch
        {

        }

    }
}