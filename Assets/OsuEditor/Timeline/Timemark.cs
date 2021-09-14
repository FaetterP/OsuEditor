﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.OsuEditor.Timeline
{
    class Timemark : MonoBehaviour, ICloneable, IComparable<Timemark>
    {
        private CreatorTimemarks creator;
        public int time;
        public Color color;
        public int height;

        void Start()
        {
            creator = FindObjectOfType<CreatorTimemarks>();
            GetComponent<Image>().color = color;
            var t = transform.localScale;
            t.y = height;
            transform.localScale = t;
        }

        void Update()
        {
            if (Global.MusicTime > time - Global.AR_ms && Global.MusicTime < time + Global.AR_ms)
            {
                int x = OsuMath.GetMarkX(time, -500, 500, Global.MusicTime - Global.AR_ms, Global.MusicTime + Global.AR_ms);
                transform.localPosition = new Vector2(x, 0);
            }
            else
            {
                DestroyFromScreen();
            }
        }

        public void DestroyFromScreen()
        {
            creator.RemoveMarkFromScreen(time);
            Destroy(gameObject);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public int CompareTo(Timemark other)
        {
            return time.CompareTo(other.time);
        }
    }
}
