using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JY;

namespace JY
{
    public class Item : MonoBehaviour
    {
        public RectTransform rt;
        public Text nameText;

        private string name;
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

        private ScrollRect m_scrollRect = null;

        public Item(string s, ModelType type, int heightCnt)
        {
            if (!string.IsNullOrWhiteSpace(s))
                name = s;

            modelType = type;
            heightCount = heightCnt;
        }

        public void SetData(string s, ModelType type, int heightCnt = 0)
        {
            name = s;
            modelType = type;
            
            if (!heightCnt.Equals(0))
                heightCount = heightCnt;

            nameText.text = name;
        }

        public void SetPosition(bool isLocal, float x, float y)
        {
            if (object.ReferenceEquals(null, rt))
                rt = GetComponent<RectTransform>();

            if (isLocal)
                rt.localPosition = new Vector3(x, y);
            else
                rt.position = new Vector3(x, y);
        }
    }
}