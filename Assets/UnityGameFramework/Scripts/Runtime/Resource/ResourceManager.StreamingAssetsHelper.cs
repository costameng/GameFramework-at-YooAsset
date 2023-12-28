using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;
using YooAsset;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        #if UNITY_EDITOR
        public sealed class StreamingAssetsHelper
        {
            public static void Init() { }
            public static bool FileExists(string packageName, string fileName, string fileCRC)
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, StreamingAssetsDefine.RootFolderName, packageName, fileName);
                if (File.Exists(filePath))
                {
                    if (GameQueryServices.CompareFileCRC)
                    {
                        
                        string crc32 = YooAsset.HashUtility.FileCRC32(filePath);
                        return crc32 == fileCRC;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        #else
        public sealed class StreamingAssetsHelper
        {
            private class PackageQuery
            {
                public readonly Dictionary<string, BuildinFileManifest.Element> Elements = new Dictionary<string, BuildinFileManifest.Element>(1000);
            }
        
            private static bool _isInit = false;
            private static readonly Dictionary<string, PackageQuery> _packages = new Dictionary<string, PackageQuery>(10);
        
            /// <summary>
            /// 初始化
            /// </summary>
            public static void Init()
            {
                if (_isInit == false)
                {
                    _isInit = true;
        
                    var manifest = Resources.Load<BuildinFileManifest>("BuildinFileManifest");
                    if (manifest != null)
                    {
                        foreach (var element in manifest.BuildinFiles)
                        {
                            if (_packages.TryGetValue(element.PackageName, out PackageQuery package) == false)
                            {
                                package = new PackageQuery();
                                _packages.Add(element.PackageName, package);
                            }
                            package.Elements.Add(element.FileName, element);
                        }
                    }
                }
            }
        
            /// <summary>
            /// 内置文件查询方法
            /// </summary>
            public static bool FileExists(string packageName, string fileName, string fileCRC32)
            {
                if (_isInit == false)
                    Init();
        
                if (_packages.TryGetValue(packageName, out PackageQuery package) == false)
                    return false;
        
                if (package.Elements.TryGetValue(fileName, out var element) == false)
                    return false;
        
                if (GameQueryServices.CompareFileCRC)
                {
                    return element.FileCRC32 == fileCRC32;
                }
                else
                {
                    return true;
                }
            }
        }
        #endif
    }
}


#if UNITY_ANDROID && UNITY_EDITOR
/// <summary>
/// 为Github对开发者的友好，采用自动补充UnityPlayerActivity.java文件的通用姿势满足各个开发者
/// </summary>
internal class AndroidPost : UnityEditor.Android.IPostGenerateGradleAndroidProject
{
    public int callbackOrder => 99;
    public void OnPostGenerateGradleAndroidProject(string path)
    {
        path = path.Replace("\\", "/");
        string untityActivityFilePath = $"{path}/src/main/java/com/unity3d/player/UnityPlayerActivity.java";
        var readContent = System.IO.File.ReadAllLines(untityActivityFilePath);
        string postContent =
            "    //auto-gen-function \n" +
            "    public boolean CheckAssetExist(String filePath) \n" +
            "    { \n" +
            "        android.content.res.AssetManager assetManager = getAssets(); \n" +
            "        try \n" +
            "        { \n" +
            "            java.io.InputStream inputStream = assetManager.open(filePath); \n" +
            "            if (null != inputStream) \n" +
            "            { \n" +
            "                 inputStream.close(); \n" +
            "                 return true; \n" +
            "            } \n" +
            "        } \n" +
            "        catch(java.io.IOException e) \n" +
            "        { \n" +
            "            e.printStackTrace(); \n" +
            "        } \n" +
            "        return false; \n" +
            "    } \n" +
            "}";

        if (CheckFunctionExist(readContent) == false)
            readContent[readContent.Length - 1] = postContent;
        System.IO.File.WriteAllLines(untityActivityFilePath, readContent);
    }
    private bool CheckFunctionExist(string[] contents)
    {
        for (int i = 0; i < contents.Length; i++)
        {
            if (contents[i].Contains("CheckAssetExist"))
            {
                return true;
            }
        }
        return false;
    }
}
#endif

/*
//auto-gen-function
public boolean CheckAssetExist(String filePath)
{
	android.content.res.AssetManager assetManager = getAssets();
	try
	{
		java.io.InputStream inputStream = assetManager.open(filePath);
		if(null != inputStream)
		{
			inputStream.close();
			return true;
		}
	}
	catch(java.io.IOException e)
	{
		e.printStackTrace();
	}
	return false;
}
*/