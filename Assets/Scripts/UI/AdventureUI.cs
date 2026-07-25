using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine.VFX;

public class AdventureUI : MonoBehaviour
{
    [Header("Main UI")]
    public TextMeshProUGUI mainText;
    public Button[] optionButtons;
    public Button continueButton;

    [Header("Dice Area")]
    public GameObject diceArea;
    public TextMeshProUGUI diceValue1Text;
    public TextMeshProUGUI diceValue2Text;
    public TextMeshProUGUI outcomeText;

    [Header("Reference")]
    public AdventureManager manager;

    [Header("Cards UI")]
    public Transform cardButtonsParent;      
    public GameObject cardArea;

    [Header("Intro")]
    public Animator journeyIntroAnimator;
    public TextMeshProUGUI textJourneyIntro;
    public string strJourneyIntroPhrase;

    [Header("Phase Intro")]
    public Animator animatorPhaseStart;          
    public TextMeshProUGUI textPhaseTitle;       
    public GameObject go_phaseintro;

    [Header("Level Summary")]
    public GameObject summaryLevelObject;
    public Animator summaryLevelAnimator;
    public Button summaryButton;
    public SFXManager sfxManager;

    void Start()
    {
        // Configura os botões de escolha
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => manager.SelectChoice(index));
        }
        if (summaryButton != null)
        {
            summaryButton.onClick.AddListener(OnSummaryButtonClicked);
        }
        // Configura o botão Avançar
        continueButton.onClick.AddListener(manager.OnContinueClicked);

        // Estado inicial
        continueButton.gameObject.SetActive(false);
        diceArea.SetActive(false);
        cardArea.SetActive(false);
    }

    public void ShowDescription(string text)
    {
        mainText.text = text;
    }

    public void ShowOptions(List<string> optionTexts)
    {
        /*
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < optionTexts.Count)
            {
                TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                buttonText.text = optionTexts[i];
                optionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
        */
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < optionTexts.Count)
            {
                TextMeshProUGUI buttonText = optionButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                    buttonText.text = optionTexts[i];

                optionButtons[i].gameObject.SetActive(true); // liga o botão
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void HideOptions()
    {
        foreach (var button in optionButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    public void ShowConsequence(string text)
    {
        mainText.text = text;
    }

    public void ShowDiceRoll(int dice1, int dice2, bool success)
    {
        diceArea.SetActive(true);
        diceValue1Text.text = dice1.ToString();
        diceValue2Text.text = dice2.ToString();
        outcomeText.text = success ? "Success" : "Failure";
    }

    public void HideDiceArea()
    {
        diceArea.SetActive(false);
    }

    public void ShowContinueButton(bool show)
    {
        continueButton.gameObject.SetActive(show);
    }

    //cards
    public void PrepareCards(List<SoCard> availableCards, PlayerStats playerStats)
    {
        if (availableCards == null || availableCards.Count == 0)
        {
            Debug.LogWarning("Não há cartas disponíveis na lista!");
            return;
        }

        if (cardButtonsParent == null)
        {
            Debug.LogWarning("cardButtonsParent não foi atribuído!");
            return;
        }

        // Sorteia 3 cartas aleatórias
        List<SoCard> selectedCards = availableCards
            .OrderBy(x => Random.value)
            .Take(cardButtonsParent.childCount)
            .ToList();

        for (int i = 0; i < cardButtonsParent.childCount; i++)
        {
            Transform child = cardButtonsParent.GetChild(i);
            CardButton cardButton = child.GetComponent<CardButton>();

            if (cardButton != null && i < selectedCards.Count)
            {
                cardButton.card = selectedCards[i];
                cardButton.playerStats = playerStats;
                cardButton.manager = manager;              
                cardButton.UpdateButtonText();

                // Atualiza o texto do botão com o nome da carta
                TextMeshProUGUI buttonText = child.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = selectedCards[i].cardName;
                }
            }
        }
    }

    public void ShowCardArea()
    {
        if (cardArea != null)
            cardArea.SetActive(true);
    }

    public void HideCardArea()
    {
        if (cardArea != null)
            cardArea.SetActive(false);
    }

    //Intro anim
    public void PlayIntro()
    {
        // Coloca o texto
        if (textJourneyIntro != null)
        {
            textJourneyIntro.text = strJourneyIntroPhrase;
        }

        // Dispara o trigger "Go"
        if (journeyIntroAnimator != null)
        {
            journeyIntroAnimator.SetTrigger("Go");
        }
    }
    public void PlayPhaseIntro(string phaseName)
    {
        if (go_phaseintro != null)
            go_phaseintro.SetActive(true);

        if (textPhaseTitle != null)
            textPhaseTitle.text = phaseName;

        if (animatorPhaseStart != null)
            animatorPhaseStart.SetTrigger("Go");

        // Não desliga mais aqui. A gente vai controlar pelo Manager ou por outra coroutine.
    }

    public IEnumerator StartTypewriter(string fullText, float delayBetweenChars = 0.03f)
    {
        /*
        StopAllCoroutines(); // evita sobreposição
        StartCoroutine(TypewriterEffect(fullText, delayBetweenChars));
        */
        mainText.text = "";

        foreach (char c in fullText)
        {
            mainText.text += c;
            yield return new WaitForSeconds(delayBetweenChars);
        }
        
    }

    IEnumerator TypewriterEffect(string fullText, float delayBetweenChars)
    {
        mainText.text = "";

        foreach (char c in fullText)
        {
            mainText.text += c;
            yield return new WaitForSeconds(delayBetweenChars);
        }
    }

    public void HidePhaseIntro()
    {
        if (go_phaseintro != null)
            go_phaseintro.SetActive(false);
    }

    IEnumerator HidePhaseIntroAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (go_phaseintro != null)
            go_phaseintro.SetActive(false);
    }

    public void PlayLevelSummary()
    {
        if (summaryLevelObject != null)
        {
            summaryLevelObject.SetActive(true);
        }

        if (summaryLevelAnimator != null)
        {
            summaryLevelAnimator.SetTrigger("Go");
        }
        if (sfxManager != null)
        {
            sfxManager.PlaySoundtrack();
        }
    }
    public void OnSummaryButtonClicked()
    {
        if (summaryLevelAnimator != null)
        {
            summaryLevelAnimator.SetTrigger("Out");
        }

        StartCoroutine(HideSummaryAndContinue(1f)); // tempo da animação de saída
    }

    IEnumerator HideSummaryAndContinue(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (summaryLevelObject != null)
            summaryLevelObject.SetActive(false);
        
        if (sfxManager != null)
        {
            sfxManager.StopSoundtrack();
        }

        // Avisa o Manager que o Summary fechou
        if (manager != null)
            manager.OnSummaryClosed();
    }
}
