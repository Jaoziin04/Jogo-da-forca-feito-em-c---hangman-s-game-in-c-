// 21239 - João Victor Cruz Ribeiro e 21250 - Maria Júlia Hofsetter Trevisan Pereira 
using System;
using System.IO;
using System.Windows.Forms;
class Dicionario
{
    const int inicoPalavra = 0,
              tamanhoPalavra = 15,
              inicioDica = tamanhoPalavra,
              tamanhoDica = 75;
         

  string  palavra, dica;

    bool[] acertou = new bool[15];

    public string Palavra 
    { 
        get => palavra; 
        set
        {
            if (value.Length < tamanhoPalavra) // se o valor for menor que o tamanho da palavra 
            {
                value.PadRight(tamanhoPalavra); // prenche o que falta com espaços a direia
                palavra = value;
            }
            palavra = value;
        }
    }

    public string Dica 
    { 
        get => dica; 
        set
        {
            if (value.Length < tamanhoDica) // se o valor for menor que o tamanho da dica
            {
                value.PadRight(tamanhoDica); // prenche o que falta com espaços a direia
                dica = value;
            }
            dica = value;
        }
    }

    public Dicionario(string palavra, string dica)
  {
    Palavra = palavra;
    Dica = dica;

    for(int i = 0; i < acertou.Length; i++)
        {
            acertou[i] = false;
        }
  }

  public Dicionario()
  {

     for (int i = 0; i < acertou.Length; i++)
        {
            acertou[i] = false;
        }
    }

 
  public void LerDados(StreamReader arq) 
  { 
    if (!arq.EndOfStream) 
    { 
      string linha = arq.ReadLine(); 
      Palavra = linha.Substring(inicoPalavra, tamanhoPalavra); // lê do inico da palavra té seu final
      Dica = linha.Substring(inicioDica, tamanhoDica);      // lê do inicio da dica até seu final
    } 

        
  }
  public String FormatoDeArquivo() 
  {
        return Palavra.ToString().PadLeft(tamanhoPalavra, ' ') +
               Dica;
  }


    public bool Acertou(char letra)
    {
        bool acerto = false;

        for (int cont = 0; cont < Palavra.Length; cont++) // contador vai de zero, até o tamanho da palavra
        {
            if(Palavra[cont] == letra) 
            {
                acertou[cont] = true;  // a posição onde o contador está recebe true
                acerto = true;         
            }
           
        }
        return
            acerto;
            
    }



    


}

