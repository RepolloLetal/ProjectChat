using GlobalChat.UI.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;

namespace GlobalChat.UI
{
    public static class ServicioPersistencia
    {
        private static readonly string ARCHIVO = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Configuracion.gc");
        private static DatosGuardado datosGuardado;
        public static bool SesionIniciada
        {
            get
            {
                return datosGuardado.SesionIniciada;
            }
        }
        public static string Usuario
        {
            get
            {
                return datosGuardado.Usuario;
            }
        }
        public static string Clave
        {
            get
            {
                return datosGuardado.Clave;
            }
        }

        // Leerá si el usuario tenia una sesión iniciada y con qué datos
        static ServicioPersistencia()
        {
            if (File.Exists(ARCHIVO))
            {
                string confTXT = File.ReadAllText(ARCHIVO);
                datosGuardado = JsonSerializer.Deserialize<DatosGuardado>(confTXT) ?? new DatosGuardado();
            }
            else
            {
                datosGuardado = new DatosGuardado();
            }
        }

        public static void CerrarSesion()
        {
            if (File.Exists(ARCHIVO))
            {
                File.Delete(ARCHIVO);
            }
        }

        public static void NuevaSesionIniciada(DatosGuardado datosGuardado)
        {
            ServicioPersistencia.datosGuardado = datosGuardado;
            string datosTXT = JsonSerializer.Serialize(datosGuardado);
            if (File.Exists(ARCHIVO))
            {
                File.Delete(ARCHIVO);
            }
            File.WriteAllLines(ARCHIVO, [datosTXT]);
        }
    }
}
