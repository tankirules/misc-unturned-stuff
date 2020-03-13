using Rocket.API;

namespace Random.miscstuff
{
	public class miscstuffConfiguration : IRocketPluginConfiguration
	{

        public int ZombieXP;
        public int MegaXP;
        public bool Siphon;
		public void LoadDefaults()
		{
            ZombieXP = 0;
            MegaXP = -1000;
            Siphon = false;
        }
	}
}
