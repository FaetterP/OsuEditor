﻿using Assets.Scripts.OsuEditor.HitObjects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.MapInfo
{
    class MapClass
    {
        public General General;
        public Editor Editor;
        public Metadata Metadata;
        public Difficulty Difficulty;
        public Events Events;
        public List<Color> Colors;
        public List<OsuHitObject> OsuHitObjects;

        private Dictionary<int, ComboInfo> _comboInfos;

        private List<TimingPoint> _timingPoints;
        public ReadOnlyCollection<TimingPoint> TimingPoints => _timingPoints.AsReadOnly();

        public MapClass()
        {
            General = new General();
            Editor = new Editor();
            Metadata = new Metadata();
            Difficulty = new Difficulty();
            Events = new Events();
            _timingPoints = new List<TimingPoint>();
            Colors = new List<Color>();
            OsuHitObjects = new List<OsuHitObject>();
            _comboInfos = new Dictionary<int, ComboInfo>();
        }

        public void UpdateComboInfos()
        {
            _comboInfos.Clear();
            OsuHitObjects.Sort();
            int color_num = 0, number = 1;
            foreach (OsuHitObject t in OsuHitObjects)
            {
                if (t is OsuCircle && !(t is OsuSlider))
                {
                    int sum_color = (t as OsuCircle).combo_sum;
                    (t as OsuCircle).combo_sum = sum_color;
                    if (sum_color == 1)
                    {
                        number++;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                    else if (sum_color == 5)
                    {
                        color_num++;
                        color_num %= Colors.Count;
                        number = 1;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                    else
                    {
                        color_num += (sum_color / 16) + 1;
                        color_num %= Colors.Count;
                        number = 1;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                }
                else if (t is OsuSlider)
                {
                    int sum_color = (t as OsuSlider).combo_sum;
                    (t as OsuSlider).combo_sum = sum_color;
                    if (sum_color == 2)
                    {
                        number++;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                    else if
                        (sum_color == 6)
                    {
                        color_num++;
                        color_num = color_num % Colors.Count;
                        number = 1;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                    else
                    {
                        color_num += (sum_color / 16) + 1;
                        color_num = color_num % Colors.Count;
                        number = 1;
                        ComboInfo toAdd = new ComboInfo(number, color_num);
                        _comboInfos.Add(t.Time, toAdd);
                    }
                }
                else if (t is OsuSpinner)
                {
                    ComboInfo toAdd = new ComboInfo(9999, 0);
                    _comboInfos.Add(t.Time, toAdd);
                }
            }
        }

        public ComboInfo GetComboInfo(int time)
        {
            return _comboInfos[time];
        }

        public void AddHitObject(OsuHitObject obj)
        {
            if (IsContains(obj.Time))
            {
                int index = OsuHitObjects.Select(x => x.Time).ToList().IndexOf(obj.Time);
                OsuHitObjects.RemoveAt(index);
                OsuHitObjects.Add(obj);
            }
            else
            {
                OsuHitObjects.Add(obj);
            }

            OsuHitObjects.Sort();
            UpdateComboInfos();
        }

        private bool IsContains(int time)
        {
            foreach (var t in OsuHitObjects)
            {
                if (t.Time == time)
                    return true;
            }
            return false;
        }

        private string GetMapTXT()
        {
            string ret =
                "osu file format v14\n\n" +
                "[General]\n" +
                "AudioFilename: " + General.AudioFilename + "\n" +
                "AudioLeadIn: " + General.AudioLeadIn + "\n" +
                "PreviewTime: " + General.PreviewTime + "\n" +
                "Countdown: " + General.Countdown + "\n" +
                "SampleSet: " + General.SampleSet + "\n" +
                "StackLeniency: " + General.StackLeniency + "\n" +
                "Mode: " + General.Mode + "\n" +
                "LetterboxInBreaks: " + General.LetterboxInBreaks + "\n" +
                "WidescreenStoryboard: " + (General.WidescreenStoryboard ? 1 : 0) + "\n" +
                "\n" +
                "[Editor]\n" +
                "Bookmarks:\n" +
                "DistanceSpacing: " + Editor.DistanceSpacing + "\n" +
                "BeatDivisor: " + Editor.BeatDivisor + "\n" +
                "GridSize: " + Editor.GridSize + "\n" +
                "TimelineZoom: " + Editor.TimelineZoom + "\n" +
                "\n" +
                "[Metadata]\n" +
                "Title:" + Metadata.Title + "\n" +
                "TitleUnicode:" + Metadata.TitleUnicode + "\n" +
                "Artist:" + Metadata.Artist + "\n" +
                "ArtistUnicode:" + Metadata.ArtistUnicode + "\n" +
                "Creator:" + Metadata.Creator + "\n" +
                "Version:" + Metadata.Version + "\n" +
                "Source:" + Metadata.Source + "\n" +
                "Tags:" + Metadata.Tags + "\n" +
                "BeatmapID:-1\n" +
                "BeatmapSetID:-1\n" +
                "\n" +
                "[Difficulty]\n" +
                "HPDrainRate:" + Difficulty.HPDrainRate + "\n" +
                "CircleSize:" + Difficulty.CircleSize + "\n" +
                "OverallDifficulty:" + Difficulty.OverallDifficulty + "\n" +
                "ApproachRate:" + Difficulty.ApproachRate + "\n" +
                "SliderMultiplier:" + Difficulty.SliderMultiplier + "\n" +
                "SliderTickRate:" + Difficulty.SliderTickRate + "\n" +
                "\n" +
                "[Events]\n" +
                "//Background and Video events\n" +
                "0,0,\"" + Events.BackgroungImage + "\",0,0\n" +
                "//Break Periods\n" +
                "//Storyboard Layer 0 (Background)\n" +
                "//Storyboard Layer 1 (Fail)\n" +
                "//Storyboard Layer 2 (Pass)\n" +
                "//Storyboard Layer 3 (Foreground)\n" +
                "//Storyboard Sound Samples\n" +
                "\n" +
                "[TimingPoints]\n";
            foreach (var t in TimingPoints)
            {
                ret += t.ToString() + "\n";
            }
            ret += "\n" +
             "[Colours]\n";
            int i = 1;
            foreach (var t in Colors)
            {
                ret += "Combo" + i + " : " + (int)(t.r * 255) + "," + (int)(t.g * 255) + "," + (int)(t.b * 255) + "\n";
                i++;
            }
            ret += "\n" +
            "[HitObjects]\n";
            foreach (var t in OsuHitObjects)
            {
                ret += t.ToString() + "\n";
            }

            return ret;
        }

        public void SaveToFile()
        {
            StreamWriter sw = new StreamWriter(Global.FullPathToMap);
            sw.Write(GetMapTXT());
            sw.Close();
        }

        public ReadOnlyCollection<TimingPoint> GetParentTimingPoints()
        {
            List<TimingPoint> ret = new List<TimingPoint>();
            foreach (var point in _timingPoints)
            {
                if (point.isParent)
                    ret.Add(point);
            }
            return ret.AsReadOnly();
        }

        public void AddTimingPoint(TimingPoint added)
        {
            _timingPoints.Add(added);
            _timingPoints.Sort();

            UpdateTimingPointsBPM();
        }

        public void RemoveTimingPoint(TimingPoint removed)
        {
            if (_timingPoints.Count <= 1)
                return;

            _timingPoints.Remove(removed);
            if (_timingPoints[0].isParent == false)
                _timingPoints[0].isParent = true;

            UpdateTimingPointsBPM();
        }

        private void UpdateTimingPointsBPM()
        {
            double lastBPM = -1;
            foreach (var t in _timingPoints)
            {
                if (t.isParent)
                {
                    lastBPM = t.BeatLength;
                    continue;
                }

                t.BeatLength = lastBPM;
            }
        }

        /////////////////////////////////////////////

        public OsuHitObject GetHitObjectFromTime(int time)
        {
            foreach (var t in OsuHitObjects)
            {
                if (t.Time == time) { return t; }
            }
            return null;
        }

        public TimingPoint GetNearestTimingPointLeft(int time, bool isParent)
        {
            TimingPoint ret = null;
            int razn = 1;
            if (isParent)
            {
                foreach (var t in TimingPoints)
                {
                    if (razn >= 0 && t.isParent)
                    {
                        razn = time - t.Offset;
                        ret = t;
                    }
                }
            }
            else
            {
                foreach (var t in TimingPoints)
                {
                    razn = time - t.Offset;
                    if (razn >= 0)
                    {
                        ret = t;
                    }
                }
            }
            return ret;
        }

        public double SliderLengthToAddedTime(double length, double multiplier, double beat_length)
        {
            double ret;
            ret = (length * beat_length) / (multiplier * 100.0 * Difficulty.SliderMultiplier);
            return ret;
        }

        public void SortTimingPoints()
        {
            _timingPoints.Sort();
        }
    }
}
