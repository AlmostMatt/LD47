using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seasonal : MonoBehaviour
{
    private Season mSeason;
    public Season Season
    {
        get { return mSeason; }
        set { mSeason = value; }
    }
}
