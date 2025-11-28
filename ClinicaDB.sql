USE master;
GO


ALTER DATABASE ClinicaDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO


DROP DATABASE IF EXISTS ClinicaDB;
GO


CREATE DATABASE ClinicaDB;
GO

USE ClinicaDB;
GO



CREATE TABLE Rol (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL
);

CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    NombreUsuario NVARCHAR(50) NOT NULL,
    Clave NVARCHAR(100) NOT NULL,
    Activo BIT DEFAULT 1,
    IdRol INT NOT NULL,
    FOREIGN KEY (IdRol) REFERENCES Rol(Id)
);

CREATE TABLE Cobertura (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Tipo NVARCHAR(50) NOT NULL,
    NombreObraSocial NVARCHAR(100) NULL,
    PlanCobertura NVARCHAR(100) NULL
);

CREATE TABLE Paciente (
    IdPaciente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Dni NVARCHAR(20) NOT NULL,
    Email NVARCHAR(100),
    Telefono NVARCHAR(30),
    IdCobertura INT NOT NULL,
    FOREIGN KEY (IdCobertura) REFERENCES Cobertura(Id)
);

CREATE TABLE Especialidad (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100) NOT NULL
);

CREATE TABLE Medico (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Matricula NVARCHAR(50),
    Email NVARCHAR(100),
    Telefono NVARCHAR(30),
    IdUsuario INT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Usuario(Id)
);

CREATE TABLE MedicoEspecialidad (
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    PRIMARY KEY (IdMedico, IdEspecialidad),
    FOREIGN KEY (IdMedico) REFERENCES Medico(Id),
    FOREIGN KEY (IdEspecialidad) REFERENCES Especialidad(Id)
);

CREATE TABLE EstadoTurno (
    IdEstadoTurno INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50) NOT NULL
);

CREATE TABLE TurnoTrabajo (
    IdTurnoTrabajo INT IDENTITY(1,1) PRIMARY KEY,
    IdMedico INT NOT NULL,
    DiaSemana NVARCHAR(20) NOT NULL,
    HoraInicio TIME NOT NULL,
    HoraFin TIME NOT NULL,
    FOREIGN KEY (IdMedico) REFERENCES Medico(Id)
);

CREATE TABLE Turno (
    IdTurno INT IDENTITY(1,1) PRIMARY KEY,
    IdPaciente INT NOT NULL,
    IdMedico INT NOT NULL,
    IdEspecialidad INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora TIME NOT NULL,
    Observaciones NVARCHAR(300),
    IdEstadoTurno INT NOT NULL,
    FOREIGN KEY (IdPaciente) REFERENCES Paciente(Id),
    FOREIGN KEY (IdMedico) REFERENCES Medico(Id),
    FOREIGN KEY (IdEspecialidad) REFERENCES Especialidad(Id),
    FOREIGN KEY (IdEstadoTurno) REFERENCES EstadoTurno(Id)
);



INSERT INTO Rol (Descripcion) VALUES 
('Administrador'), 
('Paciente'), 
('Medico');

INSERT INTO EstadoTurno (Descripcion) VALUES 
('Nuevo'),
('Reprogramado'),
('Cancelado'),
('No Asistió'),
('Cerrado');

INSERT INTO Cobertura (Tipo, NombreObraSocial, PlanCobertura) VALUES 
('Particular', NULL, NULL),
('Obra Social', 'OSDE', '210'),
('Obra Social', 'Swiss Medical', 'SMG 50');

INSERT INTO Especialidad (Nombre, Descripcion) VALUES 
('Clínica Médica', 'Atención integral de pacientes adultos y niños para diagnósticos y tratamientos generales.'),
('Odontología', 'Cuidado y tratamiento de dientes, encías y salud bucal en general.'),
('Dermatología', 'Diagnóstico y tratamiento de enfermedades de la piel, cabello y uñas.'),
('Cardiología', 'Prevención, diagnóstico y tratamiento de enfermedades del corazón y sistema circulatorio.');
