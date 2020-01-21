using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectileLimitPatch
{
    public class PlayerC : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            if(Main.netMode != NetmodeID.SinglePlayer)
            {
                ModContent.GetInstance<ProjectileLimitPatch>().ProjectilesPreJoin = Main.projectile.Length;
                Nett.RequestProjectiles(mod.GetPacket());
            }
        }
        #region 0.11.6.1
        //public override void OnEnterWorld(Player player) 
        //{
        //    if (Main.netMode != NetmodeID.SinglePlayer)
        //    {
        //        ModContent.GetInstance<ProjectileLimitPatch>().projectilesBeforeJoining = Main.projectile.Length;
        //        Nett.Send(mod.GetPacket(), true, player.whoAmI);
        //    }
        //}
        #endregion
    }
}
