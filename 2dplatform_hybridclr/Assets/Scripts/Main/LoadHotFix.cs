using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Linq;

public class LoadHotFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DownLoadDlls(this.LoadGame));
    }

    private static Dictionary<string, byte[]> s_abBytes = new Dictionary<string, byte[]>();

    public static byte[] GetAbBytes(string dllName)
    {
        return s_abBytes[dllName];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private string GetWebRequestPath(string asset)
    {
        var path = $"{Application.streamingAssetsPath}/{asset}";
        Debug.Log($"streamingAssetsPath: {path}");
        if (!path.Contains("://"))
        {
            path = "file://" + path;
        }
        return path;
    }

    IEnumerator DownLoadDlls(Action onComplete)
    {
        var abs = new string[]
        {
            "common",
            "scene"
        };
        foreach (var ab in abs)
        {
            string dllPath = GetWebRequestPath(ab);
            Debug.Log($"start download ab:{ab}");
            UnityWebRequest www = UnityWebRequest.Get(dllPath);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // Or retrieve results as binary data
                byte[] abBytes = www.downloadHandler.data;
                Debug.Log($"dll:{ab}  size:{abBytes.Length}");
                s_abBytes[ab] = abBytes;
            }
        }

        onComplete();
    }

    public static AssetBundle AssemblyAssetBundle { get; private set; }
    public static AssetBundle SceneAssetBundle { get; private set; }
    private void LoadGame()
    {
        AssetBundle dllAB = AssemblyAssetBundle = AssetBundle.LoadFromMemory(GetAbBytes("common"));
        SceneAssetBundle = AssetBundle.LoadFromMemory(GetAbBytes("scene"));
        Debug.Log("Start Load");
#if !UNITY_EDITOR
        TextAsset dllBytes1 = dllAB.LoadAsset<TextAsset>("HotFix.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes1.bytes);
        TextAsset dllBytes2 = dllAB.LoadAsset<TextAsset>("Mod.dll.bytes");
        System.Reflection.Assembly.Load(dllBytes2.bytes);
#endif
        Debug.Log("End Load");
        SceneManager.LoadScene("SampleScene");
    }
}
