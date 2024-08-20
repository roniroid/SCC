using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Results
{
	public class NotificationMatrix : CommonElements
	{
		public const SCC_BL.DBValues.Catalog.ELEMENT ELEMENT_CATEGORY = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_NOTIFICATIONMATRIX;

		public const string TABLE_NAME = "NotificationMatrix";
		public const string PHRASE_WITH_GENRE = "el elemento de matriz de notificación";
		public const string PHRASE_WITH_POSESSION = "del elemento de matriz de notificación";

		public struct Manage
        {
            public struct NotAllowedToChangeNotificationAlarms
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para cambiar las alarmas de notificaciones";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para cambiar las alarmas de notificaciones";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para cambiar las alarmas de notificaciones";
            }
        }

		public struct Insert
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
				public const string LOCAL_LOG = "Creación del elemento de \"" + TABLE_NAME + "\" exitosa.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO;

				public const string DATABASE_LOG = "Creación " + PHRASE_WITH_POSESSION + " exitosa.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO;

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha creado " + PHRASE_WITH_GENRE + " correctamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al crear el elemento de \"" + TABLE_NAME + "\".\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Hubo un error en la creación " + PHRASE_WITH_POSESSION + ".\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al crear " + PHRASE_WITH_GENRE + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
			}
		}

		public struct Delete
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
				public const string LOCAL_LOG = "Eliminación del elemento de \"" + TABLE_NAME + "\" exitoso.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

				public const string DATABASE_LOG = "Eliminación " + PHRASE_WITH_POSESSION + " exitoso.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha eliminado " + PHRASE_WITH_GENRE + " correctamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al eliminar el elemento de \"" + TABLE_NAME + "\".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Hubo un error al eliminar " + PHRASE_WITH_POSESSION + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al eliminar " + PHRASE_WITH_GENRE + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
			}
		}

		public struct UserNotification
        {
            public enum CODE
            {
                SUCCESS,
                ERROR
            }

            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al guardar la notificación.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "Hubo un error al guardar la notificación.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al guardar la notificación.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            }

            public struct Transaction
			{
				public struct Disputation
				{
					public const string REPLACE_DISPUTATING_USER = "%REPLACE_DISPUTATING_USER%";
					public const string REPLACE_TRANSACTION_IDENTIFIER = "%REPLACE_TRANSACTION_IDENTIFIER%";
					public const string REPLACE_OBJECT_INFO = "%REPLACE_OBJECT_INFORMATION%";

					public struct Creation
                    {
                        public const string SELF_AGENT_DISPUTATION = "Usted ha creado una disputa para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                        public const string AGENT_DISPUTATION = "El usuario \"" + REPLACE_DISPUTATING_USER + "\" ha creado una disputa para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                    }

					public struct Update
                    {
                        public const string SELF_AGENT_DISPUTATION = "Usted ha modificado una disputa para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                        public const string AGENT_DISPUTATION = "El usuario \"" + REPLACE_DISPUTATING_USER + "\" ha modificado una disputa para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                    }
				}

				public struct Devolution
				{
					public const string REPLACE_RETURNING_USER = "%REPLACE_RETURNING_USER%";
					public const string REPLACE_TRANSACTION_IDENTIFIER = "%REPLACE_TRANSACTION_IDENTIFIER%";
                    public const string REPLACE_OBJECT_INFO = "%REPLACE_OBJECT_INFORMATION%";

                    public struct Creation
                    {
                        public const string RETURNING_USER_DEVOLUTION = "Usted ha creado una devolución para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                        public const string AGENT_DEVOLUTION = "El usuario \"" + REPLACE_RETURNING_USER + "\" ha realizado una devolución para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                    }

                    public struct Update
                    {
                        public const string RETURNING_USER_DEVOLUTION = "Usted ha modificado una devolución para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                        public const string AGENT_DEVOLUTION = "El usuario \"" + REPLACE_RETURNING_USER + "\" ha modificado una devolución para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                    }
                }

                public struct Invalidation
                {
                    public const string REPLACE_INVALIDATING_USER = "%REPLACE_INVALIDATING_USER%";
                    public const string REPLACE_TRANSACTION_IDENTIFIER = "%REPLACE_TRANSACTION_IDENTIFIER%";
                    public const string REPLACE_OBJECT_INFO = "%REPLACE_OBJECT_INFORMATION%";

                    public struct Creation
                    {
                        public const string INVALIDATING_USER_INVALIDATION = "Usted ha creado una invalidación para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                        public const string AGENT_INVALIDATION = "El usuario \"" + REPLACE_INVALIDATING_USER + "\" ha realizado una invalidación para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                    }

                    public struct Update
                    {
                        public const string INVALIDATING_USER_INVALIDATION = "Usted ha modificado una invalidación para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                        public const string AGENT_INVALIDATION = "El usuario \"" + REPLACE_INVALIDATING_USER + "\" ha modificado una invalidación para la transacción \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                    }
                }

                public struct Calibration
                {
                    public const string REPLACE_CALIBRATING_USER = "%REPLACE_CALIBRATING_USER%";
                    public const string REPLACE_TRANSACTION_IDENTIFIER = "%REPLACE_TRANSACTION_IDENTIFIER%";
                    public const string REPLACE_OBJECT_INFO = "%REPLACE_OBJECT_INFORMATION%";

                    public struct Creation
                    {

                        public const string CALIBRATING_USER_INVALIDATION = "Usted ha creado una calibración con el identificador \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                        public const string AGENT_CALIBRATION = "El usuario \"" + REPLACE_CALIBRATING_USER + "\" ha creado una calibración con el identificador \"" + REPLACE_TRANSACTION_IDENTIFIER + "\"";
                    }

                    public struct Update
                    {
                        public const string CALIBRATING_USER_INVALIDATION = "Usted ha modificado una calibración con el identificador \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Descripción anterior: " + REPLACE_OBJECT_INFO;
                        public const string AGENT_CALIBRATION = "El usuario \"" + REPLACE_CALIBRATING_USER + "\" ha modificado una calibración con el identificador \"" + REPLACE_TRANSACTION_IDENTIFIER + "\". Datos anteriores: " + REPLACE_OBJECT_INFO;
                    }
                }

                public struct CalibrationSession
                {
                    public const string REPLACE_CALIBRATING_USER = "%REPLACE_CALIBRATING_USER%";
                    public const string REPLACE_TRANSACTION_IDENTIFIERS = "%REPLACE_TRANSACTION_IDENTIFIERS%";
                    public const string REPLACE_OBJECT_INFO = "%REPLACE_OBJECT_INFORMATION%";

                    public struct Creation
                    {

                        public const string CALIBRATING_USER_INVALIDATION = "Usted ha creado una sesión de calibración para las transacciones " + REPLACE_TRANSACTION_IDENTIFIERS;
                        public const string AGENT_CALIBRATION = "El usuario \"" + REPLACE_CALIBRATING_USER + "\" ha creado una sesión de calibración para las transacciones " + REPLACE_TRANSACTION_IDENTIFIERS;
                    }

                    public struct Update
                    {
                        public const string CALIBRATING_USER_INVALIDATION = "Usted ha modificado una sesión de calibración para las transacciones " + REPLACE_TRANSACTION_IDENTIFIERS + ". Descripción anterior: " + REPLACE_OBJECT_INFO;
                        public const string AGENT_CALIBRATION = "El usuario \"" + REPLACE_CALIBRATING_USER + "\" ha modificado una sesión de calibración para las transacciones " + REPLACE_TRANSACTION_IDENTIFIERS + ". Datos anteriores: " + REPLACE_OBJECT_INFO;
                    }
                }
            }
		}
	}
}