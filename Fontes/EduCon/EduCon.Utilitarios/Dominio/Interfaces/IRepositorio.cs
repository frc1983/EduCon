using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EduCon.Utilitarios.Dominio.Interfaces
{
    public interface IRepositorio<T>
        where T : class
    {
        void Inclui(T entidade);
        void Altera(T entidade);
        void Exclui(T entidade);

        // Consulta
        T Consulta(int id);
        T Consulta(Expression<Func<T, bool>> expressao);

        // Lista
        IEnumerable<T> ListaTodos();
        IEnumerable<T> Lista(Expression<Func<T, bool>> expressao);
    }
}