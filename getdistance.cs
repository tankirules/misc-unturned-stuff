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
    public class getdistancecommand : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "getdistance";
        public string Help => "Gets the straight line distance from player to their marker";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "getdistance" };

        public void Execute(IRocketPlayer caller, string[] command) {
            var player = (UnturnedPlayer)caller;
            var steamid = player.CSteamID;
            Vector3 playerposition;
            Vector3 markerposition;
            if (player.Player.quests.isMarkerPlaced == true)
            {
                markerposition = player.Player.quests.markerPosition;
                playerposition = player.Position;
                UnturnedChat.Say(caller, Convert.ToString(System.Math.Sqrt(System.Math.Pow((markerposition.x - playerposition.x), 2) + System.Math.Pow((markerposition.z - playerposition.z), 2))));
            }
            else {
                UnturnedChat.Say(caller, "You have not placed a marker!");
            }
            

        }
       
    }
}
