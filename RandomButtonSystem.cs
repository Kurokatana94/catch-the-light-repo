using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomButtonSystem : MonoBehaviour
{
    [Header("References")]
    public GameObject badButtons;
    public GameObject pairButtons;

    private GameObject[] buttonsArray;
    private GameObject[] pairButtonsArray;
    private GameObject[] badButtonsArray;
    
    //Variables needed to keep count of the number of buttons pressed in succession without making mistakes or finishing the time.
    private int comboCounter = 0;
    [Tooltip("Reference to the text element used to give  the user a combo feedback")]
    public TextMeshProUGUI comboCounterText;

    //Variables needed to keep track and show the user the amount of time left before the combo will be broken.
    public float cd;
    private double cdCounter;
    [Tooltip("Reference to the slider that show amount of time left to the user")]
    public Slider timeBar;

    //Variable needed to check which button is curretnly active to avoid an overlap on the next button once pressed
    private int currentButton;

    //Varibale needed to keep track of how many buttons are being pressed when the pair buttons are activated
    public int pairButtonCount;

    private void Awake()
    {
        buttonsArray = new GameObject[transform.childCount];
        badButtonsArray = new GameObject[transform.childCount];
        pairButtonsArray = new GameObject[transform.childCount];
    }

    private void Start()
    {
        currentButton = Random.Range(0, transform.parent.transform.childCount - 1);
        UpdateButtonArray();
        buttonsArray[currentButton].SetActive(true);
        cdCounter = cd;
        timeBar.maxValue = (float)cd;
    }

    private void FixedUpdate()
    {
        cdCounter -= Time.fixedDeltaTime;
    }

    private void Update()
    {
        comboCounterText.text = "Combo x " + comboCounter.ToString();
        if(cdCounter <= 0)
        {
            comboCounter = 0;
            cdCounter = cd;
        }

        timeBar.value = (float)cdCounter;

        if(pairButtonCount == 2)
        {
            NextButton();
        }

        pairButtonCount = 0;
    }

    public void NextButton()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            buttonsArray[i].SetActive(false);
            badButtonsArray[i].SetActive(false);
            pairButtonsArray[i].SetActive(false);
        }

        int n = Random.Range(1, 100);

        if (n <= 50)
        {
            OneButton();
        }else if(n>50 && n <= 75)
        {
            TwoButtons();
        }
        else
        {
            TrapButtons();
        }

        cdCounter = cd;
        comboCounter++;
    }

    public void BadButton()
    {
        comboCounter = 0;
        NextButton();
    }

    public void PairButton()
    {
        pairButtonCount++;
        Debug.Log("" + pairButtonCount);
    }

    public void WrongButton()
    {
        cdCounter = cd;
        comboCounter = 0;
    }

    private void OneButton()
    {
        int nextButton = Random.Range(0, transform.childCount - 1);

        while (nextButton == currentButton)
        {
            nextButton = Random.Range(0, transform.childCount - 1);
        }
        buttonsArray[nextButton].SetActive(true);
        currentButton = nextButton;
    }

    private void TrapButtons()
    {
        int nextButton = Random.Range(0, transform.childCount - 1);
        int badButton = Random.Range(0, transform.childCount - 1);

        while (nextButton == currentButton || nextButton == badButton || badButton == currentButton)
        {
            if(nextButton == currentButton)
            {
                nextButton = Random.Range(0, transform.childCount - 1);
            }

            if(nextButton == badButton || badButton == currentButton)
            {
                badButton = Random.Range(0, transform.childCount - 1);
            }
        }
        buttonsArray[nextButton].SetActive(true);
        badButtonsArray[badButton].SetActive(true);

        currentButton = nextButton;
    }

    private void TwoButtons()
    {
        int nextButton1 = Random.Range(0, transform.childCount - 1);
        int nextButton2 = Random.Range(0, transform.childCount - 1);

        while(nextButton1 == nextButton2 || nextButton1 == currentButton || nextButton2 == currentButton)
        {
            if(nextButton1 == currentButton)
            {
                nextButton1 = Random.Range(0, transform.childCount - 1);
            }
            
            if(nextButton1 == nextButton2 || nextButton2 == currentButton)
            {
                nextButton2 = Random.Range(0, transform.childCount - 1);
            }
        }
        pairButtonsArray[nextButton1].SetActive(true);
        pairButtonsArray[nextButton2].SetActive(true);

        currentButton = nextButton1;
    }


    private void UpdateButtonArray()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttonsArray[i] = transform.GetChild(i).gameObject;
            badButtonsArray[i] = badButtons.transform.GetChild(i).gameObject;
            pairButtonsArray[i] = pairButtons.transform.GetChild(i).gameObject;
        }
    }
}
