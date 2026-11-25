using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MEvidence
/// </summary>
public class MEvidence
{
    private int evidenceID;
    private string evidenceName;
    private string evidenceDescription;
    private DateTime evidenceDatetime;
    private string evidenceUploadedby;
    private string evidenceFilepath;
    private int grievanceID;

    public MEvidence(int evidenceID, string evidenceName, string evidenceDescription, DateTime evidenceDatetime, string evidenceUploadedby,string evidenceFilepath, int grievanceID)
    {
        this.evidenceID = evidenceID;
        this.evidenceName = evidenceName;
        this.evidenceDescription = evidenceDescription;
        this.evidenceDatetime = evidenceDatetime;
        this.evidenceUploadedby = evidenceUploadedby;
        this.evidenceFilepath = evidenceFilepath;
        this.grievanceID = grievanceID;
    }

    public int EvidenceID
    {
        get { return this.evidenceID; }
        set { this.evidenceID = value; }
    }

    public string EvidenceName
    {
        get { return this.evidenceName; }
        set { this.evidenceName = value; }
    }

    public string EvidenceDescription
    {
        get { return this.evidenceDescription; }
        set { this.evidenceDescription = value; }
    }

    public DateTime EvidenceDatetime
    {
        get { return this.evidenceDatetime; }
        set { this.evidenceDatetime = value; }
    }

    public string EvidenceUploadedby
    {
        get { return this.evidenceUploadedby; }
        set { this.evidenceUploadedby = value; }
    }
    public string EvidenceFilepath
    {
        get { return this.evidenceFilepath; }
        set { this.evidenceFilepath = value; }
    }

    public int GrievanceID
    {
        get { return this.grievanceID; }
        set { this.grievanceID = value; }
    }
}