﻿using EduCon.Dominio.Interfaces.Repositorio;
using EduCon.Repositorio;
using EduCon.Repositorio.Base;
using EduCon.Utilitarios.Dominio.Interfaces;
using SimpleInjector;
using SimpleInjector.Packaging;

namespace EduCon.Injecao.Modulos
{
    public class Repositorio : IPackage
    {
        public void RegisterServices(Container container)
        {
            container.Register(typeof(IRepositorio<>), typeof(Repositorio<>), Lifestyle.Scoped);

            container.Register<IMunicipioRepositorio, MunicipioRepositorio>(Lifestyle.Scoped);
        }
    }
}