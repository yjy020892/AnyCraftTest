using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace JY
{
    public class AddressableManager
    {
        static AddressableManager Instance = null;

        static readonly object padlock = new object();

        public AsyncOperationHandle ItemHandle;

        public GameObject addressablesItemObj;

        public static AddressableManager GetInstance {
            get {
                lock (padlock)
                {
                    if (null == Instance)
                        Instance = new AddressableManager();
                    return Instance;
                }
            }
        }

        /// <summary>
        /// ��巹���� ������ �ε�
        /// ������ �Ѱ����� ������ ��ġ �������� �߰��� Ȯ�强 ���
        /// </summary>
        /// <param name="lable"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IEnumerator AddressablesAsync(string lable, System.Action<bool> callback)
        {
            bool isDone = false;

            Addressables.LoadAssetAsync<GameObject>(lable).Completed += (AsyncOperationHandle<GameObject> obj) => {
                if (obj.IsDone)
                    isDone = true;

                ItemHandle = obj;
                addressablesItemObj = obj.Result;
            };

            yield return new WaitUntil(() => isDone);

            callback(true);
        }
    }
}