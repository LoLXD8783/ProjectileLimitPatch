using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using MonoMod.Cil;
using static Mono.Cecil.Cil.OpCodes;

using static ProjectileLimitPatch.PatchHooks;

namespace ProjectileLimitPatch
{
    internal static class Patching
    {
        public static void ApplyPatchesToEverything()
        {
            DD2Event();
            Main();
            MessageBuffer();
            NPC();
            PortalHelper();
            Projectile();
            WaterShadderData();
            Wiring();
            WorldGen();
        }
        public static void DD2Event()
        {
            IL.Terraria.GameContent.Events.DD2Event.ClearAllTowersInGame += Replace1; /*+*/
        }
        public static void Main()
        {
            IL.Terraria.Main.CacheProjDraws += Replace1; /*+*/
            IL.Terraria.Main.DoUpdate += DoUpdateHook; /*- - - - +*/
            IL.Terraria.Main.DrawInfoAccs += Replace1; /*+ ¿?*/
            IL.Terraria.Main.DrawPlayer += Skip1Replace1; /*- +*/
            IL.Terraria.Main.DrawProj += Skip2Replace1; /*- - +*/
            IL.Terraria.Main.DrawProjectiles += Replace2; /*+ +*/
            IL.Terraria.Main.Initialize += InitializeHook; /*+?*/
        }
        public static void MessageBuffer() // TODO? at least until i find what's wrong.
        {
            // GetData
        }
        // mount is clean i guess
        // net message -> send data looks also clean
        public static void NPC()
        {
            IL.Terraria.NPC.AI_007_TownEntities += Replace1; /*+*/
            IL.Terraria.NPC.ReflectProjectiles += Replace1; /*+*/
            IL.Terraria.NPC.SpawnOnPlayer += Replace1; /*+*/
            IL.Terraria.NPC.UpdateNPC_BuffApplyDOTs += Replace3; /*+ + +*/
            IL.Terraria.NPC.VanillaAI += VanillaAIHook; /* - - - - - - - - - -| - - - - - - - -| - -| + + + + + + + + - - - -| -| + */ // (20 * -) (8 * +) (5 * -) + // jesus christ
        }
        public static void Player()
        {
            IL.Terraria.Player.CommandForbiddenStorm += Replace1; /*+ */
            IL.Terraria.Player.Counterweight += Replace1; /*+ */
            IL.Terraria.Player.GrappleMovement += Replace2; /*+ +*/
            IL.Terraria.Player.ItemCheck += ItemCheckHook; /*+ + + + + + + + + - - + + + + - + + + +*/ // (9 * +) (2 * -) (4 * +) - (4 * +)
            IL.Terraria.Player.LaunchMinecartHook += Replace1; /*+ */
            IL.Terraria.Player.MoonLeechRope += Replace1; /*+ */
            IL.Terraria.Player.OnHit += Replace2; /*+ +*/
            IL.Terraria.Player.QuickGrapple += QuickGrappleHook; /* + + + + + + + + + + +*/
            IL.Terraria.Player.SporeSac += Replace2; /*+ +*/
            IL.Terraria.Player.Teleport += Replace1; /*+ */
            IL.Terraria.Player.TileInteractionsUse += Replace1; /*+ */
            IL.Terraria.Player.UpdateBuffs += Replace3; /*+ + +*/
            IL.Terraria.Player.UpdateForbiddenSetLock += Replace1; /*+ */
            IL.Terraria.Player.UpdateMaxTurrets += Replace2; /*+ +?*/
            IL.Terraria.Player.WipeOldestTurret += Replace1; /*+ */
            IL.Terraria.Player.WOFTongue += Replace1; /*+ ¿-? ¿-?*/
            IL.Terraria.Player.WouldSpotOverlapWithSentry += Replace1; /*+ */
        }
        public static void PortalHelper()
        {
            IL.Terraria.GameContent.PortalHelper.RemoveIntersectingPortals += Replace1; /*+ */
            IL.Terraria.GameContent.PortalHelper.RemoveMyOldPortal += Replace1; /*+ */
            IL.Terraria.GameContent.PortalHelper.SyncPortalsOnPlayerJoin += Replace1; /*+ */
            IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints += Replace1; /*+ */
        }
        public static void Projectile()
        {
            IL.Terraria.Projectile.AI_026 += Skip2Replace1; /*- - +*/
            IL.Terraria.Projectile.AI_062 += Replace1; /*+ -*/
            IL.Terraria.Projectile.AI_099_1 += Replace1; /*+*/
            IL.Terraria.Projectile.Damage += Skip1Replace1; /*- +*/
            IL.Terraria.Projectile.GetByUUID_int_int += Replace1; /*+ */
            IL.Terraria.Projectile.GetNextSlot += Replace2; /*+? +*/
            IL.Terraria.Projectile.Kill += Replace5; /*+ + + + +*/
            IL.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Replace3; /*+? + +?*/
            IL.Terraria.Projectile.ProjectileFixDesperation += Skip1Replace1; /*-? +*/
            IL.Terraria.Projectile.VanillaAI += pVanillaAIHook; /*+ + - - - -| - - - - + + - + - + + - + - - - + + +? +? - - - - - -*/
        }
        public static void WaterShadderData()
        {
            IL.Terraria.GameContent.Shaders.WaterShaderData.DrawWaves += Replace1; /*+ */
        }
        public static void Wiring()
        {
            IL.Terraria.Wiring.HitWireSingle += Replace3; /*+ */
        }
        public static void WorldGen()
        {
            IL.Terraria.WorldGen.clearWorld += Replace1; /*+ -? -?*/
            IL.Terraria.WorldGen.TileFrame += Replace1; /*+ */
        }
    }
    internal static class Unpatching
    {
        public static void UnapplyPatchesToEverything()
        {
            uDD2Event();
            uMessageBuffer();
            uNPC();
            uPlayer();
            uPortalHelper();
            uProjectile();
            uWaterShadderData();
            uWiring();
            uWorldGen();
        }
        public static void uDD2Event()
        {
            IL.Terraria.GameContent.Events.DD2Event.ClearAllTowersInGame -= Replace1;
        }
        public static void uMain()
        {
            IL.Terraria.Main.CacheProjDraws -= Replace1;
            IL.Terraria.Main.DoUpdate -= DoUpdateHook;
            IL.Terraria.Main.DrawInfoAccs -= Replace1;
            IL.Terraria.Main.DrawPlayer -= Skip1Replace1;
            IL.Terraria.Main.DrawProj -= Skip2Replace1;
            IL.Terraria.Main.DrawProjectiles += Replace2;
            IL.Terraria.Main.Initialize -= InitializeHook;
        }
        public static void uMessageBuffer() // TODO
        {

        }
        public static void uNPC()
        {
            IL.Terraria.NPC.AI_007_TownEntities -= Replace1;
            IL.Terraria.NPC.ReflectProjectiles -= Replace1;
            IL.Terraria.NPC.SpawnOnPlayer -= Replace1;
            IL.Terraria.NPC.UpdateNPC_BuffApplyDOTs -= Replace3;
            IL.Terraria.NPC.VanillaAI -= VanillaAIHook;
        }
        public static void uPlayer()
        {
            IL.Terraria.Player.CommandForbiddenStorm -= Replace1;
            IL.Terraria.Player.Counterweight -= Replace1;
            IL.Terraria.Player.GrappleMovement -= Replace2;
            IL.Terraria.Player.ItemCheck -= ItemCheckHook;
            IL.Terraria.Player.LaunchMinecartHook -= Replace1;
            IL.Terraria.Player.MoonLeechRope -= Replace1;
            IL.Terraria.Player.OnHit -= Replace2;
            IL.Terraria.Player.QuickGrapple -= QuickGrappleHook;
            IL.Terraria.Player.SporeSac -= Replace2;
            IL.Terraria.Player.Teleport -= Replace1;
            IL.Terraria.Player.TileInteractionsUse -= Replace1;
            IL.Terraria.Player.UpdateBuffs -= Replace3;
            IL.Terraria.Player.UpdateMaxTurrets -= Replace1;
            IL.Terraria.Player.WipeOldestTurret -= Replace1;
            IL.Terraria.Player.WOFTongue -= Replace1;
            IL.Terraria.Player.WouldSpotOverlapWithSentry -= Replace1;
        }
        public static void uPortalHelper()
        {
            IL.Terraria.GameContent.PortalHelper.RemoveIntersectingPortals += Replace1;
            IL.Terraria.GameContent.PortalHelper.RemoveMyOldPortal -= Replace1;
            IL.Terraria.GameContent.PortalHelper.SyncPortalsOnPlayerJoin -= Replace1;
            IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints -= Replace1;
        }
        public static void uProjectile()
        {
            IL.Terraria.Projectile.AI_026 -= Skip2Replace1;
            IL.Terraria.Projectile.AI_062 -= Replace1;
            IL.Terraria.Projectile.AI_099_1 -= Replace1;
            IL.Terraria.Projectile.Damage -= Skip1Replace1;
            IL.Terraria.Projectile.GetByUUID_int_int -= Replace1;
            IL.Terraria.Projectile.GetNextSlot -= Replace2;
            IL.Terraria.Projectile.Kill -= Replace5;
            IL.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Replace3;
            IL.Terraria.Projectile.ProjectileFixDesperation -= Skip1Replace1;
            IL.Terraria.Projectile.VanillaAI -= pVanillaAIHook; 
        }
        public static void uWaterShadderData()
        {
            IL.Terraria.GameContent.Shaders.WaterShaderData.DrawWaves -= Replace1; 
        }
        public static void uWiring()
        {
            IL.Terraria.Wiring.HitWireSingle -= Replace3; 
        }
        public static void uWorldGen()
        {
            IL.Terraria.WorldGen.clearWorld -= Replace1;
            IL.Terraria.WorldGen.TileFrame -= Replace1; 
        }
    }
    internal static partial class PatchHooks
    {
        internal static bool Next(ILCursor c, int value = 1000) => c.TryGotoNext(i=>i.MatchLdcI4(value));
        internal static void ReplaceWLength(ILCursor c)
        {
            c.Remove();
            c.Emit(Ldsfld, typeof(Main).GetField(nameof(Main.projectile)));
            c.Emit(Ldlen);
            c.Emit(Conv_I4);
        }
        internal static void Replace1(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void Replace2(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void Replace3(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void Replace5(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void Skip1Replace1(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return;
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void Skip2Replace1(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return;
            if (!Next(c)) return;
            if (!Next(c)) return; ReplaceWLength(c);
        }
        internal static void DoUpdateHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // +
        }
        internal static void InitializeHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c, 1001)) return; ReplaceWLength(c);
        }
        internal static void VanillaAIHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; // - 1
            if (!Next(c)) return; // - 2 
            if (!Next(c)) return; // - 3
            if (!Next(c)) return; // - 4
            if (!Next(c)) return; // - 5
            if (!Next(c)) return; // - 6
            if (!Next(c)) return; // - 7
            if (!Next(c)) return; // - 8
            if (!Next(c)) return; // - 9
            if (!Next(c)) return; // - 10
            if (!Next(c)) return; // - 11
            if (!Next(c)) return; // - 12
            if (!Next(c)) return; // - 13
            if (!Next(c)) return; // - 14
            if (!Next(c)) return; // - 15
            if (!Next(c)) return; // - 16
            if (!Next(c)) return; // - 17
            if (!Next(c)) return; // - 18
            if (!Next(c)) return; // - 19
            if (!Next(c)) return; // - 20
            if (!Next(c)) return; ReplaceWLength(c); // + 1
            if (!Next(c)) return; ReplaceWLength(c); // + 2
            if (!Next(c)) return; ReplaceWLength(c); // + 3
            if (!Next(c)) return; ReplaceWLength(c); // + 4
            if (!Next(c)) return; ReplaceWLength(c); // + 5
            if (!Next(c)) return; ReplaceWLength(c); // + 6
            if (!Next(c)) return; ReplaceWLength(c); // + 7
            if (!Next(c)) return; ReplaceWLength(c); // + 8
            if (!Next(c)) return; // - 1
            if (!Next(c)) return; // - 2 
            if (!Next(c)) return; // - 3
            if (!Next(c)) return; // - 4
            if (!Next(c)) return; // - 5
            if (!Next(c)) return; ReplaceWLength(c); // +
        }
        internal static void ItemCheckHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c); // + 1
            if (!Next(c)) return; ReplaceWLength(c); // + 2
            if (!Next(c)) return; ReplaceWLength(c); // + 3
            if (!Next(c)) return; ReplaceWLength(c); // + 4
            if (!Next(c)) return; ReplaceWLength(c); // + 5
            if (!Next(c)) return; ReplaceWLength(c); // + 6
            if (!Next(c)) return; ReplaceWLength(c); // + 7
            if (!Next(c)) return; ReplaceWLength(c); // + 8
            if (!Next(c)) return; ReplaceWLength(c); // + 9
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // + 1
            if (!Next(c)) return; ReplaceWLength(c); // + 2
            if (!Next(c)) return; ReplaceWLength(c); // + 3
            if (!Next(c)) return; ReplaceWLength(c); // + 4
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // + 1
            if (!Next(c)) return; ReplaceWLength(c); // + 2
            if (!Next(c)) return; ReplaceWLength(c); // + 3
            if (!Next(c)) return; ReplaceWLength(c); // + 4
        }
        internal static void QuickGrappleHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c); // + 1
            if (!Next(c)) return; ReplaceWLength(c); // + 2
            if (!Next(c)) return; ReplaceWLength(c); // + 3
            if (!Next(c)) return; ReplaceWLength(c); // + 4
            if (!Next(c)) return; ReplaceWLength(c); // + 5
            if (!Next(c)) return; ReplaceWLength(c); // + 6
            if (!Next(c)) return; ReplaceWLength(c); // + 7
            if (!Next(c)) return; ReplaceWLength(c); // + 8
            if (!Next(c)) return; ReplaceWLength(c); // + 9
            if (!Next(c)) return; ReplaceWLength(c); // + 10
            if (!Next(c)) return; ReplaceWLength(c); // + 11
        }
        internal static void pVanillaAIHook(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; // - 1
            if (!Next(c)) return; // - 2
            if (!Next(c)) return; // - 3
            if (!Next(c)) return; // - 4
            if (!Next(c)) return; // - 5
            if (!Next(c)) return; // - 6
            if (!Next(c)) return; // - 7
            if (!Next(c)) return; // - 8
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; // -
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
            if (!Next(c)) return; ReplaceWLength(c); // +
        }
    }
}
