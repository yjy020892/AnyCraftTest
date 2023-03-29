using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JY;
using System;

namespace JY
{
    public class ItemData
    {
        public string Name = string.Empty;
        public ModelType ModelType;
        public int HeightCount = 0;

        public ItemData(string s, ModelType type, int heightCnt)
        {
            if (!string.IsNullOrWhiteSpace(s))
                Name = s;

            ModelType = type;
            HeightCount = heightCnt;
        }
    }

    public class Item : MonoBehaviour
    {
        public RectTransform rt;
        public Text nameText;
        public Button btn;

        private string _name;
        public string Name {
            get { return name; }
            set { name = value; }
        }

        private ModelType modelType = ModelType.Sphere;
        public ModelType ModelType {
            get { return modelType; }
            set { modelType = value; }
        }

        [SerializeField] private int heightCount = 0;
        public int HeightCount {
            get { return heightCount; }
            set { heightCount = value; }
        }

        public void SetData(string s, ModelType type, int heightCnt = 0)
        {
            name = s;
            modelType = type;
            
            if (!heightCnt.Equals(0))
                heightCount = heightCnt;

            nameText.text = $"{name}_{modelType}";
        }

        public void SetPosition(bool isLocal, float x, float y)
        {
            if (isLocal)
                rt.localPosition = new Vector3(x, y);
            else
                rt.position = new Vector3(x, y);
        }
    }
}