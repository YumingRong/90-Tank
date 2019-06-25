using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public Tilemap map;
    char[,] array = new char[26,26];

    public Tile emptyTile, brickTile, steelTile, riverTile, forestTile, iceTile;
    public Image woodImage;
    private void Start()
    {
        int stage = GameManager.GetInstance().stage;
        FromCSVToMap(stage);
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Submit"))
        //    FromMapToCSV(2);
    }

    void FromMapToCSV(int stage)
    {
        Vector3Int grid = new Vector3Int(); 
        for (grid.x = -13; grid.x <13; grid.x++)
            for (grid.y = -13; grid.y <13; grid.y++)
            {
                TileBase tile = map.GetTile(grid);
                array[grid.x+13, grid.y+13] = tile.name.ToCharArray()[0];
            }
        FromArrayToCSV(stage);
        print("Map saved");
    }

    public void FromArrayToCSV(int stage)
    {
        string fileName = @"J:\My Projects\90Tank\Assets\Maps\" + stage.ToString() + ".csv";
        using (StreamWriter fileWriter = new StreamWriter(fileName, true, System.Text.Encoding.ASCII))
        {
            for (int j = 0; j < 26; j++)
            {
                for(int i = 0; i< 26;i++)
                    fileWriter.Write("{0},", array[i,j]);
                fileWriter.Write("\r\n");
            }
        }
    }

    void FromCSVToMap(int stage)
    {
        //surrounding steel wall
        map.BoxFill(new Vector3Int(-14,-14,0), steelTile, -14, -14, 13, 13);

        string fileName = @"J:\My Projects\90Tank\Assets\Maps\" + stage.ToString() + ".csv";
        try
        {
            using (StreamReader fileReader = new StreamReader(fileName, System.Text.Encoding.ASCII))
            {
                char[] b = new char[52];
                Tile tile = emptyTile;
                Vector3Int grid = new Vector3Int();
                for (int j = 0; j < 26; j++)
                {
                    string line = fileReader.ReadLine();
                    StringReader sr = new StringReader(line);
                    sr.Read(b, 0, 52);
                    for (int i = 0; i < 26; i++)
                    {
                        char c = b[i * 2];
                        if (c == 'e')
                            tile = emptyTile;
                        else if (c == 'b')
                            tile = brickTile;
                        else if (c == 's')
                            tile = steelTile;
                        else if (c == 'i')
                            tile = iceTile;
                        else if (c == 'f')
                            tile = forestTile;
                        else if (c == 'r')
                            tile = riverTile;
                        grid.x = i - 13;
                        grid.y = j - 13;
                        map.SetTile(grid, tile);
                    }
                }
            }
        }
        catch (IOException e)
        {
            print("The file could not be read:");
            print(e.Message);
        }
        print("Map loaded");
    }
}
