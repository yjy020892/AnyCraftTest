using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JY
{
    public class PooledManager : MonoBehaviour
    {
        public static PooledManager instance;

        public GameObject poolObj_Item;
        public GameObject group_Item;
        public int poolAmount_Item;
        [HideInInspector] public List<GameObject> poolObjs_Item = new List<GameObject>();

        public Transform spawnItemPoint;

        void Awake()
        {
            if (PooledManager.instance == null)
            {
                PooledManager.instance = this;
            }
        }

        private void Start()
        {
            for (int i = 0; i < poolAmount_Item; i++)
            {
                GameObject obj_Item = Instantiate(poolObj_Item, poolObj_Item.transform, false);

                obj_Item.name = "Item";
                //obj_GameText.transform.localScale = new Vector3(1.3f, 1.3f, 1);

                obj_Item.SetActive(false);
                poolObjs_Item.Add(obj_Item);
            }
        }

        public GameObject GetPooledObject_Item()
        {
            for (int i = 0; i < poolObjs_Item.Count; i++)
            {
                if (!poolObjs_Item[i].activeInHierarchy)
                {
                    //poolObjs_ChatBox[i].transform.SetPositionAndRotation(posi.position, Quaternion.identity);

                    return poolObjs_Item[i];
                }
            }

            return null;
        }
    }
}