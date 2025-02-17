﻿using EasyInputVR.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabletVisibility : MonoBehaviour
{
    private Transform UiTablet;
    private bool padClick = false;
    private bool prevPadClick = false;

    private void Awake()
    {
        UiTablet = transform.GetChild(0);
    }

    void OnEnable()
    {
        EasyInputHelper.On_ClickStart += ClickStart;
        EasyInputHelper.On_ClickEnd += ClickEnd;
    }

    public void ToggleTablet()
    {
        UiTablet.gameObject.SetActive(!UiTablet.gameObject.activeSelf);
    }
   
    void Update()
    {
        if (padClick == true && prevPadClick == false && !(PlayerState.CURRENTSTATE == PlayerState.PLAYERSTATE.DRIVING || PlayerState.CURRENTSTATE == PlayerState.PLAYERSTATE.REMOTE))
        {
            ToggleTablet();
        }

        prevPadClick = padClick;
    }

    void ClickStart(ButtonClick button)
    {
        if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
        {
            padClick = true;
        }
    }

    void ClickEnd(ButtonClick button)
    {
        if (button.button == EasyInputConstants.CONTROLLER_BUTTON.GearVRTouchClick)
        {
            padClick = false;
        }
    }

    void OnDestroy()
    {
        EasyInputHelper.On_ClickStart -= ClickStart;
        EasyInputHelper.On_ClickEnd -= ClickEnd;
    }
}
