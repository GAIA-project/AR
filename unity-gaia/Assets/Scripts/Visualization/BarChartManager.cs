using System.Collections;
using System.Collections.Generic;
using ChartAndGraph;
using UnityEngine;

public class BarChartManager : VisualizationManager {

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        managerType = DATA_TYPE.temp;

        //var comp = FindObjectOfType<WorldSpaceGraphChart>().gameObject;
        //base.AddComponent(comp);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void UpdateVisualization()
    {
        //if (values == null || values.Count == 0) return;

        //foreach (object comp in components)
        //{
            //Debug.Log("Updating graph bar");

            //var graph = ((GameObject)comp).GetComponent<WorldSpaceGraphChart>();

            //graph.DataSource.StartBatch();
            //graph.DataSource.ClearCategory("Player 1");
            //graph.DataSource.RemoveCategory("Player 2");

            //for (int i = 0; i < 20; i++)
            //{
            //    graph.DataSource.AddPointToCategory("Player 1", Random.value * 10f, Random.value * 10f);

            //}

            //graph.DataSource.EndBatch();
        //}
    }
}
