using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MGrievanceSubtype
/// </summary>
public class MGrievanceSubtype
{
    private int gvSubtypeID;
    private string gvSubtypeName;
    private string gvSubtypeDescription;
    private int categoryID;

    public MGrievanceSubtype(int gvSubtypeID, string gvSubtypeName, string gvSubtypeDescription, int categoryID)
    {
        this.gvSubtypeID = gvSubtypeID;
        this.gvSubtypeName = gvSubtypeName;
        this.gvSubtypeDescription = gvSubtypeDescription;
        this.categoryID = categoryID;
    }

    public int GvSubtypeID
    {
        get { return this.gvSubtypeID; }
        set { this.gvSubtypeID = value; }
    }

    public string GvSubtypeName
    {
        get { return this.gvSubtypeName; }
        set { this.gvSubtypeName = value; }
    }

    public string GvSubtypeDescription
    {
        get { return this.gvSubtypeDescription; }
        set { this.gvSubtypeDescription = value; }
    }

    public int CategoryID
    {
        get { return this.categoryID; }
        set { this.categoryID = value; }
    }
}