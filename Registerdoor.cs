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
using SDG.Framework.Utilities;

namespace Random.miscstuff
{
    //register a door and allow it to be used by people with the specified permissions
    class Registerdoor : IRocketCommand
    {
        
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "registerdoor";
        public string Help => "Registers a door for a specific permission group";
        public string Syntax => "/registerdoor yourdoorname permissionname";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "registerdoor" };

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var PlayerCaller = (UnturnedPlayer)caller;
            
            // actually get the door you are looking at 
            if (args.Length != 2)
            {
                UnturnedChat.Say(caller, "Invalid arguments", Color.red);
                return;
            }
            InteractableDoorHinge component = raycastdoor.Getdoor(PlayerCaller);

            if (component != null)
            {
                var name = args[0];
                var permissionname = args[1];
                InteractableDoor door = component.door;
                byte x;
                byte y;
                ushort plant;
                ushort index;
                
                BarricadeRegion barricadeRegion;
                BarricadeManager.tryGetInfo(door.transform, out x, out y, out plant, out index, out barricadeRegion);
                var permission = "Registereddoor." + permissionname;
                Registereddoortype tempvar = new Registereddoortype();
                tempvar.name = name;
                tempvar.permission = permission;
                tempvar.steamid = PlayerCaller.CSteamID.ToString();
                tempvar.doorposition = door.transform.position;
                BarricadeDrop barricadedrop = barricadeRegion.drops[index];
                var ID = barricadedrop.instanceID;
                tempvar.ID = ID;
                if (miscstuff.Instance.Configuration.Instance.listofregistereddoors.Count > 0 ){
                    foreach (Registereddoortype doorinfo in miscstuff.Instance.Configuration.Instance.listofregistereddoors)
                    {
                        if (doorinfo.name == tempvar.name) {
                            UnturnedChat.Say(caller, "A door already exists with that name!", Color.red);
                            return;
                        }
                        else if (doorinfo.ID == ID)
                        {
                            UnturnedChat.Say(caller, "This door " + doorinfo.name + " is already registered!", Color.red);
                            return;
                        }
                    }
                }
                

                miscstuff.Instance.Configuration.Instance.listofregistereddoors.Add(tempvar);
                UnturnedChat.Say(caller, "Door Registered!", Color.blue);
                miscstuff.Instance.Configuration.Save();
            }
            else
            {
                UnturnedChat.Say(caller, "Not looking at a door!");
                return;
            }
        }

    }
}
