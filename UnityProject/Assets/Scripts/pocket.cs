using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocket : MonoBehaviour
{
    private int keys = 0;
    public delegate void KeyEvent(int keys);
    public KeyEvent keyAdded;
    public KeyEvent keyUsed;

    public void addKey()
    {
        keys++;
        keyAdded?.Invoke(1);
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
            keyUsed?.Invoke(nbUsed);
            return true;
        }
    }

}
