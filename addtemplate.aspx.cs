using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;

public partial class addtemplate : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FileUpload1.Visible = true;
            Button1.Visible = true;
            btnback.Visible = true;
        }
    }
    protected void back(object sender, EventArgs e)
    {
        Response.Redirect("login");
    }
    protected void kanakanaas(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[1].Text == "&nbsp;")//Idnumber
            {
                e.Row.Cells[1].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[2].Text == "&nbsp;")//BiometricsNumber
            {
                e.Row.Cells[2].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[3].Text == "&nbsp;")//LastName
            {
                e.Row.Cells[3].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[4].Text == "&nbsp;")//FirstName
            {
                e.Row.Cells[4].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[12].Text == "&nbsp;")//BirthDate
            {
                e.Row.Cells[12].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[14].Text == "&nbsp;")//DateHired
            {
                e.Row.Cells[14].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[41].Text == "&nbsp;")//EmployeeStatus
            {
                e.Row.Cells[41].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[45].Text == "&nbsp;")//PayrollGroup
            {
                e.Row.Cells[45].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[27].Text == "&nbsp;")//PayrollType
            {
                e.Row.Cells[27].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[28].Text == "&nbsp;")//FixNoOfDays
            {
                e.Row.Cells[28].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[29].Text == "&nbsp;")//FixNoOfHours
            {
                e.Row.Cells[29].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[30].Text == "&nbsp;")//MonthlyRate
            {
                e.Row.Cells[30].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[31].Text == "&nbsp;")//PayrollRate
            {
                e.Row.Cells[31].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[32].Text == "&nbsp;")//DailyRate
            {
                e.Row.Cells[32].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[33].Text == "&nbsp;")//HourlyRate
            {
                e.Row.Cells[33].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[35].Text == "&nbsp;")//Company
            {
                e.Row.Cells[35].BackColor = System.Drawing.Color.Red;
            }
            if (e.Row.Cells[40].Text == "&nbsp;")//ShiftCode
            {
                e.Row.Cells[40].BackColor = System.Drawing.Color.Red;
            }
        }
    }
    private string GetExcelSheetNames(string excelFile)
    {
        OleDbConnection objConn = null;
        System.Data.DataTable dt = null;

        try
        {
            string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", excelFile);
            objConn = new OleDbConnection(connString);
            objConn.Open();
            dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            string excelSheets = dt.Rows[0]["TABLE_NAME"].ToString();
            return excelSheets;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (objConn != null)
            {
                objConn.Close();
                objConn.Dispose();
            }
            if (dt != null)
            {
                dt.Dispose();
            }
        }
    }
    protected void loademployee(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
        {
            System.Data.DataSet DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string path = string.Concat(Server.MapPath("~/Excel/" + FileUpload1.FileName));
            FileUpload1.SaveAs(path);
            string excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 8.0", path);
            OleDbConnection MyConnection = new OleDbConnection();
            MyConnection.ConnectionString = excelConnectionString;
            MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [" + GetExcelSheetNames(path) + "]", MyConnection);
            MyCommand.TableMappings.Add("Table", "TestTable");
            DtSet = new System.Data.DataSet();
            MyCommand.Fill(DtSet);
            MyConnection.Close();

            ViewState["test"] = DtSet.Tables[0];

            GridView1.DataSource = DtSet.Tables[0];
            GridView1.DataBind();

            DataTable dt = DtSet.Tables[0];

            DataRow[] dr = dt.Select();

            galamay(DtSet.Tables[0]);

            btnback.Visible = false;
            FileUpload1.Visible = false;
            Button1.Visible = false;
            Button4.Visible = true;
            gridcompanydistinct.Visible = true;
            gridbranch.Visible = true;
            gridposition.Visible = true;
            //gridshiftcode.Visible = true;
            griddepartment.Visible = true;
            gridggroup.Visible = true;
            gridsction.Visible = true;
        }
        else
        {
            Response.Write("Please Load Template!!!");
        }
    }
    protected void galamay(DataTable company)
    {
        DataTable dtcompany = new DataTable();
        dtcompany.Columns.Add(new DataColumn("Company", typeof(string)));
        DataRow drcom;
        foreach (DataRow drc in company.Rows)
        {
            DataRow[] chkexist = dtcompany.Select("Company='" + drc["Company"] + "'");
            if (chkexist.Count() == 0)
            {
                drcom = dtcompany.NewRow();
                drcom["Company"] = drc["company"];
                if (drc["Company"].ToString() == "")
                { }
                else
                {
                    dtcompany.Rows.Add(drcom);
                }
            }
        }
        ViewState["VSCompany"] = dtcompany;
        gridcompanydistinct.DataSource = dtcompany;
        gridcompanydistinct.DataBind();

        DataTable dtbranch = new DataTable();
        dtbranch.Columns.Add(new DataColumn("Branch", typeof(string)));
        DataRow drbra;
        foreach (DataRow drb in company.Rows)
        {
            DataRow[] chkexist = dtbranch.Select("Branch='" + drb["branch"] + "'");
            if (chkexist.Count() == 0)
            {
                drbra = dtbranch.NewRow();
                drbra["Branch"] = drb["branch"];
                if (drb["Branch"].ToString() == "")
                { }
                else
                {
                    dtbranch.Rows.Add(drbra);
                }
            }
        }
        ViewState["VSBranch"] = dtbranch;
        gridbranch.DataSource = dtbranch;
        gridbranch.DataBind();

        DataTable dtdept = new DataTable();
        dtdept.Columns.Add(new DataColumn("Department", typeof(string)));
        DataRow drdept;
        foreach (DataRow drdep in company.Rows)
        {
            DataRow[] chkexist = dtdept.Select("Department='" + drdep["department"] + "'");
            if (chkexist.Count() == 0)
            {
                drdept = dtdept.NewRow();
                drdept["Department"] = drdep["department"];
                if (drdep["Department"].ToString() == "")
                { }
                else
                {
                    dtdept.Rows.Add(drdept);
                }
            }
        }
        ViewState["VSDepartment"] = dtdept;
        griddepartment.DataSource = dtdept;
        griddepartment.DataBind();

        //payroll ni
        DataTable dtpgroup = new DataTable();
        dtpgroup.Columns.Add(new DataColumn("Payrollgroup", typeof(string)));
        DataRow drpgroup;
        foreach (DataRow drpg in company.Rows)
        {
            DataRow[] chkexist = dtpgroup.Select("Payrollgroup='" + drpg["payrollgroup"] + "'");
            if (chkexist.Count() == 0)
            {
                drpgroup = dtpgroup.NewRow();
                drpgroup["Payrollgroup"] = drpg["payrollgroup"];
                if (drpg["Payrollgroup"].ToString() == "")
                { }
                else
                {
                    dtpgroup.Rows.Add(drpgroup);
                }
            }
        }
        ViewState["VSPGroup"] = dtpgroup;
        gridggroup.DataSource = dtpgroup;
        gridggroup.DataBind();

        DataTable dtsecode = new DataTable();
        dtsecode.Columns.Add(new DataColumn("Section", typeof(string)));
        DataRow drseccode;
        foreach (DataRow drsc in company.Rows)
        {
            DataRow[] chkexist = dtsecode.Select("Section='" + drsc["section"] + "'");
            if (chkexist.Count() == 0)
            {
                drseccode = dtsecode.NewRow();
                drseccode["Section"] = drsc["section"];
                if (drsc["Section"].ToString() == "")
                { }
                else
                {
                    dtsecode.Rows.Add(drseccode);
                }
            }
        }
        ViewState["VSseccode"] = dtsecode;

        DataTable dtrelegion = new DataTable();
        dtrelegion.Columns.Add(new DataColumn("Relegion", typeof(string)));
        DataRow dtrrelegion;
        foreach (DataRow datarow in company.Rows)
        {
            DataRow[] checkexists = dtrelegion.Select("Relegion='" + datarow["relegion"] + "'");
            if (checkexists.Count() == 0)
            {
                dtrrelegion = dtrelegion.NewRow();
                dtrrelegion["Relegion"] = datarow["relegion"];
                if (datarow["Relegion"].ToString() == "")
                { }
                else
                {
                    dtrelegion.Rows.Add(dtrrelegion);
                }
            }
        }
        ViewState["VSrelegion"] = dtrelegion;

        DataTable dtzipcode = new DataTable();
        dtzipcode.Columns.Add(new DataColumn("Zipcode", typeof(string)));
        DataRow dtrzipcode;
        foreach (DataRow dtr in company.Rows)
        {
            DataRow[] check = dtzipcode.Select("Zipcode = '" + dtr["Zipcode"] + "'");
            if (check.Count() == 0)
            {
                dtrzipcode = dtzipcode.NewRow();
                dtrzipcode["Zipcode"] = dtr["zipcode"];
                if (dtr["Zipcode"].ToString() == "")
                { }
                else
                {
                    dtzipcode.Rows.Add(dtrzipcode);
                }
            }
        }
        ViewState["VSzipcode"] = dtzipcode;

        DataTable dtcitizenship = new DataTable();
        dtcitizenship.Columns.Add(new DataColumn("Citizenship", typeof(string)));
        DataRow dtrcitizen;
        foreach (DataRow dtrcitizenhip in company.Rows)
        {
            DataRow[] check = dtcitizenship.Select("Citizenship='" + dtrcitizenhip["Citizenship"] + "'");
            if (check.Count() == 0)
            {
                dtrcitizen = dtcitizenship.NewRow();
                dtrcitizen["Citizenship"] = dtrcitizenhip["citizenship"];
                if (dtrcitizenhip["Citizenship"].ToString() == "")
                { }
                else
                {
                    dtcitizenship.Rows.Add(dtrcitizen);
                }
            }
        }
        ViewState["VScitizenship"] = dtcitizenship;

        DataTable dtposition = new DataTable();
        dtposition.Columns.Add(new DataColumn("Position", typeof(string)));
        DataRow drpost;
        foreach (DataRow drpst in company.Rows)
        {
            DataRow[] chkexist = dtposition.Select("Position='" + drpst["position"] + "'");
            if (chkexist.Count() == 0)
            {
                drpost = dtposition.NewRow();
                drpost["Position"] = drpst["position"];
                if (drpst["Position"].ToString() == "")
                { }
                else
                {
                    dtposition.Rows.Add(drpost);
                }
            }
        }
        ViewState["VSPosition"] = dtposition;
        gridposition.DataSource = dtposition;
        gridposition.DataBind();

        DataTable dtempstatus = new DataTable();
        dtempstatus.Columns.Add(new DataColumn("Employeestatus", typeof(string)));
        DataRow dtrempstatus;
        foreach (DataRow dtrempstat in company.Rows)
        {
            DataRow[] check = dtempstatus.Select("Employeestatus='" + dtrempstat["employeestatus"] + "'");
            if (check.Count() == 0)
            {
                dtrempstatus = dtempstatus.NewRow();
                dtrempstatus["Employeestatus"] = dtrempstat["employeestatus"];
                if (dtrempstat["Employeestatus"].ToString() == "")
                { }
                else
                {
                    dtempstatus.Rows.Add(dtrempstatus);
                }
            }
        }
        ViewState["VSEmpstatus"] = dtempstatus;

        DataTable dtlevel = new DataTable();
        dtlevel.Columns.Add(new DataColumn("Level", typeof(string)));
        DataRow dtrlevel;
        foreach (DataRow drlevel in company.Rows)
        {
            DataRow[] check = dtlevel.Select("Level='" + drlevel["level"] + "'");
            if (check.Count() == 0)
            {
                dtrlevel = dtlevel.NewRow();
                dtrlevel["Level"] = drlevel["level"];
                if (drlevel["Level"].ToString() == "")
                { }
                else
                {
                    dtlevel.Rows.Add(dtrlevel);
                }
            }
        }
        ViewState["VSLevel"] = dtlevel;

        DataTable dtinternal = new DataTable();
        dtinternal.Columns.Add(new DataColumn("InternalOrder", typeof(string)));
        DataRow datarowinternal;
        foreach (DataRow drinternal in company.Rows)
        {
            DataRow[] check = dtinternal.Select("InternalOrder='" + drinternal["internalorder"] + "'");
            if (check.Count() == 0)
            {
                datarowinternal = dtinternal.NewRow();
                datarowinternal["InternalOrder"] = drinternal["internalorder"];
                if (drinternal["InternalOrder"].ToString() == "")
                { }
                else { dtinternal.Rows.Add(datarowinternal); }
            }
        }
        ViewState["VSInternalOrder"] = dtinternal;
    }
    protected void saveselectdistinct(object sender, EventArgs e)
    {
        //Company, Department, Position, PayrollGroup, Branch, Section, Zipcode, Religion, shiftcode, empstatus, level, InternalOrder
        DataTable internalorder = ViewState["VSInternalOrder"] as DataTable;
        foreach (DataRow drsinternal in internalorder.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MInternalOrder where InternalOrder like '%" + drsinternal["InternalOrder"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MInternalOrder (InternalOrder) values ('" + drsinternal["InternalOrder"].ToString() + "')");
            }
        }
        DataTable level = ViewState["VSLevel"] as DataTable;
        foreach (DataRow drslevel in level.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MDivision2 where Division2 = '" + drslevel["Level"].ToString() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MDivision2 values ('" + drslevel["Level"].ToString() + "')");
            }
        }
        DataTable empstatus = ViewState["VSEmpstatus"] as DataTable;
        foreach (DataRow drstatus in empstatus.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from mempstatus_setup where status like '%" + drstatus["Employeestatus"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into mempstatus_setup(status,action)values('" + drstatus["Employeestatus"].ToString() + "','Default')");
            }
        }
        DataTable citizenship = ViewState["VScitizenship"] as DataTable;
        foreach (DataRow drcitizen in citizenship.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MCitizenship where Citizenship like '%" + drcitizen["Citizenship"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MCitizenship values ('" + drcitizen["Citizenship"].ToString() + "')");
            }
        }
        DataTable relegion = ViewState["VSrelegion"] as DataTable;
        foreach (DataRow drreg in relegion.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MReligion where Religion like '%" + drreg["Relegion"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MReligion values ('" + drreg["Relegion"].ToString() + "')");
            }
        }
        DataTable zipcode = ViewState["VSzipcode"] as DataTable;
        foreach (DataRow dtzip in zipcode.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MZipCode where ZipCode='" + dtzip["Zipcode"].ToString() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MZipCode(ZipCode,Location,Area)values('" + dtzip["Zipcode"].ToString() + "','Cebu City','Cebu')");
            }
        }
        DataTable dept = ViewState["VSDepartment"] as DataTable;
        foreach (DataRow dtdept in dept.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MDepartment where Department like '%" + dtdept["department"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MDepartment(Department)values('" + dtdept["department"].ToString() + "')");
            }
        }
        DataTable sect = ViewState["VSseccode"] as DataTable;
        foreach (DataRow dtsect in sect.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from msection where seccode = '" + dtsect["Section"].ToString() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into msection(seccode,sec_desc)values('" + dtsect["Section"].ToString() + "','" + dtsect["Section"].ToString() + "')");
            }
        }
        DataTable pos = ViewState["VSPosition"] as DataTable;
        foreach (DataRow dtpos in pos.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MPosition where Position = '" + dtpos["Position"].ToString() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MPosition(Position)values('" + dtpos["Position"].ToString() + "')");
            }
        }
        DataTable pgroup = ViewState["VSPGroup"] as DataTable;
        foreach (DataRow dtgroup in pgroup.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MPayrollGroup where PayrollGroup like '%" + dtgroup["payrollgroup"].ToString() + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MPayrollGroup(PayrollGroup,status) values ('" + dtgroup["payrollgroup"].ToString() + "','1');");
            }
        }
        DataTable comps = ViewState["VSCompany"] as DataTable;
        foreach (DataRow dtcomps in comps.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MCompany where Company = '" + dtcomps["Company"].ToString() + "'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MCompany(Company,Address,SSSNumber,PHICNumber,HDMFNumber,TIN,EntryUserId,EntryDateTime,gdr_roles,ob_roles)values('" + dtcomps["Company"].ToString() + "','Default','0','0','0','0','1',GETDATE(),'1','1')");
            }
        }
        DataTable bra = ViewState["VSBranch"] as DataTable;
        foreach (DataRow drbra in bra.Rows)
        {
            DataTable dt = dbhelper.getdata("select * from MBranch where Branch like '%" + drbra["Branch"] + "%'");
            if (dt.Rows.Count == 0)
            {
                dbhelper.getdata("insert into MBranch values('" + drbra["Branch"].ToString() + "','1')");
            }
        }

        Button2.Visible = true;
        Button4.Visible = false;
        GridView1.Visible = true;
        
        gridcompanydistinct.Visible = false;
        gridbranch.Visible = false;
        gridposition.Visible = false;
        //gridshiftcode.Visible = false;
        griddepartment.Visible = false;
        gridggroup.Visible = false;
        gridsction.Visible = false;
    }
    protected void BindGrid()
    {
        GridView1.DataSource = ViewState["test"] as DataTable;
        GridView1.DataBind();
    }

    protected void loadtemplate(object sender, EventArgs e)
    {
        DataTable dtcheck = ViewState["test"] as DataTable;

        foreach (DataRow dr in dtcheck.Rows)
        {
            stateclass a = new stateclass();
            a._idnumber = dr["IdNumber"].ToString().Length > 0 ? dr["IdNumber"].ToString() : "0000-000";
            a._badgenumber = dr["SAPNumber"].ToString().Length > 0 ? dr["SAPNumber"].ToString() : "000-0000";
            a._lastname = dr["Lastname"].ToString().Length > 0 ? dr["Lastname"].ToString() : a._firstname;
            a._firstname = dr["Firstname"].ToString().Length > 0 ? dr["Firstname"].ToString() : a._lastname;
            a._middlename = dr["Middlename"].ToString().Length > 0 ? dr["Middlename"].ToString() : "";
            a._extensionname = dr["Extensionname"].ToString().Length > 0 ? dr["Extensionname"].ToString() : "";
            a._presentaddress = dr["Presentaddress"].ToString().Length > 0 ? dr["Presentaddress"].ToString() : "";
            a._permanentaddress = dr["Permanentaddress"].ToString().Length > 0 ? dr["Permanentaddress"].ToString() : "";
            DataTable dt_zipcode = dbhelper.getdata("select * from MZipCode where ZipCode='" + dr["Zipcode"] + "'");
            a._zipcode = dt_zipcode.Rows.Count > 0 ? dt_zipcode.Rows[0]["Id"].ToString() : "0";
            a._phonenumber = dr["Phonenumber"].ToString().Length > 0 ? dr["Phonenumber"].ToString() : "000-0000";
            a._cellphonenumber = dr["Cellphonenumber"].ToString().Length > 0 ? dr["Cellphonenumber"].ToString() : "000-0000-000";
            a._emailaddress = dr["Emailaddress"].ToString().Length > 0 ? dr["Emailaddress"].ToString() : "";
            a._dateofbirth = dr["Dateofbirth"].ToString().Length > 0 ? dr["Dateofbirth"].ToString() : "";
            a._placeofbirth = dr["Placeofbirth"].ToString().Length > 0 ? dr["Placeofbirth"].ToString() : "";
            DataTable dt_bzipcode = dbhelper.getdata("select * from MZipCode where ZipCode='" + dr["Birthzipcode"] + "'");
            a._zipcode = dt_bzipcode.Rows.Count > 0 ? dt_bzipcode.Rows[0]["Id"].ToString() : "0";
            a._datehired = dr["Datehired"].ToString().Length > 0 ? dr["Datehired"].ToString() : "";
            a._dateresigned = dr["Dateresigend"].ToString().Length > 0 ? dr["Dateresigend"].ToString() : "";
            a._sex = dr["Gender"].ToString().Length > 0 ? dr["Gender"].ToString() : "";
            DataTable dt_cvel = dbhelper.getdata("select * from MCivilStatus where CivilStatus like '%" + dr["Civilstatus"] + "%'");
            a._civilstatus = dt_cvel.Rows.Count > 0 ? dt_cvel.Rows[0]["Id"].ToString() : "0";
            DataTable dt_Citizenship = dbhelper.getdata("select * from MCitizenship where Citizenship='" + dr["Citizenship"] + "'");
            a._citizenship = dt_Citizenship.Rows.Count > 0 ? dt_Citizenship.Rows[0]["id"].ToString() : "0";
            DataTable dt_relegion = dbhelper.getdata("select * from MReligion where Religion='" + dr["Relegion"] + "'");
            a._religion = dt_relegion.Rows.Count > 0 ? dt_relegion.Rows[0]["id"].ToString() : "0";
            a._height = dr["Height"].ToString().Length > 0 ? dr["Height"].ToString().Replace("'", "") : "0";
            a._weight = dr["Weight"].ToString().Length > 0 ? dr["Weight"].ToString() : "0";
            DataTable dt_bood = dbhelper.getdata("select * from Mbloodtype where bloodtype = '" + dr["BloodType"] + "'");
            a._bloodtype = dt_bood.Rows.Count > 0 ? dt_bood.Rows[0]["id"].ToString() : "0";
            a._gsisnumber = dr["GSISnumber"].ToString().Length > 0 ? dr["GSISnumber"].ToString() : "0";
            a._sssnumber = dr["SSSnumber"].ToString().Length > 0 ? dr["SSSnumber"].ToString() : "0";
            a._hdmfnumber = dr["HDMFnumber"].ToString().Length > 0 ? dr["HDMFnumber"].ToString() : "0";
            a._phicnumber = dr["PHICnumber"].ToString().Length > 0 ? dr["PHICnumber"].ToString() : "0";
            a._tin = dr["TINnumber"].ToString().ToString().Length > 0 ? dr["TINnumber"].ToString() : "0";
            DataTable dt_taxcode = dbhelper.getdata("select * from MTaxCode where TaxCode = '" + dr["Taxcode"] + "'");
            a._taxcode = dt_taxcode.Rows.Count > 0 ? dt_taxcode.Rows[0]["Id"].ToString() : "1";
            a._atmaccountnumber = dr["Accountnumber"].ToString().Length > 0 ? dr["Accountnumber"].ToString() : "0";
            DataTable dt_company = dbhelper.getdata("select * from Mcompany where Company='" + dr["Company"] + "'");
            a._company = dt_company.Rows.Count > 0 ? dt_company.Rows[0]["Id"].ToString() : "1";
            DataTable dt_branch = dbhelper.getdata("select * from Mbranch where Branch='" + dr["Branch"] + "'");
            a._branch = dt_branch.Rows.Count > 0 ? dt_branch.Rows[0]["id"].ToString() : "0";
            DataTable dt_dept = dbhelper.getdata("select * from MDepartment where Department='" + dr["Department"] + "'");
            a._department = dt_dept.Rows.Count > 0 ? dt_dept.Rows[0]["Id"].ToString() : "0";
            DataTable dt_section = dbhelper.getdata("select * From msection where seccode = '" + dr["Section"] + "'");
            a._section = dt_section.Rows.Count > 0 ? dt_section.Rows[0]["sectionid"].ToString() : "0";
            DataTable dt_pos = dbhelper.getdata("select * from MPosition where Position ='" + dr["Position"] + "'");
            a._position = dt_pos.Rows.Count > 0 ? dt_pos.Rows[0]["Id"].ToString() : "0";
            DataTable dt_divs = dbhelper.getdata("select * from MDivision where Division = '" + dr["Division/BandLevel"] + "'");
            a._division = dt_divs.Rows.Count > 0 ? dt_divs.Rows[0]["Id"].ToString() : "3";
            DataTable dt_payrollgrp = dbhelper.getdata("select * from MPayrollGroup where PayrollGroup = '" + dr["Payrollgroup"] + "'");
            a._payrollgroup = dt_payrollgrp.Rows.Count > 0 ? dt_payrollgrp.Rows[0]["Id"].ToString() : "1";
            DataTable dt_glaccount = dbhelper.getdata("select * from MAccount where AccountCode = '" + dr["ChartOfAccount"] + "'");
            a._glaccount = dt_glaccount.Rows.Count > 0 ? dt_glaccount.Rows[0]["Id"].ToString() : "2";
            DataTable dt_payrolltype = dbhelper.getdata("select * from MPayrollType where PayrollType='" + dr["Payrolltype"] + "'");
            a._payrolltype = dt_payrolltype.Rows.Count > 0 ? dt_payrolltype.Rows[0]["id"].ToString() : "1";
            DataTable dt_shift = dbhelper.getdata("select * from MShiftCode where ShiftCode ='" + dr["Shiftcode"] + "'");
            a._shiftcode = dt_shift.Rows.Count > 0 ? dt_shift.Rows[0]["Id"].ToString() : "1";
            a._fixnoofdays = dr["FixNoofDays"].ToString().Length > 0 ? dr["FixNoofDays"].ToString() : "0";
            a._fixnoofhours = dr["FixNoofHours"].ToString().Length > 0 ? dr["FixNoofHours"].ToString() : "0";
            a._monthlyrate = dr["MonthlyRate"].ToString().Length > 0 ? dr["MonthlyRate"].ToString() : "0.00";
            a._payrollrate = dr["PayrollRate"].ToString().Length > 0 ? dr["PayrollRate"].ToString() : "0.00";
            a._dailyrate = dr["DailyRate"].ToString().Length > 0 ? dr["DailyRate"].ToString() : "0.00";
            a._hourlyrate = dr["HourlyRate"].ToString().Length > 0 ? dr["HourlyRate"].ToString() : "0.00";
            DataTable dt_taxtable = dbhelper.getdata("select * from taxtable where taxtable ='" + dr["Tax-table"] + "'");
            a._taxtable = dt_taxtable.Rows.Count > 0 ? dt_taxtable.Rows[0]["taxtable"].ToString() : "Semi-Monthly";
            DataTable dt_ples = dbhelper.getdata("select * from mempstatus_setup where status = '" + dr["Employeestatus"] + "'");
            a.sbb = dt_ples.Rows.Count > 0 ? dt_ples.Rows[0]["id"].ToString() : "1";
            a._nighthourlyrate = "0.00";
            a.userid = "1";
            a._sssgrosssalary = "true";
            a._isminimum = "false";
            a._nta = dr["NonTaxableAllowance"].ToString().Length > 0 ? dr["NonTaxableAllowance"].ToString() : "0.00";
            a._gmr = dr["GrossMonthlyRate"].ToString().Length > 0 ? dr["GrossMonthlyRate"].ToString() : "0.00";
            a._ntaca = dr["NonTaxableActingCapacityAllowance"].ToString().Length > 0 ? dr["NonTaxableActingCapacityAllowance"].ToString() : "0.00";
            a._ntnsd = dr["NonTaxableNightShiftDifference"].ToString().Length > 0 ? dr["NonTaxableNightShiftDifference"].ToString() : "0.00";
            DataTable dtlevel = dbhelper.getdata("select * from MDivision2 where Division2 = '" + dr["Level"] + "'");
            a._level = dtlevel.Rows.Count > 0 ? dtlevel.Rows[0]["Id"].ToString() : "0";
            DataTable dtinternal = dbhelper.getdata("select * from MInternalOrder where InternalOrder = '" + dr["InternalOrder"] + "'");
            a._internalorder = dtinternal.Rows.Count > 0 ? dtinternal.Rows[0]["Id"].ToString() : "0";

            string val = bol.loadtemplate(a);
        }
        Response.Write("<script>alert('File Upload Successfully!')</script>");
        Response.Redirect("login");
    }
    protected void rowdeleterow(object sender, GridViewDeleteEventArgs e)
    {
        GridView1.DeleteRow(e.RowIndex);
        this.BindGrid();
    }
    protected void rowdeleting(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        DataTable dt = ViewState["test"] as DataTable;
        dt.Rows[row.RowIndex].Delete();
        ViewState["test"] = dt;
        this.BindGrid();
    }
    protected void rowediting(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        this.BindGrid();
    }
    protected void rowupdate(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        string idnum = (row.Cells[1].Controls[0] as TextBox).Text;
        string sapnum = (row.Cells[2].Controls[0] as TextBox).Text;
        string lname = (row.Cells[3].Controls[0] as TextBox).Text;
        string fname = (row.Cells[4].Controls[0] as TextBox).Text;
        string mname = (row.Cells[5].Controls[0] as TextBox).Text;
        string exname = (row.Cells[6].Controls[0] as TextBox).Text;
        string addrss = (row.Cells[7].Controls[0] as TextBox).Text;
        string peradd = (row.Cells[8].Controls[0] as TextBox).Text;
        string pnum = (row.Cells[9].Controls[0] as TextBox).Text;
        string cnum = (row.Cells[10].Controls[0] as TextBox).Text;
        string ead = (row.Cells[11].Controls[0] as TextBox).Text;
        string datebrt = (row.Cells[12].Controls[0] as TextBox).Text;
        string plcebrt = (row.Cells[13].Controls[0] as TextBox).Text;
        string dhired = (row.Cells[14].Controls[0] as TextBox).Text;
        string gender = (row.Cells[15].Controls[0] as TextBox).Text;
        string civilstat = (row.Cells[16].Controls[0] as TextBox).Text;
        string ctizen = (row.Cells[17].Controls[0] as TextBox).Text;
        string releg = (row.Cells[18].Controls[0] as TextBox).Text;
        string hyt = (row.Cells[19].Controls[0] as TextBox).Text;
        string wyt = (row.Cells[20].Controls[0] as TextBox).Text;
        string bloodtype = (row.Cells[21].Controls[0] as TextBox).Text;
        string gsis = (row.Cells[22].Controls[0] as TextBox).Text;
        string sss = (row.Cells[23].Controls[0] as TextBox).Text;
        string hdmfnum = (row.Cells[24].Controls[0] as TextBox).Text;
        string phicnum = (row.Cells[25].Controls[0] as TextBox).Text;
        string tinum = (row.Cells[26].Controls[0] as TextBox).Text;
        string taxcod = (row.Cells[27].Controls[0] as TextBox).Text;
        string accnum = (row.Cells[28].Controls[0] as TextBox).Text;
        string company = (row.Cells[29].Controls[0] as TextBox).Text;
        string branch = (row.Cells[30].Controls[0] as TextBox).Text;
        string department = (row.Cells[31].Controls[0] as TextBox).Text;
        string section = (row.Cells[32].Controls[0] as TextBox).Text;
        string position = (row.Cells[33].Controls[0] as TextBox).Text;
        string blevel = (row.Cells[34].Controls[0] as TextBox).Text;
        string pgroup = (row.Cells[35].Controls[0] as TextBox).Text;
        string payroltype = (row.Cells[36].Controls[0] as TextBox).Text;
        string shiftcode = (row.Cells[37].Controls[0] as TextBox).Text;
        string fixndays = (row.Cells[38].Controls[0] as TextBox).Text;
        string fixnhrs = (row.Cells[39].Controls[0] as TextBox).Text;
        string monrate = (row.Cells[40].Controls[0] as TextBox).Text;
        string payrate = (row.Cells[41].Controls[0] as TextBox).Text;
        string dayrate = (row.Cells[42].Controls[0] as TextBox).Text;
        string hrrate = (row.Cells[43].Controls[0] as TextBox).Text;
        string nti = (row.Cells[44].Controls[0] as TextBox).Text;
        string taxtable = (row.Cells[45].Controls[0] as TextBox).Text;
        string employeestatus = (row.Cells[46].Controls[0] as TextBox).Text;
        string level = (row.Cells[47].Controls[0] as TextBox).Text;
        string internalorder = (row.Cells[48].Controls[0] as TextBox).Text;

        DataTable dt = ViewState["test"] as DataTable;
        dt.Rows[row.RowIndex]["IdNumber"] = idnum;
        dt.Rows[row.RowIndex]["SAPNumber"] = sapnum;
        dt.Rows[row.RowIndex]["LastName"] = lname;
        dt.Rows[row.RowIndex]["FirstName"] = fname;
        dt.Rows[row.RowIndex]["MiddleName"] = mname;
        dt.Rows[row.RowIndex]["ExtensionName"] = exname;
        dt.Rows[row.RowIndex]["Presentaddress"] = addrss;
        dt.Rows[row.RowIndex]["Permanentaddress"] = peradd;
        dt.Rows[row.RowIndex]["PhoneNumber"] = pnum;
        dt.Rows[row.RowIndex]["CellphoneNumber"] = cnum;
        dt.Rows[row.RowIndex]["EmailAddress"] = ead;
        dt.Rows[row.RowIndex]["DateOfBirth"] = datebrt;
        dt.Rows[row.RowIndex]["PlaceOfBirth"] = plcebrt;
        dt.Rows[row.RowIndex]["DateHired"] = dhired;
        dt.Rows[row.RowIndex]["Gender"] = gender;
        dt.Rows[row.RowIndex]["Civilstatus"] = civilstat;
        dt.Rows[row.RowIndex]["Citizenship"] = ctizen;
        dt.Rows[row.RowIndex]["Relegion"] = releg;
        dt.Rows[row.RowIndex]["Height"] = hyt;
        dt.Rows[row.RowIndex]["Weight"] = wyt;
        dt.Rows[row.RowIndex]["bloodtype"] = bloodtype;
        dt.Rows[row.RowIndex]["GSISNumber"] = gsis;
        dt.Rows[row.RowIndex]["SSSNumber"] = sss;
        dt.Rows[row.RowIndex]["HDMFNumber"] = hdmfnum;
        dt.Rows[row.RowIndex]["PHICNumber"] = phicnum;
        dt.Rows[row.RowIndex]["TINnumber"] = tinum;
        dt.Rows[row.RowIndex]["Taxcode"] = taxcod;
        dt.Rows[row.RowIndex]["Accountnumber"] = accnum;
        dt.Rows[row.RowIndex]["Company"] = company;
        dt.Rows[row.RowIndex]["Branch"] = branch;
        dt.Rows[row.RowIndex]["Department"] = department;
        dt.Rows[row.RowIndex]["Section"] = section;
        dt.Rows[row.RowIndex]["Position"] = position;
        dt.Rows[row.RowIndex]["Division/BandLevel"] = blevel;
        dt.Rows[row.RowIndex]["Payrollgroup"] = pgroup;
        dt.Rows[row.RowIndex]["Payrolltype"] = payroltype;
        dt.Rows[row.RowIndex]["Shiftcode"] = shiftcode;
        dt.Rows[row.RowIndex]["FixNoofDays"] = fixndays;
        dt.Rows[row.RowIndex]["FixNoofHours"] = fixnhrs;
        dt.Rows[row.RowIndex]["MonthlyRate"] = monrate;
        dt.Rows[row.RowIndex]["PayrollRate"] = payrate;
        dt.Rows[row.RowIndex]["DailyRate"] = dayrate;
        dt.Rows[row.RowIndex]["HourlyRate"] = hrrate;
        dt.Rows[row.RowIndex]["NTI"] = nti;
        dt.Rows[row.RowIndex]["Tax-table"] = taxtable;
        dt.Rows[row.RowIndex]["Employeestatus"] = employeestatus;
        dt.Rows[row.RowIndex]["Level"] = level;
        dt.Rows[row.RowIndex]["InternalOrder"] = internalorder;

        ViewState["test"] = dt;
        GridView1.EditIndex = -1;
        this.BindGrid();
        galamay(dt);
    }
    protected void rowcancel(object sender, EventArgs e)
    {
        GridView1.EditIndex = -1;
        this.BindGrid();
    }
}
