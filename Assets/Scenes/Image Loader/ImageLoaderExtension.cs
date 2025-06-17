using UnityEngine;
using SFB;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class ImageLoaderExtension : MonoBehaviour
{
    public Texture2D Image;
    public TextMeshProUGUI ImagePath;
    public RawImage ImagePreview;

    public static UnityEvent<Texture2D> OnImageLoaded;

    public void LoadImageFromFile()
    {
        ExtensionFilter[] filters = new[] { new ExtensionFilter("Image", "png", "jpg") };
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Choose Image Files", "", filters, false);

        if (paths.Length > 0)
        {
            StartCoroutine(StoreImage(paths[0]));
        }
    }

    IEnumerator StoreImage(string path)
    {
        byte[] data = File.ReadAllBytes(path);
        Texture2D image = new Texture2D(1,1);
        image.LoadImage(data);

        yield return null;

        Image = image;
        ImagePath.text = path;
        ImagePreview.texture = image;

        //trigger the event
        OnImageLoaded?.Invoke(image);
    }

    public void ClearImage()
    {
        Image = null;
        ImagePath.text = "";
        ImagePreview.texture = null;
    }
}