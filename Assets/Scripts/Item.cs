using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace JY
{
    public class ItemData
    {
        public string Name = string.Empty;
        public ModelType ModelType;
        public int HeightCount = -1;
        public Vector3 ItemPos = Vector3.zero;
        public bool IsPosition = false;

        public ItemData(string s, ModelType type, int heightCnt, Vector3 pos, bool isPos)
        {
            if (!string.IsNullOrWhiteSpace(s))
                Name = s;

            ModelType = type;
            HeightCount = heightCnt;
            ItemPos = pos;
            IsPosition = isPos;
        }
    }

    public class Item : MonoBehaviour
    {
        public RectTransform rt;
        public Text nameText;
        public Button btn;

        #region Property
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

        private Vector3 itemPos = Vector3.zero;
        public Vector3 ItemPos {
            get { return itemPos; }
            set { itemPos = value; }
        }

        private bool isPosition = false;
        public bool IsPosition {
            get { return isPosition; }
            set { isPosition = value; }
        }
        #endregion

        public void SetData(string s, ModelType type, int heightCnt = -1)
        {
            //Debug.Log($"name : {s}, ModelType : {type}, heightCnt : {heightCnt}");

            _name = s;
            name = s;
            modelType = type;

            if (heightCnt >= 0)
                heightCount = heightCnt;

            nameText.text = $"{_name}_{modelType}";
        }

        public void SetData(string s, ModelType type, int heightCnt, Vector3 pos, bool isPos)
        {
            //Debug.Log($"name : {s}, ModelType : {type}, heightCnt : {heightCnt}");

            _name = s;
            name = s;
            modelType = type;

            if(heightCnt >= 0)
                heightCount = heightCnt;

            itemPos = pos;
            isPosition = isPos;

            nameText.text = $"{_name}_{modelType}";
        }

        public void SetScale(float x, float y)
        {
            rt.sizeDelta = new Vector2(x, y);
        }

        public void SetPosition(bool isLocal, float x, float y)
        {
            if (isLocal)
            {
                rt.localPosition = new Vector3(x, y);

                if (!isPosition)
                {
                    ItemPos = rt.localPosition;
                    isPosition = true;
                }
            }
            else
            {
                rt.position = new Vector3(x, y);
            }
        }
    }
}