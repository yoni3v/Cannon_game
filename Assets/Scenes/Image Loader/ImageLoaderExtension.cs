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
    [SerializeField] Button AddImages;

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
            if (Images.Count >= 24)
            {
                AddImages.interactable = false;
                yield break;
            }

            byte[] data = File.ReadAllBytes(path);
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
        Destroy(image_object);
        Images.Remove(image);

        AddImages.interactable = true;
    }
}