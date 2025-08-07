using UnityEngine;

public class RotacaoGalaxia : MonoBehaviour
{
    public float velocidadeRotacao = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up * velocidadeRotacao * Time.deltaTime);
    }
}
