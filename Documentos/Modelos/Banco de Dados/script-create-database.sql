create database educon_bd;
go

create table EDC_MUNICIPIO (
    ID int primary key identity,
    COD_IBGE int,
    AGRUPADOR int,
    NOME varchar(200),
    LATITUDE decimal(9, 6),
    LONGITUDE decimal(9, 6)
);

create table EDC_TIPO_ENSINO (
    ID int primary key identity,
    NOME varchar(200)
);

create table EDC_CATEGORIA (
    ID int primary key identity,
    NOME varchar(200)
);

create table EDC_DATA (
    ID int primary key identity,
    ANO int
);

create table EDC_DADO (
    ID int primary key identity,
    ID_MUNICIPIO int,
    ID_TIPO_ENSINO int,
    ID_CATEGORIA int,
    ID_SUBCATEGORIA int,
    VALOR varchar(200),
    foreign key (ID_MUNICIPIO) references EDC_MUNICIPIO,
    foreign key (ID_TIPO_ENSINO) references EDC_TIPO_ENSINO,
    foreign key (ID_CATEGORIA) references EDC_CATEGORIA,
    foreign key (ID_SUBCATEGORIA) references EDC_CATEGORIA
);
go