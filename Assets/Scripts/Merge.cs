using UnityEngine;

public abstract class Merge : MonoBehaviour
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
        print("si");
    }

    private bool JijiJAJA()
    {
        return (jiji && jaja) || (!jiji && !jaja);
    }
    protected virtual void MergeTest()
    {
        print("arreglao");
    }

    // to-do: mergear

    public bool Mergeado()
    {
        return true;
    }
}
