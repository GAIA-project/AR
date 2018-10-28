using UnityEngine;
using ChartAndGraph;
using System;

public class GraphChartFeed : MonoBehaviour
{
	void Start ()
    {
        GraphChartBase graph = GetComponent<GraphChartBase>();
        if (graph != null)
        {
            graph.DataSource.StartBatch();
            graph.DataSource.ClearAndMakeBezierCurve("Temperature");
            for (int i = 0; i <30; i++)
            {
                if (i == 0)
                    graph.DataSource.SetCurveInitialPoint("Temperature", DateTime.Now, UnityEngine.Random.value * 10f + 10f);
                else
                    graph.DataSource.AddLinearCurveToCategory("Temperature", 
                                                              new DoubleVector2(ChartDateUtility.DateToValue(DateTime.Now) + i, UnityEngine.Random.value * 10f + 10f));
            }

            graph.DataSource.MakeCurveCategorySmooth("Temperature");
            graph.DataSource.EndBatch();
        }
    }
}
