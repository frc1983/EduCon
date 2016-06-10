namespace EduCon.Base.Dominio.Interfaces.Validacoes
{
    public interface IValidaInclui<T> : IRegraValidacao<T>
    {
        void ValidaInclui(T entidade);
    }
}