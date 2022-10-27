﻿using Assets.Scripts.OsuEditor.HitObjects;
using Assets.Scripts.OsuEditor.AiMod.Messages;
using Assets.Scripts.Utilities.Lang;
using System.Collections.Generic;

namespace Assets.Scripts.OsuEditor.AiMod.Modules
{
    class ModuleShortSpinner : IModule
    {
        private LocalizedString _message = new LocalizedString("AiMod.message.shortSpinner");

        public ModuleType Type => ModuleType.Compose;

        public List<AiMessage> GetMessages()
        {
            List<AiMessage> ret = new List<AiMessage>();

            foreach (var t in Global.Map.OsuHitObjects)
            {
                if (t is OsuSpinner)
                {
                    OsuSpinner spinner = t as OsuSpinner;

                    if (spinner.TimeEnd - spinner.Time <= 500)
                    {
                        ret.Add(new Warning(_message.GetValue(), spinner.Time));
                    }
                }
            }

            return ret;
        }
    }
}
