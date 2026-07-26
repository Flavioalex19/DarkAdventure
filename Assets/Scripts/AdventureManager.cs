using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.VFX;

public class AdventureManager : MonoBehaviour
{
    [Header("References")]
    public PlayerStats playerStats;
    public AdventureUI ui;
    public SoPhase currentPhase;

    [Header("Runtime")]
    private List<SoChoice> currentOptions = new List<SoChoice>();
    private SoPhase nextPhaseToLoad;
    private bool isShowingConsequence = false;

    [Header("Cards")]
    public List<SoCard> availableCards;

    [Header("Progression System")]
    public int beginningLevel = 1;
    public int currentLevel;
    public int maxProgression = 5;          
    public int currentProgression = 0;
    public MapProgressionManager mapProgressionManager;

    [Header("SFX")]
    public SFXManager sfxManager;

    void Start()
    {
        /*
        if (currentPhase != null)
        {
            StartPhase(currentPhase);
        }
        */
        currentLevel = beginningLevel;
        currentProgression = 0;
        StartCoroutine(StartWithIntro());
    }
    IEnumerator StartWithIntro()
    {
        // Toca a intro
        ui.PlayIntro();

        // Espera a intro (ajuste o tempo conforme a duração da animação)
        yield return new WaitForSeconds(5f); //  mude esse valor pro tempo da sua animação

        // Só depois inicia a primeira fase
        if (currentPhase != null)
        {
            StartPhase(currentPhase);
        }
    }
    public void StartPhase(SoPhase phase)
    {
        currentPhase = phase;
        currentOptions.Clear();
        isShowingConsequence = false;
        nextPhaseToLoad = null;

        ui.ShowDescription("");

        ui.PlayPhaseIntro(phase.phaseName);//play intro

        /*
        ui.ShowDescription(phase.description);
        ui.ShowContinueButton(false);
        ui.HideDiceArea();

        GenerateRandomOptions();
        */

        // Espera a intro + depois faz o typewriter
        StartCoroutine(StartPhaseAfterIntro(phase));
    }
    IEnumerator StartPhaseAfterIntro(SoPhase phase)
    {
        if (sfxManager != null && phase.SFXAmbience != null)
        {
            sfxManager.PlayAmbience(phase.SFXAmbience);
        }
        // Tempo da animação de intro da fase
        yield return new WaitForSeconds(4f);

        // Desliga o objeto da intro
        ui.HidePhaseIntro();

        if (mapProgressionManager != null)
        {
            mapProgressionManager.MoveToProgressionPoint(currentProgression);
        }

        ui.HideOptions();

        // Começa o efeito typewriter na descrição
        //ui.StartTypewriter(phase.description);
        yield return StartCoroutine(ui.StartTypewriter(phase.description));

        // Mostra as opções (pode colocar um pequeno delay extra se quiser)
        yield return new WaitForSeconds(0.5f); // tempo extra opcional
        GenerateRandomOptions();
    }
    void GenerateRandomOptions()
    {
        if (currentPhase.choicePool == null || currentPhase.choicePool.Count == 0)
        {
            Debug.LogWarning("Essa fase não tem escolhas no pool!");
            return;
        }

        currentOptions = currentPhase.choicePool
            .OrderBy(x => Random.value)
            .Take(3)
            .ToList();

        List<string> optionTexts = new List<string>();
        foreach (var choice in currentOptions)
        {
            optionTexts.Add(choice.optionText);
        }

        ui.ShowOptions(/*optionTexts*/currentOptions);
    }

    public void SelectChoice(int index)
    {
        if (isShowingConsequence) return;
        if (index < 0 || index >= currentOptions.Count) return;

        SoChoice selected = currentOptions[index];
        ui.HideOptions();

        //StartCoroutine(HandleDiceRoll(selected));
        switch (selected.choiceType)
        {
            case ChoiceType.Common:
                // Comportamento normal (o que a gente já tem)
                StartCoroutine(HandleDiceRoll(selected));
                break;

            case ChoiceType.Homestead:
                // Por enquanto pode chamar o mesmo do Common se quiser
                StartCoroutine(HandleHomestead(selected));
                break;

            case ChoiceType.Sin:
                // TODO: Lógica específica de Sin
                StartCoroutine(HandleSin(selected));
                break;

            case ChoiceType.Church:
                // TODO: Lógica específica de Church
                StartCoroutine(HandleHomestead(selected));
                break;

            default:
                StartCoroutine(HandleDiceRoll(selected));
                break;
        }

    }

    IEnumerator HandleDiceRoll(SoChoice choice)
    {
        // Rola os dois dados
        int dice1 = Random.Range(1, 7);
        int dice2 = Random.Range(1, 7);
        int totalRoll = dice1 + dice2;
        float rollPercentage = DiceUtility.RollToPercentage(totalRoll);

        float playerStatValue = GetPlayerStatValue(choice.affectedStat);
        bool success = rollPercentage >= (100f - playerStatValue);

        // Mostra a área dos dados
        ui.ShowDiceRoll(dice1, dice2, success);

        // Espera 2 segundos
        yield return new WaitForSeconds(2f);

        // Esconde a área dos dados
        ui.HideDiceArea();

        bool isBeneficial = IsBeneficialStat(choice.affectedStat);

        float finalAmount;

        if (success)
        {
            // Sucesso: aplica o que é BOM para aquele atributo
            finalAmount = isBeneficial ? choice.amount : -choice.amount;
        }
        else
        {
            // Falha: aplica o que é RUIM para aquele atributo
            finalAmount = isBeneficial ? -choice.amount : choice.amount;
        }

        ApplyStatChange(choice.affectedStat, finalAmount);

        // Atualiza o Disbelief
        if (success)
        {
            // Sucesso: diminui 1 (sem deixar ficar negativo)
            if (playerStats.disbelief > 0)
            {
                playerStats.disbelief--;
            }
        }
        else
        {
            // Falha: aumenta 1
            playerStats.disbelief++;
        }

        // Mostra a consequência correta
        string consequence = success ? choice.positiveConsequence : choice.negativeConsequence;
        ui.ShowConsequence(consequence);
        isShowingConsequence = true;

        // Prepara próxima fase
        nextPhaseToLoad = choice.nextPhase;
        ui.ShowContinueButton(true);

        Debug.Log($"Dados: {dice1} + {dice2} = {totalRoll} ({rollPercentage:F0}%) | Atributo: {playerStatValue} | {(success ? "SUCESSO" : "FALHA")}");
    }
    IEnumerator HandleHomestead(SoChoice choice)
    {
        // Aplica o amount diretamente (sem rolagem, sem inversão)
        ApplyStatChange(choice.affectedStat, choice.amount);

        // Sempre mostra a consequência positiva
        ui.ShowConsequence(choice.positiveConsequence);

        isShowingConsequence = true;

        // Guarda a próxima fase
        nextPhaseToLoad = choice.nextPhase;

        // Mostra o botão Avançar
        ui.ShowContinueButton(true);

        yield return null;
    }
    IEnumerator HandleSin(SoChoice choice)
    {
        // Aplica os dois efeitos diretamente
        ApplyStatChange(choice.affectedStat, choice.amount);
        ApplyStatChange(choice.affectedStat2, choice.amount2);

        // Mostra a consequência (por enquanto a positiva)
        ui.ShowConsequence(choice.positiveConsequence);

        isShowingConsequence = true;
        nextPhaseToLoad = choice.nextPhase;
        ui.ShowContinueButton(true);

        yield return null;
    }
    public void OnContinueClicked()
    {
        
        ui.ShowContinueButton(false);

        // Aumenta a progressão
        currentProgression++;

        // Se chegou no final do level
        if (currentProgression >= maxProgression)
        {
            currentLevel++;
            //currentProgression = 0;

            Debug.Log($"LEVEL UP! Agora está no Level {currentLevel}");

            // Só mostra o summary, NÃO avança pra próxima phase
            ui.PlayLevelSummary();
            return;   //  impede de continuar para StartPhase
        }

        // Só chega aqui se ainda NÃO for o final do level
        if (nextPhaseToLoad != null)
        {
            StartPhase(nextPhaseToLoad);
        }
        else
        {
            ui.ShowDescription("Fim da aventura.");
        }
    }

    float GetPlayerStatValue(StatType stat)
    {
        switch (stat)
        {
            case StatType.HP: return playerStats.currentHP;
            case StatType.Will: return playerStats.currentWill;
            case StatType.Fear: return playerStats.currentFear;
            case StatType.Stress: return playerStats.currentStress;
            case StatType.Attack: return playerStats.currentAttack;
            case StatType.Defense: return playerStats.currentDefense;
            default: return 0f;
        }
    }

    void ApplyStatChange(StatType stat, float amount)
    {
        Debug.Log($"Stat alterado: {stat} | Valor aplicado: {amount}");
        switch (stat)
        {
            case StatType.HP: playerStats.currentHP += amount; break;
            case StatType.Will: playerStats.currentWill += amount; break;
            case StatType.Fear: playerStats.currentFear += amount; break;
            case StatType.Stress: playerStats.currentStress += amount; break;
            case StatType.Attack: playerStats.currentAttack += amount; break;
            case StatType.Defense: playerStats.currentDefense += amount; break;
        }
    }
    bool IsBeneficialStat(StatType stat)
    {
        // Atributos que quanto MAIS, melhor
        switch (stat)
        {
            case StatType.HP:
            case StatType.Will:
            case StatType.Attack:
            case StatType.Defense:
                return true;

            // Atributos que quanto MENOS, melhor
            case StatType.Fear:
                return false;
            case StatType.Stress: 
                return false;

            default:
                return true;
        }
    }
    public void OnSummaryClosed()
    {
        currentProgression = 0;

        if (mapProgressionManager != null)
        {
            mapProgressionManager.MoveToProgressionPoint(0);
        }

        // Transição correta de áudio
        if (sfxManager != null && nextPhaseToLoad != null)
        {
            //sfxManager.TransitionToNewAmbience(nextPhaseToLoad.SFXAmbience);
        }

        if (nextPhaseToLoad != null)
        {
            StartPhase(nextPhaseToLoad);
        }
        else
        {
            ui.ShowDescription("Fim da aventura.");
        }
    }
    IEnumerator HandleCardPath(SoChoice choice)
    {
        // Guarda a próxima fase
        nextPhaseToLoad = choice.nextPhase;

        // Prepara e mostra as cartas
        ui.PrepareCards(availableCards, playerStats);
        ui.ShowCardArea();

        // A partir daqui o fluxo fica pausado até o jogador clicar numa carta.
        // O CardButton vai chamar OnCardSelected() quando for escolhido.
        yield return null;
    }

    public void OnCardSelected()
    {
        // Esconde a área de cartas
        ui.HideCardArea();

        // Mostra o botão Avançar (ou já avança direto, se preferir)
        ui.ShowContinueButton(true);

        // Se quiser pular o botão Avançar e ir direto pra próxima fase, use:
        // OnContinueClicked();
        Debug.Log("Manager recebeu o clique da carta!");
    }
}
