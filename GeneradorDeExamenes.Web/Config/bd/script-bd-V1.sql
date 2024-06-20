CREATE DATABASE ExamenIA;

USE ExamenIA;

CREATE TABLE Examen (
    IdExamen INT PRIMARY KEY IDENTITY,
    Calificacion INT,
    Feedback NVARCHAR(MAX) NOT NULL
);

CREATE TABLE Pregunta (
    IdPregunta INT PRIMARY KEY IDENTITY,
    IdExamen INT NOT NULL,
    PreguntaTexto NVARCHAR(MAX) NOT NULL,
    RespuestaUsuario NVARCHAR(MAX) NOT NULL,
    RespuestaCorrecta NVARCHAR(MAX),
    FOREIGN KEY (IdExamen) REFERENCES Examen(IdExamen)
);
