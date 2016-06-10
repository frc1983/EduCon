namespace EduCon.Dominio.Validacoes.Base
{
    public interface IValidaAltera<T> : IRegraValidacao<T>
    {
        void ValidaAltera(T entidade);
    }
}