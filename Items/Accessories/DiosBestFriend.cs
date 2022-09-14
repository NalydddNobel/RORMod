﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace RORMod.Items.Accessories
{
    public class DiosBestFriend : ModItem
    {
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            RORItem.RedTier.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 22;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 5);
        }

        public override Color? GetAlpha(Color lightColor)
        {
            if (Main.LocalPlayer.ROR().diosCooldown > 0)
            {
                byte a = lightColor.A;
                return (lightColor * 0.5f).UseA(a);
            }
            return null;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (Main.LocalPlayer.difficulty == PlayerDifficultyID.Hardcore)
            {
                tooltips.Insert(RORItem.GetIndex(tooltips, "Consumable"), new TooltipLine(Mod, "Consumable", Language.GetTextValue("LegacyTooltip.35")));
            }
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var ror = player.ROR();
            if (ror.diosCooldown > 0 && player.difficulty == PlayerDifficultyID.Hardcore)
            {
                Item.TurnToAir();
                ror.diosCooldown = 0;
            }
            ror.accDiosBestFriend = 36000;
        }
    }
}