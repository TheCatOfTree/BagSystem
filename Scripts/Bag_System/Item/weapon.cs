using System.Collections;
using System.Collections.Generic;
using Bag_System.Item;
using UnityEditor;
using UnityEngine;

public class weapon :Item
{
    public int _hit;

    new void Init()
    {
        base.Init();
        
    }

    private void Attk()
    {
        Debug.Log(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
