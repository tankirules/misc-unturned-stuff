using Rocket.Core;
using Rocket.API.Collections;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using SDG.Unturned;
using UnityEngine;
using Rocket.API;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Random.miscstuff
{
    class savedoordata : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "savedoordata";
        public string Help => "saves door data to file";
        public string Syntax => "/savedoordata";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "savedoordata" };

        public void Execute(IRocketPlayer caller, string[] args) {
            miscstuff.Instance.Configuration.Save();
            UnturnedChat.Say(caller, "Config saved!");
        }
    }
}
