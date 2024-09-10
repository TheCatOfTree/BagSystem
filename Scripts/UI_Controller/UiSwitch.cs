using System.Collections;
using System.Collections.Generic;
using Create_System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiSwitch : MonoBehaviour
{
    private BagManager _bag;
    public GameObject creteUI;
    public GameObject bagUI;

    public GameObject Big_bagUI;

    // Start is called before the first frame update
    void Start()
    {
        _bag = BagManager.Instance;
        Big_bagUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Create2Bag()
    {
        bagUI.SetActive(true);
        creteUI.SetActive(false);
        _bag.UpdateShowUp(bagUI);
    }

    public void Bag2Create()
    {
        creteUI.SetActive(true);
        bagUI.SetActive(false);
        CreateSomething.Instance.compoundMianBan.SetActive(false);
        _bag.UpdateShowUp(creteUI);
    }

    public void CloseBagUI()
    {
        if (Big_bagUI.activeInHierarchy)
            Big_bagUI.SetActive(false);
        else
        {
            Big_bagUI.SetActive(true);
            _bag.UpdateShowUp(bagUI);
        }
    }
}