using CommonServiceLocator.SimpleInjectorAdapter;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;

namespace EduCon.Injecao
{
    public class Injeta
    {
        public static void RegistraModulos(Container container)
        {
            (new Modulos.Contexto()).RegisterServices(container);
            (new Modulos.Repositorio()).RegisterServices(container);
            (new Modulos.Servico()).RegisterServices(container);
            (new Modulos.Aplicacao()).RegisterServices(container);

            ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));
        }
    }
}