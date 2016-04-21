using System;

namespace EduCon.Utilitarios.Dominio.Interfaces
{
    public interface ITransacao : IDisposable
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}