using System;

namespace EduCon.Base.Dominio.Interfaces
{
    public interface ITransacao : IDisposable
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}