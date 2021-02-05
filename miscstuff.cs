using System;
using System.IO;
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
using JetBrains.Annotations;
using UnityEngine.Assertions.Comparers;

namespace Random.miscstuff
{
    public class miscstuff : RocketPlugin<miscstuffConfiguration>
    {
        public static miscstuff Instance;
        public static miscstuffConfiguration Config;
        /*public Vector3 axishq;
        public Vector3 allieshq;*/
        public Vector2 axishq;
        public Vector2 allieshq;
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
            StructureManager.onDamageStructureRequested += OnDamageStructure;
            axishq.x = miscstuff.Instance.Configuration.Instance.axisx;
            axishq.y = miscstuff.Instance.Configuration.Instance.axisy;

            allieshq.x = miscstuff.Instance.Configuration.Instance.alliesx;
            allieshq.y = miscstuff.Instance.Configuration.Instance.alliesy;

        }
        

        protected override void Unload()
        {
            
            UnturnedPlayerEvents.OnPlayerUpdateStat -= OnPlayerUpdateStat;
            VehicleManager.onSiphonVehicleRequested -= onVehicleSiphoning;
            BarricadeManager.onTransformRequested -= OnTransformRequested;
            Level.onLevelLoaded -= OnLevelLoaded;
            BarricadeManager.onDamageBarricadeRequested -= OnDamageBarricade;
            StructureManager.onDamageStructureRequested -= OnDamageStructure;

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



        public void OnDamageStructure(CSteamID steamid, Transform structure, ref ushort num, ref bool barricadebool,
            EDamageOrigin damageorigin)
        {

            byte x;
            byte y;
            ushort index;
            StructureRegion structureRegion;
            string hq;
            hq = " ";


            StructureManager.tryGetInfo(structure, out x, out y, out index, out structureRegion);
            StructureData structuretargeted = structureRegion.structures[index];
           // if (structuretargeted.structure.health < num)
           // {
                //if (((structuretargeted.point -axishq).sqrMagnitude <= 3600) || ((structuretargeted.point - allieshq).sqrMagnitude <= 3600))
                Vector2 structurevector2;
                structurevector2.x = structuretargeted.point.x;
                structurevector2.y = structuretargeted.point.z;
                if (((structurevector2 - axishq).magnitude <= 600) || (structurevector2 - allieshq).magnitude <= 600)
                {
                    if ((structurevector2 - axishq).magnitude <= 600)
                    {
                        hq = " Axis ";

                    }
                    else if ((structurevector2 - allieshq).magnitude <= 600)
                    {
                        hq = " Allies ";
                    }
                    Rocket.Core.Logging.Logger.Log("Structure destroyed in HQ");
                    var player = UnturnedPlayer.FromCSteamID(steamid);
                    var steam64 = steamid;
                    var itemid = structuretargeted.structure.id;
                    ItemAsset itemAsset = (from i in new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>())
                        where i.itemName != null
                        orderby i.itemName.Length
                        where i.id == itemid
                        select i).FirstOrDefault<ItemAsset>();
                    //stole this from rockets /i command
                    var barricadename = itemAsset.itemName;
                    var url = player.SteamProfile.AvatarFull.ToString();
                    var bx = structuretargeted.point.x;
                    var by = structuretargeted.point.y;
                    var bz = structuretargeted.point.z;
                    //and then send to discord webhook


                    var owner = structuretargeted.owner;


                    Discord.SendWebhookPost(Configuration.Instance.raidalertchannel,
                        Discord.BuildDiscordEmbed("A structure was damaged in" + hq + "HQ",
                            "This structure was damaged at " + DateTime.Now,
                            player.DisplayName, url, 65327, new object[]
                            {
                                Discord.BuildDiscordField("Destroyer steam64", steam64.ToString(), true),
                                Discord.BuildDiscordField("Structure ID", itemid.ToString(), true),
                                Discord.BuildDiscordField("Structure Name", barricadename, true),
                                Discord.BuildDiscordField("Structure Position", "X: " + bx + " Y: " + by + " Z: " + bz,
                                    true),
                                Discord.BuildDiscordField("Owner of this structure", owner.ToString(),
                                    true),

                            }));


                }

            //}
        }


        public void OnDamageBarricade(CSteamID steamid, Transform barricade, ref ushort num2, ref bool barricadebool,
            EDamageOrigin damageorigin)
        {
            //Logdamage(steamid, barricade,num2);
            byte x;
            byte y;
            ushort num;
            ushort index;
            BarricadeRegion barricadeRegion;
            string hq;
            hq = " ";
            UnturnedPlayer player;
            CSteamID steam64;
            ushort itemid;
            float bx;
            float by;
            float bz;
            ulong owner;
            string url;
            //initialize variables in case exception happens which it will and data is missing.
            bx = 0;
            by = 0;
            bz = 0;
            url = "";
            itemid = 0;
            owner = 0;
            player = null;
            steam64.m_SteamID = 0;



            try

            
            {
                BarricadeManager.tryGetInfo(barricade, out x, out y, out num, out index, out barricadeRegion);
                BarricadeData barricadetargeted = barricadeRegion.barricades[index];
                //if (barricadetargeted.barricade.health < num2)
                //{
                //Rocket.Core.Logging.Logger.Log("Sqr Distance to Axis HQ: " + (barricadetargeted.point - axishq).sqrMagnitude);
                //Rocket.Core.Logging.Logger.Log("Sqr Distance to Allies HQ: "+ (barricadetargeted.point - allieshq).sqrMagnitude);
                /*if (((barricadetargeted.point - axishq).sqrMagnitude <= 3600) ||
                    ((barricadetargeted.point - allieshq).sqrMagnitude <= 3600))
                */
                Vector2 barricadevector2;
                barricadevector2.x = barricadetargeted.point.x;
                barricadevector2.y = barricadetargeted.point.z;
                if (((barricadevector2 - axishq).magnitude <= 600) || (barricadevector2 - allieshq).magnitude <= 600)
                {
                    if ((barricadevector2 - axishq).magnitude <= 600)
                    {
                        hq = " Axis ";

                    }
                    else if ((barricadevector2 - allieshq).magnitude <= 600)
                    {
                        hq = " Allies ";
                    }

                    Rocket.Core.Logging.Logger.Log("Barricade damaged in HQ: " + hq);

                    if (steamid.m_SteamID != 0)
                    {
                        player = UnturnedPlayer.FromCSteamID(steamid);
                    }
                    else
                    {
                        player = null;
                    }
                    
                    Rocket.Core.Logging.Logger.Log("destroying player found");
                    steam64 = steamid;
                    itemid = barricadetargeted.barricade.id;
                    Rocket.Core.Logging.Logger.Log("Barricade ID found");
                    ItemAsset itemAsset = (from i in new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>())
                        where i.itemName != null
                        orderby i.itemName.Length
                        where i.id == itemid
                        select i).FirstOrDefault<ItemAsset>();
                    //stole this from rockets /i command
                    var barricadename = itemAsset.itemName;
                    Rocket.Core.Logging.Logger.Log("barricade name found");
                    

                    bx = barricadetargeted.point.x;
                    by = barricadetargeted.point.y;
                    bz = barricadetargeted.point.z;
                    Rocket.Core.Logging.Logger.Log("barricade location found");

                    //and then send to discord webhook


                    owner = barricadetargeted.owner;
                    Rocket.Core.Logging.Logger.Log("barricade owner found");

                    if (player != null)
                    {
                        url = player.SteamProfile.AvatarFull.ToString();
                        Rocket.Core.Logging.Logger.Log("steam profile avatar found");
                    }
                    else
                    {
                        url = "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Question_mark_%28black%29.svg/200px-Question_mark_%28black%29.svg.png";
                        Rocket.Core.Logging.Logger.Log("no player - url not done");
                    }

                    

                    Discord.SendWebhookPost(Configuration.Instance.raidalertchannel,
                        Discord.BuildDiscordEmbed("A barricade was damaged in" + hq + "HQ",
                            "This barricade was damagedat " + DateTime.Now,
                            player.DisplayName, url, 16711680, new object[]
                            {
                                Discord.BuildDiscordField("Destroyer steam64", steam64.ToString(), true),
                                Discord.BuildDiscordField("Barricade ID", itemid.ToString(), true),
                                Discord.BuildDiscordField("Barricade Name", barricadename, true),
                                Discord.BuildDiscordField("Barricade Position", "X: " + bx + " Y: " + by + " Z: " + bz,
                                    true),
                                Discord.BuildDiscordField("Owner of this barricade", owner.ToString(),
                                    true),

                            }));
                }
                

            }
            catch (Exception e)
            {
                var error = e;
                Rocket.Core.Logging.Logger.Log("Exception caught: " + e);
                Discord.SendWebhookPost("https://ptb.discord.com/api/webhooks/807221467204550666/yte_hGdNflFqCtW80uhnNR1O9a0uX8GNoz5xGdur9xfLjUvRhs2sIctPypJocXdSVHRU",
                    Discord.BuildDiscordEmbed("Possible Exploit of infinite crops detected at" + hq + "HQ",
                        "Possible exploit detected at: " + DateTime.Now,
                        "Unknown Player", "https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Question_mark_%28black%29.svg/200px-Question_mark_%28black%29.svg.png" , 16711680, new object[]
                        {
                            Discord.BuildDiscordField("Crop ID", itemid.ToString(), true),
                            Discord.BuildDiscordField("Crop Position", "X: " + bx + " Y: " + by + " Z: " + bz,
                                true),
                            Discord.BuildDiscordField("Owner of this plant:", owner.ToString(),
                                true),

                        }));

            }
        }


















        public void OnLevelLoaded(int signature)
        {
            axishq.x = Configuration.Instance.axisx;
            axishq.y = Configuration.Instance.axisy;

            allieshq.x = Configuration.Instance.alliesx;
            allieshq.y = Configuration.Instance.alliesy;

            /* foreach (var node in LevelNodes.nodes)
             {
                 if (node.type != ENodeType.LOCATION) continue;
                 if (!((LocationNode)node).name.Contains("hq")) continue;
                 if (((LocationNode)node).name.Contains("Axis HQ"))
                 {
                     axishq = node.point;

                 }
                 if (((LocationNode)node).name.Contains("Allies HQ"))
                 {
                     allieshq = node.point;

                 }

             } */

            //AdvancedRegionsPlugin.Instance.RegionFlagManager.RegisterFlag<Logdamage>();
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
