using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject enemy;
    public GameObject sliderObject;
    public Slider slider;
    public GameObject[] cards;
    public GameObject currentCard;
    public string mode;
    public float mass;
    private AudioSource audio;
    // public AudioClip resetSound;
    public bool isRotating;
    public Vector3 rotator;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity = Vector3.zero; // init no gravity
        sliderObject = GameObject.Find("Slider");
        slider = GameObject.Find("Slider").GetComponent<Slider>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        currentCard = GameObject.Find("Card");
        mode = "placing";
        sliderObject.SetActive(false);
        isRotating = false;
        audio = GetComponent<AudioSource>();
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<AudioSource>().Play();
        var speed = 10;
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isRotating) {
            // if isn't rotating, then can add new block
            Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
            Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
            //Instantiate(enemy).transform.position = adjustZ;

            currentCard = Instantiate(enemy);
            currentCard.transform.position = adjustZ;

            isRotating = true;
        }
        //else if (Input.GetKeyDown(KeyCode.Mouse0)) { // if rotating, then don't add new mouse thing
        else if (isRotating) { 
            //Debug.Log("rotate");

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Debug.Log("pressing left arrow");
                rotator.z = 30;
                isRotating = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("pressing right arrow");
                rotator.z = -30;
                isRotating = true;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Debug.Log("Done rotating click to spawn another card");
                isRotating = false;
            }
            else
            {
                rotator.z = 0;
            }
            currentCard.transform.Rotate(rotator * speed * Time.deltaTime); // [todo] add rotation
            // enemy.GetComponent<AudioSource>().Play();
        }
        //else if (Input.GetKeyDown(KeyCode.Return) && isRotating) {
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("release gravity");
            Physics.gravity = new Vector3(0, -10, 0); // reset to standard gravity
            audio.Play();

            // add scoring functionality here
            Debug.Log("score");

            //Thread.Sleep(5000); // let it just fall
            isRotating = false;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            if (mode == "placing")
            {
                mode = "editing";
            }
            else
            {
                sliderObject.SetActive(false);
                mode = "placing";
            }
        }
        slider.onValueChanged.AddListener(setMass);
    }

    public void setMass(float newMass)
    {
        currentCard.GetComponent<Rigidbody>().mass = newMass;
        mass = newMass;

        // reload scene with key R
        if (Input.GetKeyDown(KeyCode.R)){
             Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
         }
    }
}
 