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
    public class raycastdoor
    {
        public static InteractableDoorHinge Getdoor(UnturnedPlayer player){
            PlayerLook look = player.Player.look;
            RaycastHit raycastHit;
            if (!PhysicsUtility.raycast(new Ray(look.aim.position, look.aim.forward), out raycastHit, 100, RayMasks.BARRICADE, 0))
            {
                return null;
            }
            InteractableDoorHinge interactabledoorhinge = raycastHit.transform.GetComponent<InteractableDoorHinge>();
            return interactabledoorhinge;
        }
    }
}
