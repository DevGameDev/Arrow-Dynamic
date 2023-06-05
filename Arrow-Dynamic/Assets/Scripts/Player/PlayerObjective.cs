using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public enum ObjectiveTypes
{
    EscapeMines,
    ProgressCaverns,
    FindSourceOfPower,
    EscapeTemple,
    FindStatue,
    FindSunset,
    GameOver
}

[Serializable]
public class PlayerObjective : MonoBehaviour
{
    public ObjectiveTypes objective;
    private bool triggered = false;

    private static Dictionary<ObjectiveTypes, string> objectiveSummaries = new Dictionary<ObjectiveTypes, string>();
    private static bool objectivesSetup = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            if (objective == ObjectiveTypes.GameOver)
            StartCoroutine(UIManager.Instance.GameFinishSequence());
            StartCoroutine(UIManager.Instance.UpdateObjectiveText(objectiveSummaries[objective]));
            triggered = true;
        }
    }

    private void Start()
    {
        if (!objectivesSetup)
        {
            objectiveSummaries[ObjectiveTypes.EscapeMines] = "Find a way out of the tunnels";
            objectiveSummaries[ObjectiveTypes.ProgressCaverns] = "Progress through the crystal caverns";
            objectiveSummaries[ObjectiveTypes.FindSourceOfPower] = "Ascend through the chamber";
            objectiveSummaries[ObjectiveTypes.EscapeTemple] = "Find a way out of the Temple";
            objectiveSummaries[ObjectiveTypes.FindStatue] = "Find the Great Statue";
            objectiveSummaries[ObjectiveTypes.FindSunset] = "Reach the sunset";
            objectiveSummaries[ObjectiveTypes.GameOver] = "";
            objectivesSetup = true;
        }
    }
}