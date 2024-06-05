using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DataLayer.Classes
{
    public enum EnumNotificationType
    {
        [Description("Approval of Appendix")]
        ApprovalofAppendix = 1,

        [Description("Change Request")]
        ChangeRequest = 2,

        [Description("MPR Creation")]
        MPRCreation = 3,

        [Description("PRN Creation")]
        PRNCreation = 4,

        [Description("Calendar Events")]
        CalendarEvents = 5,

        [Description("RTP")]
        RTP = 6,

        [Description("AP Posting")]
        APPosting = 7,

        [Description("NTP")]
        NTP = 8,

        [Description("New Trade Creation")]
        NewTradeCreation = 9,

        [Description("Class Status Change")]
        ClassStatusChange = 10,

        [Description("Trainee Status Change")]
        TraineeStatusChange = 11,

        [Description("Nadra verification")]
        Nadraverification = 12,

        [Description("Exam Status")]
        ExamStatus = 13,
    }
 
}
