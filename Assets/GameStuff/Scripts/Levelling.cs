using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Levelling : MonoBehaviour
{
    public int maxLevel;
    public float[] milestones;

    [Header("UI that shows the current level of the object (length > 0)")]
    public TextMeshProUGUI[] levelTexts;
    [Header("UI that shows the current exp of the object (optional)")]
    public TextMeshProUGUI[] expTexts;

    private Camera cam;
    private Canvas canvas;

    [HideInInspector] public int currentLevel { get; private set; } = 1;
    private int currentMilestoneIndex;
    private float experience;

    public delegate void LevelUpHandler(int level); //creating a delegate, so that other scripts can subscribe to it
    public event LevelUpHandler OnLevelUp;

    private void Awake()
    {
        cam = Camera.main;
        canvas = levelTexts[0].canvas;
        if (milestones.Length == 0)
        {
            milestones = new float[maxLevel];
        }
        if (milestones[0] == 0)
        {
            milestones[0] = 1;
        }
        for (int i = 1; i < milestones.Length; i++)
        {
            if (milestones[i] == 0)
            {
                milestones[i] = milestones[i - 1] * 1.414f;
                if (i == maxLevel - 1)
                {
                    milestones[i] = milestones[i-1] * 2 - milestones[i - 2];
                }
            }
        }
        ChangeLevelText();
        ChangeExpText();
    }
    private void Update()
    {
        canvas.transform.LookAt(canvas.transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
    }
    private void LevelUp()
    {
        currentMilestoneIndex++;
        currentLevel++;
        ChangeLevelText();
        if (OnLevelUp != null) //calling every OnLevelUp that is subscribed to this event
        {
            OnLevelUp(currentLevel); //giving the information of the current level to all methods
        }
    }
    public void AddExp(float amount)
    {
        if (currentMilestoneIndex == maxLevel-1)
        {
            return;
        }
        experience += amount;
        if (experience >= milestones[currentMilestoneIndex])
        {
            LevelUp();
        }
        ChangeExpText();
    }
    private void ChangeLevelText()
    {
        string textToDisplay = "Level " + currentLevel.ToString();
        for (int i = 0; i < levelTexts.Length; i++)
        {
            if (levelTexts[i] != null)
            {
                levelTexts[i].text = textToDisplay;
            }
        }

    }
    private void ChangeExpText()
    {
        float expOffset = currentMilestoneIndex == 0? 0 : milestones[currentMilestoneIndex - 1];
        string expToDisplay = System.Math.Round(experience - expOffset,1).ToString();
        string milestoneToDisplay = System.Math.Round(milestones[currentMilestoneIndex] - expOffset, 1).ToString();
        if (currentMilestoneIndex == maxLevel-1)
        {
            expToDisplay = milestoneToDisplay;
        }
        string textToDisplay = expToDisplay + " / " + milestoneToDisplay;
        for (int i = 0; i < expTexts.Length; i++)
        {
            if (expTexts[i] != null)
            {
                expTexts[i].text = textToDisplay;
            }
        }
    }
}
