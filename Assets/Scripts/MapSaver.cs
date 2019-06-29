using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEngine.UI;

public class MapSaver : MonoBehaviour
{
    public Tilemap map;
    public Button saveButton;
    public InputField stageInput;


    private void Start()
    {
        saveButton.onClick.AddListener(SaveMap);
    }

    void SaveMap()
    {
        char[,] array = new char[26,26];
        //FromMapToArray
        Vector3Int grid = new Vector3Int(); 
        for (grid.x = -13; grid.x <13; grid.x++)
            for (grid.y = -13; grid.y <13; grid.y++)
            {
                TileBase tile = map.GetTile(grid);
                array[grid.x+13, grid.y+13] = tile.name.ToCharArray()[0];
            }

        //FromArrayToCSV
        print("Stage Inputfield: " + stageInput.text);
        int stage = int.Parse(stageInput.text);
        string fileName = @"J:\My Projects\90Tank\Assets\Maps\stage" + stage.ToString() + ".csv";
        using (StreamWriter fileWriter = new StreamWriter(fileName, false, System.Text.Encoding.ASCII))
        {
            for (int j = 0; j < 26; j++)
            {
                for(int i = 0; i< 26;i++)
                    fileWriter.Write("{0},", array[i,j]);
                fileWriter.Write("\r\n");
            }
        }
        print("Map saved");
    }

}
