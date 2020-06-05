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
                foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                {
                    if (doorinfo.name == doorname)
                    {
                        isregistered = true;
                        doorinfo.permission = "Registereddoor." + newperm;
                        miscstuff.Instance.Configuration.Save();
                        UnturnedChat.Say(caller,"Door perm changed successfully to " + newperm);
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
                var newperm = args[0];
                bool isregistered = false;
                var PlayerCaller = (UnturnedPlayer)caller;
                InteractableDoorHinge component = raycastdoor.Getdoor(PlayerCaller);
                if (component != null)
                {

                    InteractableDoor door = component.door;
                    bool flag = !door.isOpen;
                    byte x;
                    byte y;
                    ushort plant;
                    ushort index;
                    BarricadeRegion barricadeRegion;
                    BarricadeManager.tryGetInfo(door.transform, out x, out y, out plant, out index,
                        out barricadeRegion);
                    var ID = door.GetInstanceID();
                    foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                    {
                        if (doorinfo.x == x && doorinfo.y == y && doorinfo.plant == plant && doorinfo.index == index && doorinfo.ID == ID)
                        {
                            isregistered = true;
                            doorinfo.permission = "Registereddoor." + newperm;
                            UnturnedChat.Say(caller, "Door perm changed successfully to " + newperm);
                            miscstuff.Instance.Configuration.Save();
                        }
                    }

                    if (isregistered == false)
                    {
                        UnturnedChat.Say(caller, "door is not registered", Color.red);
                    }
                }
                else
                {
                    UnturnedChat.Say(caller, "Not looking at a valid door!", Color.red);
                }
            }
        }
    }
}
