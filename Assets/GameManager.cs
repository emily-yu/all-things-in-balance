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
        sliderObject = GameObject.Find("Real Slider");
        slider = GameObject.Find("Real Slider").GetComponent<Slider>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        currentCard = GameObject.Find("Card");
        mode = "placing";
        sliderObject.SetActive(false);
        isRotating = false;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetComponent<AudioSource>().Play();
        var speed = 10;
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentCard = hit.transform.gameObject;
                if (currentCard.transform.gameObject.tag == "card")
                {
                    mode = "dragging";
                    currentCard.GetComponent<Rigidbody>().detectCollisions = false;
                }

                Debug.Log("Hit");
            }

            else if (mode == "placing")
            {
                Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
                Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
                currentCard = Instantiate(enemy);
                currentCard.transform.position = adjustZ;
                currentCard.tag = "card";
                sliderObject.SetActive(true);
                sliderObject.transform.position = worldToUISpace(canvas, adjustZ + new Vector3(3, 0, 0));
                mode = "editing";
                isRotating = true;
                Debug.Log(sliderObject.transform.position);
            }

            else
            {

            }
        }
        //else if (Input.GetKeyDown(KeyCode.Mouse0)) { // if rotating, then don't add new mouse thing
        else if (mode=="editing") { 
            //Debug.Log("rotate");

            if (Input.GetKey(KeyCode.A))
            {
                Debug.Log("pressing left arrow");
                rotator.z = 30;
                currentCard.GetComponent<Rigidbody>().detectCollisions = false;


            }
            else if (Input.GetKey(KeyCode.D))
            {
                Debug.Log("pressing right arrow");
                rotator.z = -30;
                currentCard.GetComponent<Rigidbody>().detectCollisions = false;

            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Done rotating click to spawn another card");
                sliderObject.SetActive(false);
                currentCard.GetComponent<Rigidbody>().detectCollisions = true;
                mode = "placing";
            }
            else
            {
                rotator.z = 0;
            }
            currentCard.transform.Rotate(rotator * speed * Time.deltaTime); // [todo] add rotation
            // enemy.GetComponent<AudioSource>().Play();
        }

        else if (mode == "dragging")
        {
            Vector3 relPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(relPosition);
            Vector3 adjustZ = new Vector3(worldPoint.x, worldPoint.y, enemy.transform.position.z);
            currentCard.transform.position = adjustZ;
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

        // reload scene with key R
        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (mode == "dragging")
            {
                mass = currentCard.GetComponent<Rigidbody>().mass;
                slider.value = mass;
                sliderObject.SetActive(true);
                sliderObject.transform.position = worldToUISpace(canvas, currentCard.transform.position + new Vector3(3, 0, 0));
                Debug.Log("card clicked");
                mode = "editing";
                currentCard.GetComponent<Rigidbody>().detectCollisions = true;
            }
        }
        slider.onValueChanged.AddListener(setMass);
    }

    public void setMass(float newMass)
    {
        currentCard.GetComponent<Rigidbody>().mass = newMass;
        mass = newMass;
        Debug.Log("Hi");
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


}
 