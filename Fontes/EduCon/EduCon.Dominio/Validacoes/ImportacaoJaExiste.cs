using System;
using EduCon.Base.Dominio.Interfaces.Validacoes;
using EduCon.Dominio.Entidades;
using EduCon.Dominio.Interfaces.Servico;
using Microsoft.Practices.ServiceLocation;

namespace EduCon.Dominio.Validacoes
{
    public class ImportacaoJaExiste : IValidaInclui<Processamento>
    {
        public void ValidaInclui(Processamento entidade)
        {
            var servico = ServiceLocator.Current.GetInstance<IProcessamentoServico>();

            if (servico.Existe(o => o.AnoInicial == entidade.AnoInicial && o.AnoFinal == entidade.AnoFinal))
            {
                throw new InvalidOperationException("Ano já cadastrado para importação.");
            }
        }
    }
}