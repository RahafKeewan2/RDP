/*
File Name		: ManageAttendance.aspx.cs									    
Creation Date   : 02/04/2008                                                     
Created By	    : Ashraf Abdoh                                                   
Update Date     : 30/10/2009                                                     
Updated By      : Kayyali,Omar Abd ElhadI                                        
Comments		: set absent to student, reason, and view students absent history
Arabic Name     : «œŒ«· «·”·Êﬂ Ê«·„Ê«Ÿ»…                                         
*/

using EduWave.CustomControls;
using EduWave.K12.UsersManagementCom;
using EduWave.K12Lookups;
using EduWave.K12SMS.Distribution;
using EduWave.K12SMS.SMSCom;
using EduWave.K12SMS.Users;
using EduWave.SystemSettings;
using ITG_DBAccess;
using ITG_Utilities.ITGSecurity;
using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using UT = EduWave.K12.UsersManagementCom.UsersTypes;

namespace EduWave.K12SMS
{
    /// <summary>
    /// The manage attendance.
    /// </summary>
    public partial class ManageAttendance : EduWave.CustomControls.EduWaveWebPage
    {
        #region Designer
        /// <summary>
        /// oDistributionSearch control.
        /// </summary>
        /// <remarks>
        /// Auto-generated field.
        /// To modify move field declaration from designer file to code-behind file.
        /// </remarks>
        protected global::EduWave.K12UserControls.DistributionSearchUC oDistributionSearch;
        #endregion Designer

        #region Enumerations 

        #region Enum :: Print Mode
        /// <summary>
        /// Represent Prints Modes
        /// </summary>
        private enum enumPrintMode
        {
            Original = 1,
            Print = 2
        }
        #endregion

        #region Enum :: CurrentPrivs
        /// <summary>
        /// ‰Ê⁄ «·’·«ÕÌ…
        /// </summary>
        private enum CurrentPrivs
        {
            /// <summary>
            /// ⁄—÷ ﬂ«„· «·’·«ÕÌ«  «·„„ﬂ‰ „‰ÕÂ« ··„” Œœ„
            /// </summary>
            All = 0,
            /// <summary>
            /// ⁄—÷ «·’·«ÕÌ… «·„„‰ÊÕ… ··„” Œœ„ ›ﬁÿ
            /// </summary>
            GrantedPrivs = 1
        }
        #endregion

        #endregion Enumerations

        #region Private Members
        /// <summary>
        /// The O arr degree deduct.
        /// </summary>
        private DegreeDeductAmount[] oArrDegreeDeduct;
        /// <summary>
        /// The B check header.
        /// </summary>
        private bool bCheckHeader = true; // Used to check header checkbox in gridview violation when u check all checkboxes inside violation gridview programetically 
        /// <summary>
        /// The N row disabled count.
        /// </summary>
        private int nRowDisabledCount;
        /// <summary>
        /// The O arr attendance.
        /// </summary>
        Attendance[] oArrAttendance;
        /// <summary>
        /// The O arr violations.
        /// </summary>
        Violations[] oArrViolations;
        #endregion

        #region Properties

        #region Property :: School ID
        /// <summary>
        /// Gets or Sets School ID
        /// </summary>
        public int SchoolID
        {
            set
            {
                ViewState["SchoolID"] = value;
            }
            get
            {
                return Convert.ToInt32(ViewState["SchoolID"]);
            }
        }
        #endregion

        #region Property :: Student Profile ID
        /// <summary>
        /// Gets or Sets Student Profile ID to view his absences
        /// </summary>
        public int StudentProfileID
        {
            set
            {
                ViewState["StudentProfileID"] = value;
            }
            get
            {
                return Convert.ToInt32(ViewState["StudentProfileID"]);
            }
        }
        #endregion

        #region Property :: Student ID
        /// <summary>
        /// Gets or Sets Student ID to view his absences
        /// </summary>
        public int StudentID
        {
            set
            {
                ViewState["StudentID"] = value;
            }
            get
            {
                return ViewState["StudentID"] != null ? Convert.ToInt32(ViewState["StudentID"]) : -99;
            }
        }
        #endregion

        #region Property :: IsPrivilegedUser
        /// <summary>
        /// Gets or sets IsPrivilegedUser
        /// </summary>
        public bool IsPrivilegedUser
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsPrivilegedUser"]);
            }
            set
            {
                this.ViewState["IsPrivilegedUser"] = value;
            }
        }
        #endregion

        #region Property :: IsMarksEntryClosed
        /// <summary>
        /// Gets or sets Marks Entry status
        /// </summary>
        private bool IsMarksEntryClosed
        {
            get
            {
                if (ViewState["IsMarksEntryClosed"] == null)
                {
                    return false;
                }
                return Convert.ToBoolean(ViewState["IsMarksEntryClosed"]);
            }
            set
            {
                ViewState["IsMarksEntryClosed"] = value;
            }
        }
        #endregion

        #region Property :: Active Semster
        /// <summary>
        /// Gets or Sets Active Semster
        /// </summary>
        private int ActiveSemesterID
        {
            get
            {
                return Convert.ToInt32(ViewState["ActiveSemesterID"]);
            }
            set
            {
                ViewState["ActiveSemesterID"] = value;
            }
        }
        #endregion

        #region Property :: SemesterCodeID
        /// <summary>
        /// Gets or Sets SemesterCodeID
        /// </summary>
        private int SemesterCodeID
        {
            get
            {
                return Convert.ToInt32(ViewState["SemesterCodeID"]);
            }
            set
            {
                ViewState["SemesterCodeID"] = value;
            }
        }
        #endregion

        #region Property :: Print Mode
        /// <summary>
        /// for print
        /// </summary>
        private enumPrintMode PrintMode
        {
            set { ViewState["PrintMode"] = value; }
            get { return (enumPrintMode)ViewState["PrintMode"]; }
        }
        #endregion

        #region Property :: ByViolationLectur
        /// <summary>
        /// Gets or sets a value indicating whether violation lectur.
        /// </summary>
        private bool ByViolationLectur
        {
            get
            {
                if (ViewState["ByViolationLectur"] != null)
                {
                    return Convert.ToBoolean(ViewState["ByViolationLectur"].ToString());
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["ByViolationLectur"] = value;
            }
        }
        #endregion

        #region Property :: DateSemester
        /// <summary>
        /// Gets or sets the date semester.
        /// </summary>
        private string DateSemester
        {
            get
            {
                if (ViewState["DateSemester"] != null)
                {
                    return ViewState["DateSemester"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["DateSemester"] = value;
            }
        }
        #endregion

        #region Property :: MenuPageID
        /// <summary>
        /// Gets or Sets MenuPageID
        /// </summary>
        private int MenuPageID
        {
            set { ViewState["MenuPageID"] = value; }
            get { return ViewState["MenuPageID"] == null ? -99 : Convert.ToInt32(ViewState["MenuPageID"]); }
        }
        #endregion

        #region Property :: Search Criteria
        /// <summary>
        /// Search criteria string from the previous page
        /// string[0] = School Desc
        /// string[1] = SupervisionCenter
        /// string[2] = StudyLevel
        /// string[3] = School
        /// string[4] = DepartmentCategory
        /// string[5] = SchoolCategory
        /// string[6] = StudyTime
        /// string[7] = SystemType
        /// string[8] = RegionLevel2
        /// string[9] = RegionLevel3
        /// string[10] = RegionLevel4
        /// string[11] = RegionLevel5
        /// </summary>
        private string[] SearchCriteria
        {
            set
            {
                ViewState["SearchCriteria"] = value;
            }
            get
            {
                return ViewState["SearchCriteria"] == null ? new string[0] : (string[])ViewState["SearchCriteria"];
            }
        }
        #endregion

        #region Property :: SchoolDesc
        /// <summary>
        /// Gets or sets the school desc.
        /// </summary>
        private string SchoolDesc
        {
            get
            {
                if (ViewState["SchoolDesc"] != null)
                {
                    return ViewState["SchoolDesc"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                ViewState["SchoolDesc"] = value;
            }
        }
        #endregion

        #region Property :: BookMarkID
        /// <summary>
        /// Gets or Sets BookMarkID
        /// </summary>
        private int BookMarkID
        {
            set { ViewState["BookMarkID"] = value; }
            get { return ViewState["BookMarkID"] == null ? -99 : Convert.ToInt32(ViewState["BookMarkID"]); }
        }
        #endregion

        #region Property :: StudentID_DDl
        /// <summary>
        /// Gets or Sets Student ID to view his absences
        /// </summary>
        public string StudentID_DDl
        {
            set
            {
                ViewState["StudentID"] = value;
            }
            get
            {
                return ViewState["StudentID"] != null ? ViewState["StudentID"].ToString() : string.Empty;
            }
        }
        #endregion

        #region Property :: EnableEdit
        /// <summary>
        /// Gets or sets EnableEdit
        /// </summary>
        public bool EnableEdit
        {
            get
            {
                return Convert.ToBoolean(ViewState["EnableEdit"]);
            }
            set
            {
                ViewState["EnableEdit"] = value;
            }
        }
        #endregion

        #region Property :: EditAbsencePrivilege
        /// <summary>
        /// Gets or sets EditAbsencePrivilege
        /// </summary>
        public bool EditAbsencePrivilege
        {
            get
            {
                return Convert.ToBoolean(ViewState["EditAbsencePrivilege"]);
            }
            set
            {
                ViewState["EditAbsencePrivilege"] = value;
            }
        }
        #endregion

        #region Property :: AllowEditWeek
        /// <summary>
        /// Gets or sets AllowEditWeek
        /// </summary>
        public bool AllowEditWeek
        {
            get
            {
                return Convert.ToBoolean(ViewState["AllowEditWeek"]);
            }
            set
            {
                ViewState["AllowEditWeek"] = value;
            }
        }
        #endregion       

        #region Property :: AcademicYearID
        /// <summary>
        /// Gets or Sets AcademicYearID
        /// </summary>
        private int AcademicYearID
        {
            set { ViewState["AcademicYearID"] = value; }
            get { return ViewState["AcademicYearID"] == null ? -99 : Convert.ToInt32(ViewState["AcademicYearID"]); }
        }
        #endregion

        #region Property :: AllowEditForSchoolAdmin
        /// <summary>
        /// Gets or sets AllowEditForSchoolAdmin
        /// </summary>
        public bool AllowEditForSchoolAdmin
        {
            get
            {
                return Convert.ToBoolean(ViewState["AllowEditForSchoolAdmin"]);
            }
            set
            {
                ViewState["AllowEditForSchoolAdmin"] = value;
            }
        }
        #endregion

        #endregion Properties

        #region Page Load
        /// <summary>
        /// Page load.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            vSetPostBackControl();

            #region Handler BookMarks
            base.BookMarksButtonEvent += new BookMarksButton(Handler_AddBookMarksButtonEvent);
            #endregion

            Title = string.Format(GetGlobalResourceObject("CommonMessages", "PageTitle").ToString(), GetLocalResourceObject("PageTitle"));
            ConfirmYesButtonEvent += new ConfirmYesButton(NoViolation_ConfirmYesButtonEvent);

            #region Initialize the Page controls
            oStudentAttendanceUC.IsCalledByEduWave = true;

            #region Initialize Date Calendar
            clrAttendanceDay.ServerCulture = oEduSettings.ServerCulture;
            clrAttendanceDay.ClientCulture = oClientInfo.ClientCulture;
            #endregion

            #region GridView DataSource
            gvClassStudentsAttendance.DataSourceArrayMethod = oArrGetClassStudentsAttendances;
            gvStudentLectureVaiolations.DataSourceArrayMethod = oArrGetViolations;
            #endregion

            //Check if the Attendance Calendar did a PostBack to Reschedule the dates
            if (Request.Form["__EVENTTARGET"] != null)
            {
                string sPostBack = Request.Form["__EVENTTARGET"];
                if (sPostBack.Equals("StartCalendar", StringComparison.OrdinalIgnoreCase))
                {
                    clrAttendanceDay_TextChanged(sender, e);
                }
            }
            #endregion

            #region oDistributionSearch Index_Change
            oDistributionSearch.ddlSchoolIndex_Change += new EduWave.K12UserControls.DistributionSearchUC.ddlSchoolIndex(oDistributionSearch_ddlSectionIndex_Change);
            oDistributionSearch.ddlClassIndex_Change += new EduWave.K12UserControls.DistributionSearchUC.ddlClassIndex(oDistributionSearch_ddlClassIndex_Change);
            oDistributionSearch.ddlSpecialtyIndex_Change += new EduWave.K12UserControls.DistributionSearchUC.ddlSpecialtyIndex(oDistributionSearch_ddlSectionIndex_Change);
            oDistributionSearch.ddlSectionIndex_Change += new EduWave.K12UserControls.DistributionSearchUC.ddlSectionIndex(oDistributionSearch_ddlSectionIndex_Change);
            #endregion

            if (!IsPostBack)
            {
                ibtnSaveViolation.Attributes.Add("onclick", "return CheckValidator();");

                #region Print
                lblPrinterFriendlyMsg.Text = lblPrinterMsg.Text = GetGlobalResourceObject("CommonMessages", "ViewFriendlyVersion").ToString();
                lbtnPrinter.Attributes.Add("onclick", "return PrintPage('" + GetGlobalResourceObject("Designs", "PageDirection").ToString() + "');");
                PrintMode = enumPrintMode.Original;
                #endregion

                #region PageTitle
                PageTitle = GetLocalResourceObject("PageTitle").ToString();
                #endregion

                #region Fill DDLs
                vFillDeductTypeddl();
                vFillViolationsDDL();
                vFillMowadabaDDL();
                #endregion

                #region Query String > 0
                string sAttendanceDayValue = string.Empty;
                if (Request.QueryString != null && Request.QueryString.Count > 0)
                {
                    string sMenuPageID = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                    if (Request.QueryString[sMenuPageID] != null)
                    {
                        this.MenuPageID = int.Parse(ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sMenuPageID].ToString()));
                        this.PageIDBookMarks = this.MenuPageID;
                    }
                    #region SchoolID
                    string sSchoolID = ITGTextEncryption.QuerystringEncryptKey("SchoolID");
                    if (Request.QueryString[sSchoolID] != null)
                    {
                        SchoolID = Convert.ToInt32(ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sSchoolID]));
                    }
                    #endregion
                    #region SchoolDesc
                    string sSchoolDesc = ITGTextEncryption.QuerystringEncryptKey("SchoolDesc");
                    if (Request.QueryString[sSchoolDesc] != null)
                    {
                        SchoolDesc = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sSchoolDesc]).ToString();
                    }
                    #endregion
                    #region SearchCriteria
                    string sSearchCriteria = ITGTextEncryption.QuerystringEncryptKey("SearchCriteria");
                    if (Request.QueryString[sSearchCriteria] != null)
                    {
                        SearchCriteria = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sSearchCriteria]).ToString().Trim().Split('$');
                    }
                    #endregion
                    #region SpecialityID
                    string sSpecialityID = ITGTextEncryption.QuerystringEncryptKey("SpecialityID");
                    int nSpeciality = -99;
                    if (Request.QueryString[sSpecialityID] != null)
                    {
                        nSpeciality = Convert.ToInt32(ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sSpecialityID]));
                        oDistributionSearch.SpecialityID = nSpeciality;
                    }
                    #endregion
                    #region ClassID
                    string sClassID = ITGTextEncryption.QuerystringEncryptKey("ClassID");
                    if (Request.QueryString[sClassID] != null)
                    {
                        oDistributionSearch.ClassID = Convert.ToInt32(ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sClassID]));
                        DropDownList ddlClass = oDistributionSearch.FindControl("ddlClass") as DropDownList;
                        if (nSpeciality != -99)
                        {
                            vInitializeDistibutionUC();
                            string sClassIdWithSpeciality = string.Concat(oDistributionSearch.ClassID, ",", 1);
                            ddlClass.SelectedIndex = ddlClass.Items.IndexOf(ddlClass.Items.FindByValue(sClassIdWithSpeciality));
                            oDistributionSearch_ddlClassIndex_Change(null, null);
                            DropDownList ddlSpecialty = oDistributionSearch.FindControl("ddlSpecialty") as DropDownList;
                            ddlSpecialty.SelectedIndex = ddlSpecialty.Items.IndexOf(ddlSpecialty.Items.FindByValue(nSpeciality.ToString()));
                            oDistributionSearch_ddlSectionIndex_Change(null, null);
                        }
                    }
                    #endregion
                    #region SectionID
                    string sSectionID = ITGTextEncryption.QuerystringEncryptKey("SectionID");
                    if (Request.QueryString[sSectionID] != null)
                    {
                        string sSection = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sSectionID]);
                        oDistributionSearch.SectionID = Convert.ToInt32(sSection);
                        DropDownList ddlSection = oDistributionSearch.FindControl("ddlSection") as DropDownList;
                        ddlSection.SelectedIndex = ddlSection.Items.IndexOf(ddlSection.Items.FindByValue(sSection));
                        oDistributionSearch_ddlSectionIndex_Change(null, null);
                    }
                    #endregion
                    #region MuadabaCourseID
                    string sMuadabaCourseID = ITGTextEncryption.QuerystringEncryptKey("MuadabaCourseID");
                    if (Request.QueryString[sMuadabaCourseID] != null)
                    {
                        string sMuadabaCourse = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sMuadabaCourseID]);
                        ddlMowadaba.SelectedIndex = ddlMowadaba.Items.IndexOf(ddlMowadaba.Items.FindByValue(sMuadabaCourse));
                        ddlMowadaba_SelectedIndexChanged(ddlMowadaba, null);
                    }
                    #endregion
                    #region DeductTypeID
                    string sDeductTypeID = ITGTextEncryption.QuerystringEncryptKey("DeductTypeID");
                    if (Request.QueryString[sDeductTypeID] != null)
                    {
                        string sDeductType = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sDeductTypeID]);
                        ddlDeductType.SelectedIndex = ddlDeductType.Items.IndexOf(ddlDeductType.Items.FindByValue(sDeductType));
                        ddlDeductType_SelectedIndexChanged(null, null);
                    }
                    #endregion
                    #region MuadabaLevelID
                    string sMuadabaLevelID = ITGTextEncryption.QuerystringEncryptKey("MuadabaLevelID");
                    if (Request.QueryString[sMuadabaLevelID] != null)
                    {
                        string sMuadabaLevel = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sMuadabaLevelID]);
                        ddlViolation.SelectedIndex = ddlViolation.Items.IndexOf(ddlViolation.Items.FindByValue(sMuadabaLevel));
                        ddlViolation_SelectedIndexChanged(null, null);
                    }
                    #endregion
                    #region AttendanceDay
                    string sAttendanceDay = ITGTextEncryption.QuerystringEncryptKey("AttendanceDay");
                    if (Request.QueryString[sAttendanceDay] != null)
                    {
                        sAttendanceDayValue = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sAttendanceDay]);
                        clrAttendanceDay.Text = sAttendanceDayValue;
                        clrAttendanceDay_TextChanged(null, null);
                    }
                    #endregion
                    #region LectureID
                    string sLectureID = ITGTextEncryption.QuerystringEncryptKey("LectureID");
                    if (Request.QueryString[sLectureID] != null)
                    {
                        string sLecture = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sLectureID]);
                        ddlLecture.SelectedIndex = ddlLecture.Items.IndexOf(ddlLecture.Items.FindByValue(sLecture));
                    }
                    #endregion
                    #region StudentIdQueryString
                    string sStudentIdQueryString = ITGTextEncryption.QuerystringEncryptKey("StudentIdQueryString");
                    if (Request.QueryString[sStudentIdQueryString] != null)
                    {
                        string sStudentID = ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sStudentIdQueryString]);
                        vFillStudents();
                        ddlStudents.SelectedIndex = ddlStudents.Items.IndexOf(ddlStudents.Items.FindByValue(sStudentID));
                    }

                    #endregion
                    #region QueryString BookMarks 
                    string sKeyBookMarkID = ITGTextEncryption.QuerystringEncryptKey("BookMarkID");
                    if (Request.QueryString[sKeyBookMarkID] != null)
                    {
                        this.BookMarkID = Convert.ToInt32(ITGTextEncryption.QuerystringDecrypt(Request.QueryString[sKeyBookMarkID].ToString()));
                        {
                            string[] separators = { "&&" };
                            BookMarksCom oBookMarksCom = new BookMarksCom();
                            oBookMarksCom.DBConnectionString = oEduSettings.DBConnectionString;
                            BookMark oBookMark = new BookMark();
                            oBookMark = oBookMarksCom.GetDataPageBookMarksForUser(oClientInfo.UserProfileID, this.BookMarkID);
                            if (oBookMark != null)
                            {
                                try
                                {
                                    string sControlsIDWithValues = oBookMark.ControlsIDsWithValues;
                                    if (!string.IsNullOrEmpty(sControlsIDWithValues))
                                    {
                                        string[] arrControlsIDWithValues = sControlsIDWithValues.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                                        if (arrControlsIDWithValues != null && arrControlsIDWithValues.Length > 0)
                                        {
                                            foreach (string sItem in arrControlsIDWithValues)
                                            {
                                                string sResultItem = sItem.Split('=')[1];
                                                #region Set Data For Controls  
                                                if (!string.IsNullOrEmpty(sItem.Split('=')[1]))
                                                {
                                                    if (sItem.Split('=')[0].Equals("oDistributionSearch_ddlClass"))
                                                    {
                                                        if (Convert.ToInt32((sItem.Split('=')[1]).Split(',')[0]) > 0)
                                                        {
                                                            oDistributionSearch.ClassID = Convert.ToInt32((sItem.Split('=')[1]).Split(',')[0]);
                                                            oDistributionSearch.SpecialtyFlag = Convert.ToInt32((sItem.Split('=')[1]).Split(',')[1]);
                                                            oDistributionSearch_ddlClassIndex_Change(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("oDistributionSearch_ddlSpecialty"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1].Split(',')[0]) > 0)
                                                        {
                                                            oDistributionSearch.SpecialityID = Convert.ToInt32(sItem.Split('=')[1].Split(',')[0]);
                                                            oDistributionSearch.CustomSkillsFlag = Convert.ToInt32(sItem.Split('=')[1].Split(',')[1]);
                                                            oDistributionSearch_ddlSectionIndex_Change(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("oDistributionSearch_ddlSection"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            oDistributionSearch.SectionID = Convert.ToInt32(sItem.Split('=')[1]);
                                                            oDistributionSearch_ddlSectionIndex_Change(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("ddlMowadaba"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            ddlMowadaba.SelectedIndex = ddlMowadaba.Items.IndexOf(ddlMowadaba.Items.FindByValue(sItem.Split('=')[1]));
                                                            ddlMowadaba_SelectedIndexChanged(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("ddlDeductType"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            ddlDeductType.SelectedIndex = ddlDeductType.Items.IndexOf(ddlDeductType.Items.FindByValue(sItem.Split('=')[1]));
                                                            ddlDeductType_SelectedIndexChanged(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("ddlViolation"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            ddlViolation.SelectedIndex = ddlViolation.Items.IndexOf(ddlViolation.Items.FindByValue(sItem.Split('=')[1]));
                                                            ddlViolation_SelectedIndexChanged(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("clrAttendanceDay"))
                                                    {
                                                        if (!string.IsNullOrEmpty(sItem.Split('=')[1]))
                                                        {
                                                            clrAttendanceDay.Text = sItem.Split('=')[1];
                                                            clrAttendanceDay_TextChanged(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("ddlLecture"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            ddlLecture.SelectedIndex = ddlLecture.Items.IndexOf(ddlLecture.Items.FindByValue(sItem.Split('=')[1]));
                                                            ddlLecture_SelectedIndexChanged(null, null);
                                                        }
                                                    }
                                                    else if (sItem.Split('=')[0].Equals("ddlStudents"))
                                                    {
                                                        if (Convert.ToInt32(sItem.Split('=')[1]) > 0)
                                                        {
                                                            this.StudentID_DDl = sItem.Split('=')[1];
                                                        }
                                                    }
                                                }
                                                #endregion
                                            }
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    #endregion

                    if (Request.QueryString[sClassID] != null)
                    {
                        ibtnSearch_Click(null, null);
                    }
                    else
                    {
                        vInitializeDistibutionUC();
                        vFillStudents();
                    }
                }
                #endregion

                #region Without query string
                else
                {
                    vInitializeDistibutionUC();
                    vFillStudents();
                }
                #endregion
                SetValidatorsValidationGroup(oDistributionSearch, "SearchValidation");
                #region Check User Absence Entry Privileged
                bIsUserAbsenceEntryPrivileged();
                #endregion

                #region Is Privileged User
                if (!IsPrivilegedUser)
                {
                    #region Check User Privileges
                    ibtnBackFromOperation.Visible = (oClientInfo.UserType == UT.SchoolAdmin) || (oClientInfo.UserType == UT.AgentInSchool)
                     || oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector);
                    mvAttendance.SetActiveView(viewOperationResult);
                    lblUnPrivligaedUser.Text = GetLocalResourceObject("AbsencePrivilegesFailed").ToString();
                    tblDistributionUserControl.Visible = false;
                    return; // if the user is not authorized to use this page, return back
                    #endregion
                }
                else
                {
                    #region Is Marks Entry Closed
                    // moved here becuase there is no need to check this flag if the user is not authorized to use this page
                    IsMarksEntryClosed = bIsMarksEntryClosed();
                    #endregion

                    #region Check Marks Entry Closed Status
                    if (!IsMarksEntryClosed)
                    {
                        tblDistributionUserControl.Visible = true;
                        if (sAttendanceDayValue.Equals(string.Empty)) // shasan : query string return Selected date so should not be the date of today :)
                        {
                            clrAttendanceDay.Text = DateTime.Now.ToShortDateString();
                        }
                    }
                    else
                    {
                        ibtnBackFromOperation.Visible = (oClientInfo.UserType == UsersTypes.SchoolAdmin) || (oClientInfo.UserType == UsersTypes.AgentInSchool)
                         || oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector);
                        mvAttendance.SetActiveView(viewOperationResult);
                        lblUnPrivligaedUser.Text = GetLocalResourceObject("MarksEntryPeriodClosed").ToString();
                        tblDistributionUserControl.Visible = false;
                    }
                    #endregion
                }
                #endregion

                #region Change GridView Header Text
                // This will initiliaze user information user control when the grid of Absence history is 
                // on sorting, to keep user information user control binded.
                if (this.StudentID != -99 && tblDistributionUserControl.Visible == false || ddlViolation.SelectedValue.Equals("2") && tblDistributionUserControl.Visible == false)
                {
                    vInitiliazeUserInformationUserControl();
                }
                // ⁄‰œ„«  Œ «— ‰Ê⁄ «· ﬁÊÌ„ «ÌÃ«»Ì… «⁄„· ⁄‰Ê«‰ «·⁄„Êœ ‰Ê⁄ «·«ÌÃ«»Ì… »œ· ‰Ê⁄ «·„Œ«·›… 
                if (ddlMowadaba.SelectedValue.Equals("1") && ddlDeductType.SelectedValue.Equals("1"))
                {
                    gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("ViolationHeaderText").ToString();
                }
                else if (ddlMowadaba.SelectedValue.Equals("1") && ddlDeductType.SelectedValue.Equals("2"))
                {
                    gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("PositiveType").ToString();
                }
                else if (ddlMowadaba.SelectedValue.Equals("2"))
                {
                    gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("ViolationType").ToString();
                }

                if (nGetActiveSemster().Equals(-100)) // Handle on Sort 
                {
                    gvClassStudentsAttendance.Columns[0].Visible = false;
                    ibtnSave.Visible = false;
                }
                else
                {
                    ibtnSave.Visible = gvClassStudentsAttendance.Rows.Count > 0 && PrintMode == enumPrintMode.Original && ((this.AllowEditForSchoolAdmin && AllowEditWeek) || EditAbsencePrivilege);
                }
                #endregion

                if (oClientInfo.UserType.Equals(UT.Teacher))
                {
                    vInsertUseUserServices(oClientInfo.UserTypeID);
                }
                if (oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School)
                    || oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector)
                {
                    vFillDateOfSelectSemester();
                    string sDate = ITG_Utilities.ITGDateTime.sGetDateInClientFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, DateTime.Now.ToShortDateString());
                    if (oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                    {
                        EnabledDivSectionHeaderTools = EnabledDivInstructionsMain = true;
                        InstructionsMainContent = string.Format(GetLocalResourceObject("Instruction").ToString(), sDate);
                    }
                    else
                    {
                        sDate = ITG_Utilities.ITGDateTime.sGetDateInClientFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, clrAttendanceDay.Text);
                    }
                    tblNoViolationConfirmation.Visible = true;
                    lblNoViolationConfirmation.Text = string.Format(lblNoViolationConfirmation.Text, sDate);
                    vShowNoViolationConfirmation();
                }
                ibtnSave.Attributes.Add("onclick", "return CheckAgreement1();");
                ibtnSaveViolation.Attributes.Add("onclick", "return CheckAgreement2();");
            }
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnSave);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnSearch);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnBackMain);

            ibtnSearch.Attributes.Add("onclick", "return vCheckToFromDateWithSemesterDDL('" + clrAttendanceDay.CalendarDateTimeFormat + "','" + DateSemester + "');");

            gvClassStudentsAttendance.Columns[7].Visible = false;
        }
        #endregion Page Load

        #region Methods
        #region SetValidatorsValidationGroup
        /// <summary>
        /// Recursively finds all BaseValidator controls within a parent control 
        /// and assigns them to a specified validation group.
        /// </summary>
        /// <param name="parentControl">The starting control to search within (e.g., your UserControl).</param>
        /// <param name="validationGroup">The name of the validation group to assign.</param>
        private void SetValidatorsValidationGroup(Control parentControl, string validationGroup)
        {
            foreach (Control childControl in parentControl.Controls)
            {
                // If the control is a validator, set its validation group
                if (childControl is BaseValidator)
                {
                    ((BaseValidator)childControl).ValidationGroup = validationGroup;
                }

                // If the control has its own children, search them recursively
                if (childControl.HasControls())
                {
                    SetValidatorsValidationGroup(childControl, validationGroup);
                }
            }
        }
        #endregion SetValidatorsValidationGroup

        #region Method :: bIsUserAbsenceEntry
        /// <summary>
        /// Validate user weather Is Privileged User or not
        /// </summary>
        public void bIsUserAbsenceEntryPrivileged()
        {
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                IsPrivilegedUser = true;
            }
            else
            {
                SMSCom.AttendanceAssociation oAttendanceAssociation = new EduWave.K12SMS.SMSCom.AttendanceAssociation();
                oAttendanceAssociation.LanguageID = this.oClientInfo.LanguageID;
                oAttendanceAssociation.DBConnectionString = oEduSettings.DBConnectionString;
                oAttendanceAssociation.HierarchyID = this.oClientInfo.SchoolID;
                oAttendanceAssociation.oAttendanceAssociationTypes = EduWave.K12SMS.SMSCom.AttendanceAssociation.AttendanceAssociationTypes.AbcenceAssocciationUsers;
                oAttendanceAssociation.UserProfileID = oClientInfo.UserProfileID;
                IsPrivilegedUser = (oAttendanceAssociation.oArrGetAttendanceAssociation().Length > 0);
            }
        }
        #endregion

        #region Method :: vFillDeductTypeddl
        /// <summary>
        /// fill ddlDeductType 
        /// </summary>
        private void vFillDeductTypeddl()
        {
            DDLHandler oDDLHandler = new DDLHandler(oEduSettings.DBConnectionString, oClientInfo.LanguageID);
            oDDLHandler.vFillDeductTypeDDL(ddlDeductType, -99, DDLHandler.DDLOption.Select);
            ddlDeductType.SelectedIndex = 1;
        }
        #endregion

        #region Method :: Get Muadaba Level
        /// <summary>
        /// Get Muadaba Level
        /// </summary>
        /// <returns>Array of DegreeDeductAmount</returns>
        private DegreeDeductAmount[] oArrGetMuadabaLevel()
        {
            DegreeDeductAmount oDegreeDeductAmount = new DegreeDeductAmount();
            oDegreeDeductAmount.DBConnectionString = oEduSettings.DBConnectionString;
            oDegreeDeductAmount.LanguageID = oClientInfo.LanguageID;
            return oDegreeDeductAmount.oArrGetMuadabaLevel();
        }
        #endregion

        #region Method :: Get Class Students Attendances
        /// <summary>
        /// This method view the students of a specific class with indication of thier
        /// absence on a specific date.
        /// </summary>
        /// <returns></returns>
        private Attendance[] oArrGetClassStudentsAttendances()
        {
            ActiveSemesterID = nGetActiveSemster();
            gvClassStudentsAttendance.Columns[gvClassStudentsAttendance.Columns.Count - 1].Visible = oClientInfo.UserType == UT.SchoolAdmin;
            gvClassStudentsAttendance.Columns[6].Visible = ddlDeductType.SelectedValue == "1" && ddlMowadaba.SelectedValue == "1";
            if (ActiveSemesterID != -99)
            {
                oArrAttendance = oArrGetAttendance(!string.IsNullOrEmpty(ddlStudents.SelectedValue) && !ddlStudents.SelectedValue.Equals("-99") ? Convert.ToInt32(ddlStudents.SelectedValue) : -99);

                if (oArrAttendance.Length > 0)
                {
                    EditAbsencePrivilege = Convert.ToBoolean(oArrAttendance[0].AllowEdit);
                    AllowEditWeek = Convert.ToBoolean(oArrAttendance[0].AllowEditWeek);
                }
                return oArrAttendance;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Method :: oArrGetAttendance
        /// <summary>
        /// Os arr get attendance.
        /// </summary>
        /// <param name="nStdProfileID">The n std profile ID.</param>
        /// <returns>An array of Attendances</returns>
        private Attendance[] oArrGetAttendance(int nStdProfileID)
        {
            Attendance oAttendance = new Attendance();
            oAttendance.DBConnectionString = oEduSettings.DBConnectionString;
            oAttendance.LanguageID = oClientInfo.LanguageID;
            if (!nStdProfileID.Equals(-99))
            {
                oAttendance.StudentProfileID = nStdProfileID;
            }
            oAttendance.AbsenceDate = clrAttendanceDay.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(clrAttendanceDay.Text);
            oAttendance.StudentsSection.SchoolID = oDistributionSearch.SchoolID;
            oAttendance.StudentsSection.SpecialtyID = oDistributionSearch.SpecialityID.ToString();
            oAttendance.StudentsSection.ClassID = oDistributionSearch.ClassID.ToString();
            oAttendance.StudentsSection.SectionID = oDistributionSearch.SectionID.ToString();
            oAttendance.ActiveSemetser = ActiveSemesterID;
            oAttendance.AbsencesFlag = Attendance.AbsenceFlag.AbsenseStudent;
            oAttendance.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            if (ddlMowadaba.SelectedValue.Equals("1"))
            {
                oAttendance.DeductTypeID = Convert.ToInt32(ddlDeductType.SelectedValue);
            }
            return oAttendance.oArrGetAllStudentsAttendances();
        }
        #endregion

        #region Method :: Get Student All Absences History
        /// <summary>
        /// This method used to get all the selected stude Absences
        /// </summary>
        /// <returns>Arry of objects of type "Attendance"</returns>
        private Attendance[] oArrGetStudentAttendancesHistory()
        {
            Attendance oAttendance = new Attendance();
            oAttendance.DBConnectionString = oEduSettings.DBConnectionString;
            oAttendance.LanguageID = oClientInfo.LanguageID;
            oAttendance.ActiveSemetser = ActiveSemesterID;
            oAttendance.StudentProfileID = StudentProfileID;
            oAttendance.AbsencesFlag = Attendance.AbsenceFlag.AbsenseStudentHistoey;
            oAttendance.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            return oAttendance.oArrGetStudentAttendancesHistory();
        }
        #endregion

        #region Method :: vInitializeDistibutionUC
        /// <summary>
        /// Vs initialize distibution UC.
        /// </summary>
        private void vInitializeDistibutionUC()
        {
            oDistributionSearch.MinistryID = oClientInfo.MinistryID;
            oDistributionSearch.MinistryDescription = oClientInfo.MinistryName;
            oDistributionSearch.EnableMinistry = false;
            oDistributionSearch.EnableClass = true;
            oDistributionSearch.EnableSection = true;
            oDistributionSearch.enumFilterSectionEntry = EduWave.K12UserControls.DistributionSearchUC.SectionFilterType.TeacherAbsenceSections;
            oDistributionSearch.EnableSpeciality = true;
            if (ddlViolation.SelectedValue.Equals("2") && ddlMowadaba.SelectedValue.Equals("2"))
            {
                oDistributionSearch.SpecialtyDDLValidation = true;
                oDistributionSearch.SectionDDLValidation = true;
            }
            else
            {
                oDistributionSearch.SpecialtyDDLValidation = false;
                oDistributionSearch.SectionDDLValidation = true;
            }
            oDistributionSearch.DistrictID = oClientInfo.DistrictID;
            oDistributionSearch.DistrictDescription = oClientInfo.DistrictName;
            oDistributionSearch.EnableDistrict = false;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oDistributionSearch.SchoolID = SchoolID;
                oDistributionSearch.SchoolDescription = SchoolDesc;
            }
            else
            {
                oDistributionSearch.SchoolID = oClientInfo.SchoolID;
                oDistributionSearch.SchoolDescription = oClientInfo.SchoolName;
            }
            oDistributionSearch.EnableSchool = false;
            oDistributionSearch.SchoolDDLValidation = false;
            oDistributionSearch.enumDDLOption = EduWave.K12UserControls.DistributionSearchUC.DDLOption.SectionNotRequired;
            oDistributionSearch.enumFilterAbsenceEntry = EduWave.K12UserControls.DistributionSearchUC.FilterAbsenceEntry.FilterAbsenceEntryMode;
            oDistributionSearch.StudySystemID = -99;
            oDistributionSearch.LabelWidth = 90;
            oDistributionSearch.vInitilizeDistribution();
        }
        #endregion

        #region Method :: vShowAttendanceReportViewer
        /// <summary>
        /// Show an empty attendance report
        /// </summary>
        private void ShowAttendanceReportViewer()
        {
            ReportPrepare oReportPrepare = new ReportPrepare(oEduSettings.DBConnectionString);

            #region Set Report Paremeter
            oReportPrepare.LanguageID = oClientInfo.LanguageID;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oReportPrepare.vAddParameter("pSchoolID", oDistributionSearch.SchoolID, DBConnector.DBTypes.Int);
            }
            else
            {
                oReportPrepare.vAddParameter("pSchoolID", oClientInfo.SchoolID, DBConnector.DBTypes.Int);
            }
            if (!this.SemesterCodeID.Equals(-99))
            {
                oReportPrepare.vAddParameter("pSemesterCodeID", this.SemesterCodeID, DBConnector.DBTypes.Int);
            }
            if (oDistributionSearch.ClassID != -99)
            {
                oReportPrepare.vAddParameter("pClassID", oDistributionSearch.ClassID, DBConnector.DBTypes.Int);
            }
            if (oDistributionSearch.SpecialityID != -1 && oDistributionSearch.SpecialityID != -99)
            {
                oReportPrepare.vAddParameter("pSpecialtyID", oDistributionSearch.SpecialityID, DBConnector.DBTypes.Int);
            }
            if (oDistributionSearch.SectionID != -99)
            {
                oReportPrepare.vAddParameter("pSectionID", oDistributionSearch.SectionID, DBConnector.DBTypes.Int);
            }
            oReportPrepare.vAddParameter("pEmptyFlag", 1, DBConnector.DBTypes.Int);

            oReportPrepare.StoredProcedureName = "GetStudentDailyAbsenceRPT";
            oReportPrepare.LogoImagePath = this.GetFileFullVirtualPath("images/Common/CustomerLogoBig.jpg");

            DataTable dtReportSource = oReportPrepare.dtGetReportSource();
            #endregion

            #region Validate Report DataSource
            if (dtReportSource != null && dtReportSource.Rows.Count > 0)
            {
                rvFollowUpRevealedAbsence.Visible = true;
                lblNoActiveSemester.Visible = false;
            }
            else
            {
                rvFollowUpRevealedAbsence.Visible = false;
                lblNoActiveSemester.Text = GetGlobalResourceObject("CommonMessages", "NoDataFound").ToString();
                lblNoActiveSemester.Visible = true;
                return;
            }
            #endregion

            #region Formating and Casting Hijri Date
            CultureInfo EnglishCulture = new CultureInfo(oEduSettings.ServerCulture);
            bool bConvert = true;
            DateTime dtiHajiriDate = DateTime.MinValue;
            int nRowCount = dtReportSource.Rows.Count;
            for (int i = 0; i < nRowCount; i++)
            {
                if (dtReportSource.Rows[i]["HijriDate"] != null && dtReportSource.Rows[i]["HijriDate"] != DBNull.Value)
                {
                    bConvert = DateTime.TryParse(dtReportSource.Rows[i]["HijriDate"].ToString(), EnglishCulture, DateTimeStyles.None, out dtiHajiriDate);
                    if (bConvert)
                    {
                        dtReportSource.Rows[i]["HijriDate"] = dtiHajiriDate.ToShortDateString();
                    }
                }
            }
            #endregion

            rvFollowUpRevealedAbsence.LocalReport.DataSources.Clear();
            rvFollowUpRevealedAbsence.LocalReport.DataSources.Add(new ReportDataSource("ImagePath", oReportPrepare.dtImagePathSource()));
            rvFollowUpRevealedAbsence.LocalReport.DataSources.Add(new ReportDataSource("Main", dtReportSource));
            rvFollowUpRevealedAbsence.LocalReport.ReportPath = Server.MapPath("../EduWavek12Portal/Reports/FollowUpRevealedAbsence.rdlc");
            rvFollowUpRevealedAbsence.LocalReport.Refresh();

            dtReportSource.Dispose();
            dtReportSource = null;
        }
        #endregion

        #region Method :: Get Active Semseter
        /// <summary>
        /// return active semester
        /// </summary>
        /// <returns></returns>
        private int nGetActiveSemster()
        {
            if (oDistributionSearch.SpecialityID != -99 && oDistributionSearch.SpecialityID != -1 && oDistributionSearch.ClassID != -99 && oDistributionSearch.ClassID != -1)
            {
                EduWave.SystemSettings.LevelSemesters oLevelSemesters = new EduWave.SystemSettings.LevelSemesters();

                lblNoActiveSemester.Visible = true;
                EduWave.SystemSettings.LevelSemesters[] oArrLevelSemesters = null;
                oLevelSemesters.LanguageID = oClientInfo.LanguageID;
                oLevelSemesters.DBConnectionString = oEduSettings.DBConnectionString;
                oLevelSemesters.SpecialtyID = oDistributionSearch.SpecialityID;
                oLevelSemesters.ClassID = oDistributionSearch.ClassID;
                oLevelSemesters.SchoolCategoryID = oClientInfo.SchoolCategoryID;
                oLevelSemesters.ActiveSemesterID = 1;
                oArrLevelSemesters = oLevelSemesters.oArrGetLevelSemestersByStudyLevel();

                if (oArrLevelSemesters.Length == 0)
                {
                    lblNoActiveSemester.Text = GetGlobalResourceObject("CommonMessages", "NoActiveSemester").ToString();
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = GetGlobalResourceObject("CommonMessages", "NoActiveSemester").ToString();
                    return -99;
                }
                else
                {

                    this.ActiveSemesterID = oArrLevelSemesters[0].SemesterCodeID;
                    this.SemesterCodeID = this.ActiveSemesterID;

                    if (!bIsMarksEntryClosed())
                    {
                        lblNoActiveSemester.Visible = false;
                        return this.ActiveSemesterID;
                    }
                    else
                    {
                        lblNoDataFound.Visible = true;
                        lblNoDataFound.Text = GetLocalResourceObject("MarksEntryPeriodClosed").ToString();

                        return -100; // › —… «œŒ«· «·œ—Ã«  „€·ﬁ… 
                    }
                }
            }
            return -99;
        }
        #endregion

        #region Method :: Is Marks Entry Closed
        /// <summary>
        /// /// Check Closed For Õ«·… «·≈€·«ﬁ «·⁄«œÌ
        /// </summary>
        /// <returns></returns>
        private bool bIsMarksEntryClosed()
        {
            Marks oMarks = new Marks();
            oMarks.DBConnectionString = oEduSettings.DBConnectionString;
            oMarks.LanguageID = oClientInfo.LanguageID;
            oMarks.SemesterCodeID = this.SemesterCodeID;
            oMarks.ClassID = oDistributionSearch.ClassID;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oMarks.SchoolID = oDistributionSearch.SchoolID;
            }
            else
            {
                oMarks.SchoolID = oClientInfo.SchoolID;
            }
            Marks[] oArrClosedApproval = oMarks.oArrGetGradesApprovalByClass();
            bool bIsClosed = false;
            if (oArrClosedApproval != null && oArrClosedApproval.Length > 0)
            {
                bIsClosed = ((Marks.MarksApprovalFlag)(oArrClosedApproval[0].ApprovalID)) == Marks.MarksApprovalFlag.Closed;
            }
            return bIsClosed;
        }
        #endregion

        #region Method :: Fill Mowadaba
        /// <summary>
        /// This method used to fill the Mowadaba Based on Language ID
        /// </summary>
        private void vFillMowadabaDDL()
        {
            ddlMowadaba.Items.Clear();
            Mowadaba oMowadaba = new Mowadaba();
            oMowadaba.DBConnectionString = oEduSettings.DBConnectionString;
            oMowadaba.LanguageID = oClientInfo.LanguageID;
            oMowadaba.USERPROFILEID = oClientInfo.UserProfileID;
            oMowadaba.HIERARCHYID = oClientInfo.SchoolID;
            if (oDistributionSearch.ClassID != -99 && oDistributionSearch.SpecialityID != -99 && oDistributionSearch.SectionID != -99)
            {
                oMowadaba.CLASSID = oDistributionSearch.ClassID;
                oMowadaba.SPECIALTYID = oDistributionSearch.SpecialityID;
                oMowadaba.SECTIONID = oDistributionSearch.SectionID;
                Mowadaba[] oArrMowadaba = oMowadaba.oArrGetMowadabaCourses();
                if (oArrMowadaba != null)
                {
                    ddlMowadaba.DataTextField = "Description";
                    ddlMowadaba.DataValueField = "ID";
                    ddlMowadaba.DataSource = oMowadaba.oArrGetMowadabaCourses();
                    ddlMowadaba.DataBind();

                    ddlMowadaba.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListSelect").ToString(), "-99"));
                    ddlMowadaba.Enabled = true;
                }
                else
                {
                    ddlMowadaba.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
                    ddlMowadaba.Enabled = false;
                }
            }
            else
            {
                ddlMowadaba.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
                ddlMowadaba.Enabled = false;
            }

        }
        #endregion

        #region Method :: vFillDegreeDeductAmount
        /// <summary>
        /// This method used to fill the Degree Deduct Amount
        /// </summary>
        private void vFillDegreeDeductAmount()
        {
            DegreeDeductAmount oDegreeDeductAmount = new DegreeDeductAmount();
            oDegreeDeductAmount.DBConnectionString = oEduSettings.DBConnectionString;
            oDegreeDeductAmount.LanguageID = oClientInfo.LanguageID;
            oDegreeDeductAmount.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            oDegreeDeductAmount.MuadabaLevelID = Convert.ToInt32(ddlViolation.SelectedValue);
            if (ddlMowadaba.SelectedValue.ToString().Equals("1"))
            {
                oDegreeDeductAmount.DeductTypeID = Convert.ToInt32(ddlDeductType.SelectedValue);
            }
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oDegreeDeductAmount.GenderID = oClientInfo.DistrictGender;
            }
            else if (oClientInfo.UserType != UsersTypes.SchoolAdmin && oClientInfo.UserType != UsersTypes.AgentInSchool && oClientInfo.UserType != UsersTypes.Teacher && oClientInfo.UserType != UsersTypes.ExecutiveUsers)
            {
                oDegreeDeductAmount.GenderID = oClientInfo.SchoolGender;
            }
            if (oClientInfo.StudyLevelID != -99)
            {
                oDegreeDeductAmount.StudyLevelID = oClientInfo.StudyLevelID;
            }
            oDegreeDeductAmount.DeductStatusID = 1;
            this.oArrDegreeDeduct = oDegreeDeductAmount.oArrGetDegreeDeductAmount();
        }
        #endregion

        #region Method :: vSetPostBackControl
        /// <summary>
        /// Set Post BackC ontrol
        /// </summary>
        private void vSetPostBackControl()
        {
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnSearch);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(lbtnPrintEmptyRecord);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnBackFromattendanceReport);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnBackFromOperation);
            ScriptManager.GetCurrent(this).RegisterPostBackControl(ibtnBackFromStudentAbsences);
        }
        #endregion

        #region Method :: vInitiliazeUserInformationUserControl
        /// <summary>
        /// Initiliaze User Information User Control
        /// </summary>
        private void vInitiliazeUserInformationUserControl()
        {
            UserInformationsCC.ConnectionString = oEduSettings.DBConnectionString;
            UserInformationsCC.LanguageID = oClientInfo.LanguageID;
            UserInformationsCC.UserID = StudentID;
            UserInformationsCC.UserType = UsersTypes.Student;
            UserInformationsCC.UserProfileID = this.StudentProfileID;
            UserInformationsCC.vGetUserInfo();
        }
        #endregion

        #region Mehtod :: vBindStudentAttendanceUC
        /// <summary>
        /// Bind Student Attendance UC
        /// </summary>
        private void vBindStudentAttendanceUC()
        {
            oStudentAttendanceUC.StudentProfileID = this.StudentProfileID;
            oStudentAttendanceUC.ActiveSemesterID = this.ActiveSemesterID;
            oStudentAttendanceUC.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            oStudentAttendanceUC.ClassID = oDistributionSearch.ClassID;
            oStudentAttendanceUC.SpecialityID = oDistributionSearch.SpecialityID;
            oStudentAttendanceUC.StudySystemID = oDistributionSearch.StudySystemID;
            oStudentAttendanceUC.SemesterCodeID = SemesterCodeID;
            oStudentAttendanceUC.SectionID = oDistributionSearch.SectionID;
            oStudentAttendanceUC.SchoolID = oDistributionSearch.SchoolID;
            oStudentAttendanceUC.DeductTypeID = Convert.ToInt32(ddlDeductType.SelectedValue);
            oStudentAttendanceUC.ViolationDate = Convert.ToDateTime(clrAttendanceDay.Text);
            oStudentAttendanceUC.AllowPaging = PrintMode == enumPrintMode.Original;
            oStudentAttendanceUC.Flag = 5;
            oStudentAttendanceUC.ClassScheduleID = Convert.ToInt32(ddlLecture.SelectedValue);
            oStudentAttendanceUC.GradeType = ddlMowadaba.SelectedValue == "1" && ddlDeductType.SelectedValue == "1";
            oStudentAttendanceUC.vInitializeComponent();
        }
        #endregion

        #region Method :: Fill Violation System DDL
        /// <summary>
        /// V fill violations DDL.
        /// </summary>
        private void vFillViolationsDDL()
        {
            DegreeDeductAmount[] oArrMuadabaLevels = oArrGetMuadabaLevel();
            if (oArrMuadabaLevels != null && oArrMuadabaLevels.Length > 0)
            {
                foreach (DegreeDeductAmount oMuadabaLevel in oArrMuadabaLevels)
                {
                    ddlViolation.Items.Add(new ListItem(oMuadabaLevel.MuadabaLevelDscription, oMuadabaLevel.MuadabaLevelID.ToString()));
                }
            }
            else
            {
                ddlViolation.Enabled = false;
            }
            ddlViolation.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListSelect").ToString(), "-99"));
        }
        #endregion

        #region Method :: oArrGetViolations
        /// <summary>
        /// Get Array of Violations 
        /// </summary>
        /// <returns>
        /// Array of Violations 
        /// </returns>
        protected Violations[] oArrGetViolations()
        {
            gvStudentLectureVaiolations.Columns[3].Visible = oClientInfo.UserType == UT.SchoolAdmin;

            oArrViolations = oArrGetLectureViolations(!string.IsNullOrEmpty(ddlStudents.SelectedValue) && !ddlStudents.SelectedValue.Equals("-99") ? Convert.ToInt32(ddlStudents.SelectedValue) : -99);
            gvStudentLectureVaiolations.Visible = true;
            ibtnSaveViolation.Visible = oArrViolations.Length > 0;
            return oArrViolations;
        }
        #endregion

        #region Method :: oArrGetLectureViolations
        /// <summary>
        /// Os arr get lecture violations.
        /// </summary>
        /// <param name="nStdProfileID">The n std profile ID.</param>
        /// <returns>An array of Violations</returns>
        private Violations[] oArrGetLectureViolations(int nStdProfileID)
        {
            Violations oViolations = new Violations();
            oViolations.DBConnectionString = oEduSettings.DBConnectionString;
            oViolations.LanguageID = oClientInfo.LanguageID;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oViolations.SchoolID = oDistributionSearch.SchoolID;
            }
            else
            {
                oViolations.SchoolID = oClientInfo.SchoolID;
            }
            oViolations.ClassID = oDistributionSearch.ClassID;
            oViolations.SpecialtyID = oDistributionSearch.SpecialityID;
            oViolations.SectionID = oDistributionSearch.SectionID;
            oViolations.SemesterCodeID = SemesterCodeID;
            oViolations.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            oViolations.EnumViolationsFlags = Violations.EnumViolationFlag.LecturesViolationsForDegreeDeduct;
            oViolations.ViolationDate = Convert.ToDateTime(clrAttendanceDay.Text);
            oViolations.LectureID = Convert.ToInt32(ddlLecture.SelectedValue);
            if (!nStdProfileID.Equals(-99))
            {
                oViolations.StudentProfileID = nStdProfileID;
            }
            if (ByViolationLectur)
            {
                oViolations.Flag = 4;
                oViolations.ClassSceduleID = Convert.ToInt32(ddlLecture.SelectedValue);
            }
            return oViolations.oArrGetViolations();
        }
        #endregion

        #region Method :: Fill DDL Violation Type
        /// <summary>
        /// Fill DDL Violation Type In GridView
        /// </summary>
        /// <param name="ddlVaiolationType"></param>
        void vFillDDLViolationType(DropDownList ddlVaiolationType)
        {
            ddlVaiolationType.Items.Clear();

            int nDegreeDeductAmountLength = oArrDegreeDeduct.Length;
            if (nDegreeDeductAmountLength > 0)
            {
                for (int nIndex = 0; nIndex < nDegreeDeductAmountLength; nIndex++)
                {
                    ddlVaiolationType.Items.Add(new ListItem(oArrDegreeDeduct[nIndex].Description, oArrDegreeDeduct[nIndex].DeductID.ToString() + "," + oArrDegreeDeduct[nIndex].AbsenceTypeID.ToString()));
                }
                ddlVaiolationType.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListSelect").ToString(), "-99"));
                ddlViolation.Attributes.Add("Style", "disable=false;");
            }
            else
            {
                ddlVaiolationType.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
            }
        }
        #endregion

        #region Method :: Fill Lecture DDL
        /// <summary>
        /// Get all lecture and asign it to the DDL
        /// </summary>
        private void vFillLectureDDL()
        {
            ddlLecture.Items.Clear();
            EduWave.K12SMS.SMSCom.Violations oLectures = new EduWave.K12SMS.SMSCom.Violations();
            oLectures.DBConnectionString = oEduSettings.DBConnectionString; ;
            oLectures.LanguageID = oClientInfo.LanguageID;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oLectures.HierarchyID = oDistributionSearch.SchoolID;
            }
            else
            {
                oLectures.HierarchyID = oClientInfo.SchoolID;
            }
            oLectures.ClassID = oDistributionSearch.ClassID;
            oLectures.SpecialtyID = oDistributionSearch.SpecialityID;
            oLectures.SectionID = oDistributionSearch.SectionID;
            oLectures.LectureDate = Convert.ToDateTime(clrAttendanceDay.Text);
            EduWave.K12SMS.SMSCom.Violations[] oArrLectures = oLectures.oArrGetSchoolLecturesByDate();
            if (oArrLectures != null && oArrLectures.Length > 0)
            {
                int nRowsCount = oArrLectures.Length;
                for (int nRowIndex = 0; nRowIndex < nRowsCount; nRowIndex++)
                {
                    ddlLecture.Items.Add(new ListItem(oArrLectures[nRowIndex].LectureDescription, oArrLectures[nRowIndex].LectureID.ToString()));
                }
                DDLHandler.vDDLFormationg(ddlLecture, DDLHandler.DDLOption.Select);
            }
            else
            {
                DDLHandler.vDDLFormationg(ddlLecture, DDLHandler.DDLOption.None);
            }
        }
        #endregion

        #region Method :: FillStudents
        /// <summary>
        /// V fill students.
        /// </summary>
        private void vFillStudents()
        {
            DDLHandler oDDLHandler = new DDLHandler(oEduSettings.DBConnectionString, oClientInfo.LanguageID);
            int nSchoolID = -99;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                nSchoolID = oDistributionSearch.SchoolID;
            }
            else
            {
                nSchoolID = oClientInfo.SchoolID;
            }
            oDDLHandler.vFillStudentName(ddlStudents, nSchoolID, oDistributionSearch.ClassID, oDistributionSearch.SpecialityID, oDistributionSearch.SectionID, oDistributionSearch.StudySystemID, DDLHandler.DDLOption.All);
            if (!string.IsNullOrEmpty(this.StudentID_DDl))
            {
                ddlStudents.SelectedIndex = ddlStudents.Items.IndexOf(ddlStudents.Items.FindByValue(this.StudentID_DDl));
                ddlStudents_SelectedIndexChanged(null, null);
            }
        }
        #endregion

        #region Method :: Fill Semester DDL
        /// <summary>
        /// Fill Semester DDL
        /// </summary>
        private void vFillDateOfSelectSemester()
        {
            if (oDistributionSearch.SpecialityID != -99 && oDistributionSearch.ClassID != -99 && oDistributionSearch.SpecialityID != -1 && oDistributionSearch.ClassID != -1)
            {
                EduWave.SystemSettings.LevelSemesters oLevelSemesters = new EduWave.SystemSettings.LevelSemesters();
                EduWave.SystemSettings.LevelSemesters[] oArrLevelSemesters = null;
                oLevelSemesters.LanguageID = oClientInfo.LanguageID;
                oLevelSemesters.DBConnectionString = oEduSettings.DBConnectionString;
                oLevelSemesters.SpecialtyID = oDistributionSearch.SpecialityID;
                oLevelSemesters.ClassID = oDistributionSearch.ClassID;
                oLevelSemesters.SchoolCategoryID = oClientInfo.SchoolCategoryID;
                oLevelSemesters.ActiveSemesterID = 1;
                oArrLevelSemesters = oLevelSemesters.oArrGetLevelSemestersByStudyLevel();
                if (oArrLevelSemesters.Length > 0)
                {
                    DateSemester = string.Format("{0},{1}",
                        ITG_Utilities.ITGDateTime.sGetDateInClientFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, oArrLevelSemesters[0].AcademicFromDate.ToShortDateString()),
                        ITG_Utilities.ITGDateTime.sGetDateInClientFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, oArrLevelSemesters[0].AcademicToDate.ToShortDateString()));
                    this.AcademicYearID = oArrLevelSemesters[0].AcademicYearID;
                }
            }
        }
        #endregion

        #region Method :: Insert Use User Services
        /// <summary>
        /// Vs insert use user services.
        /// </summary>
        /// <param name="nUserTypeID">The n user type ID.</param>
        private void vInsertUseUserServices(int nUserTypeID)
        {
            UseUserServices oUseUserServices = new UseUserServices();
            oUseUserServices.DBConnectionString = oEduSettings.DBConnectionString;
            oUseUserServices.LanguageID = oClientInfo.LanguageID;
            oUseUserServices.DistrictID = oClientInfo.DistrictID;
            oUseUserServices.SchoolID = oClientInfo.SchoolID;
            oUseUserServices.UserTypeID = nUserTypeID;
            oUseUserServices.enumUseUserService = UseUserServices.EnumUseUserServices.AbsentsService;
            oUseUserServices.UserProfileID = oClientInfo.UserProfileID;
            oUseUserServices.vSaveUseUserServices();
        }
        #endregion

        #region Method :: vGetMainParentID
        /// <summary>
        /// N get main parent ID.
        /// </summary>
        /// <returns>An int</returns>
        private int nGetMainParentID()
        {
            int nMainParentID = -99;
            EduWavePrivilegesCOM.UserProfileMenu oUserProfileMenu = new EduWavePrivilegesCOM.UserProfileMenu();
            oUserProfileMenu.DBConnectionString = oEduSettings.DBConnectionString;
            oUserProfileMenu.LanguageID = oClientInfo.LanguageID;
            oUserProfileMenu.UserProfileID = oClientInfo.UserProfileID;
            oUserProfileMenu.UserTypeID = oClientInfo.UserTypeID;
            oUserProfileMenu.CurrentPriv = (int)CurrentPrivs.GrantedPrivs;
            oUserProfileMenu.MenuPageID = this.MenuPageID;
            EduWavePrivilegesCOM.UserProfileMenu[] oArrUserType = oUserProfileMenu.oArrGetUserProfileMenu();
            if (oArrUserType != null && oArrUserType.Length > 0)
            {
                nMainParentID = oArrUserType[0].ParentID;
            }
            return nMainParentID;
        }
        #endregion

        #region Method :: vShowNoViolationConfirmation
        /// <summary>
        /// V show no violation confirmation.
        /// </summary>
        private void vShowNoViolationConfirmation()
        {
            DateTime dtiSelectedDate = !string.IsNullOrEmpty(clrAttendanceDay.Text) ? Convert.ToDateTime(clrAttendanceDay.Text) : DateTime.Now;
            if (!((DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday)
                && (dtiSelectedDate <= DateTime.Now && dtiSelectedDate > DateTime.Now.AddDays(-8))
                && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || dtiSelectedDate.Date.Equals(DateTime.Now.Date)
                || (EditAbsencePrivilege && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || (oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector)))
            {
                dtiSelectedDate = DateTime.Now;
                lblNoViolationConfirmation.Text = string.Format(GetLocalResourceObject("lblNoViolationConfirmation.Text").ToString(), dtiSelectedDate.ToShortDateString());
            }
            SMSCom.SchoolViolationStatus oSchoolViolationStatus = new SMSCom.SchoolViolationStatus();
            oSchoolViolationStatus.DBConnectionString = oEduSettings.DBConnectionString;
            oSchoolViolationStatus.LanguageID = oClientInfo.LanguageID;
            if (oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector)
            {
                oSchoolViolationStatus.SchoolID = oDistributionSearch.SchoolID;
            }
            else
            {
                oSchoolViolationStatus.SchoolID = oClientInfo.SchoolID;
            }
            oSchoolViolationStatus.FromViolationDate = dtiSelectedDate;
            oSchoolViolationStatus.FlagID = 2;
            SMSCom.SchoolViolationStatus[] oArrSchoolViolationStatus = oSchoolViolationStatus.oArrGetSchoolViolationStatus();

            if (oArrSchoolViolationStatus != null && !oArrSchoolViolationStatus.Length.Equals(0))
            {

                if (oArrSchoolViolationStatus[0].StatusVisibleDesc.Equals(0))
                {
                    if (!DateSemester.Equals(string.Empty))
                    {

                        DateTime dtiStartDate = Convert.ToDateTime(ITG_Utilities.ITGDateTime.sGetDateInServerFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, DateSemester.Split(',')[0].Trim()));
                        DateTime dtiEndDate = Convert.ToDateTime(ITG_Utilities.ITGDateTime.sGetDateInServerFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, DateSemester.Split(',')[1].Trim()));

                        if (!(dtiStartDate <= dtiSelectedDate && dtiSelectedDate <= dtiEndDate))
                        {
                            lbtnNoViolationConfirmation.Enabled = false;
                            lbtnNoViolationConfirmation.Attributes.Remove("onclick");
                            lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("DateOutOfActiveSemester").ToString();
                        }
                        else
                        {
                            lbtnNoViolationConfirmation.Enabled = true;
                            lbtnNoViolationConfirmation.ToolTip = string.Empty;

                            #region No Violation Confirmation
                            int nConfirmationBoxWidth = Convert.ToInt32(GetLocalResourceObject("ConfirmationBoxDimensions").ToString().Split(',')[0]);
                            int nConfirmationBoxHeight = Convert.ToInt32(GetLocalResourceObject("ConfirmationBoxDimensions").ToString().Split(',')[1]);

                            string sDate = string.Empty;
                            sDate = ITG_Utilities.ITGDateTime.sGetDateInClientFormat(oEduSettings.ServerCulture, oClientInfo.ClientCulture, dtiSelectedDate.ToShortDateString());

                            ConfirmDialog oConfirmDialog = new ConfirmDialog(
                               string.Format(GetLocalResourceObject("ConfirmationMsg").ToString(), sDate),
                                GetLocalResourceObject("ConfirmationTitle").ToString(),
                             string.Empty, string.Empty, nConfirmationBoxWidth, nConfirmationBoxHeight);
                            lbtnNoViolationConfirmation.Attributes.Add("onclick", sConfrimMessage(oConfirmDialog));
                            #endregion
                        }
                    }
                    else
                    {
                        lbtnNoViolationConfirmation.Enabled = false;
                        lbtnNoViolationConfirmation.Attributes.Remove("onclick");
                        if (oArrSchoolViolationStatus[0].ViolationScount < 1)
                        {
                            lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("NoViolationsConfirmed").ToString();// „  √ﬂÌœ ⁄œ„ ÊÃÊœ €Ì«»
                        }
                        else
                        {
                            lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("ConfirmedViolationsExist").ToString();// „ ≈œŒ«· «·€Ì«»
                        }
                    }
                }
                else
                {
                    lbtnNoViolationConfirmation.Enabled = false;
                    lbtnNoViolationConfirmation.Attributes.Remove("onclick");
                    if (oArrSchoolViolationStatus[0].ViolationScount < 1)
                    {
                        lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("NoViolationsConfirmed").ToString();// „  √ﬂÌœ ⁄œ„ ÊÃÊœ €Ì«»
                    }
                    else
                    {
                        lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("ConfirmedViolationsExist").ToString();// „ ≈œŒ«· «·€Ì«»
                    }
                }

            }
            else
            {
                lbtnNoViolationConfirmation.Enabled = false;
                lbtnNoViolationConfirmation.Attributes.Remove("onclick");
                lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("CannotConfirm").ToString();
            }

        }
        #endregion

        #region Method :: bAllowEdit
        /// <summary>
        /// B allow edit for school admin.
        /// </summary>
        /// <param name="dtiSelectedDate">The dti selected date.</param>
        /// <returns>A bool</returns>
        private bool bAllowEditForSchoolAdmin(DateTime dtiSelectedDate)
        {
            if (Convert.ToDateTime(clrAttendanceDay.Text).Date > DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                nGetActiveSemster();
                //Ì⁄Ìœ «·«”»Ê⁄ «·Õ«·Ì Ê «·«”»Ê⁄ «·”«»ﬁ «·„”„ÊÕ »«· ⁄œÌ· ⁄·ÌÂ„ Õ”» «· Ê«—ÌŒ «·—«Ã⁄Â „‰ DB
                AcademicYearWeeks oAcademicYearWeeks = new AcademicYearWeeks();
                oAcademicYearWeeks.DBConnectionString = oEduSettings.DBConnectionString;
                oAcademicYearWeeks.LanguageID = oClientInfo.LanguageID;
                oAcademicYearWeeks.AcademicYearID = this.AcademicYearID;
                oAcademicYearWeeks.FlagID = 2;
                oAcademicYearWeeks.SemesterCodeID = this.SemesterCodeID;

                AcademicYearWeeks[] oArrAcademicYearWeeks = oAcademicYearWeeks.oArrGetAcademicYearWeeks();

                if (oArrAcademicYearWeeks != null && oArrAcademicYearWeeks.Length > 0)
                {
                    dtiSelectedDate = Convert.ToDateTime(dtiSelectedDate.ToShortDateString());
                    DateTime StartDateCurrentWeek = Convert.ToDateTime(oArrAcademicYearWeeks[0].StartDate.ToShortDateString());
                    DateTime EndDateCurrentWeek = Convert.ToDateTime(oArrAcademicYearWeeks[0].EndDate.ToShortDateString());
                    if (oArrAcademicYearWeeks.ElementAtOrDefault(1) != null)
                    {
                        DateTime StartDatePreviousWeek = Convert.ToDateTime(oArrAcademicYearWeeks[1].StartDate.ToShortDateString());
                        DateTime EndDatePreviousWeek = Convert.ToDateTime(oArrAcademicYearWeeks[1].EndDate.ToShortDateString());
                        if ((dtiSelectedDate >= StartDateCurrentWeek && dtiSelectedDate <= EndDateCurrentWeek) ||
                            (dtiSelectedDate >= StartDatePreviousWeek && dtiSelectedDate <= EndDatePreviousWeek))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (dtiSelectedDate >= StartDateCurrentWeek && dtiSelectedDate <= EndDateCurrentWeek)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        #endregion

        #endregion Methods

        #region Handlers

        #region Handler :: ibtnSave_Click
        /// <summary>
        /// Ibtn save click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnSave_Click(object sender, EventArgs e)
        {
            DateTime dtiSelectedDate = Convert.ToDateTime(clrAttendanceDay.Text);
            if (((!((DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday)
                    && (dtiSelectedDate <= DateTime.Now && dtiSelectedDate > DateTime.Now.AddDays(-8)))
                    || dtiSelectedDate.Date.Equals(DateTime.Now.Date)) && !EditAbsencePrivilege)
                    && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School)
                    && ddlMowadaba.SelectedValue.Equals("2")) && oClientInfo.UserType != UsersTypes.SchoolAdmin && oClientInfo.UserType != UsersTypes.Teacher && oClientInfo.UserType != UsersTypes.AgentInSchool)
            {
                EnableEdit = false;
                gvClassStudentsAttendance.DataBind();
                gvClassStudentsAttendance.Columns[7].Visible = false;
                lblOperationResult.Text = GetLocalResourceObject("CannotSaveAttendance").ToString();
            }
            else
            {
                #region Required object
                int nRowsCount = gvClassStudentsAttendance.Rows.Count;
                StringBuilder sbDeductIDs = new StringBuilder();
                StringBuilder sbStudentProfileID = new StringBuilder();
                StringBuilder sbSMSSendingCount = new StringBuilder();
                Attendance oAttendance = new Attendance();
                DropDownList ddlDegreeDeductAmount;
                #endregion
                #region Get All checked IDs
                CheckBox cbItem;
                oArrAttendance = oArrGetAttendance(!string.IsNullOrEmpty(ddlStudents.SelectedValue) && !ddlStudents.SelectedValue.Equals("-99") ? Convert.ToInt32(ddlStudents.SelectedValue) : -99);
                int nStdProfileID = -99;
                int nSmsSendingCount = -99;

                for (int nIndex = 0; nIndex < nRowsCount; nIndex++)
                {
                    cbItem = (CheckBox)gvClassStudentsAttendance.Rows[nIndex].FindControl("cbItem");
                    ddlDegreeDeductAmount = (DropDownList)gvClassStudentsAttendance.Rows[nIndex].FindControl("ddlDegreeDeductAmount");

                    if (cbItem.Checked)
                    {
                        nStdProfileID = Convert.ToInt32(ITG_CustomControls.ITG_GridView.sGetDataKeyValue(gvClassStudentsAttendance, nIndex, "StudentProfileID"));
                        nSmsSendingCount = (from item in oArrAttendance
                                            where item.StudentProfileID.Equals(nStdProfileID)
                                            select item.SmsSendingCount).First();

                        #region Set Absence and Excuse Type
                        sbStudentProfileID.Append(nStdProfileID.ToString());
                        sbStudentProfileID.Append(',');

                        sbDeductIDs.Append(ddlDegreeDeductAmount.SelectedValue.Split(',')[0]);
                        sbDeductIDs.Append(',');
                        if (oClientInfo.UserType == UsersTypes.SchoolAdmin)
                        {
                            if (nSmsSendingCount != -99)
                            {
                                sbSMSSendingCount.Append(nSmsSendingCount.ToString());
                                sbSMSSendingCount.Append(',');
                            }
                        }
                        #endregion
                    }
                }
                #endregion
                #region Set Attendence object with Insertion Data
                oAttendance.StudentProfileIDs = sbStudentProfileID.ToString().Trim(',');
                if (ddlMowadaba.SelectedValue.Equals("1"))
                {
                    oAttendance.DeductTypeID = Convert.ToInt32(ddlDeductType.SelectedValue);
                }
                oAttendance.DeductIDs = sbDeductIDs.ToString().TrimEnd(',');
                oAttendance.AbsenceDate = Convert.ToDateTime(clrAttendanceDay.Text);
                oAttendance.CreatedBy = oClientInfo.UserProfileID;
                oAttendance.HierarchyID = oDistributionSearch.SchoolID;
                oAttendance.ClassID = oDistributionSearch.ClassID;
                if (oDistributionSearch.SpecialityID != -1 && oDistributionSearch.SpecialityID != -99)
                {
                    oAttendance.SpecialtyID = oDistributionSearch.SpecialityID;
                }
                oAttendance.ActiveSemetser = ActiveSemesterID;
                if (oDistributionSearch.SectionID != -99)
                {
                    oAttendance.SectionID = oDistributionSearch.SectionID;
                }
                oAttendance.FilteredUserProfileID = Convert.ToInt32(ddlStudents.SelectedValue);
                oAttendance.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
                oAttendance.DBConnectionString = oEduSettings.DBConnectionString;
                oAttendance.LanguageID = oClientInfo.LanguageID;
                oAttendance.SmsSendingCounts = sbSMSSendingCount.ToString().TrimEnd(',');
                oAttendance.vSaveStudentsAttendance();
                #endregion
                #region Validating Operational result
                if (oAttendance.OperationResult > 0)
                {
                    gvClassStudentsAttendance.DataBind();
                    gvClassStudentsAttendance.Columns[7].Visible = false;
                    tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                    lblOperationResult.Text = GetLocalResourceObject("AddSuccessfully").ToString();
                    divquestionnaire.Visible = false;
                    if (oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School)
                        || oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector)
                    {
                        vShowNoViolationConfirmation();
                    }
                }
                else if (oAttendance.OperationResult == -3)
                {
                    lblOperationResult.Text = GetLocalResourceObject("HoursInstalledForTheDay").ToString();
                }
                else if (oAttendance.OperationResult == -4)
                {
                    lblOperationResult.Text = GetLocalResourceObject("HolidayError").ToString();
                }
                else if (oAttendance.OperationResult == -5)
                {
                    //·« Ì„ﬂ‰ ≈÷«›… €Ì«» ›Ì ›’· €Ì— ›⁄«·
                    lblOperationResult.Text = GetLocalResourceObject("CanNotAddAbsent").ToString();
                }
                else if (oAttendance.OperationResult == -6)
                {
                    lblOperationResult.Text = GetLocalResourceObject("CannotEditPreviousDay").ToString();
                }
                else
                {
                    lblOperationResult.Text = GetLocalResourceObject("AddFailed").ToString();
                }
                #endregion
            }
        }
        #endregion

        #region Handler :: ibtnSearch_Click
        /// <summary>
        /// Ibtn search click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnSearch_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }
            lblConfirmationResult.Text = string.Empty;
            divquestionnaire.Visible = false;
            if (oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School) && ddlMowadaba.SelectedValue.Equals("2"))
            {
                DateTime dtiSelectedDate = Convert.ToDateTime(clrAttendanceDay.Text);
                EnableEdit = ((DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday)
                    && (dtiSelectedDate <= DateTime.Now && dtiSelectedDate > DateTime.Now.AddDays(-8)))
                    || dtiSelectedDate.Date.Equals(DateTime.Now.Date));
            }
            else
            {
                EnableEdit = true;
            }
            if (oClientInfo.UserType == UsersTypes.SchoolAdmin || oClientInfo.UserType == UsersTypes.Teacher || oClientInfo.UserType == UsersTypes.AgentInSchool)
            {
                this.AllowEditForSchoolAdmin = bAllowEditForSchoolAdmin(Convert.ToDateTime(clrAttendanceDay.Text));
            }
            #region Manage Distribution Hierarchy Label
            StringBuilder sblblSelectedSectionText = new StringBuilder();
            if (oDistributionSearch.ClassID != -99)
            {
                sblblSelectedSectionText.Append(oDistributionSearch.ClassDescription);
                sblblSelectedSectionText.Append(" - ");
            }
            if (oDistributionSearch.SpecialityID != -1 && oDistributionSearch.SpecialityID != -99)
            {
                sblblSelectedSectionText.Append(oDistributionSearch.SpecialityDescription);
                sblblSelectedSectionText.Append(" - ");
            }
            if (oDistributionSearch.SectionID != -99)
            {
                sblblSelectedSectionText.Append(oDistributionSearch.SectionDescription);
                sblblSelectedSectionText.Append(" - ");
            }
            #endregion

            #region Search student Absence
            this.ActiveSemesterID = nGetActiveSemster();
            if (this.ActiveSemesterID.Equals(-100)) // «–« ﬂ«‰  › —… √œŒ«· «·œ—Ã«  ·Ì”  „€·ﬁ… Ê«·›’· «·œ—«”Ì ·« ÌÕ ÊÌ ⁄·Ï ÿ·«» 
            {
                if (!ddlViolation.SelectedValue.Equals("2")) // ‰Ê⁄ «·€Ì«» ·Ì” Õ’… 
                {
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = GetLocalResourceObject("MarksEntryPeriodClosed").ToString();
                    tableViewClassStudentsAttendance.Visible = true;
                    gvClassStudentsAttendance.DataBind();
                    gvClassStudentsAttendance.Columns[7].Visible = false;
                    tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                    if (gvClassStudentsAttendance.Rows.Count > 0)
                    {
                        mvAttendance.SetActiveView(viewClassStudentsAttendance);
                    }
                    lblAbsenceTip.Visible = false;
                    gvClassStudentsAttendance.Columns[0].Visible = false;
                    trEmptyReport.Visible = false;
                }
                else
                {
                    lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                    oStudentAttendanceUC.EvaluationModes = StudentAttendanceUC.EvaluationMode.ViolationsMudaba;
                    oStudentAttendanceUC.ViolationModes = StudentAttendanceUC.ViolationsMode.Archive;
                    gvStudentLectureVaiolations.DataBind();
                    mvAttendance.SetActiveView(ViewOneLecture);
                    lblOperationResult.Text = String.Empty;
                    tableViewClassStudentsAttendance.Visible = true;
                    hdnSelectedAttendanceDate.Value = clrAttendanceDay.Text;
                    lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                    gvStudentLectureVaiolations.Columns[0].Visible = false;
                }
                ibtnSave.Visible = false; // hide save button when marke entance period is closed 
            }
            else
            {

                if (this.ActiveSemesterID != -99)
                {
                    if (ddlMowadaba.SelectedValue.Equals("1") || ddlViolation.SelectedValue.Equals("1") && this.ActiveSemesterID != -99 && this.ActiveSemesterID != -100) // «·”·Êﬂ 
                    {
                        oStudentAttendanceUC.ViolationModes = StudentAttendanceUC.ViolationsMode.StudentsAttendance;
                        lblOperationResult.Text = String.Empty;
                        tableViewClassStudentsAttendance.Visible = true;
                        if ((ddlDeductType.SelectedValue.Equals("1") || ddlViolation.SelectedValue.Equals("1")) && !ddlMowadaba.SelectedValue.Equals("2")) // „Œ«·›…«Ê „Ê«Ÿ»… ÌÊﬂ ﬂ«„·  
                        {
                            oStudentAttendanceUC.EvaluationModes = StudentAttendanceUC.EvaluationMode.Violation;
                            lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                        }
                        else if (ddlDeductType.SelectedValue.Equals("2") && ddlMowadaba.SelectedValue.Equals("1"))//  ”·Êﬂ + √ÌÃ«»Ì…
                        {
                            oStudentAttendanceUC.EvaluationModes = StudentAttendanceUC.EvaluationMode.Positive;
                            lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipPositive").ToString();
                        }
                        else
                        {
                            lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                            oStudentAttendanceUC.EvaluationModes = StudentAttendanceUC.EvaluationMode.ViolationsMudaba;
                        }
                        gvClassStudentsAttendance.Visible = true;
                        gvClassStudentsAttendance.DataBind();
                        gvClassStudentsAttendance.Columns[7].Visible = false;
                        tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                        if (gvClassStudentsAttendance.Rows.Count > 0)
                        {
                            mvAttendance.SetActiveView(viewClassStudentsAttendance);
                        }
                        else
                        {
                            if (ddlMowadaba.SelectedValue == "1")
                            {
                                if (ddlDeductType.SelectedValue == "2")
                                {
                                    lblNoDataFound.Text = GetLocalResourceObject("NoStudentRelatedToThisSections").ToString();
                                }
                                else
                                {
                                    lblNoDataFound.Text = GetLocalResourceObject("NoStudentRelatedToThisSections").ToString();
                                }
                            }
                            else
                            {
                                lblNoDataFound.Text = GetLocalResourceObject("NoStudentRelatedToThisSections").ToString();
                            }

                            lblNoDataFound.Visible = true;
                        }
                    }
                    else if (ddlMowadaba.SelectedValue.Equals("2") && this.ActiveSemesterID != -99 && this.ActiveSemesterID != -100) // «·„Ê«Ÿ»… 
                    {
                        if (ddlViolation.SelectedValue.Equals("2")) // Õ’… 
                        {
                            lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                            oStudentAttendanceUC.EvaluationModes = StudentAttendanceUC.EvaluationMode.ViolationsMudaba;
                            oStudentAttendanceUC.ViolationModes = StudentAttendanceUC.ViolationsMode.Archive;
                            gvStudentLectureVaiolations.Visible = true;
                            gvStudentLectureVaiolations.DataBind();
                            mvAttendance.SetActiveView(ViewOneLecture);
                            lblOperationResult.Text = String.Empty;
                            tableViewClassStudentsAttendance.Visible = true;
                            hdnSelectedAttendanceDate.Value = clrAttendanceDay.Text;
                            lblAbsenceTip.Text = GetLocalResourceObject("AbsenceTipViolation").ToString();
                        }
                        else if (ddlViolation.SelectedValue.Equals("1")) // ÌÊ„ ﬂ«„· 
                        {
                            gvClassStudentsAttendance.Visible = true;
                            gvClassStudentsAttendance.DataBind();
                            gvClassStudentsAttendance.Columns[7].Visible = false;
                            tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                        }
                    }
                }
                else if (this.ActiveSemesterID != -100) // «–« ﬂ«‰  › —… √œŒ«· «·œ—Ã«  ·Ì”  „€·ﬁ… Ê«·›’· «·œ—«”Ì ·« ÌÕ ÊÌ ⁄·Ï ÿ·«» 
                {
                    lblNoDataFound.Visible = true;
                    lblNoDataFound.Text = GetLocalResourceObject("NoActiveSemesterExist").ToString();
                    tableViewClassStudentsAttendance.Visible = false;
                }
            }
            #endregion

            #region Show intro Bookmarks
            if (Convert.ToBoolean(GetGlobalResourceObject("SettingsBookMarks", "EnabledBookMarksIntroViewButton").ToString()))
            {
                string sValue;
                if (Request.Cookies["ShowIntroViewButton" + oClientInfo.UserID.ToString()] != null)
                {
                    sValue = Request.Cookies["ShowIntroViewButton" + oClientInfo.UserID.ToString()].Value.ToString();
                }
                else
                {
                    sValue = GetGlobalResourceObject("SettingsBookMarks", "EnabledBookMarksIntroViewButton").ToString();
                }
                if (Convert.ToBoolean(sValue))
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "funShowIntro", "funShowIntro();", true);
                }
            }
            #endregion
        }
        #endregion

        #region Handler :: ibtnBackFromOperation_Click
        /// <summary>
        /// Ibtn back from operation click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnBackFromOperation_Click(object sender, EventArgs e)
        {
            if (IsPrivilegedUser && !IsMarksEntryClosed)
            {
                mvAttendance.SetActiveView(viewClassStudentsAttendance);

                gvClassStudentsAttendance.DataBind();
                gvClassStudentsAttendance.Columns[7].Visible = false;
                tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                tblDistributionUserControl.Visible = true;
            }
            else
            {
                if (this.MenuPageID != -99)
                {
                    string sKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                    string sValue = ITGTextEncryption.QuerystringEncryptValue(nGetMainParentID().ToString());
                    CustomRedirect("~/EduWaveSMS/Absences.aspx" + "?" + sKey + "=" + sValue);
                }
                else
                {
                    CustomTransfer("Absences.aspx");
                }
            }
        }
        #endregion

        #region Handler :: ibtnBackMain_Click
        /// <summary>
        /// Ibtn back main click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnBackMain_Click(object sender, EventArgs e)
        {
            if (this.BookMarkID != -99)
            {
                CustomRedirect("~/EduWavek12Portal/HomePage.aspx");
            }
            else
            {
                if (oClientInfo.UserType == UsersTypes.SchoolAdmin)
                {
                    if (this.MenuPageID != -99)
                    {
                        string sKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                        string sValue = ITGTextEncryption.QuerystringEncryptValue(nGetMainParentID().ToString());
                        CustomRedirect("~/EduWaveSMS/Absences.aspx" + "?" + sKey + "=" + sValue);
                    }
                    else
                    {
                        CustomRedirect("Absences.aspx");
                    }
                }
                else if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
                {
                    StringBuilder sbSearchCriteria = new StringBuilder();
                    for (int i = 0; i < SearchCriteria.Length; i++)
                    {
                        sbSearchCriteria.Append(SearchCriteria[i]);
                        sbSearchCriteria.Append('$');
                    }
                    string sSearchCriteriaKey = ITGTextEncryption.QuerystringEncryptKey("SearchCriteria");
                    string sSearchCriteriaValue = ITGTextEncryption.QuerystringEncryptValue(sbSearchCriteria.ToString());
                    string sMenuPageIDKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                    string sMenuPageIDValue = ITGTextEncryption.QuerystringEncryptValue(MenuPageID.ToString());
                    CustomRedirect("ManageAttendanceForDistrict.aspx" + "?" + sSearchCriteriaKey + "=" + sSearchCriteriaValue
                        + "&" + sMenuPageIDKey + "=" + sMenuPageIDValue);
                }
                else
                {
                    string sKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                    string sValue = ITGTextEncryption.QuerystringEncryptValue(nGetMainParentID().ToString());
                    CustomRedirect("ManageAttendanceMenu.aspx" + "?" + sKey + "=" + sValue);
                }
            }
        }
        #endregion

        #region Handler :: ibtnBackFromStudentAbsences_Click
        /// <summary>
        /// Ibtn back from student absences click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnBackFromStudentAbsences_Click(object sender, EventArgs e)
        {
            Title = string.Format(GetGlobalResourceObject("CommonMessages", "PageTitle").ToString(), GetLocalResourceObject("PageTitle"));
            PageTitle = GetLocalResourceObject("PageTitle").ToString();
            if (oStudentAttendanceUC.ViolationModes.Equals(StudentAttendanceUC.ViolationsMode.Archive))
            {
                gvStudentLectureVaiolations.DataBind();
                mvAttendance.SetActiveView(ViewOneLecture);
            }
            else
            {
                ibtnSearch_Click(null, null);
                tblPrinterMsg.Visible = gvClassStudentsAttendance.Rows.Count > 0;
                mvAttendance.SetActiveView(viewClassStudentsAttendance);
            }
            tblDistributionUserControl.Visible = true;
        }
        #endregion

        #region Handler :: clrAttendanceDay_TextChanged
        /// <summary>
        /// Clrs attendance day text changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void clrAttendanceDay_TextChanged(object sender, EventArgs e)
        {
            hdnSelectedAttendanceDate.Value = clrAttendanceDay.Text;
            lblOperationResult.Text = string.Empty;
            mvAttendance.ActiveViewIndex = -1;
            if (!oDistributionSearch.ClassID.Equals(-99) &&
                !oDistributionSearch.SpecialityID.Equals(-99) &&
                !oDistributionSearch.SectionID.Equals(-99) &&
                !clrAttendanceDay.Text.Equals(string.Empty) &&
                 ddlViolation.SelectedValue.Equals("2"))
            {
                vFillLectureDDL();
            }
            else
            {
                ddlLecture.Enabled = false;
                ddlLecture.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
            }

            DateTime dtiSelectedDate = !string.IsNullOrEmpty(clrAttendanceDay.Text) ? Convert.ToDateTime(clrAttendanceDay.Text) : DateTime.Now;
            if ((DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday)
                && (dtiSelectedDate <= DateTime.Now && dtiSelectedDate > DateTime.Now.AddDays(-8))
                && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || dtiSelectedDate.Date.Equals(DateTime.Now.Date)
                || (EditAbsencePrivilege && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || (oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector))
            {
                lblNoViolationConfirmation.Text = string.Format(GetLocalResourceObject("lblNoViolationConfirmation.Text").ToString(), clrAttendanceDay.Text);
                vShowNoViolationConfirmation();
            }
            else
            {
                lbtnNoViolationConfirmation.Enabled = false;
                lbtnNoViolationConfirmation.Attributes.Remove("onclick");
                lbtnNoViolationConfirmation.ToolTip = GetLocalResourceObject("CannotConfirm").ToString();
                lblNoViolationConfirmation.Text = string.Format(GetLocalResourceObject("lblNoViolationConfirmation.Text").ToString(), clrAttendanceDay.Text);
            }
            lblConfirmationResult.Text = string.Empty;
        }
        #endregion

        #region Handlers :: GridView Class Students Attendance

        #region Handler :: gvClassStudentsAttendance_RowDataBound
        /// <summary>
        /// Gvs class students attendance row data bound.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void gvClassStudentsAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Header
            if (e.Row.RowType == DataControlRowType.Header)
            {
                nRowDisabledCount = 0;
                vFillDegreeDeductAmount();
                CheckBox cbHeader = (CheckBox)e.Row.FindControl("cbHeader");
                bCheckHeader = true;

                if (EnableEdit || EditAbsencePrivilege)
                {
                    cbHeader.Enabled = true;
                }
                else
                {
                    cbHeader.Enabled = false;
                    cbHeader.ToolTip = GetLocalResourceObject("EditDisabled").ToString();
                }
                if (oClientInfo.UserType == UsersTypes.SchoolAdmin || oClientInfo.UserType == UsersTypes.Teacher || oClientInfo.UserType == UsersTypes.AgentInSchool)
                {
                    cbHeader.Enabled = (this.AllowEditForSchoolAdmin && AllowEditWeek) || EditAbsencePrivilege;
                }
                if (cbHeader.Enabled)
                {
                    cbHeader.Attributes["onclick"] = "CheckAllCheckBoxesClassStudentsAttendance('" + gvClassStudentsAttendance.ClientID + "','cbHeader','cbItem',event); ";

                }
            }
            #endregion
            #region DataRow
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnSend = (LinkButton)e.Row.FindControl("lbtnSend") as LinkButton;
                Label lblDegreeMuadabaCourseDesc = (Label)e.Row.FindControl("lblDegreeMuadabaCourseDesc") as Label;
                string ViolationName = ITG_CustomControls.ITG_GridView.sGetDataKeyValue(gvClassStudentsAttendance, e.Row.RowIndex, "DegreeMuadabaCourseDesc");
                int nStdProfileID = Convert.ToInt32(ITG_CustomControls.ITG_GridView.sGetDataKeyValue(gvClassStudentsAttendance, e.Row.RowIndex, "StudentProfileID"));
                Attendance[] oArrStdAttendance = (from item in oArrAttendance
                                                  where item.StudentProfileID.Equals(nStdProfileID)
                                                  select item).ToArray();

                #region Get Controls Referance
                CheckBox cbItem = (CheckBox)e.Row.FindControl("cbItem");
                cbItem.Attributes["onclick"] = "CheckThisCheckBoxClassStudentsAttendance('" + gvClassStudentsAttendance.ClientID + "','cbHeader','cbItem',event);";

                RequiredFieldValidator rfvDegreeDeductAmount = e.Row.FindControl("rfvDegreeDeductAmount") as RequiredFieldValidator;
                DropDownList ddlDegreeDeductAmount = (DropDownList)e.Row.FindControl("ddlDegreeDeductAmount") as DropDownList;
                ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");

                LinkButton lbtnStudentFullName = (LinkButton)e.Row.FindControl("lbtnStudentFullName") as LinkButton;

                if (oClientInfo.UserType == UsersTypes.SchoolAdmin)
                {
                    int nSmsSendingCount = oArrStdAttendance[0].SmsSendingCount;
                    int nViolationID = oArrStdAttendance[0].ViolationID;
                    lbtnSend.Enabled = !(nSmsSendingCount > 0) && nViolationID != -99 && PrintMode == enumPrintMode.Original;
                    if (nSmsSendingCount.Equals(1))
                    {
                        e.Row.Cells[3].ToolTip = GetLocalResourceObject("CantSendMessage").ToString();
                        lbtnSend.ToolTip = GetLocalResourceObject("CantSendMessage").ToString();
                    }
                    else if (nViolationID == -99)
                    {
                        lbtnSend.ToolTip = e.Row.Cells[3].ToolTip = GetLocalResourceObject("EnterViolation").ToString();
                    }
                    if (lbtnSend != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(e.Row.FindControl("lbtnSend"));
                    }
                }
                #endregion

                #region Behavior
                if (ddlMowadaba.SelectedValue.Equals("1")) // «·”·Êﬂ
                {
                    if (ddlDeductType.SelectedValue.Equals("1")) // „Œ«·›… +  «·”·Êﬂ
                    {
                        rfvDegreeDeductAmount.ErrorMessage = GetLocalResourceObject("ChooseViolationText").ToString();

                    }
                    else //   «ÌÃ«»Ì… +  «·”·Êﬂ
                    {
                        rfvDegreeDeductAmount.ErrorMessage = GetLocalResourceObject("ChoosePositivityType").ToString();
                    }
                }
                #endregion

                #region Muadaba
                else if (ddlMowadaba.SelectedValue.Equals("2")) // «·„Ê«Ÿ»…
                {
                    if (ddlViolation.SelectedValue.Equals("1")) // «·„Ê«Ÿ»… + ÌÊ„ ﬂ«„· 
                    {
                        rfvDegreeDeductAmount.ErrorMessage = GetLocalResourceObject("ChooseViolationAbsence").ToString();
                    }
                }
                #endregion

                #region Fill Degree Deduct Amount DDL
                if (this.oArrDegreeDeduct != null && this.oArrDegreeDeduct.Length > 0)
                {
                    int nRowCount = oArrDegreeDeduct.Length;
                    for (int nIndex = 0; nIndex < nRowCount; nIndex++)
                    {

                        ddlDegreeDeductAmount.Items.Add(new ListItem(oArrDegreeDeduct[nIndex].Description, oArrDegreeDeduct[nIndex].DeductID.ToString() + "," + oArrDegreeDeduct[nIndex].DegreeMuadabaCourseDesc));

                    }
                    ddlDegreeDeductAmount.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListSelect").ToString(), "-99"));
                }
                else
                {
                    ddlDegreeDeductAmount.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
                    ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");
                }
                if (ddlMowadaba.SelectedValue == "1" && ddlDeductType.SelectedValue == "1")
                {
                    ddlDegreeDeductAmount.Attributes.Add("onchange", "ChangeDegreeDesc('" + ddlDegreeDeductAmount.ClientID + "','" + lblDegreeMuadabaCourseDesc.ClientID + "');");
                }

                #endregion

                #region DataKeyValue
                int nCheckFlag = oArrStdAttendance[0].CheckFlag;
                int nDeductID = oArrStdAttendance[0].DeductID;
                #endregion

                #region Disable And Enable Controls
                if (nCheckFlag == 1)
                {
                    cbItem.Checked = true;
                    ddlDegreeDeductAmount.Attributes.Remove("disabled");
                    string ddlDegreeDeductAmountSelectedValue = nDeductID.ToString() + "," + ViolationName;
                    ddlDegreeDeductAmount.SelectedIndex = ddlDegreeDeductAmount.Items.IndexOf(ddlDegreeDeductAmount.Items.FindByValue(ddlDegreeDeductAmountSelectedValue));
                    rfvDegreeDeductAmount.Enabled = true;
                }
                else
                {
                    bCheckHeader = false;
                    cbItem.Checked = false;
                    rfvDegreeDeductAmount.Enabled = false;
                }
                #endregion

                #region RegisterPostBackControl
                if (ScriptManager.GetCurrent(this) != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(e.Row.FindControl("lbtnStudentFullName"));
                }
                #endregion

                #region Disable Reaseon
                int nEnableFlag = oArrStdAttendance[0].EnableFlag;
                if (!Convert.ToBoolean(nEnableFlag))
                {
                    nRowDisabledCount += 1;

                    cbItem.Enabled = false;
                    ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");
                    rfvDegreeDeductAmount.Enabled = false;

                    if (this.ByViolationLectur)
                    {
                        e.Row.ToolTip = GetLocalResourceObject("IsDerived").ToString();
                    }
                    else
                    {
                        e.Row.ToolTip = GetLocalResourceObject("IsConfirmed").ToString();
                    }
                }
                //if (!EnableEdit && !EditAbsencePrivilege)
                //{
                //    cbItem.Enabled = false;
                //    ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");
                //    rfvDegreeDeductAmount.Enabled = false;
                //    e.Row.ToolTip = GetLocalResourceObject("EditDisabled").ToString();
                //}
                #endregion

                #region PrintMode
                if (PrintMode == enumPrintMode.Print)
                {
                    ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");
                    lbtnStudentFullName.Enabled = false;
                }
                else if (nCheckFlag.Equals(1) && PrintMode == enumPrintMode.Original)
                {
                    //bool DisabeldFlag = EnableEdit || EditAbsencePrivilege;
                    //if (!DisabeldFlag)
                    //{
                    //    ddlDegreeDeductAmount.Attributes.Add("disabled", "disabled");

                    //}

                    lbtnStudentFullName.Enabled = true;
                }
                #endregion

                #region  ⁄œÌ· ‘—Êÿ ⁄‰œ „œÌ— „œ—”…
                if (oClientInfo.UserType == UsersTypes.SchoolAdmin || oClientInfo.UserType == UsersTypes.Teacher || oClientInfo.UserType == UsersTypes.AgentInSchool)
                {
                    cbItem.Enabled = true;
                }
                #endregion
            }
            #endregion
            #region Footer Row
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                CheckBox cbHeader = (CheckBox)gvClassStudentsAttendance.HeaderRow.FindControl("cbHeader");
                cbHeader.Checked = bCheckHeader;

                ibtnSave.Visible = !nGetActiveSemster().Equals(-100) && gvClassStudentsAttendance.Rows.Count > 0 && PrintMode == enumPrintMode.Original && (EnableEdit || EditAbsencePrivilege);
                if (oClientInfo.UserType == UsersTypes.SchoolAdmin || oClientInfo.UserType == UsersTypes.Teacher || oClientInfo.UserType == UsersTypes.AgentInSchool)
                {
                    ibtnSave.Visible = true;
                }
            }
            #endregion
        }
        #endregion

        #region Handler :: gvClassStudentsAttendance_RowCommand
        /// <summary>
        /// Gvs class students attendance row command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void gvClassStudentsAttendance_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("StudentAttendance", StringComparison.OrdinalIgnoreCase))
            {
                //View Student Attendances
                StudentProfileID = Convert.ToInt32(e.CommandArgument);
                oArrAttendance = oArrGetAttendance(StudentProfileID);
                mvAttendance.SetActiveView(viewStudentAttendance);
                //initialize user info custom control
                StudentID = oArrAttendance[0].StudentID;
                vBindStudentAttendanceUC();
                vInitiliazeUserInformationUserControl();
                tblDistributionUserControl.Visible = false;
                UserInformationsCC.Attributes.Add("printable", "true");
                ibtnPrint.Attributes.Add("onclick", "return PrintPage('" + GetGlobalResourceObject("Designs", "PageDirection").ToString() + "');");

                Title = string.Format(GetGlobalResourceObject("CommonMessages", "PageTitle").ToString(), GetLocalResourceObject("MuadabaTitle").ToString() + " " + ddlMowadaba.SelectedItem.Text);
                PageTitle = GetLocalResourceObject("MuadabaTitle").ToString() + " " + ddlMowadaba.SelectedItem.Text;

                if (ddlMowadaba.SelectedValue.Equals("1")) // ”·Êﬂ
                {
                    lblAttendanceHistoryTitle.Text = GetLocalResourceObject("BehaviourLevel").ToString();
                }
                else if (ddlMowadaba.SelectedValue.Equals("2")) // ÌÊ„ ﬂ«„·- „Ê«Ÿ»…
                {
                    lblAttendanceHistoryTitle.Text = GetLocalResourceObject("MudabaLevel").ToString() + " " + ddlViolation.SelectedItem.Text;
                }
                ITG_CustomControls.ITG_GridView gvStudentAttendances = (ITG_CustomControls.ITG_GridView)oStudentAttendanceUC.FindControl("gvStudentAttendances");
                tblPrintViolations.Visible = gvStudentAttendances.Rows.Count > 0;
            }
            else if (e.CommandName.Equals("SendSMS", StringComparison.OrdinalIgnoreCase))
            {
                int nStdProfileID = Convert.ToInt32(e.CommandArgument);
                oArrAttendance = oArrGetAttendance(nStdProfileID);

                string sSchoolID = string.Empty;
                string sSchoolIDValue = string.Empty;
                string sRefPage = ITGTextEncryption.QuerystringEncryptKey("RefPage");

                string sRefPageValue = ITGTextEncryption.QuerystringEncryptKey("~/EduWaveSMS/ManageAttendance.aspx?");

                string sStudentName = ITGTextEncryption.QuerystringEncryptKey("StudentName");

                string sStudentNameValue = ITGTextEncryption.QuerystringEncryptValue(oArrAttendance[0].StudentFullName);

                string sSendSMS = ITGTextEncryption.QuerystringEncryptKey("SendSMS");

                string sSendSMSValue = ITGTextEncryption.QuerystringEncryptKey("1");

                string sMuadabaCourseID = ITGTextEncryption.QuerystringEncryptKey("MuadabaCourseID");
                string sMuadabaCourseIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlMowadaba.SelectedValue);

                string sMuadabaLevelID = ITGTextEncryption.QuerystringEncryptKey("MuadabaLevelID");
                string sMuadabaLevelIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlViolation.SelectedValue);

                string sMobile = ITGTextEncryption.QuerystringEncryptKey("Mobile");
                string sMobileNumValue = ITGTextEncryption.QuerystringEncryptValue(oArrAttendance[0].ParentMobile);

                string sID = ITGTextEncryption.QuerystringEncryptKey("ID");
                string sIDValue = ITGTextEncryption.QuerystringEncryptValue(oArrAttendance[0].ViolationID.ToString());

                string sFlagID = ITGTextEncryption.QuerystringEncryptKey("FlagID");
                string sFlagIDValue = ITGTextEncryption.QuerystringEncryptKey("1");

                string sClassID = ITGTextEncryption.QuerystringEncryptKey("ClassID");
                string sClassIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.ClassID.ToString());

                string sSectionID = ITGTextEncryption.QuerystringEncryptKey("SectionID");
                string sSectionIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.SectionID.ToString());

                string sSpecialityID = ITGTextEncryption.QuerystringEncryptKey("SpecialityID");
                string sSpecialityIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.SpecialityID.ToString());

                string sDeductTypeID = ITGTextEncryption.QuerystringEncryptKey("DeductTypeID");
                string sDeductTypeIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlDeductType.SelectedValue.ToString());

                string sAttendanceDay = ITGTextEncryption.QuerystringEncryptKey("AttendanceDay");
                string sAttendanceDayValue = ITGTextEncryption.QuerystringEncryptValue(clrAttendanceDay.Text);

                string sStudentIdQueryStringGV = ITGTextEncryption.QuerystringEncryptKey("StudentIdQueryStringGV");
                string sStudentIdQueryStringValueGV = ITGTextEncryption.QuerystringEncryptValue(nStdProfileID.ToString());

                string sStudentIdQueryString = ITGTextEncryption.QuerystringEncryptKey("StudentIdQueryString");
                string sStudentIdQueryStringValue = ITGTextEncryption.QuerystringEncryptValue(ddlStudents.SelectedValue);

                string sMenuPageIDKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                string sMenuPageIDValue = ITGTextEncryption.QuerystringEncryptValue(MenuPageID.ToString());

                CustomRedirect("~/EduWaveMessages/AbsentSMS.aspx?"
                    + sRefPage + "=" + sRefPageValue + "&"
                    + sSendSMS + "=" + sSendSMSValue + "&"
                    + sStudentName + "=" + sStudentNameValue + "&"
                    + sMuadabaCourseID + "=" + sMuadabaCourseIDValue + "&"
                    + sID + "=" + sIDValue + "&"
                    + sMuadabaLevelID + "=" + sMuadabaLevelIDValue + "&"
                    + sSpecialityID + "=" + sSpecialityIDValue + "&"
                    + sSectionID + "=" + sSectionIDValue + "&"
                    + sFlagID + "=" + sFlagIDValue + "&"
                    + sDeductTypeID + "=" + sDeductTypeIDValue + "&"
                    + sAttendanceDay + "=" + sAttendanceDayValue + "&"
                    + sStudentIdQueryString + "=" + sStudentIdQueryStringValue + "&"
                    + sMobile + "=" + sMobileNumValue + "&"
                    + sStudentIdQueryStringGV + "=" + sStudentIdQueryStringValueGV + "&"
                    + sMenuPageIDKey + "=" + sMenuPageIDValue + "&"
                    + sClassID + "=" + sClassIDValue);
            }
        }
        #endregion

        #endregion

        #region Handlers :: GridView Student Lecture Vaiolations

        #region Handler :: gvStudentLectureVaiolations_RowDataBound
        /// <summary>
        /// Gvs student lecture vaiolations row data bound.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void gvStudentLectureVaiolations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            #region Header Row
            if (e.Row.RowType == DataControlRowType.Header)
            {
                nRowDisabledCount = 0;
                vFillDegreeDeductAmount();
                bCheckHeader = true;

                #region Add JavaScript Function to Check Box
                CheckBox cbCheckFlageAll = (CheckBox)e.Row.FindControl("cbCheckFlageAll");
                cbCheckFlageAll.Attributes.Add("onclick", "CheckAllCheckBoxes('" + gvStudentLectureVaiolations.ClientID + "','cbCheckFlageAll','cbCheckFlage',event)");
                #endregion
            }
            #endregion
            #region Data Row
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int nStdProfileID = Convert.ToInt32(ITG_CustomControls.ITG_GridView.sGetDataKeyValue(gvStudentLectureVaiolations, e.Row.RowIndex, "StudentProfileID"));
                Violations[] oArrStdViolations = (from item in oArrViolations
                                                  where item.StudentProfileID.Equals(nStdProfileID)
                                                  select item).ToArray();
                #region Find Controls
                DropDownList ddlVaiolationType = (DropDownList)e.Row.FindControl("ddlVaiolationType");
                CheckBox cbCheckFlage = (CheckBox)e.Row.FindControl("cbCheckFlage");
                RequiredFieldValidator rfvddlVaiolationType = (RequiredFieldValidator)e.Row.FindControl("rfvddlVaiolationType");
                RequiredFieldValidator rfvLateMinute = (RequiredFieldValidator)e.Row.FindControl("rfvLateMinute");
                RequiredFieldValidator rfvtbLateHour = (RequiredFieldValidator)e.Row.FindControl("rfvtbLateHour");
                CompareValidator cvtbLateMinute = (CompareValidator)e.Row.FindControl("cvtbLateMinute");
                CompareValidator cvtbLateHour = (CompareValidator)e.Row.FindControl("cvtbLateHour");
                TextBox tbLateMinute = (TextBox)e.Row.FindControl("tbLateMinute");
                TextBox tbLateHour = (TextBox)e.Row.FindControl("tbLateHour");
                HtmlTable tblViolationLate = (HtmlTable)e.Row.FindControl("tblViolationLate");

                if (oClientInfo.UserType == UsersTypes.SchoolAdmin)
                {
                    LinkButton lbtnSend = (LinkButton)e.Row.FindControl("lbtnSend") as LinkButton;
                    int nSmsSendingCount = oArrStdViolations[0].SmsSendingCount;
                    int nLecturesViolationID = oArrStdViolations[0].LecturesViolationID;
                    lbtnSend.Enabled = !(nSmsSendingCount > 0) && nLecturesViolationID != -99;

                    if (nSmsSendingCount.Equals(1))
                    {
                        e.Row.Cells[3].ToolTip = GetLocalResourceObject("CantSendMessage").ToString();
                    }
                    else if (nLecturesViolationID == -99)
                    {
                        e.Row.Cells[3].ToolTip = GetLocalResourceObject("EnterViolation").ToString();
                    }
                    if (lbtnSend != null)
                    {
                        ScriptManager.GetCurrent(this).RegisterPostBackControl(lbtnSend);
                    }
                }
                #endregion

                #region Set OnKeyPress On TextBoxess Of Late Minuet and Hour
                tbLateMinute.Attributes.Add("onkeypress", "SetMaxValueOfMinuteOrHour('" + tbLateMinute.ClientID + "');");
                tbLateMinute.Attributes.Add("onkeydown", "SetMaxValueOfMinuteOrHour('" + tbLateMinute.ClientID + "');");
                tbLateMinute.Attributes.Add("onkeyup", "SetMaxValueOfMinuteOrHour('" + tbLateMinute.ClientID + "');");
                tbLateHour.Attributes.Add("onkeypress", "SetMaxValueOfMinuteOrHour('" + tbLateHour.ClientID + "');");
                tbLateHour.Attributes.Add("onkeydown", "SetMaxValueOfMinuteOrHour('" + tbLateHour.ClientID + "');");
                tbLateHour.Attributes.Add("onkeyup", "SetMaxValueOfMinuteOrHour('" + tbLateHour.ClientID + "');");
                #endregion

                #region Fill Vaiolation Type
                vFillDDLViolationType(ddlVaiolationType);
                #endregion

                #region Add JavaScript Function to Check Box And Get Check flag from datakey
                cbCheckFlage.Attributes.Add("onclick", "CheckThisCheckBox('" + gvStudentLectureVaiolations.ClientID + "','cbCheckFlageAll','cbCheckFlage',event);");
                ddlVaiolationType.Attributes.Add("onchange", "ShowLateTableAndValidation('" + ddlVaiolationType.ClientID + "','" + tblViolationLate.ClientID + "','" + tbLateMinute.ClientID + "','" + tbLateHour.ClientID + "','" + rfvLateMinute.ClientID + "','" + rfvtbLateHour.ClientID + "','" + cvtbLateMinute.ClientID + "','" + cvtbLateHour.ClientID + "');");
                bool bCheckFlag = oArrStdViolations[0].CheckFlag.Equals(1) ? true : false;
                //1.	Enable
                //0.	Disable
                int nEnableFlag = oArrStdViolations[0].EnableFlag;
                #endregion

                #region Assign returned value to students gridview
                cbCheckFlage.Checked = bCheckFlag;
                //     ddlVaiolationType.Enabled = bCheckFlag;
                if (!bCheckFlag)
                {
                    ddlVaiolationType.Attributes.Add("disabled", "disabled");

                }
                rfvddlVaiolationType.Enabled = bCheckFlag;

                if (!bCheckFlag)
                {
                    bCheckHeader = false;
                }

                #region DDLViolation Type
                int nItemsCount;
                for (nItemsCount = 0; nItemsCount < ddlVaiolationType.Items.Count; nItemsCount++)
                {
                    if (ddlVaiolationType.Items[nItemsCount].Value.Contains(oArrStdViolations[0].DeductID + ","))
                    {
                        break;
                    }
                }
                // get selected index
                if (nItemsCount != ddlVaiolationType.Items.Count)
                {
                    ddlVaiolationType.SelectedIndex = nItemsCount;
                }
                // show or hide late hour and minute if selected item is LATE violation type
                tbLateMinute.Text = string.Empty;
                tbLateHour.Text = string.Empty;
                rfvLateMinute.Enabled = false;
                rfvtbLateHour.Enabled = false;
                cvtbLateMinute.Enabled = false;
                cvtbLateHour.Enabled = false;
                tblViolationLate.Style["display"] = "none";

                if (!ddlVaiolationType.SelectedValue.Equals("-99") && ddlVaiolationType.SelectedValue.Split(',')[1].Equals("2"))
                {
                    string sLateMinute = oArrStdViolations[0].LateMinute;
                    int nLateHour = int.TryParse(sLateMinute, out nLateHour) ? (Convert.ToInt32(sLateMinute) / 60) : 0;
                    int nLateMinute = int.TryParse(sLateMinute, out nLateMinute) ? (Convert.ToInt32(sLateMinute) % 60) : 0;

                    tbLateHour.Text = nLateHour.ToString();
                    tbLateMinute.Text = nLateMinute.ToString();
                    tblViolationLate.Style["display"] = "block";
                    rfvLateMinute.Enabled = bCheckFlag;
                    rfvtbLateHour.Enabled = bCheckFlag;
                    cvtbLateMinute.Enabled = bCheckFlag;
                    cvtbLateHour.Enabled = bCheckFlag;
                }
                #endregion
                #endregion

                #region Enable And Disable Vaiolation Type DDL on print
                if (this.PrintMode == enumPrintMode.Print)
                {

                    ddlVaiolationType.Attributes.Add("disabled", "disabled");

                    tbLateMinute.Enabled = false;
                    tbLateHour.Enabled = false;
                }
                else
                {
                    if (!bCheckFlag)
                    {
                        ddlVaiolationType.Attributes.Add("disabled", "disabled");

                    }
                    else
                    {
                        ddlVaiolationType.Attributes.Remove("disabled");

                    }

                    tbLateMinute.Enabled = bCheckFlag;
                    tbLateHour.Enabled = bCheckFlag;
                }
                #endregion

                if (!Convert.ToBoolean(nEnableFlag))
                {
                    ddlVaiolationType.Attributes.Add("disabled", "disabled");
                    cbCheckFlage.Enabled = false;
                    nRowDisabledCount += 1;

                    if (this.ByViolationLectur)
                    {
                        e.Row.ToolTip = GetLocalResourceObject("IsDerived").ToString();
                    }
                    else
                    {
                        e.Row.ToolTip = GetLocalResourceObject("IsConfirmed").ToString();
                    }
                }
                if (ScriptManager.GetCurrent(this) != null)
                {
                    ScriptManager.GetCurrent(this).RegisterPostBackControl(e.Row.FindControl("lbtnViolationStudentName"));
                }
            }
            #endregion
            #region Footer Row
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                CheckBox cbCheckFlageAll = (CheckBox)gvStudentLectureVaiolations.HeaderRow.FindControl("cbCheckFlageAll");
                cbCheckFlageAll.Checked = bCheckHeader;

                ibtnSaveViolation.Visible = nRowDisabledCount != gvStudentLectureVaiolations.Rows.Count;
            }
            #endregion
        }
        #endregion

        #region Handler :: gvStudentLectureVaiolations_RowCommand
        /// <summary>
        /// Gvs student lecture vaiolations row command.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void gvStudentLectureVaiolations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("StudentArchiveing")) //√—‘Ì› «·ÿ·«»  -„Ê«Ÿ»… „” ÊÏ Õ’… 
            {
                // get StudentProfileID from Values Field in the returned DataTable
                StudentProfileID = Convert.ToInt32(e.CommandArgument);
                mvAttendance.SetActiveView(viewStudentAttendance);
                vBindStudentAttendanceUC();
                vInitiliazeUserInformationUserControl();
                tblDistributionUserControl.Visible = false;
                UserInformationsCC.Attributes.Add("printable", "true");
                ibtnPrint.Attributes.Add("onclick", "return PrintPage('" + GetGlobalResourceObject("Designs", "PageDirection").ToString() + "');");

                Title = string.Format(GetGlobalResourceObject("CommonMessages", "PageTitle").ToString(), GetLocalResourceObject("TitleMuadaba").ToString());
                PageTitle = GetLocalResourceObject("TitleMuadaba").ToString() + " " + ddlMowadaba.SelectedItem.Text;

                lblAttendanceHistoryTitle.Text = GetLocalResourceObject("MudabaLevel").ToString() + " " + ddlViolation.SelectedItem.Text;
                ITG_CustomControls.ITG_GridView gvArchiceStudentViolations = (ITG_CustomControls.ITG_GridView)oStudentAttendanceUC.FindControl("gvArchiceStudentViolations");
                tblPrintViolations.Visible = gvArchiceStudentViolations.Rows.Count > 0;
            }
            else if (e.CommandName.Equals("SendSMS", StringComparison.OrdinalIgnoreCase))
            {
                int nStdProfileID = Convert.ToInt32(e.CommandArgument);
                oArrViolations = oArrGetLectureViolations(nStdProfileID);

                string sRefPage = ITGTextEncryption.QuerystringEncryptKey("RefPage");

                string sRefPageValue = ITGTextEncryption.QuerystringEncryptKey("~/EduWaveSMS/ManageAttendance.aspx?");

                string sStudentName = ITGTextEncryption.QuerystringEncryptKey("StudentName");

                string sStudentNameValue = ITGTextEncryption.QuerystringEncryptValue(oArrViolations[0].StudentName);

                string sSendSMS = ITGTextEncryption.QuerystringEncryptKey("SendSMS");

                string sSendSMSValue = ITGTextEncryption.QuerystringEncryptKey("1");

                string sMuadabaCourseID = ITGTextEncryption.QuerystringEncryptKey("MuadabaCourseID");
                string sMuadabaCourseIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlMowadaba.SelectedValue);

                string sMuadabaLevelID = ITGTextEncryption.QuerystringEncryptKey("MuadabaLevelID");
                string sMuadabaLevelIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlViolation.SelectedValue);

                string sMobile = ITGTextEncryption.QuerystringEncryptKey("Mobile");
                string sMobileNumValue = ITGTextEncryption.QuerystringEncryptValue(oArrViolations[0].ParentMobile);

                string sID = ITGTextEncryption.QuerystringEncryptKey("ID");
                string sIDValue = ITGTextEncryption.QuerystringEncryptValue(oArrViolations[0].LecturesViolationID.ToString());

                string sFlagID = ITGTextEncryption.QuerystringEncryptKey("FlagID");
                string sFlagIDValue = ITGTextEncryption.QuerystringEncryptKey("2");

                string sClassID = ITGTextEncryption.QuerystringEncryptKey("ClassID");
                string sClassIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.ClassID.ToString());

                string sSectionID = ITGTextEncryption.QuerystringEncryptKey("SectionID");
                string sSectionIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.SectionID.ToString());

                string sSpecialityID = ITGTextEncryption.QuerystringEncryptKey("SpecialityID");
                string sSpecialityIDValue = ITGTextEncryption.QuerystringEncryptValue(oDistributionSearch.SpecialityID.ToString());

                string sDeductTypeID = ITGTextEncryption.QuerystringEncryptKey("DeductTypeID");
                string sDeductTypeIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlDeductType.SelectedValue.ToString());

                string sAttendanceDay = ITGTextEncryption.QuerystringEncryptKey("AttendanceDay");
                string sAttendanceDayValue = ITGTextEncryption.QuerystringEncryptValue(clrAttendanceDay.Text);

                string sLectureID = ITGTextEncryption.QuerystringEncryptKey("LectureID");
                string sLectureIDValue = ITGTextEncryption.QuerystringEncryptValue(ddlLecture.SelectedValue);

                string sStudentIdQueryString = ITGTextEncryption.QuerystringEncryptKey("StudentIdQueryString");
                string sStudentIdQueryStringValue = ITGTextEncryption.QuerystringEncryptValue(ddlStudents.SelectedValue);

                string sStudentIdQueryStringGV = ITGTextEncryption.QuerystringEncryptKey("StudentIdQueryStringGV");
                string sStudentIdQueryStringValueGV = ITGTextEncryption.QuerystringEncryptValue(nStdProfileID.ToString());

                string sMenuPageIDKey = ITGTextEncryption.QuerystringEncryptKey("MenuPageID");
                string sMenuPageIDValue = ITGTextEncryption.QuerystringEncryptValue(MenuPageID.ToString());

                CustomRedirect("~/EduWaveMessages/AbsentSMS.aspx?"
                    + sRefPage + "=" + sRefPageValue + "&"
                    + sSendSMS + "=" + sSendSMSValue + "&"
                    + sStudentName + "=" + sStudentNameValue + "&"
                    + sMobile + "=" + sMobileNumValue + "&"
                    + sMuadabaCourseID + "=" + sMuadabaCourseIDValue + "&"
                    + sID + "=" + sIDValue + "&"
                    + sFlagID + "=" + sFlagIDValue + "&"
                    + sMuadabaLevelID + "=" + sMuadabaLevelIDValue + "&"
                    + sSpecialityID + "=" + sSpecialityIDValue + "&"
                    + sSectionID + "=" + sSectionIDValue + "&"
                    + sDeductTypeID + "=" + sDeductTypeIDValue + "&"
                    + sAttendanceDay + "=" + sAttendanceDayValue + "&"
                    + sStudentIdQueryString + "=" + sStudentIdQueryStringValue + "&"
                    + sStudentIdQueryStringGV + "=" + sStudentIdQueryStringValueGV + "&"
                    + sLectureID + "=" + sLectureIDValue + "&"
                    + sMenuPageIDKey + "=" + sMenuPageIDValue + "&"
                    + sClassID + "=" + sClassIDValue);
            }
        }
        #endregion

        #endregion

        #region Handler :: lbtnPrintEmptyRecord_Click
        /// <summary>
        /// Lbtn print empty record click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void lbtnPrintEmptyRecord_Click(object sender, EventArgs e)
        {
            mvAttendance.SetActiveView(viewPrintEmptyRecord);
            tblDistributionUserControl.Visible = false;
            ShowAttendanceReportViewer();
        }
        #endregion

        #region Handler :: ibtnBackFromattendanceReport_Click
        /// <summary>
        /// Ibtn back fromattendance report click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnBackFromattendanceReport_Click(object sender, EventArgs e)
        {
            ibtnBackFromStudentAbsences_Click(sender, e);
        }
        #endregion

        #region Handler :: oDistributionSearch_ddlSectionIndex_Change
        /// <summary>
        /// Os distribution searchddl section index change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        void oDistributionSearch_ddlSectionIndex_Change(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            tableViewClassStudentsAttendance.Visible = false;
            lblNoDataFound.Visible = false;
            mvAttendance.ActiveViewIndex = -1;
            if (oDistributionSearch.ClassID.Equals(-99))
            {
                ddlLecture.Items.Clear();
                ddlLecture.Enabled = false;
                ddlLecture.Items.Insert(0, new ListItem(GetGlobalResourceObject("CommonMessages", "DropDownListNotExist").ToString(), "-99"));
            }
            else
            {
                vFillDateOfSelectSemester();
                clrAttendanceDay_TextChanged(null, null);
            }
            ibtnSaveViolation.Visible = false;
            gvStudentLectureVaiolations.Visible = false;
            vFillStudents();
            vFillMowadabaDDL();
            oDistributionSearch.SectionDDLValidation = true;

        }
        #endregion

        #region Handler :: oDistributionSearch_ddlClassIndex_Change
        /// <summary>
        /// Os distribution searchddl class index change.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        void oDistributionSearch_ddlClassIndex_Change(object sender, EventArgs e)
        {
            oDistributionSearch_ddlSectionIndex_Change(null, null);
            mvAttendance.ActiveViewIndex = -1;
            vFillStudents();
            vFillMowadabaDDL();
        }
        #endregion
        #region Handler :: ddlDeductType_SelectedIndexChanged
        /// <summary>
        /// Ddls deduct type selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ddlDeductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            mvAttendance.ActiveViewIndex = -1;
            lblNoDataFound.Visible = false;
            ibtnSaveViolation.Visible = false;
            gvStudentLectureVaiolations.Visible = false;


            if (ddlMowadaba.SelectedValue.Equals("1") && ddlDeductType.SelectedValue.Equals("1"))
            {
                gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("ViolationHeaderText").ToString();
            }
            else if (ddlMowadaba.SelectedValue.Equals("1") && ddlDeductType.SelectedValue.Equals("2"))
            {
                gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("PositiveType").ToString();
            }
            else if (ddlMowadaba.SelectedValue.Equals("2"))
            {
                gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("ViolationType").ToString();
            }

        }
        #endregion

        #region Handler :: ddlMowadaba_SelectedIndexChanged
        /// <summary>
        /// Ddls mowadaba selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ddlMowadaba_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            if (!string.IsNullOrEmpty(ddlMowadaba.SelectedValue) && ddlMowadaba.SelectedValue != "-99")
            {
                mvAttendance.ActiveViewIndex = -1;
            }
            if (ddlMowadaba.SelectedValue.Equals("2")) // when you select muadaba of type[„Ê«Ÿ»…]show ddl violations which contains [Õ’… «Ê ÌÊ„ ﬂ«„· ]
            {
                ddlViolation.SelectedIndex = 0;
                ddlViolation_SelectedIndexChanged(null, null);
                gvClassStudentsAttendance.Columns[5].HeaderText = GetLocalResourceObject("ViolationType").ToString();

                trDeductType.Visible = false;//hide deduct type
                trViolation.Visible = true;
            }
            else if (!ddlMowadaba.SelectedValue.Equals("-99"))
            {
                ddlDeductType.SelectedIndex = 0;
                ddlDeductType_SelectedIndexChanged(null, null);

                trDeductType.Visible = true; //show deduct type
                trViolation.Visible = false;
            }
            else
            {
                trDeductType.Visible = false;
                trViolation.Visible = false;
                this.ByViolationLectur = false;
            }
            trLectureDDL.Visible = false;
            ddlViolation.SelectedIndex = 0;
            mvAttendance.ActiveViewIndex = -1;
            lblNoDataFound.Visible = false;
            ibtnSaveViolation.Visible = false;
            gvStudentLectureVaiolations.Visible = false;
            vInitializeDistibutionUC();
        }
        #endregion

        #region Handler :: lbtnClickHere_Click
        /// <summary>
        /// Lbtns click here click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void lbtnClickHere_Click(object sender, EventArgs e)
        {
            if (PrintMode == enumPrintMode.Original)
            {
                lblPrinterFriendlyMsg.Text = GetGlobalResourceObject("CommonMessages", "ViewOriginalVersion").ToString();
                ibtnPrint.Visible = true;
                ibtnBackFromStudentAbsences.Visible = false;
                PrintMode = enumPrintMode.Print;
            }
            else
            {
                ibtnPrint.Visible = false;
                lblPrinterFriendlyMsg.Text = GetGlobalResourceObject("CommonMessages", "ViewFriendlyVersion").ToString();
                ibtnBackFromStudentAbsences.Visible = true;
                PrintMode = enumPrintMode.Original;
            }
            UserInformationsCC.Visible = true;

            vInitiliazeUserInformationUserControl();
            vBindStudentAttendanceUC();
        }
        #endregion

        #region Handler :: lbtnPrinterMsgClickHere_Click
        /// <summary>
        /// Lbtns printer msg click here click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void lbtnPrinterMsgClickHere_Click(object sender, EventArgs e)
        {
            lblNoDataFound.Text = lblOperationResult.Text = string.Empty;
            int nRowsCount = gvClassStudentsAttendance.Rows.Count;
            if (PrintMode == enumPrintMode.Original)
            {
                lbtnPrinter.Visible = true;
                lblPrinterMsg.Text = GetGlobalResourceObject("CommonMessages", "ViewOriginalVersion").ToString();
                ibtnSave.Visible = false;
                tblSearchBackButtons.Visible = false;
                IndicatesMandatoryField1.Visible = false;
                tblEmptyReport.Visible = false;
                ddlMowadaba.Enabled = false;
                ddlDeductType.Enabled = false;
                ddlViolation.Enabled = false;
                ddlLecture.Enabled = false;
                ddlStudents.Enabled = false;
                oDistributionSearch.DisableEnableAll = false;
                gvClassStudentsAttendance.Columns[0].Visible = false;
                PrintMode = enumPrintMode.Print;
                lblAttendanceDateValue.Text = clrAttendanceDay.Text;
                tdlblAttendanceDateValue.Visible = true;
                tdclrAttendanceDay.Visible = false;
                gvClassStudentsAttendance.AllowSorting = false;
                tblGuideMessageFiltering.Visible = false;
                lblSelectDay.Text = GetLocalResourceObject("Date").ToString();
                tblNoViolationConfirmation.Visible = false;
            }
            else
            {
                lbtnPrinter.Visible = false;
                lblPrinterMsg.Text = GetGlobalResourceObject("CommonMessages", "ViewFriendlyVersion").ToString();
                tblSearchBackButtons.Visible = true;
                IndicatesMandatoryField1.Visible = true;
                tblEmptyReport.Visible = true;
                ddlMowadaba.Enabled = true;
                ddlDeductType.Enabled = true;
                ddlViolation.Enabled = true;
                ddlLecture.Enabled = true;
                ddlStudents.Enabled = true;
                oDistributionSearch.DisableEnableAll = true;
                DropDownList ddlSection = (DropDownList)oDistributionSearch.FindControl("ddlSection");
                oDistributionSearch.DDLSectionEnabled = ddlSection.Items.Count > 1;

                if (nGetActiveSemster().Equals(-100)) // Handle on Sort 
                {
                    gvClassStudentsAttendance.Columns[0].Visible = false;
                    ibtnSave.Visible = false;
                }
                else
                {
                    gvClassStudentsAttendance.Columns[0].Visible = true;
                    ibtnSave.Visible = gvClassStudentsAttendance.Rows.Count > 0 && PrintMode == enumPrintMode.Original && ((this.AllowEditForSchoolAdmin && AllowEditWeek) || EditAbsencePrivilege);
                }
                PrintMode = enumPrintMode.Original;
                tdclrAttendanceDay.Visible = true;
                tdlblAttendanceDateValue.Visible = false;
                gvClassStudentsAttendance.AllowSorting = true;
                tblGuideMessageFiltering.Visible = true;
                lblSelectDay.Text = GetLocalResourceObject("lblSelectDayResource1.Text").ToString();
                tblNoViolationConfirmation.Visible = oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School)
                    || oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector;
            }
            gvClassStudentsAttendance.DataBind();
            gvClassStudentsAttendance.Columns[7].Visible = false;
        }
        #endregion

        #region Handler :: ddlViolation_SelectedIndexChanged
        /// <summary>
        /// Ddls violation selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ddlViolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            mvAttendance.ActiveViewIndex = -1;
            lblNoDataFound.Visible = false;
            if (ddlViolation.SelectedValue.Equals("2"))
            {
                this.ByViolationLectur = true;
                trLectureDDL.Visible = true;
                clrAttendanceDay_TextChanged(null, null);
            }
            else
            {
                this.ByViolationLectur = false;
                trLectureDDL.Visible = false;
            }
            ibtnSaveViolation.Visible = false;
            gvStudentLectureVaiolations.Visible = false;
            vInitializeDistibutionUC();
        }
        #endregion

        #region Handler :: ddlLecture_SelectedIndexChanged
        /// <summary>
        /// Ddls lecture selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ddlLecture_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            mvAttendance.ActiveViewIndex = -1;
            lblNoDataFound.Visible = false;
            gvStudentLectureVaiolations.Visible = false;
            ibtnSaveViolation.Visible = false;
            gvStudentLectureVaiolations.Visible = false;
        }
        #endregion

        #region Handler :: ibtnSaveViolation_Click
        /// <summary>
        /// Ibtn save violation click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ibtnSaveViolation_Click(object sender, EventArgs e)
        {
            #region StringBuilders
            StringBuilder sbUserProfileIDs = new StringBuilder();
            StringBuilder sbDeductIDs = new StringBuilder();
            StringBuilder sbLateMinute = new StringBuilder();
            StringBuilder sbSmsSendingCount = new StringBuilder();
            #endregion

            #region Loop to collect the (UserProfileIDs && DeductIDs) from students grid
            int nGridViewRowCount = gvStudentLectureVaiolations.Rows.Count;
            oArrViolations = oArrGetLectureViolations(!string.IsNullOrEmpty(ddlStudents.SelectedValue) && !ddlStudents.SelectedValue.Equals("-99") ? Convert.ToInt32(ddlStudents.SelectedValue) : -99);
            int nStdProfileID = -99;
            int nSmsSendingCount = -99;

            for (int nRowCount = 0; nRowCount < nGridViewRowCount; nRowCount++)
            {
                GridViewRow oGridViewRow = gvStudentLectureVaiolations.Rows[nRowCount];
                if (oGridViewRow.RowType == DataControlRowType.DataRow)
                {
                    #region GridView Controls Referance
                    CheckBox cbCheckFlage = (CheckBox)oGridViewRow.FindControl("cbCheckFlage");
                    DropDownList ddlVaiolationType = (DropDownList)oGridViewRow.FindControl("ddlVaiolationType");
                    TextBox tbLateMinute = (TextBox)oGridViewRow.FindControl("tbLateMinute");
                    TextBox tbLateHour = (TextBox)oGridViewRow.FindControl("tbLateHour");
                    #endregion

                    if (cbCheckFlage.Checked && !ddlVaiolationType.SelectedValue.Equals("-99") && oGridViewRow.Enabled)
                    {
                        nStdProfileID = Convert.ToInt32(ITG_CustomControls.ITG_GridView.sGetDataKeyValue(gvStudentLectureVaiolations, nRowCount, "StudentProfileID"));
                        nSmsSendingCount = (from item in oArrViolations
                                            where item.StudentProfileID.Equals(nStdProfileID)
                                            select item.SmsSendingCount).First();

                        sbUserProfileIDs.Append(nStdProfileID);
                        sbDeductIDs.Append(ddlVaiolationType.SelectedValue.Split(',')[0]);

                        if (oClientInfo.UserType == UsersTypes.SchoolAdmin)
                        {
                            if (nSmsSendingCount != -99)
                            {
                                sbSmsSendingCount.Append(nSmsSendingCount);
                                sbSmsSendingCount.Append(',');
                            }
                        }
                        HtmlTable tblViolationLate = (HtmlTable)oGridViewRow.FindControl("tblViolationLate");
                        // [1] refear on on AbsenceTypeID (  √ŒÌ— «Ê €Ì«» )
                        if (ddlVaiolationType.SelectedValue.Split(',')[1].Equals("2"))
                        {
                            int nTotalMinute = (Convert.ToInt32(tbLateHour.Text) * 60) + Convert.ToInt32(tbLateMinute.Text);
                            sbLateMinute.Append(nTotalMinute.ToString());
                            tblViolationLate.Style["display"] = "block";
                        }
                        else
                        {
                            sbLateMinute.Append(0);
                            tblViolationLate.Style["display"] = "none";
                        }
                        sbLateMinute.Append(",");
                        // Append (,) into string till last item in the gridview
                        if (nRowCount != nGridViewRowCount - 1)
                        {
                            sbUserProfileIDs.Append(",");
                            sbDeductIDs.Append(",");
                        }
                    }
                }
            }
            #endregion

            #region Addign Data To Violations
            Violations oViolations = new Violations();
            oViolations.DBConnectionString = oEduSettings.DBConnectionString;
            if (oClientInfo.UserType.Equals(UT.DirectorOfGuidanceInDistrict) || oClientInfo.UserType.Equals(UT.DistrictExaminationAdmissionDirector))
            {
                oViolations.SchoolID = oDistributionSearch.SchoolID;
            }
            else
            {
                oViolations.SchoolID = oClientInfo.SchoolID;
            }
            oViolations.ClassID = oDistributionSearch.ClassID;
            oViolations.SpecialtyID = oDistributionSearch.SpecialityID;
            oViolations.SectionID = oDistributionSearch.SectionID;
            oViolations.SemesterCodeID = SemesterCodeID;
            oViolations.MuadabaCourseID = Convert.ToInt32(ddlMowadaba.SelectedValue);
            oViolations.CreatedBy = oClientInfo.UserProfileID;
            oViolations.SmsSendingCounts = sbSmsSendingCount.ToString().TrimEnd(',');
            oViolations.FilteredUserProfileID = Convert.ToInt32(ddlStudents.SelectedValue);
            // if there is at least one check in the grid 
            if (!sbUserProfileIDs.ToString().Equals(string.Empty) && !sbDeductIDs.ToString().Equals(string.Empty))
            {
                oViolations.DeductIDs = sbDeductIDs.ToString().TrimEnd(',');
                oViolations.UserProfileIDs = sbUserProfileIDs.ToString().TrimEnd(',');
            }
            if (sbLateMinute.ToString() != string.Empty)
            {
                oViolations.LateMinute = sbLateMinute.ToString().TrimEnd(',');
            }
            oViolations.MuadabaLevelID = Convert.ToInt32(ddlViolation.SelectedValue);
            oViolations.EnumViolationsFlags = Violations.EnumViolationFlag.ArchiveStudentByMuadabaLevel;
            oViolations.ViolationDate = Convert.ToDateTime(clrAttendanceDay.Text);
            oViolations.StudySystem = oDistributionSearch.StudySystemID;
            if (this.ByViolationLectur)
            {
                oViolations.ClassScheduleID = Convert.ToInt32(ddlLecture.SelectedValue);
            }
            oViolations.LectureID = Convert.ToInt32(ddlLecture.SelectedValue);
            oViolations.vSaveViolations();
            #endregion

            #region OperationResult
            if (oViolations.OperationResult > 0) // Save Violations Success
            {
                lblResult.Text = GetLocalResourceObject("SaveSuccess").ToString();
                gvStudentLectureVaiolations.DataBind(); // Bind data to gvStudentLectureVaiolationss 
            }
            else if (oViolations.OperationResult == -3)
            {
                lblResult.Text = GetLocalResourceObject("HoursInstalledForTheDay").ToString();
            }
            else if (oViolations.OperationResult == -4)
            {
                lblResult.Text = GetLocalResourceObject("HolidayError").ToString();
            }
            else if (oViolations.OperationResult.Equals(-5))
            {
                lblResult.Text = GetLocalResourceObject("ConnotAddAbsenceInSectionNotActive").ToString();
            }
            else if (oViolations.OperationResult.Equals(-6))
            {
                lblResult.Text = GetLocalResourceObject("CannotEditPreviousDay").ToString();
            }
            else if (oViolations.OperationResult.Equals(-7))
            {
                lblResult.Text = GetLocalResourceObject("CannotEdit").ToString();
            }
            else if (oViolations.OperationResult.Equals(-8))
            {
                lblResult.Text = GetLocalResourceObject("CannotEditt").ToString();
            }
            else if (oViolations.OperationResult.Equals(0)) // Save Violations Faild
            {
                lblResult.Text = GetLocalResourceObject("SaveFaild").ToString();
            }
            #endregion
        }
        #endregion

        #region Handler :: ddlStudents_SelectedIndexChanged
        /// <summary>
        /// Ddls students selected index changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        protected void ddlStudents_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            mvAttendance.ActiveViewIndex = -1;
            lblNoDataFound.Visible = false;
        }
        #endregion

        #region Handler :: NoViolation_ConfirmYesButtonEvent
        /// <summary>
        /// No violation confirm yes button event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        void NoViolation_ConfirmYesButtonEvent(object sender, EventArgs e)
        {
            lblConfirmationResult.Text = string.Empty;
            DateTime dtiSelectedDate = !string.IsNullOrEmpty(clrAttendanceDay.Text) ? Convert.ToDateTime(clrAttendanceDay.Text) : DateTime.Now;
            if ((DateTime.Now.DayOfWeek.Equals(DayOfWeek.Thursday)
                && (dtiSelectedDate <= DateTime.Now && dtiSelectedDate > DateTime.Now.AddDays(-8))
                && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || dtiSelectedDate.Date.Equals(DateTime.Now.Date)
                || (EditAbsencePrivilege && oClientInfo.HierarchyLevelID.Equals((int)DistributionLevel.School))
                || (oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector))
            {
                SMSCom.SchoolViolationStatus oSchoolViolationStatus = new SMSCom.SchoolViolationStatus();
                oSchoolViolationStatus.DBConnectionString = oEduSettings.DBConnectionString;
                oSchoolViolationStatus.CreatedBy = oClientInfo.UserProfileID;
                if (oClientInfo.UserType == UT.DistrictExaminationAdmissionDirector)
                {
                    oSchoolViolationStatus.SchoolID = oDistributionSearch.SchoolID;
                }
                else
                {
                    oSchoolViolationStatus.SchoolID = oClientInfo.SchoolID;
                }
                oSchoolViolationStatus.ViolationDateSTR = dtiSelectedDate.ToShortDateString();
                oSchoolViolationStatus.StatusIDs = "1";
                oSchoolViolationStatus.vSaveSchoolViolationStatus();
                if (oSchoolViolationStatus.OperationResult.Equals(1))
                {
                    lblConfirmationResult.Text = GetLocalResourceObject("ConfirmationSucceed").ToString();
                    vShowNoViolationConfirmation();
                }
                else if (oSchoolViolationStatus.OperationResult.Equals(-1))
                {
                    lblConfirmationResult.Text = GetLocalResourceObject("ConfirmationFailedError_1").ToString();
                }
                else
                {
                    lblConfirmationResult.Text = GetLocalResourceObject("ConfirmationFailed").ToString();
                }
            }
        }
        #endregion

        #region  Handler :: Handler_AddBookMarksButtonEvent
        /// <summary>
        /// Handler add book marks button event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The E.</param>
        void Handler_AddBookMarksButtonEvent(object sender, EventArgs e)
        {
            StringBuilder sbSearchDataControlsID = new StringBuilder();
            sbSearchDataControlsID.Clear();
            sbSearchDataControlsID.Append("oDistributionSearch_ddlClass=" + oDistributionSearch.ClassID + "," + oDistributionSearch.SpecialtyFlag + "&&");
            sbSearchDataControlsID.Append("oDistributionSearch_ddlSpecialty=" + oDistributionSearch.SpecialityID + "," + oDistributionSearch.CustomSkillsFlag + "&&");
            sbSearchDataControlsID.Append("oDistributionSearch_ddlSection=" + oDistributionSearch.SectionID + "&&");
            sbSearchDataControlsID.Append("ddlMowadaba=" + ddlMowadaba.SelectedValue + "&&");
            sbSearchDataControlsID.Append("ddlDeductType=" + ddlDeductType.SelectedValue + "&&");
            sbSearchDataControlsID.Append("ddlViolation=" + ddlViolation.SelectedValue + "&&");
            sbSearchDataControlsID.Append("clrAttendanceDay=" + clrAttendanceDay.Text + "&&");
            sbSearchDataControlsID.Append("ddlLecture=" + ddlLecture.SelectedValue + "&&");
            sbSearchDataControlsID.Append("ddlStudents=" + ddlStudents.SelectedValue);

            string sQueryString = string.Empty;
            if (this.MenuPageID > 0)
            {
                sQueryString = "?" + ITGTextEncryption.QuerystringEncryptKey("MenuPageID") + "=" + ITGTextEncryption.QuerystringEncryptValue(this.MenuPageID.ToString());
            }
            BookMark oBookMark = new BookMark();
            oBookMark.UserProfileID = oClientInfo.UserProfileID;
            oBookMark.BookMarkDesc = this.BookMarksName;
            oBookMark.FolderID = this.FolderIDBookMarks;
            oBookMark.URLLink = "EduWaveSMS/ManageAttendance.aspx" + sQueryString;
            oBookMark.PageViewID = string.Empty;//No found View 
            oBookMark.PageTapID = string.Empty;//No found tap
            oBookMark.PageID = this.PageIDBookMarks;
            oBookMark.ControlsIDsWithValues = sbSearchDataControlsID.ToString();

            BookMarksCom oBookMarksCom = new BookMarksCom();
            oBookMarksCom.DBConnectionString = oEduSettings.DBConnectionString;
            int nResult = oBookMarksCom.AddBookMarkForUser(oBookMark);
            ScriptManager.RegisterStartupScript(this, GetType(), "ShowNotifyBookMarks", "ShowNotifyBookMarks('" + nResult + "','.sectionHeaderTools');", true);
        }
        #endregion

        #endregion Handlers
    }
}
