using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using JY;

namespace JY
{
    [RequireComponent(typeof(ScrollRect))]
    public class InfiScrollView : MonoBehaviour
    {
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

        private Dictionary<int, Model> modelData = new Dictionary<int, Model>();
        private Dictionary<int, Item> itemData = new Dictionary<int, Item>();

        private List<Item> itemList = new List<Item>();

        // Start is called before the first frame update
        void Start()
        {
            Init();
            SetContent();
        }

        void Init()
        {
            SetData();

            scrollWidthCnt = (int)(rt.rect.width / itemWidth);
            scrollHeightCnt = (int)(rt.rect.height / itemHeight);

            itemSpacing = (int)((rt.rect.width - (scrollWidthCnt * itemWidth)) / (scrollWidthCnt + 1));

            Debug.Log($"itemSpacing : {itemSpacing}");

            totalCnt = (scrollWidthCnt * scrollHeightCnt) + (scrollWidthCnt * 1);

            Debug.Log($"[{rt.rect.width}]scrollWidthCnt : {scrollWidthCnt}");
            Debug.Log($"[{rt.rect.height}]scrollHeightCnt : {scrollHeightCnt}");

            for (int i = 0; i < totalCnt; i++)
            {
                Item item = Instantiate(itemObj, scrollRect.content).GetComponent<Item>();
                item.SetData(string.Format($"{itemData[i].Name}"), itemData[i].ModelType, itemData[i].HeightCount);

                setItemCount++;

                itemList.Add(item);
            }

            UpdateContent();

            scrollRect.onValueChanged.AddListener(OnScrollPosChanged);
        }

        private void SetData()
        {
            for (int i = 0; i < modelCnt; i++)
            {
                Model model = new Model($"{i}", (ModelType)Random.Range(0, 3));

                modelData.Add(i, model);
            }

            int widthCnt = 0;
            for (int i = 0; i < itemCnt; i++)
            {
                Item item = new Item($"{i}", (ModelType)Random.Range(0, 3), heightCnt);

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
    }
}