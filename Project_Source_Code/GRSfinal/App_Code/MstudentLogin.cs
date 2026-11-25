using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MstudentLogin
/// </summary>
public class MstudentLogin
{
    private int stdLoginID;
    private string stdLoginPassword;
    private int stdID;
    private bool mustChangePassword;
    public MstudentLogin( int stdLoginID, string stdLoginPassword,int stdID,bool mustChangePassword = false)
	{
		
        this. stdLoginID = stdLoginID;
        this.stdLoginPassword = stdLoginPassword;
        this.stdID = stdID;
        this.mustChangePassword = mustChangePassword;
	}
     public int StdLoginID
    {
        get { return this.stdLoginID; }
        set { this.stdLoginID = value; }
    }
   
    public string StdLoginPassword
    {
        get { return this.stdLoginPassword; }
        set { this.stdLoginPassword = value; }
    }
    public int StdID
    {
        get { return this.stdID; }
        set { this.stdID = value; }
    }
    public bool MustChangePassword
    {
        get { return this.mustChangePassword; }
        set { this.mustChangePassword = value; }
    }
}
