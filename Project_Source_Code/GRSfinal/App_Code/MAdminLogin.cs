using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class MLogin
{
    private int adminLoginID;
    private string adminLoginName;
    private string adminLoginEmail;
    private string adminLoginPassword;
    private string adminLoginPhoneNo;
    private string adminLoginDescription;

    public MLogin(int adminLoginID, string adminLoginName, string adminLoginEmail, string adminLoginPassword, string adminLoginPhoneNo, string adminLoginDescription)
	{
        this.adminLoginID = adminLoginID;
        this.adminLoginName = adminLoginName;
        this.adminLoginEmail = adminLoginEmail;
        this.adminLoginPassword = adminLoginPassword;
        this.adminLoginPhoneNo = adminLoginPhoneNo;
        this.adminLoginDescription = adminLoginDescription;
	}
    public int AdminLoginID
    {
        get{ return this.adminLoginID;}
        set { this.adminLoginID = value; }
    }
    public string AdminLoginName
    {
        get { return this.adminLoginName; }
        set { this.adminLoginName = value; }
    }
    public string AdminLoginEmail
    {
        get { return this.adminLoginEmail; }
        set { this.adminLoginEmail = value; }
    }
    public string AdminLoginPassword
    {
        get { return this.adminLoginPassword; }
        set { this.adminLoginPassword = value; }
    }
    public string AdminLoginPhoneNo
    {
        get { return this.adminLoginPhoneNo; }
        set { this.adminLoginPhoneNo = value; }
    }
    public string AdminLoginDescription
    {
        get { return this.adminLoginDescription; }
        set { this.adminLoginDescription = value; }
    }
}