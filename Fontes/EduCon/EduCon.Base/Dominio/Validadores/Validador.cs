using System;
using System.Collections.Generic;
using EduCon.Base.Dominio.Interfaces.Validacoes;

namespace EduCon.Base.Dominio.Validadores
{
    public class Validador<T>
    {
        private T _entidade;

        private IList<IRegraValidacao<T>> _regras;

        public Validador(T entidade)
        {
            _entidade = entidade;
            _regras = new List<IRegraValidacao<T>>();
        }

        public void Adiciona(IRegraValidacao<T> regra)
        {
            _regras.Add(regra);
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

        private void ExecutarValidacoes(Action<object> metodo)
        {
            foreach (var regra in _regras)
            {
                metodo(regra);
            }
        }

        private void ExecutaValidacaoGenerica(object regra)
        {
            if (regra is IValida<T>)
            {
                ((IValida<T>)regra).Valida(_entidade);
            }
        }

        private void ExecutaValidacaoInclui(object regra)
        {
            if (regra is IValidaInclui<T>)
            {
                ((IValidaInclui<T>)regra).ValidaInclui(_entidade);
            }
        }

        private void ExecutaValidacaoAltera(object regra)
        {
            if (regra is IValidaAltera<T>)
            {
                ((IValidaAltera<T>)regra).ValidaAltera(_entidade);
            }
        }

        private void ExecutaValidacaoExclui(object regra)
        {
            if (regra is IValidaExclui<T>)
            {
                ((IValidaExclui<T>)regra).ValidaExclui(_entidade);
            }
        }
    }
}