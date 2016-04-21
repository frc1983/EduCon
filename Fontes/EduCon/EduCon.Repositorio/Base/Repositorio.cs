using EduCon.Utilitarios.Contexto;

namespace EduCon.Repositorio.Base
{
    public class Repositorio<T> : RepositorioBase<T>
        where T : class
    {
        public Repositorio() : base() { }
    }
}