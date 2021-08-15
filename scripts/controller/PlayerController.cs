using System;

public class PlayerController : DAO
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Save(Player p)
    {
        //aqui entra todo o sql do player  para a criação no bd, contudo o controle n deveria ter 
        //esse tipo de conteudo, posteriormente devo fazer um PlayerDAO onde cou colocar somente os
        //sql do player

        throw new NotImplementedException();
    }
}
