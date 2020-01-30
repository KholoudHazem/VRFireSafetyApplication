//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

namespace Valve.VR.InteractionSystem.Sample
{
    public class AlarmButtonEffect : MonoBehaviour
    {
        public AudioClip alarmClip;
        public AudioSource alarmClipSource;
        public GameObject telPoint;
        public void Start()
        {
            alarmClipSource.clip = alarmClip;
            Debug.Log("alarmbutton script started");
        }
        public void OnButtonDown(Hand fromHand)
        {
            ColorSelf(Color.cyan);
            fromHand.TriggerHapticPulse(1000);
            Debug.Log("Play alarm if  not playing " + alarmClipSource.isPlaying);
            if (!alarmClipSource.isPlaying)
            {
                alarmClipSource.Play();
                Debug.Log("Play alarm sound!");
            }

            telPoint.GetComponent<TeleportPoint>().locked = false;

        }

        public void OnButtonUp(Hand fromHand)
        {
            ColorSelf(Color.white);
        }

        private void ColorSelf(Color newColor)
        {
            Renderer[] renderers = this.GetComponentsInChildren<Renderer>();
            for (int rendererIndex = 0; rendererIndex < renderers.Length; rendererIndex++)
            {
                renderers[rendererIndex].material.color = newColor;
            }
        }
    }
}