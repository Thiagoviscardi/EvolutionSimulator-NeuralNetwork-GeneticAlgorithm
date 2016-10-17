﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Tile_V2
{
    public const int TILE_RED = 0;
    public const int TILE_GREEN = 1;
    public const int TILE_BLUE = 2;
    public const int TILE_ORANGE = 3;
    public const int TILE_PURPLE = 4;
    public const int TILE_INFERTIAL = 0;
    


    public const int TILE_FERT = 0;
    public const int TILE_INFERT = 1;

    public const int TILE_WATER = -1;

    public int type;
    public HSBColor detail;
    public float maxEnergy;
    public float currentEnergy;

    public List<Creature_V2> creatureListOnTile = new List<Creature_V2>();

}
public class TileMap_V2
{
    private Tile_V2[,] tiles;
    private Texture2D texture;
    private int sizeX;
    private int sizeY;
    float worldDeltaTime = 0.001f; //each year last
    private float maxEnergyGrownOnTile = 0.75f;
    private float climate = -0.1f; //1 is excellent climate for growth, 0 means nothing will grow, and below zero, vegetation starts to die
    private List<int[]> floorTiles = new List<int[]>();

    public TileMap_V2(Texture2D tex, int sizeX, int sizeY)
    {
        Color[] texColor = tex.GetPixels(0, 0, sizeX, sizeY);
        this.texture = tex;
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        tiles = new Tile_V2[sizeY, sizeX];

        /*noiseSeed(SEED);
        randomSeed(SEED);*/
        float stepSize = 0.1f;
        float stepSizeWater = 0.01f;

        for (int x = 0; x < sizeY; x++)
        {
            for (int y = 0; y < sizeX; y++)
            {
                /*float bigForce = Mathf.Pow(((float)y) / sizeY, 0.5f);
                float fertility = Mathf.PerlinNoise(x * stepSize * 3f, y * stepSize * 3f) * (1f - bigForce) * 5.0f + Mathf.PerlinNoise(x * stepSize * 0.5f, y * stepSize * 0.5f) * bigForce * 5.0f - 1.5f;
                float climateType = Mathf.PerlinNoise(x * stepSize * 0.2f + 10000f, y * stepSize * 0.2f + 10000f) * 1.63f - 0.4f;
                float waterType = Mathf.PerlinNoise(x * stepSizeWater + 10425f, y * stepSizeWater + 34224f);
                climateType = Mathf.Min(Mathf.Max(climateType, 0f), 0.8f);

                tiles[y,x] = new Tile_V2();

                tiles[y, x].maxEnergy = fertility;
                tiles[y, x].currentEnergy = fertility;


                HSBColor color = HSBColor.HSBCOLOR_BLACK;

                if (fertility > 0.05f)
                    tiles[y, x].type = Tile_V2.TILE_FERT;
                else if (waterType<0.5f)
                    tiles[y, x].type = Tile_V2.TILE_INFERT;
                else
                    tiles[y, x].type = Tile_V2.TILE_WATER;*/



                tiles[y, x] = new Tile_V2();
                HSBColor color = HSBColor.HSBCOLOR_BLACK;

                float bigForce = Mathf.Pow(((float)y) / sizeY, 0.5f);
                float fertility = Mathf.PerlinNoise(x * stepSize * 3f, y * stepSize * 3f) * (1f - bigForce) * 5.0f + Mathf.PerlinNoise(x * stepSize * 0.5f, y * stepSize * 0.5f) * bigForce * 5.0f - 1.5f;
                float climateType = Mathf.PerlinNoise(x * stepSize * 0.2f + 10000f, y * stepSize * 0.2f + 10000f) * 1.63f - 0.4f;
                float waterType = Mathf.PerlinNoise(x * stepSizeWater * 3f, y * stepSizeWater * 3f) * (1f - bigForce) * 5.0f + Mathf.PerlinNoise(x * stepSizeWater * 0.5f, y * stepSizeWater * 0.5f) * bigForce * 5.0f - 1.5f;
                climateType = Mathf.Min(Mathf.Max(climateType, 0f), 0.8f);

                

                tiles[y, x].maxEnergy = fertility;
                tiles[y, x].currentEnergy = fertility;


                

                if (fertility > 0.05f)
                    tiles[y, x].type = Tile_V2.TILE_FERT;
                else if (waterType < 0.5f)
                    tiles[y, x].type = Tile_V2.TILE_INFERT;
                else
                    tiles[y, x].type = Tile_V2.TILE_WATER;


                if (tiles[y, x].type == Tile_V2.TILE_FERT)
                {
                    color.h = Mathf.Max(0f, climateType);
                    color.s = tiles[y, x].currentEnergy;
                    color.b = 1f - (0.25f - (color.s * 0.25f));
                }
                else if (tiles[y, x].type == Tile_V2.TILE_INFERT)
                {
                    color = HSBColor.HSBCOLOR_WHITE;
                }
                else if (tiles[y, x].type == Tile_V2.TILE_WATER)
                {
                    color = HSBColor.HSBCOLOR_BLACK;
                }

                tiles[y, x].detail = color;     
                texture.SetPixel(x,y,tiles[y,x].detail.ToColor());
            }
        }
        texture.Apply();





        /*int colorIndex = 0;
        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                tiles[y, x] = new Tile_V2();
                float r = texColor[colorIndex].r;
                float g = texColor[colorIndex].g;
                float b = texColor[colorIndex].b;

                tiles[y, x].detail = new HSBColor(texColor[colorIndex]);
                tiles[y, x].detail.b = 1f;
                if (r == 0 && g == 0 && b == 0)
                {
                    tiles[y, x].type = Tile_V2.TILE_WATER;
                    tiles[y, x].maxEnergy = 0f;
                   
                }
                else if (r == 1 && g == 1 && b == 1)
                {
                    tiles[y, x].type = Tile_V2.TILE_INFERTIAL;
                    tiles[y, x].maxEnergy = 0f;
                }
                else
                {
                    if (r == 1 && g == 0 && b == 0)
                    {
                        tiles[y, x].type = Tile_V2.TILE_RED;
                        tiles[y, x].maxEnergy = maxEnergyGrownOnTile;
                    }
                    else if (r == 0 && g == 1 && b == 0)
                    {
                        tiles[y, x].type = Tile_V2.TILE_GREEN;
                        tiles[y, x].maxEnergy = maxEnergyGrownOnTile;
                    }
                    else if (r == 0 && g == 0 && b == 1)
                    {
                        tiles[y, x].type = Tile_V2.TILE_BLUE;
                        tiles[y, x].maxEnergy = maxEnergyGrownOnTile;
                    }
                    else if (r == b)
                    {
                        tiles[y, x].type = Tile_V2.TILE_PURPLE;
                        tiles[y, x].maxEnergy = maxEnergyGrownOnTile;
                        tiles[y, x].detail = new HSBColor(0.7777778f, 1f, 1f);
                    }
                    else
                    {
                        tiles[y, x].type = Tile_V2.TILE_ORANGE;
                        tiles[y, x].maxEnergy = maxEnergyGrownOnTile;
                    }
                    floorTiles.Add(new int[] { x, y });
                }


                tiles[y, x].currentEnergy = maxEnergyGrownOnTile;

                colorIndex++;
            }
        }*/
    }


    bool check = false;


    int time = 0;
    float seed = 0f;
    public void Apply(float playSpeed, bool visual)
    {

        /*for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {

                if (tiles[y, x].type != Tile_V2.TILE_INFERTIAL && tiles[y, x].type != Tile_V2.TILE_WATER)
                {
                    if (climate > 0)
                    {
                        if (tiles[y, x].currentEnergy< tiles[y, x].maxEnergy)
                        {
                            tiles[y, x].currentEnergy += climate * worldDeltaTime * playSpeed;
                        }
                    }
                    else if (climate < 0)
                    {
                        if (tiles[y, x].currentEnergy > 0f)
                        {
                            tiles[y, x].currentEnergy += climate * worldDeltaTime * playSpeed;
                        }
                    }

                    float saturationToEnergyRatio = tiles[y, x].currentEnergy;
                    tiles[y, x].detail.s = saturationToEnergyRatio;
                    tiles[y, x].detail.b = 1f - (0.25f - (saturationToEnergyRatio * 0.25f));

                    texture.SetPixel(x, y, tiles[y, x].detail.ToColor());
                }
            }
        }

        if (visual == true)
        {
            texture.Apply();
        }*/


        /*time++;
        if (time == 1)
        {
            time = 0;
            seed += 0.05f;
            float stepSize = 0.1f;
            float stepSizeWater = 0.01f;
            for (int x = 0; x < sizeY; x++)
            {
                for (int y = 0; y < sizeX; y++)
                {
                    float bigForce = Mathf.Pow(((float)y) / sizeY, 0.5f);
                    float fertility = Mathf.PerlinNoise(x * stepSize * 3f + seed, y * stepSize * 3f + seed) * (1f - bigForce) * 5.0f + Mathf.PerlinNoise(x * stepSize * 0.5f + seed, y * stepSize * 0.5f + seed) * bigForce * 5.0f - 1.5f;
                    float climateType = Mathf.PerlinNoise(x * stepSize * 0.2f + 10000f + seed, y * stepSize * 0.2f + 10000f + seed) * 1.63f - 0.4f;
                    float waterType = Mathf.PerlinNoise(x * stepSizeWater + 10425f + seed, y * stepSizeWater + 34224f + seed);
                    climateType = Mathf.Min(Mathf.Max(climateType, 0f), 0.8f);

                    tiles[y, x] = new Tile_V2();

                    tiles[y, x].maxEnergy = fertility;
                    tiles[y, x].currentEnergy = fertility;


                    HSBColor color = HSBColor.HSBCOLOR_BLACK;

                    if (fertility > 0.05f)
                        tiles[y, x].type = Tile_V2.TILE_FERT;
                    else if (waterType < 0.5f)
                        tiles[y, x].type = Tile_V2.TILE_INFERT;
                    else
                        tiles[y, x].type = Tile_V2.TILE_WATER;

                    if (tiles[y, x].type == Tile_V2.TILE_FERT)
                    {
                        color.h = Mathf.Max(0f, climateType);
                        color.s = tiles[y, x].currentEnergy;
                        color.b = 1f - (0.25f - (color.s * 0.25f));
                    }
                    else if (tiles[y, x].type == Tile_V2.TILE_INFERT)
                    {
                        color = HSBColor.HSBCOLOR_WHITE;
                    }
                    else if (tiles[y, x].type == Tile_V2.TILE_WATER)
                    {
                        color = HSBColor.HSBCOLOR_BLACK;
                    }

                    tiles[y, x].detail = color;
                    texture.SetPixel(x, y, tiles[y, x].detail.ToColor());
                }
            }
            texture.Apply();
        }*/
    }

    public float GetWorldDeltaTime()
    {
        return worldDeltaTime;
    }

    public HSBColor GetColor(int x, int y)
    {
        /*if (IsValidLocation(x, y) == true)
            return tiles[y, x].detail;*/

        return HSBColor.FromColor(Color.black);

    }
    
    public float Eat(int x, int y)
    {
        float energy = 0;

        /*if (IsValidLocation(x, y) == true)
        {
            if (tiles[y, x].currentEnergy > 0)
            {
                energy = worldDeltaTime *10f;

                tiles[y, x].currentEnergy -= energy;

                if (tiles[y, x].currentEnergy < 0)
                {
                    energy += tiles[y, x].currentEnergy;
                    tiles[y, x].currentEnergy = 0;
                }


            }
        }*/

        return energy;


    }

    public bool IsValidLocation(int x, int y)
    {
        if (x < 0 || x > sizeX - 1 || y < 0 || y > sizeY - 1)
            return false;

        return true;
    }

    public int[] RandomFloorTile()
    {
        //return floorTiles[UnityEngine.Random.Range(0, floorTiles.Count)];
        return new int[] {UnityEngine.Random.Range(0,sizeY), UnityEngine.Random.Range(0, sizeX) };
    }

    public int GetTileType(int x, int y)
    {
        /*if (IsValidLocation(x, y) == true)
        {
            return tiles[y, x].type;
        }*/
        return Tile_V2.TILE_WATER;
    }

    public float GetTileEnergy(int x, int y)
    {
        /*if (IsValidLocation(x, y) == true)
        {
            return tiles[y, x].currentEnergy;
        }*/
        return 0f;
    }

    public void RemoveCreatureFromTileList(int x, int y, Creature_V2 creature)
    {
        if (IsValidLocation(x, y) == true)
        {
            int index = tiles[y, x].creatureListOnTile.IndexOf(creature);
            if (index != -1)
            {
                tiles[y, x].creatureListOnTile.RemoveAt(index);
            }
        }
    }

    public void AddCreatureToTileList(int x, int y, Creature_V2 creature)
    {
        if (IsValidLocation(x, y) == true)
        {
            tiles[y, x].creatureListOnTile.Add(creature);
        }
    }

    // search in a 3x by 3x grid, (8 searches)
    public List<List<Creature_V2>> ExistCreaturesNearTile(int x, int y)
    {
        List<List<Creature_V2>> creatureIndexList = new List<List<Creature_V2>>();
        if (IsValidLocation(x, y) == true)
        {
            for (int i = y - 1; i < y + 1; i++)
            {
                for (int j = x - 1; j < x + 1; j++)
                {
                    if (IsValidLocation(j, i) == true)
                    {
                        List<Creature_V2> list = tiles[i, j].creatureListOnTile;
                        
                        creatureIndexList.Add(tiles[i, j].creatureListOnTile);
                    }
                }
            }
        }

        return creatureIndexList;
    }

    //search in only the given grid, (1 search)
    public List<Creature_V2> ExistCreatureAtTile(int x, int y)
    {
        if (IsValidLocation(x, y) == true)
        {
            return tiles[y, x].creatureListOnTile;
        }
        return null;
    }

    public string TileToString(int x, int y)
    {
        return x + "," + y + "\nE: " + String.Format("{0:###.00}", tiles[y, x].currentEnergy*100f) + "\nC: " + String.Format("{0:###.00}", climate + "\nT: "+tiles[y,x].type);
    }
}
