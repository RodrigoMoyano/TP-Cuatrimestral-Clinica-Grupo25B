using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string host = "smtp.gmail.com";
    private readonly int port = 587;

    //correo gmaikl
    private readonly string user;

    //clave aplicacion
    private readonly string pass;

    public EmailService()
    {
        //Usuario guardado en el Web.config
        user = ConfigurationManager.AppSettings["userEmail"];
        //Constrseña guardada en el Web.config
        pass = ConfigurationManager.AppSettings["passEmail"];
    }

    public void EnviarConfirmacionTurno(string emailDestino, string nombrePaciente, DateTime fecha, string medicoNombre)
    {
        if(string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            throw new Exception("Faltan las credenciale del correo en el Web.config");
        }

        string desde = user; //From desde donde sale el correo
        string asunto = "Confirmación de Turno";
        string cuerpo = $"Hola {nombrePaciente},\n\n" +
                        $"Tu turno ha sido registrado para el día {fecha:dd/MM/yyyy} con el médico {medicoNombre}.\n\n" +
                        "¡Gracias por confiar en nosotros!";

        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user, pass),
            EnableSsl = true
        };

        client.Send(desde, emailDestino, asunto, cuerpo);
    }

    public void EnviarContraseñaOlvidada(string emailDestino, string nombreUsuario, string clave)
    {
        if(string.IsNullOrEmpty (user) || string.IsNullOrEmpty(pass))
        {
            throw new Exception("Faltan credenciales en el Web.Config");
        }

        string desde = user;
        string asunto = "Recuperacion de Contraseña - Gestor de Turnos";
        string cuerpo = $"Hola {nombreUsuario}, \n\n" +
            "Solicitaste el cambio de contraseña \n" +
            $"Tu constraseña es:  {clave}\n\n" +
            "Te recomendamos cambiarla luego de iniciar sesion.\n\n" +
            "Saludos!";


        var client = new SmtpClient(host, port)
        {
            Credentials = new NetworkCredential(user,pass),
            EnableSsl = true
        };

        client.Send(desde, emailDestino, asunto, cuerpo);
    }
}
