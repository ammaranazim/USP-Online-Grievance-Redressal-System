
using System;

/// <summary>
/// Summary description for MStaff
/// </summary>
public class MStaff
{
    private int staffID;
    private string staffName;
    private string staffUserName;
    private string staffEmail;
    private string staffPhoneNo;
    private string staffDesignation;
    private string staffStatus;
    private int deptID;
    private bool mustChangePassword;
    private string deptName;

    public MStaff(
        int staffID,
        string staffName,
        string staffUserName,
        string staffEmail,
        string staffPhoneNo,
        string staffDesignation,
        string staffStatus,
        int deptID,
        bool mustChangePassword,
        string deptName)
    {
        this.staffID = staffID;
        this.staffName = staffName;
        this.staffUserName = staffUserName;
        this.staffEmail = staffEmail;
        this.staffPhoneNo = staffPhoneNo;
        this.staffDesignation = staffDesignation;
        this.staffStatus = staffStatus;
        this.deptID = deptID;
        this.mustChangePassword = mustChangePassword;
        this.deptName = deptName;
    }

    public int StaffID
    {
        get { return this.staffID; }
        set { this.staffID = value; }
    }

    public string StaffName
    {
        get { return this.staffName; }
        set { this.staffName = value; }
    }

    public string StaffUserName
    {
        get { return this.staffUserName; }
        set { this.staffUserName = value; }
    }

    public string StaffEmail
    {
        get { return this.staffEmail; }
        set { this.staffEmail = value; }
    }

    public string StaffPhoneNo
    {
        get { return this.staffPhoneNo; }
        set { this.staffPhoneNo = value; }
    }

    public string StaffDesignation
    {
        get { return this.staffDesignation; }
        set { this.staffDesignation = value; }
    }

    public string StaffStatus
    {
        get { return this.staffStatus; }
        set { this.staffStatus = value; }
    }

    public int DeptID
    {
        get { return this.deptID; }
        set { this.deptID = value; }
    }

    public bool MustChangePassword
    {
        get { return this.mustChangePassword; }
        set { this.mustChangePassword = value; }
    }

    public string DeptName
    {
        get { return this.deptName; }
        set { this.deptName = value; }
    }
}
