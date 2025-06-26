using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int vidaJogador1 = 100;
    public int vidaJogador2 = 100;
    // 🔥 Efeitos de queimadura (2 turnos)
    private int turnosQueimandoJ1 = 0;
    private int turnosQueimandoJ2 = 0;

// ❄️ Congelamento (1 turno sem jogar)
    private int turnoCongeladoJ1 = 0;
    private int turnoCongeladoJ2 = 0;


    public TextMeshProUGUI textoVida1;
    public TextMeshProUGUI textoVida2;
    public TextMeshProUGUI textoTurno;


    public Button botaoAtacar1;
    public Button botaoAtacar2;
    public GameObject botaoReiniciar;

    private int turnoAtual = 1; // 1 = Jogador 1, 2 = Jogador 2

    void Start()
    {
        AtualizarInterface();
        botaoAtacar1.onClick.AddListener(() => Atacar(1));
        botaoAtacar2.onClick.AddListener(() => Atacar(2));
    }

    void Atacar(int jogador)
    {
        float chance = Random.value; // número entre 0 e 1

        if (jogador == 1)
        {
            int dano = Random.Range(10, 21);
            vidaJogador2 -= dano;

            if (chance < 0.2f)
            {
                turnosQueimandoJ2 = 2;
                textoTurno.text = "Jogador 1 causou QUEIMADURA!";
            }
            else if (chance < 0.4f)
            {
                turnoCongeladoJ2 = 1;
                textoTurno.text = "Jogador 1 congelou o inimigo!";
            }
        }
        else if (jogador == 2)
        {
            int dano = Random.Range(10, 21);
            vidaJogador1 -= dano;

            if (chance < 0.2f)
            {
                turnosQueimandoJ1 = 2;
                textoTurno.text = "Jogador 2 causou QUEIMADURA!";
            }
            else if (chance < 0.4f)
            {
                turnoCongeladoJ1 = 1;
                textoTurno.text = "Jogador 2 congelou o inimigo!";
            }
        }

        void AtualizarInterface()
        {
            // Aplica efeitos antes de liberar o botão do jogador

            if (turnoAtual == 1)
            {
                if (turnoCongeladoJ1 > 0)
                {
                    textoTurno.text = "Jogador 1 está congelado!";
                    turnoCongeladoJ1 = 0;
                    turnoAtual = 2; // passa a vez
                    return;
                }

                if (turnosQueimandoJ1 > 0)
                {
                    vidaJogador1 -= 5;
                    textoTurno.text = "Jogador 1 está queimando! -5 de vida";
                    turnosQueimandoJ1--;
                }
            }
            else if (turnoAtual == 2)
            {
                if (turnoCongeladoJ2 > 0)
                {
                    textoTurno.text = "Jogador 2 está congelado!";
                    turnoCongeladoJ2 = 0;
                    turnoAtual = 1; // passa a vez
                    return;
                }

                if (turnosQueimandoJ2 > 0)
                {
                    vidaJogador2 -= 5;
                    textoTurno.text = "Jogador 2 está queimando! -5 de vida";
                    turnosQueimandoJ2--;
                }
            }

            // Exibe turnos e ativa botões
            textoVida1.text = "Vida J1: " + Mathf.Max(vidaJogador1, 0);
            textoVida2.text = "Vida J2: " + Mathf.Max(vidaJogador2, 0);
            textoTurno.text = "Turno do Jogador " + turnoAtual;

            botaoAtacar1.interactable = (turnoAtual == 1);
            botaoAtacar2.interactable = (turnoAtual == 2);
        }

        if ((turnoAtual == 1 && jogador == 1) || (turnoAtual == 2 && jogador == 2))
        {
            if (jogador == 1)
                vidaJogador2 -= Random.Range(10, 21); // ataque de 10 a 20
            else
                vidaJogador1 -= Random.Range(10, 21);

            turnoAtual = (turnoAtual == 1) ? 2 : 1;
            VerificarFimDeJogo();
            AtualizarInterface();
        }
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
            botaoReiniciar.SetActive(true); // Mostra o botão!
        }
        else if (vidaJogador2 <= 0)
        {
            textoTurno.text = "Jogador 1 Venceu!";
            botaoAtacar1.interactable = false;
            botaoAtacar2.interactable = false;
            botaoReiniciar.SetActive(true); // Mostra o botão!
        }
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
