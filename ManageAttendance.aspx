<%@ Page Language="C#" MasterPageFile="<%$ Resources:CommonMessages, MasterPageURL %>"
    AutoEventWireup="true" CodeBehind="ManageAttendance.aspx.cs" Inherits="EduWave.K12SMS.ManageAttendance"
    Title="Manage Attendance" meta:resourcekey="PageResource1" EnableEventValidation="false"
    MaintainScrollPositionOnPostback="true" %>

<%@ Register Src="StudentAttendanceUC.ascx" TagName="StudentAttendanceUC" TagPrefix="uc1" %>
<%@ Register Assembly="EduWave.CustomControls" Namespace="UserInformations" TagPrefix="cc1" %>
<%@ Register Assembly="ITG_CustomControls" Namespace="ITG_CustomControls" TagPrefix="cc1" %>
<%@ Register Src="../UserControls/DistributionSearchUC.ascx" TagName="DistributionSearchUC"
    TagPrefix="uc2" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845DCD8080CC91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="contentMain" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <style>
        #div-2 {
            display: flex;
            flex-direction: row;
        }
    </style>

    <asp:UpdatePanel ID="UpdatePanelMain" runat="server">
        <ContentTemplate>
            <div id="tblDistributionUserControl" runat="server">
                <div class="form-operation-links">
                    <div class="form-operation-links-body divscrollbar-horizontal">
                        <asp:UpdatePanel ID="upGuideMessageFiltering" runat="server">
                            <ContentTemplate>
                                <table aria-describedby="TableDesc" cellspacing="1" id="tblGuideMessageFiltering"
                                    runat="server">
                                    <tr>
                                        <td>
                                            <i class="itg-icons bullet"></i>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblGuideMessageFiltering" runat="server" meta:resourcekey="lblGuideMessageFilteringResource1"
                                                SkinID="Instructions" Text="Choose the desired search creteria and hit [Search]"
                                                EnableViewState="false"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanel15" runat="server">
                            <ContentTemplate>
                                <table aria-describedby="TableDesc" cellspacing="1" id="tblNoViolationConfirmation"
                                    runat="server" visible="false">
                                    <tr>
                                        <td>
                                            <i class="itg-icons bullet"></i>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblNoViolationConfirmation" runat="server" meta:resourcekey="lblNoViolationConfirmation"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lbtnNoViolationConfirmation" runat="server" Text="<%$ Resources:CommonMessages, ClickHereText %>"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="form-controls">
                    <div class="form-controls-header">
                        <asp:Label ID="lblFilers" runat="server" SkinID="searchtitle"
                            Text="<%$ Resources:CommonMessages,SearchFields %>"></asp:Label>
                    </div>
                    <div class="form-controls-body">
                        <div class="row feild_row">
                            <div class="col-sm-6 col-12 col-lg-6 no_padding">
                                <uc2:distributionsearchuc id="oDistributionSearch"
                                    runat="server" />
                            </div>
                            <asp:HiddenField ID="hfSemster" runat="server" />
                            <div class="col-sm-6 col-12 col-lg-6 no_padding">
                                <div class="col-12 no_padding">
                                    <div class="col-5 col-sm-4 feild_title">
                                        <span class="manditory">*</span>
                                        <asp:Label ID="lblTitleModubaCourses" runat="server" Text="Moudba Courses"
                                            meta:resourcekey="lblTitleModubaCoursesResource1" EnableViewState="false"></asp:Label>
                                    </div>
                                    <div class="col-7 col-sm-8 feild_data">
                                        <asp:DropDownList ID="ddlMowadaba" runat="server" Width="100%"
                                            meta:resourcekey="ddlMowadabaResource1" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlMowadaba_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvMudabaCourses" runat="server"
                                            ErrorMessage="Select Mudaba Course." ValidationGroup="SearchValidation" Display="Dynamic"
                                            InitialValue="-99" ControlToValidate="ddlMowadaba"
                                            meta:resourcekey="rfvMudabaCoursesResource1"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-12 no_padding" runat="server" id="trDeductType" visible="false">
                                    <div class="col-5 col-sm-4 feild_title">
                                        <span class="manditory">*</span>
                                        <asp:Label ID="lblDeductType" runat="server" Text="Deduct Type"
                                            meta:resourcekey="lblDeductTypeResource1" EnableViewState="false"></asp:Label>
                                    </div>
                                    <div class="col-7 col-sm-8 feild_data">
                                        <asp:DropDownList ID="ddlDeductType" runat="server" Width="100%"
                                            meta:resourcekey="ddlDeductTypeResource1" AutoPostBack="True"
                                            OnSelectedIndexChanged="ddlDeductType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rvDeductType" runat="server"
                                            ErrorMessage="Select Deduct Type Course." ValidationGroup="SearchValidation"
                                            InitialValue="-99" Display="Dynamic" ControlToValidate="ddlDeductType"
                                            meta:resourcekey="rvDeductTypeResource1"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:UpdatePanel class="col-12 no_padding" ID="trViolation" runat="server" Visible="false">
                                    <ContentTemplate>
                                        <div class="col-5 col-sm-4 feild_title">
                                            <span class="manditory">*</span>
                                            <asp:Label ID="lblViolation" runat="server" Text="Violation"
                                                meta:resourcekey="lblTitleViolation" EnableViewState="false"></asp:Label>
                                        </div>
                                        <div class="col-7 col-sm-8 feild_data">
                                            <asp:DropDownList ID="ddlViolation" runat="server" Width="100%"
                                                meta:resourcekey="ddlMowadabaResource1" AutoPostBack="True"
                                                OnSelectedIndexChanged="ddlViolation_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rvViolation" runat="server" ErrorMessage="Select Violation."
                                                ValidationGroup="SearchValidation" Display="Dynamic" InitialValue="-99"
                                                ControlToValidate="ddlViolation" meta:resourcekey="rvViolationResource">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="col-12 no_padding">
                                    <asp:UpdatePanel class="col-5 col-sm-4 feild_title" ID="up" runat="server">
                                        <ContentTemplate>
                                            <span class="manditory">*</span>
                                            <asp:Label ID="lblSelectDay" runat="server" Text="Select day"
                                                meta:resourcekey="lblSelectDayResource1"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel class="col-7 col-sm-8 feild_data" ID="upAttendanceDate" runat="server">
                                        <ContentTemplate>
                                            <div id="tdclrAttendanceDay" runat="server">
                                                <asp:HiddenField ID="hdnSelectedAttendanceDate" runat="server" />
                                                <cc1:ITG_Calendar Width="100%" ID="clrAttendanceDay" runat="server" AutoPostBack="false"
                                                    OnUpdateFunction="AttendanceDateCalendarClosed"
                                                    OnTextChanged="clrAttendanceDay_TextChanged" ClientCulture="" DateTextFunction=""
                                                    DateTimeSelectionType="Date" DisableFunction="DisableAfterTodayDate"
                                                    EnableClear="False" End="2200" Layout="Normal"
                                                    meta:resourcekey="clrAttendanceDayResource1" ServerCulture="" Start="1900"
                                                    HijriCalenderPage="calendar"></cc1:ITG_Calendar>
                                            </div>
                                            <asp:Label ID="lblDateValue" runat="server" SkinID="Validation"></asp:Label>

                                            <div id="tdlblAttendanceDateValue" runat="server">
                                                <asp:Label ID="lblAttendanceDateValue" runat="server"></asp:Label>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvAttendanceDay" runat="server"
                                                ControlToValidate="clrAttendanceDay" Display="Dynamic"
                                                ErrorMessage="Select attendance day" meta:resourcekey="rfvAttendanceDayResource1">
                                            </asp:RequiredFieldValidator>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <asp:UpdatePanel class="col-12 no_padding" runat="server" ID="trLectureDDL" Visible="false">
                                    <ContentTemplate>
                                        <div class="col-5 col-sm-4 feild_title">
                                            <span class="manditory">*</span>
                                            <asp:Label ID="lblLecture" runat="server" meta:resourcekey="lblLecture" EnableViewState="false"></asp:Label>
                                        </div>
                                        <div class="col-7 col-sm-8 feild_data">
                                            <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList DataTextField="LectureDescription" DataValueField="ClassScheduleID"
                                                        ID="ddlLecture" AutoPostBack="true" OnSelectedIndexChanged="ddlLecture_SelectedIndexChanged"
                                                        runat="server" Width="100%">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <asp:RequiredFieldValidator ID="rfvLecture" runat="server"
                                                Display="Dynamic" InitialValue="-99" meta:resourcekey="rfvLecture" ControlToValidate="ddlLecture"></asp:RequiredFieldValidator>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdatePanel class="col-12 no_padding" runat="server" ID="trStudents" Visible="true">
                                    <ContentTemplate>
                                        <div class="col-5 col-sm-4 feild_title">
                                            <span class="manditory"></span>
                                            <asp:Label ID="lblStudents" runat="server" meta:resourcekey="lblStudentsResource1"
                                                EnableViewState="false"></asp:Label>
                                        </div>
                                        <div class="col-7 col-sm-8 feild_data">
                                            <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                                                <ContentTemplate>
                                                    <asp:DropDownList ID="ddlStudents" AutoPostBack="true" runat="server" Width="100%"
                                                        OnSelectedIndexChanged="ddlStudents_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-mandatory-field">
                    <asp:UpdatePanel ID="upIndicatesMandatory" runat="server">
                        <ContentTemplate>
                            <cc1:IndicatesMandatoryField ID="IndicatesMandatoryField1" runat="server" meta:resourcekey="IndicatesMandatoryField1Resource1" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-operation-message">
                    <asp:UpdatePanel ID="UpdatePanel16" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblConfirmationResult" runat="server" SkinID="Validation" EnableViewState="false"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="form-operation-message">
                    <asp:Label ID="lblNoDataFound" runat="server" SkinID="Validation" Visible="false"
                        EnableViewState="false"
                        meta:resourcekey="lblNoDataFoundResource1"></asp:Label>
                </div>
                <div class="form-actions">
                    <asp:UpdatePanel ID="upSearchBackButtons" runat="server">
                        <ContentTemplate>
                            <ul id="tblSearchBackButtons" runat="server">
                                <li>
                                    <asp:Button ID="ibtnSearch" runat="server" Text="<%$ Resources:ButtonsToolTip, SearchButton %>"
                                        ValidationGroup="SearchValidation"
                                        OnClick="ibtnSearch_Click"
                                        ToolTip="<%$ Resources:ButtonsToolTip, SearchButton %>" />
                                </li>
                                <li>
                                    <asp:Button ID="ibtnBackMain" runat="server" Text="<%$ Resources:ButtonsToolTip, BackButton %>"
                                        CausesValidation="False" OnClick="ibtnBackMain_Click"
                                        ToolTip="<%$ Resources:ButtonsToolTip, BackButton %>" />
                                </li>
                            </ul>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <input id="hdnDeductIDs" runat="server" type="hidden" />
            <asp:MultiView ID="mvAttendance" runat="server">
                <asp:View ID="viewClassStudentsAttendance" runat="server">
                    <div id="tableViewClassStudentsAttendance" runat="server">
                        <div class="form-operation-links">
                            <div class="form-operation-links-body divscrollbar-horizontal">
                                <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                    <ContentTemplate>
                                        <table aria-describedby="TableDesc" id="tblEmptyReport" runat="server">
                                            <tr id="trEmptyReport" runat="server">
                                                <td>
                                                    <i class="itg-icons bullet"></i>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblAbsenceTip" runat="server" meta:resourcekey="lblAbsenceTipResource1"
                                                        SkinID="Instructions" Text="To register an absence, select the absent student then click [Save]."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <i class="itg-icons bullet"></i>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblToAddNewExamcommittee" runat="server" meta:resourcekey="lblToAddNewExamcommitteeResource1"
                                                        Text="To print an empty attendence records" EnableViewState="false"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lbtnPrintEmptyRecord" runat="server" OnClick="lbtnPrintEmptyRecord_Click"
                                                        CausesValidation="false" Text="<%$ Resources:CommonMessages, ClickHereText %>"
                                                        ToolTip="<%$ Resources:CommonMessages, ClickHereText %>"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-table">
                            <div class="form-table-print">
                                <asp:UpdatePanel ID="oUpdatePanelPrinterMsg" runat="server">
                                    <ContentTemplate>
                                        <table aria-describedby="TableDesc" id="tblPrinterMsg" runat="server" style="width: 98%;">
                                            <tr>
                                                <td>
                                                    <table aria-describedby="TableDesc" cellpadding="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPrinterMsg" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lbtnPrinterMsgClickHere" runat="server" Text="<%$ Resources:CommonMessages, ClickHereText %>"
                                                                    ToolTip="<%$ Resources:CommonMessages, ClickHereText %>" CausesValidation="False"
                                                                    OnClick="lbtnPrinterMsgClickHere_Click"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="lbtnPrinter" runat="server" AlternateText="<%$ Resources:ButtonsToolTip, PrintButton %>"
                                                        ImageUrl="<%$ Resources:CommonImages, PrintIcon %>" ToolTip="<%$ Resources:ButtonsToolTip, PrintButton %>"
                                                        Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="form-table-body divscrollbar-horizontal" printable="true">
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <cc1:ITG_GridView ID="gvClassStudentsAttendance" runat="server" AutoGenerateColumns="False"
                                            EnableRowHighlight="False" InitialSortDirection="Ascending" OnRowDataBound="gvClassStudentsAttendance_RowDataBound"
                                            RowHighlightColor="Gainsboro" InitialSortExpression="StudentFullName"
                                            DataKeyNames="StudentProfileID,DegreeMuadabaCourseDesc,ViolationName"
                                            OnRowCommand="gvClassStudentsAttendance_RowCommand" NoRowsWarningMessage="There are no students in this section."
                                            meta:resourcekey="gvClassStudentsAttendanceResource1" TotalRowsCountMessage="Total rows count: "
                                            TotalRowsCountMessageDirection="Left" AllowPaging="False" Width="100%">
                                            <Columns>
                                                <asp:TemplateField meta:resourcekey="TemplateFieldResource2">
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="cbHeader" runat="server" meta:resourcekey="cbHeaderResource1" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="cbItem" runat="server" Checked='<%# Bind("IsAbsence") %>' meta:resourceKey="cbItemResource1" />
                                                    </ItemTemplate>
                                                    <ItemStyle Width="7px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Student Name" meta:resourcekey="TemplateFieldResource3"
                                                    SortExpression="StudentFullName">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnStudentFullName" runat="server" CommandName="StudentAttendance"
                                                            CausesValidation="false" meta:resourcekey="lbtnStudentFullNameResource1" Text='<%# Bind("StudentFullName") %>'
                                                            CommandArgument='<%# Bind("StudentProfileID") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle Width="100%" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ClassDesc" meta:resourcekey="ClassDesc" SortExpression="ClassDesc" />
                                                <asp:BoundField DataField="SpecialtyDesc" meta:resourcekey="SpecialtyDesc" SortExpression="SpecialtyDesc" />
                                                <asp:BoundField DataField="SectionDesc" meta:resourcekey="SectionDesc" SortExpression="SectionDesc" />
                                                <asp:TemplateField HeaderText="Violation Type" meta:resourcekey="ViolationTypeResource">
                                                    <ItemTemplate>
                                                        <table aria-describedby="TableDesc">
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList Width="100%" ID="ddlDegreeDeductAmount" runat="server">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td>
                                                                    <asp:RequiredFieldValidator ID="rfvDegreeDeductAmount" runat="server" ControlToValidate="ddlDegreeDeductAmount"
                                                                        ErrorMessage="Select Degree Deduct Amount." ValidationGroup="SaveValidation"
                                                                        Enabled="true" InitialValue="-99" meta:resourcekey="rfvDegreeDeductAmountResource1"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DegreeMuadabaCourseDesc" meta:resourcekey="DegreeMuadabaCourseDesc"
                                                    SortExpression="DegreeMuadabaCourseDesc">
                                                    <ItemTemplate>
                                                        <asp:Label Width="100%" ID="lblDegreeMuadabaCourseDesc" runat="server" Text='<%# Bind("DegreeMuadabaCourseDesc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Options" meta:resourcekey="Options">
                                                    <ItemTemplate>
                                                        <table aria-describedby="TableDesc" visible="false" cellpadding="5" cellspacing="2" class="tblOptions"
                                                            id="tblSendSMS" runat="server">
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnSend" runat="server" CommandName="SendSMS" CausesValidation="False"
                                                                        Text="Edit" meta:resourcekey="SendSMS" CommandArgument='<%# Bind("StudentProfileID") %>'></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </cc1:ITG_GridView>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="form-mandatory-field">
                            <asp:CheckBox ID="cbAgreement1" runat="server" Checked="true" meta:resourcekey="cbAgreement" />
                        </div>
                        <div class="form-operation-message">
                            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                                <ContentTemplate>
                                    <asp:Label ID="lblOperationResult" runat="server" meta:resourcekey="lblOperationResultResource1"
                                        SkinID="Validation" EnableViewState="false"></asp:Label>
                                    <br />
                                    <div runat="server" id="divquestionnaire" visible="false">
                                        <asp:Label ID="lblquestionnaire" SkinID="Validation" runat="server" meta:resourcekey="lblquestionnaire"></asp:Label>
                                        <a href="https://customerexp.moe.gov.sa/s/952F4475F2E1426FA0DF09BACE8DC4DD?PlatformOrCenterID=95&ServiceID=2427"
                                            target="_blank" rel="noopener">
                                            <asp:Label runat="server" Style="color: #115c74" Text="<%$ Resources:CommonMessages, ClickHereText %>" />
                                        </a>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="form-actions">
                            <ul>
                                <li>
                                    <asp:UpdatePanel ID="upSaveButton" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="ibtnSave" runat="server" Text="<%$ Resources:ButtonsToolTip, SaveButton %>"
                                                ToolTip="<%$ Resources:ButtonsToolTip, SaveButton %>"
                                                ValidationGroup="SaveValidation" OnClick="ibtnSave_Click" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </li>
                            </ul>
                        </div>
                    </div>
                </asp:View>
                <asp:View ID="ViewOneLecture" runat="server">
                    <div class="form-table">
                        <div class="form-table-body divscrollbar-horizontal">
                            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                                <ContentTemplate>
                                    <cc1:ITG_GridView ID="gvStudentLectureVaiolations" Width="100%" runat="server" AutoGenerateColumns="false"
                                        AllowPaging="False" InitialSortDirection="Ascending" OnRowCommand="gvStudentLectureVaiolations_RowCommand"
                                        InitialSortExpression="StudentName" DataKeyNames="StudentProfileID"
                                        OnRowDataBound="gvStudentLectureVaiolations_RowDataBound" EnableRowHighlight="False"
                                        RowHighlightColor="Gainsboro" TotalRowsCountMessage="Total rows count:" TotalRowsCountMessageDirection="Left"
                                        meta:resourcekey="gvStudentLectureVaiolationsResource1">
                                        <Columns>
                                            <asp:TemplateField meta:resourcekey="TemplateFieldResource1">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="cbCheckFlageAll" runat="server" meta:resourcekey="cbCheckFlageAllResource1" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="cbCheckFlage" runat="server" meta:resourcekey="cbCheckFlageResource1" />
                                                </ItemTemplate>
                                                <ItemStyle Width="7px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="StudentName" SortExpression="StudentName" meta:resourcekey="StudentNameResource">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnViolationStudentName" runat="server" CommandArgument='<%# Bind("StudentProfileID") %>'
                                                        CommandName="StudentArchiveing" CausesValidation="false" Text='<%# Bind("StudentName") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="100%" Height="44px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Violation Type" meta:resourcekey="ViolationTypeResource">
                                                <ItemTemplate>
                                                    <table aria-describedby="TableDesc" style="width: 100%; min-width: 650px;">
                                                        <tr>
                                                            <td style="width: 200px;">
                                                                <asp:DropDownList Width="100%" ID="ddlVaiolationType" DataTextField="Description"
                                                                    DataValueField="DeductID" runat="server" meta:resourcekey="ddlVaiolationTypeResource1">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:RequiredFieldValidator ValidationGroup="SaveValidation" InitialValue="-99" ControlToValidate="ddlVaiolationType"
                                                                    ID="rfvddlVaiolationType" runat="server" ErrorMessage="Select Violation Type ."
                                                                    meta:resourcekey="rfvddlVaiolationTypeResource1"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td style="float: left;">
                                                                <table aria-describedby="TableDesc" width="100%" runat="server" id="tblViolationLate">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblLateDuration" runat="server" Text="Late Duration" meta:resourcekey="LateDurationResource2"></asp:Label>
                                                                            :
                                                                        </td>
                                                                        <td style="width: 10px">
                                                                            <asp:RequiredFieldValidator ValidationGroup="SaveValidation" Display="Dynamic" ID="rfvLateMinute"
                                                                                meta:resourcekey="rfvLateMinuteResource1" ControlToValidate="tbLateMinute" runat="server"
                                                                                ToolTip="Enter Minute" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ValidationGroup="SaveValidation" ID="cvtbLateMinute" runat="server"
                                                                                meta:resourcekey="cvtbLateMinuteResource1" ErrorMessage="*" ToolTip="Lessa than 60 minute"
                                                                                ControlToValidate="tbLateMinute" Operator="LessThan" ValueToCompare="60" Type="Integer"
                                                                                Display="Dynamic"></asp:CompareValidator>
                                                                        </td>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="lblMinute" meta:resourcekey="lblMinuteResource1" runat="server" Text="Minute"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 20px">
                                                                            <asp:TextBox ID="tbLateMinute" MaxLength="2" Width="20px" runat="server"></asp:TextBox>
                                                                        </td>
                                                                        <td style="width: 10px">
                                                                            <asp:RequiredFieldValidator ValidationGroup="SaveValidation" ID="rfvtbLateHour" Display="Dynamic"
                                                                                meta:resourcekey="rfvtbLateHourResource1" ToolTip="Enter Hour" ControlToValidate="tbLateHour"
                                                                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                            <asp:CompareValidator ValidationGroup="SaveValidation" ID="cvtbLateHour" runat="server"
                                                                                meta:resourcekey="cvtbLateHourResource1" ErrorMessage="*" ToolTip="Lessa than or equal 12 hour"
                                                                                ControlToValidate="tbLateHour" Operator="LessThanEqual" ValueToCompare="5" Type="Integer"
                                                                                Display="Dynamic"></asp:CompareValidator>
                                                                        </td>
                                                                        <td style="width: 30px">
                                                                            <asp:Label ID="lblHour" runat="server" Text="Hour" meta:resourcekey="lblHourResource1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 20px">
                                                                            <asp:TextBox ID="tbLateHour" MaxLength="1" Width="20px" runat="server"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Options" meta:resourcekey="Options">
                                                <ItemTemplate>
                                                    <table aria-describedby="TableDesc" cellpadding="5" cellspacing="2" class="tblOptions"
                                                        id="tblSendSMS" runat="server">
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lbtnSend" runat="server" CommandName="SendSMS" CausesValidation="False"
                                                                    Text="Edit" meta:resourcekey="SendSMS" CommandArgument='<%# Bind("StudentProfileID") %>'></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </cc1:ITG_GridView>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="form-mandatory-field">
                        <asp:CheckBox ID="cbAgreement2" runat="server" Checked="true" meta:resourcekey="cbAgreement" />
                    </div>
                    <div class="form-operation-message">
                        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblResult" runat="server" SkinID="Validation" EnableViewState="false"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-actions">
                        <ul>
                            <li>
                                <asp:Button ID="ibtnSaveViolation" runat="server" ValidationGroup="SaveValidation"
                                    Text="<%$ Resources:ButtonsToolTip, SaveButton %>"
                                    OnClick="ibtnSaveViolation_Click" ToolTip="<%$ Resources:ButtonsToolTip, SaveButton %>"
                                    Visible="true" />
                            </li>
                        </ul>
                    </div>
                </asp:View>
                <asp:View ID="viewStudentAttendance" runat="server">
                    <div class="dashboard-theme">
                        <cc1:UserInformationsViewer ID="UserInformationsCC" runat="server" ClientIDLbtnToolTip=""
                            ClientNameLbtnToolTip="" GenderID="-99" IdentificationID="" IdentificationTypeID="-99"
                            IsHeaderColorFull="True" IsUserIDLinkable="False" IsUserNameLinkable="False"
                            meta:resourcekey="UserInformationsCCResource1" NationlityID="-99" OnClickClientID=""
                            OnClickClientName="" ParentName="" ParentUserID="-99" ParentUserProfileID="-99"
                            RemoveSpecialChar="False" ShowBirthDate="False" ShowClassName="True" ShowDistrictName="True"
                            ShowEMail="False" ShowFamilyName="False" ShowFirstName="False" ShowFullName="True"
                            ShowGender="False" ShowIdentificationID="False" ShowMinistryName="True" ShowMobile="False"
                            ShowNationality="False" ShowParentEmail="False" ShowParentHomePhoneNo="False"
                            ShowParentMobileNo="False" ShowParentOrgName="False" ShowParentWork="False" ShowParentWorkPhoneNo="False"
                            ShowPOBox="False" ShowProfileStatus="False" ShowSchoolName="True" ShowSecondName="False"
                            ShowSectionName="True" ShowSpecialtyName="True" ShowStreet="False" ShowStudySystemDesc="False"
                            ShowTelephone1="False" ShowTelephone2="False" ShowThirdName="False" ShowUsername="False"
                            ShowWebAddress="False" ShowZipCode="False" StudySystemID="-99" UserID="-99" UserProfileID="-99"
                            UserProfileStatusID="Undefined" Visible="true" />
                        <div class="form-controls box">
                            <div class="form-controls-header">
                                <asp:Label ID="lblAttendanceHistoryTitle" runat="server" SkinID="searchtitle"
                                    Text="<%$ Resources:CommonMessages,SearchFields %>"></asp:Label>
                            </div>
                            <div class="form-controls-body">
                                <div class="form-table">
                                    <div class="form-table-print">
                                        <table aria-describedby="TableDesc" id="tblPrintViolations" runat="server" style="width: 98%;">
                                            <tr>
                                                <td>
                                                    <table aria-describedby="TableDesc">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblPrinterFriendlyMsg" runat="server"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lbtnClickHere0" runat="server" OnClick="lbtnClickHere_Click"
                                                                    Text="<%$ Resources:CommonMessages, ClickHereText %>" ToolTip="<%$ Resources:CommonMessages, ClickHereText %>"></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="ibtnPrint" runat="server" AlternateText="<%$ Resources:ButtonsToolTip, PrintButton %>"
                                                        ImageUrl="<%$ Resources:CommonImages, PrintIcon %>" ToolTip="<%$ Resources:ButtonsToolTip, PrintButton %>"
                                                        Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div class="form-table-body" printable="true">
                                        <uc1:StudentAttendanceUC ID="oStudentAttendanceUC" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Button ID="ibtnBackFromStudentAbsences" runat="server" Text="<%$ Resources:ButtonsToolTip, BackButton %>"
                            OnClick="ibtnBackFromStudentAbsences_Click"
                            ToolTip="<%$ Resources:ButtonsToolTip, BackButton %>" />
                    </div>
                </asp:View>
                <asp:View ID="viewOperationResult" runat="server">
                    <div class="form-operation-message">
                        <asp:Label ID="lblUnPrivligaedUser" runat="server" SkinID="Validation" meta:resourcekey="lblUnPrivligaedUserResource1"
                            EnableViewState="false"></asp:Label>
                    </div>
                    <div class="form-actions">
                        <ul>
                            <li>
                                <asp:Button ID="ibtnBackFromOperation" runat="server" Text="<%$ Resources:ButtonsToolTip, BackButton %>"
                                    ToolTip="<%$ Resources:ButtonsToolTip, BackButton %>"
                                    OnClick="ibtnBackFromOperation_Click" />
                            </li>
                        </ul>
                    </div>
                </asp:View>
                <asp:View ID="viewPrintEmptyRecord" runat="server">
                    <div class="form-operation-message">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblNoActiveSemester" runat="server" EnableViewState="False" SkinID="Validation"
                                    meta:resourcekey="lblNoActiveSemesterResource1"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="form-actions">
                        <ul>
                            <li>
                                <asp:Button ID="ibtnBackFromattendanceReport" runat="server" Text="<%$ Resources:ButtonsToolTip, BackButton %>"
                                    OnClick="ibtnBackFromattendanceReport_Click"
                                    ToolTip="<%$ Resources:ButtonsToolTip, BackButton %>" />
                            </li>
                        </ul>
                    </div>
                    <div class="form-reportviewer divscrollbar-horizontal-vertical">
                        <rsweb:ReportViewer CssClass="ReportViewerScroll" ID="rvFollowUpRevealedAbsence"
                            runat="server" Font-Names="Verdana"
                            Font-Size="8pt" Height="600px" meta:resourcekey="ReportViewer1Resource1" Visible="False"
                            Width="100%">
                            <LocalReport EnableExternalImages="True">
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </div>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function ChangeDegreeDesc(ddlDegreeDeductAmount, lblDegreeMuadabaCourseDesc) {

            var ddlDegreeDeductAmount = document.getElementById(ddlDegreeDeductAmount);
            var lblDegreeMuadabaCourseDesc = document.getElementById(lblDegreeMuadabaCourseDesc);
            lblDegreeMuadabaCourseDesc.innerHTML = "";
            if (ddlDegreeDeductAmount != null && lblDegreeMuadabaCourseDesc != null) {

                var SelectedValue = ddlDegreeDeductAmount.value.split(",")[1];
                if (SelectedValue != "" && SelectedValue != "-99" && SelectedValue != undefined && SelectedValue != "undefined") {
                    lblDegreeMuadabaCourseDesc.innerHTML = SelectedValue;
                }
            }
        }

        function funShowIntro() {
            functionsBookmarks["funShowIntroViewButton"]();
            return true;
        }

        function CheckAllCheckBoxesClassStudentsAttendance(gvFullID, cbHeaderID, cbItemID, CodeEvent) {
            var Elem = window.event != window.undefined ? window.event.srcElement : CodeEvent.target;
            gvFullID = gvFullID + "_ctl";
            var InitialChkBoxID = 2;  //check Box ID will start from _ctl02, _ctl03
            var sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
            var bHeaderState = document.getElementById(gvFullID + '01_' + cbHeaderID).checked;
            var objID = gvFullID + sIndex + '_' + cbItemID;
            while (document.getElementById(objID) != null) {
                if (!document.getElementById(objID).disabled) {
                    document.getElementById(objID).checked = bHeaderState;

                    var ddlDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "ddlDegreeDeductAmount"));
                    var rfvDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "rfvDegreeDeductAmount"));

                    RecursiveEnableDisable(ddlDegreeDeductAmount, Elem.checked);
                    ValidatorEnable(rfvDegreeDeductAmount, Elem.checked);

                } // end if 
                InitialChkBoxID++;
                sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
                objID = gvFullID + sIndex + '_' + cbItemID;
            } //while end            
        } //end function

        // Loop through the Parent Control of the Target Control to Disable/Enable them
        function RecursiveEnableDisable(control, disable) {
            var children = control.childNodes;
            try {
                if (disable) {
                    control.removeAttribute('disabled');
                }
                else {
                    control.setAttribute('disabled', 'disabled');
                }
            }
            catch (ex) { }
            for (var j = 0; j < children.length; j++) {
                RecursiveEnableDisable(children[j], disable);
            }
        }

        //Check Single Check Box
        function CheckThisCheckBoxClassStudentsAttendance(gvFullID, cbHeaderID, cbItemID, CodeEvent) {

            var Elem = window.event != window.undefined ? window.event.srcElement : CodeEvent.target;
            gvFullID = gvFullID + "_ctl";
            var InitialChkBoxID = 2;  //check Box ID will start from _ctl02, _ctl03
            var sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
            var bHeaderState = true;
            var objID = gvFullID + sIndex + '_' + cbItemID;
            var IsFound = false;
            while (document.getElementById(objID) != null) {
                var ddlDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "ddlDegreeDeductAmount"));
                var rfvDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "rfvDegreeDeductAmount"));

                if (Elem.id == objID) {
                    RecursiveEnableDisable(ddlDegreeDeductAmount, Elem.checked);
                    ValidatorEnable(rfvDegreeDeductAmount, Elem.checked);
                    RecursiveEnableDisable(ddlDegreeDeductAmount, Elem.checked);
                }
                if (bHeaderState && document.getElementById(objID).checked == false && !document.getElementById(objID).disabled) {

                    bHeaderState = false;
                }
                InitialChkBoxID++;
                sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
                objID = gvFullID + sIndex + '_' + cbItemID;
            } //while end
            document.getElementById(gvFullID + '01_' + cbHeaderID).checked = bHeaderState;

        } //end function

        // Disable Calander Starting from Tomorrow
        function DisableAfterTodayDate(date) {
            if (date.getDay() === 5 || date.getDay() === 6) {
                return true;
            }

            var today = new Date();
            today.setHours(0, 0, 0, 0);

            var checkDate = new Date(date);
            checkDate.setHours(0, 0, 0, 0);

            if (checkDate > today) {
                return true;
            }
            return false;
        }


        //view/Hide Absence Reason DDL according to the check box selec state
        function HideShowAbsenceReasonDDL(oCbItem, ddlAbsenseReasonID, lblAbsenceReason) {
            var oDdlAbsenseReason = document.getElementById(ddlAbsenseReasonID);
            if (oCbItem.checked) {//Enabled
                oDdlAbsenseReason.disabled = '';
                if (oDdlAbsenseReason.value == "-99") {
                    document.getElementById(lblAbsenceReason).innerHTML = "Select absence reason.";
                }
            }
            else {//disabled
                oDdlAbsenseReason.disabled = 'true';
                document.getElementById(lblAbsenceReason).innerHTML = "";
            }
        }

        //Check for selected attendance reason befor save
        function CheckSelectedAttendanceReason(gvFullOrg, cbItemID, ddlAbsenseReasonID, lblErrorID) {
            var bRetValue = true;
            var gvFullID = gvFullOrg + "_ctl";
            var InitialChkBoxID = 2;  //check Box ID will start from _ctl02, _ctl03
            var sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
            var objID = gvFullID + sIndex + '_' + cbItemID;
            var objDllID = gvFullID + sIndex + '_' + ddlAbsenseReasonID;
            var objLblError = gvFullID + sIndex + '_' + lblErrorID;
            while (document.getElementById(objID) != null) {
                if (document.getElementById(objID).checked == true) {//Check the Attendance Reason DDL
                    if (document.getElementById(objDllID) != null && document.getElementById(objDllID).value == "-99") {
                        document.getElementById(objLblError).innerHTML = "Select Absence Reason";
                        bRetValue = false;
                    }
                    else {
                        document.getElementById(objLblError).innerHTML = "";
                    }
                }
                InitialChkBoxID++;
                sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
                objID = gvFullID + sIndex + '_' + cbItemID;
                objDllID = gvFullID + sIndex + '_' + ddlAbsenseReasonID;
                objLblError = gvFullID + sIndex + '_' + lblErrorID;
            } //while end

            return bRetValue;
        }
        //the calendar closed event handler [fired when u close the calender]
        function AttendanceDateCalendarClosed(cal) {

            var oCalDate = document.getElementById('<%= clrAttendanceDay.ClientID %>'); // retrieve Current Date from Calender
            var oCalOrgDate = document.getElementById('<%= hdnSelectedAttendanceDate.ClientID %>'); // get Orignal Date from Hidden Field Store the selected date on calender chagne event
            if (oCalOrgDate.value != "") { // if hidden feild value not empty 
                var SelctedDate = oCalOrgDate.value.split('/'); // return array of Numbers to set the Selected date format to MM/dd/yyyy
                var Month = "";
                var Day = "";
                if (parseInt(SelctedDate[1]) < 10 && SelctedDate[1].length == 1) { // when u select Date After select Hijri Date its return month without 0 [return just one digit]
                    Month = "0" + SelctedDate[1];
                }
                else {
                    Month = SelctedDate[1];
                }

                if (SelctedDate[0] < 10 && SelctedDate[0].length == 1) {// when u select Date After select Hijri Date its return day without 0 [return just one digit]
                    Day = "0" + SelctedDate[0];
                }
                else {
                    Day = SelctedDate[0];
                }
                var CompareDate = Month + "/" + Day + "/" + SelctedDate[2];
                //when the user select different date -> do post back
                if (oCalDate.value != CompareDate) {//do post back
                    __doPostBack('StartCalendar', '');
                }
                else {//when the same date choosen or the calendar closed without selection. dont do postback
                    if (cal) {
                        cal.hide();
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function SetMaxValueOfMinuteOrHour(tbID) {
            var tbToSet = document.getElementById(tbID);

            if (tbToSet.id.indexOf("Minute") != -1) {
                if (parseInt(tbToSet.value) > 59) {
                    tbToSet.value = 59;
                }
            }
            else {
                if (parseInt(tbToSet.value) > 5) {
                    tbToSet.value = 5;
                }
            }
        }

        function CheckValidator() {
            Page_IsValid = true;
            Page_ClientValidate('SaveValidation');
            if (Page_IsValid) {
                return true;
            }
            else {
                var noOfValidators = Page_Validators.length;
                for (var validatorIndex = 0; validatorIndex < noOfValidators; validatorIndex++) {
                    var validator = Page_Validators[validatorIndex];
                    if (validator.validationGroup == "SaveValidation") {
                        validator.focusOnError = "t";
                        ValidatorValidate(validator);
                        if (!validator.isvalid) {
                            if (validator.offsetLeft != undefined && validator.offsetTop != undefined) {
                                var selectedPosX = 0;
                                var selectedPosY = 0;
                                var tempValidator = validator;
                                while (tempValidator != null) {
                                    selectedPosX += tempValidator.offsetLeft;
                                    selectedPosY += tempValidator.offsetTop;
                                    tempValidator = tempValidator.offsetParent;
                                }
                                $(window).scrollTo({ top: selectedPosY - 200, left: selectedPosX }, 400, { queue: true });
                            }
                            return false;
                        }
                    }
                }
            }
        }

        function ShowLateTableAndValidation(ddlVaiolationType, tblViolationLate, tbLateMinute, tbLateHour
            , rfvLateMinute, rfvtbLateHour, cvtbLateMinute, cvtbLateHour) {

            var ddlVaiolation = document.getElementById(ddlVaiolationType);
            var tblViolation = document.getElementById(tblViolationLate);
            var tbLateM = document.getElementById(tbLateMinute);
            var tbLateH = document.getElementById(tbLateHour);
            var rfvLateM = document.getElementById(rfvLateMinute);
            var rfvtbLateH = document.getElementById(rfvtbLateHour);
            var cvtbLateM = document.getElementById(cvtbLateMinute);
            var cvtbLateH = document.getElementById(cvtbLateHour);

            if (ddlVaiolation.value.indexOf(",2") != -1) {
                tblViolation.style.display = "block";
                tbLateM.value = "";
                tbLateH.value = "";
                ValidatorEnable(rfvLateM, true);
                ValidatorEnable(rfvtbLateH, true);
                ValidatorEnable(cvtbLateM, true);
                ValidatorEnable(cvtbLateH, true);
            }
            else {
                tblViolation.style.display = "none";
                ValidatorEnable(rfvLateM, false);
                ValidatorEnable(rfvtbLateH, false);
                ValidatorEnable(cvtbLateM, false);
                ValidatorEnable(cvtbLateH, false);
            }
        }

        //Check Single Check Box
        function CheckThisCheckBox(gvFullID, cbHeaderID, cbItemID, CodeEvent) {

            var Elem = window.event != window.undefined ? window.event.srcElement : CodeEvent.target;
            gvFullID = gvFullID + "_ctl";
            var InitialChkBoxID = 2;
            var sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
            var bHeaderState = true;
            var objID = gvFullID + sIndex + '_' + cbItemID;
            var IsFound = false;

            while (document.getElementById(objID) != null) {

                var ddlDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "ddlVaiolationType"));
                var rfvDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "rfvddlVaiolationType"));
                var tbLateMinute = document.getElementById(objID.replace(cbItemID, "tbLateMinute"));
                var rfvLateMinute = document.getElementById(objID.replace(cbItemID, "rfvLateMinute"));
                var tbLateHour = document.getElementById(objID.replace(cbItemID, "tbLateHour"));
                var rfvtbLateHour = document.getElementById(objID.replace(cbItemID, "rfvtbLateHour"));
                var cvtbLateMinute = document.getElementById(objID.replace(cbItemID, "cvtbLateMinute"));
                var cvtbLateHour = document.getElementById(objID.replace(cbItemID, "cvtbLateHour"));
                var tblViolationLate = document.getElementById(objID.replace(cbItemID, "tblViolationLate"));

                if (Elem.id == objID) {
                    RecursiveEnableDisable(ddlDegreeDeductAmount, Elem.checked);
                    RecursiveEnableDisable(tbLateMinute, Elem.checked);
                    RecursiveEnableDisable(tbLateHour, Elem.checked);
                    RecursiveEnableDisable(tblViolationLate, Elem.checked);
                    ValidatorEnable(rfvDegreeDeductAmount, Elem.checked);
                    ValidatorEnable(rfvLateMinute, Elem.checked);
                    ValidatorEnable(rfvtbLateHour, Elem.checked);
                    ValidatorEnable(cvtbLateMinute, Elem.checked);
                    ValidatorEnable(cvtbLateHour, Elem.checked);
                }
                if (bHeaderState && document.getElementById(objID).checked == false && !document.getElementById(objID).disabled) {
                    bHeaderState = false;
                }
                InitialChkBoxID++;
                sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
                objID = gvFullID + sIndex + '_' + cbItemID;
            }
            document.getElementById(gvFullID + '01_' + cbHeaderID).checked = bHeaderState;
        }

        function CheckAllCheckBoxes(gvFullID, cbHeaderID, cbItemID, CodeEvent) {
            var Elem = window.event != window.undefined ? window.event.srcElement : CodeEvent.target;
            gvFullID = gvFullID + "_ctl";
            var InitialChkBoxID = 2;
            var sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
            var bHeaderState = document.getElementById(gvFullID + '01_' + cbHeaderID).checked;
            var objID = gvFullID + sIndex + '_' + cbItemID;

            while (document.getElementById(objID) != null) {
                if (!document.getElementById(objID).disabled) {
                    document.getElementById(objID).checked = bHeaderState;

                    var ddlDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "ddlVaiolationType"));
                    var tbLateMinute = document.getElementById(objID.replace(cbItemID, "tbLateMinute"));
                    var tbLateHour = document.getElementById(objID.replace(cbItemID, "tbLateHour"));
                    var rfvDegreeDeductAmount = document.getElementById(objID.replace(cbItemID, "rfvddlVaiolationType"));
                    var rfvLateMinute = document.getElementById(objID.replace(cbItemID, "rfvLateMinute"));
                    var rfvtbLateHour = document.getElementById(objID.replace(cbItemID, "rfvtbLateHour"));
                    var cvtbLateMinute = document.getElementById(objID.replace(cbItemID, "cvtbLateMinute"));
                    var cvtbLateHour = document.getElementById(objID.replace(cbItemID, "cvtbLateHour"));
                    var tblViolationLate = document.getElementById(objID.replace(cbItemID, "tblViolationLate"));

                    RecursiveEnableDisable(ddlDegreeDeductAmount, Elem.checked);
                    RecursiveEnableDisable(tbLateMinute, Elem.checked);
                    RecursiveEnableDisable(tbLateHour, Elem.checked);
                    RecursiveEnableDisable(tblViolationLate, Elem.checked);
                    ValidatorEnable(rfvDegreeDeductAmount, Elem.checked);
                    ValidatorEnable(rfvLateMinute, Elem.checked);
                    ValidatorEnable(rfvtbLateHour, Elem.checked);
                    ValidatorEnable(cvtbLateMinute, Elem.checked);
                    ValidatorEnable(cvtbLateHour, Elem.checked);
                }
                InitialChkBoxID++;
                sIndex = ((InitialChkBoxID < 10) ? ("0" + InitialChkBoxID) : InitialChkBoxID);
                objID = gvFullID + sIndex + '_' + cbItemID;
            }
        }

        function RecursiveEnableDisable(control, disable) {
            var children = control.childNodes;
            try {
                if (disable) {
                    control.removeAttribute('disabled');
                }
                else {
                    control.setAttribute('disabled', 'disabled');
                    if (control.id.indexOf("tblViolationLate") != -1) {
                        control.style.display = "none";
                    }
                    if (control.id.indexOf("ddlVaiolationType") != -1) {
                        control.selectedIndex = 0;
                    }
                }
            }
            catch (ex) { }
            for (var j = 0; j < children.length; j++) {
                RecursiveEnableDisable(children[j], disable);
            }
        }

        function vCheckToFromDateWithSemesterDDL(clrDateTimeFormat, DateSemester) {
            var nCount = 0;
            var hfSemster = DateSemester;
            var lblNoDataFoundControl = '<%=lblDateValue.ClientID%>';

            if (hfSemster != "" && hfSemster != null) {
                var dtiCurrentFromDate = hfSemster.split(',')[0];
                var dtiCurrentToDate = hfSemster.split(',')[1];
                var dtiCurrentFromDateHijri = hfSemster.split(',')[2];
                var dtiCurrentToDateHijri = hfSemster.split(',')[3];
                var SelectAttendanceDayControl = $("#" + '<%=clrAttendanceDay.ClientID %>').val();
                var CheckAllDate = dtiCurrentFromDate + "-" + SelectAttendanceDayControl + "-" + lblNoDataFoundControl + "," + SelectAttendanceDayControl + "-" + dtiCurrentToDate + "-" + lblNoDataFoundControl;
                var oArrCheckAllDate = CheckAllDate.split(',');
                for (i = 0; i < oArrCheckAllDate.length; i++) {
                    if (oArrCheckAllDate[i] == "") continue;
                    var clrStartDate = oArrCheckAllDate[i].split('-')[0];
                    var clrStartDateCalendar = oArrCheckAllDate[i].split('-')[0];
                    var clrEndDateCalendar = oArrCheckAllDate[i].split('-')[1];
                    if (clrStartDateCalendar == null || clrEndDateCalendar == null) continue;
                    var sClientToFormat = clrDateTimeFormat;
                    if (sClientToFormat != "") {
                        var sEndDate = String(clrEndDateCalendar);
                        var YearStart, currentYearStart, currentMonthStart, currentDayStart;
                        var FirstElement = sClientToFormat.split('/')[0].toString();
                        if (FirstElement == "%m") {
                            currentMonthStart = sEndDate.split('/')[0];
                            currentDayStart = sEndDate.split('/')[1];
                        } else {
                            currentDayStart = sEndDate.split('/')[0];
                            currentMonthStart = sEndDate.split('/')[1];
                        }
                        YearStart = sEndDate.split('/')[2];
                        if (YearStart != undefined) {
                            currentYearStart = YearStart.substring(0, 4);
                        }
                        var dEndDate = new Date();
                        dEndDate.setFullYear(currentYearStart, currentMonthStart - 1, currentDayStart);
                        dEndDate.setHours(0, 0, 0, 0);
                        var ID_ = oArrCheckAllDate[i].split('-')[2];
                        if (vCheckDate(dEndDate, clrDateTimeFormat, clrStartDate, false)) {
                            nCount++;
                            document.getElementById(ID_).innerHTML = "<%=GetLocalResourceObject("ValidationDateMessage").ToString()%>" + " " + dtiCurrentFromDate + "--" + dtiCurrentFromDateHijri + " " + '<%=GetLocalResourceObject("To").ToString()%>' + " " + dtiCurrentToDate + "--" + dtiCurrentToDateHijri;
                        }
                    }
                }
                if (nCount > 0) {
                    Page_ClientValidate("SearchValidation");
                    return false;
                } else {
                    document.getElementById(lblNoDataFoundControl).innerHTML = "";
                }
            }
        }

        function vCheckDate(date, clrFormat, clrFrom, bGetfromdate) {
            var sClientFormat = clrFormat;
            if (sClientFormat != "") {
                var sStartDate = String(bGetfromdate ? document.getElementById(clrFrom).value : clrFrom);
                var YearStart, currentYearStart, currentMonthStart, currentDayStart;
                var FirstElement = sClientFormat.split('/')[0].toString();
                if (FirstElement == "%m") {
                    currentMonthStart = returnValue(sStartDate.split('/')[0]);
                    currentDayStart = returnValue(sStartDate.split('/')[1]);
                } else {
                    currentDayStart = returnValue(sStartDate.split('/')[0]);
                    currentMonthStart = returnValue(sStartDate.split('/')[1]);
                }
                YearStart = sStartDate.split('/')[2];
                if (YearStart != undefined) {
                    currentYearStart = returnValue(YearStart.substring(0, 4));
                }
                var DisableAfterStartDate = new Date();
                DisableAfterStartDate.setFullYear(currentYearStart, currentMonthStart - 1, currentDayStart);
                DisableAfterStartDate.setHours(0, 0, 0, 0);
                return date < DisableAfterStartDate;
            }
            return false;
        }

        function returnValue(sDateValue) {
            var sValue = sDateValue;
            if (sDateValue < 10 && String(sDateValue).length >= 2) {
                sValue = String(sDateValue).substr(1, 1);
            }
            return sValue;
        }

        function CheckAgreement1() {
            Page_IsValid = true;
            if (document.getElementById("<%=cbAgreement1.ClientID %>").checked) {
                document.getElementById("<%=lblOperationResult.ClientID %>").innerHTML = "";
                Page_ClientValidate("SaveValidation");
                return Page_IsValid;
            }
            else {
                document.getElementById("<%=lblOperationResult.ClientID %>").innerHTML = '<%=GetLocalResourceObject("CheckOnAgreement").ToString() %>';
                return false;
            }
        }

        function CheckAgreement2() {
            Page_IsValid = true;
            if (document.getElementById("<%=cbAgreement2.ClientID %>").checked) {
                document.getElementById("<%=lblResult.ClientID %>").innerHTML = "";
                Page_ClientValidate("SaveValidation");
                return Page_IsValid;
            }
            else {
                document.getElementById("<%=lblResult.ClientID %>").innerHTML = '<%=GetLocalResourceObject("CheckOnAgreement").ToString() %>';
                return false;
            }
        }
    </script>
</asp:Content>
