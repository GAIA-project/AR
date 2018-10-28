using UnityEngine;
using System.Collections;
using ChartAndGraph;
using System;

public class TemperatureManager : VisualizationManager
{

    public Material barMaterial;

    DateTime initialDateTime;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        managerType = DATA_TYPE.temp;

        var comp = FindObjectOfType<VisualTemperature>().gameObject;
        base.AddComponent(comp);

        //var tempBarChart = FindObjectOfType<WorldSpaceGraphChart>();
        //tempBarChart.DataSource.RemoveCategory("Player 2");
        //base.AddComponent(tempBarChart.gameObject);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void UpdateValues(object value)
    {
        base.UpdateValues(value);

        if (initialDateTime == DateTime.MinValue)
        {
            initialDateTime = DateTime.Now;
        }
    }

    public override void UpdateVisualization()
    {
        if (newValues == null || newValues.Count == 0) 
        {
            //Debug.Log("No new values, skipping..");
            return;
        }

        int newValuesCount = newValues.Count;

        base.UpdateVisualization();

        // get 10th from last element
        int initalPosition = values.Count - 11;
        if (initalPosition < 0) initalPosition = 0;

        initialDateTime = ((DataModel)values[initalPosition]).dateTime;


        foreach (object comp in components)
        {
            //var text = ((GameObject)comp).GetComponent<VisualTemperature>();
            //if (text != null)
            //{
            //    text.UpdateText(data.val);
            //}

            var graph = ((GameObject)comp).GetComponent<WorldSpaceBarChart>();

            if (graph != null)
            {
                graph.DataSource.StartBatch();
                graph.DataSource.ClearCategories();
                graph.DataSource.ClearValues();
                graph.DataSource.MaxValue = 50.0f;

                //for (int i = values.Count - 1; i > values.Count - 11; i--)
                //{
                //if (i < 0) break;

                int index = 0;
                string timestamp;

                foreach (DataModel data in values)
                {
                    Debug.Log("DataModel: " + data);
                    timestamp = data.dateTime.ToString("HH:mm:ss");

                    graph.DataSource.AddCategory(timestamp, barMaterial);

                    if (index >= values.Count - newValuesCount)
                    {
                        graph.DataSource.SlideValue(timestamp, "All", data.val, 1f);
                    }
                    else
                    {
                        graph.DataSource.SetValue(timestamp, "All", data.val);
                    }

                    //graph.mData.Add(new DoubleVector2(data.dateTime.Ticks - initialDateTime.Ticks, data.val));
                    index++;
                }

                graph.DataSource.EndBatch();
            }
        }
    }
}
