using System;
using System.Collections.Generic;

namespace EduCon.Base.Aplicacao.Interfaces
{
    public interface IAplServico<T> : IDisposable
        where T : class
    {
        void Inclui(T dto);
        void Inclui(IEnumerable<T> dtos);
        void Altera(T dto);
        void Exclui(T dto);

        // Consulta
        T Consulta(int id);

        // Lista
        IEnumerable<T> ListaTodos();
        IEnumerable<T> Lista(T filtro);
    }
}