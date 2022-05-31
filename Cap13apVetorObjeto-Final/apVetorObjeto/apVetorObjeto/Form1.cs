// 21239 - João Victor Cruz Ribeiro e 21250 - Maria Júlia Hofsetter Trevisan Pereira 
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace apVetorObjeto
{
  public partial class FrmForca : Form
  {
    vetorDicionario vetDic;
    Dicionario dicio = new Dicionario();

   const int maximoErros = 8;

    int posicaoDeInclusao;
    int tempo = 0;
    int erros = 0;
    int acertos = 0;
    int sorteio;
    int xEnforcado, yEnforcado;

        public FrmForca()
    {
      InitializeComponent();
    }

    private void btnSair_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void FrmFunc_Load(object sender, EventArgs e)
    {
      int indice = 0;
      tsBotoes.ImageList = imlBotoes;
      foreach (ToolStripItem item in tsBotoes.Items)
        if (item is ToolStripButton) // se não é separador:
          (item as ToolStripButton).ImageIndex = indice++;

      vetDic = new vetorDicionario(20); // instancia com vetor dados com 20 posições

      if (dlgAbrir.ShowDialog() == DialogResult.OK)
      {
        var arquivo = new StreamReader(dlgAbrir.FileName);
        while (!arquivo.EndOfStream)
        {
          Dicionario dadoLido = new Dicionario();
          dadoLido.LerDados(arquivo); // método da classe Funcionario
          vetDic.Incluir(dadoLido);   // método de VetorFuncionario – inclui ao final
        }
        arquivo.Close();
        vetDic.PosicionarNoPrimeiro(); // posiciona no 1o registro a visitação nos dados
        AtualizarTela();               // mostra na tela as informações do registro visitado agora 
      }
    }


    private void btnInicio_Click(object sender, EventArgs e)
    {
      vetDic.PosicionarNoPrimeiro();
      AtualizarTela();
    }

    private void btnAnterior_Click(object sender, EventArgs e)
    {
      vetDic.RetrocederPosicao();
      AtualizarTela();
    }

    private void AtualizarTela()
    {
      if (!vetDic.EstaVazio)
      {
        int indice = vetDic.PosicaoAtual;
        txtPalavra.Text = vetDic[indice].Palavra + "";
        txtDica.Text = vetDic[indice].Dica;
        TestarBotoes();
        stlbMensagem.Text = "Registro " + (vetDic.PosicaoAtual + 1) +
        "/" + vetDic.Tamanho;
      }
    }

    private void TestarBotoes()
    {
      btnInicio.Enabled   = true;
      btnAnterior.Enabled = true;
      btnProximo.Enabled  = true;
      btnUltimo.Enabled   = true;
      if (vetDic.EstaNoInicio)
      {   
        btnInicio.Enabled   = false;
        btnAnterior.Enabled = false;
      }

      if (vetDic.EstaNoFim)
      {
        btnProximo.Enabled = false;
        btnUltimo.Enabled = false;
      }
    }
    private void LimparTela()
    {
      txtPalavra.Clear();
      txtDica.Clear();
    }

    private void btnProximo_Click(object sender, EventArgs e)
    {
      vetDic.AvancarPosicao();
      AtualizarTela();
    }

    private void btnUltimo_Click(object sender, EventArgs e)
    {
      vetDic.PosicionarNoUltimo();
      AtualizarTela();
    }

    private void FrmFunc_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (dlgAbrir.FileName != "")  // foi selecionado um arquivo com dados
         vetDic.GravarDados(dlgAbrir.FileName);
    }

    private void btnNovo_Click(object sender, EventArgs e)
    {
      // saímos do modo de navegação e entramos no modo de inclusão:
      vetDic.SituacaoAtual = Situacao.incluindo;

      // preparamos a tela para que seja possível digitar dados do novo funcionário
      LimparTela();

      txtPalavra.ReadOnly = false;
      // colocamos o cursor no campo chave
      txtPalavra.Focus();

      // Exibimos mensagem no statusStrip para instruir o usuário a digitar dados
      stlbMensagem.Text = "Digite a nova palavra do novo funcionário.";
    }

    private void txtMatricula_Leave(object sender, EventArgs e)
    {   
    }

    private void btnSalvar_Click(object sender, EventArgs e)
    {
      if (vetDic.SituacaoAtual == Situacao.incluindo)  // só guarda novo funcionário no vetor se estiver incluindo
      {
                // criamos objeto com o registro do novo funcionário digitado no formulário
                var novoDicio = new Dicionario(txtPalavra.Text, txtNome.Text);
                                      

        vetDic.Incluir(novoDicio, posicaoDeInclusao);

        vetDic.SituacaoAtual = Situacao.navegando;  // voltamos ao mode de navegação

        vetDic.PosicaoAtual = posicaoDeInclusao;

        AtualizarTela();
      }
      else
        if (vetDic.SituacaoAtual == Situacao.editando)
        {
          vetDic[vetDic.PosicaoAtual].Dica = txtNome.Text;
          vetDic.SituacaoAtual = Situacao.navegando;
        }
      btnSalvar.Enabled     = false;    // desabilita pois a inclusão terminou
      txtPalavra.ReadOnly = true;
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show("Deseja realmente excluir?", "Exclusão",
          MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
      {
        vetDic.Excluir(vetDic.PosicaoAtual);
        if (vetDic.PosicaoAtual >= vetDic.Tamanho)
           vetDic.PosicionarNoUltimo();
        AtualizarTela();
      }
    }

    private void btnProcurar_Click(object sender, EventArgs e)
    {
      vetDic.SituacaoAtual = Situacao.pesquisando;  // entramos no modo de busca
      LimparTela();
      txtPalavra.ReadOnly = false;
      txtPalavra.Focus();
      stlbMensagem.Text = "Digite a palavra que deseja procurar.";
    }

    private void btnCancelar_Click(object sender, EventArgs e)
    {
      vetDic.SituacaoAtual = Situacao.navegando;
      AtualizarTela();
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
      // permitimos ao usuario editar o registro atualmente
      // exibido na tela
      vetDic.SituacaoAtual = Situacao.editando;
            if (vetDic.SituacaoAtual == Situacao.editando)
            {
                btnSalvar.Enabled = true; // habilita o botão salvar
                txtPalavra.ReadOnly = true;  // não deixamos usuário alterar palavra (chave primária)

                stlbMensagem.Text = "Digite a nova dica e pressione [Salvar].";
                txtDica.Cursor = Cursors.Hand; // coloca o cursor no txtDica
            }
    }

    private void tpLista_Enter(object sender, EventArgs e)
    {
      vetDic.ExibirDados(dgvCadastro);
    }

        private void btnIncluir_Click(object sender, EventArgs e)
        {
        }

        private void btnIncluir_Leave(object sender, EventArgs e)
        {

            vetDic.SituacaoAtual = Situacao.incluindo; 
                if (txtPalavra.Text == "") // verifica se o usuario digitou algo no txtPalavra
                {
                    MessageBox.Show("Digite uma matrícula válida!");
                    txtPalavra.Cursor = Cursors.Hand;
                }
                else  // temos um valor digitado no txtPalavra
                {
                    int palavraProcurada = posicaoDeInclusao;
                    int posicao;
                    bool achouRegistro = vetDic.Existe(palavraProcurada, out posicao);
                    switch (vetDic.SituacaoAtual)
                    {
                        case Situacao.incluindo:
                            if (achouRegistro)
                            {
                                MessageBox.Show("Palavra repetida! Inclusão cancelada.");
                                vetDic.SituacaoAtual = Situacao.navegando;
                                AtualizarTela(); // exibe novamente o registro que estava na tela antes de esta ser limpa
                            }
                            else  // a matrícula não existe e podemos incluí-la no índice ondeIncluir
                            {     // incluí-la no índice ondeIncluir do vetor interno dados de osFunc
                                txtDica.Cursor = Cursors.Hand;
                                stlbMensagem.Text = "Digite a dica e pressione [Salvar].";
                                btnSalvar.Enabled = true;  // habilita quando é possível incluir
                                posicaoDeInclusao = posicao;  // guarda índice de inclusão em variável global
                            }
                            break;

                        case Situacao.pesquisando:
                            if (achouRegistro)
                            {
                                // a variável posicao contém o índice do funcionário que se buscou
                                vetDic.PosicaoAtual = posicao;   // reposiciona o índice da posição visitada
                                AtualizarTela();
                            }
                            else
                            {
                                MessageBox.Show("Matrícula digitada não foi encontrada.");
                                AtualizarTela();  // reexibir o registro que aparecia antes de limparmos a tela
                            }

                            vetDic.SituacaoAtual = Situacao.navegando;
                            txtPalavra.ReadOnly = true;
                            break;
                    }
                }


        }
        void HabilitarBotoes()
        {
            btnA.Enabled = true;
            btnB.Enabled = true;
            btnC.Enabled = true;
            btnD.Enabled = true;
            btnE.Enabled = true;
            btnF.Enabled = true;
            btnG.Enabled = true;
            btnH.Enabled = true;
            btnI.Enabled = true;
            btnJ.Enabled = true;
            btnK.Enabled = true;
            btnL.Enabled = true;
            btnM.Enabled = true;
            btnN.Enabled = true;
            btnO.Enabled = true;
            btnP.Enabled = true;
            btnQ.Enabled = true;
            btnR.Enabled = true;
            btnS.Enabled = true;
            btnT.Enabled = true;
            btnU.Enabled = true;
            btnV.Enabled = true;
            btnW.Enabled = true;
            btnX.Enabled = true;
            btnY.Enabled = true;
            btnZ.Enabled = true;
            btnÇ.Enabled = true;
            btnÁ.Enabled = true;
            btnÃ.Enabled = true;
            btnÂ.Enabled = true;
            btnÉ.Enabled = true;
            btnÊ.Enabled = true;
            btnÍ.Enabled = true;
            btnÓ.Enabled = true;
            bntÔ.Enabled = true;
            btnÕ.Enabled = true;
            btnÕ.Enabled = true;
            btnÚ.Enabled = true;
            btnInfen.Enabled = true;
            btnEspaço.Enabled = true;
        }

        void DesabilitarBotoes()
        {
            btnA.Enabled = false;
            btnB.Enabled = false;
            btnC.Enabled = false;
            btnD.Enabled = false;
            btnE.Enabled = false;
            btnF.Enabled = false;
            btnG.Enabled = false;
            btnH.Enabled = false;
            btnI.Enabled = false;
            btnJ.Enabled = false;
            btnK.Enabled = false;
            btnL.Enabled = false;
            btnM.Enabled = false;
            btnN.Enabled = false;
            btnO.Enabled = false;
            btnP.Enabled = false;
            btnQ.Enabled = false;
            btnR.Enabled = false;
            btnS.Enabled = false;
            btnT.Enabled = false;
            btnU.Enabled = false;
            btnV.Enabled = false;
            btnW.Enabled = false;
            btnX.Enabled = false;
            btnY.Enabled = false;
            btnZ.Enabled = false;
            btnÇ.Enabled = false;
            btnÁ.Enabled = false;
            btnÃ.Enabled = false;
            btnÂ.Enabled = false;
            btnÉ.Enabled = false;
            btnÊ.Enabled = false;
            btnÍ.Enabled = false;
            btnÓ.Enabled = false;
            bntÔ.Enabled = false;
            btnÕ.Enabled = false;
            btnÕ.Enabled = false;
            btnÚ.Enabled = false;
            btnInfen.Enabled = false;
            btnEspaço.Enabled = false;
        }
        void LimparImagens()
        {
            pbCorda.Visible = true;
            pbFinal.Visible = true;
            pbMeioCorda.Visible = true;
            pbFinal.Visible = true; ;
            pbTriangulo.Visible = true;
            pbQuase.Visible = true;
            pbComeco.Visible = true;
            pbFinal.Visible = true;
            pbFinalCorda.Enabled = true;

            pbAnjo.Visible = false;
            pbBracoDireito.Visible = false;
            pbBracoEsquerdo.Visible = false;
            pbCabeca.Visible = false;
            pbPernaDireita.Visible = false;
            pbPernaEsquerda.Visible = false;
            pbQuadril.Visible = false;
            pbQueixo.Visible = false;
            pbTronco.Visible = false;
            pbCabecaVitoria.Visible = false;
            pbBandeiraBrasil.Visible = false;
            pbBracoBrasileiro.Visible = false;
            pbPedacodaBandeira.Visible = false;
            pbMorto.Visible = false;
        }

        void LiberarArduino()
        {
            lbArduino.Visible = true;
            txtPorta.Visible = true;
            txtPorta.ReadOnly = false;
            btnConectar.Visible = true;
        }




        private void btnIniciar_Click(object sender, EventArgs e)
        {
            dgvCadastro.Visible = false;
            txtPalavra.Visible = false;
            txtDica.Visible = false;
            tsBotoes.Visible = false;

            if (sp.IsOpen)
            {
                string scr = "";

                scr = "I";

                sp.Write(scr);
            }

            HabilitarBotoes();
            LimparImagens();

            // BackColor deixa o botão na cor padrão

            btnA.BackColor = DefaultBackColor;
            btnB.BackColor = DefaultBackColor;
            btnC.BackColor = DefaultBackColor;
            btnD.BackColor = DefaultBackColor; 
            btnE.BackColor = DefaultBackColor; 
            btnF.BackColor = DefaultBackColor;
            btnG.BackColor = DefaultBackColor; 
            btnH.BackColor = DefaultBackColor; 
            btnI.BackColor = DefaultBackColor; 
            btnJ.BackColor = DefaultBackColor; 
            btnK.BackColor = DefaultBackColor; 
            btnL.BackColor = DefaultBackColor; 
            btnM.BackColor = DefaultBackColor; 
            btnN.BackColor = DefaultBackColor; 
            btnO.BackColor = DefaultBackColor; 
            btnP.BackColor = DefaultBackColor; 
            btnQ.BackColor = DefaultBackColor;
            btnR.BackColor = DefaultBackColor; 
            btnS.BackColor = DefaultBackColor; 
            btnT.BackColor = DefaultBackColor; 
            btnU.BackColor = DefaultBackColor; 
            btnV.BackColor = DefaultBackColor;
            btnW.BackColor = DefaultBackColor;
            btnX.BackColor = DefaultBackColor;
            btnY.BackColor = DefaultBackColor;
            btnZ.BackColor = DefaultBackColor;
            btnÇ.BackColor = DefaultBackColor;
            btnÁ.BackColor = DefaultBackColor;
            btnÃ.BackColor = DefaultBackColor;
            btnÂ.BackColor = DefaultBackColor;
            btnÉ.BackColor = DefaultBackColor;
            btnÊ.BackColor = DefaultBackColor;
            btnÍ.BackColor = DefaultBackColor;
            btnÓ.BackColor = DefaultBackColor;
            bntÔ.BackColor = DefaultBackColor;
            btnÕ.BackColor = DefaultBackColor;
            btnÕ.BackColor = DefaultBackColor;
            btnÚ.BackColor = DefaultBackColor;
            btnInfen.BackColor = DefaultBackColor;
            btnEspaço.BackColor = DefaultBackColor;


            dgvPalavra.Rows.Clear();  // Limpar o dgvPalavra 
            lbErros.Text = "Erros: ";
            lbPontos.Text = "Pontos: ";

            chcbDica.Enabled = false;
            btnIniciar.Enabled = false;

            tmrAnjo.Enabled = false;
            tmrAnjo.Stop();

            pbAnjo.Location = new Point(x: 124, y: 41); // faz o anjo voltar a sua posição de origem


            btnConectar.Enabled = true;

            acertos = 0; // zera os erros
            erros = 0; // zera os acertos
            

          
            var alearotio = new Random(); // variavel aleatoria

            sorteio = alearotio.Next(vetDic.Tamanho);  // soeteio tem o tamanho de vetorDicionario - 1 

            dicio = vetDic[sorteio]; // atribui a palavra sorteada a classe dicionario

            string palavra = vetDic[sorteio].Palavra; // string que recebe a palavra sorteada



            palavra.ToString().ToUpper(); // deixa a palavra maíscula
            dgvPalavra.ColumnCount = palavra.Trim().Length; // dgvPalavra recebe a palvra sem os espaços a direita
            

            for (int i = 0; i < dgvPalavra.ColumnCount; i++)
            {
                dgvPalavra.Columns[i].Width = 30;
            }





            if (chcbDica.Checked == true)
            {
                lbDica.Visible = true;
                lbDica.Text = vetDic[sorteio].Dica.ToString(); // dica aparece na tela

                tempo = 180;
                tmrTempo.Interval = 1000;
                tmrTempo.Start();
            }
            else
            {
                lbDica.Visible = false; // dica não aparece na tela
                chcbDica.Enabled = false; // não permite que o usuario exiba a dica após clicar no botão iniciar
                tmrTempo.Stop();
                lbTempo.Visible = false;
            }

       
            


        }


        void MexerAnjo()
        {
            xEnforcado = pbAnjo.Location.X; // xEnforcado recebe a posição de x de pbAnjo
            yEnforcado = pbAnjo.Location.Y; // yEnforcado recebe a posição de y de pbAnjo
            tmrAnjo.Enabled = true;
            tmrAnjo.Start();    // inicia o tmrAnjo
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            char letrinha;

            if ((sender as Button).Text == "" || (sender as Button).Text == null)
                letrinha = ' ';
            else
                letrinha = (sender as Button).Text[0];

          

            lbErros.Text = "Erros: ";
            lbPontos.Text = "Pontos: ";

            (sender as Button).Enabled = false; // não permite que o usuário clique no botão duas vezes
            (sender as Button).BackColor = Color.Red; // quando o usuario clicar no botão ele fica vermelho

          
            if (dicio.Acertou(letrinha))
              {
                int quantasVezes = 0; // indica quantas vezes a letra ocorre na palavra
                int i = 0;

                for (   ; i < dicio.Palavra.TrimEnd().Length; i++)
                {
                    if (dicio.Palavra.TrimEnd()[i] == letrinha) 
                    {
                        quantasVezes++; // se a letra ocorre mais dee uma vez somamos 1 em quantasVezes
                        dgvPalavra.Rows[0].Cells[i].Value = letrinha; // coloca a letra no dgvPalavra
                    }
                }
                acertos += quantasVezes;

               

                if (acertos == dicio.Palavra.TrimEnd().Length)
                {
                    MessageBox.Show("parabéns você ganhou!!!");

                    

                    pbCabeca.Visible = false; 
                    pbCorda.Visible = false; 
                    pbFinal.Visible = false;
                    pbMeioCorda.Visible = false;
                    pbFinal.Visible = false;
                    pbMorto.Visible = false;
                    pbTriangulo.Visible = false;
                    pbQuase.Visible = false;  // deixa de exibir a parte do meio da forca
                    pbComeco.Visible = false; // deixa de exibir a parte debaixo da forca
                    pbFinal.Visible = false;  // deixa de exibir a parte onde a corda é encaixada na forca
                    pbFinalCorda.Visible = false;

                    pbCabecaVitoria.Visible = true; // exibe a cabeça sorrindo
                    pbQueixo.Visible = true;
                    pbTronco.Visible = true;
                    pbBracoBrasileiro.Visible = true; // exibe o braço que segura a bandeira do Brasil
                    pbPedacodaBandeira.Visible = true; // exibe a aste da bandeira e um pedaçod dela
                    pbBandeiraBrasil.Visible = true; // exibe o resto da bandeira do Brasil
                    pbBracoDireito.Visible = true;
                    pbQuadril.Visible = true;
                    pbPernaDireita.Visible = true;
                    pbPernaEsquerda.Visible = true;

                    tsBotoes.Enabled = true; // permite que o usuario aperte os botões novamente
                    txtDica.Visible = true; // volta a exibir o txtDica na aba do cadastro
                    txtPalavra.Visible = true; // volta a exibir o txtPalavra na aba do cadastro
                    dgvCadastro.Visible = true; // volta a exibir o dgvCadastro


                    tmrTempo.Stop();

                    chcbDica.Enabled = true;   // permite ao usuario selecionar a dica novamente
                    btnIniciar.Enabled = true; // permite ao ususario iniciar o jogo novamente

                    DesabilitarBotoes();

                }
            
            }
            else
            {
                   erros++;

                if(chcbArduino.Checked)
                {
                    string str = "";

                    if (erros == 1)
                    {
                        str = "P";
                    }
                    else if (erros == 2)
                    {
                        str = "D";
                    }
                    else if (erros == 3)
                    {
                        str = "T";
                    }
                    else if (erros == 4)
                    {
                        str = "Q";
                    }
                    else if (erros == 5)
                    {
                        str = "C";
                    }
                    else if(erros == 6)
                    {
                        str = "S";
                    }
                    else if(erros == 7)
                    {
                        str = "E";
                    }
                    else if(erros == 8)
                    {
                        str = "O";
                    }
                    sp.Write(str);
                }



                    if (erros == 1)
                    {
                        

                        pbCabeca.Visible = true;
                        pbQueixo.Visible = true;
                   
                    }
                    else if (erros == 2)
                    {
                        pbTronco.Visible = true;
                     
                    }
                    else if (erros == 3)
                    {
                        pbBracoEsquerdo.Visible = true;
                   
                    }
                    else if (erros == 4)
                    {
                        pbBracoDireito.Visible = true;
                       
                    }
                    else if (erros == 5)
                    {
                        pbQuadril.Visible = true;
                      
                    }
                    else if (erros == 6)
                    {
                        pbPernaDireita.Visible = true; // exibe a perna direita
                    
                    }
                    else if (erros == 7)
                    {
                        pbPernaEsquerda.Visible = true;
                      
                    }
                    else if (erros == maximoErros)
                    {
                        DesabilitarBotoes();

                        pbMorto.Visible = true; // exibe a cabeça com os olhos ebugalhados
                        pbCabeca.Visible = false; // deixa de exibir a cabeça triste
                        pbAnjo.Visible = true;  // exibe o anjo
                        pbAnjo.BringToFront();
                        tmrTempo.Stop();
                        MexerAnjo(); // chama o método que irá mexer o anjo

                        tsBotoes.Enabled = true; // permite que o usuario aperte os botões novamente
                        txtDica.Visible = true; // volta a exibir o txtDica na aba do cadastro
                        txtPalavra.Visible = true; // volta a exibir o txtPalavra na aba do cadastro
                        dgvCadastro.Visible = true; // volta a exibir o dgvCadastro

                        chcbDica.Enabled = true; // permite ao usuario selecionar a dica novamente
                        btnIniciar.Enabled = true; // permite ao ususario iniciar o jogo novamente
                        
                    }
              
            }
            lbPontos.Text = acertos.ToString(); // exibe os acertos
            lbErros.Text = erros.ToString(); // exibe os erros
            







        }

   

        private void tmrAnjo_Tick(object sender, EventArgs e)
        {
        
            xEnforcado++; 
            yEnforcado--;
            

            pbAnjo.Location = new Point(x:xEnforcado, y:yEnforcado); // mexe a posição do anjo, conforme o tick

           
        }

        private void chcbArduino_CheckedChanged(object sender, EventArgs e)
        {
            LiberarArduino();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            sp.PortName = txtPorta.Text; // o nome da porta recebe a COM que o jogador digitou no txtPorta
            
                sp.Open();
         
                
        }

        private void tmrTempo_Tick(object sender, EventArgs e)
        {
            lbTempo.Text = tempo + "s";
            tempo--;

            if(tempo == 0)
            {
                tmrTempo.Stop();
                MessageBox.Show("Tempo esgotado, você perdeu");
                

            }
        }
    }
}
