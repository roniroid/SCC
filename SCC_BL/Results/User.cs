using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
    public class User : CommonElements
    {
        public const SCC_BL.DBValues.Catalog.ELEMENT ELEMENT_CATEGORY = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_USER;

        const string TABLE_NAME = "User";
        const string PHRASE_WITH_GENRE = "el usuario";
        const string PHRASE_WITH_POSESSION = "del usuario";

        public static class SignIn
        {
            public enum CODE
            {
                SUCCESS,
                ERROR,
                ALREADY_EXISTS = -100,
                PASSWORDS_DONT_MATCH = -101
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Registro en el sistema exitoso";

                public const string DATABASE_LOG = "Registro en el sistema exitoso";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Registro en el sistema exitoso";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de registrarse en el sistema.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al intentar registrarse en el sistema.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct AlreadyExists
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Registro fallido porque ya existe un usuario con ese nombre. \r\n\t→ Datos del objeto: " + REPLACE_JSON_INFO;

                //public const string DATABASE_LOG = "ERROR - El usuario ya existe";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese usuario ya existe";
            };

            public struct PasswordsDoNotMatch
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Registro fallido del usuario por inconsistencias en las contraseñas. \r\n\t→ Datos del objeto: " + REPLACE_JSON_INFO;

                //public const string DATABASE_LOG = "ERROR - Las contraseñas no coinciden";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Las contraseñas no coinciden";
            };

            public struct SendMail
            {
                public enum CODE
                {
                    SUCCESS,
                    ERROR,
                    SERVER_ERROR,
                }

                public struct Success
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.SUCCESS;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                    public const string LOCAL_LOG = "Correo de creación de usuario enviado";

                    public const string MESSAGE_TITLE = "Éxito";
                    public const string MESSAGE_CONTENT = "Correo de creación de usuario enviado";
                };

                public struct Error
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la recuperación de contraseña. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                    public const string MESSAGE_TITLE = "Error";
                    public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la creación de usuario";
                };

                public struct ServerError
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la creación de usuario";
                };
            }
        }

        public static class LogIn
        {
            public enum CODE
            {
                SUCCESS,
                ERROR = -103,
                WRONG_PASSWORD = -102,
                WRONG_USERNAME = -101,
                DISABLED = -104,
                UNAUTHORIZED = -105,
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Ingreso al sistema exitoso";

                public const string DATABASE_LOG = "Ingreso al sistema exitoso";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de ingresar al sistema. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de ingresar al sistema. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al intentar ingresar al sistema. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct WrongPassword
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido por contraseña errónea";

                public const string DATABASE_LOG = "ERROR - Ingreso fallido por contraseña errónea";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Contraseña incorrecta";
            };

            public struct WrongUsername
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido " + PHRASE_WITH_POSESSION + " por nombre de usuario erróneo";

                //public const string DATABASE_LOG = "ERROR - Ingreso fallido por nombre de usuario erróneo";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese nombre de usuario no se encuentra registrado en el sistema";
            };

            public struct Disabled
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido del usuario por usuario deshabilitado.";

                //public const string DATABASE_LOG = "Ingreso fallido por usuario deshabilitado";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese usuario se encuentra deshabilitado";
            };

            public struct Unauthorized
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido del usuario por usuario no autorizado para ingresar al sistema.";

                //public const string DATABASE_LOG = "Ingreso fallido por usuario no autorizado para ingresar al sistema";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese usuario no puede ingresar al sistema";
            };
        }

        public static class PasswordRecovery
        {
            public enum CODE
            {
                SUCCESS,
                ERROR,
                PASSWORDS_DONT_MATCH,
                WRONG_USERNAME,
                WRONG_TOKEN
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Cambio de contraseña exitoso";

                public const string DATABASE_LOG = "Cambio de contraseña exitoso";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de recuperar la contraseña. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al momento de recuperar la contraseña.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al intentar cambiar la contraseña";
            };

            public struct PasswordsDoNotMatch
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Intento de ingreso. Razón: contraseñas no coinciden";

                public const string DATABASE_LOG = "ERROR - Intento de ingreso. Razón: contraseñas no coinciden";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Las contraseñas no coinciden";
            };

            public struct WrongUsername
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido por nombre de usuario erróneo";

                public const string DATABASE_LOG = "ERROR - Ingreso fallido por nombre de usuario erróneo. Nombre de usuario: \"" + REPLACE_USERNAME + "\"";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese usuario no se encuentra registrado en el sistema";
            };

            public struct WrongToken
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ingreso fallido por token erróneo";

                public const string DATABASE_LOG = "ERROR - Ingreso fallido por token erróneo. Nombre de usuario: \"" + REPLACE_USERNAME + "\"";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "El token recibido no coincide con el usuario";
            };
        }

        public static class PasswordChange
        {
            public enum CODE
            {
                SUCCESS,
                ERROR,
                PASSWORDS_DONT_MATCH
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Cambio de contraseña exitoso";

                public const string DATABASE_LOG = "Cambio de contraseña exitoso";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al cambiar la contraseña. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al cambiar la contraseña.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al intentar cambiar la contraseña";
            };

            public struct PasswordsDoNotMatch
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Fallo al cambiar contraseña. Razón: contraseñas no coinciden";

                public const string DATABASE_LOG = "ERROR - Fallo al cambiar contraseña. Razón: contraseñas no coinciden";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Las contraseñas no coinciden";
            };

            public struct NotAllowedToChangeOtherUsersPasswords
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para cambiar las contraseñas de otros usuarios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para cambiar las contraseñas de otros usuarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para cambiar las contraseñas de otros usuarios";
            }

            public struct SendMail
            {
                public enum CODE
                {
                    SUCCESS,
                    ERROR,
                    SERVER_ERROR,
                }

                public struct Success
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.SUCCESS;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                    public const string LOCAL_LOG = "Correo de actualización de contraseña enviado";

                    public const string MESSAGE_TITLE = "Éxito";
                    public const string MESSAGE_CONTENT = "Correo de actualización de contraseña enviado";
                };

                public struct Error
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la actualización de contraseña. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                    public const string MESSAGE_TITLE = "Error";
                    public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la actualización de contraseña";
                };

                public struct ServerError
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la actualización de contraseña";
                };
            }
        }

        public static class UpdateLanguageList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de lenguajes para el usuario de manera correcta";

                public const string DATABASE_LOG = "Lenguajes actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de lenguajes para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de lenguajes. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de lenguajes. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de lenguajes";
            };
        }

        public static class UpdateRoleList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de roles para el usuario de manera correcta";

                public const string DATABASE_LOG = "Roles actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de roles para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de roles. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de roles. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de roles";
            };
        }

        public static class AsignRolesAndPermissions
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizaron las listas de roles y permisos para el usuario de manera correcta";

                public const string DATABASE_LOG = "Roles y permisos actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizaron las listas de roles y permisos para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de roles y permisos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de roles y permisos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de roles y permisos";
            };
        }

        public static class AsignProgramsAndProgramGroups
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizaron las listas de programas y grupos de programas para el usuario de manera correcta";

                public const string DATABASE_LOG = "Programas y grupos de programas actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizaron las listas de programas y grupos de programas para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de programas y grupos de programas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de programas y grupos de programas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de programas y grupos de programas";
            };
        }

        public static class UpdatePermissionList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de permisos para el usuario de manera correcta";

                public const string DATABASE_LOG = "Permisos actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de permisos para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de permisos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de permisos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de permisos";
            };
        }

        public static class UpdateSupervisorList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de supervisores para el usuario de manera correcta";

                public const string DATABASE_LOG = "Supervisores actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de supervisores para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de supervisores. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de supervisores. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de supervisores";
            };
        }

        public static class UpdateWorkspaceList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de puestos para el usuario de manera correcta";

                public const string DATABASE_LOG = "Puestos actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de puestos para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de puestos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de puestos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de puestos";
            };
        }

        public static class UpdateGroupList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de grupos para el usuario de manera correcta";

                public const string DATABASE_LOG = "Grupos actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de grupos para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de grupos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de grupos. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de grupos";
            };
        }

        public static class UpdateProgramList
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se actualizó la lista de programas para el usuario de manera correcta";

                public const string DATABASE_LOG = "Programas actualizados correctamente";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se actualizó la lista de programas para el usuario de manera correcta";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de programas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de programas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de programas";
            };
        }

        public static class ForgottenPassword
        {
            public enum CODE
            {
                SUCCESS,
                ERROR,
                SERVER_ERROR,
                SENDING_MAIL_ERROR,
                SENDING_MAIL_SUCCESS,
                WRONG_USERNAME
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                //public const string LOCAL_LOG = "Correo de recuperación de contraseña enviado";

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Correo de recuperación de contraseña enviado";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la recuperación de contraseña. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la recuperación de contraseña";
            };

            public struct ServerError
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la recuperación de contraseña";
            };

            public struct WrongUsername
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Solicitud fallida por nombre de usuario erróneo";

                public const string DATABASE_LOG = "ERROR - Solicitud fallida por nombre de usuario erróneo. Nombre de usuario: \"" + REPLACE_USERNAME + "\"";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ese usuario no se encuentra registrado en el sistema";
            };
        }

        public static class Update
        {
            public enum CODE
            {
                INFO,
                SUCCESS,
                ERROR,
                ALREADY_EXISTS = -100,
                PASSWORDS_DONT_MATCH = -101
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Modificación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string DATABASE_LOG = "Modificación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha editado el usuario correctamente";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al momento de modificar al usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Hubo un error en la edición del usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al editar al usuario: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct AlreadyExists
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Edición del usuario fallida. El usuario se encontró en el sistema.";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "El usuario ya está registrado en el sistema";
            };

            public struct NotAllowedToChangePermissions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";

                public const string DATABASE_LOG = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";
            }

            public struct NotAllowedToModifyUsers
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para modificar usuarios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para modificar usuarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para modificar usuarios";
            }
        }

        public static class Insert
        {
            public enum CODE
            {
                INFO,
                SUCCESS,
                ERROR,
                ALREADY_EXISTS = -100,
                PASSWORDS_DONT_MATCH = -101
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Creación del usuario exitosa.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO;

                public const string DATABASE_LOG = "Creación del usuario exitosa.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha creado el usuario correctamente";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al crear al usuario.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                //public const string DATABASE_LOG = "ERROR - Hubo un error en la creación del usuario.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al crear al usuario: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct AlreadyExists
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Creación del usuario fallida. El usuario se encontró en el sistema.";

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "El usuario ya está registrado en el sistema";
            };

            public struct NotAllowedToChangePermissions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";

                public const string DATABASE_LOG = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "No se han modificado los permisos ya que no cuenta con los permisos necesarios";
            }

            public struct NotAllowedToCreateUsers
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para crear usuarios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para crear usuarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para crear usuarios";
            }
        }

        public static class Delete
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Eliminación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string DATABASE_LOG = "Eliminación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha eliminado el usuario correctamente";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al eliminar al usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Hubo un error en la eliminación del usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al eliminar al usuario: " + REPLACE_EXCEPTION_MESSAGE;
            };

            public struct NotAllowedToDeleteUsers
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para eliminar usuarios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para eliminar usuarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para eliminar usuarios";
            }
        }

        public static class Activate
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha activado al usuario de manera exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string DATABASE_LOG = "Activación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha activado al usuario correctamente";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al activar al usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Hubo un error en la activación del usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al activar al usuario: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }

        public static class Deactivate
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Success
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.SUCCESS;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Se ha desactivado al usuario de manera exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string DATABASE_LOG = "Desactivación del usuario exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Éxito";
                public const string MESSAGE_CONTENT = "Se ha desactivado al usuario correctamente";
            };

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al desactivar al usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Hubo un error en la desactivación del usuario.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al desactivar al usuario: " + REPLACE_EXCEPTION_MESSAGE;
            };
        }
    }
}
