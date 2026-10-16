using Fargowiltas.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Items.Renewals;

public class CorruptRenewal : BaseRenewalItem
{
    public CorruptRenewal() : base("Corruption Renewal", "Corrupts a large radius", ItemID.PurpleSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<CorruptNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class CorruptRenewalSupreme : BaseRenewalItem
{
    public CorruptRenewalSupreme() : base("Corruption Renewal Supreme", "Corrupts the entire world", -1, true, ModContent.ItemType<CorruptRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<CorruptNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class CrimsonRenewal : BaseRenewalItem
{
    public CrimsonRenewal() : base("Crimson Renewal", "Crimsons a large radius", ItemID.RedSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<CrimsonNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class CrimsonRenewalSupreme : BaseRenewalItem
{
    public CrimsonRenewalSupreme() : base("Crimson Renewal Supreme", "Crimsons the entire world", -1, true, ModContent.ItemType<CrimsonRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<CrimsonNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class HallowRenewal : BaseRenewalItem
{
    public HallowRenewal() : base("Hallowed Renewal", "Hallows a large radius", ItemID.BlueSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<HallowNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class HallowRenewalSupreme : BaseRenewalItem
{
    public HallowRenewalSupreme() : base("Hallowed Renewal Supreme", "Hallows the entire world", -1, true, ModContent.ItemType<HallowRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<HallowNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class MushroomRenewal : BaseRenewalItem
{
    public MushroomRenewal() : base("Mushroom Renewal", "Shroomifies a large radius", ItemID.DarkBlueSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<MushroomNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class MushroomRenewalSupreme : BaseRenewalItem
{
    public MushroomRenewalSupreme() : base("Mushroom Renewal Supreme", "Shroomifies the entire world", -1, true, ModContent.ItemType<MushroomRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<MushroomNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class PurityRenewal : BaseRenewalItem
{
    public PurityRenewal() : base("Purity Renewal", "Cleanses a large radius", ItemID.GreenSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<PurityNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class PurityRenewalSupreme : BaseRenewalItem
{
    public PurityRenewalSupreme() : base("Purity Renewal Supreme", "Cleanses the entire world", -1, true, ModContent.ItemType<PurityRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<PurityNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class SnowRenewal : BaseRenewalItem
{
    public SnowRenewal() : base("Snow Renewal", "Snows a large radius", ItemID.SnowSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<SnowNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class SnowRenewalSupreme : BaseRenewalItem
{
    public SnowRenewalSupreme() : base("Snow Renewal Supreme", "Snows the entire world", -1, true, ModContent.ItemType<SnowRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<SnowNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class SandRenewal : BaseRenewalItem
{
    public SandRenewal() : base("Desert Renewal", "Sands a large radius", ItemID.SandSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<SandNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class SandRenewalSupreme : BaseRenewalItem
{
    public SandRenewalSupreme() : base("Desert Renewal Supreme", "Sands the entire world", -1, true, ModContent.ItemType<SandRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<SandNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class DirtRenewal : BaseRenewalItem
{
    public DirtRenewal() : base("Forest Renewal", "Dirts a large radius", ItemID.SandSolution)
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<DirtNukeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}

public class DirtRenewalSupreme : BaseRenewalItem
{
    public DirtRenewalSupreme() : base("Forest Renewal Supreme", "Dirts the entire world", -1, true, ModContent.ItemType<DirtRenewal>())
    {
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(player.GetSource_ItemUse(source.Item), position, velocity, ModContent.ProjectileType<DirtNukeSupremeProj>(), 0, 0, Main.myPlayer);

        return false;
    }
}