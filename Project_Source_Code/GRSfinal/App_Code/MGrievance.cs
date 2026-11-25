using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MGrievance
/// </summary>
public class MGrievance
{
    private int grievanceID;
    private int stdID;
    private int categoryID;
    private int deptID;
    private string grievanceTitle;
    private string grievanceDescription;
    private string grievancePriority;
    private string grievanceStatus;
    private DateTime grievanceSubmissionDate;
    private DateTime grievanceResolutionDate;
    private string categoryName;
    private string departmentName;


    public MGrievance(
        int grievanceID,
        int stdID,
        int categoryID,
        int deptID,
        string grievanceTitle,
        string grievanceDescription,
        string grievancePriority,
        string grievanceStatus,
        DateTime grievanceSubmissionDate,
        DateTime grievanceResolutionDate)
    {
        this.grievanceID = grievanceID;
        this.stdID = stdID;
        this.categoryID = categoryID;
        this.deptID = deptID;
        this.grievanceTitle = grievanceTitle;
        this.grievanceDescription = grievanceDescription;
        this.grievancePriority = grievancePriority;
        this.grievanceStatus = grievanceStatus;
        this.grievanceSubmissionDate = grievanceSubmissionDate;
        this.grievanceResolutionDate = grievanceResolutionDate;
    }


    public int GrievanceID
    {
        get { return this.grievanceID; }
        set { this.grievanceID = value; }
    }


    public int StdID
    {
        get { return this.stdID; }
        set { this.stdID = value; }
    }


    public int CategoryID
    {
        get { return this.categoryID; }
        set { this.categoryID = value; }
    }


    public int DeptID
    {
        get { return this.deptID; }
        set { this.deptID = value; }
    }


    public string GrievanceTitle
    {
        get { return this.grievanceTitle; }
        set { this.grievanceTitle = value; }
    }


    public string GrievanceDescription
    {
        get { return this.grievanceDescription; }
        set { this.grievanceDescription = value; }
    }


    public string GrievancePriority
    {
        get { return this.grievancePriority; }
        set { this.grievancePriority = value; }
    }


    public string GrievanceStatus
    {
        get { return this.grievanceStatus; }
        set { this.grievanceStatus = value; }
    }


    public DateTime GrievanceSubmissionDate
    {
        get { return this.grievanceSubmissionDate; }
        set { this.grievanceSubmissionDate = value; }
    }


    public DateTime GrievanceResolutionDate
    {
        get { return this.grievanceResolutionDate; }
        set { this.grievanceResolutionDate = value; }
    }
    public string CategoryName
    {
        get { return this.categoryName; }
        set { this.categoryName = value; }
    }

    public string DepartmentName
    {
        get { return this.departmentName; }
        set { this.departmentName = value; }
    }
}