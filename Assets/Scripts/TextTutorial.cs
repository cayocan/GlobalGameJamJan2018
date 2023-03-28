using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class TextTutorial : MonoBehaviour
{

    public string[] legends;
    private int indexlegends = 0;
    public List<string> audioNames;
    public Text text;
    public Button buttonPrevious;
    public Button buttonNext;
    

    void Start()
    {

        // O texto de começo do é igual ao primeiro elemento do array de strings.
        AudioManager.instance.PlaySound(audioNames[indexlegends]);
        text.text = legends[indexlegends];
    }


    public void NextText()
    {
        if (indexlegends < legends.Length - 1)
        {
            // Troca o text para o index seguinte ao atual.
            AudioManager.instance.GetAudioSource(audioNames[indexlegends]).Stop();

            text.text = legends[++indexlegends];
            
            AudioManager.instance.PlaySound(audioNames[indexlegends]);
        }


        // Checagem para tornar o botão de voltar ativo de novo, pois caso o index seja maior do que zero, ele deve voltar a ser funcional.
        if (indexlegends > 0)
        {
            buttonPrevious.interactable = true;
        }

        // Se o jogador chegar na última legenda, ele torna um botão de proximo desativado.
        if (indexlegends == legends.Length - 1)
        {
            buttonNext.interactable = false;
        }
    }

    public void PreviousText()
    {
        if (indexlegends > 0)
        {
            AudioManager.instance.GetAudioSource(audioNames[indexlegends]).Stop();
            // Troca o text para o index anterior ao atual.
            text.text = legends[--indexlegends];
            AudioManager.instance.PlaySound(audioNames[indexlegends]);
        }

        // Se o jogador chegar na primeira legenda, ele torna um botão de legenda anterior desativado.
        if (indexlegends == 0)
        {
            buttonPrevious.interactable = false;
        }

        // Checagem para tornar o botão de avançar ativo de novo, pois caso o index seja menor do que o tamanho do array, ele deve voltar a ser funcional.
        if (indexlegends < legends.Length - 1)
        {
            buttonNext.interactable = true;
        }
    }

    public void PauseAudios()
    {
        foreach (var item in audioNames)
        {
            AudioManager.instance.GetAudioSource(item).Stop();    
       }
        
    }

}