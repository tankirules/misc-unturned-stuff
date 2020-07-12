using System;
using System.Net.Configuration;
using Rocket.Core;
using Rocket.API;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Chat;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using SDG.Unturned;
using Steamworks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
            BarricadeManager.onTransformRequested += OnTransformRequested;
            Level.onLevelLoaded += OnLevelLoaded;
            Instance = this;
            Config = Instance.Configuration.Instance;
            Rocket.Core.Logging.Logger.Log("Plugin Loaded");
            BarricadeManager.onDamageBarricadeRequested += OnDamageBarricade;
        }
        

        protected override void Unload()
        {
            
            UnturnedPlayerEvents.OnPlayerUpdateStat -= OnPlayerUpdateStat;
            VehicleManager.onSiphonVehicleRequested -= onVehicleSiphoning;
            BarricadeManager.onTransformRequested -= OnTransformRequested;
            Level.onLevelLoaded -= OnLevelLoaded;
            BarricadeManager.onDamageBarricadeRequested -= OnDamageBarricade;

            var barricades = GetBarricades(CSteamID.Nil, false);
            foreach (BarricadeData barricade in barricades)
            {
                foreach (Registereddoortype registereddoor in Configuration.Instance.listofregistereddoors)
                {
                    if (registereddoor.ID == barricade.instanceID)
                    {
                        registereddoor.doorposition = barricade.point;
                    }
                }
            }
            Configuration.Save();
            Rocket.Core.Logging.Logger.Log("Plugin Unloaded");
        }

        
        public void OnDamageBarricade(CSteamID steamid, Transform barricade,ref ushort barricadeushort, ref bool barricadebool, EDamageOrigin damageorigin)
        {
            byte x;
            byte y;
            ushort num;
            ushort index;
            BarricadeRegion barricadeRegion;
            BarricadeManager.tryGetInfo(barricade, out x, out y, out num, out index, out barricadeRegion);
            Rocket.Core.Logging.Logger.Log("Barricade damaged");
            
                var player = UnturnedPlayer.FromCSteamID(steamid);
                var steam64 = steamid;
                BarricadeData barricadetargeted = barricadeRegion.barricades[index];
                var itemid = barricadetargeted.barricade.id;
                ItemAsset itemAsset = (from i in new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>())
                    where i.itemName != null
                    orderby i.itemName.Length
                    where i.id == itemid
                    select i).FirstOrDefault<ItemAsset>();
                //stole this from rockets /i command
                var barricadename = itemAsset.itemName;
                var url = player.SteamProfile.AvatarFull.ToString();
                var bx = barricadetargeted.point.x;
                var by = barricadetargeted.point.y;
                var bz = barricadetargeted.point.z;
                //and then send to discord webhook
                Discord.SendWebhookPost(Configuration.Instance.raidalertchannel,
                    Discord.BuildDiscordEmbed("A barricade was damaged", "This barricade was damaged at " + DateTime.Now,
                        player.DisplayName, url, 16711680, new object[]
                        {
                            Discord.BuildDiscordField("Player steam64", steam64.ToString(), true),
                            Discord.BuildDiscordField("Barricade ID", itemid.ToString(), true),
                            Discord.BuildDiscordField("Barricade Name", barricadename, true),
                            Discord.BuildDiscordField("Barricade Position", "X: " + bx + " Y: " + by + " Z: " + bz,
                            true)

            }));



            

            
        }

        public void OnLevelLoaded(int signature)
        {
            var barricades = GetBarricades(CSteamID.Nil, false);
            foreach (BarricadeData barricade in barricades)
            {
                foreach (Registereddoortype registereddoor in Configuration.Instance.listofregistereddoors)
                {
                    if (registereddoor.doorposition == barricade.point)
                    {
                        registereddoor.ID = barricade.instanceID;
                        Rocket.Core.Logging.Logger.Log("Changed door " + registereddoor.name + " to instance ID " + barricade.instanceID);
                    }
                }
            }
            Configuration.Save();
        }






        public static IEnumerable<BarricadeData> GetBarricades(CSteamID id, bool includePlants)
        {
            var result = BarricadeManager.regions.Cast<BarricadeRegion>().SelectMany(brd => brd.barricades).ToList();

            if (includePlants)
                result.AddRange(BarricadeManager.plants.SelectMany(region => region.barricades));

            return id == CSteamID.Nil ? result : result.Where(k => k.owner == (ulong)id);
        }

        public void OnTransformRequested(CSteamID instigator, byte x, byte y, ushort plant, uint instanceID,ref Vector3 point, ref byte angle_x,ref byte angle_y, ref byte angle_z, ref bool shouldAllow)
        {
            foreach (Registereddoortype door in Config.listofregistereddoors)
            {
                if (door.ID == instanceID)
                {
                    door.doorposition = point;
                }
            }
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
