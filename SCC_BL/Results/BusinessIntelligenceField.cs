﻿using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
	public class BusinessIntelligenceField : CommonElements
	{
		public const SCC_BL.DBValues.Catalog.ELEMENT ELEMENT_CATEGORY = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_BUSINESSINTELLIGENCEFIELD;

		public const string TABLE_NAME = "BusinessIntelligenceField";
		public const string PHRASE_WITH_GENRE = "el campo de Inteligencia de Negocios";
		public const string PHRASE_WITH_POSESSION = "del campo de Inteligencia de Negocios";

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

            public struct NotAllowedToCreateBIQuestions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";
            }
        }

		public struct Update
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
				public const string LOCAL_LOG = "Modificación del elemento de \"" + TABLE_NAME + "\" exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

				public const string DATABASE_LOG = "Modificación " + PHRASE_WITH_POSESSION + " exitosa.\r\n\tDatos previos del objeto: " + REPLACE_JSON_INFO;

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha editado " + PHRASE_WITH_GENRE + " correctamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al modificar el elemento de \"" + TABLE_NAME + "\".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Hubo un error en la edición " + PHRASE_WITH_POSESSION + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al editar " + PHRASE_WITH_GENRE + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
            }

            public struct NotAllowedToCreateBIQuestions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para crear preguntas de inteligencia de negocios";
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

		public static class UpdateBIFieldValueCatalogList
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
				public const string LOCAL_LOG = "Se actualizó la lista de opciones para el campo de inteligencia de negocios de manera correcta";

				public const string DATABASE_LOG = "Opciones actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de opciones para el campo de inteligencia de negocios de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de opciones. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de opciones. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de opciones";
			};
		}

		public static class UpdateBIFieldChildList
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
				public const string LOCAL_LOG = "Se actualizó la lista de respuestas para el campo de inteligencia de negocios de manera correcta";

				public const string DATABASE_LOG = "Respuestas actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de respuestas para el campo de inteligencia de negocios de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de respuestas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de respuestas. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de respuestas";
			};
		}
	}
}