using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomButtonSystem : MonoBehaviour
{
    private int currentButton;
    public Button[] buttonsArray;
    private int comboCounter = 0;
    public TextMeshProUGUI comboCounterText;
    public float cd;
    private double cdCounter;
    public Slider timeBar;

    private void Awake()
    {
        buttonsArray = new Button[transform.childCount];
    }

    private void Start()
    {
        currentButton = Random.Range(0, transform.parent.transform.childCount - 1);
        UpdateButtonArray();
        buttonsArray[currentButton].interactable = true;
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
    }

    public void NextButton()
    {
        int nextButton = Random.Range(0, transform.childCount-1);

        comboCounter++;

        while(nextButton == currentButton)
        {
            nextButton = Random.Range(0, transform.childCount - 1);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            buttonsArray[i].interactable = false;
        }

        cdCounter = cd;
        buttonsArray[nextButton].interactable = true;
        currentButton = nextButton;
    }

    private void UpdateButtonArray()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            buttonsArray[i] = transform.GetChild(i).GetComponent<Button>();
        }
    }
}
