using UnityEngine;

public class EnemyFaceChanger : MonoBehaviour
{
    public MeshRenderer m_MeshRenderer;

    void OnEnable()
    {
        FetchTexture();
    }

    private void FetchTexture()
    {
        ImageLoaderExtension Loader = FindAnyObjectByType<ImageLoaderExtension>();

        if (Loader.Images.Count > 0)
        {
            m_MeshRenderer.gameObject.SetActive(true);

            //Calculate a random index for random image
            int RandomIndex = Random.Range(0, Loader.Images.Count);

            //Then we have the face to apply
            m_MeshRenderer.material.mainTexture = Loader.Images[RandomIndex];
        }
        else
        {
            m_MeshRenderer.gameObject.SetActive(false);
        }
    }
}