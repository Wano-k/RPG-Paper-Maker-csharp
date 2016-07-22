﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPG_Paper_Maker
{
    [Serializable]
    public class Autotiles
    {
        public int Id;
        public Dictionary<int[], Autotile> Tiles = new Dictionary<int[], Autotile>(new IntArrayComparer());
        public static Dictionary<string, int> AutotileBorder = new Dictionary<string, int>{
            { "A1", 2 },
            { "B1", 3 },
            { "C1", 6 },
            { "D1", 7 },
            { "A2", 8 },
            { "B4", 9 },
            { "A4", 10 },
            { "B2", 11 },
            { "C5", 12 },
            { "D3", 13 },
            { "C3", 14 },
            { "D5", 15 },
            { "A5", 16 },
            { "B3", 17 },
            { "A3", 18 },
            { "B5", 19 },
            { "C2", 20 },
            { "D4", 21 },
            { "C4", 22 },
            { "D2", 23 },
        };

        [NonSerialized()]
        VertexBuffer VB;
        [NonSerialized()]
        VertexPositionTexture[] VerticesArray;
        [NonSerialized()]
        IndexBuffer IB;
        [NonSerialized()]
        int[] IndexesArray;


        // -------------------------------------------------------------------
        // Constructor
        // -------------------------------------------------------------------

        public Autotiles(int id)
        {
            Id = id;
        }

        // -------------------------------------------------------------------
        // CreateCopy
        // -------------------------------------------------------------------

        public Autotiles CreateCopy()
        {
            Autotiles newAutotiles = new Autotiles(Id);
            foreach (KeyValuePair<int[], Autotile> entry in Tiles)
            {
                newAutotiles.Tiles[entry.Key] = entry.Value.CreateCopy();
            }

            return newAutotiles;
        }

        // -------------------------------------------------------------------
        // IsEmpty
        // -------------------------------------------------------------------

        public bool IsEmpty()
        {
            return Tiles.Count == 0;
        }

        // -------------------------------------------------------------------
        // Add
        // -------------------------------------------------------------------

        public void Add(int[] coords, bool update = true)
        {
            if (!Tiles.ContainsKey(coords))
            {
                Tiles[coords] = new Autotile();
                UpdateAround(coords[0], coords[1], coords[2], coords[3], update);
            }
        }

        // -------------------------------------------------------------------
        // Remove
        // -------------------------------------------------------------------

        public void Remove(int[] coords, bool update = true)
        {
            if (Tiles.ContainsKey(coords))
            {
                Tiles.Remove(coords);
                UpdateAround(coords[0], coords[1], coords[2], coords[3], update);
            }
        }

        // -------------------------------------------------------------------
        // UpdateAround
        // -------------------------------------------------------------------

        public void UpdateAround(int x, int y1, int y2, int z, bool update)
        {
            int[] portion = MapEditor.Control.GetPortion(x, z); // portion where you are setting autotile
            for (int X = x - 1; X <= x + 1; X++)
            {
                for (int Z = z - 1; Z <= z + 1; Z++)
                {
                    int[] coords = new int[] { X, y1, y2, Z };
                    Autotile autotileAround = TileOnWhatever(coords);
                    if (autotileAround != null)
                    {
                        if (update) autotileAround.Update(this, coords, portion);
                        else
                        {
                            int[] newPortion = MapEditor.Control.GetPortion(X, Z);
                            if (WANOK.IsInPortions(newPortion))
                            {
                                MapEditor.Control.AddPortionsAutotileToUpdate(newPortion);
                                WANOK.AddPortionsToAddCancel(MapEditor.Control.Map.MapInfos.RealMapName, MapEditor.Control.GetGlobalPortion(newPortion));
                            }
                            else autotileAround.Update(this, coords, portion);
                        }
                    }
                }
            }
        }

        // -------------------------------------------------------------------
        // Update
        // -------------------------------------------------------------------

        public void Update(int[] coords, int[] portion)
        {
            Tiles[coords].Update(this, coords, portion);
        }

        // -------------------------------------------------------------------
        // TILES
        // -------------------------------------------------------------------

        public bool TileOnLeft(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] - 1, coords[1], coords[2], coords[3] }) != null;
        }

        public bool TileOnRight(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] + 1, coords[1], coords[2], coords[3] }) != null;
        }

        public bool TileOnTop(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0], coords[1], coords[2], coords[3] - 1 }) != null;
        }

        public bool TileOnBottom(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0], coords[1], coords[2], coords[3] + 1 }) != null;
        }

        public bool TileOnTopLeft(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] - 1, coords[1], coords[2], coords[3] - 1 }) != null;
        }

        public bool TileOnTopRight(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] + 1, coords[1], coords[2], coords[3] - 1 }) != null;
        }

        public bool TileOnBottomLeft(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] - 1, coords[1], coords[2], coords[3] + 1 }) != null;
        }

        public bool TileOnBottomRight(int[] coords, int[] portion)
        {
            return TileOnWhatever(new int[] { coords[0] + 1, coords[1], coords[2], coords[3] + 1 }) != null;
        }

        public Autotile TileOnWhatever(int[] coords)
        {
            int[] portion = MapEditor.Control.GetPortion(coords[0], coords[3]);
            if (MapEditor.Control.Map.Portions.ContainsKey(portion))
            {
                if (MapEditor.Control.Map.Portions[portion] != null && MapEditor.Control.Map.Portions[portion].Autotiles.ContainsKey(Id))
                {
                    if (MapEditor.Control.Map.Portions[portion].Autotiles[Id].Tiles.ContainsKey(coords)) return MapEditor.Control.Map.Portions[portion].Autotiles[Id].Tiles[coords];
                }
                return null;
            }
            else
            {
                return null;
            }
        }

        // -------------------------------------------------------------------
        // GenAutotiles
        // -------------------------------------------------------------------

        public void GenAutotiles(GraphicsDevice device)
        {
            DisposeBuffers(device);
            CreatePortion(device);
        }

        // -------------------------------------------------------------------
        // CreatePortion
        // -------------------------------------------------------------------

        public void CreatePortion(GraphicsDevice device)
        {
            List<VertexPositionTexture> verticesList = new List<VertexPositionTexture>();
            List<int> indexesList = new List<int>();
            int[] indexes = new int[]
            {
                0, 1, 2, 0, 2, 3,
                4, 5, 6, 4, 6, 7,
                8, 9, 10, 8, 10, 11,
                12, 13, 14, 12, 14, 15,
            };
            int offset = 0;

            if (MapEditor.TexAutotiles.ContainsKey(Id) && MapEditor.TexAutotiles[Id].Width == (2 * WANOK.SQUARE_SIZE) && MapEditor.TexAutotiles[Id].Height == (3 * WANOK.SQUARE_SIZE))
            {
                foreach (KeyValuePair<int[], Autotile> entry in Tiles)
                {
                    foreach (VertexPositionTexture vertex in CreateTex(MapEditor.TexAutotiles[Id], entry.Key, entry.Value))
                    {
                        verticesList.Add(vertex);
                    }
                    for (int n = 0; n < 24; n++)
                    {
                        indexesList.Add(indexes[n] + offset);
                    }
                    offset += 16;
                }

                VerticesArray = verticesList.ToArray();
                IndexesArray = indexesList.ToArray();
                IB = new IndexBuffer(device, IndexElementSize.ThirtyTwoBits, IndexesArray.Length, BufferUsage.None);
                IB.SetData(IndexesArray);
                VB = new VertexBuffer(device, VertexPositionTexture.VertexDeclaration, VerticesArray.Length, BufferUsage.None);
                VB.SetData(VerticesArray);
            }
        }

        // -------------------------------------------------------------------
        // CreateTex
        // -------------------------------------------------------------------

        protected VertexPositionTexture[] CreateTex(Texture2D texture, int[] coords, Autotile autotile)
        {
            VertexPositionTexture[] res = new VertexPositionTexture[16];

            int x = coords[0], y = coords[1] * WANOK.SQUARE_SIZE + coords[2], z = coords[3];
            float[] left = new float[4], top = new float[4], bot = new float[4], right = new float[4];
            float[] leftPos = new float[4], topPos = new float[4], botPos = new float[4], rightPos = new float[4];

            for (int i = 0; i < 4; i++)
            {
                int xTile = autotile.Tiles[i] % 4;
                int yTile = autotile.Tiles[i] / 4;
                float pos = i < 2 ? 0.0f : 0.5f;

                // Texture coords
                leftPos[i] = (float)(i % 2) / 2;
                topPos[i] = pos;
                botPos[i] = pos + 0.5f;
                rightPos[i] = leftPos[i] + 0.5f;
                left[i] = (xTile + leftPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Width;
                top[i] = (yTile + topPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Height;
                bot[i] = (yTile + botPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Height;
                right[i] = (xTile + rightPos[i]) * (WANOK.SQUARE_SIZE / 2) / texture.Width;

                // Adjust in order to limit risk of textures flood
                float width = left[i] + right[i];
                float height = top[i] + bot[i];
                left[i] += width / WANOK.COEF_BORDER_TEX;
                right[i] -= width / WANOK.COEF_BORDER_TEX;
                top[i] += height / WANOK.COEF_BORDER_TEX;
                bot[i] -= height / WANOK.COEF_BORDER_TEX;

                // Vertex Position and Texture
                res[i * 4] = new VertexPositionTexture(new Vector3(x + leftPos[i], y, z + topPos[i]), new Vector2(left[i], top[i]));
                res[i * 4 + 1] = new VertexPositionTexture(new Vector3(x + rightPos[i], y, z + topPos[i]), new Vector2(right[i], top[i]));
                res[i * 4 + 2] = new VertexPositionTexture(new Vector3(x + rightPos[i], y, z + botPos[i]), new Vector2(right[i], bot[i]));
                res[i * 4 + 3] = new VertexPositionTexture(new Vector3(x + leftPos[i], y, z + botPos[i]), new Vector2(left[i], bot[i]));
            }

            return res;
        }

        // -------------------------------------------------------------------
        // Draw
        // -------------------------------------------------------------------

        public void Draw(GraphicsDevice device, AlphaTestEffect effect)
        {
            if (VB != null)
            {
                if (!MapEditor.TexAutotiles.ContainsKey(Id)) effect.Texture = MapEditor.TexNone;
                else effect.Texture = MapEditor.TexAutotiles[Id];

                device.SetVertexBuffer(VB);
                device.Indices = IB;
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    device.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, VerticesArray, 0, VerticesArray.Length, IndexesArray, 0, VerticesArray.Length / 2);
                }
            }
        }

        // -------------------------------------------------------------------
        // DisposeBuffers
        // -------------------------------------------------------------------

        public void DisposeBuffers(GraphicsDevice device, bool nullable = true)
        {
            if (VB != null)
            {
                device.SetVertexBuffer(null);
                device.Indices = null;
                VB.Dispose();
                IB.Dispose();
                if (nullable)
                {
                    VB = null;
                    IB = null;
                }
            }
        }
    }
}
