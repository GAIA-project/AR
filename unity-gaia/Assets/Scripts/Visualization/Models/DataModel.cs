using System;
using System.Collections;
using System.Collections.Generic;

public class DataModel : IComparable
{
    public float val;
    public DateTime dateTime;

    public DataModel(DateTime now, object val)
    {
        this.dateTime = now;
        this.val = (float) val;
    }

    public int CompareTo(object obj)
    {
        return this.dateTime.CompareTo(((DataModel) obj).dateTime);
    }

    public override string ToString()
    {
        return string.Format("[DataModel] " + dateTime + ": " + val);
    }
}
