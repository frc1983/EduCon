using System;
using EduCon.Base.Dominio.Interfaces.Validacoes;

namespace EduCon.Base.Dominio.Validadores
{
    public class EntidadeNula<T> : IValida<T>
    {
        public void Valida(T entidade)
        {
            if (entidade == null)
            {
                throw new InvalidOperationException("Não é possível realizar operação sobre uma entidade nula.");
            }
        }
    }
}