using Rocket.Core;
using Rocket.API.Collections;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using SDG.Unturned;
using UnityEngine;

namespace Random.miscstuff
{
    public class miscstuff : RocketPlugin<miscstuffConfiguration>
    {
        public static miscstuff Instance;
        public static miscstuffConfiguration Config;
        protected override void Load()
        {
            UnturnedPlayerEvents.OnPlayerUpdateStat += OnPlayerUpdateStat;
            VehicleManager.onSiphonVehicleRequested += onVehicleSiphoning;
            Instance = this;
            Config = Instance.Configuration.Instance;
            Rocket.Core.Logging.Logger.Log("Plugin Loaded");

        }

        protected override void Unload()
        {
            UnturnedPlayerEvents.OnPlayerUpdateStat -= OnPlayerUpdateStat;
            VehicleManager.onSiphonVehicleRequested -= onVehicleSiphoning;
            Rocket.Core.Logging.Logger.Log("Plugin Unloaded");
        }



        public void OnPlayerUpdateStat(UnturnedPlayer player, EPlayerStat stat)
        {
            if (player == null) return;
            if (stat == EPlayerStat.KILLS_ZOMBIES_NORMAL)
            {
                ChangeExperience(player, Config.ZombieXP);
            }
            else if (stat == EPlayerStat.KILLS_ZOMBIES_MEGA)
            {
                ChangeExperience(player, Config.MegaXP);
            }

        }
        public void onVehicleSiphoning(InteractableVehicle vehicle, Player instigatingPlayer, ref bool shouldAllow, ref ushort desiredAmount)
        {
            if (Config.Siphon == false)
            {
                shouldAllow = false;
                UnturnedChat.Say(UnturnedPlayer.FromPlayer(instigatingPlayer), "You are not allowed to remove fuel from vehicles!", Color.red);
            }
        }
        public int ChangeExperience(UnturnedPlayer player, int change)
        {
            // COPY PHASERS CODE AND HOPE IT WORKS XD
            long exp = player.Experience + change;
            player.Experience = (exp < uint.MinValue) ? uint.MinValue : (exp > uint.MaxValue) ? uint.MaxValue : (uint)exp;
            return (int)(change + player.Experience - exp);
        }




    }       
}
