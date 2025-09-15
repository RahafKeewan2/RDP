/*
Class Name    : FeedingModel.cs 		         							  			            	
Created By    : copilot             															        
Ceation Date  : 9/15/2025   	                               										
Document Path :	\\devserver\\NOOR\\NoorDocuments\\Meals\\Requirements\\
                 166404-UC1FR2-Edit-FeedingModel.doc;
                 166406-UC1FR3-Delete-FeedingModel.doc;
                 165939-UC2-Select-Directorates-Schools.doc;
                 165940-UC3-School-Feeding-Committee.doc;
                 165941-UC4-Receipt-Form.doc;
                 165942-UC5-Distribution-Form.doc;
                 UC8-Directorate-Monthly-Reports.doc
Description   : School Meals - UC.1 (Create/Update/Delete), UC.2 (Select Dist/Schools/Classes + Approve),
                UC.3 (School Committee), UC.4 (Receipt Forms), UC.5 (Distribution Forms),
                UC.8 (Directorate Monthly Statistical Reports).
Notes         : Stored procedure prefix is SM_ per standard. Kept coding style and regions similar to GuidanceUser.cs.
*/

using System;
using System.Data;
using System.Collections.Generic;
using ITG_DBAccess;
using ITG_CustomControls;

namespace EduWaveTeacherAffairsCom
{
    /// <summary>
    /// The Feeding Model.
    /// </summary>
    public class FeedingModel : ITG_ClassLibrary
    {
        #region Members
        /// <summary>
        /// The n languageid.
        /// </summary>
        private int nLanguageid;

        /// <summary>
        /// The n id.
        /// </summary>
        private int nID;
        /// <summary>
        /// The s academic year.
        /// </summary>
        private string sAcademicYear;
        /// <summary>
        /// The s model type.
        /// </summary>
        private string sModelType;
        /// <summary>
        /// The s notes.
        /// </summary>
        private string sNotes;
        /// <summary>
        /// The s target group.
        /// </summary>
        private string sTargetGroup;
        /// <summary>
        /// The dti start date.
        /// </summary>
        private DateTime dtiStartDate;
        /// <summary>
        /// The dti end date.
        /// </summary>
        private DateTime dtiEndDate;
        /// <summary>
        /// The n feeding days.
        /// </summary>
        private int nFeedingDays;
        /// <summary>
        /// The s region ids (CSV).
        /// </summary>
        private string sRegionIDs;
        /// <summary>
        /// The s implementing agency ids (CSV).
        /// </summary>
        private string sImplementingAgencyIDs;
        /// <summary>
        /// The s supervising agency ids (CSV).
        /// </summary>
        private string sSupervisingAgencyIDs;
        /// <summary>
        /// The s meal components.
        /// </summary>
        private string sMealComponents;
        /// <summary>
        /// The n status id.
        /// </summary>
        private int nStatusID;
        /// <summary>
        /// The s status desc.
        /// </summary>
        private string sStatusDesc;
        /// <summary>
        /// The n created by.
        /// </summary>
        private int nCreatedBy;
        /// <summary>
        /// The dti created on.
        /// </summary>
        private DateTime dtiCreatedOn;

        // UC.2 context members
        /// <summary>
        /// The n dist id (context/select).
        /// </summary>
        private int nDistID;
        /// <summary>
        /// The s dist desc.
        /// </summary>
        private string sDistDesc;
        /// <summary>
        /// The n school id (context/select).
        /// </summary>
        private int nSchoolID;
        /// <summary>
        /// The s school desc.
        /// </summary>
        private string sSchoolDesc;
        /// <summary>
        /// The n class id (context/select).
        /// </summary>
        private int nClassID;
        /// <summary>
        /// The s class desc.
        /// </summary>
        private string sClassDesc;
        /// <summary>
        /// The s dist ids (CSV for save).
        /// </summary>
        private string sDistIDs;
        /// <summary>
        /// The s school ids (CSV for save).
        /// </summary>
        private string sSchoolIDs;
        /// <summary>
        /// The s class ids (CSV for save).
        /// </summary>
        private string sClassIDs;
        /// <summary>
        /// The n approved by.
        /// </summary>
        private int nApprovedBy;

        // UC.3 (School Feeding Committee) members
        /// <summary>
        /// The n committee head user id (school principal).
        /// </summary>
        private int nCommitteeHeadUserID;
        /// <summary>
        /// The s committee head name (read-only).
        /// </summary>
        private string sCommitteeHeadName;
        /// <summary>
        /// The n nutrition officer user id (required).
        /// </summary>
        private int nNutritionOfficerUserID;
        /// <summary>
        /// The s nutrition officer name.
        /// </summary>
        private string sNutritionOfficerName;
        /// <summary>
        /// The n section id (context/select).
        /// </summary>
        private int nSectionID;
        /// <summary>
        /// The s section desc.
        /// </summary>
        private string sSectionDesc;
        /// <summary>
        /// The n responsible employee id (per section assignment).
        /// </summary>
        private int nResponsibleEmployeeUserID;
        /// <summary>
        /// The s responsible employee name.
        /// </summary>
        private string sResponsibleEmployeeName;
        /// <summary>
        /// The s section assignments CSV (e.g., "SectionID:UserID,SectionID:UserID").
        /// </summary>
        private string sSectionAssignments;
        /// <summary>
        /// The n employee id (generic staff list).
        /// </summary>
        private int nEmployeeID;
        /// <summary>
        /// The s employee name (generic staff list).
        /// </summary>
        private string sEmployeeName;

        // UC.4 (Receipt Form) members
        /// <summary>
        /// The n receipt id.
        /// </summary>
        private int nReceiptID;
        /// <summary>
        /// The n quantity received.
        /// </summary>
        private int nQuantityReceived;
        /// <summary>
        /// The dti receipt date.
        /// </summary>
        private DateTime dtiReceiptDate;
        /// <summary>
        /// The n receipt time id.
        /// </summary>
        private int nReceiptTimeID;
        /// <summary>
        /// The s receipt time desc.
        /// </summary>
        private string sReceiptTimeDesc;
        /// <summary>
        /// The dti receipt created on.
        /// </summary>
        private DateTime dtiReceiptCreatedOn;

        // UC.5 (Distribution Form) members
        /// <summary>
        /// The n distribution id.
        /// </summary>
        private int nDistributionID;
        /// <summary>
        /// The dti distribution date.
        /// </summary>
        private DateTime dtiDistributionDate;
        /// <summary>
        /// The n student id.
        /// </summary>
        private int nStudentID;
        /// <summary>
        /// The s student name.
        /// </summary>
        private string sStudentName;
        /// <summary>
        /// The n student status id (1=Received, 2=Absent, 3=Refused).
        /// </summary>
        private int nStudentStatusID;
        /// <summary>
        /// The s student status desc.
        /// </summary>
        private string sStudentStatusDesc;
        /// <summary>
        /// The n is received flag (1=selected/received, 0=not received).
        /// </summary>
        private int nIsReceived;
        /// <summary>
        /// The s student statuses CSV mapping "StudentID:StatusID,StudentID:StatusID".
        /// </summary>
        private string sStudentStatuses;

        // UC.8 (Directorate Monthly Reports) members
        /// <summary>
        /// The n report month (1..12).
        /// </summary>
        private int nReportMonth;
        /// <summary>
        /// The n report year (e.g., 2025).
        /// </summary>
        private int nReportYear;
        /// <summary>
        /// The n total students (aggregation).
        /// </summary>
        private int nTotalStudents;
        /// <summary>
        /// The n received count.
        /// </summary>
        private int nReceivedCount;
        /// <summary>
        /// The n absent count.
        /// </summary>
        private int nAbsentCount;
        /// <summary>
        /// The n refused count.
        /// </summary>
        private int nRefusedCount;
        /// <summary>
        /// The n distributed count (meals handed out).
        /// </summary>
        private int nDistributedCount;
        // UC.9 (Parent Notes) members
        /// <summary>
        /// Parent Note (ولي الأمر) - معرف الملاحظة
        /// </summary>
        private int nParentNoteID;
        /// <summary>
        /// Parent Note (ولي الأمر) - معرف المستخدم لولي الأمر
        /// </summary>
        private int nParentUserID;
        /// <summary>
        /// Parent Note (ولي الأمر) - نص الملاحظة (حتى 300 حرف)
        /// </summary>
        private string sParentNoteText;
        /// <summary>
        /// Parent Note (ولي الأمر) - تاريخ إدخال الملاحظة
        /// </summary>
        private DateTime dtiParentNoteDate;

        /// <summary>
        /// Parent Note (ولي الأمر) - حالة الملاحظة (1=Submitted, 2=Acknowledged)
        /// </summary>
        private int nParentNoteStatusID;
        /// <summary>
        /// Parent Note (ولي الأمر) - وصف حالة الملاحظة
        /// </summary>
        private string sParentNoteStatusDesc;

        /// <summary>
        /// Parent Note (ولي الأمر) - تم الاستلام؟ (1=نعم, 0=لا)
        /// </summary>
        private int nIsAcknowledged;
        /// <summary>
        /// Parent Note (ولي الأمر) - المستخدم الذي أكد الاستلام (مدير المدرسة)
        /// </summary>
        private int nAcknowledgedByUserID;
        /// <summary>
        /// Parent Note (ولي الأمر) - تاريخ/وقت التأكيد
        /// </summary>
        private DateTime dtiAcknowledgedOn;
        #endregion

        #region Properties

        #region Property :: Languageid
        /// <summary>
        /// Gets or sets Languageid
        /// </summary>
        public int Languageid
        {
            set { nLanguageid = value; }
            get { return nLanguageid; }
        }
        #endregion

        #region Property :: ID
        /// <summary>
        /// Gets or sets ID (ModelID)
        /// </summary>
        public int ID
        {
            set { nID = value; }
            get { return nID; }
        }
        #endregion

        #region Property :: AcademicYear
        /// <summary>
        /// Gets or sets AcademicYear
        /// </summary>
        public string AcademicYear
        {
            set { sAcademicYear = value; }
            get { return sAcademicYear; }
        }
        #endregion

        #region Property :: ModelType
        /// <summary>
        /// Gets or sets ModelType
        /// </summary>
        public string ModelType
        {
            set { sModelType = value; }
            get { return sModelType; }
        }
        #endregion

        #region Property :: Notes
        /// <summary>
        /// Gets or sets Notes
        /// </summary>
        public string Notes
        {
            set { sNotes = value; }
            get { return sNotes; }
        }
        #endregion

        #region Property :: TargetGroup
        /// <summary>
        /// Gets or sets TargetGroup
        /// </summary>
        public string TargetGroup
        {
            set { sTargetGroup = value; }
            get { return sTargetGroup; }
        }
        #endregion

        #region Property :: StartDate
        /// <summary>
        /// Gets or sets StartDate
        /// </summary>
        public DateTime StartDate
        {
            set { dtiStartDate = value; }
            get { return dtiStartDate; }
        }
        #endregion

        #region Property :: EndDate
        /// <summary>
        /// Gets or sets EndDate
        /// </summary>
        public DateTime EndDate
        {
            set { dtiEndDate = value; }
            get { return dtiEndDate; }
        }
        #endregion

        #region Property :: FeedingDays
        /// <summary>
        /// Gets or sets FeedingDays
        /// </summary>
        public int FeedingDays
        {
            set { nFeedingDays = value; }
            get { return nFeedingDays; }
        }
        #endregion

        #region Property :: RegionIDs
        /// <summary>
        /// Gets or sets RegionIDs (CSV)
        /// </summary>
        public string RegionIDs
        {
            set { sRegionIDs = value; }
            get { return sRegionIDs; }
        }
        #endregion

        #region Property :: ImplementingAgencyIDs
        /// <summary>
        /// Gets or sets ImplementingAgencyIDs (CSV)
        /// </summary>
        public string ImplementingAgencyIDs
        {
            set { sImplementingAgencyIDs = value; }
            get { return sImplementingAgencyIDs; }
        }
        #endregion

        #region Property :: SupervisingAgencyIDs
        /// <summary>
        /// Gets or sets SupervisingAgencyIDs (CSV)
        /// </summary>
        public string SupervisingAgencyIDs
        {
            set { sSupervisingAgencyIDs = value; }
            get { return sSupervisingAgencyIDs; }
        }
        #endregion

        #region Property :: MealComponents
        /// <summary>
        /// Gets or sets MealComponents
        /// </summary>
        public string MealComponents
        {
            set { sMealComponents = value; }
            get { return sMealComponents; }
        }
        #endregion

        #region Property :: StatusID
        /// <summary>
        /// Gets or sets StatusID
        /// </summary>
        public int StatusID
        {
            set { nStatusID = value; }
            get { return nStatusID; }
        }
        #endregion

        #region Property :: StatusDesc
        /// <summary>
        /// Gets or sets StatusDesc
        /// </summary>
        public string StatusDesc
        {
            set { sStatusDesc = value; }
            get { return sStatusDesc; }
        }
        #endregion

        #region Property :: CreatedBy
        /// <summary>
        /// Gets or sets CreatedBy
        /// </summary>
        public int CreatedBy
        {
            set { nCreatedBy = value; }
            get { return nCreatedBy; }
        }
        #endregion

        #region Property :: CreatedOn
        /// <summary>
        /// Gets or sets CreatedOn
        /// </summary>
        public DateTime CreatedOn
        {
            set { dtiCreatedOn = value; }
            get { return dtiCreatedOn; }
        }
        #endregion

        #region Property :: DistID
        /// <summary>
        /// Gets or sets DistID (UC.2 context)
        /// </summary>
        public int DistID
        {
            set { nDistID = value; }
            get { return nDistID; }
        }
        #endregion

        #region Property :: DistDesc
        /// <summary>
        /// Gets or sets DistDesc
        /// </summary>
        public string DistDesc
        {
            set { sDistDesc = value; }
            get { return sDistDesc; }
        }
        #endregion

        #region Property :: SchoolID
        /// <summary>
        /// Gets or sets SchoolID (UC.2/UC.3/UC.4/UC.5 context)
        /// </summary>
        public int SchoolID
        {
            set { nSchoolID = value; }
            get { return nSchoolID; }
        }
        #endregion

        #region Property :: SchoolDesc
        /// <summary>
        /// Gets or sets SchoolDesc
        /// </summary>
        public string SchoolDesc
        {
            set { sSchoolDesc = value; }
            get { return sSchoolDesc; }
        }
        #endregion

        #region Property :: ClassID
        /// <summary>
        /// Gets or sets ClassID (UC.2/UC.3/UC.5 context)
        /// </summary>
        public int ClassID
        {
            set { nClassID = value; }
            get { return nClassID; }
        }
        #endregion

        #region Property :: ClassDesc
        /// <summary>
        /// Gets or sets ClassDesc
        /// </summary>
        public string ClassDesc
        {
            set { sClassDesc = value; }
            get { return sClassDesc; }
        }
        #endregion

        #region Property :: DistIDs
        /// <summary>
        /// Gets or sets DistIDs (CSV for save)
        /// </summary>
        public string DistIDs
        {
            set { sDistIDs = value; }
            get { return sDistIDs; }
        }
        #endregion

        #region Property :: SchoolIDs
        /// <summary>
        /// Gets or sets SchoolIDs (CSV for save)
        /// </summary>
        public string SchoolIDs
        {
            set { sSchoolIDs = value; }
            get { return sSchoolIDs; }
        }
        #endregion

        #region Property :: ClassIDs
        /// <summary>
        /// Gets or sets ClassIDs (CSV for save)
        /// </summary>
        public string ClassIDs
        {
            set { sClassIDs = value; }
            get { return sClassIDs; }
        }
        #endregion

        #region Property :: ApprovedBy
        /// <summary>
        /// Gets or sets ApprovedBy (approver user id)
        /// </summary>
        public int ApprovedBy
        {
            set { nApprovedBy = value; }
            get { return nApprovedBy; }
        }
        #endregion

        #region Property :: CommitteeHeadUserID
        /// <summary>
        /// Gets or sets CommitteeHeadUserID (school principal)
        /// </summary>
        public int CommitteeHeadUserID
        {
            set { nCommitteeHeadUserID = value; }
            get { return nCommitteeHeadUserID; }
        }
        #endregion

        #region Property :: CommitteeHeadName
        /// <summary>
        /// Gets or sets CommitteeHeadName (read-only display)
        /// </summary>
        public string CommitteeHeadName
        {
            set { sCommitteeHeadName = value; }
            get { return sCommitteeHeadName; }
        }
        #endregion

        #region Property :: NutritionOfficerUserID
        /// <summary>
        /// Gets or sets NutritionOfficerUserID (required)
        /// </summary>
        public int NutritionOfficerUserID
        {
            set { nNutritionOfficerUserID = value; }
            get { return nNutritionOfficerUserID; }
        }
        #endregion

        #region Property :: NutritionOfficerName
        /// <summary>
        /// Gets or sets NutritionOfficerName
        /// </summary>
        public string NutritionOfficerName
        {
            set { sNutritionOfficerName = value; }
            get { return sNutritionOfficerName; }
        }
        #endregion

        #region Property :: SectionID
        /// <summary>
        /// Gets or sets SectionID (UC.2/UC.3/UC.5 context)
        /// </summary>
        public int SectionID
        {
            set { nSectionID = value; }
            get { return nSectionID; }
        }
        #endregion

        #region Property :: SectionDesc
        /// <summary>
        /// Gets or sets SectionDesc
        /// </summary>
        public string SectionDesc
        {
            set { sSectionDesc = value; }
            get { return sSectionDesc; }
        }
        #endregion

        #region Property :: ResponsibleEmployeeUserID
        /// <summary>
        /// Gets or sets ResponsibleEmployeeUserID (per section)
        /// </summary>
        public int ResponsibleEmployeeUserID
        {
            set { nResponsibleEmployeeUserID = value; }
            get { return nResponsibleEmployeeUserID; }
        }
        #endregion

        #region Property :: ResponsibleEmployeeName
        /// <summary>
        /// Gets or sets ResponsibleEmployeeName
        /// </summary>
        public string ResponsibleEmployeeName
        {
            set { sResponsibleEmployeeName = value; }
            get { return sResponsibleEmployeeName; }
        }
        #endregion

        #region Property :: SectionAssignments
        /// <summary>
        /// Gets or sets SectionAssignments (CSV mapping "SectionID:UserID,SectionID:UserID")
        /// </summary>
        public string SectionAssignments
        {
            set { sSectionAssignments = value; }
            get { return sSectionAssignments; }
        }
        #endregion

        #region Property :: EmployeeID
        /// <summary>
        /// Gets or sets EmployeeID (generic staff item)
        /// </summary>
        public int EmployeeID
        {
            set { nEmployeeID = value; }
            get { return nEmployeeID; }
        }
        #endregion

        #region Property :: EmployeeName
        /// <summary>
        /// Gets or sets EmployeeName (generic staff item)
        /// </summary>
        public string EmployeeName
        {
            set { sEmployeeName = value; }
            get { return sEmployeeName; }
        }
        #endregion

        #region Property :: ReceiptID
        /// <summary>
        /// Gets or sets ReceiptID
        /// </summary>
        public int ReceiptID
        {
            set { nReceiptID = value; }
            get { return nReceiptID; }
        }
        #endregion

        #region Property :: QuantityReceived
        /// <summary>
        /// Gets or sets QuantityReceived
        /// </summary>
        public int QuantityReceived
        {
            set { nQuantityReceived = value; }
            get { return nQuantityReceived; }
        }
        #endregion

        #region Property :: ReceiptDate
        /// <summary>
        /// Gets or sets ReceiptDate
        /// </summary>
        public DateTime ReceiptDate
        {
            set { dtiReceiptDate = value; }
            get { return dtiReceiptDate; }
        }
        #endregion

        #region Property :: ReceiptTimeID
        /// <summary>
        /// Gets or sets ReceiptTimeID (1=OnTime, 2=Early, 3=Late)
        /// </summary>
        public int ReceiptTimeID
        {
            set { nReceiptTimeID = value; }
            get { return nReceiptTimeID; }
        }
        #endregion

        #region Property :: ReceiptTimeDesc
        /// <summary>
        /// Gets or sets ReceiptTimeDesc
        /// </summary>
        public string ReceiptTimeDesc
        {
            set { sReceiptTimeDesc = value; }
            get { return sReceiptTimeDesc; }
        }
        #endregion

        #region Property :: ReceiptCreatedOn
        /// <summary>
        /// Gets or sets ReceiptCreatedOn
        /// </summary>
        public DateTime ReceiptCreatedOn
        {
            set { dtiReceiptCreatedOn = value; }
            get { return dtiReceiptCreatedOn; }
        }
        #endregion

        #region Property :: DistributionID
        /// <summary>
        /// Gets or sets DistributionID
        /// </summary>
        public int DistributionID
        {
            set { nDistributionID = value; }
            get { return nDistributionID; }
        }
        #endregion

        #region Property :: DistributionDate
        /// <summary>
        /// Gets or sets DistributionDate
        /// </summary>
        public DateTime DistributionDate
        {
            set { dtiDistributionDate = value; }
            get { return dtiDistributionDate; }
        }
        #endregion

        #region Property :: StudentID
        /// <summary>
        /// Gets or sets StudentID
        /// </summary>
        public int StudentID
        {
            set { nStudentID = value; }
            get { return nStudentID; }
        }
        #endregion

        #region Property :: StudentName
        /// <summary>
        /// Gets or sets StudentName
        /// </summary>
        public string StudentName
        {
            set { sStudentName = value; }
            get { return sStudentName; }
        }
        #endregion

        #region Property :: StudentStatusID
        /// <summary>
        /// Gets or sets StudentStatusID (1=Received, 2=Absent, 3=Refused)
        /// </summary>
        public int StudentStatusID
        {
            set { nStudentStatusID = value; }
            get { return nStudentStatusID; }
        }
        #endregion

        #region Property :: StudentStatusDesc
        /// <summary>
        /// Gets or sets StudentStatusDesc
        /// </summary>
        public string StudentStatusDesc
        {
            set { sStudentStatusDesc = value; }
            get { return sStudentStatusDesc; }
        }
        #endregion

        #region Property :: IsReceived
        /// <summary>
        /// Gets or sets IsReceived (1=Received, 0=Not Received)
        /// </summary>
        public int IsReceived
        {
            set { nIsReceived = value; }
            get { return nIsReceived; }
        }
        #endregion

        #region Property :: StudentStatuses
        /// <summary>
        /// Gets or sets StudentStatuses (CSV mapping "StudentID:StatusID,StudentID:StatusID")
        /// </summary>
        public string StudentStatuses
        {
            set { sStudentStatuses = value; }
            get { return sStudentStatuses; }
        }
        #endregion

        #region Property :: ReportMonth
        /// <summary>
        /// Gets or sets ReportMonth (1..12)
        /// </summary>
        public int ReportMonth
        {
            set { nReportMonth = value; }
            get { return nReportMonth; }
        }
        #endregion

        #region Property :: ReportYear
        /// <summary>
        /// Gets or sets ReportYear (e.g., 2025)
        /// </summary>
        public int ReportYear
        {
            set { nReportYear = value; }
            get { return nReportYear; }
        }
        #endregion

        #region Property :: TotalStudents
        /// <summary>
        /// Gets or sets TotalStudents (aggregation)
        /// </summary>
        public int TotalStudents
        {
            set { nTotalStudents = value; }
            get { return nTotalStudents; }
        }
        #endregion

        #region Property :: ReceivedCount
        /// <summary>
        /// Gets or sets ReceivedCount
        /// </summary>
        public int ReceivedCount
        {
            set { nReceivedCount = value; }
            get { return nReceivedCount; }
        }
        #endregion

        #region Property :: AbsentCount
        /// <summary>
        /// Gets or sets AbsentCount
        /// </summary>
        public int AbsentCount
        {
            set { nAbsentCount = value; }
            get { return nAbsentCount; }
        }
        #endregion

        #region Property :: RefusedCount
        /// <summary>
        /// Gets or sets RefusedCount
        /// </summary>
        public int RefusedCount
        {
            set { nRefusedCount = value; }
            get { return nRefusedCount; }
        }
        #endregion

        #region Property :: DistributedCount
        /// <summary>
        /// Gets or sets DistributedCount (meals handed out)
        /// </summary>
        public int DistributedCount
        {
            set { nDistributedCount = value; }
            get { return nDistributedCount; }
        }
        #endregion

        // UC.9 (Parent Notes) properties

        /// <summary>
        /// ParentNoteID (المعرف التسلسلي للملاحظة)
        /// </summary>
        public int ParentNoteID
        {
            set { nParentNoteID = value; }
            get { return nParentNoteID; }
        }

        /// <summary>
        /// ParentUserID (UserID الخاص بولي الأمر)
        /// </summary>
        public int ParentUserID
        {
            set { nParentUserID = value; }
            get { return nParentUserID; }
        }

        /// <summary>
        /// ParentNoteText (نص ملاحظة ولي الأمر — حد أقصى 300 حرف)
        /// </summary>
        public string ParentNoteText
        {
            set { sParentNoteText = value; }
            get { return sParentNoteText; }
        }

        /// <summary>
        /// ParentNoteDate (تاريخ إدخال الملاحظة)
        /// </summary>
        public DateTime ParentNoteDate
        {
            set { dtiParentNoteDate = value; }
            get { return dtiParentNoteDate; }
        }

        /// <summary>
        /// ParentNoteStatusID (1=Submitted, 2=Acknowledged)
        /// </summary>
        public int ParentNoteStatusID
        {
            set { nParentNoteStatusID = value; }
            get { return nParentNoteStatusID; }
        }

        /// <summary>
        /// ParentNoteStatusDesc (وصف حالة الملاحظة)
        /// </summary>
        public string ParentNoteStatusDesc
        {
            set { sParentNoteStatusDesc = value; }
            get { return sParentNoteStatusDesc; }
        }

        /// <summary>
        /// IsAcknowledged (1=تم الاستلام, 0=لم يتم)
        /// </summary>
        public int IsAcknowledged
        {
            set { nIsAcknowledged = value; }
            get { return nIsAcknowledged; }
        }

        /// <summary>
        /// AcknowledgedByUserID (UserID لمدير المدرسة الذي أكد الاستلام)
        /// </summary>
        public int AcknowledgedByUserID
        {
            set { nAcknowledgedByUserID = value; }
            get { return nAcknowledgedByUserID; }
        }

        /// <summary>
        /// AcknowledgedOn (تاريخ/وقت التأكيد)
        /// </summary>
        public DateTime AcknowledgedOn
        {
            set { dtiAcknowledgedOn = value; }
            get { return dtiAcknowledgedOn; }
        }


        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedingModel"/> class.
        /// </summary>
        public FeedingModel()
        {
            this.nLanguageid = -99;

            this.nID = -99;
            this.sAcademicYear = string.Empty;
            this.sModelType = string.Empty;
            this.sNotes = string.Empty;
            this.sTargetGroup = string.Empty;
            this.dtiStartDate = DateTime.MinValue;
            this.dtiEndDate = DateTime.MinValue;
            this.nFeedingDays = -99;

            this.sRegionIDs = string.Empty;
            this.sImplementingAgencyIDs = string.Empty;
            this.sSupervisingAgencyIDs = string.Empty;
            this.sMealComponents = string.Empty;

            this.nStatusID = -99;
            this.sStatusDesc = string.Empty;

            this.nCreatedBy = -99;
            this.dtiCreatedOn = DateTime.MinValue;

            this.nDistID = -99;
            this.sDistDesc = string.Empty;
            this.nSchoolID = -99;
            this.sSchoolDesc = string.Empty;
            this.nClassID = -99;
            this.sClassDesc = string.Empty;

            this.sDistIDs = string.Empty;
            this.sSchoolIDs = string.Empty;
            this.sClassIDs = string.Empty;

            this.nApprovedBy = -99;

            this.nCommitteeHeadUserID = -99;
            this.sCommitteeHeadName = string.Empty;
            this.nNutritionOfficerUserID = -99;
            this.sNutritionOfficerName = string.Empty;
            this.nSectionID = -99;
            this.sSectionDesc = string.Empty;
            this.nResponsibleEmployeeUserID = -99;
            this.sResponsibleEmployeeName = string.Empty;
            this.sSectionAssignments = string.Empty;
            this.nEmployeeID = -99;
            this.sEmployeeName = string.Empty;

            this.nReceiptID = -99;
            this.nQuantityReceived = -99;
            this.dtiReceiptDate = DateTime.MinValue;
            this.nReceiptTimeID = -99;
            this.sReceiptTimeDesc = string.Empty;
            this.dtiReceiptCreatedOn = DateTime.MinValue;

            this.nDistributionID = -99;
            this.dtiDistributionDate = DateTime.MinValue;
            this.nStudentID = -99;
            this.sStudentName = string.Empty;
            this.nStudentStatusID = -99;
            this.sStudentStatusDesc = string.Empty;
            this.nIsReceived = -99;
            this.sStudentStatuses = string.Empty;

            this.nReportMonth = -99;
            this.nReportYear = -99;
            this.nTotalStudents = -99;
            this.nReceivedCount = -99;
            this.nAbsentCount = -99;
            this.nRefusedCount = -99;
            this.nDistributedCount = -99;
        }
        #endregion

        #region Methods

        #region Method :: oArrGetFeedingModels
        /// <summary>
        /// This function is used to get FeedingModels
        /// </summary>
        public FeedingModel[] oArrGetFeedingModels()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliFeedingModels = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    if (!this.ID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    }
                    if (!this.AcademicYear.Trim().Equals(string.Empty))
                    {
                        oDBConnector.AddInParam("pAcademicYear", this.AcademicYear.Trim(), DBConnector.DBTypes.NVarChar);
                    }
                    if (!this.StatusID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pStatusID", this.StatusID, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetFeedingModels");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel oModel = new FeedingModel();
                            oModel.ID = oReader.FieldExists("ModelID") ? Convert.ToInt32(oReader["ModelID"]) : -99;
                            oModel.AcademicYear = oReader.FieldExists("AcademicYear") ? oReader["AcademicYear"].ToString() : string.Empty;
                            oModel.ModelType = oReader.FieldExists("ModelType") ? oReader["ModelType"].ToString() : string.Empty;
                            oModel.Notes = oReader.FieldExists("Notes") ? oReader["Notes"].ToString() : string.Empty;
                            oModel.TargetGroup = oReader.FieldExists("TargetGroup") ? oReader["TargetGroup"].ToString() : string.Empty;

                            oModel.StartDate = oReader.FieldExists("StartDate") ? Convert.ToDateTime(oReader["StartDate"]) : DateTime.MinValue;
                            oModel.EndDate = oReader.FieldExists("EndDate") ? Convert.ToDateTime(oReader["EndDate"]) : DateTime.MinValue;

                            oModel.FeedingDays = oReader.FieldExists("FeedingDays") ? Convert.ToInt32(oReader["FeedingDays"]) : -99;

                            oModel.RegionIDs = oReader.FieldExists("RegionIDs") ? oReader["RegionIDs"].ToString() : string.Empty;
                            oModel.ImplementingAgencyIDs = oReader.FieldExists("ImplementingAgencyIDs") ? oReader["ImplementingAgencyIDs"].ToString() : string.Empty;
                            oModel.SupervisingAgencyIDs = oReader.FieldExists("SupervisingAgencyIDs") ? oReader["SupervisingAgencyIDs"].ToString() : string.Empty;

                            oModel.MealComponents = oReader.FieldExists("MealComponents") ? oReader["MealComponents"].ToString() : string.Empty;

                            oModel.StatusID = oReader.FieldExists("StatusID") ? Convert.ToInt32(oReader["StatusID"]) : -99;
                            oModel.StatusDesc = oReader.FieldExists("StatusDesc") ? oReader["StatusDesc"].ToString() : string.Empty;

                            oModel.CreatedBy = oReader.FieldExists("CreatedBy") ? Convert.ToInt32(oReader["CreatedBy"]) : -99;
                            oModel.CreatedOn = oReader.FieldExists("CreatedOn") ? Convert.ToDateTime(oReader["CreatedOn"].ToString()) : DateTime.MinValue;

                            oliFeedingModels.Add(oModel);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliFeedingModels.ToArray();
        }
        #endregion

        #region Method :: vSaveFeedingModel
        /// <summary>
        /// Method to save (Create/Upsert) FeedingModel
        /// </summary>
        public void vSaveFeedingModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                if (!this.ID.Equals(-99))
                {
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                }
                if (!this.sAcademicYear.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pAcademicYear", this.sAcademicYear.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sModelType.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pModelType", this.sModelType.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sNotes.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pNotes", this.sNotes.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sTargetGroup.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pTargetGroup", this.sTargetGroup.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.dtiStartDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pStartDate", this.dtiStartDate, DBConnector.DBTypes.DateTime);
                }
                if (!this.dtiEndDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pEndDate", this.dtiEndDate, DBConnector.DBTypes.DateTime);
                }
                if (!this.nFeedingDays.Equals(-99))
                {
                    oDBConnector.AddInParam("pFeedingDays", this.nFeedingDays, DBConnector.DBTypes.Int);
                }
                if (!this.sRegionIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pRegionIDs", this.sRegionIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sImplementingAgencyIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pImplementingAgencyIDs", this.sImplementingAgencyIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sSupervisingAgencyIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pSupervisingAgencyIDs", this.sSupervisingAgencyIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sMealComponents.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pMealComponents", this.sMealComponents.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.nStatusID.Equals(-99))
                {
                    oDBConnector.AddInParam("pStatusID", this.nStatusID, DBConnector.DBTypes.Int);
                }
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveFeedingModel");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vUpdateFeedingModel
        /// <summary>
        /// Method to update FeedingModel (Explicit Update)
        /// </summary>
        public void vUpdateFeedingModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required for update");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);

                if (!this.sAcademicYear.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pAcademicYear", this.sAcademicYear.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sModelType.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pModelType", this.sModelType.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sNotes.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pNotes", this.sNotes.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sTargetGroup.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pTargetGroup", this.sTargetGroup.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.dtiStartDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pStartDate", this.dtiStartDate, DBConnector.DBTypes.DateTime);
                }
                if (!this.dtiEndDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pEndDate", this.dtiEndDate, DBConnector.DBTypes.DateTime);
                }
                if (!this.nFeedingDays.Equals(-99))
                {
                    oDBConnector.AddInParam("pFeedingDays", this.nFeedingDays, DBConnector.DBTypes.Int);
                }
                if (!this.sRegionIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pRegionIDs", this.sRegionIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sImplementingAgencyIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pImplementingAgencyIDs", this.sImplementingAgencyIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sSupervisingAgencyIDs.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pSupervisingAgencyIDs", this.sSupervisingAgencyIDs.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.sMealComponents.Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pMealComponents", this.sMealComponents.Trim(), DBConnector.DBTypes.NVarChar);
                }
                if (!this.nStatusID.Equals(-99))
                {
                    oDBConnector.AddInParam("pStatusID", this.nStatusID, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_UpdateFeedingModel");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vDeleteFeedingModel
        /// <summary>
        /// Method to delete (archive) FeedingModel
        /// </summary>
        public void vDeleteFeedingModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required for delete");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pDeletedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_DeleteFeedingModel");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        // ---------------- UC.2: مديريات / مدارس / صفوف + اعتماد النموذج ----------------

        #region Method :: oArrGetDirectoratesForModel
        /// <summary>
        /// This function is used to get available Directorates for a model (by regions)
        /// </summary>
        public FeedingModel[] oArrGetDirectoratesForModel()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliItems = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetDirectoratesForModel");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.DistID = oReader.FieldExists("DistID") ? Convert.ToInt32(oReader["DistID"]) :
                                        (oReader.FieldExists("ID") ? Convert.ToInt32(oReader["ID"]) : -99);
                            it.DistDesc = oReader.FieldExists("DistDesc") ? oReader["DistDesc"].ToString() :
                                          (oReader.FieldExists("Desc") ? oReader["Desc"].ToString() :
                                          (oReader.FieldExists("Name") ? oReader["Name"].ToString() : string.Empty));
                            oliItems.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliItems.ToArray();
        }
        #endregion

        #region Method :: vSaveDirectoratesForModel
        /// <summary>
        /// Method to save selected Directorates for model
        /// </summary>
        public void vSaveDirectoratesForModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.DistIDs.Trim().Equals(string.Empty))
            {
                throw new Exception("DistIDs (CSV) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pDistIDs", this.DistIDs.Trim(), DBConnector.DBTypes.NVarChar);
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveModelDirectorates");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: oArrGetSchoolsForModel
        /// <summary>
        /// This function is used to get available Schools for a model in a given Directorate
        /// </summary>
        public FeedingModel[] oArrGetSchoolsForModel()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.DistID.Equals(-99))
            {
                throw new Exception("DistID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliItems = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pDistID", this.DistID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetSchoolsForModel");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.SchoolID = oReader.FieldExists("SchoolID") ? Convert.ToInt32(oReader["SchoolID"]) :
                                          (oReader.FieldExists("ID") ? Convert.ToInt32(oReader["ID"]) : -99);
                            it.SchoolDesc = oReader.FieldExists("SchoolDesc") ? oReader["SchoolDesc"].ToString() :
                                            (oReader.FieldExists("Desc") ? oReader["Desc"].ToString() :
                                            (oReader.FieldExists("Name") ? oReader["Name"].ToString() : string.Empty));
                            oliItems.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliItems.ToArray();
        }
        #endregion

        #region Method :: vSaveSchoolsForModel
        /// <summary>
        /// Method to save selected Schools for model (under a directorate)
        /// </summary>
        public void vSaveSchoolsForModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.DistID.Equals(-99))
            {
                throw new Exception("DistID is required");
            }
            if (this.SchoolIDs.Trim().Equals(string.Empty))
            {
                throw new Exception("SchoolIDs (CSV) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pDistID", this.DistID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolIDs", this.SchoolIDs.Trim(), DBConnector.DBTypes.NVarChar);
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveModelSchools");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: oArrGetClassesForModel
        /// <summary>
        /// This function is used to get available Classes for a model in a given School
        /// </summary>
        public FeedingModel[] oArrGetClassesForModel()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliItems = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetClassesForModel");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.ClassID = oReader.FieldExists("ClassID") ? Convert.ToInt32(oReader["ClassID"]) :
                                         (oReader.FieldExists("ID") ? Convert.ToInt32(oReader["ID"]) : -99);
                            it.ClassDesc = oReader.FieldExists("ClassDesc") ? oReader["ClassDesc"].ToString() :
                                           (oReader.FieldExists("Desc") ? oReader["Desc"].ToString() :
                                           (oReader.FieldExists("Name") ? oReader["Name"].ToString() : string.Empty));
                            oliItems.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliItems.ToArray();
        }
        #endregion

        #region Method :: vSaveClassesForModel
        /// <summary>
        /// Method to save selected Classes for model (under a school)
        /// </summary>
        public void vSaveClassesForModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.ClassIDs.Trim().Equals(string.Empty))
            {
                throw new Exception("ClassIDs (CSV) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pClassIDs", this.ClassIDs.Trim(), DBConnector.DBTypes.NVarChar);
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveModelClasses");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vApproveFeedingModel
        /// <summary>
        /// Method to approve FeedingModel (UC.2)
        /// </summary>
        public void vApproveFeedingModel()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.ApprovedBy.Equals(-99) && this.CreatedBy.Equals(-99))
            {
                throw new Exception("ApprovedBy or CreatedBy is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                int approver = !this.ApprovedBy.Equals(-99) ? this.ApprovedBy : this.CreatedBy;

                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pApprovedBy", approver, DBConnector.DBTypes.Int);

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_ApproveFeedingModel");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        // ---------------- UC.3: لجنة التغذية في المدرسة ----------------

        #region Method :: oArrGetSchoolCommittee
        /// <summary>
        /// This function is used to get the School Feeding Committee (Head and Nutrition Officer) for a given model and school
        /// </summary>
        public FeedingModel[] oArrGetSchoolCommittee()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliCommittee = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetSchoolCommittee");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.CommitteeHeadUserID = oReader.FieldExists("CommitteeHeadUserID") ? Convert.ToInt32(oReader["CommitteeHeadUserID"]) : -99;
                            it.CommitteeHeadName = oReader.FieldExists("CommitteeHeadName") ? oReader["CommitteeHeadName"].ToString() : string.Empty;

                            it.NutritionOfficerUserID = oReader.FieldExists("NutritionOfficerUserID") ? Convert.ToInt32(oReader["NutritionOfficerUserID"]) : -99;
                            it.NutritionOfficerName = oReader.FieldExists("NutritionOfficerName") ? oReader["NutritionOfficerName"].ToString() : string.Empty;

                            oliCommittee.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliCommittee.ToArray();
        }
        #endregion

        #region Method :: oArrGetSchoolStaff
        /// <summary>
        /// This function is used to get all staff in the school for selection (DDL)
        /// </summary>
        public FeedingModel[] oArrGetSchoolStaff()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliStaff = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetSchoolStaff");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.EmployeeID = oReader.FieldExists("EmployeeID") ? Convert.ToInt32(oReader["EmployeeID"]) :
                                            (oReader.FieldExists("UserID") ? Convert.ToInt32(oReader["UserID"]) : -99);
                            it.EmployeeName = oReader.FieldExists("EmployeeName") ? oReader["EmployeeName"].ToString() :
                                              (oReader.FieldExists("FullName") ? oReader["FullName"].ToString() : string.Empty);
                            oliStaff.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliStaff.ToArray();
        }
        #endregion

        #region Method :: vSaveSchoolCommittee
        /// <summary>
        /// Method to save/approve the School Feeding Committee
        /// </summary>
        public void vSaveSchoolCommittee()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.NutritionOfficerUserID.Equals(-99))
            {
                throw new Exception("NutritionOfficerUserID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pNutritionOfficerUserID", this.NutritionOfficerUserID, DBConnector.DBTypes.Int);

                if (!this.CommitteeHeadUserID.Equals(-99))
                {
                    oDBConnector.AddInParam("pCommitteeHeadUserID", this.CommitteeHeadUserID, DBConnector.DBTypes.Int);
                }
                if (!this.CommitteeHeadName.Trim().Equals(string.Empty))
                {
                    oDBConnector.AddInParam("pCommitteeHeadName", this.CommitteeHeadName.Trim(), DBConnector.DBTypes.NVarChar);
                }

                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveSchoolCommittee");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: oArrGetModelSchoolSections
        /// <summary>
        /// This function is used to get classes and sections under the model-school with current responsible employees (if any)
        /// </summary>
        public FeedingModel[] oArrGetModelSchoolSections()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliSections = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetModelSchoolSections");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.ClassID = oReader.FieldExists("ClassID") ? Convert.ToInt32(oReader["ClassID"]) : -99;
                            it.ClassDesc = oReader.FieldExists("ClassDesc") ? oReader["ClassDesc"].ToString() : string.Empty;

                            it.SectionID = oReader.FieldExists("SectionID") ? Convert.ToInt32(oReader["SectionID"]) : -99;
                            it.SectionDesc = oReader.FieldExists("SectionDesc") ? oReader["SectionDesc"].ToString() : string.Empty;

                            it.ResponsibleEmployeeUserID = oReader.FieldExists("ResponsibleEmployeeUserID") ? Convert.ToInt32(oReader["ResponsibleEmployeeUserID"]) : -99;
                            it.ResponsibleEmployeeName = oReader.FieldExists("ResponsibleEmployeeName") ? oReader["ResponsibleEmployeeName"].ToString() : string.Empty;

                            oliSections.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliSections.ToArray();
        }
        #endregion

        #region Method :: vSaveSectionResponsibles
        /// <summary>
        /// Method to save responsible employees for each section (CSV mapping "SectionID:UserID,SectionID:UserID")
        /// </summary>
        public void vSaveSectionResponsibles()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.SectionAssignments.Trim().Equals(string.Empty))
            {
                throw new Exception("SectionAssignments (CSV) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSectionAssignments", this.SectionAssignments.Trim(), DBConnector.DBTypes.NVarChar);
                if (!this.nCreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pCreatedBy", this.nCreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveSectionResponsibles");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        // ---------------- UC.4: نموذج استلام (مسؤول التغذية) ----------------

        #region Method :: oArrGetReceiptForms
        /// <summary>
        /// This function is used to get Receipt Forms
        /// </summary>
        public FeedingModel[] oArrGetReceiptForms()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliReceipts = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    if (!this.ReceiptID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pReceiptID", this.ReceiptID, DBConnector.DBTypes.Int);
                    }
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetReceiptForms");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.ReceiptID = oReader.FieldExists("ReceiptID") ? Convert.ToInt32(oReader["ReceiptID"]) : -99;

                            it.ID = oReader.FieldExists("ModelID") ? Convert.ToInt32(oReader["ModelID"]) : -99;
                            it.ModelType = oReader.FieldExists("ModelType") ? oReader["ModelType"].ToString() : string.Empty;
                            it.Notes = oReader.FieldExists("Notes") ? oReader["Notes"].ToString() : string.Empty;

                            it.SchoolID = oReader.FieldExists("SchoolID") ? Convert.ToInt32(oReader["SchoolID"]) : -99;
                            it.SchoolDesc = oReader.FieldExists("SchoolDesc") ? oReader["SchoolDesc"].ToString() : string.Empty;

                            it.QuantityReceived = oReader.FieldExists("QuantityReceived") ? Convert.ToInt32(oReader["QuantityReceived"]) : -99;
                            it.ReceiptDate = oReader.FieldExists("ReceiptDate") ? Convert.ToDateTime(oReader["ReceiptDate"]) : DateTime.MinValue;

                            it.ReceiptTimeID = oReader.FieldExists("ReceiptTimeID") ? Convert.ToInt32(oReader["ReceiptTimeID"]) : -99;
                            it.ReceiptTimeDesc = oReader.FieldExists("ReceiptTimeDesc") ? oReader["ReceiptTimeDesc"].ToString() : string.Empty;

                            it.ReceiptCreatedOn = oReader.FieldExists("CreatedOn") ? Convert.ToDateTime(oReader["CreatedOn"]) : DateTime.MinValue;

                            oliReceipts.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliReceipts.ToArray();
        }
        #endregion

        #region Method :: vSaveReceiptForm
        /// <summary>
        /// Method to save (Create/Upsert) a Receipt Form
        /// </summary>
        public void vSaveReceiptForm()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.QuantityReceived.Equals(-99))
            {
                throw new Exception("QuantityReceived is required");
            }
            if (this.ReceiptDate.Equals(DateTime.MinValue))
            {
                throw new Exception("ReceiptDate is required");
            }
            if (this.ReceiptTimeID.Equals(-99))
            {
                throw new Exception("ReceiptTimeID is required");
            }
            if (this.CreatedBy.Equals(-99))
            {
                throw new Exception("CreatedBy is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                if (!this.ReceiptID.Equals(-99))
                {
                    oDBConnector.AddInParam("pReceiptID", this.ReceiptID, DBConnector.DBTypes.Int);
                }
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);

                oDBConnector.AddInParam("pQuantityReceived", this.QuantityReceived, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pReceiptDate", this.ReceiptDate, DBConnector.DBTypes.DateTime);
                oDBConnector.AddInParam("pReceiptTimeID", this.ReceiptTimeID, DBConnector.DBTypes.Int);

                oDBConnector.AddInParam("pCreatedBy", this.CreatedBy, DBConnector.DBTypes.Int);
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveReceiptForm");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vUpdateReceiptForm
        /// <summary>
        /// Method to update a Receipt Form (Explicit Update)
        /// </summary>
        public void vUpdateReceiptForm()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ReceiptID.Equals(-99))
            {
                throw new Exception("ReceiptID is required for update");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pReceiptID", this.ReceiptID, DBConnector.DBTypes.Int);

                if (!this.SchoolID.Equals(-99))
                {
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                }
                if (!this.ID.Equals(-99))
                {
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                }
                if (!this.QuantityReceived.Equals(-99))
                {
                    oDBConnector.AddInParam("pQuantityReceived", this.QuantityReceived, DBConnector.DBTypes.Int);
                }
                if (!this.ReceiptDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pReceiptDate", this.ReceiptDate, DBConnector.DBTypes.DateTime);
                }
                if (!this.ReceiptTimeID.Equals(-99))
                {
                    oDBConnector.AddInParam("pReceiptTimeID", this.ReceiptTimeID, DBConnector.DBTypes.Int);
                }
                if (!this.CreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pModifiedBy", this.CreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_UpdateReceiptForm");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vDeleteReceiptForm
        /// <summary>
        /// Method to delete a Receipt Form
        /// </summary>
        public void vDeleteReceiptForm()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ReceiptID.Equals(-99))
            {
                throw new Exception("ReceiptID is required for delete");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pReceiptID", this.ReceiptID, DBConnector.DBTypes.Int);
                if (!this.CreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pDeletedBy", this.CreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_DeleteReceiptForm");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        // ---------------- UC.5: نموذج توزيع يومي (طلاب الشعبة) ----------------

        #region Method :: oArrGetDistributionStudents
        /// <summary>
        /// This function is used to get students for the distribution grid by date, class and section
        /// </summary>
        public FeedingModel[] oArrGetDistributionStudents()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.ClassID.Equals(-99))
            {
                throw new Exception("ClassID is required");
            }
            if (this.SectionID.Equals(-99))
            {
                throw new Exception("SectionID is required");
            }
            if (this.DistributionDate.Equals(DateTime.MinValue))
            {
                throw new Exception("DistributionDate is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliStudents = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pClassID", this.ClassID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSectionID", this.SectionID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pDistributionDate", this.DistributionDate, DBConnector.DBTypes.DateTime);
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetDistributionStudents");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.StudentID = oReader.FieldExists("StudentID") ? Convert.ToInt32(oReader["StudentID"]) : -99;
                            it.StudentName = oReader.FieldExists("StudentName") ? oReader["StudentName"].ToString() :
                                             (oReader.FieldExists("FullName") ? oReader["FullName"].ToString() : string.Empty);

                            it.IsReceived = oReader.FieldExists("IsReceived") ? Convert.ToInt32(oReader["IsReceived"]) : 1; // default selected
                            it.StudentStatusID = oReader.FieldExists("StudentStatusID") ? Convert.ToInt32(oReader["StudentStatusID"]) : 1; // default Received
                            it.StudentStatusDesc = oReader.FieldExists("StudentStatusDesc") ? oReader["StudentStatusDesc"].ToString() : string.Empty;

                            it.DistributionID = oReader.FieldExists("DistributionID") ? Convert.ToInt32(oReader["DistributionID"]) : -99;
                            it.DistributionDate = oReader.FieldExists("DistributionDate") ? Convert.ToDateTime(oReader["DistributionDate"]) : this.DistributionDate;

                            oliStudents.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliStudents.ToArray();
        }
        #endregion

        #region Method :: vSaveDistribution
        /// <summary>
        /// Method to save (Create/Upsert) distribution statuses for students (CSV mapping "StudentID:StatusID")
        /// </summary>
        public void vSaveDistribution()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }
            if (this.ClassID.Equals(-99))
            {
                throw new Exception("ClassID is required");
            }
            if (this.SectionID.Equals(-99))
            {
                throw new Exception("SectionID is required");
            }
            if (this.DistributionDate.Equals(DateTime.MinValue))
            {
                throw new Exception("DistributionDate is required");
            }
            if (this.StudentStatuses.Trim().Equals(string.Empty))
            {
                throw new Exception("StudentStatuses (CSV) is required");
            }
            if (this.CreatedBy.Equals(-99))
            {
                throw new Exception("CreatedBy is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                if (!this.DistributionID.Equals(-99))
                {
                    oDBConnector.AddInParam("pDistributionID", this.DistributionID, DBConnector.DBTypes.Int);
                }
                oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pClassID", this.ClassID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSectionID", this.SectionID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pDistributionDate", this.DistributionDate, DBConnector.DBTypes.DateTime);

                oDBConnector.AddInParam("pStudentStatuses", this.StudentStatuses.Trim(), DBConnector.DBTypes.NVarChar);

                oDBConnector.AddInParam("pCreatedBy", this.CreatedBy, DBConnector.DBTypes.Int);
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveDistribution");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: vUpdateDistribution
        /// <summary>
        /// Method to update distribution (Explicit Update)
        /// </summary>
        public void vUpdateDistribution()
        {
            if (this.DBConnectionString.Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.DistributionID.Equals(-99))
            {
                throw new Exception("DistributionID is required for update");
            }
            if (this.StudentStatuses.Trim().Equals(string.Empty))
            {
                throw new Exception("StudentStatuses (CSV) is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                #region Parameters
                oDBConnector.AddInParam("pDistributionID", this.DistributionID, DBConnector.DBTypes.Int);

                if (!this.ID.Equals(-99))
                {
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                }
                if (!this.SchoolID.Equals(-99))
                {
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                }
                if (!this.ClassID.Equals(-99))
                {
                    oDBConnector.AddInParam("pClassID", this.ClassID, DBConnector.DBTypes.Int);
                }
                if (!this.SectionID.Equals(-99))
                {
                    oDBConnector.AddInParam("pSectionID", this.SectionID, DBConnector.DBTypes.Int);
                }
                if (!this.DistributionDate.Equals(DateTime.MinValue))
                {
                    oDBConnector.AddInParam("pDistributionDate", this.DistributionDate, DBConnector.DBTypes.DateTime);
                }

                oDBConnector.AddInParam("pStudentStatuses", this.StudentStatuses.Trim(), DBConnector.DBTypes.NVarChar);

                if (!this.CreatedBy.Equals(-99))
                {
                    oDBConnector.AddInParam("pModifiedBy", this.CreatedBy, DBConnector.DBTypes.Int);
                }
                #endregion

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_UpdateDistribution");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                    {
                        this.oDBConnector.Close();
                    }
                }
            }
        }
        #endregion

        #region Method :: oArrGetDistributions
        /// <summary>
        /// This function is used to get saved distributions (header or student-level) for view/export
        /// </summary>
        public FeedingModel[] oArrGetDistributions()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.ID.Equals(-99))
            {
                throw new Exception("ModelID (ID) is required");
            }
            if (this.SchoolID.Equals(-99))
            {
                throw new Exception("SchoolID is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliDistributions = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.ClassID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pClassID", this.ClassID, DBConnector.DBTypes.Int);
                    }
                    if (!this.SectionID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pSectionID", this.SectionID, DBConnector.DBTypes.Int);
                    }
                    if (!this.DistributionID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pDistributionID", this.DistributionID, DBConnector.DBTypes.Int);
                    }
                    if (!this.DistributionDate.Equals(DateTime.MinValue))
                    {
                        oDBConnector.AddInParam("pDistributionDate", this.DistributionDate, DBConnector.DBTypes.DateTime);
                    }
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetDistributions");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.DistributionID = oReader.FieldExists("DistributionID") ? Convert.ToInt32(oReader["DistributionID"]) : -99;
                            it.DistributionDate = oReader.FieldExists("DistributionDate") ? Convert.ToDateTime(oReader["DistributionDate"]) : DateTime.MinValue;

                            it.ClassID = oReader.FieldExists("ClassID") ? Convert.ToInt32(oReader["ClassID"]) : -99;
                            it.ClassDesc = oReader.FieldExists("ClassDesc") ? oReader["ClassDesc"].ToString() : string.Empty;

                            it.SectionID = oReader.FieldExists("SectionID") ? Convert.ToInt32(oReader["SectionID"]) : -99;
                            it.SectionDesc = oReader.FieldExists("SectionDesc") ? oReader["SectionDesc"].ToString() : string.Empty;

                            it.StudentID = oReader.FieldExists("StudentID") ? Convert.ToInt32(oReader["StudentID"]) : -99;
                            it.StudentName = oReader.FieldExists("StudentName") ? oReader["StudentName"].ToString() : string.Empty;

                            it.StudentStatusID = oReader.FieldExists("StudentStatusID") ? Convert.ToInt32(oReader["StudentStatusID"]) : -99;
                            it.StudentStatusDesc = oReader.FieldExists("StudentStatusDesc") ? oReader["StudentStatusDesc"].ToString() : string.Empty;

                            it.IsReceived = oReader.FieldExists("IsReceived") ? Convert.ToInt32(oReader["IsReceived"]) : -99;

                            oliDistributions.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliDistributions.ToArray();
        }
        #endregion

        // ---------------- UC.7: تقارير إحصائية شهرية على مستوى المديرية ----------------

        #region Method :: oArrGetDirectorateMonthlyReport
        /// <summary>
        /// UC.8 — التقارير الإحصائية على مستوى المديرية (شهري)
        /// This function is used to get monthly aggregated report per school for a given directorate.
        /// SP: SM_GetDirectorateMonthlyReport
        /// In: pDistID, pReportMonth (1..12), pReportYear, [pModelID], [pLanguageID]
        /// Out (per row): SchoolID, SchoolDesc, TotalStudents, ReceivedCount, AbsentCount, RefusedCount, DistributedCount
        /// </summary>
        public FeedingModel[] oArrGetDirectorateMonthlyReport()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
            {
                throw new Exception("The DBConnectionString property is not set");
            }
            if (this.DistID.Equals(-99))
            {
                throw new Exception("DistID is required");
            }
            if (this.ReportMonth.Equals(-99))
            {
                throw new Exception("ReportMonth is required");
            }
            if (this.ReportYear.Equals(-99))
            {
                throw new Exception("ReportYear is required");
            }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliReports = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pDistID", this.DistID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pReportMonth", this.ReportMonth, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pReportYear", this.ReportYear, DBConnector.DBTypes.Int);

                    if (!this.ID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    }
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetDirectorateMonthlyReport");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.SchoolID = oReader.FieldExists("SchoolID") ? Convert.ToInt32(oReader["SchoolID"]) : -99;
                            it.SchoolDesc = oReader.FieldExists("SchoolDesc") ? oReader["SchoolDesc"].ToString() :
                                            (oReader.FieldExists("Name") ? oReader["Name"].ToString() : string.Empty);

                            it.TotalStudents = oReader.FieldExists("TotalStudents") ? Convert.ToInt32(oReader["TotalStudents"]) : -99;
                            it.ReceivedCount = oReader.FieldExists("ReceivedCount") ? Convert.ToInt32(oReader["ReceivedCount"]) : -99;
                            it.AbsentCount = oReader.FieldExists("AbsentCount") ? Convert.ToInt32(oReader["AbsentCount"]) : -99;
                            it.RefusedCount = oReader.FieldExists("RefusedCount") ? Convert.ToInt32(oReader["RefusedCount"]) : -99;
                            it.DistributedCount = oReader.FieldExists("DistributedCount") ? Convert.ToInt32(oReader["DistributedCount"]) : -99;

                            // Echo back context for convenience
                            it.DistID = this.DistID;
                            it.ReportMonth = this.ReportMonth;
                            it.ReportYear = this.ReportYear;

                            oliReports.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliReports.ToArray();
        }
        #endregion

        #region Method :: oArrGetSchoolMonthlyReport
        /// <summary>
        /// UC.8 — School-level monthly statistical report
        /// SP: SM_GetSchoolMonthlyReport
        /// In: pSchoolID, pReportMonth (1..12), pReportYear, [pModelID], [pLanguageID]
        /// Out (per row): SchoolID, SchoolDesc, [ClassID], [ClassDesc], [SectionID], [SectionDesc], TotalStudents, ReceivedCount, AbsentCount, RefusedCount, DistributedCount
        /// </summary>
        public FeedingModel[] oArrGetSchoolMonthlyReport()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty)) { throw new Exception("The DBConnectionString property is not set"); }
            if (this.SchoolID.Equals(-99)) { throw new Exception("SchoolID is required"); }
            if (this.ReportMonth.Equals(-99)) { throw new Exception("ReportMonth is required"); }
            if (this.ReportYear.Equals(-99)) { throw new Exception("ReportYear is required"); }

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliReports = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    #region Parameters
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pReportMonth", this.ReportMonth, DBConnector.DBTypes.Int);
                    oDBConnector.AddInParam("pReportYear", this.ReportYear, DBConnector.DBTypes.Int);

                    if (!this.ID.Equals(-99))
                    {
                        oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    }
                    if (!this.Languageid.Equals(-99))
                    {
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);
                    }
                    #endregion

                    #region GetFieldsFromReader
                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetSchoolMonthlyReport");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();

                            it.SchoolID = oReader.FieldExists("SchoolID") ? Convert.ToInt32(oReader["SchoolID"]) : this.SchoolID;
                            it.SchoolDesc = oReader.FieldExists("SchoolDesc") ? oReader["SchoolDesc"].ToString() :
                                            (oReader.FieldExists("Name") ? oReader["Name"].ToString() : string.Empty);

                            it.ClassID = oReader.FieldExists("ClassID") ? Convert.ToInt32(oReader["ClassID"]) : -99;
                            it.ClassDesc = oReader.FieldExists("ClassDesc") ? oReader["ClassDesc"].ToString() : string.Empty;

                            it.SectionID = oReader.FieldExists("SectionID") ? Convert.ToInt32(oReader["SectionID"]) : -99;
                            it.SectionDesc = oReader.FieldExists("SectionDesc") ? oReader["SectionDesc"].ToString() : string.Empty;

                            it.TotalStudents = oReader.FieldExists("TotalStudents") ? Convert.ToInt32(oReader["TotalStudents"]) : -99;
                            it.ReceivedCount = oReader.FieldExists("ReceivedCount") ? Convert.ToInt32(oReader["ReceivedCount"]) : -99;
                            it.AbsentCount = oReader.FieldExists("AbsentCount") ? Convert.ToInt32(oReader["AbsentCount"]) : -99;
                            it.RefusedCount = oReader.FieldExists("RefusedCount") ? Convert.ToInt32(oReader["RefusedCount"]) : -99;
                            it.DistributedCount = oReader.FieldExists("DistributedCount") ? Convert.ToInt32(oReader["DistributedCount"]) : -99;

                            it.ReportMonth = this.ReportMonth;
                            it.ReportYear = this.ReportYear;

                            oliReports.Add(it);
                        }

                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                    #endregion
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }

            return oliReports.ToArray();
        }
        #endregion

        // ---------------- UC.9: إرسال وعرض ومتابعة ملاحظات ولي الأمر ----------------

        #region Method :: oArrGetGuardianMealInfo
        /// <summary>
        /// UC.9 — صفحة ولي الأمر: إظهار معلومات النموذج/الوجبة ومكوناتها وفترة التوزيع.
        /// SP: SM_GetGuardianMealInfo
        /// In: pStudentID, [pSchoolID], [pLanguageID]
        /// Out: ModelID, StartDate, EndDate, ModelType, MealComponents
        /// </summary>
        public FeedingModel[] oArrGetGuardianMealInfo()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
                throw new Exception("The DBConnectionString property is not set");
            if (this.StudentID.Equals(-99))
                throw new Exception("StudentID is required");

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliInfo = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    oDBConnector.AddInParam("pStudentID", this.StudentID, DBConnector.DBTypes.Int);
                    if (!this.SchoolID.Equals(-99))
                        oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);

                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetGuardianMealInfo");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.ID = oReader.FieldExists("ModelID") ? Convert.ToInt32(oReader["ModelID"]) : -99;
                            it.StartDate = oReader.FieldExists("StartDate") ? Convert.ToDateTime(oReader["StartDate"]) : DateTime.MinValue;
                            it.EndDate = oReader.FieldExists("EndDate") ? Convert.ToDateTime(oReader["EndDate"]) : DateTime.MinValue;
                            it.ModelType = oReader.FieldExists("ModelType") ? oReader["ModelType"].ToString() : string.Empty;
                            it.MealComponents = oReader.FieldExists("MealComponents") ? oReader["MealComponents"].ToString() : string.Empty;
                            oliInfo.Add(it);
                        }
                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }
            return oliInfo.ToArray();
        }
        #endregion

        #region Method :: vSaveParentNote
        /// <summary>
        /// UC.9 — ولي الأمر: إرسال ملاحظة إلى مدير المدرسة.
        /// قيود: طول الملاحظة ≤ 300 حرف.
        /// SP: SM_SaveParentNote
        /// In: pModelID, pSchoolID, pStudentID, pParentUserID, pNoteText, pCreatedBy
        /// Out: pError
        /// </summary>
        public void vSaveParentNote()
        {
            if (this.DBConnectionString.Equals(string.Empty))
                throw new Exception("The DBConnectionString property is not set");
            if (this.SchoolID.Equals(-99))
                throw new Exception("SchoolID is required");
            if (this.StudentID.Equals(-99))
                throw new Exception("StudentID is required");
            if (this.ParentUserID.Equals(-99) && this.CreatedBy.Equals(-99))
                throw new Exception("ParentUserID or CreatedBy is required");
            if (this.ParentNoteText.Trim().Equals(string.Empty))
                throw new Exception("ParentNoteText is required");
            if (this.ParentNoteText.Length > 300)
                throw new Exception("ParentNoteText must be 300 characters or less");

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                if (!this.ID.Equals(-99))
                    oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pStudentID", this.StudentID, DBConnector.DBTypes.Int);

                int parentUser = !this.ParentUserID.Equals(-99) ? this.ParentUserID : this.CreatedBy;
                oDBConnector.AddInParam("pParentUserID", parentUser, DBConnector.DBTypes.Int);
                oDBConnector.AddInParam("pNoteText", this.ParentNoteText.Trim(), DBConnector.DBTypes.NVarChar);

                int creator = !this.CreatedBy.Equals(-99) ? this.CreatedBy : parentUser;
                oDBConnector.AddInParam("pCreatedBy", creator, DBConnector.DBTypes.Int);

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_SaveParentNote");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                        this.oDBConnector.Close();
                }
            }
        }
        #endregion

        #region Method :: oArrGetParentNotesForSchool
        /// <summary>
        /// UC.9 — مدير المدرسة: عرض ملاحظات أولياء الأمور.
        /// SP: SM_GetParentNotesForSchool
        /// In: pSchoolID, [pModelID], [pLanguageID]
        /// Out per row: ParentNoteID, ParentNoteDate, StudentID, StudentName, ParentNoteText, ParentNoteStatusID, ParentNoteStatusDesc, IsAcknowledged
        /// </summary>
        public FeedingModel[] oArrGetParentNotesForSchool()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
                throw new Exception("The DBConnectionString property is not set");
            if (this.SchoolID.Equals(-99))
                throw new Exception("SchoolID is required");

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliNotes = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    oDBConnector.AddInParam("pSchoolID", this.SchoolID, DBConnector.DBTypes.Int);
                    if (!this.ID.Equals(-99))
                        oDBConnector.AddInParam("pModelID", this.ID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);

                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetParentNotesForSchool");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.ParentNoteID = oReader.FieldExists("ParentNoteID") ? Convert.ToInt32(oReader["ParentNoteID"]) : -99;
                            it.ParentNoteDate = oReader.FieldExists("ParentNoteDate") ? Convert.ToDateTime(oReader["ParentNoteDate"]) : DateTime.MinValue;
                            it.StudentID = oReader.FieldExists("StudentID") ? Convert.ToInt32(oReader["StudentID"]) : -99;
                            it.StudentName = oReader.FieldExists("StudentName") ? oReader["StudentName"].ToString() : string.Empty;
                            it.ParentNoteText = oReader.FieldExists("ParentNoteText") ? oReader["ParentNoteText"].ToString() : string.Empty;
                            it.ParentNoteStatusID = oReader.FieldExists("ParentNoteStatusID") ? Convert.ToInt32(oReader["ParentNoteStatusID"]) : -99;
                            it.ParentNoteStatusDesc = oReader.FieldExists("ParentNoteStatusDesc") ? oReader["ParentNoteStatusDesc"].ToString() : string.Empty;
                            it.IsAcknowledged = oReader.FieldExists("IsAcknowledged") ? Convert.ToInt32(oReader["IsAcknowledged"]) : 0;
                            oliNotes.Add(it);
                        }
                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }
            return oliNotes.ToArray();
        }
        #endregion

        #region Method :: vAcknowledgeParentNote
        /// <summary>
        /// UC.9 — مدير المدرسة: وضع حالة "تم الاستلام" على الملاحظة لإشعار ولي الأمر.
        /// SP: SM_AcknowledgeParentNote
        /// In: pParentNoteID, pAcknowledgedBy
        /// Out: pError
        /// </summary>
        public void vAcknowledgeParentNote()
        {
            if (this.DBConnectionString.Equals(string.Empty))
                throw new Exception("The DBConnectionString property is not set");
            if (this.ParentNoteID.Equals(-99))
                throw new Exception("ParentNoteID is required");
            if (this.AcknowledgedByUserID.Equals(-99) && this.CreatedBy.Equals(-99))
                throw new Exception("AcknowledgedByUserID or CreatedBy is required");

            this.oDBConnector = new DBConnector(this.DBConnectionString);

            if (this.oDBConnector != null)
            {
                oDBConnector.AddInParam("pParentNoteID", this.ParentNoteID, DBConnector.DBTypes.Int);
                int ackBy = !this.AcknowledgedByUserID.Equals(-99) ? this.AcknowledgedByUserID : this.CreatedBy;
                oDBConnector.AddInParam("pAcknowledgedBy", ackBy, DBConnector.DBTypes.Int);

                this.oDBConnector.AddOutParam("pError", 0, DBConnector.DBTypes.Int);
                try
                {
                    this.oDBConnector.Open();
                    this.oDBConnector.InsertToDbWithStoredProcedure("SM_AcknowledgeParentNote");
                    this.nOperationResult = Convert.ToInt32(this.oDBConnector.GetParamValue("pError"));
                }
                finally
                {
                    if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                        this.oDBConnector.Close();
                }
            }
        }
        #endregion

        #region Method :: oArrGetParentNotesForGuardian
        /// <summary>
        /// UC.9 — ولي الأمر: متابعة حالة ملاحظاته (مثلاً: تم الاستلام).
        /// SP: SM_GetParentNotesForGuardian
        /// In: pStudentID, [pParentUserID], [pLanguageID]
        /// Out per row: ParentNoteID, ParentNoteDate, ParentNoteText, ParentNoteStatusID, ParentNoteStatusDesc, IsAcknowledged
        /// </summary>
        public FeedingModel[] oArrGetParentNotesForGuardian()
        {
            if (this.DBConnectionString.Trim().Equals(string.Empty))
                throw new Exception("The DBConnectionString property is not set");
            if (this.StudentID.Equals(-99))
                throw new Exception("StudentID is required");

            this.oDBConnector = new DBConnector(this.DBConnectionString);
            List<FeedingModel> oliNotes = new List<FeedingModel>();

            try
            {
                if (this.oDBConnector != null)
                {
                    oDBConnector.AddInParam("pStudentID", this.StudentID, DBConnector.DBTypes.Int);
                    if (!this.ParentUserID.Equals(-99))
                        oDBConnector.AddInParam("pParentUserID", this.ParentUserID, DBConnector.DBTypes.Int);
                    if (!this.Languageid.Equals(-99))
                        oDBConnector.AddInParam("pLanguageID", this.Languageid, DBConnector.DBTypes.Int);

                    this.oDBConnector.Open();
                    this.oDBConnector.BeginTransAction();
                    IDataReader oReader = this.oDBConnector.ReadDbWithStoredProcedureDataReader("SM_GetParentNotesForGuardian");

                    if (oReader != null)
                    {
                        while (oReader.Read())
                        {
                            FeedingModel it = new FeedingModel();
                            it.ParentNoteID = oReader.FieldExists("ParentNoteID") ? Convert.ToInt32(oReader["ParentNoteID"]) : -99;
                            it.ParentNoteDate = oReader.FieldExists("ParentNoteDate") ? Convert.ToDateTime(oReader["ParentNoteDate"]) : DateTime.MinValue;
                            it.ParentNoteText = oReader.FieldExists("ParentNoteText") ? oReader["ParentNoteText"].ToString() : string.Empty;
                            it.ParentNoteStatusID = oReader.FieldExists("ParentNoteStatusID") ? Convert.ToInt32(oReader["ParentNoteStatusID"]) : -99;
                            it.ParentNoteStatusDesc = oReader.FieldExists("ParentNoteStatusDesc") ? oReader["ParentNoteStatusDesc"].ToString() : string.Empty;
                            it.IsAcknowledged = oReader.FieldExists("IsAcknowledged") ? Convert.ToInt32(oReader["IsAcknowledged"]) : 0;
                            oliNotes.Add(it);
                        }
                        oReader.Close();
                        this.oDBConnector.CommitTransAction();
                        this.nOperationResult = this.oDBConnector.DBResult;
                    }
                }
            }
            finally
            {
                if (this.oDBConnector != null && this.oDBConnector.IsOpen)
                {
                    this.oDBConnector.Close();
                    this.oDBConnector.CommitTransAction();
                }
            }
            return oliNotes.ToArray();
        }
        #endregion

        #endregion
    }
}
