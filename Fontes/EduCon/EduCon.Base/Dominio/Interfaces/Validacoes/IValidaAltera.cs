namespace EduCon.Base.Dominio.Interfaces.Validacoes
{
    public interface IValidaAltera<T> : IRegraValidacao<T>
    {
        void ValidaAltera(T entidade);
    }
}