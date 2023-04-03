using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JY
{
    public class Title : MonoBehaviour
    {
        float timer = 0.0f;

        bool b_Start = false;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            if (timer > 2.0f)
            {
                if (!b_Start)
                {
                    Loading.LoadScene("Lobby");
                    b_Start = true;
                }
            }
        }
    }
}