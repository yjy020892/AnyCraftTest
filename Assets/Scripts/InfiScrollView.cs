using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JY;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;

namespace JY
{
    [RequireComponent(typeof(ScrollRect))]
    public class InfiScrollView : MonoBehaviour
    {
        public GameObject[] models;

        public GameObject itemObj;

        public int modelCnt = 0;
        public int itemCnt = 0;
        [SerializeField] int totalCnt = 0;

        private float itemWidth = 150.0f;
        private float itemHeight = 150.0f;

        private int scrollWidthCnt;
        private int scrollHeightCnt;
        
        private int heightCnt = 0;
        
        private int itemSpacing;

        [SerializeField] private int widthCheck = 0;
        [SerializeField] private int setItemCount = 0;

        private ScrollRect scrollRect => GetComponent<ScrollRect>();
        RectTransform rt => GetComponent<RectTransform>();

        private Vector2 prevScrollPos;

        private Dictionary<int, ModelData> modelData = new Dictionary<int, ModelData>();
        private Dictionary<int, ItemData> itemData = new Dictionary<int, ItemData>();

        private List<Item> itemList = new List<Item>();

        private void Awake()
        {
            Init();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetContent();
        }

        void Init()
        {
            SetData();

            itemSpacing = (int)((rt.rect.width - (scrollWidthCnt * itemWidth)) / (scrollWidthCnt + 1));

            Debug.Log($"itemSpacing : {itemSpacing}");

            totalCnt = (scrollWidthCnt * scrollHeightCnt) + (scrollWidthCnt * 1);

            Debug.Log($"[{rt.rect.width}]scrollWidthCnt : {scrollWidthCnt}");
            Debug.Log($"[{rt.rect.height}]scrollHeightCnt : {scrollHeightCnt}");

            StartCoroutine(CreateItem());

            scrollRect.onValueChanged.AddListener(OnScrollPosChanged);
        }

        private void SetData()
        {
            scrollWidthCnt = (int)(rt.rect.width / itemWidth);
            scrollHeightCnt = (int)(rt.rect.height / itemHeight);

            for (int i = 0; i < modelCnt; i++)
            {
                ModelData model = new ModelData($"{i}", (ModelType)Random.Range(0, 3));

                modelData.Add(i, model);
            }

            int widthCnt = 0;
            for (int i = 0; i < itemCnt; i++)
            {
                ItemData item = new ItemData($"{i}", (ModelType)Random.Range(0, 3), heightCnt);

                widthCnt++;
                if (widthCnt == scrollWidthCnt)
                {
                    widthCnt = 0;
                    heightCnt++;
                }

                itemData.Add(i, item);
            }
        }

        public void UpdateContent()
        {
            int widthCnt = 0;

            for (int i = 0; i < itemList.Count; i++)
            {
                itemList[i].SetPosition(true, ((itemSpacing * (widthCnt + 1)) + (widthCnt * itemWidth)), -(itemSpacing * (itemList[i].HeightCount + 1) + (itemList[i].HeightCount * itemHeight)));

                heightCnt = itemList[i].HeightCount;
                
                widthCnt++;

                if (widthCnt == scrollWidthCnt)
                {
                    widthCnt = 0;
                }
            }
        }

        private void SetContent()
        {
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, (itemHeight * itemCnt) / scrollHeightCnt);
        }

        public void OnScrollPosChanged(Vector2 scrollPos)
        {
            
            UpdateItems((scrollPos.y < prevScrollPos.y) ? 1 : -1);

            prevScrollPos = scrollPos;
        }

        IEnumerator CreateItem()
        {
            GameObject obj = null;

            for (int i = 0; i < totalCnt; i++)
            {
                Addressables.InstantiateAsync("Assets/Prefabs/Item.prefab", scrollRect.content).Completed += handle => {
                    obj = handle.Result;
                };

                yield return new WaitUntil(() => obj != null);

                Item item = obj.GetComponent<Item>();
                //Item item = Instantiate(itemObj, scrollRect.content).GetComponent<Item>();
                item.SetData(string.Format($"{itemData[i].Name}"), itemData[i].ModelType, itemData[i].HeightCount);
                item.btn.onClick.AddListener(() => OnClickChangeModel(item));

                setItemCount++;

                itemList.Add(item);
            }

            UpdateContent();
        }

        private void UpdateItems(int scrollDirection)
        {
            float contentY = scrollRect.content.anchoredPosition.y;

            if (scrollDirection > 0)
            {
                foreach (var item in itemList)
                {
                    if (item.rt.localPosition.y + contentY > itemHeight)
                    {
                        item.SetData(itemData[setItemCount].Name, itemData[setItemCount].ModelType, itemData[setItemCount].HeightCount);
                        
                        widthCheck++;
                       
                        if (widthCheck == scrollWidthCnt)
                        {
                            itemList = itemList.OrderBy(x => int.Parse(x.Name)).ToList();

                            widthCheck = 0;
                            UpdateContent();
                        }

                        setItemCount++;
                    }
                }
            }
            else if(scrollDirection < 0)
            {
                foreach (var item in itemList)
                {
                    if (item.rt.localPosition.y + contentY < -(rt.rect.height + (itemHeight * 1)))
                    {
                        if (setItemCount - totalCnt - 1 >= 0)
                        {
                            item.SetData(itemData[setItemCount - totalCnt - 1].Name, itemData[setItemCount - totalCnt - 1].ModelType, itemData[setItemCount - totalCnt - 1].HeightCount);

                            widthCheck++;

                            if (widthCheck == scrollWidthCnt)
                            {
                                itemList = itemList.OrderBy(x => int.Parse(x.Name)).ToList();

                                widthCheck = 0;
                                UpdateContent();
                            }

                            setItemCount--;
                        } 
                    }
                }
            }
        }

        private void OnClickChangeModel(Item item)
        {
            //Debug.Log($"Name : {item.Name}, Type : {item.ModelType}");
            foreach(var model in models)
            {
                model.SetActive(false);
            }

            models[(int)item.ModelType].SetActive(true);
        }
    }
}