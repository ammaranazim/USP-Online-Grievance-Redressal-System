using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MEscalatedGrievance
/// </summary>
public class MEscalatedGrievance
{
    private int escalationID;
    private int grievanceID;
    private string escalationReason;
    private string escalationDescription;
    private DateTime requestedDate;
    private string requestStatus;
    private string hodRemarks;
    private DateTime? hodDecisionDate;
    private int? reviewedByDeptHeadID;
    private string adminRemarks;
    private DateTime? adminDecisionDate;
    private int? decidedByAdminID;


    public MEscalatedGrievance(
        int escalationID,
        int grievanceID,
        string escalationReason,
        string escalationDescription,
        DateTime requestedDate,
        string requestStatus,
        string hodRemarks,
        DateTime? hodDecisionDate,
        int? reviewedByDeptHeadID,
        string adminRemarks,
        DateTime? adminDecisionDate,
        int? decidedByAdminID)
    {
        this.escalationID = escalationID;
        this.grievanceID = grievanceID;
        this.escalationReason = escalationReason;
        this.escalationDescription = escalationDescription;
        this.requestedDate = requestedDate;
        this.requestStatus = requestStatus;
        this.hodRemarks = hodRemarks;
        this.hodDecisionDate = hodDecisionDate;
        this.reviewedByDeptHeadID = reviewedByDeptHeadID;
        this.adminRemarks = adminRemarks;
        this.adminDecisionDate = adminDecisionDate;
        this.decidedByAdminID = decidedByAdminID;
    }


    public int EscalationID
    {
        get { return this.escalationID; }
        set { this.escalationID = value; }
    }


    public int GrievanceID
    {
        get { return this.grievanceID; }
        set { this.grievanceID = value; }
    }


    public string EscalationReason
    {
        get { return this.escalationReason; }
        set { this.escalationReason = value; }
    }


    public string EscalationDescription
    {
        get { return this.escalationDescription; }
        set { this.escalationDescription = value; }
    }


    public DateTime RequestedDate
    {
        get { return this.requestedDate; }
        set { this.requestedDate = value; }
    }


    public string RequestStatus
    {
        get { return this.requestStatus; }
        set { this.requestStatus = value; }
    }


    public string HodRemarks
    {
        get { return this.hodRemarks; }
        set { this.hodRemarks = value; }
    }


    public DateTime? HodDecisionDate
    {
        get { return this.hodDecisionDate; }
        set { this.hodDecisionDate = value; }
    }


    public int? ReviewedByDeptHeadID
    {
        get { return this.reviewedByDeptHeadID; }
        set { this.reviewedByDeptHeadID = value; }
    }


    public string AdminRemarks
    {
        get { return this.adminRemarks; }
        set { this.adminRemarks = value; }
    }


    public DateTime? AdminDecisionDate
    {
        get { return this.adminDecisionDate; }
        set { this.adminDecisionDate = value; }
    }


    public int? DecidedByAdminID
    {
        get { return this.decidedByAdminID; }
        set { this.decidedByAdminID = value; }
    }
}