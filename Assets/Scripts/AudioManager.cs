using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public ButtonComportament buttonMusic;
    public ButtonComportament buttonSFX;
    public Sound[] sounds;



    // Variável estática que referencia instancia desse AudioManager.
    public static AudioManager instance;

    void Awake()
    {

        // Checa se existe um instancia do AudioManager criado, senão instancia a primeira. Caso já exista uma instância, ele destrói a nova.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        

        AudioManagerSetup();


    }

    private void Start()
    {
        AudioManager.instance.PlaySound("Theme");
        VerifySoundPrefs(Sound.SoundType.SFX, true);
        VerifySoundPrefs(Sound.SoundType.Music, true);
    }

    private void AudioManagerSetup()
    {
        foreach (Sound s in instance.sounds)
        {
            // Adiciono o s.source da classe Sound no AudioManager (objeto em branco que contém o script).
            s.source = gameObject.AddComponent<AudioSource>();

            // Aqui eu digo que o audio(clip) da lista, tem os mesmo valores (clip, volume, pitch e loop) do AudioSource da Classe Sound.
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }


    public AudioSource GetAudioSource(string name)
    {
        // Procura no array "sounds" um elemento que tenha o mesmo nome da string passada pelo método Play();
        Sound s = Array.Find(instance.sounds, sound => sound.name == name);

        // Se a refência a string não existir ele faz o Debug.log e retorna.
        if (s == null)
        {
            Debug.Log("Não foi possível localizar o áudio: " + name);
            return null;
        }

        return s.source; // retorno o AudioSource correspondente.
    }


    public void PlaySound(string name)
    {
        // ERRO IMPORTANTE: tenta pegar o AudioSource correspondente ao nome passado pelo parametro do método PlaySound.
        try
        {
            GetAudioSource(name).Play();
        }
        catch (System.Exception)
        {
            Debug.LogError(name + " não pode ser tocado pois não esta contido na lista de sons!");
        }
    }

    public void MuteSoundByType(Sound.SoundType type) // Passo qual tipo de áudio ele no Inspecto (FX ou Music).
    {
        bool isMuted = false;

        foreach (Sound s in instance.sounds)
        {
            if (s.soundType == type)
            {
                s.source.mute = !s.source.mute;
                isMuted = s.source.mute;
            }
        }

        if (type == Sound.SoundType.Music)
        {
            PlayerPrefs.SetInt("MusicMuted", Convert.ToInt32(isMuted));
        }

        if (type == Sound.SoundType.SFX)
        {
            PlayerPrefs.SetInt("SfxMuted", Convert.ToInt32(isMuted));
        }
    }

    public void VerifySoundPrefs(Sound.SoundType type, bool isInitializing)
    {
        bool isMuted = false;

        if (type == Sound.SoundType.Music)
        {
            try
            {
                isMuted = Convert.ToBoolean(PlayerPrefs.GetInt("MusicMuted"));
                instance.buttonMusic.changeButtonSprite(!isMuted);
            }
            catch (Exception ex)
            {
                //throw;
            }
            
        }

        if (type == Sound.SoundType.SFX)
        {
            try
            {
                isMuted = Convert.ToBoolean(PlayerPrefs.GetInt("SfxMuted"));
                instance.buttonSFX.changeButtonSprite(!isMuted);
            }
            catch (Exception ex)
            {
            
                //throw;
            }
            
        }

        if (isInitializing)
        {
            if (isMuted)
            {
                MuteSoundByType(type);
            }
        }
    }

    public bool IsThisCurrentMusicPlaying(String name)
    {
        AudioSource dispute = GetAudioSource(name);

        foreach (Sound s in instance.sounds)
        {
            if (s.source.isPlaying)
            {
                if (s.source == dispute)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void StopAllMusic()
    {
        foreach (Sound s in instance.sounds)
        {
            if (s.soundType == Sound.SoundType.Music)
            {
                s.source.Stop();
            }
        }
    }
}

