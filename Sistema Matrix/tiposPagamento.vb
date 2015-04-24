﻿Imports System.Data.OleDb
Imports System.Data
Imports Sistema_Matrix.ConexaoAccess
Public Class tiposPagamento
    'Armazena o valor da PK
    Public valorCodigo As Integer
    'Variável que recebe o valor retornado pela função que verifica se os campos estão vazios
    Private valorRetornado As Integer
    'Instância da classe de conexão ao banco
    Private objBanco As New ConexaoAccess
    'Variável que le os dados da tabela
    Private tabela As DataTable
    'Variável usada para os comandos select
    Private leitor As OleDbDataReader
    'String que armazena os comandos SQL
    Private strSQL As String
    Private Sub geraCodigo()
        'Atribui o valor retornado pela função atribuiCodigo a textbox do Código
        valorCodigo = atribuiCodigo("tpaCodigo", "tiposPagamento")
        txtCodigo.Text = valorCodigo
    End Sub
    Private Sub botCadastrar_Click(sender As Object, e As EventArgs) Handles botCadastrar.Click
        'Armazena na variável o valor que foi retornado pela função, o argumento é o próprio form
        valorRetornado = Funcoes.verificaVazio(Me)

        'Teste se o valor retornado é 0 ou mais, para poder proceder
        If (valorRetornado = 0) Then
            'Instrução SQL
            strSQL = "INSERT INTO tiposPagamento (tpaCodigo, tpaDescricao) VALUES (" & valorCodigo & ", '" & txtDescricao.Text & "')"
            'Executa a instrução de inserção no banco através do objeto da classe ConexaoAccess
            objBanco.ExecutaQuery(strSQL)
            MsgBox("Dados gravados com sucesso", vbInformation, "Aviso")
            'Função para limpar os campos do formulário
            Funcoes.Limpar(Me)
            'Chama a função privada para atribuir código
            geraCodigo()
            'Sub que fecha as conexões com banco e zera as variáveis usadas
            zeraVariaveisBanco()
        End If

    End Sub

    Private Sub tiposPagamento_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Chama a função privada para atribuir código
        geraCodigo()
        'String que seleciona os dados
        strSQL = "SELECT tpaCodigo as Código, tpaDescricao as Descrição FROM tiposPagamento"
        'Procedimento da classe conexão access que carrega os dados no datagrid
        objBanco.carregaDataGrid(dtgConsultaTiposPagamento, strSQL)

        'Atribui uma largura as colunas
        dtgConsultaTiposPagamento.Columns(0).Width = 50
        dtgConsultaTiposPagamento.Columns(1).Width = 200

    End Sub
    Private Sub botLimpar_Click(sender As Object, e As EventArgs) Handles botLimpar.Click
        'Função para limpar os campos do formulário
        Funcoes.Limpar(Me)
        'Chama a função privada para atribuir código
        geraCodigo()
    End Sub
    Private Sub tabConsulta_Enter(sender As Object, e As EventArgs) Handles tabConsulta.Enter
        strSQL = "SELECT tpaCodigo as Código, tpaDescricao as Descrição FROM tiposPagamento"
        objBanco.carregaDataGrid(dtgConsultaTiposPagamento, strSQL)
        'Atualiza o datagrid
        dtgConsultaTiposPagamento.Refresh()
        'Coloca a seleção no datagrid
        dtgConsultaTiposPagamento.Select()

    End Sub
    Private Sub dtgConsultaTiposPagamento_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgConsultaTiposPagamento.CellDoubleClick
        Try
            'Armazena na variável o código da linha que será utilizada na clausula where do select
            valorCodigo = dtgConsultaTiposPagamento.CurrentRow.Cells(0).Value

            'Seleciona a tab de cadastro
            abaTiposPagamento.SelectTab(0)

            'Função que mostra os botões de alteração
            mostraBotoesAlteracao()

            'String com comando SQL
            strSQL = "SELECT * FROM tiposPagamento WHERE tpaCodigo = " & valorCodigo
            leitor = objBanco.ExecutaDataRead(strSQL)
            'Le os dados
            leitor.Read()
            'Atribui os dados aos campos
            txtCodigo.Text = leitor.Item(0)
            txtDescricao.Text = leitor.Item(1)
        Catch exc As SqlClient.SqlException
            MsgBox("Erro com banco de dados" & vbCrLf & Err.Description, vbCritical, "Erro com Banco de dados")
        Catch exc As Exception
            MsgBox("Erro" & vbCrLf & Err.Number & vbCrLf & Err.Description, vbCritical, "Erro")
        Finally
            'Sub que fecha as conexões com banco e zera as variáveis usadas
            zeraVariaveisBanco()
        End Try
    End Sub
    Private Sub botModoNovo_Click(sender As Object, e As EventArgs) Handles botModoNovo.Click
        escondeBotoesAlteracao()
        Funcoes.Limpar(Me)
        geraCodigo()
    End Sub

    Private Sub botAlterar_Click(sender As Object, e As EventArgs) Handles botAlterar.Click
        Try
            'String com comando SQL
            strSQL = "UPDATE tiposPagamento SET tpaDescricao = '" & txtDescricao.Text & "' WHERE tpaCodigo = " & txtCodigo.Text
            'Executa a instrução
            objBanco.ExecutaQuery(strSQL)
            MsgBox("Dados alterados com sucesso", vbInformation, "Sucesso")
            escondeBotoesAlteracao()
            Funcoes.Limpar(Me)
            geraCodigo()
        Catch exc As SqlClient.SqlException
            MsgBox("Erro com banco de dados" & vbCrLf & Err.Description, vbCritical, "Erro com Banco de dados")
        Catch exc As Exception
            MsgBox("Erro" & vbCrLf & Err.Number & vbCrLf & Err.Description, vbCritical, "Erro")
        Finally
            'Sub que fecha as conexões com banco e zera as variáveis usadas
            zeraVariaveisBanco()
        End Try
    End Sub
    Private Sub botExcluir_Click(sender As Object, e As EventArgs) Handles botExcluir.Click
        Try
            'Mensagem de confirmação
            If MsgBox("Deseja realmente excluir este registro?", vbExclamation + vbYesNo, "Confirme") = vbYes Then
                'String com comando SQL
                strSQL = "DELETE FROM tiposPagamento WHERE tpaCodigo = " & txtCodigo.Text
                'Executa a instrução
                objBanco.ExecutaQuery(strSQL)
                MsgBox("Dados excluídos com sucesso", vbInformation, "Sucesso")
                'Esconde os botões de alteração e exclusão
                escondeBotoesAlteracao()
                'Limpa os campos
                Funcoes.Limpar(Me)
                'Gera a PK
                geraCodigo()
            End If
        Catch exc As SqlClient.SqlException
            MsgBox("Erro com banco de dados" & vbCrLf & Err.Description, vbCritical, "Erro com Banco de dados")
        Catch exc As Exception
            MsgBox("Erro" & vbCrLf & Err.Number & vbCrLf & Err.Description, vbCritical, "Erro")
        Finally
            'Sub que fecha as conexões com banco e zera as variáveis usadas
            zeraVariaveisBanco()
        End Try
    End Sub
    Private Sub mostraBotoesAlteracao()
        'Exibe os botões de alteração e exclusão e oculta os de adição
        botCadastrar.Visible = False
        lblCadastrar.Visible = False
        botLimpar.Visible = False
        lblLimpar.Visible = False

        botAlterar.Visible = True
        lblAlterar.Visible = True
        botExcluir.Visible = True
        lblExcluir.Visible = True
        botModoNovo.Visible = True
        lblInserir.Visible = True
    End Sub
    Private Sub escondeBotoesAlteracao()
        botCadastrar.Visible = True
        lblCadastrar.Visible = True
        botLimpar.Visible = True
        lblLimpar.Visible = True

        botAlterar.Visible = False
        lblAlterar.Visible = False
        botExcluir.Visible = False
        lblExcluir.Visible = False
        botModoNovo.Visible = False
        lblInserir.Visible = False
    End Sub
    Private Sub zeraVariaveisBanco()
        'Fecha a conexão com banco
        objBanco.DesconectarBanco()
        'Zera a variável strSQL
        strSQL = String.Empty
        'Testa se a variável leitor foi alterada, se sim a conexão com banco de dados será fechada
        If leitor IsNot Nothing Then
            leitor.Close()
            leitor = Nothing
        End If
    End Sub
End Class