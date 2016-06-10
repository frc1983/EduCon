namespace EduCon.Base.Dominio.Interfaces.Validacoes
{
    public interface IValidaExclui<T> : IRegraValidacao<T>
    {
        void ValidaExclui(T entidade);
    }
}