using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pocket : MonoBehaviour
{
    private int keys = 10;

    public void addKey()
    {
        keys++;
    }

    public int getKeys()
    {
        return keys;
    }

    public bool useKeys(int nbUsed)
    {
        if (keys < nbUsed)
        {
            return false;
        }
        else
        {
            keys -= nbUsed;
            return true;
        }
    }

}
