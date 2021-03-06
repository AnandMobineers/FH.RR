﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net.Mail;
using iTextSharp;
using System.Text;


public partial class Hotel_AfterBookingDetails : System.Web.UI.Page
{

    DataTable Bookingdt;
    DataTable bookingmealdt;
    double TotalPaybleAmt = 0;
    public DataTable dtGetBookedRooms;
    public int LoopCounter = 0;

    BALAgentPayment blagentpayment = new BALAgentPayment();
    DALAgentPayment dlagentpayment = new DALAgentPayment();

    DALOpenDates dlOpenDates = new DALOpenDates();
    BALOpenDates blOpenDates = new BALOpenDates();

    BALCustomers blcus = new BALCustomers();
    DALCustomers dlcus = new DALCustomers();
    DataTable dtGetReturnedData;
    int getQueryResponse = 0;

    int CountryId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            Session["Redirection"] = "../../Hotel/AfterBookingDetails.aspx";
            if (Session["UserCode"] != null||Session["CustomerCode"]!=null)
            {

                if (Session["UserCode"] != null)
                {
                    BookRef.Style.Remove("display");
                    ReqBookRef.Enabled = true;
                }
                else
                {
                    BookRef.Style.Add("display", "None");
                    ReqBookRef.Enabled = false;
                }
               
                lnkLogout.Visible = true;
            }
            else
            {


                BookRef.Style.Add("display", "None");

                ReqBookRef.Enabled = false;
                lnkLogout.Visible = false;

            }
            this.pnlLogin.Visible = false;
            this.pnlFullDetails.Visible = false;
            pnlBookButton.Visible = false;
            panelwithoutCreditAgent.Visible = false;
            Bookingdt = new DataTable();
            Bookingdt = Session["Bookingdt"] as DataTable;
            bookingmealdt = new DataTable();
            bookingmealdt = Session["BookinMealdt"] as DataTable;
            gdvSelectedRooms.DataSource = Bookingdt;
            gdvSelectedRooms.DataBind();

            //lblChkin.Text = Session["Chkin"].ToString();
            //lblChkout.Text = Session["chkout"].ToString();

            calcamt(Bookingdt);
            LoadCountries();

            pnlCustReg.Visible = false;
            customerLogin.Visible = false;
          


        }
        Bookingdt = Session["Bookingdt"] as DataTable;

        preparetables(Bookingdt);
    }

    public void preparetables(DataTable bdt)
    {
        try
        {


            for (int k = 0; k < bdt.Rows.Count; k++)
            {


                Table tbl1 = new Table();
                TableRow tr;
                TableCell tc;
                tbl1.Style.Add("background-color", "floralwhite");
                tbl1.Style.Add("padding", "10px");
                tbl1.Style.Add("Height", "235px");
                tbl1.Style.Add("width", "417px");
                tbl1.Style.Add("text-align", "center");
                tbl1.Style.Add("border", "1px solid #eaeaea");
                tbl1.Style.Add("margin-bottom", "10px");
                tr = new TableRow();
                tc = new TableCell();
                //Request.QueryString["AccomName"].ToString();
                tc.Text = "Room" + (k + 1).ToString();
                tc.Style[HtmlTextWriterStyle.FontWeight] = "Bold";
                tr.Cells.Add(tc);

                tbl1.Rows.Add(tr);


                tr = new TableRow();
                tc = new TableCell();
                //Request.QueryString["AccomName"].ToString();
                tc.Text = Request.QueryString["AccomName"].ToString();
                tc.Style[HtmlTextWriterStyle.Color] = "chocolate";
                tr.Cells.Add(tc);

                tbl1.Rows.Add(tr);


                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Check in:";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Session["Chkin"].ToString(); ;
                tr.Cells.Add(tc);
                tbl1.Rows.Add(tr);

                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Check out:";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = Session["chkout"].ToString();
                tr.Cells.Add(tc);
                tbl1.Rows.Add(tr);

                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Guests:";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = bdt.Rows[k]["Pax"].ToString();
                tr.Cells.Add(tc);
                tbl1.Rows.Add(tr);

                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Room:";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = bdt.Rows[k]["CategoryName"].ToString();
                tr.Cells.Add(tc);
                tbl1.Rows.Add(tr);

                tr = new TableRow();

                tc = new TableCell();
                tc.Text = "Total";

                tr.Cells.Add(tc);

                tc = new TableCell();
                tc.Text = bdt.Rows[k]["Total"].ToString();

                tr.Cells.Add(tc);
                tbl1.Rows.Add(tr);






                pnlRooms.Controls.Add(tbl1);


            }



            lblCurrency.Text = bdt.Rows[0]["Currency"].ToString();




        }

        catch
        {
        }
    }



    public void calcamt(DataTable dts)
    {
        try
        {



            for (int j = 0; j < dts.Rows.Count; j++)
            {
                string[] arr = dts.Rows[j]["Total"].ToString().Split(' ');

                TotalPaybleAmt = TotalPaybleAmt + Convert.ToInt32(arr[1]); ;
            }
            hdnfTotalPaybleAmt.Value = TotalPaybleAmt.ToString();

            txtPaidAmt.Text = Math.Round(((25 * TotalPaybleAmt) / 100)).ToString("N2");
            Bookingdt = Session["Bookingdt"] as DataTable;

            lbltotAmt.Text = Bookingdt.Rows[0]["Currency"].ToString() + " " + TotalPaybleAmt.ToString();
            lblCurrency.Text = Bookingdt.Rows[0]["Currency"].ToString().ToString() + " ";
            txtPaidAmt.Text = Math.Round(((25 * TotalPaybleAmt) / 100)).ToString("#.##");
            hftxtpaidamt.Value = Convert.ToDouble(txtPaidAmt.Text).ToString("N2").Replace(",", "");

            lblBalanceAmt.Text = Bookingdt.Rows[0]["Currency"].ToString().ToString() + " " + Math.Round((TotalPaybleAmt - Convert.ToDouble(txtPaidAmt.Text))).ToString();

            //   lbltotAmt.Text = dtGetBookedRooms.Rows[0]["Currency"].ToString() + " " + TotalPaybleAmt.ToString();

        }

        catch
        {
        }
    }
    protected void btnPayProceed_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Hotel"] = 1;
            if (btnPayProceed.Text == "Proceed For Payment")
            {


                if (Session["UserCode"] != null)
                {
                    //aev@farhorizonindia.com [1:48:55 PM] Augurs  Technologies Pvt. Ltd.: 12345

                    DataTable dtrpax = Session["Bookingdt"] as DataTable;

                    string BookRef = txtBookRef.Text + "X" + Convert.ToDouble(dtrpax.Compute("SUM(Pax)", string.Empty)).ToString();

                    Session.Add("BookingRef", BookRef);
                    Session["Paid"] = Convert.ToDouble(txtPaidAmt.Text.Trim() == "" ? "0" : txtPaidAmt.Text.Trim());
                    blagentpayment._Action = "MailValidate";
                    blagentpayment._EmailId = Session["AgentMailId"].ToString();
                    blagentpayment._Password = Session["Password"].ToString();
                    dtGetReturnedData = dlagentpayment.BindControls(blagentpayment);
                    if (dtGetReturnedData.Rows.Count > 0)
                    {
                        string BookingPayId = lbPaymentMethod.Text.Trim().Substring(0, 2) + DateTime.Now.ToString("MMddhhmmssfff");
                        string Email = Session["AgentMailId"].ToString();
                        string PhoneNumber = "9999999999";// hdnfPhoneNumber.Value.Trim().ToString();
                        string FirstName = dtGetReturnedData.Rows[0]["FirstName"].ToString();
                        string LastName = "XYZ";// dtGetReturnedData.Rows[0]["LastName"].ToString();
                        string PaidAmt = hftxtpaidamt.Value.Trim().ToString();
                        string PaymentId = BookingPayId.ToString();
                        string BillingAddress = "abc/wsdd,vasant vihar";// lblBillingAddress.Text.Trim().ToString();
                        Session["BookingPayId"] = txtBookRef.Text.Trim();// BookingPayId;

                        Session["Address"] = lblBillingAddress.Text.Trim().ToString();
                        Session["InvName"] = FirstName;

                        Session["SubInvName"] = FirstName;
                        string[] arr = {};
                        if (FirstName != "" && FirstName != null)
                        {
                            arr = FirstName.Split(' ');
                        }

                        //Response.Redirect("PaymentGatewayResponse.aspx?BookingPayId=" + PaymentId + "&EmailId=" + Email.ToString() + "&PhoneNumber=" + PhoneNumber.ToString() + "&FirstName=" + FirstName.ToString() + "&LastName=" + LastName.ToString() + "&PaidAmt=" + PaidAmt.ToString() + "&BillingAddress=" + BillingAddress.ToString());
                        //http://adventureresortscruises.in/Cruise/booking/sendtoairpay.aspx?BookedId=0&PackName=7N8D+Downstream+Cruise&NoOfNights=7&CheckinDate=12%2f4%2f2016&PackId=Pack4
                        Response.Redirect("~/Cruise/booking/sendtoairpay.aspx?BookingPayId=" + PaymentId + "&EmailId=" + Email.ToString() + "&PhoneNumber=" + PhoneNumber.ToString() + "&FirstName=" +arr[0].ToString() + "&LastName=" + LastName.ToString() + "&PaidAmt=" + PaidAmt.ToString() + "&BillingAddress=" + BillingAddress.ToString());
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('not registered!!!')", true);

                    }
                }
                else
                {

                   


                    Session.Add("BookingRef", ViewState["BookRef"].ToString());


                    blcus.Email =Session["CustomerMailId"].ToString();
                    blcus.Password = Session["CustPassword"].ToString();

                    blcus.action = "LoginCust";
                    dtGetReturnedData = new DataTable();
                    dtGetReturnedData = dlcus.checkDuplicateemail(blcus);
                    Session["Paid"] = Convert.ToDouble(txtPaidAmt.Text.Trim() == "" ? "0" : txtPaidAmt.Text.Trim());
                    if (dtGetReturnedData.Rows.Count > 0)
                    {
                        Random rnd = new Random();
                        string BookingPayId = rnd.Next(10000, 20000).ToString() + DateTime.Now.ToString("MMddhhmmssfff");
                        Session["BookingPayId"] = BookingPayId;
                        string Email = Session["CustomerMailId"].ToString();

                        string PhoneNumber = "9999999999";// hdnfPhoneNumber.Value.Trim().ToString();


                        string FirstName = dtGetReturnedData.Rows[0]["FirstName"].ToString();
                        string LastName = "XYZ";// dtGetReturnedData.Rows[0]["LastName"].ToString();
                        string PaidAmt = hftxtpaidamt.Value.Trim().ToString();
                        string PaymentId = BookingPayId.ToString();
                        string BillingAddress = "abc/wsdd,vasant vihar";// lblBillingAddress.Text.Trim().ToString();
                        Session["Address"] = lblBillingAddress.Text.Trim().ToString();
                        Session["InvName"] = dtGetReturnedData.Rows[0]["Title"].ToString() + " " + dtGetReturnedData.Rows[0]["LastName"].ToString();
                        Session["SubInvName"] = dtGetReturnedData.Rows[0]["LastName"].ToString()+", " + dtGetReturnedData.Rows[0]["Title"].ToString() + " " + FirstName;

                        string[] arr={};
                        if (FirstName != "" && FirstName != null)
                        {
                            arr = FirstName.Split(' ');
                        }
                       

                        Response.Redirect("~/Cruise/booking/sendtoairpay.aspx?BookingPayId=" + PaymentId + "&EmailId=" + Email.ToString() + "&PhoneNumber=" + PhoneNumber.ToString() + "&FirstName=" + arr[0].ToString() + "&LastName=" + LastName.ToString() + "&PaidAmt=" + PaidAmt.ToString() + "&BillingAddress=" + BillingAddress.ToString());

                        //http://localhost:1897/ResortManager/Cruise/booking/SummerisedDetails.aspx?BookedId=0&PackName=Ganges+Exclusive&NoOfNights=5&CheckinDate=5%2f1%2f2016

                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('not registered!!!')", true);

                    }
                }
            }
            else
            {

                if (Convert.ToDecimal(txtPaidAmt.Text) <= Convert.ToDecimal(hdnfCreditLimit.Value))
                {
                    string BookingPayId = lbPaymentMethod.Text.Trim().Substring(0, 2) + DateTime.Now.ToString("MMddhhmmssfff");
                    Session["BookingPayId"] = BookingPayId;
                    Session.Add("BookingRef", txtBookRef.Text.Trim().ToString());
                    Session["Paid"] = Convert.ToDouble(hftxtpaidamt.Value.Trim() == "" ? "0" : hftxtpaidamt.Value.Trim().ToString());

                    Session["InvName"] = Session["UserName"].ToString();
                    Session["Address"] = null;
                    Response.Redirect("PaymentGatewayResponse.aspx");
                }
                else
                {
                    lblPaymentErr.Text = "Payment Amount Exceeding Credit Limit";
                }
            }
        }
        catch
        {

        }
    }
    protected void btnSbmt_Click(object sender, EventArgs e)
    {

    }
    protected void btnSmbt_Click(object sender, EventArgs e)
    {
        pnlCustReg.Visible = false;
        if (Session["UserCode"] != null)
        {
            //  pnlLogin.Visible = true;
            try
            {
                customerLogin.Visible = false;
                if (Session["AgentMailId"] != null && Session["Password"] != null)
                {
                    blagentpayment._Action = "MailValidate";
                    blagentpayment._EmailId = Session["AgentMailId"].ToString();
                    blagentpayment._Password = Session["Password"].ToString();
                    dtGetReturnedData = dlagentpayment.BindControls(blagentpayment);
                    if (dtGetReturnedData.Rows.Count > 0)
                    {

                        lblAgentName.Text = Session["UserName"].ToString();
                        lblBillingAddress.Text = dtGetReturnedData.Rows[0]["BillingAddress"].ToString();
                        lbPaymentMethod.Text = dtGetReturnedData.Rows[0]["PaymentMethod"].ToString();
                        hdnfPhoneNumber.Value = dtGetReturnedData.Rows[0]["Phone"].ToString();
                        hdnfCreditLimit.Value = dtGetReturnedData.Rows[0]["CreditLimit"].ToString();
                        bool oncredit = Convert.ToBoolean(dtGetReturnedData.Rows[0]["ChkCredit"].ToString());


                        pnlFullDetails.Visible = true;
                        pnlBookButton.Visible = true;


                        if (oncredit)
                        {
                            panelwithoutCreditAgent.Visible = false;

                            btnPayProceed.Text = "Book";
                        }
                        else
                        {
                            panelwithoutCreditAgent.Visible = true;
                            btnPayProceed.Text = "Proceed For Payment";
                        }




                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Agent Payment Details Not found')", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('It seems you are not logged in')", true);
                }
            }
            catch (Exception sqe)
            {

            }




        }

        else if (Session["CustomerCode"] != null)
        {

            try
            {
                blcus.Email = Session["CustomerMailId"].ToString(); ;
                blcus.Password = Session["CustPassword"].ToString();
                blcus.action = "LoginCust";
                dtGetReturnedData = new DataTable();
                dtGetReturnedData = dlcus.checkDuplicateemail(blcus);
                if (dtGetReturnedData != null)
                {


                    Session["CustMailId"] = Session["CustomerMailId"].ToString();


                    ViewState["Pass"] = txtCustPass.Text.Trim();
                    lblBillingAddress.Text = dtGetReturnedData.Rows[0]["BillingAddress"].ToString();


                    lblAgentName.Text = dtGetReturnedData.Rows[0]["FirstName"].ToString() + " " + dtGetReturnedData.Rows[0]["LastName"].ToString();
                    lbPaymentMethod.Text = dtGetReturnedData.Rows[0]["PaymentMethod"].ToString();
                    hdnfPhoneNumber.Value = dtGetReturnedData.Rows[0]["Telephone"].ToString();
                    Session["CustId"] = dtGetReturnedData.Rows[0]["CustId"].ToString();

                    DataTable dtrpax = Session["Bookingdt"] as DataTable;

                    string BookRef = dtGetReturnedData.Rows[0]["FirstName"].ToString() + dtGetReturnedData.Rows[0]["LastName"].ToString() + "X" + Convert.ToDouble(dtrpax.Compute("SUM(Pax)", string.Empty)).ToString() + "-" + "Direct Client";
                    ViewState["BookRef"] = BookRef;
                    lbPaymentMethod.Text = "Online";


                    pnlFullDetails.Visible = true;
                    panelwithoutCreditAgent.Visible = true;
                    pnlBookButton.Visible = true;
                    customerLogin.Visible = false;
                }
                Bookingdt = Session["Bookingdt"] as DataTable;
               // preparetables(Bookingdt);

            }
            catch
            {

            }
        }
        else
        {
            pnlLogin.Visible = false;
            customerLogin.Visible = true;

        }

        //Bookingdt = Session["Bookingdt"] as DataTable;
        //preparetables(Bookingdt);
    }
    protected void btnCustLogin_Click(object sender, EventArgs e)
    {
        try
        {
            blcus.Email = txtCustMailId.Text.Trim();
            blcus.Password = txtCustPass.Text.Trim();
            blcus.action = "LoginCust";
            dtGetReturnedData = new DataTable();
            dtGetReturnedData = dlcus.checkDuplicateemail(blcus);
            if (dtGetReturnedData != null)
            {

                if (dtGetReturnedData.Rows.Count > 0)
                {
                    Session["CustMailId"] = txtCustMailId.Text.Trim();


                    ViewState["Pass"] = txtCustPass.Text.Trim();
                    lblBillingAddress.Text = dtGetReturnedData.Rows[0]["BillingAddress"].ToString();


                    lblAgentName.Text = dtGetReturnedData.Rows[0]["FirstName"].ToString() + " " + dtGetReturnedData.Rows[0]["LastName"].ToString();
                    lbPaymentMethod.Text = dtGetReturnedData.Rows[0]["PaymentMethod"].ToString();
                    hdnfPhoneNumber.Value = dtGetReturnedData.Rows[0]["Telephone"].ToString();
                    Session["CustId"] = dtGetReturnedData.Rows[0]["CustId"].ToString();

                    Session["CustomerCode"] = dtGetReturnedData.Rows[0]["CustId"].ToString();
                    Session.Add("CustomerMailId", dtGetReturnedData.Rows[0]["Email"].ToString());
                    Session.Add("CustPassword", dtGetReturnedData.Rows[0]["Password"].ToString());
                    DataTable dtrpax = Session["Bookingdt"] as DataTable;

                    string BookRef = dtGetReturnedData.Rows[0]["FirstName"].ToString() + dtGetReturnedData.Rows[0]["LastName"].ToString() + "X" + Convert.ToDouble(dtrpax.Compute("SUM(Pax)", string.Empty)).ToString() + "-" + "Direct Client";
                    ViewState["BookRef"] = BookRef;
                    lbPaymentMethod.Text = "Online";


                    pnlFullDetails.Visible = true;
                    panelwithoutCreditAgent.Visible = true;
                    pnlBookButton.Visible = true;
                    customerLogin.Visible = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Password or Email Id incorrect')", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Password or Email Id incorrect')", true);
            }
            //Bookingdt = Session["Bookingdt"] as DataTable;
          //  preparetables(Bookingdt);

        }
        catch
        {

        }
    }
    private void LoadCountries()
    {
        try
        {
            blOpenDates._Action = "GetCountry";
            dtGetReturnedData = dlOpenDates.BindControls(blOpenDates);
            if (dtGetReturnedData.Rows.Count > 0)
            {
                ddlCountry.DataSource = dtGetReturnedData;
                ddlCountry.DataTextField = "CountryName";
                ddlCountry.DataValueField = "CountryId";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-Select-", "0"));
            }
            else
            {
                ddlCountry.Items.Clear();
                ddlCountry.DataSource = null;
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-Select-", "0"));
            }
        }
        catch (Exception sqe)
        {
            ddlCountry.Items.Clear();
            ddlCountry.DataSource = null;
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, "-No Accom-");

        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ViewState["CustPass"] = txtPassWord.Text.Trim();
        sendMail();

        pnlCustReg.Visible = false;
        //try
        //{
        //    blcus.action = "InsCustomers";
        //    blcus.Address1 = txtAddress1.Text;
        //    blcus.Address2 = txtaddress2.Text;
        //    blcus.City = txtCity.Text;
        //    Int32.TryParse(ddlCountry.SelectedValue, out CountryId);
        //    blcus.CountryId = CountryId;
        //    blcus.Email = txtMailAddress.Text.Trim();
        //    blcus.FirstName = txtFirstName.Text;
        //    blcus.LastName = txtLastName.Text;
        //    blcus.PostalCode = txtPostcode.Text;
        //    blcus.State = txtState.Text;
        //    blcus.Telephone = txtTelephone.Text.Trim();
        //    blcus.Password = txtPassWord.Text.Trim();
        //    blcus.Title = ddltitle.SelectedItem.Text;
        //    blcus.PaymentMethod = txtPaymentMethod.Text.Trim();
        //    getQueryResponse = dlcus.AddCustomers(blcus);
        //    if (getQueryResponse > 0)
        //    {
        //        ScriptManager.RegisterStartupScript(upd1, typeof(string), "alertscipt", "alert('Customer Added Successfully')", true);
        //        pnlCustReg.Visible = false;
        //        customerLogin.Visible = true;

        //    }
        //    else
        //    {
        //        ScriptManager.RegisterStartupScript(upd1, typeof(string), "alertscipt", "alert('Customer could not be Added ')", true);
        //    }


        //}

        //catch
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Please check entries')", true);
        //}
    }

    public void sendMail()
    {
        try
        {


            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("adventureresortscruises.in");

            mail.From = new MailAddress("reservations@adventureresortscruises.in");

            mail.To.Add(txtMailAddress.Text.Trim());


            mail.Subject = "Mail Verification";

            Random rnd = new Random();
            string Code = rnd.Next(10000, 99999).ToString();
            hfVCode.Value = Code;




            StringBuilder sb = new StringBuilder();

            sb.Append("<div>");
            sb.Append("<div> Dear " + txtFirstName.Text + ",</div> <div><br/></div><div>Thanks for your registering with us.</div> <div><br/> </div><div>For security reasons we have added this step so that we verify the email address before any booking details is sent across.</div> <div><br/></div> ");
            sb.Append(" <div>To verify your email address please enter the code " + Code + " in the registration screen. </div> <div><br/> </div><div>Do contact us if you have any issue at reservations@adventureresort</div><div><br/></div><div>Thanking you,</div><div><br/></div><div>Reservations Office</div> ");
            sb.Append("</div>");
            sb.Append("<img src='http://adventureresortscruises.in/Cruise/booking/img_logo.png' alt='Image'/><br /><div> Adventure Resorts & Cruises Pvt. Ltd.</div><div> B209, CR Park, New Delhi 110019 </div> <div> Phone: +91 - 011 - 41057370 / 1 / 2 </div><div> Mobile: +91 - 9599755353 </div><div><br/> </div> ");


            mail.IsBodyHtml = true;


            mail.Body = sb.ToString();






            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("reservations@adventureresortscruises.in", "Augurs@123");
            SmtpServer.EnableSsl = false;

            SmtpServer.Send(mail);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Please check your Mail for the Verification Code')", true);
            pnlCustReg.Visible = false;
            customerLogin.Visible = true;

            TableCust.Visible = false;
            tableVerify.Visible = true;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('" + ex.Message.ToString() + "')", true);
        }
    }



    protected void txtMailAddress_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (IsValid(txtMailAddress.Text.Trim()))
            {
                blcus.action = "chkDuplicate";
                blcus.Email = txtMailAddress.Text.Trim();
                dtGetReturnedData = dlcus.checkDuplicateemail(blcus);
                if (dtGetReturnedData != null)
                {
                    if (dtGetReturnedData.Rows.Count > 0)
                    {
                        lblError.Text = "This Email Id already Exists";
                        txtMailAddress.Text = "";
                    }
                    else
                    {
                        lblError.Text = "";
                    }
                }
            }
            else
            {
                lblError.Text = "Invalid Email Id";
            }

        }

        catch
        {
        }
    }


    public bool IsValid(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
    protected void txtRegNow_Click(object sender, EventArgs e)
    {
        pnlCustReg.Visible = true;
    }
    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("../Cruise/booking/SearchProperty.aspx");
    }
    protected void btnCloseCust_Click(object sender, EventArgs e)
    {
        pnlCustReg.Visible = false;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect(Session["HotelBokingUrl"].ToString());
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCode.Text == hfVCode.Value)
            {

                ClientRegister();
                txtCode.Text = "";
                tableVerify.Visible = false;
                TableCust.Visible = true;

            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Wrong Code Entered!')", true);
                TableCust.Visible = false;
                tableVerify.Visible = true;
            }
        }
        catch
        {
        }
    }

    public void ClientRegister()
    {
        try
        {
            blcus.action = "InsCustomers";
            blcus.Address1 = txtAddress1.Text;
            blcus.Address2 = txtaddress2.Text;
            blcus.City = txtCity.Text;
            Int32.TryParse(ddlCountry.SelectedValue, out CountryId);
            blcus.CountryId = CountryId;
            blcus.Email = txtMailAddress.Text.Trim();
            blcus.FirstName = txtFirstName.Text;
            blcus.LastName = txtLastName.Text;
            blcus.PostalCode = txtPostcode.Text;
            blcus.State = txtState.Text;
            blcus.Telephone = txtTelephone.Text.Trim();
            blcus.Password = ViewState["CustPass"].ToString();
            blcus.Title = ddltitle.SelectedItem.Text;
            blcus.PaymentMethod = "Online";
               // txtPaymentMethod.Text.Trim();


            getQueryResponse = dlcus.AddCustomers(blcus);


            if (getQueryResponse > 0)
            {


                ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Verification Done! Please Login')", true);


            }
            else
            {

            }


        }

        catch
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Showstatus", "javascript:alert('Please check entries')", true);
        }
    }
}