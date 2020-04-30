using Rocket.API;
using System;
using System.Collections.Generic;

namespace Random.miscstuff
{
	public class miscstuffConfiguration : IRocketPluginConfiguration
	{

        public float artyshipoffsetlistx;
        public float artyshipoffsetlisty;
        public float artyshipoffsetlistz;

        public int ZombieXP;
        public int MegaXP;
        public bool Siphon;        
        public List<Registereddoortype> listofregistereddoors;
        public void LoadDefaults()  
        {
            listofregistereddoors = new List<Registereddoortype>
            { };
            ZombieXP = 0;
            MegaXP = -1000;
            Siphon = false;
            artyshipoffsetlistx = 0f;
            artyshipoffsetlisty = 15f;
            artyshipoffsetlistz = 0.84f;
        }
	}
}
