using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    class customdoorperm : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "customdoorperm";
        public string Help => "Changes the permissions of a door to a custom permission someone already has";
        public string Syntax => "/customdoorperm customperm or /customdoorperm doorname customperm";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "registerdoor" };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length > 2 || args.Length < 1)
            {
                UnturnedChat.Say(caller, "invalid arguments", Color.red);
                return;
    
            }
            else if (args.Length == 2)
            {

            }
        }
    }
}
