using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonComportament : MonoBehaviour
{

    public enum ButtonFuncionalities { Sound, Music } // lista de enumeradores que aparecerá no Inspector. Adicionei "sound" e "Music" pois sei que será usado lá na frente.
    public ButtonFuncionalities ButtonTypes;
    public Button button; // Botão em que o script está atachado.
    public Sprite buttonOn;
    public Sprite buttonOff;
    private bool active = false;

    private void OnEnable()
    {
        switch (ButtonTypes)
        {
            case ButtonFuncionalities.Sound:
                AudioManager.instance.buttonSFX = this;
                AudioManager.instance.VerifySoundPrefs(Sound.SoundType.SFX, false);
                break;
            case ButtonFuncionalities.Music:
                AudioManager.instance.buttonMusic = this;
                AudioManager.instance.VerifySoundPrefs(Sound.SoundType.Music, false);
                break;
            default:
                break;
        }
    }

    public void ButtonFunction()
    {

        // Funcionlidades comuns a todos os botões com a caracteristíca de On e Off: trocar os sprites. Desta forma, ele é sempre chamado.
        if (active == false)
        {
            button.GetComponent<Image>().sprite = buttonOff;
        }

        if (active == true)
        {
            button.GetComponent<Image>().sprite = buttonOn;
        }

        active = !active;

        switch (ButtonTypes)
        {           
            case ButtonFuncionalities.Sound:
                AudioManager.instance.MuteSoundByType(Sound.SoundType.SFX);
                // Adicionar aqui a funcionalidade do botão de efeito sonoro quando clicado.
                break;
            case ButtonFuncionalities.Music:
                AudioManager.instance.MuteSoundByType(Sound.SoundType.Music);
                //Adicionar aqui a funcionalidade do butão de música quando clicado.
                break;

        }
    }

    public void changeButtonSprite(bool setOn)
    {
        active = !setOn;

        if (setOn == true)
        {
            button.GetComponent<Image>().sprite = buttonOn;
        }

        if (setOn == false)
        {
            button.GetComponent<Image>().sprite = buttonOff;
        }
    }

}
