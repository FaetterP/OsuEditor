﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.OsuEditor
{
    class MusicLineMark : MonoBehaviour
    {
        [NonSerialized]public Color Color;
        [NonSerialized]public int timestamp;
        [NonSerialized] public bool isUp;

        void Start()
        {
            AudioSource music = FindObjectOfType<AudioSource>();
            GetComponent<Image>().color = Color;
            var pos = transform.localPosition;
            pos.x = OsuMath.GetMarkX(timestamp, (int)(transform.parent.GetComponent<RectTransform>().rect.width / -2) , (int)(transform.parent.GetComponent<RectTransform>().rect.width / 2), 0, (int)(music.clip.length * 1000));
            pos.y = isUp ? 10 : -10;
            transform.localPosition = pos;
        }
    }
}
