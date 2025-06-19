using UnityEngine;
using SFB;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class ImageLoaderExtension : MonoBehaviour
{
    //All the textures
    public List<Texture2D> Images = new List<Texture2D>();

    [Header("Components")]
    [SerializeField] GameObject ImagePrefab;
    [SerializeField] Transform ImagesHolder;

    [SerializeField] ImageWrapper ImagesDictionary = new ImageWrapper{ ImagesDict = new List<CustomDict>() };

    private void Awake()
    {
        if(File.Exists(Application.dataPath + "/" + "Images.AHEKFILE"))
        {
            string RawData = File.ReadAllText(Application.dataPath + "/" + "Images.AHEKFILE");
            ImagesDictionary = JsonUtility.FromJson<ImageWrapper>(RawData);
            StartCoroutine(LoadImage());
        }
    }

    public void LoadImageFromFile()
    {
        ExtensionFilter[] filters = new[] { new ExtensionFilter("Images", "png", "jpg") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Choose Images Files", "", filters, true);

        if (paths.Length > 0)
        {
            StartCoroutine(StoreImage(paths));
        }
    }

    IEnumerator StoreImage(string[] paths)
    {
        foreach (string path in paths)
        {
            byte[] data = File.ReadAllBytes(path);
            Texture2D image = new Texture2D(1, 1);
            image.LoadImage(data);
            yield return null;

            if (!Images.Contains(image))
            {
                Images.Add(image);
                ImagesDictionary.ImagesDict.Add(new CustomDict { Key = Images.IndexOf(image), Value = path});
            }
        }

        SavePath();
        UpdatePreviews();
    }

    IEnumerator LoadImage()
    {
        foreach (var item in ImagesDictionary.ImagesDict)
        {
            byte[] data = File.ReadAllBytes(item.Value);
            Texture2D image = new Texture2D(1, 1);
            image.LoadImage(data);
            yield return null;

            if (!Images.Contains(image))
            {
                Images.Add(image);
            }
        }

        UpdatePreviews();
    }

    private void SavePath()
    {
        string jsonData = JsonUtility.ToJson(ImagesDictionary);
        File.WriteAllText(Application.dataPath + "/" + "Images.AHEKFILE", jsonData);
    }

    private void UpdatePreviews()
    {
        for(int i = 0; i < ImagesHolder.transform.childCount; i++)
        {
            Destroy(ImagesHolder.transform.GetChild(i).gameObject);
        }

        //Once all the images are added to the list update the preview
        foreach (Texture2D image in Images)
        {
            GameObject ImageObject = Instantiate(ImagePrefab, ImagesHolder);

            //update the preview
            ImageObject.GetComponent<RawImage>().texture = image;

            //button event
            Button RemoveButton = ImageObject.transform.GetChild(0).GetComponent<Button>();
            RemoveButton.onClick.AddListener(() => RemoveImage(ImageObject, image));
        }
    }

    public void RemoveImage(GameObject image_object, Texture2D image)
    {
        ImagesDictionary.RemoveKey(Images.IndexOf(image));
        Destroy(image_object);
        Images.Remove(image);

        SavePath();
    }
}

[System.Serializable]
public struct ImageWrapper 
{
    public List<CustomDict> ImagesDict;

    public void RemoveKey(int key)
    {
        foreach (var item in ImagesDict)
        {
            if (item.Key == key)
            {
                ImagesDict.Remove(item);
                break;
            }
        }
    }
}

[System.Serializable]
public struct CustomDict
{
    public int Key;
    public string Value;
}