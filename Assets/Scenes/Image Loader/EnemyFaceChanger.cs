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

        if (Loader.Image != null)
        {
            m_MeshRenderer.gameObject.SetActive(true);

            //Then we have the face to apply
            m_MeshRenderer.material.mainTexture = Loader.Image;
        }
        else
        {
            m_MeshRenderer.gameObject.SetActive(false);
        }
    }
}