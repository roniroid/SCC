using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
    public class Report : CommonElements
    {
        public static class Index
        {
            public struct NotAllowedToSeeReports
            {
                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para ver reportes";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para ver reportes";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para ver reportes";
            }
        }
        
        public static class OverallAccuracy
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class ComparativeByUser
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class ComparativeByProgram
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class CalibratorComparison
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class CalibratorComparisonWithAttributes
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class CalibratorComparisonByError
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
        
        public static class AccuracyByAttribute
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
                
        public static class AccuracyBySubattribute
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }

        public static class AccuracyTrend
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }

        public static class AccuracyTrendByAttribute
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }

        public static class ParetoBI
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha generado el reporte correctamente";

                public const string DATABASE_LOG = "Se ha generado el reporte correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha generado el reporte correctamente";
            };

            public struct Error
            {
                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de generar el reporte.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de generar el reporte.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
    }
}
