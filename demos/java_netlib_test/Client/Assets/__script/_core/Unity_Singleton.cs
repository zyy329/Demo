using UnityEngine;

public static class Singleton1
{
    public static bool applicationIsQuitting = false;
    public static GameObject _singleton_root = null;
}

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public abstract class Unity_Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
	
    /// <summary>
    /// 直接返回实例
    /// </summary>
    public static T RawInstance
    {
        get { return _instance; }
    }
	
	public static T Instance
	{
		get
		{
            if (Singleton1.applicationIsQuitting)
            {
                //Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                //                 "' already destroyed on application quit." +
                //                 " Won't create again - returning null.");
                return _instance;
            }
			
			if (_instance == null)
			{
				_instance = (T) FindObjectOfType(typeof(T));
					
				if (FindObjectsOfType(typeof(T)).Length > 1)
				{
					Debug.LogError(string.Format("[Singleton] Something went really wrong " 
                        + "- there should never be more than 1 singleton: {0}! Reopenning the scene might fix it.", typeof(T)));
					return _instance;
				}
					
				if (_instance == null)
				{
                    var startTime = Time.realtimeSinceStartup;
					var singleton = new GameObject();
					_instance = singleton.AddComponent<T>();
					singleton.name = typeof(T).ToString();

                    // 挂接到全局节点中;
                    if (Singleton1._singleton_root == null)
                    {
                        Debug.Log("_singleton_root : ");
                        Singleton1._singleton_root = new GameObject("_singleton_root");
                    }
                    singleton.transform.SetParent(Singleton1._singleton_root.transform, false);

                    Log.Loggers.nomal.Debug(string.Format("[Singleton] An instance of {0} was created with DontDestroyOnLoad, elapsed: {1}.", typeof(T), Time.realtimeSinceStartup - startTime));
				} 
                else
                {
                    Log.Loggers.nomal.Debug(string.Format("[Singleton] {0} Using instance of already created {1}!", typeof(T), _instance.gameObject.name));
				}
			}
				
			return _instance;
		}
	}

    protected virtual void Awake()
    {
#if UNITY_EDITOR
        if (FindObjectsOfType(typeof(T)).Length > 1)
        {
            Debug.LogError(string.Format("[Singleton] Something went really wrong "
                + "- there should never be more than 1 singleton: {0}! Reopenning the scene might fix it.", typeof(T)));
            return;
        }
#endif
        _instance = this as T;
        DontDestroyOnLoad(_instance.gameObject);
    }

    public static bool ApplicationIsQuit
    {
        get { return Singleton1.applicationIsQuitting; }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        Singleton1.applicationIsQuitting = true;
    }

    protected virtual void OnDestroy()
    {
        //if (Global.LOG) Debug.Log(string.Format("Singleton {0} destoryed!", GetType()));
        Singleton1.applicationIsQuitting = true;
    }
}
