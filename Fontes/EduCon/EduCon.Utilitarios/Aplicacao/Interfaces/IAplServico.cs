using System;
using System.Collections.Generic;

namespace EduCon.Utilitarios.Aplicacao.Interfaces
{
    public interface IAplServico<T> : IDisposable
        where T : class
    {
        void Inclui(T dto);
        void Altera(T dto);
        void Exclui(T dto);

        // Consulta
        T Consulta(int id);

        // Lista
        IEnumerable<T> ListaTodos();
        IEnumerable<T> Lista(T filtro);
    }
}