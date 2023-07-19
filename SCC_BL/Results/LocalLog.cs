using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
    public class LocalLog : CommonElements
    {
        public struct Save
        {
            public enum CODE
            {
                SUCCESS,
                ERROR,
                EMPTY_LOG_PATH,
                NON_EXISTENT_LOG_PATH,
                WRITE_LOG_ERROR,
                SERIALIZATION_LOG_ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se creó una bitácora para el objeto: \r\n\t→ Datos del objeto: " + REPLACE_JSON_INFO;
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Se detectó un error en la creación de una bitácora para el objeto: \r\n\t→ Datos del objeto: " + REPLACE_JSON_INFO + ". \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct EmptyLogPath
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.FATAL;
                public const string LOCAL_LOG = "No se ha encontrado ruta para almacenar las bitácoras de la aplicación";
            };

            public struct NonExistentLogPath
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.FATAL;
                public const string LOCAL_LOG = "No se ha encontrado ruta para almacenar las bitácoras de la aplicación";
            };

            public struct WriteLogError
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.FATAL;
                public const string LOCAL_LOG = "Error al escribir archivo de bitácora";
            };

            public struct SerializationLogError
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "No se pudo serializar el objeto. Mensaje: " + REPLACE_LOCAL_LOG_MESSAGE + ". StackTrace: " + REPLACE_LOCAL_LOG_STACK_TRACE + ". InnerException: " + REPLACE_EXCEPTION_MESSAGE + "";
            };
        }
    }
}
