using ServiceSupplyChain.Class;
using ServiceSupplyChain.Class.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSupplyChain
{
    class Program
    {
        static void Main(string[] args)
        {

            var idLogMaster = LogHelper.GerarLogMaster("Processo Iniciado.");

            var connection = new ConnectionHelper();

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "grupoProduto.Sync");
                GrupoProdutoRepository grupoProduto = new GrupoProdutoRepository(connection);
                grupoProduto.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "grupoProduto.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "grupoProduto.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "dep.Sync");
                connection = new ConnectionHelper();
                DepositoHelper dep = new DepositoHelper(connection);
                dep.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "dep.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "dep.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "config.Sync");
                connection = new ConnectionHelper();
                ConfigGeraisHelper config = new ConfigGeraisHelper(connection);
                config.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "config.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "config.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "subGrupoProduto.Sync");
                connection = new ConnectionHelper();
                SubGrupoProdutoHelper subGrupoProduto = new SubGrupoProdutoHelper(connection);
                subGrupoProduto.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "subGrupoProduto.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "subGrupoProduto.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "prod.Sync");
                connection = new ConnectionHelper();
                ProdutoHelper prod = new ProdutoHelper(connection);
                prod.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "prod.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "prod.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "estr.Sync");
                connection = new ConnectionHelper();
                EstruturaProdutoHelper estr = new EstruturaProdutoHelper(connection);
                estr.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "estr.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "estr.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relCadeia.ReadData");
                connection = new ConnectionHelper();
                RelCadeiaSuprimentosHelper relCadeia = new RelCadeiaSuprimentosHelper(connection);
                relCadeia.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relCadeia.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relCadeia.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoSaldo.ReadData");
                connection = new ConnectionHelper();
                ResumoSaldoEstoqueHelper resumoSaldo = new ResumoSaldoEstoqueHelper(connection);
                resumoSaldo.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoSaldo.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoSaldo.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoMovto.ReadData");
                connection = new ConnectionHelper();
                ResumoMovtoHelper resumoMovto = new ResumoMovtoHelper(connection);
                resumoMovto.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoMovto.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoMovto.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoEntradas.ReadData");
                connection = new ConnectionHelper();
                ResumoEntradasHelper resumoEntradas = new ResumoEntradasHelper(connection);
                resumoEntradas.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoEntradas.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoEntradas.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoProdMod.ReadData");
                connection = new ConnectionHelper();
                RelResumoProducaoHelper resumoProdMod = new RelResumoProducaoHelper(connection);
                resumoProdMod.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoProdMod.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoProdMod.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoMovtoFornecedorExterno.ReadData");
                connection = new ConnectionHelper();
                ResumoMovtoFornecExternoHelper resumoMovtoFornecedorExterno = new ResumoMovtoFornecExternoHelper(connection);
                resumoMovtoFornecedorExterno.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoMovtoFornecedorExterno.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoMovtoFornecedorExterno.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoProd.ReadData");
                connection = new ConnectionHelper();
                ResumoSaidasProdHelper resumoProd = new ResumoSaidasProdHelper(connection);
                resumoProd.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoProd.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoProd.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "resumoSaidas.ReadData");
                connection = new ConnectionHelper();
                ResumoSaidasHelper resumoSaidas = new ResumoSaidasHelper(connection);
                resumoSaidas.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "resumoSaidas.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "resumoSaidas.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "prevProducao.Sync");
                connection = new ConnectionHelper();
                ProgramacaoProducao prevProducao = new ProgramacaoProducao(connection);
                prevProducao.Sync();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "prevProducao.Sync");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "prevProducao.Sync");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relGerencialCadeiaSuprimentos.ReadData");
                connection = new ConnectionHelper();
                RelGerencialCadeiaSuprimentos relGerencialCadeiaSuprimentos = new RelGerencialCadeiaSuprimentos(connection);
                relGerencialCadeiaSuprimentos.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relGerencialCadeiaSuprimentos.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relGerencialCadeiaSuprimentos.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relGerencial.ReadData");
                connection = new ConnectionHelper();
                RelGerencialHelper relGerencial = new RelGerencialHelper(connection);
                relGerencial.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relGerencial.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relGerencial.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relEtapa.ReadData");
                connection = new ConnectionHelper();
                RelSaldoEtapaProcesso relEtapa = new RelSaldoEtapaProcesso(connection);
                relEtapa.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relEtapa.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relEtapa.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relAprovTriagem.ReadData");
                connection = new ConnectionHelper();
                RelAproveitamentoTriagem relAprovTriagem = new RelAproveitamentoTriagem(connection);
                relAprovTriagem.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relAprovTriagem.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relAprovTriagem.ReadData");
            }

            try
            {
                LogHelper.GerarLogItem(idLogMaster, "Iniciando Metodo.", "relPcUtilizacao.ReadData");
                connection = new ConnectionHelper();
                RelPcUtilizacaoComponentesHelper relPcUtilizacao = new RelPcUtilizacaoComponentesHelper(connection);
                relPcUtilizacao.ReadData();
                LogHelper.GerarLogItem(idLogMaster, "Metodo finalizado com sucesso.", "relPcUtilizacao.ReadData");
            }
            catch (Exception ex)
            {
                LogHelper.GerarLogItem(idLogMaster, ex.Message, "relPcUtilizacao.ReadData");
            }

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("Processo encerrado");
            //Console.ReadKey();
            LogHelper.GerarLogMasterFinalizada(idLogMaster, "Processo Finalizado.");
        }
    }
}
