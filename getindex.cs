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
    class getindex : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "getindex";
        public string Help => "gets the barricade index for debugging of a door";
        public string Syntax => "/getindex";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "getindex" };

        public void Execute(IRocketPlayer caller, string[] args)
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
                UnturnedChat.Say(caller,"Index of this door is: " + index);
                return;


            }
        }
    }
}
