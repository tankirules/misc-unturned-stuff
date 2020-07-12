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
using ImperialPlugins.AdvancedRegions.RegionFlags;

namespace Random.miscstuff
{
    [FlagInfo("Log when Barricade is destroyed in this", SupportsGroupValues = false, SupportsEnvironmentGroupValue = false)]
    public sealed class MyFlag : RegionFlag<MyFlagOptions>
    {
        public void OnBarricadeDestroyed(Barricade barricade)
        {
            Rocket.Core.Logging.Logger.Log("Barricade destroyed in HQ");
            var itemid = barricade.id;
            ItemAsset itemAsset = (from i in new List<ItemAsset>(Assets.find(EAssetType.ITEM).Cast<ItemAsset>())
                where i.itemName != null
                orderby i.itemName.Length
                where i.id == itemid
                select i).FirstOrDefault<ItemAsset>();
            var barricadename = itemAsset.itemName;
            var barricades = miscstuff.GetBarricades(CSteamID.Nil, false);
            foreach (BarricadeData barricadedata in barricades)
            {
                if (barricadedata.barricade == barricade)
                {
                    var point = barricadedata.point;
                    break;
                }
            }

            Discord.SendWebhookPost(miscstuff.Instance.Configuration.Instance.hqalertchannel,
                Discord.BuildDiscordEmbed("A barricade was destroyed in HQ:", "This barricade was destroyed at " + DateTime.Now,
                    "Metrix WW2 Bot by Random", "https://b7.pngbarn.com/png/379/130/explosion-hot-sauce-explode-png-clip-art.png", 16711680, new object[]
                    {
                        Discord.BuildDiscordField("Barricade ID", itemid.ToString(), true),
                        Discord.BuildDiscordField("Barricade Name", barricadename, true),
                            }));

        }

        public void OnStructureDestroyed(Structure structure)
        {
           
            Rocket.Core.Logging.Logger.Log("Structure destroyed in HQ");
        }
    }
    [Serializable]
    public sealed class MyFlagOptions : ToggleableFlagOptions
    {
        [FlagOption(Description = "Useless?", Syntax = "idk")]
        public string MyCustomValue { get; set; }
    }
}
