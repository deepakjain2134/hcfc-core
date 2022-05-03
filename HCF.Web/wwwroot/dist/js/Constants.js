(function ($) {
    $.Constants =
    {
        Policy: 106,
        Report: 107,
        MiscDocuments: 108,
        SampleDocument: -1,
        AssetReport: 109,
        CHILD_PAGE_ADMIN_CONFIRMATION: "These options have system-wide implications. Please contact your CRx Advisor before making changes.",
        Check_All_Status: "Please change status of all checkpoints.",
        Check_All_Status_Generator: 'All items do not have values. Are you sure you have completed  everything that is applicable.',
        Create_Work_Order: "Do you want to create a work order ?",
        Alert_Msg_After_InsGrop_Completed: "This EP is not complete until EP checkpoints are reviewed and completed in next step.",
        Select_Atleast_One_Asset_To_Create_WO: "Select at least one asset to create a work order",
        PDF_Footer_Msg_Fire_Watch_Portal_module: "CRx by HCFCompliance: Fire Watch Portal Module",
        Delete_Manufacture_Msg: "Are you sure you wish to delete this manufacture?",
        This_Ep_Is_Already_Selected: 'This EP is already selected!!',
        Select_standard_Ep: "Please select standard and EP to bind.",
        Email_Sent_Successfully: "Email sent successfully.",
        EMAIL_FAILED_MSG: "An error occurred while sending mail.",
        Remove_Assets_From_FloorPlan_Warning: "Are you want to remove this asset from floor!",
        Remove_Assets_From_FloorPlan_Success: "Your assets has been removed",
        RequestVerificationToken: "__RequestVerificationToken",
        Floor_ddl_text_All: "All",
        Floor_ddl_text_Select_Floor: "Select Floor",
        First_Close_Pending_ILSM: "Please close pending ILSM first before reinspecting",
        First_Close_Pending_Workorder: "Please close pending workorder first before reinspecting",
        Pin_Generated_Successfully: "Pin generated successfully",
        Browse_View_Files_In_IFrame: "https://view.officeapps.live.com/op/view.aspx?src=",  /* "https://docs.google.com/gview?url="*/ //embed
        ILSM_Information_Message: "Based on deficiencies, an ILSM incident #{0} was created.Please Document all Interim Life safety measures that are taken!",
        ILSM_Information_Existing_Message: "Based on deficiencies, this asset was added to existing ILSM incident #{0}.Please review to ensure all Interim Life safety measures that are taken and documented!",
        Delete_Digital_Signature: "Are you sure you wish to delete this signature?",
        Select_Atleast_One_Siemens_To_Create_WO: "Select at least one deficiency to create a work order",
        EP_Inspection_Ongoing_User: "<b>To Review this EP, the frequency must be changed from “Ongoing” by your CRx system administrator in <br /> Setup>More configurations>EP Configuration>EP Frequency </b>",
        EP_Inspection_Ongoing_Assets: "To Review this Asset, the frequency must be changed from “Ongoing” by your CRx system administrator in <br /> Setup>More configurations>EP Configuration>EP Frequency ",
        EP_Inspection_Attach_Assets: "Attach Assets First",
        All_Assets_Inspected: "Do you want to mark ep compliant ?",
        User_Deleted_Successfully:"User Deleted Successfully"

    },
        $.FloorAlias = {
            AliasItem: [
                { Sequence: "25", AliasValue: "PE", AliasText: "Penthouse" },
                { Sequence: "24", AliasValue: "R", AliasText: "Roof" },
                { Sequence: "10", AliasValue: "10", AliasText: "10th Floor" },
                { Sequence: "9", AliasValue: "9", AliasText: "9th Floor" },
                { Sequence: "8", AliasValue: "8", AliasText: "8th Floor" },
                { Sequence: "7", AliasValue: "7", AliasText: "7th Floor" },
                { Sequence: "6", AliasValue: "6", AliasText: "6th Floor" },
                { Sequence: "5", AliasValue: "5", AliasText: "5th Floor" },
                { Sequence: "4", AliasValue: "4", AliasText: "4th Floor" },
                { Sequence: "3", AliasValue: "3", AliasText: "3rd Floor" },
                { Sequence: "2", AliasValue: "2", AliasText: "2nd Floor" },
                { Sequence: "1", AliasValue: "1", AliasText: "1st Floor" },
                { Sequence: "1", AliasValue: "GF", AliasText: "Ground Floor" },
                { Sequence: "0", AliasValue: "00", AliasText: "Level 00" },
                { Sequence: "-1", AliasValue: "LL", AliasText: "Lower Lobby" },
                { Sequence: "-2", AliasValue: "LL0", AliasText: "Lower Level" },
                { Sequence: "-2", AliasValue: "LL 1", AliasText: "Lower Level 1" },
                { Sequence: "-2", AliasValue: "LL 2", AliasText: "Lower Level 2" },
                { Sequence: "-2", AliasValue: "LL 3", AliasText: "Lower Level 3" },
                { Sequence: "-3", AliasValue: "B1", AliasText: "Basement" },
                { Sequence: "-5", AliasValue: "SB", AliasText: "Sub-Basement" }, /*B2*/
                { Sequence: "-7", AliasValue: "B3", AliasText: "Sub-Sub-Basement" }
            ]
        },
        $.CRx_DateFormat = "M d, yy";
})(jQuery);