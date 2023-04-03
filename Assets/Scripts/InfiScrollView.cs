using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.AddressableAssets;

namespace JY
{
    [RequireComponent(typeof(ScrollRect))]
    public class InfiScrollView : MonoBehaviour
    {
        public GameObject[] models;

        //public GameObject itemObj;

        //public int modelCnt = 0;
        public int itemCnt = 0; // ModelList UI ����
        int totalCnt = 0; // ȭ�鿡 �׷��� ModelList UI �� ����

        public float itemWidth = 0; // ModelList UI ���� ����
        public float itemHeight = 0; // ModelList UI ���� ����

        private int scrollWidthCnt; // ���ο� �� ����
        private int scrollHeightCnt; // ���ο� �� ����
        
        private int itemFloor = 0; // ModelList UI �ݷ� ��
        private int maxItemFloor = 0; // ModelList UI �ݷ� �ְ� ��
        
        private int itemSpacing; // ModelList UI ���� �Ÿ�

        private int setItemCount = 0; // ModelList UI ���� ī��Ʈ

        private ScrollRect scrollRect => GetComponent<ScrollRect>();
        RectTransform rt => GetComponent<RectTransform>();

        private Vector2 prevScrollPos;

        //private Dictionary<int, ModelData> modelData = new Dictionary<int, ModelData>();
        private Dictionary<int, ItemData> itemData = new Dictionary<int, ItemData>(); // itemCnt��ŭ�� ModelList ������ ����

        private List<Item> itemList = new List<Item>(); // ȭ�鿡 �������� ModelList ����

        private void Awake()
        {
            Init();
        }

        // Start is called before the first frame update
        private void Start()
        {
            SetContent();
        }

        void Init()
        {
            scrollWidthCnt = (int)(rt.rect.width / itemWidth);
            scrollHeightCnt = (int)(rt.rect.height / itemHeight);

            itemSpacing = (int)((rt.rect.width - (scrollWidthCnt * itemWidth)) / (scrollWidthCnt + 1));

            totalCnt = (scrollWidthCnt * scrollHeightCnt) + (scrollWidthCnt * 2);

            if (totalCnt >= itemCnt)
                totalCnt = itemCnt;

            //Debug.Log($"itemSpacing : {itemSpacing}");
            //Debug.Log($"[{rt.rect.width}] scrollWidthCnt : {scrollWidthCnt}");
            //Debug.Log($"[{rt.rect.height}] scrollHeightCnt : {scrollHeightCnt}");

            SetData();

            //StartCoroutine(CreateItem());
            CreateItem();

            scrollRect.onValueChanged.AddListener(OnScrollPosChanged);
        }

        /// <summary>
        /// ModelList UI ������ ������ ���� ����
        /// </summary>
        private void SetData()
        {
            //for (int i = 0; i < itemCnt; i++)
            //{
            //    ModelData model = new ModelData($"{i}", GetRandomModelType());

            //    modelData.Add(i, model);
            //}
            
            int widthCnt = 0;
            for (int i = 0; i < itemCnt; i++)
            {
                ItemData item = new ItemData($"{i}", GetRandomModelType(), itemFloor, new Vector3(((itemSpacing * (widthCnt + 1)) + (widthCnt * itemWidth)), -(itemSpacing * (itemFloor + 1) + (itemFloor * itemHeight))), true);

                widthCnt++;
                if (widthCnt == scrollWidthCnt)
                {
                    widthCnt = 0;
                    itemFloor++;
                }

                itemData.Add(i, item);
            }

            //Debug.Log($"{itemCnt % scrollWidthCnt}");

            if(itemCnt % scrollWidthCnt > 0)
                maxItemFloor = itemFloor + 1;
            else
                maxItemFloor = itemFloor;
        }

        /// <summary>
        /// ScrollView Content ������Ʈ
        /// </summary>
        public void UpdateContent()
        {
            int widthCnt = 0;

            for (int i = 0; i < itemList.Count; i++)
            {
                if(itemList[i].IsPosition)
                    itemList[i].SetPosition(true, itemList[i].ItemPos.x, itemList[i].ItemPos.y);
                else
                {
                    itemList[i].SetPosition(true, ((itemSpacing * (widthCnt + 1)) + (widthCnt * itemWidth)), -(itemSpacing * (itemList[i].HeightCount + 1) + (itemList[i].HeightCount * itemHeight)));

                    itemFloor = itemList[i].HeightCount;

                    widthCnt++;

                    if (widthCnt == scrollWidthCnt)
                    {
                        widthCnt = 0;
                    }
                }
            }
        }

        /// <summary>
        /// ScrollView Content ������ ����
        /// </summary>
        private void SetContent()
        {
            //scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, ((itemHeight * (itemCnt / scrollWidthCnt)) + (itemSpacing * (itemCnt / scrollWidthCnt) + itemSpacing)));
            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, ((itemHeight * maxItemFloor) + (itemSpacing * maxItemFloor + itemSpacing)));
        }

        /// <summary>
        /// ��ũ�� ��ġ ���� üũ (���� : 1, �Ʒ��� -1)
        /// </summary>
        /// <param name="scrollPos"></param>
        public void OnScrollPosChanged(Vector2 scrollPos)
        {
            UpdateItems((scrollPos.y < prevScrollPos.y) ? 1 : -1);

            prevScrollPos = scrollPos;
        }

        /// <summary>
        /// ��巹���� ModelList UI ������ ������Ʈ �񵿱� ���� �� ������ ����
        /// ���� �Ϸ�Ǹ� �ε�â ����
        /// </summary>
        /// <returns></returns>
        //IEnumerator CreateItem()
        private void CreateItem()
        {
            //GameObject obj = null;

            for (int i = 0; i < totalCnt; i++)
            {
                //Addressables.InstantiateAsync("Assets/Prefabs/Item.prefab", scrollRect.content).Completed += handle => {
                //    obj = handle.Result;
                //};

                //yield return new WaitUntil(() => obj != null);

                //Item item = obj.GetComponent<Item>();

                Item item = Instantiate(AddressableManager.GetInstance.addressablesItemObj, scrollRect.content).GetComponent<Item>();
                item.SetData(string.Format($"{itemData[i].Name}"), itemData[i].ModelType, itemData[i].HeightCount, itemData[i].ItemPos, itemData[i].IsPosition);
                item.SetScale(itemWidth, itemHeight);
                item.btn.onClick.AddListener(() => OnClickChangeModel(item));

                setItemCount++;

                itemList.Add(item);
            }

            Addressables.Release(AddressableManager.GetInstance.ItemHandle); // �޸𸮿��� �ε��� ��巹���� Model UI ������

            UpdateContent();
        }

        /// <summary>
        /// ��ũ�� ���¿����� ModelList UI ������ ���� ������Ʈ
        /// </summary>
        /// <param name="scrollDirection"></param>
        private void UpdateItems(int scrollDirection)
        {
            float contentY = scrollRect.content.anchoredPosition.y;

            if (scrollDirection > 0)
            {
                foreach (var item in itemList)
                {
                    if (item.rt.localPosition.y + contentY > itemHeight)
                    {
                        if(setItemCount < itemCnt)
                        {
                            item.SetData(itemData[setItemCount].Name, itemData[setItemCount].ModelType, itemData[setItemCount].HeightCount, itemData[setItemCount].ItemPos, itemData[setItemCount].IsPosition);

                            itemList = itemList.OrderBy(x => int.Parse(x.Name)).ToList();
                            UpdateContent();

                            setItemCount++;
                        }
                        else
                        {
                            setItemCount = itemCnt;
                        }
                    }
                }
            }
            else if(scrollDirection < 0)
            {
                foreach (var item in itemList)
                {
                    if (item.rt.localPosition.y + contentY < -(rt.rect.height + (itemHeight * 0.5f)))
                    {
                        if (setItemCount - totalCnt - 1 >= 0)
                        {
                            item.SetData(itemData[setItemCount - totalCnt - 1].Name, itemData[setItemCount - totalCnt - 1].ModelType, itemData[setItemCount - totalCnt - 1].HeightCount, itemData[setItemCount - totalCnt - 1].ItemPos, itemData[setItemCount - totalCnt - 1].IsPosition);

                            itemList = itemList.OrderBy(x => int.Parse(x.Name)).ToList();
                            UpdateContent();

                            setItemCount--;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// �� ���� ����
        /// </summary>
        /// <returns></returns>
        private ModelType GetRandomModelType()
        {
            return (ModelType)Random.Range(0, models.Length);
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