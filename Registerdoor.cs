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
            PlayerLook look = PlayerCaller.Player.look;
            RaycastHit raycastHit;
            // actually get the door you are looking at 
            if (args.Length != 2)
            {
                UnturnedChat.Say(caller, "Invalid arguments", Color.red);
                return;
            }
            if (!PhysicsUtility.raycast(new Ray(look.aim.position, look.aim.forward), out raycastHit, 100, RayMasks.BARRICADE, 0))
            {
                UnturnedChat.Say(caller, "Not looking at an object!");
                return;
            }
            InteractableDoorHinge component = raycastHit.transform.GetComponent<InteractableDoorHinge>();
            if (component != null)
            {
                var name = args[0];
                var permissionname = args[1];
                InteractableDoor door = component.door;
                bool flag = !door.isOpen;
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
                tempvar.x = x;
                tempvar.y = y;
                tempvar.plant = plant;
                tempvar.index = index;
                tempvar.barricadeRegion = barricadeRegion;
                miscstuff.Instance.Configuration.Instance.listofregistereddoors.Add(tempvar);
                
            }
            else
            {
                UnturnedChat.Say(caller, "Not looking at a door!");
            }
        }

    }
}
