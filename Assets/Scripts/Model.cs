using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JY
{
    public class ModelData
    {
        public string Name = string.Empty;
        public ModelType ModelType;

        public ModelData(string s, ModelType type)
        {
            if (!string.IsNullOrWhiteSpace(s))
                Name = s;

            ModelType = type;
        }
    }
    public class Model : MonoBehaviour
    {
        Animator anim;

        float blendTreeValue = 0.0f;

        bool b_Plus = true;

        #region Property
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
        #endregion

        private void Start()
        {
            anim = GetComponent<Animator>();
        }

        private void Update()
        {
            if(b_Plus)
                blendTreeValue += (Time.deltaTime * 0.2f);
            else
                blendTreeValue -= (Time.deltaTime * 0.2f);

            if (blendTreeValue >= 1.0f)
            {
                blendTreeValue = 1.0f;
                b_Plus = false;
            }
            else if (blendTreeValue <= 0)
            {
                blendTreeValue = 0.0f;
                b_Plus = true;
            }

            anim.SetFloat("3DMove", blendTreeValue);
        }
    }
}