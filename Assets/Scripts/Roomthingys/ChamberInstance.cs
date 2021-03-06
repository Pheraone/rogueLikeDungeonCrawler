﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChamberInstance : MonoBehaviour
{

    public Texture2D tex;

    [HideInInspector]
    public Vector2 gridPosition;

    public int type; //Levelgenerator -> 0 = normal, 1 = enter

    [HideInInspector]
    public bool doorTop, doorBot, doorLeft, doorRight;

    [SerializeField]
    GameObject doorU, doorD, doorL, doorR, doorWall;

    [SerializeField]
    ColorToGameObject[] mappings;

    float tileSize = 16;
    Vector2 roomSizeInTiles = new Vector2(9, 17);

    //[SerializeField] public static readonly Color FirstColour;
    //public static readonly Color SecondColour = new Color(0, 0, 0, 1);

    public void Setup(Texture2D tex, Vector2 gridPosition, int type, bool doorTop, bool doorBot, bool doorLeft, bool doorRight)
    {
        this.tex = tex;
        this.gridPosition = gridPosition;
        this.type = type;
        this.doorTop = doorTop;
        this.doorBot = doorBot;
        this.doorLeft = doorLeft;
        this.doorRight = doorRight;
        MakeDoors();
        GenerateRoomTiles();

    }

    public void MakeDoors()
    {
        //possibilties for doors
        Vector3 spawnPosition = transform.position + Vector3.up * (roomSizeInTiles.y / 4 * tileSize) - Vector3.up * (tileSize / 4);
        PlaceDoor(spawnPosition, doorTop, doorU);

        spawnPosition = transform.position + Vector3.down * (roomSizeInTiles.y / 4 * tileSize) - Vector3.down * (tileSize / 4);
        PlaceDoor(spawnPosition, doorBot, doorD);

        spawnPosition = transform.position + Vector3.right * (roomSizeInTiles.x * tileSize) - Vector3.right * (tileSize);
        PlaceDoor(spawnPosition, doorRight, doorR);

        spawnPosition = transform.position + Vector3.left * (roomSizeInTiles.x * tileSize) - Vector3.left * (tileSize);
        PlaceDoor(spawnPosition, doorLeft, doorL);
    }


    void PlaceDoor(Vector3 spawnPosition, bool door, GameObject doorSpawn)
    {
        //spawn doors and walls
        if (door)
        {
            Instantiate(doorSpawn, spawnPosition, Quaternion.identity).transform.parent = transform;
        }
        else
        {
            Instantiate(doorWall, spawnPosition, Quaternion.identity).transform.parent = transform;
        }
    }

    void GenerateRoomTiles()
    {
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                GenerateTile(x, y);
            }
        }
    }

    void GenerateTile(int x, int y)
    {

      


        Color pixel = tex.GetPixel(x, y);

        if (pixel.a == 0)
        {
            return;
        }
        foreach (ColorToGameObject mapping in mappings)
        {
            Debug.Log("mappingColor = " + mapping.color + "  pixelcolor = " + pixel);
            //if (mapping.color.Equals(pixel))
            if (mapping.color.r == pixel.r && mapping.color.g == pixel.g && mapping.color.b == pixel.b)
            {
               

                Vector3 spawnPosition = PositionFromTileGrid(x, y);
                Instantiate(mapping.prefab, spawnPosition, Quaternion.identity).transform.parent = this.transform;
            }
            else
            {

            }
        }
    }


        Vector3 PositionFromTileGrid(int x, int y)
        {
            Vector3 ret;
            Vector3 offset = new Vector3((-roomSizeInTiles.x + 1) * tileSize, (roomSizeInTiles.y / 4) * tileSize - (tileSize / 4), 0);
            ret = new Vector3(tileSize * (float)x, -tileSize * (float)y, 0) + offset + transform.position;
            return ret;
        }
    }

