using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Results
{
	public class UploadedFile : CommonElements
	{
		public const SCC_BL.DBValues.Catalog.ELEMENT ELEMENT_CATEGORY = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_UPLOADEDFILE;

		public const string TABLE_NAME = "UploadedFile";
		public const string PHRASE_WITH_GENRE = "el archivo cargado";
		public const string PHRASE_WITH_POSESSION = "del archivo cargado";

		public struct Insert
		{
			public enum CODE
			{
				SUCCESS,
				ERROR = -107
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

				//public const string DATABASE_LOG = "Hubo un error en la creación " + PHRASE_WITH_POSESSION + ".\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al crear " + PHRASE_WITH_GENRE + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
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

		public struct Obtaining
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
				public const string LOCAL_LOG = "Obtención y conversión del objeto de \"" + TABLE_NAME + "\" exitoso.";

				public const string DATABASE_LOG = "Obtención y conversión " + PHRASE_WITH_POSESSION + " exitoso.";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha obtenido y convertido " + PHRASE_WITH_GENRE + " correctamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al obtener y convertir el elemento de \"" + TABLE_NAME + "\".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Hubo un error al obtener y convertir " + PHRASE_WITH_POSESSION + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al obtener y convertir " + PHRASE_WITH_GENRE + "\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
			}
		}

		public static class UserMassiveImport
		{
			public enum CODE
			{
				SUCCESS,
				ERROR
			}

			public const string REPLACE_MAX_SIZE = "%maxSize%";

			public const string NO_FILE_FOUND = "No se ha encontrado ningún archivo";
			public const string WRONG_FILE_EXTENSION = "No se permite subir archivos con esa extensión";
			public const string MAX_SIZE_EXCEEDED = "Se ha sobrepasado el tamaño máximo del archivo (" + REPLACE_MAX_SIZE + ")";

			public struct Success
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.SUCCESS;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
				public const string LOCAL_LOG = "Se ha procesado el archivo correctamente";

				public const string DATABASE_LOG = "Se ha procesado el archivo correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha procesado el archivo correctamente";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al importar los usuarios. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al importar los usuarios. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al importar los usuarios";
			};

			public struct ErrorSingleRow
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al encapsular los datos del usuario\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al importar los usuarios. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al encapsular los datos del usuario";

				public const string CUSTOM_ERROR_EXCEL_LINES = "Se encontraron errores en las líneas:" + REPLACE_CUSTOM_CONTENT;
				public const string CUSTOM_ERROR_USER_NOT_INSERTED = "Ocurrió un error al tratar de ingresar al usuario. La persona ingresada se eliminará del sistema.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
				public const string CUSTOM_ERROR_PERSON_NOT_INSERTED = "Ocurrió un error al tratar de ingresar a la persona.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
			};
		}

		public static class FormUpload
		{
			public enum CODE
			{
				SUCCESS,
				ERROR
			}

			public const string REPLACE_MAX_SIZE = "%maxSize%";

			public const string NO_FILE_FOUND = "No se ha encontrado ningún archivo";
			public const string WRONG_FILE_EXTENSION = "No se permite subir archivos con esa extensión";
			public const string MAX_SIZE_EXCEEDED = "Se ha sobrepasado el tamaño máximo del archivo (" + REPLACE_MAX_SIZE + ")";

			public struct Success
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.SUCCESS;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
				public const string LOCAL_LOG = "Se ha realizado la carga de formulario de manera correcta";

				public const string DATABASE_LOG = "Se ha realizado la carga de formulario de manera correcta";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha realizado la carga de formulario de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al cargar el formulario. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al cargar el formulario. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al cargar el formulario";
			};

			public struct ErrorSingleRow
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al encapsular los datos del formulario\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al cargar el formulario. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al encapsular los datos del formulario";

				public const string CUSTOM_ERROR_EXCEL_LINES = "Se encontraron errores en las líneas:" + REPLACE_CUSTOM_CONTENT;
				public const string CUSTOM_ERROR_FORM_NOT_LOADED = "Ocurrió un error al tratar de cargar el formulario.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
				public const string CUSTOM_ERROR_ATTRIBUTES_NOT_CREATED = "Ocurrió un error al tratar de ingresar los atributos del formulario.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
			};
		}

		public static class TransactionImport
		{
			public enum CODE
			{
				SUCCESS,
				ERROR
			}

			public const string REPLACE_MAX_SIZE = "%maxSize%";

			public const string NO_FILE_FOUND = "No se ha encontrado ningún archivo";
			public const string WRONG_FILE_EXTENSION = "No se permite subir archivos con esa extensión";
			public const string MAX_SIZE_EXCEEDED = "Se ha sobrepasado el tamaño máximo del archivo (" + REPLACE_MAX_SIZE + ")";

			public struct Success
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.SUCCESS;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
				public const string LOCAL_LOG = "Se ha realizado la importación de transacciones de manera correcta";

				public const string DATABASE_LOG = "Se ha realizado la importación de transacciones de manera correcta";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha realizado la importación de transacciones de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al importar las transacciones. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al importar las transacciones. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al importar las transacciones";
			};

			public struct ErrorSingleRow
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al encapsular los datos de la transacción.\r\n\tDatos del objeto: " + REPLACE_JSON_INFO + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				//public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al importar las transacciones. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al encapsular los datos de la transacción";

				public const string CUSTOM_ERROR_EXCEL_LINES = "Se encontraron errores en las líneas:" + REPLACE_CUSTOM_CONTENT;
				public const string CUSTOM_ERROR_FORM_NOT_LOADED = "Ocurrió un error al tratar de importar la transacción.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
				public const string CUSTOM_ERROR_ATTRIBUTES_NOT_CREATED = "Ocurrió un error al tratar de ingresar los atributos de la transacción.\r\nDatos del objeto: " + REPLACE_JSON_INFO;
			};
		}
	}
}