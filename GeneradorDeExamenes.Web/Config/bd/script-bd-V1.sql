CREATE DATABASE ExamenIA;

USE ExamenIA;

CREATE TABLE Examen (
    IdExamen INT PRIMARY KEY IDENTITY,
    Calificacion INT NOT NULL
);

CREATE TABLE Pregunta (
    IdPregunta INT PRIMARY KEY IDENTITY,
    IdExamen INT NOT NULL,
    PreguntaTexto NVARCHAR(MAX) NOT NULL,
    Respuesta NVARCHAR(MAX),
    Feedback NVARCHAR(MAX),
    FOREIGN KEY (IdExamen) REFERENCES Examen(IdExamen)
);
