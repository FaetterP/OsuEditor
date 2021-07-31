﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.OsuEditor.Settings.TimingPoints.AddParent
{
    class TapButton : IClickable
    {
        [SerializeField] private Controller Controller;
        [SerializeField] private AudioSource Music;
        private double last_time;
        public override void Click()
        {
            if (!Music.isPlaying) { Music.Play(); }
            if (Controller.status == 0) { last_time = Time.time; Controller.SetStartTIme(Global.MusicTime); Controller.status = 1; return; }
            if (Controller.status == 2) { return; }

            Controller.AddTime((Time.time - last_time) * 1000);
            last_time = Time.time;
        }
    }
}
