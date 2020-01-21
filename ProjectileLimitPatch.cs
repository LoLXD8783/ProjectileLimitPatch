using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
//using MonoMod.Cil;
//using static Mono.Cecil.Cil.OpCodes;
using System.IO;

namespace ProjectileLimitPatch
{
    public class ProjectileLimitPatch : Mod
    {
        // Tmodloader 0.11.6.2
        // ResetProjectileLength 
        // projectilesBeforeJoining
        // TODO MessageBuffer
        // TODO Modloader cleanup mod references or something.
        // TODO Flip the goddamn if statements in the patches.
        public int ProjectilesPreJoin;
        public static void RestartLength(int length)
        {
            Main.projectile = new Projectile[length];
            Main.projectileIdentity = new int[Main.player.Length, Main.projectile.Length];
            for(int i = 0, nm = Main.projectile.Length; i < nm; i++)
                Main.projectile[i] = new Projectile();
        }
        public override void PreSaveAndQuit()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) RestartLength(ProjectilesPreJoin);
            base.PreSaveAndQuit();
        }
        public override void Load()
        {
            base.Load();
            int? expcet = ModContent.GetInstance<ProjectileConfig>()?.MaxProjectiles;
            RestartLength(expcet != null && expcet > 1000 ? (int)expcet : 2000);
            Patching.ApplyPatchesToEverything();
        }
        public override void Unload()
        {
            RestartLength(1001);
            Unpatching.UnapplyPatchesToEverything();
            base.Unload();
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Nett.Recieve(reader, GetPacket(), whoAmI);
        }
        public ProjectileLimitPatch() { }
    }

    #region 0.11.6.1
    // 0.11.6.1 code 
    //public class ProjectileLimitPatch : Mod
    //{
    //       internal int projectilesBeforeJoining;  
    //       private struct BitsInt
    //       {
    //           private int number;
    //           public bool this[int index]
    //           {
    //               get
    //               {
    //                   if (index > 32) throw new IndexOutOfRangeException();
    //                   return (number & (1 << index)) == (1 << index);
    //               }
    //               set
    //               {
    //                   if (index > 32) throw new IndexOutOfRangeException();
    //                   if (value) number |= 1 << index;
    //                   else number &= int.MaxValue - (1 << index);
    //               }
    //           }
    //       }
    //	public ProjectileLimitPatch()
    //	{
    //	}
    //       public static void ResetProjectileLength(int newlength)
    //       {
    //           if (newlength < 1000) return;
    //           Main.projectile = new Projectile[newlength];
    //           for (long i = 0, maxp = Main.projectile.Length; i < maxp; i++) Main.projectile[i] = new Projectile();

    //           Main.projectileIdentity = new int[Main.player.Length, Main.projectile.Length];
    //           // ModContent.GetInstance<ProjectileLimitPatch>().maxprojectiles2 = Main.projectile.Length;
    //       }

    //       BitsInt h1 = new BitsInt();
    //       BitsInt h2 = new BitsInt();
    //       public override void PreSaveAndQuit()
    //       {
    //           if(Main.netMode == NetmodeID.MultiplayerClient)
    //           {
    //               ResetProjectileLength(projectilesBeforeJoining);
    //           }
    //       }
    //       public override void HandlePacket(BinaryReader reader, int whoAmI) => Nett.Recieve(reader, GetPacket());
    //       public override void Load()
    //       {
    //           base.Load();
    //           // talbez deba cambiar la wea q cuenta por otra variable; estatica, o q c baya a crear con la longitud del array y se usa esa.
    //           // talves acer algo que tome + - como parametro o algo para editar
    //           // xacer editar el 'hook' del tmodloader ModContent.CleanupModReferences para que pase por todos los projectiles y no solo 1001.


    //           // if (Main.projectile.Length != 1000) throw new Exception("If you have a mod that changes projectile length there could be issues with this mod");

    //           int? estimated = ModContent.GetInstance<ProjectileConfig>()?.MaxProjectiles;

    //           int NewMaxProjectileAmmount = (estimated == null || estimated < 1000 ? 2000 : (int)estimated);

    //           Main.projectile = new Projectile[NewMaxProjectileAmmount + 1]; // +1 cause elegance
    //           for (long i = 0, maxp = Main.projectile.Length; i < maxp; i++) Main.projectile[i] = new Projectile();

    //           Main.projectileIdentity = new int[Main.player.Length, Main.projectile.Length]; // Yeah, idk.

    //           byte position = 0; // the array accesses are for verifying which patches were applied

    //           IL.Terraria.Projectile.Damage += Skip1Replace1;                                                                          h1[position++] = true;// - +
    //           /*Importnt owo*/IL.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float += Replace3;  h1[position++] = true;// + + + // idk, it could also be * + * too
    //           IL.Terraria.Projectile.AI_026 += AI_26Hook;                                                                              h1[position++] = true;// - - +
    //           IL.Terraria.Projectile.AI_062 += Replace1;                                                                               h1[position++] = true;// +
    //           IL.Terraria.Projectile.AI_099_1 += Replace1;                                                                             h1[position++] = true;// +
    //           IL.Terraria.Projectile.Kill += Replace5;                                                                                 h1[position++] = true;// + + + + +
    //           IL.Terraria.Projectile.GetNextSlot += Replace2;                                                                          h1[position++] = true;// +? +
    //           IL.Terraria.Projectile.GetByUUID_int_int += Replace1;                                                                    h1[position++] = true;// +?
    //           IL.Terraria.Projectile.ProjectileFixDesperation += Replace2;                                                             h1[position++] = true;// +? +

    //           IL.Terraria.Player.OnHit += Replace2;                                                                                    h1[position++] = true;// /*+ +*/
    //           IL.Terraria.Player.UpdateBuffs += Replace3;                                                                              h1[position++] = true;// /*+ + +*/
    //           IL.Terraria.Player.Counterweight += Replace1;                                                                            h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.GrappleMovement += Replace2;                                                                          h1[position++] = true;// /*+ +*/
    //           IL.Terraria.Player.TileInteractionsUse += Replace1;                                                                      h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.LaunchMinecartHook += Replace1;                                                                       h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.ItemCheck += ItemCheckHook;                                                                           h1[position++] = true;// /*+ + + + + + + +|32551 + - -|32870 + + + +|32899 - + + + +|37396 // jesus christ*/
    //           IL.Terraria.Player.MoonLeechRope += Replace1;                                                                            h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.QuickGrapple += QuickGrappleHook;                                                                     h1[position++] = true;// /*+ + + + + + + + + + +*/
    //           IL.Terraria.Player.WouldSpotOverlapWithSentry += Replace1;                                                               h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.UpdateForbiddenSetLock += Replace1;                                                                   h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.CommandForbiddenStorm += Replace1;                                                                    h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.WOFTongue += Replace1;                                                                                h1[position++] = true;// /* + - -*/
    //           IL.Terraria.Player.Teleport += Replace1;                                                                                 h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.SporeSac += Replace2;                                                                                 h1[position++] = true;// /*+ +*/
    //           IL.Terraria.Player.WipeOldestTurret += Replace1;                                                                         h1[position++] = true;// /*+*/
    //           IL.Terraria.Player.UpdateMaxTurrets += Replace2;                                                                         h1[position++] = true;// /*+ +?*/

    //           IL.Terraria.NPC.ReflectProjectiles += Replace1;                                                                          h1[position++] = true;// /*+*/
    //           IL.Terraria.NPC.SpawnOnPlayer += Replace2;                                                                               h1[position++] = true;// + +
    //           IL.Terraria.NPC.AI_007_TownEntities += Replace1; h1[position++] = true;// +
    //           IL.Terraria.NPC.UpdateNPC_BuffApplyDOTs += Replace3; h1[position++] = true;// + + +

    //           /*Importnt owo*/
    //           IL.Terraria.Main.DrawProjectiles += Replace2; h1[position++] = true;// + +
    //           IL.Terraria.Main.CacheProjDraws += Replace1; h1[position++] = true;// +
    //           IL.Terraria.Main.Initialize += InitializeHook; h1[position++] = true; position = 0;// +
    //           IL.Terraria.Main.DrawPlayer += Skip1Replace1; h2[position++] = true; // - + // here /*------------------------------------------------------------------------------*/
    //           IL.Terraria.Main.DoUpdate += DoUpdateHook; h2[position++] = true;// -? -? - -|12368 +
    //           IL.Terraria.Main.DrawInfoAccs += Replace1; h2[position++] = true;// +

    //           IL.Terraria.MessageBuffer.GetData += GetDataHook1; h2[position++] = true;// + - + + + + +|1585 - - -|2883 +|3135
    //           // + -? + + + + + - - - +

    //           IL.Terraria.WorldGen.TileFrame += Replace1; h2[position++] = true;// +
    //           IL.Terraria.WorldGen.clearWorld += Replace1; h2[position++] = true;// +

    //           IL.Terraria.Wiring.HitWireSingle += Replace3; h2[position++] = true;// + + +

    //           IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints += Replace1; h2[position++] = true;// +
    //           IL.Terraria.GameContent.PortalHelper.RemoveMyOldPortal += Replace1; h2[position++] = true;// +
    //           IL.Terraria.GameContent.PortalHelper.RemoveIntersectingPortals += Replace1; h2[position++] = true;// +
    //           IL.Terraria.GameContent.PortalHelper.SyncPortalsOnPlayerJoin += Replace1; h2[position++] = true;// +

    //           IL.Terraria.GameContent.Events.DD2Event.ClearAllTowersInGame += Replace1; h2[position++] = true;// +

    //           IL.Terraria.GameContent.Shaders.WaterShaderData.DrawWaves += Replace1; h2[position++] = true;// +
    //       }
    //       public override void Unload()
    //       {
    //           base.Unload();
    //           Main.projectile = new Projectile[1001]; // cause that is the original length for what i found in 0.11.6.1 :p

    //           for (int i = 0, maxp = Main.projectile.Length; i < maxp; i++) Main.projectile[i] = new Projectile();

    //           Main.projectileIdentity = new int[Main.player.Length, Main.projectile.Length];

    //           int position = 0;
    //           try
    //           {
    //               if (h1[position++])
    //               IL.Terraria.Projectile.Damage -= Skip1Replace1; if (h1[position++])
    //               IL.Terraria.Projectile.NewProjectile_float_float_float_float_int_int_float_int_float_float -= Replace3; if (h1[position++])
    //               IL.Terraria.Projectile.AI_026 -= AI_26Hook; if (h1[position++])
    //               IL.Terraria.Projectile.AI_062 -= Replace1; if (h1[position++])
    //               IL.Terraria.Projectile.AI_099_1 -= Replace1; if (h1[position++])
    //               IL.Terraria.Projectile.Kill -= Replace5; if (h1[position++])
    //               IL.Terraria.Projectile.GetNextSlot -= Replace2; if (h1[position++])
    //               IL.Terraria.Projectile.GetByUUID_int_int -= Replace1; if (h1[position++])
    //               IL.Terraria.Projectile.ProjectileFixDesperation -= Replace2; if (h1[position++])
    //               IL.Terraria.Player.OnHit -= Replace2; if (h1[position++])
    //               IL.Terraria.Player.UpdateBuffs -= Replace3; if (h1[position++])
    //               IL.Terraria.Player.Counterweight -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.GrappleMovement -= Replace2; if (h1[position++])
    //               IL.Terraria.Player.TileInteractionsUse -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.LaunchMinecartHook -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.ItemCheck -= ItemCheckHook; if (h1[position++])
    //               IL.Terraria.Player.MoonLeechRope -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.QuickGrapple -= QuickGrappleHook; if (h1[position++])
    //               IL.Terraria.Player.WouldSpotOverlapWithSentry -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.UpdateForbiddenSetLock -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.CommandForbiddenStorm -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.WOFTongue -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.Teleport -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.SporeSac -= Replace2; if (h1[position++])
    //               IL.Terraria.Player.WipeOldestTurret -= Replace1; if (h1[position++])
    //               IL.Terraria.Player.UpdateMaxTurrets -= Replace2; if (h1[position++])
    //               IL.Terraria.NPC.ReflectProjectiles -= Replace1; if (h1[position++])
    //               IL.Terraria.NPC.SpawnOnPlayer -= Replace2; if (h1[position++])
    //               IL.Terraria.NPC.AI_007_TownEntities -= Replace1; if (h1[position++])
    //               IL.Terraria.NPC.UpdateNPC_BuffApplyDOTs -= Replace3; if (h1[position++])
    //               IL.Terraria.Main.DrawProjectiles -= Replace2; if (h1[position++])
    //               IL.Terraria.Main.CacheProjDraws -= Replace1; if (h1[position++])
    //               IL.Terraria.Main.Initialize -= InitializeHook; position = 0; if (h2[position++]) // here
    //               IL.Terraria.Main.DrawPlayer -= Skip1Replace1; if (h2[position++])
    //               IL.Terraria.Main.DoUpdate -= DoUpdateHook; if (h2[position++])
    //               IL.Terraria.Main.DrawInfoAccs -= Replace1; if (h2[position++])
    //               IL.Terraria.MessageBuffer.GetData -= GetDataHook1; if (h2[position++])
    //               IL.Terraria.WorldGen.TileFrame -= Replace1; if (h2[position++])
    //               IL.Terraria.WorldGen.clearWorld -= Replace1; if (h2[position++])
    //               IL.Terraria.Wiring.HitWireSingle -= Replace3; if (h2[position++])
    //               IL.Terraria.GameContent.PortalHelper.UpdatePortalPoints -= Replace1; if (h2[position++])
    //               IL.Terraria.GameContent.PortalHelper.RemoveMyOldPortal -= Replace1; if (h2[position++])
    //               IL.Terraria.GameContent.PortalHelper.RemoveIntersectingPortals -= Replace1; if (h2[position++])
    //               IL.Terraria.GameContent.PortalHelper.SyncPortalsOnPlayerJoin -= Replace1; if (h2[position++])
    //               IL.Terraria.GameContent.Events.DD2Event.ClearAllTowersInGame -= Replace1; if (h2[position++])
    //               IL.Terraria.GameContent.Shaders.WaterShaderData.DrawWaves -= Replace1;
    //           }
    //           catch // Just in case
    //           {

    //           }
    //           for (int i = 0; i < 32; i++) h1[i] = h2[i] = false;
    //       }
    //       #region hooks

    //       private void GetDataHook1(ILContext il) // TODO it's found out what's wrong
    //       {
    //           //  + -? + + + + + - - - +
    //           // ILCursor c = new ILCursor(il);
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           //if (!Next(c)) return; // -?
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 1
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 2
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 3
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 4
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 5
    //           //if (!Next(c)) return; // -
    //           //if (!Next(c)) return; // -
    //           //if (!Next(c)) return; // -
    //           //if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }
    //       private void DoUpdateHook(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; // -?
    //           if (!Next(c)) return; // -?
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }
    //       /// <summary>
    //       /// kinda useless
    //       /// </summary>
    //       /// <param name="il"></param>
    //       private void InitializeHook(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c, 1001)) return; ReplaceCurrentWithLength(c); // + // wat
    //       }

    //       private void Replace5(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }

    //       private void AI_26Hook(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }

    //       private void QuickGrappleHook(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 1
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 2
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 3
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 4
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 5
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 6
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 7
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 8
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 9
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 10
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 11
    //       }


    //       private void ItemCheckHook(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           // for (int n = 0; n < 9; n++) { if (!Next(c)) return; ReplaceCurrentWithLength(c); } // + + + + + + + + +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 1
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 2
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 3 
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 4
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 5
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 6
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 7 
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 8
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // + 9
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; // -
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }

    //       private void Replace3(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //           if (!Next(c)) return; ReplaceCurrentWithLength(c); // +
    //       }

    //       private void Replace1(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return;
    //           ReplaceCurrentWithLength(c);
    //       }

    //       private void Replace2(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!Next(c)) return; // +
    //           ReplaceCurrentWithLength(c);
    //           if (!Next(c)) return; // +
    //           ReplaceCurrentWithLength(c);

    //       }

    //       private void Skip1Replace1(ILContext il)
    //       {
    //           ILCursor c = new ILCursor(il);
    //           if (!c.TryGotoNext(i => i.MatchLdcI4(1000))) return; // -
    //           if (!c.TryGotoNext(i => i.MatchLdcI4(1000))) return; // +
    //           ReplaceCurrentWithLength(c);
    //       }
    //       #endregion

    //       #region hooks2
    //       private static bool TryReplaceNext(ILCursor c, int value = 1000)
    //       {
    //           if (!Next(c, value)) return false;
    //           ReplaceCurrentWithLength(c); return true;
    //       }

    //       private static bool Next(ILCursor c, int value = 1000) => c.TryGotoNext(i => i.MatchLdcI4(value)); // if things don't work like this i'll have to pass ref, else i'll just put the code directly.

    //       private static void ReplaceCurrentWithLength(ILCursor c)
    //       {
    //           c.Remove();
    //           c.Emit(Ldsfld, typeof(Main).GetField(nameof(Main.projectile)));
    //           c.Emit(Ldlen);
    //           c.Emit(Conv_I4);
    //       }
    //       #endregion
    //   }
    #endregion
}