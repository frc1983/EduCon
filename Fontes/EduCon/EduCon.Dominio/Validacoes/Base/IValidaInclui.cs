namespace EduCon.Dominio.Validacoes.Base
{
    public interface IValidaInclui<T> : IRegraValidacao<T>
    {
        void ValidaInclui(T entidade);
    }
}