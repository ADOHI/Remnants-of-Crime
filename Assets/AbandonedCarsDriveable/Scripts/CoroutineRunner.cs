using UnityEngine;

namespace AbandonedCarsDriveable.Scripts
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance == null)
                {
                    if (!Application.isPlaying)
                    {
                        return null;
                    }

                    var obj = new GameObject("CoroutineRunner");
                    _instance = obj.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(obj);
                }
                return _instance;
            }
        }
    }
}