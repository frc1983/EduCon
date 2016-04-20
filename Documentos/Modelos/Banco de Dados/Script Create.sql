CREATE DATABASE EduCon
GO

CREATE TABLE EduCon.dbo.TipoEnsino (
Id int PRIMARY KEY IDENTITY(1,1),
Nome varchar(255)
)

CREATE TABLE EduCon.dbo.Categoria (
Id int PRIMARY KEY IDENTITY(1,1),
Nome varchar(255)
)

CREATE TABLE EduCon.dbo.Municipio (
Id int PRIMARY KEY IDENTITY(1,1),
CodIBGE int
)

CREATE TABLE EduCon.dbo.Dado (
Id INT PRIMARY KEY IDENTITY(1,1),
IdMunicipio int,
IdTipoEnsino int,
IdCategoria int,
IdAno int,
Valor varchar(255),
FOREIGN KEY(IdMunicipio) REFERENCES Municipio (Id),
FOREIGN KEY(IdTipoEnsino) REFERENCES TipoEnsino (Id),
FOREIGN KEY(IdCategoria) REFERENCES Categoria (Id)
)

CREATE TABLE EduCon.dbo.Ano (
Id int PRIMARY KEY IDENTITY(1,1),
Ano INT,
Unidade varchar(255),
Valor int
)

CREATE TABLE EduCon.dbo.UnidadesGeograficas (
CodIBGE int PRIMARY KEY IDENTITY(1,1),
Agrupador int,
Nome varchar(255),
Latitude decimal(9,6),
Longitude decimal(9,6)
)

ALTER TABLE EduCon.dbo.Municipio ADD FOREIGN KEY(CodIBGE) REFERENCES UnidadesGeograficas (CodIBGE)
ALTER TABLE EduCon.dbo.Dado ADD FOREIGN KEY(IdAno) REFERENCES Ano (Id)
