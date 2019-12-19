using UnityEngine;
using System.Collections;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

    private float CEILING_HEIGHT = 4.5f;
    private const float ITEM_HEIGHT = 0.55f;
	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	//public GameObject Pillar = null;
    public GameObject Door = null;
    public GameObject Key = null;

	public int Rows = 200;
	public int Columns = 200;
	public const float CellWidth = 5;
	public const float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject Bad = null;
    public GameObject Stretcher = null;
    public GameObject Ethan = null;
    public GameObject SD = null;
    public GameObject Lamp = null;
    public GameObject Atom = null;
    public GameObject Bucky = null;
    public GameObject Eye = null;
    public GameObject Split = null;
    public GameObject Crawler = null;
    public GameObject Scarecrow = null;
    public GameObject Capsule;

    private const int CrawlerCount = 5;
    private const int ScarecrowCount = 1;


	private BasicMazeGenerator mMazeGenerator = null;

    void Start()
    {
        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }

        int scarecrowAdded = 0;
        int crawlerAdded = 0;
        mMazeGenerator.GenerateMaze();
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                //tmp = Instantiate(Floor, new Vector3(x, CEILING_HEIGHT, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                //tmp.transform.parent = transform;
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                }
                if (cell.IsKey)
                {
                    tmp = Instantiate(Key, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(90, -45, 0)) as GameObject;// key
                    tmp.transform.parent = transform;
                    continue;
                }

                //if (cell.IsDoor)
                //{
                //    tmp = Instantiate(Door, new Vector3(x, 0, z - CellHeight / 2), Quaternion.Euler(0, 180, 0)) as GameObject;// key
                //    tmp.transform.parent = transform;
                //    continue;
                //}

                int random = Random.Range(0, 11);

                if (random == 0 && Bad != null)
                {
                    tmp = Instantiate(Bad, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(-90, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 1 && Stretcher != null)
                {
                    tmp = Instantiate(Stretcher, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 2 && Ethan != null)
                {
                    tmp = Instantiate(Ethan, new Vector3(x, 0.2f, z), Quaternion.Euler(-90, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 3 && SD != null)
                {
                    tmp = Instantiate(SD, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(-90, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if ((random == 4 || random == 11) && Lamp != null)
                {
                    tmp = Instantiate(Lamp, new Vector3(x, 2.5f, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 5 && Atom != null)
                {
                    tmp = Instantiate(Atom, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 6 && Bucky != null)
                {
                    tmp = Instantiate(Bucky, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 7 && Eye != null)
                {
                    tmp = Instantiate(Eye, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(0, 90, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
                if (random == 8 && Split != null)
                {
                    tmp = Instantiate(Split, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(90, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }

                if (random > 9 && Crawler != null && crawlerAdded < CrawlerCount)
                {
                    crawlerAdded++;
                    tmp = Instantiate(Crawler, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    var comp = tmp.GetComponent<CrawlerMovement>();
                    comp.Init(mMazeGenerator, column, row, Capsule);
                }
                if (random == 10 && Scarecrow != null && scarecrowAdded < ScarecrowCount)
                {
                    tmp = Instantiate(Scarecrow, new Vector3(x, ITEM_HEIGHT, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
        //if (Pillar != null)
        //{
        //    for (int row = 0; row < Rows + 1; row++)
        //    {
        //        for (int column = 0; column < Columns + 1; column++)
        //        {
        //            float x = column * (CellWidth + (AddGaps ? .2f : 0));
        //            float z = row * (CellHeight + (AddGaps ? .2f : 0));
        //            GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
        //            tmp.transform.parent = transform;
        //        }
        //    }
        //}
        if (Door != null)
        {
            for (int row = 0; row < 1; row++)
            {
                for (int column = 0; column < 1; column++)
                {
                    float x = column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = row * (CellHeight + (AddGaps ? .2f : 0));
                    GameObject tmp = Instantiate(Door, new Vector3(x + 20, 2, -3), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }
    }
}


