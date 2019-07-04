using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System.Collections;

public class MapLoader : MonoBehaviour
{
    public Tilemap map;
    public Tile emptyTile, brickTile, steelTile, riverTile, iceTile;
    public GameObject woods;

    // Start is called before the first frame update
    void Start()
    {
        int stage = GameManager.GetInstance().stage;
        LoadMap(stage);

    }

    void LoadMap(int stage)
    {
        //surrounding steel wall
        map.BoxFill(new Vector3Int(-14, -14, 0), steelTile, -14, -14, 13, 13);

        string fileName = @"J:\My Projects\90Tank\Assets\Maps\stage" + stage.ToString() + ".csv";
        print("Load map " + fileName);
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
                        GameObject wood;
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
                        {
                            //tile = forestTile;
                            tile = emptyTile;
                            wood = Object.Instantiate<GameObject>(woods);
                            wood.transform.position = new Vector3(0.25f * (i - 12.5f), 0.25f * (j - 12.5f));
                        }
                        else if (c == 'r')
                            tile = riverTile;
                        else
                        {
                            print("unknown tile:" + i + "," + j +","+ c);
                        }
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

    Vector3Int[] fence = { new Vector3Int(-2, -13, 0), new Vector3Int(1, -13, 0), new Vector3Int(-2, -12, 0), new Vector3Int(1, -12, 0),
                    new Vector3Int(-2,-11, 0), new Vector3Int(-1, -11, 0), new Vector3Int(0, -11, 0), new Vector3Int(1,-11, 0)};

    public void OnPrizeShovel()
    {
        foreach (Vector3Int v in fence)
            map.SetTile(v, steelTile);
        StartCoroutine(SwitchFence());
    }

    private IEnumerator SwitchFence()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i <3; i++)
        {
            yield return new WaitForSeconds(0.5f);
            foreach (Vector3Int v in fence)
                map.SetTile(v, steelTile);
            yield return new WaitForSeconds(0.5f);
            foreach (Vector3Int v in fence)
                map.SetTile(v, brickTile);
        }
    }

}
