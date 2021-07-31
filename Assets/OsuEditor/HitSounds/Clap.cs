﻿using Assets.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.OsuEditor.HitSounds
{
    class Clap : MonoBehaviour
    {
        private Image thisImage;

        void Awake()
        {
            thisImage = GetComponent<Image>();
        }

        void OnEnable()
        {
            if((Global.SelectedHitObject as OsuCircle).clap)
            {
                thisImage.color = new Color(1, 1, 1, 1);
            }
            else
            {
                thisImage.color = new Color(1, 1, 1, 0.5f);
            }
        }
        void OnMouseDown()
        {
            OsuCircle c = (Global.SelectedHitObject as OsuCircle);
            if (c.clap)
            {
                c.clap = false;
                thisImage.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                c.clap = true;
                thisImage.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
