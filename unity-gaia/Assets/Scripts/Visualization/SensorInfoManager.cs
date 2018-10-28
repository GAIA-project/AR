using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorInfoManager : VisualizationManager {

	// Use this for initialization
	public override void Start () {
        base.Start();

        //var comp = FindObjectOfType<SensorInfoVisual>().gameObject;
        //base.AddComponent(comp);
	}
	
	// Update is called once per frame
    public override void Update () {
        base.Update();

        managerType = DATA_TYPE.all;
	}

    public override void UpdateValues(object value)
    {
        base.UpdateValues(value);
    }

    public override void UpdateVisualization()
    {
        if (newValues == null || newValues.Count == 0)
        {
            //Debug.Log("No new values, skipping..");
            return;
        }

        base.UpdateVisualization();

        foreach (object comp in components)
        {
            GameObject obj = ((GameObject)comp).transform.Find("Plane").Find("LastUpdated").gameObject;
            var text = obj.GetComponent<TextMeshPro>();

            DataModel dataModel = (DataModel) values[values.Count - 1];
            text.text = "Last update: " + dataModel.dateTime.ToString();
        }
    }
}
