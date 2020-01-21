using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ProjectileLimitPatch
{
    public class ProjectileConfig : ModConfig
    {
        [DefaultValue(2000)]
        [Range(1001,99999)]
        [Tooltip("You have to reload mods in order for the changes to this to take effect.\n" +
            "If you're going to try to play multiplayer the max ammount resets to the server's max.")]
        [ReloadRequired()]
        public int MaxProjectiles = 2000;

        public override ConfigScope Mode => ConfigScope.ServerSide;

        public override bool AcceptClientChanges(ModConfig pendingConfig, int whoAmI, ref string message)
        {
            return false;
        }
        public override bool NeedsReload(ModConfig pendingConfig)
        {
            return false;
        }
    }
}
