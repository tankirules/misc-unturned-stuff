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
    class Unregisterdoor : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "unregisterdoor";
        public string Help => "Unregisters a door you are looking at or a door by name";
        public string Syntax => "/unregisterdoor or /unregisterdoor doorname";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "registerdoor" };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            if(args.Length > 1)
            {
                UnturnedChat.Say(caller,"invalid arguments",Color.red);
                return;
            }
            else if (args.Length == 1)
            {
                bool isregistered = false;
                foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                {
                    if (doorinfo.name == args[0])
                    {
                        isregistered = true;
                        miscstuff.Instance.Configuration.Instance.listofregistereddoors.Remove(doorinfo);
                        UnturnedChat.Say(caller,"Door" + doorinfo.name + " Unregistered!");
                        miscstuff.Instance.Configuration.Save();

                    }
                }
                if (isregistered == false){
                    UnturnedChat.Say(caller, "Registered Door does not exist with that name",Color.red);
                }

            }
            else if (args.Length == 0)
            {
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
                    BarricadeManager.tryGetInfo(door.transform, out x, out y, out plant, out index, out barricadeRegion);
                    BarricadeDrop barricadedrop = barricadeRegion.drops[index];
                    var ID = barricadedrop.instanceID;



                    bool isregistered = false;
                    foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                    {
                        if (doorinfo.ID == ID)
                        {
                            isregistered = true;
                            miscstuff.Instance.Configuration.Instance.listofregistereddoors.Remove(doorinfo);
                            UnturnedChat.Say(caller, "Door" + doorinfo.name + " Unregistered!");
                            miscstuff.Instance.Configuration.Save();
                        }
                    }

                    if (isregistered == false)
                    {
                        UnturnedChat.Say(caller, "That door is not registered!", Color.red);
                    }
                }

                else
                {
                    UnturnedChat.Say(caller, "You are not looking at a valid object", Color.red);
                }

            }

        }

    }
}
