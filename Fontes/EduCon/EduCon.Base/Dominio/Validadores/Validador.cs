using System;
using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces.Validacoes;

namespace EduCon.Base.Dominio.Validadores
{
    public class Validador<T>
    {
        private T _entidade;

        private IList<IRegraValidacao<T>> _validacoes;

        public Validador(T entidade)
        {
            _entidade = entidade;
            _validacoes = new List<IRegraValidacao<T>>();
        }

        public void Adiciona(IRegraValidacao<T> regra)
        {
            _validacoes.Add(regra);
        }

        public void Adiciona(IList<IRegraValidacao<T>> regras)
        {
            foreach (var regra in regras)
            {
                Adiciona(regra);
            }
        }

        public void Executa(TipoOperacao operacao)
        {
            ExecutarValidacoes(ExecutaValidacaoGenerica);

            switch (operacao)
            {
                case TipoOperacao.Inclusao:
                    ExecutarValidacoes(ExecutaValidacaoInclui);
                    break;
                case TipoOperacao.Alteracao:
                    ExecutarValidacoes(ExecutaValidacaoAltera);
                    break;
                case TipoOperacao.Exclusao:
                    ExecutarValidacoes(ExecutaValidacaoExclui);
                    break;
            }
        }

        private void ExecutarValidacoes(Action<object> executor)
        {
            foreach (var validator in _validacoes)
            {
                executor(validator);
            }
        }

        private void ExecutaValidacaoGenerica(object validador)
        {
            if (validador is IValida<T>)
            {
                ((IValida<T>)validador).Valida(_entidade);
            }
        }

        private void ExecutaValidacaoInclui(object validador)
        {
            if (validador is IValidaInclui<T>)
            {
                ((IValidaInclui<T>)validador).ValidaInclui(_entidade);
            }
        }

        private void ExecutaValidacaoAltera(object validador)
        {
            if (validador is IValidaAltera<T>)
            {
                ((IValidaAltera<T>)validador).ValidaAltera(_entidade);
            }
        }

        private void ExecutaValidacaoExclui(object validador)
        {
            if (validador is IValidaExclui<T>)
            {
                ((IValidaExclui<T>)validador).ValidaExclui(_entidade);
            }
        }
    }
}