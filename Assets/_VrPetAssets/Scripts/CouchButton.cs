﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class CouchButton : BaseToyClickButton
{
    public Transform television;
    public Transform cameraRig;
    public Transform couchCamera;
    public Transform couchTablet;
    public Transform tablet;
    public Transform tableTVPosition;
    public GameObject[] offMeshLinks;

    private Vector3 initialTabletPosition;
    private Quaternion initialTabletRotation;
    private Vector3 initialTelevisionPosition;
    private Vector3 initialCameraPosition;
    private Quaternion initialCameraRotation;

    public OVRScreenFade faderLeft;
    public OVRScreenFade fadeRight;
    public AudioMixer music;

    private bool onCouch;
    [HideInInspector]
    public bool imFading;
#if UNITY_EDITOR
    public bool fakeButton;
#endif

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!imFading)
        {
            imFading = true;
            faderLeft.FadeOut();
            fadeRight.FadeOut();
        }
    }

    private void Awake()
    {
        initialCameraPosition = cameraRig.position;
        initialCameraRotation = cameraRig.rotation;
        initialTelevisionPosition = television.position;
        faderLeft.FadeOutExit += FadeOutExit;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (fakeButton)
        {
            OnPointerDown(null);
            fakeButton = false;
        }
    }
#endif

    private void OnDestroy()
    {
        faderLeft.FadeOutExit -= FadeOutExit;
    }

    public void FadeOutExit()
    {
        if (onCouch)
        {
            if (imFading)
            {
                cameraRig.position = initialCameraPosition;
                cameraRig.rotation = initialCameraRotation;
            }
            television.position = initialTelevisionPosition;
            tablet.position = initialTabletPosition;
            tablet.rotation = initialTabletRotation;
            music.SetFloat("MusicVolume", 0);

            onCouch = false;

            foreach (GameObject link in offMeshLinks)
            {
                if (link)
                {
                    link.SetActive(true);
                }
            }
        }
        else if (imFading)
        {
            cameraRig.position = couchCamera.position;
            cameraRig.rotation = couchCamera.rotation;
            television.position = tableTVPosition.position;
            tablet.position = couchTablet.position;
            tablet.rotation = couchTablet.rotation;
            music.SetFloat("MusicVolume", -80f);

            onCouch = true;

            foreach (GameObject link in offMeshLinks)
            {
                if (link)
                {
                    link.SetActive(false);
                }
            }
        }

        if (imFading)
        {
            faderLeft.FadeIn();
            fadeRight.FadeIn();

            imFading = false;
        }
    }
}
