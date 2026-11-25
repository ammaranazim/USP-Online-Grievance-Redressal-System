using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MTrackGrievance
/// </summary>
public class MTrackGrievance
{
    private int trackID;
    private int grievanceID;
    private int loginID;
    private string trackingactivityDescription;
    private DateTime trackingactivitydatetime;
    private int? escalationID;

    // Existing constructor
    // Keeps all current Student/Admin code working.
    public MTrackGrievance(
        int trackID,
        int grievanceID,
        int loginID,
        string trackingactivityDescription,
        DateTime trackingactivitydatetime)
    {
        this.trackID = trackID;
        this.grievanceID = grievanceID;
        this.loginID = loginID;
        this.trackingactivityDescription = trackingactivityDescription;
        this.trackingactivitydatetime = trackingactivitydatetime;
        this.escalationID = null;
    }

    // New constructor for escalation-related activities
    public MTrackGrievance(
        int trackID,
        int grievanceID,
        int loginID,
        string trackingactivityDescription,
        DateTime trackingactivitydatetime,
        int? escalationID)
    {
        this.trackID = trackID;
        this.grievanceID = grievanceID;
        this.loginID = loginID;
        this.trackingactivityDescription = trackingactivityDescription;
        this.trackingactivitydatetime = trackingactivitydatetime;
        this.escalationID = escalationID;
    }

    public int TrackID
    {
        get { return this.trackID; }
        set { this.trackID = value; }
    }

    public int GrievanceID
    {
        get { return this.grievanceID; }
        set { this.grievanceID = value; }
    }

    public int LoginID
    {
        get { return this.loginID; }
        set { this.loginID = value; }
    }

    public string TrackingactivityDescription
    {
        get { return this.trackingactivityDescription; }
        set { this.trackingactivityDescription = value; }
    }

    public DateTime Trackingactivitydatetime
    {
        get { return this.trackingactivitydatetime; }
        set { this.trackingactivitydatetime = value; }
    }

    public int? EscalationID
    {
        get { return this.escalationID; }
        set { this.escalationID = value; }
    }
}