﻿using Assets.Scripts.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.OsuEditor.HitSounds
{
    class SpinnerLength : MonoBehaviour
    {
        private InputField thisInput;

        void Awake()
        {
            thisInput = GetComponent<InputField>();
        }

        void OnEnable()
        {
            OsuSpinner t = Global.SelectedHitObject as OsuSpinner;
            thisInput.text = (t.TimeEnd - t.Time).ToString();
        }
    }
}