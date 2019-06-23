using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;

public class MapConverter
{
    Tilemap map;
    char[,] array = new char[26,26];

    void FromMapToCSV()
    {
        Vector3Int grid = new Vector3Int(); 
        for (grid.x = -13; grid.x <13; grid.x++)
            for (grid.y = -13; grid.y <13; grid.y++)
            {
                TileBase tile = map.GetTile(grid);
                array[grid.x+13, grid.y+13] = tile.name.ToCharArray()[0];
                //switch (tile.name)
                //{
                //    case "brickwall":
                //        array[grid.x, grid.y] = 'b';
                //        break;
                //    case "steelwall":
                //        array[grid.x, grid.y] = 's';
                //        break;
                //    case "empty":
                //        array[grid.x, grid.y] = 'e';
                //        break;
                //    case "forest":
                //        array[grid.x, grid.y] = 'f';
                //        break;
                //    case "ice":
                //        array[grid.x, grid.y] = 'i';
                //        break;
                //    case "river":
                //        array[grid.x, grid.y] = 'r';
                //        break;

                //}
            }
    }

    void FromArrayToCSV(int stage)
    {
        string fileName = "\\Maps\\" + stage.ToString() + ".csv";
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
}
