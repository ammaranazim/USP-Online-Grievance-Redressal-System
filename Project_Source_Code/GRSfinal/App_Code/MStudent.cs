using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MStudent
/// </summary>
public class MStudent
{
    private int stdID;
    private string stdRegNo;
    private string stdFirstName;
    private string stdLastName;
    private string stdFName;
    private string stdCnic;
    private string stdPhoneNo;
    private string stdSemester;
    private string stdProgram;
    private string stdSession;
    private string stdDescription;
    private string stdGender;
    private string stdAddress;
    private DateTime stdDateOfBirth;
    private DateTime stdEnrollmentDate;
    private string stdCity;
    private string stdStatus;
    private int deptID;
    private string stdEmail;

    public MStudent(int stdID, string stdRegNo, string stdFirstName, string stdLastName,
        string stdFName, string stdCnic, string stdPhoneNo, string stdSemester,
        string stdProgram, string stdSession, string stdDescription,
        string stdGender, string stdAddress, DateTime stdDateOfBirth,
        DateTime stdEnrollmentDate, string stdCity, string stdStatus,
        int deptID,  string stdEmail)
    {
        this.stdID = stdID;
        this.stdRegNo = stdRegNo;
        this.stdFirstName = stdFirstName;
        this.stdLastName = stdLastName;
        this.stdFName = stdFName;
        this.stdCnic = stdCnic;
        this.stdPhoneNo = stdPhoneNo;
        this.stdSemester = stdSemester;
        this.stdProgram = stdProgram;
        this.stdSession = stdSession;
        this.stdDescription = stdDescription;
        this.stdGender = stdGender;
        this.stdAddress = stdAddress;
        this.stdDateOfBirth = stdDateOfBirth;
        this.stdEnrollmentDate = stdEnrollmentDate;
        this.stdCity = stdCity;
        this.stdStatus = stdStatus;
        this.deptID = deptID;
        this.stdEmail = stdEmail;
    }

    public int StdID
    {
        get { return this.stdID; }
        set { this.stdID = value; }
    }

    public string StdRegNo
    {
        get { return this.stdRegNo; }
        set { this.stdRegNo = value; }
    }

    public string StdFirstName
    {
        get { return this.stdFirstName; }
        set { this.stdFirstName = value; }
    }

    public string StdLastName
    {
        get { return this.stdLastName; }
        set { this.stdLastName = value; }
    }

    public string StdFName
    {
        get { return this.stdFName; }
        set { this.stdFName = value; }
    }

    public string StdCnic
    {
        get { return this.stdCnic; }
        set { this.stdCnic = value; }
    }

    public string StdPhoneNo
    {
        get { return this.stdPhoneNo; }
        set { this.stdPhoneNo = value; }
    }

    public string StdSemester
    {
        get { return this.stdSemester; }
        set { this.stdSemester = value; }
    }

    public string StdProgram
    {
        get { return this.stdProgram; }
        set { this.stdProgram = value; }
    }

    public string StdSession
    {
        get { return this.stdSession; }
        set { this.stdSession = value; }
    }

    public string StdDescription
    {
        get { return this.stdDescription; }
        set { this.stdDescription = value; }
    }

    public string StdGender
    {
        get { return this.stdGender; }
        set { this.stdGender = value; }
    }

    public string StdAddress
    {
        get { return this.stdAddress; }
        set { this.stdAddress = value; }
    }

    public DateTime StdDateOfBirth
    {
        get { return this.stdDateOfBirth; }
        set { this.stdDateOfBirth = value; }
    }

    public DateTime StdEnrollmentDate
    {
        get { return this.stdEnrollmentDate; }
        set { this.stdEnrollmentDate = value; }
    }

    public string StdCity
    {
        get { return this.stdCity; }
        set { this.stdCity = value; }
    }

    public string StdStatus
    {
        get { return this.stdStatus; }
        set { this.stdStatus = value; }
    }

    public int DeptID
    {
        get { return this.deptID; }
        set { this.deptID = value; }
    }

    public string StdEmail
    {
        get { return this.stdEmail; }
        set { this.stdEmail = value; }
    }
}