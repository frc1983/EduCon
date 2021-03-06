create database educon_bd;
go

use educon_bd;
go

/*
drop table EDC_DADO;
drop table EDC_FONTE;
drop table EDC_MUNICIPIO;
drop table EDC_TIPO_ENSINO;
drop table EDC_CATEGORIA;
drop table EDC_DATA;
drop table EDC_PROCESSAMENTO;
*/

create table EDC_MUNICIPIO (
    ID int primary key identity,
    COD_IBGE int not null,
    AGRUPADOR int,
    NOME varchar(200) not null,
    LATITUDE decimal(17, 12),
    LONGITUDE decimal(17, 12)
);

create table EDC_TIPO_ENSINO (
    ID int primary key identity,
    NOME varchar(200) not null
);

create table EDC_CATEGORIA (
    ID int primary key identity,
    NOME varchar(200) not null
);

create table EDC_DATA (
    ID int primary key identity,
    ANO int not null
);

create table EDC_FONTE (
    ID int primary key identity,
    NOME varchar(500) not null
);

create table EDC_DADO (
    ID int primary key identity,
    ID_FONTE int,
    ID_MUNICIPIO int,
    ID_TIPO_ENSINO int,
    ID_CATEGORIA int,
    ID_SUBCATEGORIA int,
    ID_DATA int,
    VALOR varchar(200) not null,
    foreign key (ID_FONTE) references EDC_FONTE,
    foreign key (ID_MUNICIPIO) references EDC_MUNICIPIO,
    foreign key (ID_TIPO_ENSINO) references EDC_TIPO_ENSINO,
    foreign key (ID_CATEGORIA) references EDC_CATEGORIA,
    foreign key (ID_SUBCATEGORIA) references EDC_CATEGORIA,
    foreign key (ID_DATA) references EDC_DATA
);

create table EDC_PROCESSAMENTO (
    ID int primary key identity,
    TEXTO varchar(100) null,
    ANO_INI int not null,
    ANO_FIM int not null,
    SITUACAO int not null,
    DATA datetime null,
    QTD_REGISTROS int
);

go