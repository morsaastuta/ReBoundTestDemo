using UnityEngine;

public class Merge : MonoBehaviour
{
    [SerializeField] bool jiji;
    [SerializeField] bool jaja;


    void Start()
    {
        print("Los Skibidis ganaron a los toilets");
    }

    // Update is called once per frame
    void Update()
    {
        print("ESto es una prueba muy segura");
    }

    private bool JijiJAJA()
    {
        if (jiji && jaja) return true;
        else if (!jiji && jaja) return false;
        else if (jiji && !jaja) return false;
        else if (!jiji && !jaja) return true;
        return false;
    }
}
