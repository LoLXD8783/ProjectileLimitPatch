using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ProjectileLimitPatch
{
    public static class Nett
    {
        public static void Send(ModPacket p, bool request, int data = 0)
        {
            p.Write(request);
            p.Write(data);
            p.Send();
        }
        public static void RequestProjectiles(ModPacket p)
        {
            p.Write(true);
            p.Send();
        }
        public static void Recieve(BinaryReader reader, ModPacket p, int fromwhomst)
        {
            if (reader.ReadBoolean())// if it's request
            {
                p.Write(false);
                p.Write(Main.projectile.Length);
                p.Send(fromwhomst);
            }
            else // if it's info
            {
                int nm;
                try
                {
                    nm = reader.ReadInt32();
                }
                catch // i hate this
                {
                    nm = 2000;
                }
                ProjectileLimitPatch.RestartLength(nm);
            }
        }
        #region 0.11.6.1
        //public static void Send(ModPacket p, bool request, int data = 0)
        //{
        //    p.Write(request);
        //    p.Write(data);
        //    p.Send();
        //    // Console.WriteLine("sent packet");
        //}
        //public static void Recieve(BinaryReader reader, ModPacket p)
        //{
        //    if(reader.ReadBoolean())
        //    {
        //        p.Write(false);
        //        p.Write(Main.projectile.Length);
        //        p.Send(reader.ReadInt32());
        //        // Console.WriteLine($"Sent packet to: {reader.ReadInt16()}");
        //    }
        //    else
        //    {
        //        ProjectileLimitPatch.ResetProjectileLength(reader.ReadInt32());
        //    }
        //}
        #endregion
    }
}
