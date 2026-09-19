using Fargowiltas.Content.Items.Explosives;
using Fargowiltas.Content.Items.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Content.Projectiles.Explosives;

public class ObsidianWreckingBallProj : ModProjectile
{
    private float fallSpinVariance;
    public ref float State => ref Projectile.ai[0];

    private static readonly HashSet<ushort> HouseTileTypes = new()
    {
        TileID.ObsidianBrick, TileID.HellstoneBrick, TileID.Platforms, TileID.ClosedDoor, TileID.OpenDoor,
        TileID.Containers, TileID.Containers2, TileID.Hellforge, TileID.Torches, TileID.Chairs, TileID.Tables, TileID.Tables2, TileID.WorkBenches, TileID.Candles, TileID.Chandeliers, TileID.Dressers, TileID.Lamps, TileID.Beds, TileID.Banners,
        TileID.Cobweb, TileID.SmallPiles, TileID.LargePiles, TileID.LargePiles2, TileID.Pots,
        TileID.Painting3X3, TileID.Painting4X3, TileID.Painting6X4, TileID.Painting2X3, TileID.Painting3X2
    };

    public override void SetDefaults()
    {
        Projectile.width = 28;
        Projectile.height = 28;
        Projectile.penetrate = -1;
        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;
        Projectile.hide = true;
        Projectile.netImportant = true;
        Projectile.timeLeft = 2;
    }

    private int swing_direction = 1;
    private readonly List<int> _landingDust = new List<int>();
    public ref float Timer => ref Projectile.ai[1];

    public static bool PlayerHasActiveBall(int owner)
    {
        int proj_type = ModContent.ProjectileType<ObsidianWreckingBallProj>();
        for (int i = 0; i < Main.maxProjectiles; i++)
        {
            var proj = Main.projectile[i];
            if (proj.active && proj.type == proj_type && proj.owner == owner && proj.ai[0] != 2f) return true;
        }
        return false;
    }

    private readonly List<int> house_rows = new();
    private Dictionary<int, List<(int x, int y)>> houseTilesByRow = new Dictionary<int, List<(int x, int y)>>();
    private int currentDestructionRowIndex;
    public ref float Depth => ref Projectile.localAI[0];
    public ref float SpinVariance => ref Projectile.localAI[1];

    private static readonly HashSet<ushort> HouseWallTypes = new HashSet<ushort> {
        WallID.ObsidianBrickUnsafe, WallID.HellstoneBrickUnsafe
    };

    public override bool? CanDamage() => false;

    public override void SendExtraAI(BinaryWriter writer)
    {
        writer.Write(Projectile.localAI[1]);
        writer.Write(fallSpinVariance);
    }

    public override void ReceiveExtraAI(BinaryReader reader)
    {
        Projectile.localAI[1] = reader.ReadSingle();
        fallSpinVariance = reader.ReadSingle();
    }

    private static bool IsValidHouseTile(Tile t) => t.HasTile && (t.TileType == TileID.ObsidianBrick || t.TileType == TileID.HellstoneBrick || t.TileType == TileID.ClosedDoor || t.TileType == TileID.OpenDoor || t.TileType == TileID.Platforms);

    private static bool ChestHasItemsAt(int x, int y)
    {
        Point16 top_left = FargoGlobalTile.FindChestTopLeft(x, y, false);
        if (top_left == Point16.NegativeOne) return false;
        int idx = Chest.FindChest(top_left.X, top_left.Y);
        if (idx == -1 || Main.chest[idx] == null) return false;

        for (var i = 0; i < Main.chest[idx].item.Length; i++)
        {
            var itm = Main.chest[idx].item[i];
            if (itm != null && !itm.IsAir) return true;
        }
        return false;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];
        if (!player.active || player.dead)
        {
            Projectile.Kill();
            return;
        }
        for (int i = _landingDust.Count - 1; i >= 0; i--)
        {
            Dust dust = Main.dust[_landingDust[i]];
            if (!dust.active || Collision.SolidCollision(dust.position, 2, 2))
            {
                if (dust.active) dust.active = false;
                _landingDust.RemoveAt(i);
            }
        }
        if (Projectile.ai[0] == 0f)
        {
            AI_StateSwing(player);
            return;
        }
        if (Projectile.ai[0] == 1f)
        {
            Projectile.timeLeft = 2;
            Projectile.velocity.Y += Projectile.velocity.Y < 0f ? 0.2f : 0.8f;
            Projectile.rotation += Projectile.velocity.Y * 0.05f * (Projectile.velocity.Y < 0f ? Projectile.localAI[1] : fallSpinVariance);
            return;
        }
        if (Projectile.ai[0] == 2f)
        {
            AI_StateDestruction();
        }
    }

    private void AI_StateSwing(Player p)
    {
        Projectile.timeLeft = 2;
        if (p.whoAmI == Main.myPlayer && p.HeldItem.type != ModContent.ItemType<ObsidianWreckingBall>())
        {
            Projectile.Kill();
            return;
        }
        p.heldProj = Projectile.whoAmI;
        p.itemTime = 2;
        p.itemAnimation = 2;
        if (Projectile.ai[1] == 0f) swing_direction = p.direction;

        Projectile.ai[1] += 1f;
        float angle = MathHelper.Clamp(Projectile.ai[1] / 60f, 0f, 1f) * MathHelper.TwoPi;
        float offsetX = MathF.Cos(angle) * 56f * swing_direction;
        float offsetY = -16f + (MathF.Sin(angle) * 18f);

        Projectile.localAI[0] = -MathF.Sin(angle);
        Projectile.Center = p.RotatedRelativePoint(p.MountedCenter) + new Vector2(offsetX, offsetY);
        Projectile.rotation = 0f;

        if (p.whoAmI != Main.myPlayer || !(Projectile.ai[1] >= 60f)) return;
        Projectile.ai[0] = 1f;
        Projectile.ai[1] = 0f;
        Projectile.localAI[1] = Main.rand.NextFloat(0.4f, 1.8f) * (Main.rand.NextBool() ? 1f : -1f);
        fallSpinVariance = Main.rand.NextFloat(0.4f, 1.8f) * (Main.rand.NextBool() ? 1f : -1f);
        Projectile.velocity = new Vector2(0f, -MathF.Sqrt(2f * 0.2f * 128f));
        Projectile.tileCollide = true;
        Projectile.netUpdate = true;
    }

    private void AI_StateDestruction()
    {
        Projectile.tileCollide = true;
        Projectile.velocity.Y += 0.4f;
        if (Projectile.velocity.Y > 14f) Projectile.velocity.Y = 14f;

        if (house_rows.Count == 0 || currentDestructionRowIndex >= house_rows.Count)
        {
            if (Projectile.ai[1] == 0f)
            {
                Projectile.ai[1] = 210f;
            }

            Projectile.timeLeft = 2;
            Projectile.ai[1]--;

            if (Projectile.ai[1] <= 0f)
            {
                Projectile.Kill();
            }
            return;
        }

        Projectile.timeLeft = 10;
        int row_idx = house_rows[currentDestructionRowIndex++];
        if (!houseTilesByRow.TryGetValue(row_idx, out List<(int x, int y)> tilesInRow)) return;
        for (int k = 0; k < tilesInRow.Count; k++)
        {
            (int x, int y) = tilesInRow[k];
            if (ChestHasItemsAt(x, y)) continue;

            ushort originalWallType = Framing.GetTileSafely(x, y).WallType;
            bool destroyedAnything = false;
            var tile_ref = Framing.GetTileSafely(x, y);
            if (tile_ref.HasTile && HouseTileTypes.Contains(tile_ref.TileType))
            {
                WorldGen.KillTile(x, y);
                destroyedAnything = true;
            }
            if (tile_ref.WallType > 0 && HouseWallTypes.Contains(tile_ref.WallType))
            {
                tile_ref.WallType = 0;
                WorldGen.SquareWallFrame(x, y);
                destroyedAnything = true;

                int materialDustType = (originalWallType == WallID.HellstoneBrickUnsafe) ? DustID.Lava : DustID.Obsidian;
                Vector2 worldPos = new Point16(x, y).ToWorldCoordinates();
                int materialDust = Dust.NewDust(worldPos, 16, 16, materialDustType, Main.rand.NextFloat(-1.5f, 1.5f), Main.rand.NextFloat(-1f, 1f));
                Main.dust[materialDust].scale = Main.rand.NextFloat(0.8f, 1.3f);
            }
            if (destroyedAnything && Main.netMode == NetmodeID.Server) NetMessage.SendTileSquare(-1, x, y, 1);
        }
    }

    public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
    {
        overPlayers.Add(index);
    }

    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        if (Projectile.ai[0] != 1f)
        {
            if (Projectile.velocity.Y != oldVelocity.Y) Projectile.velocity.Y = 0f;
            if (Projectile.velocity.X != oldVelocity.X) Projectile.velocity.X = 0f;
            return false;
        }
        if (oldVelocity.Y < 0f)
        {
            Projectile.velocity.Y = 0f;
            Projectile.netUpdate = true;
            return false;
        }

        SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

        for (int i = 0; i < 12; i++)
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Obsidian, Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-9f, -3f));
            Main.dust[d].scale = Main.rand.NextFloat(0.9f, 1.3f);
            _landingDust.Add(d);
        }
        for (int i = 0; i < 6; i++)
        {
            int dust_idx = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-7f, -3f));
            Main.dust[dust_idx].scale = Main.rand.NextFloat(1f, 1.5f);
            Main.dust[dust_idx].fadeIn = 0.5f;
            _landingDust.Add(dust_idx);
        }
        if (Main.netMode != NetmodeID.MultiplayerClient && Main.player[Projectile.owner].ZoneUnderworldHeight)
        {
            ScanAndSetupHouseDestruction();
        }

        Projectile.velocity = Vector2.Zero;
        Projectile.ai[0] = 2f;
        Projectile.ai[1] = 0f;
        Projectile.netUpdate = true;
        return false;
    }

    private void ScanAndSetupHouseDestruction()
    {
        Point16 columnStart = Projectile.Bottom.ToTileCoordinates16();
        for (int offset = 0; offset <= 8; offset += 2)
        {
            Point16 resting = (Projectile.Bottom + new Vector2(0f, offset)).ToTileCoordinates16();
            if (Framing.GetTileSafely(resting.X, resting.Y).HasTile)
            {
                columnStart = resting;
                break;
            }
        }
        Point16 start = Point16.NegativeOne;
        for (int i = 0; i < 12; i++)
        {
            Tile t = Framing.GetTileSafely(columnStart.X, columnStart.Y + i);
            if (IsValidHouseTile(t))
            {
                start = new Point16(columnStart.X, columnStart.Y + i);
                break;
            }
        }

        if (start == Point16.NegativeOne) return;
        HashSet<(int x, int y)> visited = new() { (start.X, start.Y) };
        Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
        queue.Enqueue((start.X, start.Y));

        int min_x = start.X, max_x = start.X, minY = start.Y, maxY = start.Y;
        (int dx, int dy)[] floodOffsets = new (int dx, int dy)[] { (1, 0), (-1, 0), (0, 1), (0, -1) };
        while (queue.Count > 0 && visited.Count < 3000)
        {
            (int x, int y) = queue.Dequeue();
            min_x = Math.Min(min_x, x);
            max_x = Math.Max(max_x, x);
            minY = Math.Min(minY, y);
            maxY = Math.Max(maxY, y);
            for (int o = 0; o < floodOffsets.Length; o++)
            {
                (int x, int y) next = (x + floodOffsets[o].dx, y + floodOffsets[o].dy);
                Tile nextTile = Framing.GetTileSafely(next.x, next.y);

                if (visited.Contains(next) || !IsValidHouseTile(nextTile)) continue;
                visited.Add(next);
                queue.Enqueue(next);
            }
        }

        if (visited.Count < 50) return;
        int width = max_x - min_x + 1;
        int margin = Math.Max(5, width);
        int sampleMinX = min_x - margin;
        int sampleMaxX = max_x + margin;
        int groundRow = -1;
        int misses = 0;

        for (int y = maxY + 8; y >= minY; y--)
        {
            int naturalCount = 0;
            int sampled = 0;
            for (int x = sampleMinX; x <= sampleMaxX; x++)
            {
                if (x >= min_x && x <= max_x) continue;

                Tile tile = Framing.GetTileSafely(x, y);
                bool solidGround = tile.HasTile && (tile.TileType == TileID.Ash || tile.TileType == TileID.AshGrass || tile.TileType == TileID.Hellstone || tile.TileType == TileID.Obsidian);
                bool lava = tile.LiquidAmount > 0 && tile.LiquidType == LiquidID.Lava;

                sampled++;
                if (solidGround || lava) naturalCount++;
            }

            if (sampled > 0 && naturalCount >= sampled * 0.5f)
            {
                groundRow = y;
                misses = 0;
                continue;
            }

            if (groundRow < 0) continue;
            misses++;
            if (misses > 1) break;
        }

        int floorSearchStart = groundRow > 0 ? Math.Min(maxY, groundRow + 3) : maxY;
        int searchMaxY = Math.Max(floorSearchStart, minY);

        Dictionary<int, HashSet<int>> rowColumns = new Dictionary<int, HashSet<int>>();
        foreach ((int x, int y) in visited)
        {
            if (!rowColumns.ContainsKey(y)) rowColumns[y] = new HashSet<int>();
            rowColumns[y].Add(x);
        }

        int floorRow = searchMaxY;
        for (int y = searchMaxY; y >= minY; y--)
        {
            if (!rowColumns.TryGetValue(y, out HashSet<int> columns)) continue;

            int longestRun = 0, run = 0;
            for (int x = min_x; x <= max_x; x++)
            {
                if (!columns.Contains(x)) { run = 0; continue; }
                run++;
                if (run > longestRun) longestRun = run;
            }
            if (columns.Count >= width * 0.6f || longestRun >= width * 0.5f)
            {
                floorRow = y;
                break;
            }
        }

        visited.RemoveWhere(p => p.y >= floorRow);
        if (visited.Count == 0) return;
        min_x = int.MaxValue;
        max_x = int.MinValue;
        minY = int.MaxValue;

        foreach ((int x, int y) in visited)
        {
            min_x = Math.Min(min_x, x);
            max_x = Math.Max(max_x, x);
            minY = Math.Min(minY, y);
        }

        int maxAllowedY = floorRow - 1;
        HashSet<(int x, int y)> allHousePositions = new HashSet<(int x, int y)>();

        foreach ((int x, int y) in visited)
        {
            if (y <= maxAllowedY) allHousePositions.Add((x, y));
        }

        for (int x = min_x; x <= max_x; x++)
        {
            for (int y = minY; y <= maxAllowedY; y++)
            {
                Tile tile = Framing.GetTileSafely(x, y);
                bool isBreakableTile = tile.HasTile && HouseTileTypes.Contains(tile.TileType);
                bool isBreakableWall = tile.WallType > 0 && HouseWallTypes.Contains(tile.WallType);

                if (isBreakableTile || isBreakableWall) allHousePositions.Add((x, y));
            }
        }

        houseTilesByRow = new Dictionary<int, List<(int x, int y)>>();
        foreach ((int x, int y) in allHousePositions)
        {
            if (!houseTilesByRow.ContainsKey(y)) houseTilesByRow[y] = new List<(int x, int y)>();
            houseTilesByRow[y].Add((x, y));
        }

        house_rows.Clear();
        List<int> sortedRowKeys = new List<int>(houseTilesByRow.Keys);
        sortedRowKeys.Sort();
        for (int r = 0; r < sortedRowKeys.Count; r++) house_rows.Add(sortedRowKeys[r]);

        currentDestructionRowIndex = 0;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        float normalizedDepth = (Projectile.localAI[0] + 1f) * 0.5f;
        float zScale = 1f;
        Color renderColor = lightColor;

        if (Projectile.ai[0] == 2f && Projectile.ai[1] > 0f)
        {
            float alpha = MathHelper.Clamp(Projectile.ai[1] / 30f, 0f, 1f);
            renderColor *= alpha;
        }

        if (Projectile.ai[0] == 0f)
        {
            zScale = MathHelper.Lerp(0.75f, 1.25f, normalizedDepth);
            if (Projectile.localAI[0] < 0f) renderColor *= MathHelper.Lerp(0.7f, 1.0f, normalizedDepth);

            Player p = Main.player[Projectile.owner];
            Vector2 start = p.RotatedRelativePoint(p.MountedCenter);
            Vector2 end = Projectile.Center;
            var chainTexture = ModContent.Request<Texture2D>("Fargowiltas/Content/Projectiles/Explosives/ObsidianWreckingBallChain").Value;
            Texture2D endTexture = ModContent.Request<Texture2D>("Fargowiltas/Content/Projectiles/Explosives/ObsidianWreckingBallChainEnd").Value;
            Texture2D ballTexture = TextureAssets.Projectile[Type].Value;
            Vector2 direction = end - start;
            Vector2 dirNormalized = direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction);
            Vector2 adjustedEnd = end - (dirNormalized * (ballTexture.Width / 2f * zScale * 0.2f));

            for (int i = 1; i <= 5; i++)
            {
                float lerpAmount = (float)i / 5;
                Vector2 segmentPos = Vector2.Lerp(start, adjustedEnd, lerpAmount);
                Color color = Lighting.GetColor(segmentPos.ToTileCoordinates());

                if (Projectile.localAI[0] < 0f) color *= MathHelper.Lerp(0.7f, 1.0f, (MathHelper.Lerp(0f, Projectile.localAI[0], lerpAmount) + 1f) * 0.5f);

                Texture2D textureToDraw = i == 5 ? endTexture : chainTexture;
                Main.EntitySpriteDraw(textureToDraw, segmentPos - Main.screenPosition, null, color, 0f, textureToDraw.Size() / 2f, MathHelper.Lerp(1.0f, zScale, lerpAmount), SpriteEffects.None, 0);
            }
        }

        Texture2D ballTextureProj = TextureAssets.Projectile[Type].Value;
        Main.EntitySpriteDraw(ballTextureProj, Projectile.Center - Main.screenPosition, null, renderColor, Projectile.rotation, ballTextureProj.Size() / 2f, Projectile.scale * zScale, SpriteEffects.None, 0);
        return false;
    }
}