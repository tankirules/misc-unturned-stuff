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
    public class spottargetcommand : IRocketCommand
    {
        private static miscstuffConfiguration Config;
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "spottarget";
        public string Help => "Gets the distance from the gunners in an arty ship to the target the spotter is looking at";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "spottarget" };

        public Vector3? GetEyePositionray(UnturnedPlayer player,float distance, int masks)
        {
            PlayerLook Look = player.Player.look;
            Physics.Raycast(Look.aim.position, Look.aim.forward, out var raycastHit, distance, masks);

            if (raycastHit.transform == null)
                return null;

            return raycastHit.point;
        }

        public Vector3? GetEyePosition(UnturnedPlayer player, float distance)
        {
            return GetEyePositionray(player,distance, RayMasks.BLOCK_COLLISION & ~(1 << 0x15));
        }

        //geteyeposition geteyelook = new geteyeposition();
        public void Execute(IRocketPlayer caller, string[] command)
        {

            var playercaller = (UnturnedPlayer)caller;
            if (playercaller.CurrentVehicle == null) {
                UnturnedChat.Say(playercaller, "You are Not In A Vehicle!", Color.red);

            }
            else{
                PlayerLook Look = playercaller.Player.look;
                var dist = 3000f;
                var target = GetEyePosition(playercaller, dist);
                if (!target.HasValue)
                {
                    UnturnedChat.Say(playercaller, "The target you are looking at is invalid!", Color.red);
                }
                else
                {
                    byte callerseat ;
                    playercaller.CurrentVehicle.findPlayerSeat(playercaller.CSteamID, out callerseat);
                    Passenger playercalling = playercaller.CurrentVehicle.passengers[(int)callerseat];
                    if (playercalling.turret == null)
                    {
                        UnturnedChat.Say(playercaller, "Cannot spot in driver seat!", Color.red);
                    }
                    else {
                        var targetconverted = target.Value;
                        foreach (Passenger vehiclepassenger in playercaller.CurrentVehicle.passengers)
                        {
                            var playerpassenger = UnturnedPlayer.FromSteamPlayer(vehiclepassenger.player);
                            var passengerposition = playerpassenger.Position;
                            passengerposition.y += Config.artyshipoffsetlisty;
                            passengerposition.x += Config.artyshipoffsetlistx;
                            passengerposition.z += Config.artyshipoffsetlistz;
                            var angle = Vector3.Angle(passengerposition, targetconverted);
                            UnturnedChat.Say(playerpassenger, "Range to target by spotter: " + Convert.ToString(System.Math.Sqrt(System.Math.Pow((targetconverted.x -
                                passengerposition.x), 2) + System.Math.Pow((targetconverted.z - passengerposition.z), 2))) + " at bearing: " + Convert.ToString(angle), Color.white);

                        }
                    }
                    

                }
        }


            
            
    }

    }
               
    

                     
    
                
    
}
