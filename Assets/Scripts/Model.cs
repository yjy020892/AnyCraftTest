using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JY;

public class Model
{
    private string name;
    public string Name {
        get { return name; }
        set { name = value; }
    }

    private ModelType modelType;
    public ModelType ModelType {
        get { return modelType; }
        set { modelType = value; }
    }

    public Model(string s, ModelType type)
    {
        if (!string.IsNullOrWhiteSpace(s))
            name = s;

        modelType = type;
    }
}
