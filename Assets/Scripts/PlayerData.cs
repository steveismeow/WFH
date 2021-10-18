using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    public bool testBool = false;

    public int performance = 0;

    void Awake()
    {
        instance = this;
    }
}
