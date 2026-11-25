using System;

public class MDepartment
{
    private int deptID;
    private string deptName;
    private string deptCode;
    private string deptStatus;
    private string deptDescription;
    private string deptEmail;
    private string deptContactNo;
    private string deptWH;
    private string deptLocation;
    private int categoryID;

    // Existing constructor retained for compatibility
    public MDepartment(
        int deptID,
        string deptName,
        string deptCode,
        string deptStatus,
        string deptDescription,
        string deptEmail,
        string deptContactNo,
        string deptWH,
        string deptLocation)
    {
        this.deptID = deptID;
        this.deptName = deptName;
        this.deptCode = deptCode;
        this.deptStatus = deptStatus;
        this.deptDescription = deptDescription;
        this.deptEmail = deptEmail;
        this.deptContactNo = deptContactNo;
        this.deptWH = deptWH;
        this.deptLocation = deptLocation;
        this.categoryID = 0;
    }

    // New constructor including CategoryID
    public MDepartment(
        int deptID,
        string deptName,
        string deptCode,
        string deptStatus,
        string deptDescription,
        string deptEmail,
        string deptContactNo,
        string deptWH,
        string deptLocation,
        int categoryID)
        : this(
            deptID,
            deptName,
            deptCode,
            deptStatus,
            deptDescription,
            deptEmail,
            deptContactNo,
            deptWH,
            deptLocation)
    {
        this.categoryID = categoryID;
    }

    public int DeptID
    {
        get { return deptID; }
        set { deptID = value; }
    }

    public string DeptName
    {
        get { return deptName; }
        set { deptName = value; }
    }

    public string DeptCode
    {
        get { return deptCode; }
        set { deptCode = value; }
    }

    public string DeptStatus
    {
        get { return deptStatus; }
        set { deptStatus = value; }
    }

    public string DeptDescription
    {
        get { return deptDescription; }
        set { deptDescription = value; }
    }

    public string DeptEmail
    {
        get { return deptEmail; }
        set { deptEmail = value; }
    }

    public string DeptContactNo
    {
        get { return deptContactNo; }
        set { deptContactNo = value; }
    }

    public string DeptWH
    {
        get { return deptWH; }
        set { deptWH = value; }
    }

    public string DeptLocation
    {
        get { return deptLocation; }
        set { deptLocation = value; }
    }

    public int CategoryID
    {
        get { return categoryID; }
        set { categoryID = value; }
    }
}