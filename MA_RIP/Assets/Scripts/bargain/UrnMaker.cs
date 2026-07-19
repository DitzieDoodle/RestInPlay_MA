using UnityEngine;
using UnityEngine.Audio;

public class UrnMaker : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite metal ;
    public Sprite wood;
    public Sprite ceramic;
    public Sprite jar;

    [Header("Decor")]
    public Sprite whoops;
    public Sprite cross;
    public Sprite flower;
    public Sprite star;
    public Sprite heart;

    [Header("Sounds")]

    public AudioSource audioSource;

    public AudioClip metalSFX;
    public AudioClip woodSFX;
    public AudioClip ceramicSFX;
    public AudioClip jarSFX;

    public AudioClip whoopsSFX;
    public AudioClip crossSFX;
    public AudioClip flowerSFX;
    public AudioClip starSFX;
    public AudioClip heartSFX;



    public SpriteRenderer UrnRenderer;
    public SpriteRenderer DecorRenderer;
    void Awake()
    {
        // AudioSource entweder von diesem GameObject oder neu erstellen
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Urn sprite changer
    public void SetSpriteMetal()
    {
        if (metal == null || UrnRenderer == null) return;
        UrnRenderer.sprite = metal;

        audioSource.clip = metalSFX;
        audioSource.Play();

        Debug.Log("Sprite is now Metal");
    }

    public void SetSpriteWood()
    {
        if (wood == null || UrnRenderer == null) return;
        UrnRenderer.sprite = wood;

        audioSource.clip = woodSFX;
        audioSource.Play();

        Debug.Log("Sprite is now Wood");
    }

    public void SetSpriteCeramic()
    {
        if (ceramic == null || UrnRenderer == null) return;
        UrnRenderer.sprite = ceramic;

        audioSource.clip = ceramicSFX;
        audioSource.Play();

        Debug.Log("Sprite is now Ceramic");

    }

    public void SetSpriteJar()
    {
        if (jar == null || UrnRenderer == null) return;
        UrnRenderer.sprite = jar;

        audioSource.clip = jarSFX;
        audioSource.Play();
        Debug.Log("Sprite is now a Jar haha");

    }


    // decor sprite changer
    public void SetWhoops()
    {
        if (whoops == null || DecorRenderer == null) return;
        DecorRenderer.sprite = whoops;

        audioSource.clip = whoopsSFX;
        audioSource.Play();
    }
    public void SetCross()
    {
        if (cross == null || DecorRenderer == null) return;
        DecorRenderer.sprite = cross;

        audioSource.clip = crossSFX;
        audioSource.Play();
    }

    public void SetFlower()
    {
        if (flower == null || DecorRenderer == null) return;
        DecorRenderer.sprite = flower;

        audioSource.clip = flowerSFX;
        audioSource.Play();
    }
    public void SetStar()
    {
        if (star == null || DecorRenderer == null) return;
        DecorRenderer.sprite = star;

        audioSource.clip = starSFX;
        audioSource.Play();
    }
    public void SetHeart()
    {
        if (heart == null || DecorRenderer == null) return;
        DecorRenderer.sprite = heart;

        audioSource.clip = heartSFX;
        audioSource.Play();
    }

}