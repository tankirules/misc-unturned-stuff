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
    public class calculatepreciseangle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "calculatepreciseangle";
        public string Help => "Calculate precise firing angle (MUST BE IN VEHICLE BINO SEAT WITH MARKER PLACED)";
        public string Syntax => "/calculatepreciseangle";
        public List<string> Aliases => new List<string> { "cpa" };
        public List<string> Permissions => new List<string> { "calculatepreciseangle" };

        //With inspiration from gaming: Tanks
        public void Execute(IRocketPlayer caller, string[] command)
        {
            
            UnturnedPlayer player = (UnturnedPlayer)caller;
            
            //dont need this
            ////check if player already calculating
            //if (miscstuff.Instance.calcangle.ContainsKey(player.Player))
            //{
            //    UnturnedChat.Say(player, "Already calculating!", Color.red);
            //    return;
            //}

            //just stop coroutine if found in miscstuf.cs

            //check if player has marker placed
            if (!player.Player.quests.isMarkerPlaced)
            {
                UnturnedChat.Say(player, "No Marker Placed!", Color.red);
                return;
            }

            //Vector3 playerLocation = player.Player.look.aim.position;
            //check
            Vector3 markerLocation = new Vector3(player.Player.quests.markerPosition.x, player.Position.y, player.Player.quests.markerPosition.z);
            //check if player is in vehicle
            if (player.CurrentVehicle == null)
            {
                UnturnedChat.Say(player, "You are not in vehicle!", Color.red);
                return;
            }

            

            var steamplayer = player.SteamPlayer();
            Passenger pass = null;
            foreach (Passenger passenger in player.CurrentVehicle.turrets)
            {
                if (passenger.player == steamplayer)
                {
                    pass = passenger;
                    break;
                }
            }

            //get the current "gun" of the player calling

            ushort currentturretgun = pass.turret.itemID;

            //if player isnt hoplding valid id
            if (!miscstuff.Config.artycalculatorids.Contains(currentturretgun))
            {
                UnturnedChat.Say(player, "Current turret gun: " + currentturretgun);
                UnturnedChat.Say(player, "You are not in a seat that can do that!", Color.red);
            }

            miscstuff.Instance.startrangefinding(caller);

            

            //UnturnedChat.Say(player, "playerloc is " + playerLocation.ToString());
            //UnturnedChat.Say(player, "turretloc is " + pass.turretAim.position.ToString());

        }
     }
}
