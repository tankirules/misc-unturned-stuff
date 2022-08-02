using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Reflection;
using SDG.Framework.Utilities;

namespace Random.miscstuff
{
    class viewangle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "viewangle";
        public string Help => "Get viewangle while in vehicle";
        public string Syntax => "/viewangle";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "viewangle" };
        public void Execute(IRocketPlayer caller, string[] args)
        {
            var player = ((UnturnedPlayer)caller).Player;
            miscstuff.Instance.toggleviewangle(player);
        }

        }
}
