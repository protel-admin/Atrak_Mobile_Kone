using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attendance.Model;
using System.Data.Entity.Migrations;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;


namespace Attendance.Repository
{

    public class Data
    {
        public string GradeId { get; set; }
        public string StaffId { get; set; }
        public string EmailId { get; set; }
        public string StaffName { get; set; }
        public string Department { get; set; }
    };

    public class VisitorPassRepository : IDisposable
    {
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
    {
                handle.Dispose();
            }
            disposed = true;
        }
          private AttendanceManagementContext context = null;
          public VisitorPassRepository()
            {
           
                context = new AttendanceManagementContext();
            }

          public List<VisitAppointment> GetAppointmentList(string StaffId)
          {
              var ConStr = string.Empty;
              StringBuilder QryStr = null;
              List<VisitAppointment> _Data_ = null;
              try
              {
                  ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                  if (string.IsNullOrEmpty(ConStr) == true)
                  {
                      throw new Exception("Connection string is empty in the configuration file.");
                  }
              }
              catch
              {
                  throw new Exception("Connection string is missing from the configuration file.");
              }

              using (SqlConnection _CON_ = new SqlConnection(ConStr))
              {
                  using (SqlCommand _CMD_ = new SqlCommand())
                  {
                      try
                      {
                          QryStr = new StringBuilder();
                          QryStr.Clear();
                          QryStr.Append("SELECT VT.VISITID , VT.SLNO as Id, VI.VisitorName , VI.Mobile as VisitorCellNo , UPPER ( REPLACE ( CONVERT ( VARCHAR , VT.FromDateTime , 106 ) , ' ' , '-' ) ) AS VisitDate, ");
                          QryStr.Append("COMP.VisitorCompanyName as VisitorCompany, BRN.BranchAddress as VisitorAddress ,  ");
                          QryStr.Append("CASE  ");
                          QryStr.Append("WHEN UPPER ( VT.VisitStatus ) = 'PENDING' THEN 'APPROVAL PENDING' ");
                          QryStr.Append("WHEN UPPER ( VT.VisitStatus ) = 'OPEN' THEN 'APPOINTMENT' END as VisitStatus ");
                          QryStr.Append("FROM VISITTRANSACTION VT INNER JOIN VISITORINFO VI ON VT.VISITORID = VI.VisitorID ");
                          QryStr.Append("INNER JOIN VISITORCOMPANY COMP ON VI.VisitorCompanyId = COMP.VisitorCompanyID ");
                          QryStr.Append("INNER JOIN VISITORBRANCH BRN ON VI.VISITORBRANCHID = BRN.VisitorBranchID ");
                          QryStr.Append("WHERE UPPER ( VT.VisitStatus ) IN ('PENDING','OPEN') AND VT.STAFFID = '"+StaffId+"' ");
                          QryStr.Append("ORDER BY VT.VISITID DESC ");

                          _CMD_.CommandText = QryStr.ToString();
                          _CMD_.CommandTimeout = 0;
                          _CMD_.CommandType = System.Data.CommandType.Text;
                          _CMD_.Connection = _CON_;
                          _CON_.Open();

                          SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                          if (_Dr_.HasRows == true)
                          {
                              _Data_ = new List<VisitAppointment>();
                              while(_Dr_.Read())
                              {
                                  _Data_.Add(new VisitAppointment()
                                  {
                                      VisitId = Convert.ToInt16(_Dr_["VisitId"]),
                                      Id = _Dr_["Id"].ToString(),
                                      VisitorName = _Dr_["VisitorName"].ToString(),
                                      VisitorCellNo = _Dr_["VisitorCellNo"].ToString(),
                                      VisitDate = _Dr_["VisitDate"].ToString(),
                                      VisitorCompany = _Dr_["VisitorCompany"].ToString(),
                                      VisitorAddress = _Dr_["VisitorAddress"].ToString(),
                                      VisitStatus = _Dr_["VisitStatus"].ToString()
                                  });
                              }
                          }
                          else
                          {
                              return new List<VisitAppointment>();
                          }
                      }
                      catch
                      {
                          return new List<VisitAppointment>();
                      }
                  }
              }
              return _Data_;
          }

        public string SaveVisitTransaction(VisitAppointment Model)
        {
            string ConStr = string.Empty;
            string QryStr = string.Empty;
            string VisitTxnId = string.Empty;
            string AppointmentId = string.Empty;

            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string has not been configured.");
                }
            }
            catch (Exception)
            {
                throw;
            }

            using(SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using(SqlCommand _CMD_ = new SqlCommand())
                {
                    _CMD_.CommandTimeout = 0;
                    _CMD_.CommandType = System.Data.CommandType.Text;
                    _CMD_.Connection = _CON_;
                    _CON_.Open();



                    using (SqlTransaction _TRANS_ = _CON_.BeginTransaction())
                    {
                        _CMD_.Transaction = _TRANS_;

                        try
                        {
                            SaveCompany(Model, _CMD_);
                            SaveBranch(Model, _CMD_);
                            SaveVisitor(Model, _CMD_);
                            GetVisitTransactionId(Model, _CMD_);

                            if ((Model.MaterialCount > 0 && Model.NeedApproval == true) || Model.IsEmailToBeSent == true)
                            {
                                SendForApproval(Model, _CMD_);
                            }
                            else
                            {
                                Model.IsEmailToBeSent = false;
                                Model.VisitStatus = "open";
                            }
                            //CHECK WHETHER EMAIL HAS TO BE SENT OR NOT.
                            if(Model.IsEmailToBeSent == true ) // IF EMAIL IS NOT SUPPOSED TO BE SENT THEN...
                            {
                                //send email to the reporting manager.
                                SendEmail(Model, _CMD_);
                            }

                            SaveAppointment(Model, _CMD_);
                            SaveMaterials(Model, _CMD_);
                            if(Model.IsEmailToBeSent == true )
                            {
                                //send email to the reporting manager.
                                SaveForApproval(Model, _CMD_); 
                            }
                            _TRANS_.Commit();
                            return Model.Id;
                        }
                        catch (Exception)
                        {
                            _TRANS_.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        public void SaveForApproval(VisitAppointment model, SqlCommand cmd)
        {
            var repo = new CommonRepository();
            try
            {
                repo.SaveIntoApplicationApproval(model.Id, "VA", model.StaffId, model.ReportingManagerId, false);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void SendIntimationEmail(VisitAppointment model, bool Approve)
        {
            string ConStr = string.Empty;
            string QryStr = string.Empty;
            string VisitTxnId = string.Empty;
            string AppointmentId = string.Empty;
            StringBuilder EmailMessage = new StringBuilder();

            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string has not been configured.");
                }
            }
            catch (Exception)
            {
                throw;
            }

            if ( Approve == true)
            {
                EmailMessage.Append(""); 
            }
            else if(Approve == false)
            {

            }

        }

        public void SendEmail(VisitAppointment model, SqlCommand cmd)
        {
            StringBuilder HTMLString = new StringBuilder();
            string Materials = string.Empty;
            string From = string.Empty;
            string To = string.Empty;
            string BaseAddress = string.Empty;

            try
            {
                //try to get the server ip from the web.config file.
                BaseAddress = ConfigurationManager.AppSettings["BASEADDRESS"].ToString();
                //check if the server ip address has been given or not.
                if (string.IsNullOrEmpty(BaseAddress) == true) //if the server ip address has not been given then...
                    //throw exception.
                    throw new Exception("BaseAddress parameter is blank in web.config file.");
            }
            catch (Exception)
            {
                throw;
            }

            try
            {
                HTMLString.Clear();
                HTMLString.Append("<!DOCTYPE html>");
                HTMLString.Append("<html><head></head>");
                HTMLString.Append("	<body leftmargin=\"0\" topmargin=\"0\">");
                HTMLString.Append("		<p>");
                HTMLString.Append("			<div style=\"font-family:arial; font-size:9pt;\">Dear "+model.ReportingManagerName +",</div>");
                HTMLString.Append("		</p>");
                HTMLString.Append("		<p>");
                HTMLString.Append("			<div style=\"font-family:arial; font-size:9pt;\">");
                HTMLString.Append("				A meeting appointment is waiting for your approval. Please refer the appointment details given below.");
                HTMLString.Append("			</div>");
                HTMLString.Append("		</p>");
                HTMLString.Append("		<p>");
                HTMLString.Append("			<table border=\"1\" style=\"width:35%; font-family:arial; font-size:9pt;\">");
                HTMLString.Append("				<tr style=\"font-weight:bold;\"><td colspan=\"3\">Employee details</td></tr>");
                HTMLString.Append("				<tr><td width=\"25%\">Staff Id</td><td width=\"1%\">:</td><td>"+model.StaffId+"</td></tr>");
                HTMLString.Append("				<tr><td>Name</td><td>:</td><td>"+model.StaffName+"</td></tr>");
                HTMLString.Append("				<tr><td>Department</td><td>:</td><td>" + model.Department + "</td></tr>");
                HTMLString.Append("				<tr><td>Phone No</td><td>:</td><td>"+model.CellNo+"</td></tr>");
                HTMLString.Append("				<tr style=\"font-weight:bold;\"><td colspan=\"3\">Appointment Details</td></tr>");
                HTMLString.Append("				<tr><td>Visitor Name</td><td>:</td><td>"+model.VisitorName+"</td></tr>");
                HTMLString.Append("				<tr><td>Phone No</td><td>:</td><td>"+model.VisitorCellNo+"</td></tr>");
                HTMLString.Append("				<tr><td>Company</td><td>:</td><td>"+model.VisitorCompany+"</td></tr>");
                HTMLString.Append("				<tr><td>Address</td><td>:</td><td>"+model.VisitorAddress+"</td></tr>");
                HTMLString.Append("				<tr><td>Date</td><td>:</td><td>"+Convert.ToDateTime(model.VisitDate).ToString("dd-MMM-yyyy HH:mm:ss") +"</td></tr>");

                foreach(var lst in model.PermittedMaterialList)
                {
                    if(lst.Checked == true)
                    {
                        Materials = Materials + lst.PermittedMaterialName + ",";
                    }
                }
                if (string.IsNullOrEmpty(Materials) == false)
                {
                    Materials = Materials.Substring(0,Materials.Length - 1);
                }

                HTMLString.Append("				<tr><td>Materials</td><td>:</td><td>" + Materials + "</td></tr>");
                HTMLString.Append("			</table>");
                HTMLString.Append("		</p>");
                HTMLString.Append("<p>PLEASE NOTE THAT YOU ARE TAKING COMPLETE RESPONSIBILITY OF THE VISITOR WHO IS CARRYING THE MATERIALS ONCE YOU APPROVE THIS APPOINTMENT.</p>");
                HTMLString.Append("		<p>");
                HTMLString.Append("			<a href=\"" + BaseAddress + "VisitorPass/ApproveRejectApplication?ApproverId=" + model.ReportingManagerId + "&ApplicationApprovalId=" + model.Id + "&Approve=true\">Approve</a>&nbsp;|&nbsp;<a href=\"" + BaseAddress + "VisitorPass/ApproveRejectApplication?ApproverId=" + model.ReportingManagerId + "&ApplicationApprovalId=" + model.Id + "&Approve=false\">Reject</a>");
                HTMLString.Append("		</p>");
                HTMLString.Append("	</body>");
                HTMLString.Append("</html>");

                CommonRepository repo = new CommonRepository();
                repo.SendEmailMessage(model.StaffEmailId, model.ReportingManagerEmailId, "", "", "Visitor Management System - Meeting Approval.", HTMLString.ToString());

            }
            catch(Exception)
            {
                throw;
            }
        }

        public void SaveCompany(VisitAppointment model, SqlCommand cmd)
        {
            int CompanyId = 0;
            StringBuilder QryStr = new StringBuilder();
            SqlDataReader _dr_ = null;

RE_READ_CODE:
            try
            {
                //fetch the company name from the model.
                //query the VisitorCompanyTable based on the company name.
                QryStr.Clear();
                QryStr.Append("select VisitorCompanyId from visitorcompany where VisitorCompanyName = '" + model.VisitorCompany + "'");
                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;

                _dr_ = cmd.ExecuteReader();
                //check if the record was found.
                if (_dr_.HasRows == true) //if the record was found then...
                {
                    _dr_.Read();
                    //return the company id back to the calling function.
                    CompanyId = Convert.ToInt16(_dr_["VisitorCompanyId"]);
                    model.VisitorCompanyId = CompanyId;
                    return;
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                try
                {
                    _dr_.Close();
                    _dr_.Dispose();
                }
                catch { }
            }

            try
            {
                QryStr.Clear();
                QryStr.Append("insert into visitorcompany ");
                QryStr.Append("( VisitorCompanyName , isHostCompany , WebSite ,  ");
                QryStr.Append("IsActive , CreatedOn , CreatedByUser ) ");
                QryStr.Append("values ");
                QryStr.Append("( '"+model.VisitorCompany+"' , 0 , '-' ,  ");
                QryStr.Append("1 , GetDate() , '-' ) ");

                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                goto RE_READ_CODE;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public void SaveBranch(VisitAppointment model, SqlCommand cmd)
        {
            int BranchID = 0;
            StringBuilder QryStr = new StringBuilder();
            SqlDataReader _dr_ = null;

        RE_READ_CODE:
            try
            {
                //fetch the company name from the model.
                //query the VisitorCompanyTable based on the company name.
                QryStr.Clear();
                QryStr.Append("select VISITORBRANCHID , VISITORBRANCHNAME from visitorbranch WHERE BRANCHADDRESS = '" + model.VisitorAddress + "'");
                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;

                _dr_ = cmd.ExecuteReader();
                //check if the record was found.
                if (_dr_.HasRows == true) //if the record was found then...
                {
                    _dr_.Read();
                    //return the company id back to the calling function.
                    BranchID = Convert.ToInt16(_dr_["VISITORBRANCHID"]);
                    model.VisitorBranchId = BranchID;
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                try
                {
                    _dr_.Close();
                    _dr_.Dispose();
                }
                catch { }
            }

            try
            {
                QryStr.Clear();
                QryStr.Append("insert into visitorbranch ");
                QryStr.Append("( visitorcompanyid , visitorbranchname , branchaddress ,  ");
                QryStr.Append("phone , fax , emailid ,  ");
                QryStr.Append("isactive , city ) ");
                QryStr.Append("values ");
                QryStr.Append("( "+ model.VisitorCompanyId.ToString() +" , 'DEFAULT' , '"+model.VisitorAddress +"' ,  ");
                QryStr.Append("'-' , '-' , '-' ,  ");
                QryStr.Append("1 , 'DEFAULT' ) ");

                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                goto RE_READ_CODE;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveVisitor(VisitAppointment model, SqlCommand cmd)
        {
            int VisitorID = 0;
            StringBuilder QryStr = new StringBuilder();
            SqlDataReader _dr_ = null;

        RE_READ_CODE:
            try
            {
                //fetch the company name from the model.
                //query the VisitorCompanyTable based on the company name.
                QryStr.Clear();
                QryStr.Append("select VisitorID from visitorinfo where Mobile = '"+model.VisitorCellNo+"' and VisitorCompanyId = "+model.VisitorCompanyId +" and VisitorBranchId = "+model.VisitorBranchId);
                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;

                _dr_ = cmd.ExecuteReader();
                //check if the record was found.
                if (_dr_.HasRows == true) //if the record was found then...
                {
                    _dr_.Read();
                    //return the company id back to the calling function.
                    VisitorID = Convert.ToInt16(_dr_["VisitorID"]);
                    model.VisitorId = VisitorID;
                    return;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                try
                {
                    _dr_.Close();
                    _dr_.Dispose();
                }
                catch { }
            }

            try
            {
                QryStr.Clear();
                QryStr.Append("INSERT INTO VISITORINFO ");
                QryStr.Append("( EMPLOYEEID , DOMAIN , VISITORNAME ,  ");
                QryStr.Append("IMAGETRANSACTIONID , VISITORTYPEID , MOBILE ,  ");
                QryStr.Append("ISACTIVE , CREATEDON , CREATEDBYUSER ,  ");
                QryStr.Append("MODIFIEDON , MODIFIEDBYUSER , VISITORCOMPANYID ,  ");
                QryStr.Append("VISITORBRANCHID ) ");
                QryStr.Append("VALUES ");
                QryStr.Append("( '001' , 'DEFAULT' , '"+model.VisitorName+"' ,  ");
                QryStr.Append("111 , "+model.VisitorTypeId +" , '"+model.VisitorCellNo +"' ,  ");
                QryStr.Append("1 , GETDATE() , '-' , ");
                QryStr.Append("GETDATE() , '-' , "+model.VisitorCompanyId +" ,  ");
                QryStr.Append(model.VisitorBranchId+ " ) ");

                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();
                goto RE_READ_CODE;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SendForApproval(VisitAppointment model, SqlCommand cmd)
        {
            StringBuilder QryStr = new StringBuilder();
            string GradeId = string.Empty;
            bool SendForApproval = false;


            
            
            //************************************************************************************************************************
            // FETCH THE GRADE.
            //************************************************************************************************************************
            //get the grade of the person who is raising the meeting appointment.
            QryStr.Clear();
            QryStr.Append("SELECT GradeId , StaffId , Email as EmailId , dbo.fnGetStaffName (StaffId) as StaffName , dbo.fnGetMAsterName(StaffId , 'DP') as DEpartment FROM STAFFOFFICIAL WHERE STAFFID = '" + model.StaffId + "'");

            try
            {
                var data = context.Database.SqlQuery<Data>(QryStr.ToString()).FirstOrDefault();

                if(data == null)
                {
                    throw new Exception("The information of the employee who is raising the ticket is missing. Appointment ticket could not be raised.");
                }
                else
                {
                    model.StaffEmailId = data.EmailId;
                    model.StaffName = data.StaffName;
                    model.Department = data.Department;
                    GradeId = data.GradeId;
                }
            }
            catch (Exception)
            {
                throw;
            }

            //QryStr.Clear();
            //QryStr.Append("SELECT SENDFORAPPROVAL FROM VisitorPassApprovalHierarchy WHERE GRADEID = '" + GradeId + "'");

            //try
            //{
            //    SendForApproval = context.Database.SqlQuery<Boolean>(QryStr.ToString()).FirstOrDefault();
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            //check if the ticket has to be sent for approval or not.
            if (model.IsEmailToBeSent) //if to be sent for approval then...
            {
                model.VisitStatus = "PENDING";
                //get the reporting manager information and the email id.

                try
                {
                    //get the grade of the person who is raising the meeting appointment.
                    QryStr.Clear();
                    QryStr.Append("SELECT GradeId , StaffId , EMAIL " +
                                    "as EmailId , dbo.fnGetStaffName ( StaffId ) as StaffName " +
                                    " , dbo.fnGetMasterName ( StaffId , 'DP' ) as Department FROM STAFFOFFICIAL WHERE STAFFID = '" + model.ReportingManagerId + "'");

                    var data = context.Database.SqlQuery<Data>(QryStr.ToString()).FirstOrDefault();

                    if (data == null)
                    {
                        //throw exception.
                        throw new Exception("The information of the employee who raised the ticket is missing. Appointment ticket could not be raised.");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(data.StaffId))
                        {
                            //throw exception.
                            throw new Exception("The reporting manager of the employee who raised the ticket is missing. Appointment ticket could not be raised.");
                        }
                        else
                        {
                            if (data.StaffId == "-")
                            {
                                //throw exception.
                                throw new Exception("The reporting manager of the employee who raised the ticket is missing. Appointment ticket could not be raised.");
                            }
                        }

                        if (string.IsNullOrEmpty(data.EmailId))
                        {
                            //throw exception.
                            throw new Exception("The reporting manager emailid of the employee who raised the ticket is missing. Appointment ticket could not be raised.");
                        }
                        else
                        {
                            if (data.EmailId == "-")
                            {
                                //throw exception.
                                throw new Exception("The reporting manager emailid of the employee who raised the ticket is missing. Appointment ticket could not be raised.");
                            }
                        }
                        //model.IsEmailToBeSent = SendForApproval;
                        model.ReportingManagerId = data.StaffId;
                        model.ReportingManagerEmailId = data.EmailId;
                        model.ReportingManagerName = data.StaffName;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else //if the ticket is not to be sent for approval then...
            {
                //directly approve the ticket.
                model.VisitStatus = "open";
                model.IsEmailToBeSent = SendForApproval;
            }
        }


        public void SaveAppointment(VisitAppointment model, SqlCommand cmd)
        {
            StringBuilder QryStr = new StringBuilder();
            var MaterialList = new StringBuilder();
            var MaterialName = string.Empty;
            try
            {

                foreach (var l in model.PermittedMaterialList)
                {
                    if (l.Checked == true)
                    {
                        if(MaterialList.Length>0)
                            MaterialList.Append(",");

                        if (l.PermittedMaterialId == 1)
                            MaterialName = "LAPTOP:";
                        else if (l.PermittedMaterialId == 2)
                            MaterialName = "PEN DRIVE:";
                        else if (l.PermittedMaterialId == 3)
                            MaterialName = "MOBILE PHONE:";
                        else if (l.PermittedMaterialId == 4)
                            MaterialName = "CAMERA:";

                        MaterialList.Append(MaterialName);
                    }
                }

                QryStr.Clear();
                QryStr.Append("insert into visittransaction ");
                QryStr.Append("( VisitorId , StaffId , VisitPurposeId ,  ");
                QryStr.Append("VisitorTypeId , WaitLocationId , SLNo ,  ");
                QryStr.Append("VehicleNo , BadgeNo , CellTokenNo ,  ");
                QryStr.Append("FromDateTime , TODateTime ,  VisitStatus ,  ");
                QryStr.Append("CardCode , Materials , AccessAreas ,  ");
                QryStr.Append("CreatedOn , CreatedByUser , SignInDateTime ,  ");
                QryStr.Append("SignInUser , SignInUserName , SignOutDateTime ,  ");
                QryStr.Append("SignOutUser , VisitorCompanyId , AdditionalVisitors) ");
                QryStr.Append("values ");
                QryStr.Append("( "+model.VisitorId +" , '"+ model.StaffId +"' , "+model.PurposeId +" ,  ");
                QryStr.Append( model.VisitorTypeId.ToString() + " , "+ model.WaitLocationId.ToString() +" , '"+ model.Id +"' ,  ");
                QryStr.Append("0 , 0 , 0 ,  ");
                QryStr.Append("'"+ model.VisitDate +"' , '"+ Convert.ToDateTime( model.VisitDate ).AddHours(1).ToString("dd-MMM-yyyy HH:mm:ss") +"' ,  '"+model.VisitStatus+"' ,  ");
                QryStr.Append("'-' , '"+MaterialList.ToString()+"' , '-' ,  ");
                QryStr.Append("GETDATE() , '-' , GETDATE() ,  ");
                QryStr.Append("'-' , '-' , GETDATE() ,  ");
                QryStr.Append("'-' , " + model.VisitorCompanyId + "," + model.AdditionalVisitors + ") ");

                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.ExecuteNonQuery();

                SqlDataReader _dr_  = null;
                QryStr.Clear();
                QryStr.Append("SELECT IDENT_CURRENT('VISITTRANSACTION') as VisitId");

                try
                {
                    cmd.CommandText = QryStr.ToString();
                    cmd.CommandTimeout = 0;
                    cmd.CommandType = System.Data.CommandType.Text;
                    _dr_ = cmd.ExecuteReader();

                    if(_dr_.HasRows ==true)
                    {
                        _dr_.Read();
                        model.VisitId = Convert.ToInt16(_dr_["VisitId"]);
                    }
                }
                catch(Exception)
                {
                    throw;
                }
                finally
                {
                    try
                    {
                        _dr_.Close();
                        _dr_.Dispose();
                    }
                    catch { }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SaveMaterials(VisitAppointment model, SqlCommand cmd)
        {
            StringBuilder QryStr = new StringBuilder();

            try
            {

                foreach (var l in model.PermittedMaterialList)
                {
                    if (l.Checked == true)
                    {
                        QryStr.Clear();
                        QryStr.Append("insert into material ");
                        QryStr.Append("( PermittedMaterialId , serialnumber , visitid )  ");
                        QryStr.Append("values ");
                        QryStr.Append("( '" + l.PermittedMaterialId + "' , '' , " + model.VisitId + " )  ");

                        cmd.CommandText = QryStr.ToString();
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.ExecuteNonQuery();
                    
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void GetVisitTransactionId(VisitAppointment model, SqlCommand cmd)
        {
            StringBuilder QryStr = new StringBuilder();
            SqlDataReader _dr_ = null;
            string VisitTxnId = string.Empty;
            QryStr.Clear();
            QryStr.Append("select 'V'+SUBSTRING ( convert ( varchar , getdate() , 112 ) , 3 , 8 ) + ");
            QryStr.Append("right ( '000' + convert ( nvarchar , convert ( int , right ( isnull ( max ( SLNO ) , 0 ) ,3  ) ) + 1 )  , 3 ) as SLNO  ");
            QryStr.Append("from [VisitTransaction] WHERE CONVERT ( DATETIME , CONVERT ( VARCHAR , CREATEDON , 106 ) )  ");
            QryStr.Append("= CONVERT ( DATETIME , CONVERT ( VARCHAR , GETDATE() , 106 ) ) ");

            try
            {
                cmd.CommandText = QryStr.ToString();
                cmd.CommandTimeout = 0;
                cmd.CommandType = System.Data.CommandType.Text;

                _dr_ = cmd.ExecuteReader();

                if(_dr_.HasRows == true)
                {
                    _dr_.Read();
                    VisitTxnId = _dr_["SLNO"].ToString();
                    model.Id = VisitTxnId;
                }
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                try
                {
                    _dr_.Close();
                    _dr_.Dispose();
                }
                catch { }
            }
        }

        public VisitorDetails GetVisitorDetails(string PhoneNumber)
        {
            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            VisitorDetails _Data_ = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string is not configured.");
                }
            }
            catch(Exception err)
            {
                throw new Exception(err.Message);
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("SELECT			VI.Mobile , VI.VisitorName , VC.VisitorCompanyName , BranchAddress ");
                        QryStr.Append("FROM			    VISITORINFO VI ");
                        QryStr.Append("INNER JOIN		VISITORCOMPANY VC ON VI.VisitorCompanyId = VC.VisitorCompanyID ");
                        QryStr.Append("INNER JOIN		VISITORBRANCH VB ON VI.VisitorBranchID = VB.VisitorBranchID ");
                        QryStr.Append("WHERE			VI.MOBILE = '" + PhoneNumber + "' or VI.EmailId = '" + PhoneNumber + "' ");

                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                        if (_Dr_.HasRows == true)
                        {
                            _Dr_.Read();
                            _Data_ = new VisitorDetails() { 
                                Mobile = _Dr_["Mobile"].ToString(),
                                VisitorName = _Dr_["VisitorName"].ToString(),
                                VisitorCompanyName = _Dr_["VisitorCompanyName"].ToString(),
                                BranchAddress = _Dr_["BranchAddress"].ToString()
                            };
                        }
                        else
                        {
                            throw new Exception("0 record");
                        }
                    }
                    catch(Exception err)
                    {
                        throw new Exception(err.Message);
                    }
                }
            }
            return _Data_;            
        }

        public void CancelVisitorPass(string Id)
        {
            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string is empty in the configuration file.");
                }
            }
            catch
            {
                throw new Exception("Connection settings is missing from the configuration file.");
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("UPDATE VISITTRANSACTION SET VISITSTATUS = 'cancel' WHERE SLNO = '" + Id + "' ");


                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        _CMD_.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public List<VisitPurposeList> GetVisitPurposeList()
        {
            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            List<VisitPurposeList> _Lst_ = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    return new List<VisitPurposeList>();
                }
            }
            catch
            {
                return new List<VisitPurposeList>();
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("SELECT [VisitPurposeID],[Description] FROM VisitPurpose");

                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                        if (_Dr_.HasRows == true)
                        {
                            _Lst_ = new List<Model.VisitPurposeList>();
                            while (_Dr_.Read())
                            {
                                _Lst_.Add(new VisitPurposeList()
                                {
                                    VisitPurposeID = Convert.ToInt16(_Dr_["VisitPurposeID"].ToString()),
                                    Description = _Dr_["Description"].ToString()
                                });
                            }
                        }
                        else
                        {
                            return new List<Model.VisitPurposeList>();
                        }
                    }
                    catch
                    {
                        return new List<Model.VisitPurposeList>();
                    }
                }
            }
            return _Lst_;
        }


        public List<VisitTypeList> GetVisitorTypeList()
        {
            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            List<VisitTypeList> _Lst_ = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    return new List<VisitTypeList>();
                }
            }
            catch
            {
                return new List<VisitTypeList>();
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("SELECT [VisitorTypeID] , [Description] FROM [VisitorType]");

                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                        if (_Dr_.HasRows == true)
                        {
                            _Lst_ = new List<Model.VisitTypeList>();
                            while (_Dr_.Read())
                            {
                                _Lst_.Add(new VisitTypeList()
                                {
                                    VisitorTypeID = Convert.ToInt16(_Dr_["VisitorTypeID"].ToString()),
                                    Description = _Dr_["Description"].ToString()
                                });
                            }
                        }
                        else
                        {
                            return new List<Model.VisitTypeList>();
                        }
                    }
                    catch
                    {
                        return new List<Model.VisitTypeList>();
                    }
                }
            }
            return _Lst_;
        }


        public List<VisitingAreaList> GetVisitingAreaList()
        {

            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            List<VisitingAreaList> _Lst_ = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    return new List<VisitingAreaList>();
                }
            }
            catch
            {
                return new List<VisitingAreaList>();
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("SELECT [WaitLocationID],[Description] FROM [WaitLocation]");

                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                        if (_Dr_.HasRows == true)
                        {
                            _Lst_ = new List<Model.VisitingAreaList>();
                            while (_Dr_.Read())
                            {
                                _Lst_.Add(new VisitingAreaList()
                                {
                                    WaitLocationID = Convert.ToInt16(_Dr_["WaitLocationID"].ToString()),
                                    Description = _Dr_["Description"].ToString()
                                });
                            }
                        }
                        else
                        {
                            return new List<Model.VisitingAreaList>();
                        }
                    }
                    catch
                    {
                        return new List<Model.VisitingAreaList>();
                    }
                }
            }
            return _Lst_;
        }

        public StaffView GetStaffDetails(string Id, string IdType)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();

            if(IdType.Trim() == "domain")
            {
                QryStr.Append(" ");
                QryStr.Append(" SELECT StaffId , dbo.fnGetStaffName(staffid) AS StaffName , CompanyId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'C') AS CompanyName ,  ");
                QryStr.Append(" BranchId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'BR') AS BranchName ,  ");
                QryStr.Append(" DepartmentId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'DP') AS DepartmentName ,  ");
                QryStr.Append(" PHONE AS OfficialPhone ,  ");
                QryStr.Append(" SendForApproval ,  ");
                QryStr.Append(" DomainId ");
                QryStr.Append(" FROM staffofficial A INNER JOIN VisitorPassApprovalHierarchy B ");
                QryStr.Append(" ON A.GradeId = B.GradeId ");
                QryStr.Append(" WHERE DOMAINID = '" + Id + "' ");
            }
            else if(IdType.Trim() == "staffid")
            {
                QryStr.Append(" SELECT StaffId , dbo.fnGetStaffName(staffid) AS StaffName , CompanyId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'C') AS CompanyName ,  ");
                QryStr.Append(" BranchId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'BR') AS BranchName ,  ");
                QryStr.Append(" DepartmentId ,  ");
                QryStr.Append(" DBO.fnGetMasterName(STAFFID , 'DP') AS DepartmentName ,  ");
                QryStr.Append(" PHONE AS OfficialPhone ,  ");
                QryStr.Append(" SendForApproval ,  ");
                QryStr.Append(" DomainId ");
                QryStr.Append(" FROM staffofficial A INNER JOIN VisitorPassApprovalHierarchy B ");
                QryStr.Append(" ON A.GradeId = B.GradeId ");
                QryStr.Append(" WHERE STAFFID = '" + Id + "' ");
            }

            try
            {
                var data = context.Database.SqlQuery<StaffView>(QryStr.ToString()).Select(d => new StaffView()
                {
                    StaffId = d.StaffId,
                    StaffName = d.StaffName,
                    CompanyId = d.CompanyId,
                    CompanyName = d.CompanyName,
                    BranchId = d.BranchId,
                    BranchName = d.BranchName,
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName,
                    OfficialPhone = d.OfficialPhone,
                    SendForApproval = d.SendForApproval
                }).FirstOrDefault();

                if(data == null)
                {
                    throw new Exception("You do not have privileges to raise appointment ticket.");
                }
                return data;

            }
            catch(Exception)
            {
                throw;
            }
        }

        public List<DeptWiseReportingManagers>GetReportingManagersOfDepartment(string DepartmentId)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select ReportingManagerId , dbo.fnGetStaffName( ReportingManagerId ) ReportingManagerName "+
                "from TeamHierarchy where DepartmentId = '"+DepartmentId+"'");

            try
            {
                var lst = context.Database.SqlQuery<DeptWiseReportingManagers>(QryStr.ToString()).Select(d => new DeptWiseReportingManagers()
                {
                    ReportingManagerId = d.ReportingManagerId,
                    ReportingManagerName = d.ReportingManagerName
                }).ToList();

                if(lst == null)
                {
                    return new List<DeptWiseReportingManagers>();
                }
                else
                {
                    return lst;
                }
            }
            catch(Exception)
            {
                return new List<DeptWiseReportingManagers>();
            }

        }


        public List<PermittedMaterialList> GetPermittedMaterialList()
        {

            var ConStr = string.Empty;
            StringBuilder QryStr = null;
            List<PermittedMaterialList> _Lst_ = null;
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    return new List<PermittedMaterialList>();
                }
            }
            catch
            {
                return new List<PermittedMaterialList>();
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr = new StringBuilder();
                        QryStr.Clear();
                        QryStr.Append("select PermittedMaterialId , PermittedMaterialName from permittedmaterial WHERE ISACTIVE = 1");

                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _Dr_ = _CMD_.ExecuteReader();

                        if (_Dr_.HasRows == true)
                        {
                            _Lst_ = new List<Model.PermittedMaterialList>();
                            while (_Dr_.Read())
                            {
                                _Lst_.Add(new PermittedMaterialList()
                                {
                                    PermittedMaterialId = Convert.ToInt16(_Dr_["PermittedMaterialId"].ToString()),
                                    PermittedMaterialName = _Dr_["PermittedMaterialName"].ToString()
                                });
                            }
                        }
                        else
                        {
                            return new List<Model.PermittedMaterialList>();
                        }
                    }
                    catch
                    {
                        return new List<Model.PermittedMaterialList>();
                    }
                }
            }
            return _Lst_;
        }

        public void GetVisitorPassDetails(VisitAppointment model, string SlNo)
        {
            var ConStr = string.Empty;
            StringBuilder QryStr = new StringBuilder();
            List<PermittedMaterialList> _Lst_ = null;
            var repo = new CommonRepository ();
            
            try
            {
                ConStr = ConfigurationManager.ConnectionStrings["ConVizitas"].ToString();

                if (string.IsNullOrEmpty(ConStr) == true)
                {
                    throw new Exception("Connection string for Visitor Management has not been configured.");
                }
            }
            catch(Exception)
            {
                throw;
            }

            using (SqlConnection _CON_ = new SqlConnection(ConStr))
            {
                using (SqlCommand _CMD_ = new SqlCommand())
                {
                    try
                    {
                        QryStr.Clear();
                        QryStr.Append("SELECT * FROM VISITTRANSACTION A INNER JOIN VISITORINFO C ON A.VISITORID = C.VisitorID LEFT JOIN STAFF B ON A.StaffID = B.StaffID WHERE SLNO = '" + SlNo + "'");
                        _CMD_.CommandText = QryStr.ToString();
                        _CMD_.CommandTimeout = 0;
                        _CMD_.CommandType = System.Data.CommandType.Text;
                        _CMD_.Connection = _CON_;
                        _CON_.Open();

                        SqlDataReader _DR_ = _CMD_.ExecuteReader();

                        if(_DR_.HasRows)
                        {
                            _DR_.Read();
                            model.StaffId = _DR_["StaffId"].ToString();
                            model.StaffName = _DR_["StaffName"].ToString();
                            model.VisitDate =Convert.ToDateTime(_DR_["FromDateTime"].ToString()).ToString("dd-MMM-yyyy HH:mm:ss");
                            model.Id = _DR_["SLNO"].ToString();
                            model.VisitorName = _DR_["VisitorName"].ToString();
                            model.VisitorCellNo = _DR_["Mobile"].ToString();
                            model.StaffEmailId = _DR_["Email"].ToString();
                            model.ReportingManagerEmailId = repo.GetEmailIdOfEmployee(repo.GetReportingManager(model.StaffId));
                        }
                        else
                        {
                            throw new Exception("No Record found.");
                        }
                    }
                    catch(Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public CellNo GetCellNo(string staffid)
        {
            var QryStr = new StringBuilder();
            QryStr.Clear();
            QryStr.Append("select Phone  from staffofficial WHERE STAFFID = '" + staffid + "'");

            try
            {
                var si = context.Database.SqlQuery<CellNo>(QryStr.ToString()).Select(d => new CellNo()
                {
                    Phone = d.Phone
                }).FirstOrDefault();

                if (si == null)
                {
                    return new CellNo();
                }
                else
                {
                    return si;
                }
            }
            catch (Exception)
            {
                return new CellNo();
            }
        }
    }
}
     
    
        

