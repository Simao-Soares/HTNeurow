using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PresetFiller : MonoBehaviour
{
    public GameObject canvas;
    public Toggle editButton;

    [HideInInspector]public bool updateSettings = false;
    [HideInInspector]public bool editAux = false;

    [System.Serializable]
    public class SettingsPreset
    {
        public string description;

        public int Gender;
        public int ControlMethod;
        public int HemiLimb;

        public float motionRange;
        public float colliderSize;
        public int trackAxis;

        public float turnAngle;
        public float boatSpeed;
        public float turnSpeed;
        public int turnSense;    //---------------------------------------------------------------------------------------------------------------------->
        public bool invertTurn;

        public int challengeLevel;
        public int angleDev;
        public float maxDistance;
        public float maxDistance2;
        public int selfCorrect;
        public float autoCorrect;

        public int playArea;
        public int objectiveNum;
        public float objectiveRad;
    }

    [System.Serializable]
    public class PresetsList
    {
        public SettingsPreset[] Presets;
    }

    public PresetsList myPresets = new PresetsList();


    private void Start()
    {
        //SetPreset(0);
    }


    void Update()
    {
        Debug.Log(editAux);
        if (updateSettings)
        {
            canvas.GetComponent<MenuScript>().updateSettingsAux = true;
        }
        updateSettings = false;

        if (editAux)
        {
            Debug.Log("saving");
            SavePreset();
        }
    }


    public void SetToEdit()
    {
        if (GameManager.SelectedPreset != -1)
        {
            if (editAux)
            {
                editAux = false;
                updateSettings = true;
            }
            else
            {
                editAux = true;
                SavePreset();
            }
        }
        else
        {
            Debug.Log("First choose the preset you want to edit");

        }
    }

    public void SetPreset(int index)
    {
        //Debug.Log("index " + index);
        editAux = false;

        GameManager.SelectedPreset = index;

        GameManager.Gender = myPresets.Presets[index].Gender;
        GameManager.ControlMethod = myPresets.Presets[index].ControlMethod;
        GameManager.HemiLimb = myPresets.Presets[index].HemiLimb;

        GameManager.motionRange = myPresets.Presets[index].motionRange;
        GameManager.colliderSize = myPresets.Presets[index].colliderSize;
        GameManager.trackAxis = myPresets.Presets[index].trackAxis;

        GameManager.turnAngle = myPresets.Presets[index].turnAngle;
        GameManager.boatSpeed = myPresets.Presets[index].boatSpeed;
        GameManager.turnSpeed = myPresets.Presets[index].turnSpeed;
        GameManager.turnSense = myPresets.Presets[index].turnSense;
        GameManager.invertTurn = myPresets.Presets[index].invertTurn;

        GameManager.challengeLevel = myPresets.Presets[index].challengeLevel;
        GameManager.angleDev = myPresets.Presets[index].angleDev;
        GameManager.maxDistance = myPresets.Presets[index].maxDistance;
        GameManager.maxDistance2 = myPresets.Presets[index].maxDistance2;
        GameManager.selfCorrect = myPresets.Presets[index].selfCorrect;
        GameManager.autoCorrect = myPresets.Presets[index].autoCorrect;

        GameManager.playArea = myPresets.Presets[index].playArea;
        GameManager.objectiveNum = myPresets.Presets[index].objectiveNum;
        GameManager.objectiveRad = myPresets.Presets[index].objectiveRad;

        updateSettings = true;
        
    }

    public void SavePreset()
    {
        var index = GameManager.SelectedPreset;

        myPresets.Presets[index].Gender = GameManager.Gender;
        myPresets.Presets[index].ControlMethod = GameManager.ControlMethod;
        myPresets.Presets[index].HemiLimb = GameManager.HemiLimb;

        myPresets.Presets[index].motionRange = GameManager.motionRange;
        myPresets.Presets[index].colliderSize = GameManager.colliderSize;
        myPresets.Presets[index].trackAxis = GameManager.trackAxis;

        myPresets.Presets[index].turnAngle = GameManager.turnAngle;
        myPresets.Presets[index].boatSpeed = GameManager.boatSpeed;
        myPresets.Presets[index].turnSpeed = GameManager.turnSpeed;
        myPresets.Presets[index].turnSense = GameManager.turnSense;
        myPresets.Presets[index].invertTurn = GameManager.invertTurn;

        myPresets.Presets[index].challengeLevel = GameManager.challengeLevel;
        myPresets.Presets[index].angleDev = GameManager.angleDev;
        myPresets.Presets[index].maxDistance = GameManager.maxDistance;
        myPresets.Presets[index].maxDistance2 = GameManager.maxDistance2;
        myPresets.Presets[index].selfCorrect = GameManager.selfCorrect;
        myPresets.Presets[index].autoCorrect = GameManager.autoCorrect;

        myPresets.Presets[index].playArea = GameManager.playArea;
        myPresets.Presets[index].objectiveNum = GameManager.objectiveNum;
        myPresets.Presets[index].objectiveRad = GameManager.objectiveRad;

        updateSettings = true;

    }

    public void PresetName_0(string input)
    {
        myPresets.Presets[0].description = input;
    }
    public void PresetName_1(string input)
    {
        myPresets.Presets[1].description = input;
    }
    public void PresetName_2(string input)
    {
        myPresets.Presets[2].description = input;
    }
    public void PresetName_3(string input)
    {
        myPresets.Presets[3].description = input;
    }
    public void PresetName_4(string input)
    {
        myPresets.Presets[4].description = input;
    }
    public void PresetName_5(string input)
    {
        myPresets.Presets[5].description = input;
    }



}
