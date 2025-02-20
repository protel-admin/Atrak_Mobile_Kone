﻿using System;
using System.Data.Entity;
using System.Text.RegularExpressions;
using Attendance.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Attendance.Repository
{
    public class AttendanceManagementContext : DbContext
    {
        public DbSet<ShiftExtensionAndDoubleShift> ShiftExtensionAndDoubleShift { get; set; }
        public DbSet<LeaveGroup> LeaveGroup { get; set; }
        public DbSet<LeaveType> LeaveType { get; set; }
        public DbSet<LeaveGroupTxn> LeaveGroupTxn { get; set; }
        public DbSet<ShiftsImportData> ShiftsImportData { get; set; }
        public DbSet<PrefixSuffixSetting> PrefixSuffixSetting { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<Designation> Designation { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<StaffPersonal> StaffPersonal { get; set; }
        public DbSet<StaffOfficial> StaffOfficial { get; set; }
        public DbSet<StaffFamily> StaffFamily { get; set; }
        public DbSet<StaffEducation> StaffEducation { get; set; }
        public DbSet<RelationType> RelationType { get; set; }
        public DbSet<BloodGroup> BloodGroup { get; set; }
        public DbSet<MaritalStatus> MaritalStatus { get; set; }
        public DbSet<StaffStatus> StaffStatus { get; set; }
        public DbSet<EmployeeLeaveAccount> EmployeeLeaveAccount { get; set; }
        public DbSet<LeaveTransactionType> LeaveTransactionType { get; set; }
        public DbSet<LeaveApplication> LeaveApplication { get; set; }
        public DbSet<LeaveDuration> LeaveDuration { get; set; }
        public DbSet<ViewApproval> ViewApproval { get; set; }
        //public DbSet<WeekDays> WeekDays { get; set; }
        public DbSet<WeeklyOffs> WeeklyOffs { get; set; }
        //public DbSet<WeeklyOffGroupTxns> WeeklyOffGroupTxns { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<HolidayGroup> HolidayGroups { get; set; }
        public DbSet<HolidayGroupTxn> HolidayGroupTxns { get; set; }
        public DbSet<Shifts> Shifts { get; set; }
        public DbSet<ShiftPattern> ShiftPattern { get; set; }
        public DbSet<ShiftPatternTxn> ShiftPatternTxn { get; set; }
        public DbSet<EmployeeGroup> EmployeeGroup { get; set; }
        public DbSet<EmployeeGroupTxn> EmployeeGroupTxn { get; set; }
        public DbSet<EmployeeGroupShiftPatternTxn> EmployeeGroupShiftPatternTxn { get; set; }
        public DbSet<ManualPunch> ManualPunch { get; set; }
        public DbSet<SwipeData> SwipeData { get; set; }
        public DbSet<AttendanceData> AttendanceData { get; set; }
        public DbSet<PermissionTxn> PermissionTxn { get; set; }
        public DbSet<PermissionType> PermissionType { get; set; }
        public DbSet<LogItem> LogItem { get; set; }
        public DbSet<TeamHierarchy> TeamHierarchy { get; set; }
        public DbSet<AbsenceApproval> AbsenceApproval { get; set; }
        public DbSet<Reader> Reader { get; set; }
        public DbSet<Door> Door { get; set; }
        public DbSet<SqlExecute> SqlExecute { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Rule> Rule { get; set; }
        public DbSet<RuleGroup> RuleGroup { get; set; }
        public DbSet<RuleGroupTxn> RuleGroupTxn { get; set; }
        public DbSet<HolidayFixedDay> HolidayFixedDay { get; set; }
        public DbSet<ShiftChangeApplication> ShiftChangeApplication { get; set; }
        public DbSet<ApplicationApproval> ApplicationApproval { get; set; }
        public DbSet<LeaveApplicationWabco> LeaveApplicationWabco { get; set; }
        public DbSet<CompensatoryOff> CompensatoryOff { get; set; }
        public DbSet<LaterOff> LaterOff { get; set; }
        public DbSet<MaintenanceOff> MaintenanceOff { get; set; }
        public DbSet<PermissionOff> PermissionOff { get; set; }
        public DbSet<HolidayZone> HolidayZone { get; set; }
        public DbSet<HolidayZoneTxn> HolidayZoneTxn { get; set; }
        public DbSet<WorkingDayPattern> WorkingDayPattern { get; set; }
        public DbSet<DefaultShift> DefaultShift { get; set; }
        public DbSet<Salutation> Salutation { get; set; }
        public DbSet<ShiftRelay> ShiftRelay { get; set; }
        public DbSet<Screen> Screen { get; set; }
        public DbSet<SecurityGroup> SecurityGroup { get; set; }
        public DbSet<SecurityGroupTxns> SecurityGroupTxns { get; set; }
        public DbSet<EmailSendLog> EmailSendLog { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }
        public DbSet<ReportToBeSent> ReportToBeSent { get; set; }
        public DbSet<ReportToBeSentTxn> ReportToBeSentTxn { get; set; }
        public DbSet<ReportsByEmail> ReportsByEmail { get; set; }
        public DbSet<GroupAssociation> GroupAssociation { get; set; }
        public DbSet<FinancialYear> FinancialYear { get; set; }
        public DbSet<Association> Association { get; set; }
        public DbSet<OTApplication> OTApplication { get; set; }
        public DbSet<OTApplicationEntry> OTApplicationEntry { get; set; }
        public DbSet<SMaxTransaction> SMaxTransaction { get; set; }
        public DbSet<ODApplication> ODApplication { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CostCentre> CostCentre { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<PeopleSoftDump> PeopleSoftDump { get; set; }
        public DbSet<ExcelImport> ExcelImport { get; set; }
        public DbSet<MOffYear> MOffYear { get; set; }
        public DbSet<LeaveReason> LeaveReason { get; set; }
        public DbSet<RestrictedHolidays> RestrictedHolidays { get; set; }
        public DbSet<RHApplication> RHApplication { get; set; }
        public DbSet<ApplicationEntry> ApplicationEntry { get; set; }
        public DbSet<AttendanceReaders> AttendanceReaders { get; set; }
        public DbSet<ShiftParentTxn> ShiftParentTxn { get; set; }
        public DbSet<LaterOffDate> LaterOffDate { get; set; }
        public DbSet<LeaveDebits> LeaveDebits { get; set; }
        public DbSet<CardBlock> CardBlock { get; set; }
        public DbSet<Volume> Volume { get; set; }
        public DbSet<AttendanceControlTable> AttendanceControlTable { get; set; }
        public DbSet<AdditionalField> AdditionalField { get; set; }
        public DbSet<AdditionalFieldValue> AdditionalFieldValue { get; set; }

        //public DbSet<VisitorPass> VisitorPass { get; set; }
        public DbSet<VisitorPassApprovalHierarchy> VisitorPassApprovalHierarchy { get; set; }
        public DbSet<ShiftPostingPattern> ShiftPostingPattern { get; set; }
        public DbSet<EmployeeShiftPlan> EmployeeShiftPlan { get; set; }
        public DbSet<LateComing> LateComing { get; set; }
        public DbSet<UploadControlTable> UploadControlTable { get; set; }
        public DbSet<ChangeAuditLog> ChangeAuditLog { get; set; }
        public DbSet<EmployeePhoto> EmployeePhoto { get; set; }
        public DbSet<StaffEditRequest> StaffEditRequest { get; set; }
        public DbSet<BenchReportingManager> BenchReportingManager { get; set; }
        public DbSet<Workstation> WorkStation { get; set; }
        public DbSet<WorkstationAllocation> WorkStationAllocation { get; set; }
        public DbSet<SubordinateTree> SubordinateTree { get; set; }
        public DbSet<AttachDetachLog> AttachDetachLog { get; set; }
        public DbSet<RequestApplication> RequestApplication { get; set; }
        public DbSet<Testing> Testing { get; set; }
        public DbSet<LeaveCreditDebitReason> LeaveCreditDebitReason { get; set; }
        public DbSet<DocumentUpload> DocumentUpload { get; set; }
        public DbSet<AtrakUserDetails> AtrakUserDetails { get; set; }
        public DbSet<ChangeDetailsPW> PasswordChangeDetails { get; set; }
        public DbSet<LeaveTypeMaster> LeaveTypeMaster { get; set; }
        public DbSet<AlternativePersonAssign> AlternativePersonAssign { get; set; }
        public DbSet<ManualShiftChangeGrid> ManualShiftChangeGrid { get; set; }
        public DbSet<EmailForwardingconfig> EmailForwardingconfig { get; set; }
        public DbSet<OnDutyDuration> OnDutyDuration { get; set; }
        public DbSet<MobileSwipeTransactions> MobileSwipeTransactions { get; set; }

        public AttendanceManagementContext()
            : base("AttendanceManagementContext")
        {
            Configuration.ProxyCreationEnabled = true;
            Configuration.LazyLoadingEnabled = true;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<ShiftExtensionAndDoubleShift>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<LeaveGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<LeaveType>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<LeaveGroupTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ShiftsImportData>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PrefixSuffixSetting>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Company>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Branch>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Department>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Designation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Division>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Grade>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Staff>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<StaffPersonal>().Property(r => r.StaffId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<StaffOfficial>().Property(r => r.StaffId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<StaffFamily>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<StaffEducation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<RelationType>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<BloodGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MaritalStatus>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StaffStatus>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmployeeLeaveAccount>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LeaveTransactionType>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LeaveApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<LeaveDuration>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ViewApproval>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<WeekDays>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<WeeklyOffs>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            //modelBuilder.Entity<WeeklyOffGroupTxns>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Holiday>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<HolidayGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<HolidayGroupTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Shifts>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<ShiftPattern>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ShiftPatternTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmployeeGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<EmployeeGroupTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<EmployeeGroupShiftPatternTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ManualPunch>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<SwipeData>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AttendanceData>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PermissionTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<PermissionType>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LogItem>().Property(r => r.LogId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<TeamHierarchy>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AbsenceApproval>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Reader>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Door>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AttendanceControlTable>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmployeeShiftPlan>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<VisitorPass>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ChangeAuditLog>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<StaffEditRequest>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AdditionalField>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AdditionalFieldValue>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<SqlExecute>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Settings>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ErrorLog>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Rule>().Property(r => r.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<RuleGroup>().Property(r => r.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<RuleGroupTxn>().Property(r => r.id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<HolidayFixedDay>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ShiftChangeApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ApplicationApproval>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<LeaveApplicationWabco>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<CompensatoryOff>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<LaterOff>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<MaintenanceOff>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<PermissionOff>().Property(r => r.Id) .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<HolidayZone>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<HolidayZoneTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<WorkingDayPattern>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<DefaultShift>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Salutation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ShiftRelay>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Screen>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SecurityGroup>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<SecurityGroupTxns>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<EmailSendLog>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<EmailSettings>().Property(r => r.OutgoingServer).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ReportToBeSent>().Property(r => r.StaffId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ReportToBeSentTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ReportsByEmail>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<GroupAssociation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<FinancialYear>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Association>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<OTApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<OTApplicationEntry>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<SMaxTransaction>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ODApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Category>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<CostCentre>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Location>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<PeopleSoftDump>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ExcelImport>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<MOffYear>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LeaveReason>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<RestrictedHolidays>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<RHApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ApplicationEntry>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<AttendanceReaders>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<ShiftParentTxn>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LaterOffDate>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<VisitorPassApprovalHierarchy>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LeaveDebits>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<CardBlock>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Volume>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<ShiftPostingPattern>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<LateComing>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<UploadControlTable>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmployeePhoto>().Property(r => r.StaffId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<BenchReportingManager>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Workstation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<WorkstationAllocation>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<SubordinateTree>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AttachDetachLog>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<RequestApplication>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<Testing>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<LeaveCreditDebitReason>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<DocumentUpload>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<AtrakUserDetails>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ChangeDetailsPW>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LeaveTypeMaster>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<AlternativePersonAssign>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ManualShiftChangeGrid>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<EmailForwardingconfig>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<OnDutyDuration>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<MobileSwipeTransactions>().Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("smalldatetime"));
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            Database.SetInitializer<AttendanceManagementContext>(null);
            base.OnModelCreating(modelBuilder);

        }
    }

}