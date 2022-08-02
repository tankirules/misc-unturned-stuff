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
    public class exactangle : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "exactangle";
        public string Help => "exact angle for debugging";
        public string Syntax => "/exactangle";
        public List<string> Aliases => new List<string> { "ea" };
        public List<string> Permissions => new List<string> { "ea" };

        //With inspiration from gaming: Tanks
        public void Execute(IRocketPlayer caller, string[] command)
        {
            
            UnturnedPlayer player = (UnturnedPlayer)caller;

            float force = (6000 / 50);
            float gravity = 9.82f;
            Vector3 marker = player.Player.quests.markerPosition;
            Vector2 targetposition;
            targetposition.x = marker.x;
            targetposition.y = marker.z;
            Vector2 playerpos;
            playerpos.x = player.Player.look.aim.position.x;
            playerpos.y = player.Player.look.aim.position.z;
            var distance = Vector2.Distance(playerpos, targetposition);

            Vector3 raycastorigin = new Vector3(targetposition.x, 300, targetposition.y);
            RaycastHit raycastHit;
            PhysicsUtility.raycast(new Ray(raycastorigin, Vector3.down), out raycastHit, 300, RayMasks.GROUND, 0);

            Vector3 hit = raycastHit.point;

            UnturnedChat.Say("Raycast hit at " + hit, Color.yellow);

            var height = hit.y - player.Player.look.aim.position.y;

            var x = distance;
            var y = height;
            var v = force;
            var g = gravity;

            var AngleDegplus = Math.Atan((v*v+(Math.Sqrt(Math.Pow(v,4) - g*(g*x*x+2*y*v*v))))/ (gravity * x));
            var AngleDegminus = Math.Atan((v * v - (Math.Sqrt(Math.Pow(v, 4) - g * (g * x * x + 2 * y * v * v)))) / (gravity * x));

            AngleDegplus = (180 / Math.PI) * AngleDegplus;
            AngleDegminus = (180 / Math.PI) * AngleDegminus;

            UnturnedChat.Say(caller, "Exact Angle plus: " + AngleDegplus, Color.yellow);
            UnturnedChat.Say(caller, "Exact Angle minus: " + AngleDegminus, Color.yellow);

        }
     }
}
