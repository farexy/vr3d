using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MovingGoal : MonoBehaviour {

    [SerializeField] private string itemName;
    public float speed = 6.0f;
    private CharacterController _charController;
    public float gravity = -9.8f;


    public AudioClip CoinSound = null;
    public AudioClip ExitSound = null;

    private Rigidbody mRigidBody = null;
    private AudioSource mAudioSource = null;
    private const int ITEMS_TO_WIN = 6;
    // Use this for initialization
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        mRigidBody = GetComponent<Rigidbody>();
        mAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        //   transform.Translate(deltaX*Time.deltaTime, 0, deltaZ*Time.deltaTime);
        movement = Vector3.ClampMagnitude(movement, speed);
        movement.y = gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Goal") || other.gameObject.tag.Equals("Key"))
        {
            collectItem(other.gameObject);
        }
        if (other.gameObject.tag.Equals("Door"))
        {
            tryOpenDoor();
        }
    }

    void collectItem(GameObject item)
    {
        if (mAudioSource != null && CoinSound != null)
        {
            mAudioSource.PlayOneShot(CoinSound);
        }
        itemName = item.name.Substring(0, item.name.IndexOf('('));
        Debug.Log("Item Collected " + itemName);
        Managers.Inventory.AddItem(itemName);
        Destroy(item);
    }

    void tryOpenDoor()
    {
        
        if ((Managers.Inventory.Count() >= ITEMS_TO_WIN + 1) && Managers.Inventory.Contains("Key"))
            {
                //wining sound
                if (mAudioSource != null && ExitSound != null)
                {
                    mAudioSource.PlayOneShot(ExitSound);
                }

                //GameObject canvasEnd = new GameObject();
                //Text endGame = canvasEnd.AddComponent<Text>();
                //endGame.text = "You win!!! Game over.";

                //Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                //endGame.font = ArialFont;
                //endGame.material = ArialFont.material;
                //endGame.fontSize = 25;
                //endGame.color = Color.blue;


                SceneManager.LoadScene(3);
               
          }
    }
}

