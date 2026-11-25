using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MGrievanceCategory
/// </summary>
public class MGrievanceCategory
{
    private int categoryID;
    private string categoryName;
    private string categoryDescription;
    private string categoryStatus;

    public MGrievanceCategory(int categoryID, string categoryName, string categoryDescription, string categoryStatus)
    {
        this.categoryID = categoryID;
        this.categoryName = categoryName;
        this.categoryDescription = categoryDescription;
        this.categoryStatus = categoryStatus;
    }

    public int CategoryID
    {
        get { return this.categoryID; }
        set { this.categoryID = value; }
    }

    public string CategoryName
    {
        get { return this.categoryName; }
        set { this.categoryName = value; }
    }

    public string CategoryDescription
    {
        get { return this.categoryDescription; }
        set { this.categoryDescription = value; }
    }

    public string CategoryStatus
    {
        get { return this.categoryStatus; }
        set { this.categoryStatus = value; }
    }
}