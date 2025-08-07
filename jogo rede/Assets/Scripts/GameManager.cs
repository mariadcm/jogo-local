using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public int vidaJogador1 = 100;
    public int vidaJogador2 = 100;

    // 🔥 Queimadura
    private int turnosQueimandoJ1 = 0;
    private int turnosQueimandoJ2 = 0;

    // ❄️ Congelamento
    private int turnoCongeladoJ1 = 0;
    private int turnoCongeladoJ2 = 0;

    public TextMeshProUGUI textoVida1;
    public TextMeshProUGUI textoVida2;
    public TextMeshProUGUI textoTurno;

    public Button botaoAtacar1;
    public Button botaoAtacar2;
    public GameObject botaoReiniciar;

    private int turnoAtual = 1;

    void Start()
    {
        botaoAtacar1.onClick.AddListener(() => Atacar(1));
        botaoAtacar2.onClick.AddListener(() => Atacar(2));
        ProcessarTurnoAtual(); // começa aplicando efeitos, se houver
    }

    void Atacar(int jogador)
    {
        float chance = Random.value;

        if ((turnoAtual == 1 && jogador == 1) || (turnoAtual == 2 && jogador == 2))
        {
            if (jogador == 1)
            {
                int dano = Random.Range(10, 21);
                vidaJogador2 -= dano;

                if (chance < 0.2f)
                {
                    turnosQueimandoJ2 = 2;
                    StartCoroutine(MostrarMensagemTemporaria("Jogador 1 causou QUEIMADURA!"));
                }
                else if (chance < 0.4f)
                {
                    turnoCongeladoJ2 = 1;
                    StartCoroutine(MostrarMensagemTemporaria("Jogador 1 congelou o inimigo!"));
                }
            }
            else
            {
                int dano = Random.Range(10, 21);
                vidaJogador1 -= dano;

                if (chance < 0.2f)
                {
                    turnosQueimandoJ1 = 2;
                    StartCoroutine(MostrarMensagemTemporaria("Jogador 2 causou QUEIMADURA!"));
                }
                else if (chance < 0.4f)
                {
                    turnoCongeladoJ1 = 1;
                    StartCoroutine(MostrarMensagemTemporaria("Jogador 2 congelou o inimigo!"));
                }
            }

            turnoAtual = (turnoAtual == 1) ? 2 : 1;
            VerificarFimDeJogo();
            ProcessarTurnoAtual(); // aplica efeitos logo no início do novo turno
        }
    }

    void ProcessarTurnoAtual()
    {
        if (vidaJogador1 <= 0 || vidaJogador2 <= 0)
            return;

        if (turnoAtual == 1)
        {
            if (turnoCongeladoJ1 > 0)
            {
                turnoCongeladoJ1 = 0;
                StartCoroutine(MostrarMensagemTemporaria("Jogador 1 está congelado!"));
                turnoAtual = 2;
                Invoke("ProcessarTurnoAtual", 1f); // espera e processa o próximo turno
                return;
            }

            if (turnosQueimandoJ1 > 0)
            {
                vidaJogador1 -= 5;
                turnosQueimandoJ1--;
                StartCoroutine(QueimarEContinuar("Jogador 1 está queimando! -5 de vida"));

            }
        }
        else
        {
            if (turnoCongeladoJ2 > 0)
            {
                turnoCongeladoJ2 = 0;
                StartCoroutine(MostrarMensagemTemporaria("Jogador 2 está congelado!"));
                turnoAtual = 1;
                Invoke("ProcessarTurnoAtual", 1f);
                return;
            }

            if (turnosQueimandoJ2 > 0)
            {
                vidaJogador2 -= 5;
                turnosQueimandoJ2--;
                StartCoroutine(QueimarEContinuar("Jogador 2 está queimando! -5 de vida"));

            }
        }

        AtualizarInterface();
    }

    void AtualizarInterface()
    {
        textoVida1.text = "Vida J1: " + Mathf.Max(vidaJogador1, 0);
        textoVida2.text = "Vida J2: " + Mathf.Max(vidaJogador2, 0);
        textoTurno.text = "Turno do Jogador " + turnoAtual;

        botaoAtacar1.interactable = (turnoAtual == 1);
        botaoAtacar2.interactable = (turnoAtual == 2);
    }

    void VerificarFimDeJogo()
    {
        if (vidaJogador1 <= 0)
        {
            textoTurno.text = "Jogador 2 Venceu!";
            botaoAtacar1.interactable = false;
            botaoAtacar2.interactable = false;
            botaoReiniciar.SetActive(true);
        }
        else if (vidaJogador2 <= 0)
        {
            textoTurno.text = "Jogador 1 Venceu!";
            botaoAtacar1.interactable = false;
            botaoAtacar2.interactable = false;
            botaoReiniciar.SetActive(true);
        }
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    IEnumerator QueimarEContinuar(string mensagem)
    {
        textoTurno.text = mensagem;
        yield return new WaitForSeconds(1f);
        AtualizarInterface();
    }


    IEnumerator MostrarMensagemTemporaria(string mensagem)
    {
        textoTurno.text = mensagem;
        yield return new WaitForSeconds(1f);
        textoTurno.text = "Turno do Jogador " + turnoAtual;

        AtualizarInterface();
    }
}
