using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MDepartmentHead
/// </summary>
public class MDepartmentHead
{
    private int deptHeadID;
    private string deptHeadName;
    private string deptHeadUserName;
    private string deptHeadPassword;
    private string deptHeadEmail;
    private string deptHeadPhoneNo;
    private string deptHeadDesignation;
    private string deptHeadStatus;
    private int deptID;
    private bool mustChangePassword;

    public MDepartmentHead(int deptHeadID,
                           string deptHeadName,
                           string deptHeadUserName,
                           string deptHeadPassword,
                           string deptHeadEmail,
                           string deptHeadPhoneNo,
                           string deptHeadDesignation,
                           string deptHeadStatus,
                           int deptID,
       bool mustChangePassword = false)
    {
        this.deptHeadID = deptHeadID;
        this.deptHeadName = deptHeadName;
        this.deptHeadUserName = deptHeadUserName;
        this.deptHeadPassword = deptHeadPassword;
        this.deptHeadEmail = deptHeadEmail;
        this.deptHeadPhoneNo = deptHeadPhoneNo;
        this.deptHeadDesignation = deptHeadDesignation;
        this.deptHeadStatus = deptHeadStatus;
        this.deptID = deptID;
        this.mustChangePassword = mustChangePassword;
    }

    public int DeptHeadID
    {
        get { return this.deptHeadID; }
        set { this.deptHeadID = value; }
    }

    public string DeptHeadName
    {
        get { return this.deptHeadName; }
        set { this.deptHeadName = value; }
    }

    public string DeptHeadUserName
    {
        get { return this.deptHeadUserName; }
        set { this.deptHeadUserName = value; }
    }

    public string DeptHeadPassword
    {
        get { return this.deptHeadPassword; }
        set { this.deptHeadPassword = value; }
    }

    public string DeptHeadEmail
    {
        get { return this.deptHeadEmail; }
        set { this.deptHeadEmail = value; }
    }

    public string DeptHeadPhoneNo
    {
        get { return this.deptHeadPhoneNo; }
        set { this.deptHeadPhoneNo = value; }
    }

    public string DeptHeadDesignation
    {
        get { return this.deptHeadDesignation; }
        set { this.deptHeadDesignation = value; }
    }

    public string DeptHeadStatus
    {
        get { return this.deptHeadStatus; }
        set { this.deptHeadStatus = value; }
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

}