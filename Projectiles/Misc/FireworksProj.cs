﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RORMod.Projectiles.Misc
{
    public class FireworksProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Generic;
            Projectile.width = 12;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.velocity = new Vector2(0, -5);
                Projectile.ai[0]--;
                Projectile.penetrate = -1;
            }
            else
            {
                Projectile.penetrate = 1;
                int npcIndex = Projectile.FindTargetWithLineOfSight(800f);

                if (npcIndex == -1)
                {
                    return;
                }

                var closest = Main.npc[npcIndex];

                Projectile.velocity = Vector2.Lerp(Projectile.velocity, (closest.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * 8f, 0.08f);
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                int p = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.RocketFireworkRed + Main.rand.Next(4), Projectile.damage, 1);
                Main.projectile[p].timeLeft = 1;
            }
        }
    }
}