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
    class changedoorperm : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "doorperm";
        public string Help => "Changes the permissions of a door";
        public string Syntax => "/doorperm newperm or /doorperm doorname newperm";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> {"registerdoor"};

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if (args.Length > 2||args.Length < 1){
                UnturnedChat.Say(caller,"invalid arguments",Color.red);
                return;

            }
            else if (args.Length == 2)
            {
                var doorname = args[0];
                var newperm = args[1];
                bool isregistered = false;
                foreach (Registereddoortype doorinfo in miscstuff.Config.listofregistereddoors)
                {
                    if (doorinfo.name == doorname)
                    {
                        isregistered = true;
                        doorinfo.permission = "Registereddoor." + newperm;
                        return;
                    }
                }
                if(isregistered == false) {
                    UnturnedChat.Say(caller, "door does not exist with that name", Color.red);
                    return;
                }

            }
            else if (args.Length == 1)
            {

            }
        }
    }
}
