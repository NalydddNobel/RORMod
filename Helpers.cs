﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RORMod.Content.Artifacts;
using RORMod.Content.Elites;
using RORMod.NPCs;
using RORMod.Projectiles;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod
{
    public static class Helpers
    {
        public static Color UseR(this Color color, int R) => new Color(R, color.G, color.B, color.A);
        public static Color UseR(this Color color, float R) => new Color((int)(R * 255), color.G, color.B, color.A);

        public static Color UseG(this Color color, int G) => new Color(color.R, G, color.B, color.A);
        public static Color UseG(this Color color, float G) => new Color(color.R, (int)(G * 255), color.B, color.A);

        public static Color UseB(this Color color, int B) => new Color(color.R, color.G, B, color.A);
        public static Color UseB(this Color color, float B) => new Color(color.R, color.G, (int)(B * 255), color.A);

        public static Color UseA(this Color color, int alpha) => new Color(color.R, color.G, color.B, alpha);
        public static Color UseA(this Color color, float alpha) => new Color(color.R, color.G, color.B, (int)(alpha * 255));

        public static string GetKeyName(ModKeybind keybind)
        {
            var s = keybind.GetAssignedKeys();
            if (s.Count == 0)
            {
                return Language.GetTextValue("Mods.RORMod.UnboundKey");
            }
            return s[0];
        }

        public static void GetItemDrawData(int item, out Rectangle frame)
        {
            frame = Main.itemAnimations[item] == null ? TextureAssets.Item[item].Value.Frame() : Main.itemAnimations[item].GetFrame(TextureAssets.Item[item].Value);
        }
        public static void GetItemDrawData(this Item item, out Rectangle frame)
        {
            GetItemDrawData(item.type, out frame);
        }

        public static bool IsElite(this NPC npc)
        {
            foreach (var e in RORNPC.RegisteredElites)
            {
                if (npc.GetGlobalNPC(e).Active)
                {
                    return true;
                }
            }
            return false;
        }
        public static void GetElitePrefixes(this NPC npc, out List<EliteNPC> prefixes)
        {
            prefixes = new List<EliteNPC>();
            foreach (var e in RORNPC.RegisteredElites)
            {
                var npcE = npc.GetGlobalNPC(e);
                if (npcE.Active)
                {
                    prefixes.Add(npcE);
                }
            }
        }

        public static void DrawRectangle(Rectangle rect, Color color)
        {
            Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, rect, color);
        }

        public static void DrawRectangle(Rectangle rect, Vector2 offset, Color color)
        {
            rect.X += (int)offset.X;
            rect.Y += (int)offset.Y;
            DrawRectangle(rect, color);
        }

        public static bool IsProbablyACritter(this NPC npc)
        {
            return NPCID.Sets.CountsAsCritter[npc.type] || (npc.lifeMax < 5 && npc.lifeMax != 1);
        }

        public static Rectangle Frame(this Projectile projectile)
        {
            return TextureAssets.Projectile[projectile.type].Value.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
        }

        public static float UnNaN(this float value)
        {
            return float.IsNaN(value) ? 0f : value;
        }
        public static Vector2 UnNaN(this Vector2 value)
        {
            return new Vector2(UnNaN(value.X), UnNaN(value.Y));
        }

        public static void CollideWithOthers(this Projectile projectile, float speed = 0.05f)
        {
            var rect = projectile.getRect();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                if (Main.projectile[i].active && i != projectile.whoAmI && projectile.type == Main.projectile[i].type && projectile.owner == Main.projectile[i].owner
                    && projectile.Colliding(rect, Main.projectile[i].getRect()))
                {
                    projectile.velocity += Main.projectile[i].DirectionTo(projectile.Center).UnNaN() * speed;
                }
            }
        }

        public static void GetDrawInfo(this Projectile projectile, out Texture2D texture, out Vector2 offset, out Rectangle frame, out Vector2 origin, out int trailLength)
        {
            texture = TextureAssets.Projectile[projectile.type].Value;
            offset = projectile.Size / 2f;
            frame = Frame(projectile);
            origin = frame.Size() / 2f;
            trailLength = ProjectileID.Sets.TrailCacheLength[projectile.type];
        }

        public static void DefaultToExplosion(this Projectile projectile, int size, DamageClass damageClass, int timeLeft = 2)
        {
            projectile.width = size;
            projectile.height = size;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.DamageType = damageClass;
            projectile.aiStyle = -1;
            projectile.timeLeft = timeLeft;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = projectile.timeLeft + 1;
            projectile.penetrate = -1;
        }

        public static ArtifactProj Artifacts(this Projectile npc)
        {
            return npc.GetGlobalProjectile<ArtifactProj>();
        }
        public static RORProjectile ROR(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<RORProjectile>();
        }

        public static ArtifactNPC Artifacts(this NPC npc)
        {
            return npc.GetGlobalNPC<ArtifactNPC>();
        }
        public static RORNPC ROR(this NPC npc)
        {
            return npc.GetGlobalNPC<RORNPC>();
        }

        public static ArtifactPlayer Artifacts(this Player player)
        {
            return player.GetModPlayer<ArtifactPlayer>();
        }
        public static RORPlayer ROR(this Player player)
        {
            return player.GetModPlayer<RORPlayer>();
        }
    }
}