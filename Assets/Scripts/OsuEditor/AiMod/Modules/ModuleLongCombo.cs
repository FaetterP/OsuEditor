﻿using Assets.Scripts.MapInfo.HitObjects;
using Assets.Scripts.OsuEditor.AiMod.Messages;
using Assets.Scripts.Utilities.Lang;
using System.Collections.Generic;

namespace Assets.Scripts.OsuEditor.AiMod.Modules
{
    class ModuleLongCombo : IModule
    {
        private LocalizedString _message = new LocalizedString("AiMod.message.longCombo");

        public ModuleType Type => ModuleType.Compose;

        public List<AiMessage> GetMessages()
        {
            List<AiMessage> ret = new List<AiMessage>();

            foreach (var t in Global.Map.OsuHitObjects)
            {
                if (t is OsuSpinner)
                    continue;

                if (Global.Map.GetComboInfo(t.Time).Number >= 25)
                {
                    ret.Add(new Warning(_message.GetValue(), t.Time));
                }
            }

            return ret;
        }
    }
}
