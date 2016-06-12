using System;
using CommonServiceLocator.SimpleInjectorAdapter;
using EduCon.Aplicacao;
using EduCon.Injecao;
using Microsoft.Practices.ServiceLocation;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace EduCon.ImportaFee.Infra
{
    public static class Injecao
    {
        public static Container Inicializa()
        {
            var container = new Container();

            try
            {
                InicializaAplicacao.Inicia();
                container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
                Injeta.RegistraModulos(container);
                ServiceLocator.SetLocatorProvider(() => new SimpleInjectorServiceLocatorAdapter(container));
                container.Verify();
            }
            catch (Exception)
            {
                container.Dispose();
                throw;
            }

            return container;
        }
    }
}