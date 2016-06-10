namespace EduCon.Base.Dominio.Interfaces.Validacoes
{
    public interface IValida<T> : IRegraValidacao<T>
    {
        void Valida(T entidade);
    }
}