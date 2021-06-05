using Rocket.API;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Random.miscstuff
{
	public class miscstuffConfiguration : IRocketPluginConfiguration
    {
        public float axisy;
        public float axisx;
        public float alliesy;
        public float alliesx;
        public string raidalertchannel;
        public string hqalertchannel;
        public float artyshipoffsetlistx;
        public float artyshipoffsetlisty;
        public float artyshipoffsetlistz;
        

        public int ZombieXP;
        public int MegaXP;
        public bool Siphon;        
        public List<Registereddoortype> listofregistereddoors;
        [XmlArrayItem("ItemId")]  public List<ushort> listofblacklistedspawners;
        [XmlArrayItem("ItemId")] public List<ushort> listofignoredexploitbarricades;
        
        public void LoadDefaults()
        {
            alliesy = 1003;
            alliesx = 1666;
            axisx = -1482;
            axisy = -1684;
            listofregistereddoors = new List<Registereddoortype>
            { };
            ZombieXP = 0;
            MegaXP = -1000;
            Siphon = false;
            artyshipoffsetlistx = 0f;
            artyshipoffsetlisty = 15f;
            artyshipoffsetlistz = 0.84f;
            raidalertchannel =
                "https://ptb.discordapp.com/api/webhooks/731832920544641045/L5vfn6rGJIAd3gzCulgASwshPmA1Jx5d4OYSDH4X6h3Mi4HykY8TE8lSsS-Gep65KsW-";
            hqalertchannel =
                "https://ptb.discordapp.com/api/webhooks/731855861013807174/nq19tM__yYiub57hJbvRdN8KXG9DhuJ1t358z0XnnTkgKQOkmVRBMN5JSoknSLP4jNV0";
            listofblacklistedspawners = new List<ushort>
                { 76 };
            listofignoredexploitbarricades = new List<ushort>
                { 386};
        }
	}
}
