using UnityEngine;
using System.Collections;

public abstract class VisualizationManager : MonoBehaviour
{

    public enum DATA_TYPE
    {
        temp,
        sound,
        pir,
        humid,
        light,
        all
    }

    public float TIMEOUT_VALUE = 1.0f;

    public DATA_TYPE managerType;

    protected ArrayList components;
    protected ArrayList values;
    protected ArrayList newValues;
    protected float timeout;

    // Use this for initialization
    public virtual void Start()
    {
        timeout = TIMEOUT_VALUE;

        components = new ArrayList();
        values = new ArrayList();
        newValues = new ArrayList();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        timeout -= Time.deltaTime;

        if (timeout < 0.0f)
        {
            timeout = TIMEOUT_VALUE;
            UpdateVisualization();
        }
    }

    public virtual void UpdateValues(object value)
    {
        if (values == null)
        {
            values = new ArrayList();
        }

        if (newValues == null)
        {
            newValues = new ArrayList();
        }

        var data = new DataModel(System.DateTime.Now, value);
        newValues.Add(data);
    }

    public void AddComponent(GameObject component)
    {
        components.Add(component);
    }

    public void RemoveComponent(GameObject component)
    {
        components.Remove(component);
    }

    public virtual void UpdateVisualization()
    {
        foreach (object item in newValues)
        {
            values.Add(item);
        }
        newValues.Clear();

        // keep only last 10 items
        ArrayList updatedKeepValues = new ArrayList();
        for (int i = values.Count - 1; i > values.Count - 11; i--)
        {
            if (i < 0) break;

            updatedKeepValues.Add(values[i]);
        }
        updatedKeepValues.Sort();
        values = updatedKeepValues;
    }
}
