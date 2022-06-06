
<%@ Application Language="C#" %>
<%@ Import Namespace="System.Web.Routing" %>
<%@ Import Namespace="System.Web.Globalization" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
        Application["UsersLoggedIn"] = new System.Collections.Generic.List<string>();
        RegisterRoutes(RouteTable.Routes);
    }
    
    void RegisterRoutes(RouteCollection routes)
    {
    
        routes.MapPageRoute("", "AdjustUT", "~/content/Admin/AdjustUndertime.aspx");
        /**Printable**/
        routes.MapPageRoute("", "otherpyment", "~/content/printable/payslip_others.aspx");
    
         /**OT MEAL**/
         
        routes.MapPageRoute("", "otmeal", "~/content/Employee/OTMeal.aspx");
        routes.MapPageRoute("", "mealallowance", "~/content/Manager/approve_otmeal.aspx");
        routes.MapPageRoute("", "vmla", "~/content/hr/verify_meal.aspx");
        
         /**Recruitment**/
        routes.MapPageRoute("", "appl", "~/content/Manager/Recruitment/applicants.aspx");
        routes.MapPageRoute("", "MP", "~/content/Manager/Recruitment/ManPower.aspx");
        routes.MapPageRoute("", "VMP", "~/content/Manager/Recruitment/ViewMonPower.aspx");
        routes.MapPageRoute("", "MPHR", "~/content/Manager/Recruitment/ManPowerHR.aspx");
        routes.MapPageRoute("", "FDLR", "~/content/Manager/Recruitment/filedownloader.aspx");
        
        /**massupload**/
        routes.MapPageRoute("", "upload", "~/addnewemployee.aspx");
        
        /**Overriding**/
        routes.MapPageRoute("", "empdtre", "~/content/Admin/EmpDTR.aspx");
        
        /**Monitoring**/
        routes.MapPageRoute("", "obt", "~/content/report/monitoring/officialbusinesstrip.aspx");
        routes.MapPageRoute("", "overtime", "~/content/report/monitoring/overtimelist.aspx");
        routes.MapPageRoute("", "undertime", "~/content/report/monitoring/undertimelist.aspx");
        routes.MapPageRoute("", "leave", "~/content/report/monitoring/leavelist.aspx");
        routes.MapPageRoute("", "workverification", "~/content/report/monitoring/workverification.aspx");
        routes.MapPageRoute("", "changeshift", "~/content/report/monitoring/changeshift.aspx");
        routes.MapPageRoute("", "manual", "~/content/report/monitoring/timeadjustment.aspx");
        routes.MapPageRoute("", "otmealemp", "~/content/report/monitoring/otmeal.aspx");
        
        routes.MapPageRoute("", "setup", "~/content/Admin/SetUp.aspx");
        
        routes.MapPageRoute("", "approverscheck", "~/content/report/WithoutApprovers.aspx");
        routes.MapPageRoute("", "approvers_list", "~/content/report/approvers_list.aspx");
        routes.MapPageRoute("", "daraccess", "~/content/Admin/DARAccess.aspx");
        routes.MapPageRoute("", "homepage", "~/Home2.aspx");
        routes.MapPageRoute("", "homepage2", "~/Home.aspx");

        routes.MapPageRoute("", "documents", "~/content/Employee/ArchiveEmp.aspx");
        routes.MapPageRoute("", "orgchart", "~/content/Admin/orgChart.aspx");
        
	    routes.MapPageRoute("", "KIOSK_offsetlist", "~/content/Employee/offsetlist.aspx"); 
        routes.MapPageRoute("", "request", "~/content/hr/verifyrequestupdate.aspx");
        routes.MapPageRoute("", "approver-config", "~/content/hr/allow_admin.aspx");
        routes.MapPageRoute("", "emposition", "~/content/Admin/employee_position.aspx");
        
        routes.MapPageRoute("", "oicadjust", "~/content/Admin/oicAdjustment.aspx");
        routes.MapPageRoute("", "head-attendance", "~/content/GM/attsummary.aspx");
        routes.MapPageRoute("", "view-schedule", "~/content/manager/display_schedule.aspx");
        routes.MapPageRoute("", "schedule-approval", "~/content/manager/approve_schedule.aspx");
        routes.MapPageRoute("", "heartist", "~/content/hr/dtr_period_emp.aspx");
        routes.MapPageRoute("", "scheduler", "~/content/hr/dtr_period.aspx");   
        routes.MapPageRoute("", "payrollsum", "~/content/payroll/payrollsummary.aspx");
        routes.MapPageRoute("", "logsss", "~/content/payroll/dtrlist.aspx");

        routes.MapPageRoute("", "attendance-summary", "~/content/GM/attendance.aspx");
        routes.MapPageRoute("", "attendance", "~/content/HOD/attendance.aspx");
        //routes.MapPageRoute("", "attendance", "~/content/Manager/attendance.aspx");
        routes.MapPageRoute("", "leave-monitoring", "~/content/Manager/leavemonitoring.aspx");
        routes.MapPageRoute("", "employee-attlogs", "~/content/Manager/attinout.aspx");
        routes.MapPageRoute("", "h_range", "~/content/Manager/range_h.aspx");
        routes.MapPageRoute("", "h_schedule", "~/content/Manager/schedule_h.aspx");

        routes.MapPageRoute("", "dtrmanagement", "~/content/hr/dtrmanagement.aspx");
        routes.MapPageRoute("", "oledb", "~/temp/OLEDB.aspx");
        routes.MapPageRoute("", "changerate", "~/content/hr/changeMWE.aspx");
        routes.MapPageRoute("", "read-notification", "~/temp/ReadNotification.aspx");
        routes.MapPageRoute("", "notificationlist", "~/content/hr/NotificationList.aspx");
        routes.MapPageRoute("", "attendance-a", "~/content/hr/DTR_att.aspx");
        routes.MapPageRoute("", "att-logs", "~/content/hr/attinout.aspx");
        routes.MapPageRoute("", "att-inout", "~/content/hr/DTR_logs.aspx");
        routes.MapPageRoute("", "sys-importleave", "~/content/nobel/leave_load.aspx");
        routes.MapPageRoute("", "sys-leave", "~/content/nobel/leave.aspx");
        routes.MapPageRoute("", "masterfile", "~/content/hr/masterfile.aspx");
        routes.MapPageRoute("", "access", "~/content/Admin/accessright.aspx");
        routes.MapPageRoute("", "adjustments", "~/content/Admin/adjustments.aspx");
        routes.MapPageRoute("", "dtr_emp", "~/content/Manager/dtr_employee.aspx");
        routes.MapPageRoute("", "ot-request", "~/content/Manager/dtr_employee_clone.aspx");
        routes.MapPageRoute("", "sanction", "~/content/Manager/SanctionManagement.aspx");
        routes.MapPageRoute("", "sendmemo", "~/content/hr/Memoemps.aspx");
         routes.MapPageRoute("", "system_approval", "~/content/Admin/system_approval.aspx");
        
        //admin
        routes.MapPageRoute("", "tkm", "~/content/hr/tk.aspx");
        routes.MapPageRoute("", "KOISK_addLEAVEadminside", "~/content/Employee/Adminsideleave.aspx");
        routes.MapPageRoute("", "reset", "~/content/Admin/resetpass.aspx");

        routes.MapPageRoute("", "vs", "~/content/hr/verify_schedule.aspx");
        
        routes.MapPageRoute("", "cp", "~/content/account/change_password.aspx");
        
        routes.MapPageRoute("", "vo", "~/content/hr/verify_offset.aspx");
        routes.MapPageRoute("", "vu", "~/content/hr/verify_undertime.aspx");
        routes.MapPageRoute("", "vl", "~/content/hr/verify_leave.aspx");
        routes.MapPageRoute("", "vmla", "~/content/hr/verify_meal.aspx");
        routes.MapPageRoute("", "sample", "~/content/manager/default.aspx"); 
        routes.MapPageRoute("", "schedule", "~/content/canteen/category.aspx"); 
        routes.MapPageRoute("", "schedule-viewer", "~/content/canteen/Default.aspx"); 
        routes.MapPageRoute("", "dtrp", "~/content/hr/dtr_period.aspx");
        routes.MapPageRoute("", "vcs", "~/content/hr/verify_changeShift.aspx");
        routes.MapPageRoute("", "scheduler", "~/content/hr/dtr_period.aspx");
        
        routes.MapPageRoute("", "ccs", "~/content/Scheduler/create_schedule.aspx");
        routes.MapPageRoute("", "csl", "~/content/Scheduler/approver_schedule.aspx");
        routes.MapPageRoute("", "esl", "~/content/Scheduler/employee_schedule.aspx");
        
        routes.MapPageRoute("", "acs", "~/content/Manager/approve_shift.aspx");
        
        routes.MapPageRoute("", "csr", "~/content/Employee/change_shift_list.aspx");
        routes.MapPageRoute("", "cs", "~/content/Employee/change_shift.aspx");
        
        
        routes.MapPageRoute("", "sys-account", "~/content/nobel/account.aspx");
        routes.MapPageRoute("", "sys-route", "~/content/nobel/route.aspx");
        routes.MapPageRoute("", "sys-user", "~/content/nobel/user.aspx");
        
        routes.MapPageRoute("", "canteen", "~/content/canteen/cafe/intro.aspx");
        routes.MapPageRoute("", "404", "~/temp/Default.aspx");
        routes.MapPageRoute("", "edit-product", "~/content/canteen/product_edit.aspx");
        routes.MapPageRoute("", "create-product", "~/content/canteen/product_create.aspx");
        routes.MapPageRoute("", "product", "~/content/canteen/product.aspx");
        routes.MapPageRoute("", "create-category", "~/content/canteen/category_create.aspx");
        routes.MapPageRoute("", "category", "~/content/canteen/category.aspx");
        routes.MapPageRoute("", "canteen-dashboard", "~/content/canteen/Default.aspx");
        routes.MapPageRoute("", "serve", "~/content/canteen/serve.aspx");
        routes.MapPageRoute("", "Travel", "~/content/hr/verifyTravel.aspx");
        routes.MapPageRoute("", "exit", "~/content/hr/exit.aspx");
        routes.MapPageRoute("", "employee-dashboard", "~/content/Employee/Default.aspx");
        routes.MapPageRoute("", "dashboard", "~/content/account/admin/dashboard.aspx");
        routes.MapPageRoute("", "profile", "~/content/account/profile.aspx");
        routes.MapPageRoute("", "login", "~/Default.aspx"); 
        routes.MapPageRoute("", "menu", "~/content/menu.aspx");
        routes.MapPageRoute("", "Memo", "~/content/hr/memo.aspx");
        routes.MapPageRoute("", "MemoList", "~/content/hr/memo_list.aspx");
        routes.MapPageRoute("", "KIOSK_travel", "~/content/Employee/travel_list.aspx");
        routes.MapPageRoute("", "KIOSK_addtravel", "~/content/Employee/travel.aspx");
        routes.MapPageRoute("", "KIOSK_memo", "~/content/Employee/memo_list.aspx"); 
	
		routes.MapPageRoute("", "employee-profile", "~/content/Employee/Employee201.aspx");
        routes.MapPageRoute("", "offset", "~/content/Manager/approveoffset.aspx");
        routes.MapPageRoute("", "KOISK_addLOAN", "~/content/Employee/coop.aspx");
        routes.MapPageRoute("", "KOISK_DTR", "~/content/Employee/dtr.aspx");
        routes.MapPageRoute("", "KOISK_addLEAVE", "~/content/Employee/leave.aspx");
	    routes.MapPageRoute("", "KOISK_addempsLEAVE", "~/content/Employee/InbehalfLeave.aspx");
        routes.MapPageRoute("", "KOISK_addMANUAL", "~/content/Employee/manual_login.aspx");
        routes.MapPageRoute("", "KOISK_PAYSLIP", "~/content/Employee/payslip.aspx");
        routes.MapPageRoute("", "KOISK_addRestday", "~/content/Employee/rest_day.aspx");
        routes.MapPageRoute("", "KOISK_Restday", "~/content/Employee/rest_day_list.aspx");
        routes.MapPageRoute("", "KOISK_MA", "~/content/Employee/MealAllowanceList.aspx");
        routes.MapPageRoute("", "KOISK_OT","~/content/Employee/ot_list.aspx");
        routes.MapPageRoute("", "KOISK_addOT", "~/content/Employee/addot.aspx");
        routes.MapPageRoute("", "KOISK_preOT", "~/content/Employee/Overtime.aspx");
        
        routes.MapPageRoute("", "KOISK_MANUAL", "~/content/Employee/manual_login_list.aspx");
        routes.MapPageRoute("", "KOISK_LEAVE", "~/content/Employee/leave_list.aspx");
        routes.MapPageRoute("", "KOISK_LOAN", "~/content/Employee/coop_list.aspx");
        routes.MapPageRoute("", "KIOSK_undertime", "~/content/Employee/undertimeList.aspx");
        routes.MapPageRoute("", "KIOSK_addUndertime", "~/content/Employee/underTime.aspx");
        routes.MapPageRoute("", "KIOSK_Masterfile", "~/content/Employee/RequestUpdate.aspx");
        routes.MapPageRoute("", "forms", "~/content/Employee/downloadform.aspx");
        routes.MapPageRoute("", "kiosk_res", "~/content/Employee/Resignation.aspx");
        routes.MapPageRoute("", "kiosk_comp_res", "~/content/Employee/composeresignation.aspx");
        routes.MapPageRoute("", "cleardet", "~/content/printable/printclear.aspx");


        routes.MapPageRoute("", "applist", "~/content/hr/appraisallist.aspx");
        routes.MapPageRoute("", "Credentials", "~/content/hr/credentials.aspx");
        routes.MapPageRoute("", "addCredentials", "~/content/hr/addCredentials.aspx");
        routes.MapPageRoute("", "MEmployee", "~/content/hr/employeelist.aspx");
        routes.MapPageRoute("", "Msystemtable", "~/content/hr/sytemtables.aspx");
        routes.MapPageRoute("", "Mshiftcode", "~/content/hr/shiftcode.aspx");
        routes.MapPageRoute("", "Mmandatorytable", "~/content/hr/mandatorytable.aspx");
        routes.MapPageRoute("", "Motherdeduction", "~/content/hr/Otherdeduction.aspx");
        routes.MapPageRoute("", "Motherincome", "~/content/hr/Otherincome.aspx");
        routes.MapPageRoute("", "Mdaytype", "~/content/hr/daytypelist.aspx");
        routes.MapPageRoute("", "Mleaveapplication", "~/content/hr/leaveapplicationlist.aspx");
        routes.MapPageRoute("", "Motapplication", "~/content/hr/otapplicationlist.aspx");
        routes.MapPageRoute("", "Mdtrlist", "~/content/hr/listdtr.aspx");
        routes.MapPageRoute("", "Mchangeshiftlist", "~/content/hr/changeshiftlist.aspx");
        routes.MapPageRoute("", "verifyot", "~/content/hr/verifyovertime.aspx");
        routes.MapPageRoute("", "reslistver", "~/content/hr/resignationlist.aspx");
        routes.MapPageRoute("", "dtrrange", "~/content/hr/dtrrangesetup.aspx");
        routes.MapPageRoute("", "vclear", "~/content/hr/verify_clearance.aspx");


        routes.MapPageRoute("", "Mpayotherdeduction", "~/content/hr/PayrollOtherDeduction.aspx");
        routes.MapPageRoute("", "addOtherDeduction", "~/content/hr/addPayrollOtherDeduction.aspx");
     
        //approver
        routes.MapPageRoute("", "al", "~/content/Manager/approve_leave.aspx");
        routes.MapPageRoute("", "al", "~/content/Manager/approve_leave.aspx");
        routes.MapPageRoute("", "ao", "~/content/Manager/approve_ot.aspx");
        routes.MapPageRoute("", "preot", "~/content/Manager/approve_preovertime.aspx");
        routes.MapPageRoute("", "am", "~/content/Manager/approve_manual.aspx");
        routes.MapPageRoute("", "rd", "~/content/Manager/approve_rest.aspx");
        routes.MapPageRoute("", "aut", "~/content/Manager/approve_undertime.aspx");
        routes.MapPageRoute("", "at", "~/content/Manager/approve_travel.aspx");
        routes.MapPageRoute("", "reslist", "~/content/Manager/resignationlist.aspx");
        routes.MapPageRoute("", "clear", "~/content/Manager/exitdoc.aspx");
        
        //set up
        routes.MapPageRoute("", "leavesetup", "~/content/setup/leavesetup.aspx");
        
        //ADD
        routes.MapPageRoute("", "employee", "~/content/hr/addemployee.aspx");
        routes.MapPageRoute("", "addemployee", "~/content/hr/addemployee.aspx");
        routes.MapPageRoute("", "addshiftcode", "~/content/hr/addshiftcode.aspx");
        routes.MapPageRoute("", "adddaytype", "~/content/hr/adddaytype.aspx");
        routes.MapPageRoute("", "addleaveapplication", "~/content/hr/addleaveapplication.aspx");
        routes.MapPageRoute("", "addotapplication", "~/content/hr/addotapplication.aspx");
        routes.MapPageRoute("", "addchangeshift", "~/content/hr/addchangeshift.aspx");
        routes.MapPageRoute("", "addDTR", "~/content/payroll/addDTR.aspx");
        routes.MapPageRoute("", "addproccesspayroll", "~/content/payroll/addproccesspayroll.aspx");
        routes.MapPageRoute("", "adddtrlogs", "~/content/hr/DTRfromBIO.aspx");
        routes.MapPageRoute("", "viewtrlogs", "~/content/hr/DTRfromBIOline.aspx");
        
        routes.MapPageRoute("", "Mmanual","~/content/hr/manual_log.aspx");
        routes.MapPageRoute("", "addManualog","~/content/hr/addmanual.aspx");

        routes.MapPageRoute("", "apl", "~/content/hr/approve_loan.aspx");

        routes.MapPageRoute("", "Mtimeadjust", "~/content/hr/timeAdjustments.aspx");
        
        
        routes.MapPageRoute("", "manageasset", "~/content/hr/assetinv.aspx");
        routes.MapPageRoute("", "assetcat", "~/content/hr/assetcat.aspx");
        routes.MapPageRoute("", "assetassign", "~/content/hr/assetassign.aspx");
        routes.MapPageRoute("", "archive", "~/content/hr/archive.aspx");
        
        //report  
        routes.MapPageRoute("", "trail", "~/content/report/AuditTrail.aspx");
        routes.MapPageRoute("", "replistabsent", "~/content/report/absentlist.aspx");
        routes.MapPageRoute("", "replatelist", "~/content/report/latelist.aspx");
        routes.MapPageRoute("", "repundertimelist", "~/content/report/undertimelist.aspx");
        routes.MapPageRoute("", "otlist", "~/content/report/overtimelist.aspx");
        routes.MapPageRoute("", "leavelist", "~/content/report/leavelist.aspx");
        routes.MapPageRoute("", "changeshifthistory", "~/content/report/changeshiftlist.aspx");
        routes.MapPageRoute("", "demographic", "~/content/report/demographic.aspx");
        
        
        //new report
        routes.MapPageRoute("", "loan_leadger", "~/content/report/New/loan_leadger.aspx");
        
        routes.MapPageRoute("", "att_report", "~/content/report/att_report.aspx");
        routes.MapPageRoute("", "atttendance", "~/content/report/Attendance.aspx");
        //routes.MapPageRoute("", "loan_leadger","~/content/report/loan_leadger.aspx");
        //routes.MapPageRoute("", "print_leadger", "~/content/report/print_leadger.aspx");
        //routes.MapPageRoute("", "sss_rep", "~/content/report/SSSreport.aspx");
        //routes.MapPageRoute("", "phic_rep", "~/content/report/PHICreport.aspx");
        //routes.MapPageRoute("", "hdmf_rep", "~/content/report/HDMFreport.aspx");
        routes.MapPageRoute("", "controlreport", "~/content/report/Control_report.aspx");
        routes.MapPageRoute("", "leave_rep", "~/content/report/leave_credit_report.aspx");
        routes.MapPageRoute("", "wht_rep", "~/content/report/wht_report.aspx");
        
        
        //new Reports
        routes.MapPageRoute("", "sss_rep", "~/content/report/New/SSSreport.aspx");
        routes.MapPageRoute("", "hdmf_rep", "~/content/report/New/HDMFreportnew.aspx");
        routes.MapPageRoute("", "phic_rep", "~/content/report/New/PHICreportnew.aspx");
        routes.MapPageRoute("", "loan_leadger", "~/content/report/New/loan_leadger.aspx");
        routes.MapPageRoute("", "print_leadger", "~/content/report/New/print_leadger.aspx");
        routes.MapPageRoute("", "dtr-summary", "~/content/hr/DTRSummary.aspx");
        routes.MapPageRoute("", "mrritw", "~/content/hr/mrritwc1601-C.aspx");
        
        //LIST
        routes.MapPageRoute("", "Manuallogs", "~/content/hr/manuallog.aspx");
        routes.MapPageRoute("", "Restdayverification", "~/content/hr/workrdhd.aspx");
        
        //edit
        routes.MapPageRoute("", "editemployee", "~/content/hr/editemployee.aspx");
        routes.MapPageRoute("", "editshiftcode", "~/content/hr/editshiftcode.aspx");
        routes.MapPageRoute("", "editdaytype", "~/content/hr/editdaytype.aspx");

        routes.MapPageRoute("", "deduction-applicaiton", "~/content/hr/addloan.aspx");
        routes.MapPageRoute("", "addloan", "~/content/hr/addloan.aspx");
        routes.MapPageRoute("", "addOtherIncome", "~/content/hr/addPayrollOtherIncome.aspx");
        routes.MapPageRoute("", "deduction", "~/content/hr/loan.aspx");
        routes.MapPageRoute("", "Mloan", "~/content/hr/loan.aspx");
        routes.MapPageRoute("", "MotherList", "~/content/hr/PayrollOtherIncome.aspx");
        routes.MapPageRoute("", "editloan", "~/content/hr/editloan.aspx");
        routes.MapPageRoute("", "ttos", "~/content/hr/ttos.aspx");
        routes.MapPageRoute("", "mrritw", "~/content/hr/mrritwc1601-C.aspx");
        
        //payroll
        routes.MapPageRoute("", "procpay", "~/content/payroll/proccesspayroll.aspx");
        routes.MapPageRoute("", "payrolldetails", "~/content/payroll/detailsperpayroll.aspx");
        routes.MapPageRoute("", "dtrdetails", "~/content/payroll/detailsperdtr.aspx");
        routes.MapPageRoute("", "viewpayslipallviewing", "~/content/payroll/payslipviewing.aspx");
        routes.MapPageRoute("", "transotherincome", "~/content/payroll/transotherincome.aspx");
        routes.MapPageRoute("", "transotherdeduction", "~/content/payroll/transotherdeduction.aspx");
        routes.MapPageRoute("", "viewdetailsotherdeduction", "~/content/payroll/otherdeductiondetails.aspx");
        routes.MapPageRoute("", "viewdetailsotherincome", "~/content/payroll/otherincomedetails.aspx");
        routes.MapPageRoute("", "payworksheet", "~/content/payroll/payrollworksheet.aspx");
         routes.MapPageRoute("", "quitclaim", "~/content/payroll/quitclaimrequest.aspx");
        routes.MapPageRoute("", "2316process", "~/content/payroll/2316.aspx");
        routes.MapPageRoute("", "quitclaimcompute", "~/content/payroll/quitclaimcomputetation.aspx");

        routes.MapPageRoute("", "sc", "~/content/payroll/sc.aspx");
        routes.MapPageRoute("", "scdet", "~/content/payroll/scdetails.aspx");
        routes.MapPageRoute("", "scdets", "~/content/payroll/scdet.aspx");


        routes.MapPageRoute("", "tmp", "~/content/payroll/thirteenmp.aspx");
        routes.MapPageRoute("", "tmpdet", "~/content/payroll/thirteendetails.aspx");
        
        
        //printable
        routes.MapPageRoute("", "tmpslip", "~/content/printable/tmpslip.aspx");
        routes.MapPageRoute("", "scslip", "~/content/printable/scpayslip.aspx");
        routes.MapPageRoute("", "printablepayslip", "~/content/printable/payslip.aspx");
        routes.MapPageRoute("", "bdgreq", "~/content/printable/budgetrequest.aspx");
        routes.MapPageRoute("", "requestbudget", "~/content/printable/request4budget.aspx");
        routes.MapPageRoute("", "sevenone", "~/content/printable/sevenone.aspx");
       
        routes.MapPageRoute("", "leaveform", "~/content/printable/leave_form.aspx");
        routes.MapPageRoute("", "travel_form", "~/content/printable/travel_form.aspx"); 
        routes.MapPageRoute("", "prntapp", "~/content/printable/printappraisal.aspx");
        routes.MapPageRoute("", "pdf", "~/content/printable/pdf.aspx");
        
        routes.MapPageRoute("", "Approval", "~/content/hr/Approval.aspx");

  
        
        //realtime monitoring
        routes.MapPageRoute("", "timesheetmonitoring", "~/content/Monitoring/realtimedashboard.aspx");
        
        routes.MapPageRoute("", "quit", "~/signout.aspx");
        routes.MapPageRoute("", "bd", "~/bdaygreetings.aspx");
        routes.MapPageRoute("", "anniversary", "~/anivgreetings.aspx");
        
        //loaddata
        routes.MapPageRoute("", "shiftcode", "~/loadshiftcode.aspx");
        routes.MapPageRoute("", "employee", "~/loademployee.aspx");
        routes.MapPageRoute("", "location", "~/loadlocation.aspx");
        routes.MapPageRoute("", "leavecredits", "~/loadleavecredits.aspx");
        routes.MapPageRoute("", "hmo", "~/loadhmo.aspx");
        routes.MapPageRoute("", "twothree", "~/content/printable/ttospdf.aspx");
        routes.MapPageRoute("", "BIR1601C", "~/content/printable/BIR1601-C.aspx");

        //supervisor
        routes.MapPageRoute("", "appr", "~/content/supervisor/empList.aspx");
        routes.MapPageRoute("", "apprform", "~/content/supervisor/appraisalform.aspx");
        
        routes.MapPageRoute("", "par", "~/content/payroll/PAR.aspx");
        routes.MapPageRoute("", "PayregRptViewer", "~/content/Payroll/PayRegreportviewer.aspx");
        routes.MapPageRoute("", "Payrollnetsummary", "~/content/payroll/payrollnetsummary.aspx");
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started
    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

        string userLoggedIn = Session["UserLoggedIn"] == null ? string.Empty : (string)Session["UserLoggedIn"];
        if (userLoggedIn.Length > 0) {
            System.Collections.Generic.List<string> d = Application["UsersLoggedIn"] 
                as System.Collections.Generic.List<string>;
            if (d != null) {
                lock (d) {
                    d.Remove(userLoggedIn);
                }
            }
        }
    }
       
</script>
