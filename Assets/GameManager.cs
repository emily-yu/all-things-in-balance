using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentCard = hit.transform.gameObject;
                if (currentCard.transform.gameObject.tag == "card")
                {
                    mode = "editing";
                    mass = currentCard.GetComponent<Rigidbody>().mass;
                    slider.value = mass;
                    sliderObject.SetActive(true);
                    sliderObject.transform.position = worldToUISpace(canvas, currentCard.transform.position + new Vector3(3, 0, 0));
                    Debug.Log("card clicked");
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
            }

            else
            {
             
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space)) {
            Debug.Log("Space");
            Physics.gravity = new Vector3(0, -10, 0); // reset to standard gravity
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
    }
}
 