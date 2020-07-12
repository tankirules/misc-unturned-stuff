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
    //open a registered door
    class opendoor : IRocketCommand
    {
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "opendoor";
        public string Help => "opens a registered door you have permissions to";
        public string Syntax => "/opendoor";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "opendoor" };
        
        public void Execute(IRocketPlayer caller, string[] args)
        {
            var PlayerCaller = (UnturnedPlayer)caller;

            InteractableDoorHinge component = raycastdoor.Getdoor(PlayerCaller);

            if (component != null)
            {
                bool isregistered = false;
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
                foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                {
                    if (doorinfo.ID == ID)
                    {
                        isregistered = true;
                        if (caller.HasPermission(doorinfo.permission))

                        {
                            //OPEN THE DOOOOOOOR
                            Rocket.Core.Logging.Logger.Log("Player has perm: " + doorinfo.permission);
                            SteamCaller steamCaller = (BarricadeManager)typeof(BarricadeManager).GetField("manager", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
                            door.updateToggle(flag);
                            steamCaller.channel.send("tellToggleDoor", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                             {
                               x,
                               y,
                               plant,
                               index,
                               flag
                          });
                            Rocket.Core.Logging.Logger.Log("Player toggled door " + doorinfo.ID);
                            UnturnedChat.Say(caller, doorinfo.name + " door toggled", Color.yellow);
                        }
                        else {
                            UnturnedChat.Say(caller, "You do not have the right permissions to open this door", Color.red);
                        }
                    }
                    
                }
                if (isregistered == false){
                    UnturnedChat.Say(caller, "Door is not registered!", Color.red);

                }



            }




        }




    }
}

