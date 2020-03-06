using UnityEngine;

public class HexTileGenerator : MonoBehaviour
{
    [SerializeField] int mapWidth = 25;
    [SerializeField] int mapHeight = 12;
    public GameObject hexTilePrefab;

    //desired distance position
    float titleXOffset = 1.8f;
    float titleZOffset = 1.565f;
    public static HexTileGenerator instance = null;

    //public float gap = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        CreateHexTileMap();
        //AddGap();
    }

    //void AddGap()
    //{
    //    titleXOffset += titleXOffset * gap;
    //    titleZOffset += titleZOffset * gap;
    //}

    // Update is called once per frame
    void CreateHexTileMap()
    {
        for (int x = 0; x <= mapWidth; x++)
        {
            for(int z = 0; z <= mapHeight; z++)
            {
                GameObject tempGo = Instantiate(hexTilePrefab);

                if (z % 2 == 0)
                {
                    //construct placement 
                    tempGo.transform.position = new Vector3(x * titleXOffset, 0, z * titleZOffset);
                }
                else
                {
                    tempGo.transform.position = new Vector3(x * titleXOffset +  titleZOffset / 2, 0, z * titleZOffset);
                }
                SetTitleInfo(tempGo, x, z);
            }
        }
    }
    //For instantiate cordinate placements
    void SetTitleInfo( GameObject GO, int x, int z)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + "," + z.ToString();
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }

}
