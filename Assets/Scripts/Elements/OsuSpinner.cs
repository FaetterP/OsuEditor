﻿using Assets.Scripts.OsuEditor.Timeline;
using Assets.Scripts.OsuEditor.Timeline.Timemarks;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Elements
{
    class OsuSpinner : OsuHitObject
    {
        private Image _thisImage;
        private int _timeEnd;
        public int TimeEnd
        {
            get
            {
                return _timeEnd;
            }
            set
            {
                if (value <= Time) { throw new ArgumentException(); }
                _timeEnd = value;
            }
        }

        void Awake()
        {
            _thisImage = GetComponent<Image>();
        }

        void Start()
        {
            gameObject.transform.localPosition = OsuMath.OsuCoordsToUnity(new Vector2(X, Y));
        }

        void Update()
        {
            if (Global.MusicTime < Time - Global.AR_ms)
            {
                Destroy(gameObject);
            }
            else if (Global.MusicTime < Time)
            {
                transform.rotation = Quaternion.Euler(0, 0, Time);
                _thisImage.color = new Color(1, 1, 1, 0.1f);
            }
            else if (Global.MusicTime > Time && Global.MusicTime < TimeEnd)
            {
                transform.rotation = Quaternion.Euler(0, 0, Global.MusicTime);
                _thisImage.color = new Color(1, 1, 1, 0.1f + 0.9f * ((Global.MusicTime - Time * 1.0f) / (TimeEnd - Time)));
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public OsuSpinner Clone()
        {
            return (OsuSpinner)MemberwiseClone();
        }

        public override string ToString()
        {
            // 256,192,734,12,8,4992,0:1:0:0:
            return "256,192," + Time + ",12,8," + TimeEnd + ",0:1:0:0";
        }

        public override bool IsRightTime()
        {
            return Global.MusicTime < TimeEnd && Global.MusicTime > Time - Global.AR_ms;
        }

        public override void Init(OsuHitObject obj)
        {
            OsuSpinner other = obj as OsuSpinner;
            SetCoords(256, 192);
            Time = other.Time;
            TimeEnd = other.TimeEnd;
        }

        public override TimemarkCircle[] GetTimemark()
        {
            TimemarkCircle[] ret = new TimemarkCircle[2];

            TimemarkSpinnerStart toAdd = TimemarkSpinnerStart.GetTimemark(this);
            ret[0] = toAdd;

            TimemarkSpinnerEnd toAdd2 = TimemarkSpinnerEnd.GetTimemark(this);
            ret[1] = toAdd2;

            return ret;
        }
    }
}