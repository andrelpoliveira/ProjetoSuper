using System.Collections;
using UnityEngine;

public class TecladoController : MonoBehaviour
{
    [Header("Materials")]
    public Material initMaterial;
    public Material newMaterial;
    private Renderer objRenderer;
    public float duration = 0.5f;
    [Space]
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip audioClip;

    private void Start()
    {
        objRenderer = GetComponent<Renderer>();
        //audioSource = GetComponent<AudioSource>();

        initMaterial = objRenderer.material;
    }

    IEnumerator ChangeMaterialTemporary()
    {
        objRenderer.material = newMaterial;

        yield return new WaitForSeconds(duration);

        objRenderer.material = initMaterial;
    }

    void PlaySound()
    {
        if (audioSource != null)
            audioSource.PlayOneShot(audioClip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(ChangeMaterialTemporary());
            PlaySound();
        }
    }
}
