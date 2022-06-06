using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;

public partial class content_Employee_offsetlist : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session.Count == 0)
            Response.Redirect("quit?key=out",true);
        if (!IsPostBack)
            disp();
    }
    protected void disp()
    {

        string query = "create table #dummy(rowcnt int,offsetid int)  " +
"create table #finaldummy(offsetid int,date_from varchar(max))  " +
"create table #finaldummy1(offsetid int,date_from varchar(max)) " +
"declare @date_from VARCHAR(max) " +
"declare @cnt int " +
"declare @i int " +
"declare @offsetid int " +
"set @i=1 " +
"insert into #dummy (rowcnt,offsetid) " +
"select (ROW_NUMBER() OVER(ORDER BY id asc)),id from toffset where (status  not like '%cancel%' and status  not like '%delet%') and empid=" + Session["emp_id"] + " " +
"set @cnt=(select COUNT(*) from #dummy )  " +
"while(@i<=@cnt) " +
"begin " +
"select @offsetid=offsetid from #dummy where rowcnt=@i " +
"select @date_from= COALESCE(@date_from + ', ', '') + left(convert(varchar,date_from,101),10) from Toffset_date where offset_id=@offsetid " +
"insert into #finaldummy (offsetid,date_from)values(@offsetid,@date_from) " +
"set @date_from=NULL " +
"set @i=@i+1 " +
"end " +
"select id,Date_Input,status,empid, case when status='OT Offset' then date_from else LEFT(convert(varchar,appliedfrom,101),10)end DateFrom, case when status='OT Offset' then LEFT(convert(varchar,appliedfrom,101),10) else LEFT(convert(varchar,appliedto,101),10)  end DateTo, remarks,transtatus from " +
"( " +
"select z.id,lEFT(convert(varchar,z.date,101),10)Date_Input, " +
"case when z.appliedto IS null then 'OT Offset' else 'RD/HD Offset' end status,  " +
"z.appliedfrom,z.appliedto,z.remarks,z.empid,y.date_from,z.status transtatus  " +
"from toffset z " +
"left join #finaldummy y on y.offsetid=z.id " +
"where empid=" + Session["emp_id"] + " and (status  not like '%cancel%' and status  not like '%delet%') " +
")tt " +

"drop table #dummy  " +
"drop table #finaldummy " +
"drop table #finaldummy1 ";
        DataTable dt = dbhelper.getdata(query);
        grid_view.DataSource = dt;
        grid_view.DataBind();
        
    }
   
    private DataTable convertStringToDataTable(string xmlString)
    {
        DataSet dataSet = new DataSet();
        StringReader stringReader = new StringReader(xmlString);
        dataSet.ReadXml(stringReader);
        return dataSet.Tables[0];
    }
}