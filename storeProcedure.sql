USE [ClinicaDB]
GO

/****** Object:  StoredProcedure [dbo].[spVerTurno]    Script Date: 04/12/2025 05:45:30 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spVerTurno]
as
begin 
    SELECT 
        T.Id AS IdTurno,
        T.Fecha,
        T.Hora,
        P.Nombre + ' ' + P.Apellido AS Paciente,
        M.Nombre + ' ' + M.Apellido AS Medico,
        E.Descripcion AS Especialidad,
        C.Tipo AS Cobertura,
        C.NombreObraSocial,
        ET.Descripcion AS Estado
    FROM Turno T
    INNER JOIN Paciente P
        ON P.Id = T.IdPaciente
    LEFT JOIN Cobertura C
        ON C.Id = P.IdCobertura      -- 💥 ESTE JOIN ERA EL QUE FALTABA
    INNER JOIN EstadoTurno ET
        ON T.IdEstadoTurno = ET.Id
    INNER JOIN MedicoEspecialidad ME
        ON ME.IdEspecialidad = T.IdEspecialidad
    INNER JOIN Medico M
        ON M.Id = ME.IdMedico
    INNER JOIN Especialidad E
        ON E.Id = ME.IdEspecialidad
end
GO

