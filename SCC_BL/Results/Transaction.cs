using SCC_BL.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SCC_BL.Results
{
	public class Transaction : CommonElements
	{
		public const SCC_BL.DBValues.Catalog.ELEMENT ELEMENT_CATEGORY = SCC_BL.DBValues.Catalog.ELEMENT.ELEMENT_TRANSACTION;

		public const string TABLE_NAME = "Transaction";
		public const string PHRASE_WITH_GENRE = "la transacción";
		public const string PHRASE_WITH_POSESSION = "de la transacción";

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

            public struct NotAllowedToModifyTransactions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para modificar transacciones";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para modificar transacciones";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para modificar transacciones";
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

            public struct NotAllowedToModifyTransactions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para modificar las transacciones";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para modificar las transacciones";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para modificar las transacciones";
            }

            public struct NotAllowedToModifyOtherUsersTransactions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para modificar las transacciones de otros usuarios";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para modificar las transacciones de otros usuarios";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para modificar las transacciones de otros usuarios";
            }

            public struct NotAllowedToMonitorTransactions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para monitorear transacciones";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para monitorear transacciones";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para monitorear transacciones";
            }

            public struct NotAllowedToSeeOwnMonitorings
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para ver sus propios monitoreos";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para ver sus propios monitoreos";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para ver sus propios monitoreos";
            }
        }

		public struct Search
        {
            public struct NotAllowedToSearchTransactions
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.WARNING;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.INFO;
                public const string LOCAL_LOG = "Usted no cuenta con los permisos para buscar transacciones";

                public const string DATABASE_LOG = "Usted no cuenta con los permisos para buscar transacciones";

                public const string MESSAGE_TITLE = "Aviso";
                public const string MESSAGE_CONTENT = "Usted no cuenta con los permisos para buscar transacciones";
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

		public struct FormView
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
				public const string LOCAL_LOG = "Carga de vista con el formulario exitosa.";

				public const string DATABASE_LOG = "Carga de vista con el formulario exitosa";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se ha cargado la vista exitosamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al cargar la vista.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Hubo un error al cargar la vista.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Hubo un error al cargar la vista\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
			}
		}

		public static class UpdateAttributeList
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
				public const string LOCAL_LOG = "Se actualizó la lista de atributos para la transacción de manera correcta";

				public const string DATABASE_LOG = "Atributos actualizados correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de atributos para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de atributos.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de atributos.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de atributos";
			};
		}

		public static class UpdateCustomFieldList
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
				public const string LOCAL_LOG = "Se actualizó la lista de controles personalizados para la transacción de manera correcta";

				public const string DATABASE_LOG = "Controles personalizados actualizados correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de controles personalizados para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de controles personalizados.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de controles personalizados.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de controles personalizados";
			};
		}

		public static class UpdateBIFieldList
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
				public const string LOCAL_LOG = "Se actualizó la lista de campos para inteligencia de negocios para la transacción de manera correcta";

				public const string DATABASE_LOG = "Campos para inteligencia de negocios actualizados correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de campos para inteligencia de negocios para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de campos para inteligencia de negocios.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de campos para inteligencia de negocios.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de campos para inteligencia de negocios";
			};
		}

		public static class UpdateDisputeCommentList
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
				public const string LOCAL_LOG = "Se actualizó la lista de disputas para la transacción de manera correcta";

				public const string DATABASE_LOG = "Disputas actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de disputas para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de disputas.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de disputas.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de disputas";
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
                    public const string LOCAL_LOG = "Correo de creación de disputa enviado";

                    public const string MESSAGE_TITLE = "Éxito";
                    public const string MESSAGE_CONTENT = "Correo de creación de disputa enviado";
                };

                public struct Error
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la creación de disputa. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                    public const string MESSAGE_TITLE = "Error";
                    public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la creación de disputa";
                };

                public struct ServerError
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la creación de disputa";
                };
            }
        }

		public static class UpdateInvalidationCommentList
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
				public const string LOCAL_LOG = "Se actualizó la lista de invalidaciones para la transacción de manera correcta";

				public const string DATABASE_LOG = "Invalidaciones actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de invalidaciones para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de invalidaciones.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de invalidaciones.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de invalidaciones";
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
                    public const string LOCAL_LOG = "Correo de creación de invalidación enviado";

                    public const string MESSAGE_TITLE = "Éxito";
                    public const string MESSAGE_CONTENT = "Correo de creación de invalidación enviado";
                };

                public struct Error
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la creación de invalidación. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                    public const string MESSAGE_TITLE = "Error";
                    public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la creación de invalidación";
                };

                public struct ServerError
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la creación de invalidación";
                };
            }
        }

		public static class UpdateDevolutionCommentList
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
				public const string LOCAL_LOG = "Se actualizó la lista de devoluciones para la transacción de manera correcta";

				public const string DATABASE_LOG = "Devoluciones actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de devoluciones para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de devoluciones.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de devoluciones.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de devoluciones";
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
                    public const string LOCAL_LOG = "Correo de creación de devolución enviado";

                    public const string MESSAGE_TITLE = "Éxito";
                    public const string MESSAGE_CONTENT = "Correo de creación de devolución enviado";
                };

                public struct Error
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    //public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    //public const string LOCAL_LOG = "Ha ocurrido un error al momento de enviar el correo para la creación de devolución. \r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                    public const string MESSAGE_TITLE = "Error";
                    public const string MESSAGE_CONTENT = "Ha ocurrido un error al momento de enviar el correo para la creación de devolución";
                };

                public struct ServerError
                {
                    public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                    public const Notification.Type TYPE = Notification.Type.ERROR;

                    public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                    public const string LOCAL_LOG = "Ha ocurrido un error en el servidor de correos para la creación de devolución";
                };
            }
        }

		public static class UpdateTransactionLabelList
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
				public const string LOCAL_LOG = "Se actualizó la lista de etiquetas para la transacción de manera correcta";

				public const string DATABASE_LOG = "Etiquetas actualizadas correctamente";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se actualizó la lista de etiquetas para la transacción de manera correcta";
			};

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Ha ocurrido un error al actualizar la lista de etiquetas.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al actualizar la lista de etiquetas.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Ha ocurrido un error al actualizar la lista de etiquetas";
			};
		}

		public struct DownloadPDF
        {
            public struct Error
            {
                public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

                public const Notification.Type TYPE = Notification.Type.ERROR;

                public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
                public const string LOCAL_LOG = "Ha ocurrido un error al descargar el archivo PDF para la transacción.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string DATABASE_LOG = "ERROR - Ha ocurrido un error al descargar el archivo PDF para la transacción.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

                public const string MESSAGE_TITLE = "Error";
                public const string MESSAGE_CONTENT = "Ha ocurrido un error al descargar el archivo PDF para la transacción";
            };
        }

		public struct ImportData
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
				public const string LOCAL_LOG = "Importación de datos exitosa";

				public const string DATABASE_LOG = "Importación de datos exitosa";

				public const string MESSAGE_TITLE = "Éxito";
				public const string MESSAGE_CONTENT = "Se han importado las transacciones correctamente";
			}

			public struct Error
			{
				public const SCC_BL.DBValues.Catalog.ELEMENT METHOD_ELEMENT_CATEGORY = ELEMENT_CATEGORY;

				public const Notification.Type TYPE = Notification.Type.ERROR;

				public const Notification.LogLevel LOCAL_LOG_LEVEL = Notification.LogLevel.ERROR;
				public const string LOCAL_LOG = "Han ocurrido errores al importar las transacciones.\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string DATABASE_LOG = "Han ocurrido errores al importar las transacciones\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

				public const string MESSAGE_TITLE = "Error";
				public const string MESSAGE_CONTENT = "Han ocurrido errores al importar las transacciones\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
			}

			public struct ErrorList
            {
				public struct User
                {
					public struct Agent
                    {
						public const string ELEMENT_NAME = "usuario agente";

						public const string NO_IDENTIFICATION_ENTERED = "No se ha encontrado la identificación en el archivo para el " + ELEMENT_NAME;
						public const string NO_IDENTIFICATION_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tIdentificación: " + REPLACE_CUSTOM_CONTENT;

						public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
						public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
						public const string NO_VALID_FORMAT = "No se ha ingresado el formato correcto para el nombre del " + ELEMENT_NAME + ".\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

						public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
					}

					public struct Supervisor
                    {
						public const string ELEMENT_NAME = "usuario supervisor";

						public const string NO_IDENTIFICATION_ENTERED = "No se ha encontrado la identificación en el archivo para el " + ELEMENT_NAME;
						public const string NO_IDENTIFICATION_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tIdentificación: " + REPLACE_CUSTOM_CONTENT;

						public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
						public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
						public const string NO_VALID_FORMAT = "No se ha ingresado el formato correcto para el nombre del " + ELEMENT_NAME + ".\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

						public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
					}

					public struct Evaluator
                    {
						public const string ELEMENT_NAME = "usuario evaluador";

						public const string NO_IDENTIFICATION_ENTERED = "No se ha encontrado la identificación en el archivo para el " + ELEMENT_NAME;
						public const string NO_IDENTIFICATION_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tIdentificación: " + REPLACE_CUSTOM_CONTENT;

						public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
						public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
						public const string NO_VALID_FORMAT = "No se ha ingresado el formato correcto para el nombre del " + ELEMENT_NAME + ".\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

						public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
					}
                }

				public struct Program
				{
					public const string ELEMENT_NAME = "programa";

					public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
					public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct ProgramFormCatalog
				{
					public const string ELEMENT_NAME = "elemento de catálogo de programas en formulario";

					public const string NO_PROGRAM_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tID del programa: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct Attribute
				{
					public const string ELEMENT_NAME = "atributo";

					public const string NO_COLUMNS = "No se han encontrado celdas que contentan atributos en el archivo";
					public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
					public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
					public const string NO_VALUE_ENTERED = "No se ha encontrado el valor en el archivo para el " + ELEMENT_NAME;
					public const string NO_VALUE_FOUND = "No se ha encontrado al valor del " + ELEMENT_NAME + " en el sistema.\r\n\tValor: " + REPLACE_CUSTOM_CONTENT;
					public const string NO_SUBATTRIBUTE_NAME_FOUND = "No se ha encontrado al subatributo en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct CustomControl
				{
					public const string ELEMENT_NAME = "control personalizado";

					public const string NO_COLUMNS = "No se han encontrado celdas que contentan atributos en el archivo";
					public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
					public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
					public const string NO_VALUE_ENTERED = "No se ha encontrado el valor en el archivo para el " + ELEMENT_NAME;
					public const string NO_VALUE_FOUND = "No se ha encontrado al valor del " + ELEMENT_NAME + " en el sistema.\r\n\tNombre del control: " + REPLACE_CUSTOM_CONTENT_2 + ". Valor: " + REPLACE_CUSTOM_CONTENT;
					public const string EMPTY_VALUE_FOUND = "Se ha encontrado un valor vacío para el " + ELEMENT_NAME + ".";
					public const string NO_SUBATTRIBUTE_NAME_FOUND = "No se ha encontrado al subatributo en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string NO_CUSTOM_FIELD_FOUND = "No se ha encontrado al el control personalizado enlazado al formulario.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct BusinessIntelligenceField
				{
					public const string ELEMENT_NAME = "campo de inteligencia de negocios";

					public const string NO_COLUMNS = "No se han encontrado celdas que contentan atributos en el archivo";
					public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
					public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;
					public const string NO_SUBFIELD_NAME_FOUND = "No se ha encontrado al campo de inteligencia de negocios en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct Form
				{
					public const string ELEMENT_NAME = "formulario";

					public const string NO_NAME_ENTERED = "No se ha encontrado el nombre en el archivo para el " + ELEMENT_NAME;
					public const string NO_NAME_FOUND = "No se ha encontrado al " + ELEMENT_NAME + " en el sistema.\r\n\tNombre: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar al " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;
				}

				public struct Transaction
				{
					public const string ELEMENT_NAME = "transacción";

					public const string NO_IDENTIFIER_ENTERED = "No se ha encontrado el identificador en el archivo para la " + ELEMENT_NAME;
					public const string NO_EVALUATION_DATE_ENTERED = "No se ha encontrado la fecha de evaluación en el archivo para la " + ELEMENT_NAME;
					public const string NO_TRANSACTION_DATE_ENTERED = "No se ha encontrado la fecha de transacción en el archivo para la " + ELEMENT_NAME;
					public const string NO_LOAD_DATE_ENTERED = "No se ha encontrado la fecha de carga en el archivo para la " + ELEMENT_NAME;
					public const string NO_COMMENT_ENTERED = "No se ha encontrado el comentario en el archivo para la " + ELEMENT_NAME;
					public const string NO_TIME_ELAPSED_ENTERED = "No se ha encontrado el tiempo transcurrido en el archivo para la " + ELEMENT_NAME;
					public const string BAD_FORMAT_TIME_ELAPSED = "Se encontró un formato erróneo en el tiempo transcurrido en el archivo para la " + ELEMENT_NAME + ". Formato correcto: {horas}:{minutos}:{segundos}";

					public const string NO_IDENTIFIER_FOUND = "No se ha encontrado la " + ELEMENT_NAME + " en el sistema.\r\n\tIdentificador: " + REPLACE_CUSTOM_CONTENT;

					public const string UNKNOWN = "Ha ocurrido un error al buscar la " + ELEMENT_NAME + ".\r\n\tExcepción: " + REPLACE_EXCEPTION_MESSAGE;

					public struct Results
                    {
						public struct General
						{
							public const string NO_GENERAL_RESULT_ENTERED = "No se ha encontrado el resultado general en el archivo para la " + ELEMENT_NAME;
							public const string NO_FUCE_RESULT_ENTERED = "No se ha encontrado el resultado general de error crítico de usuario final en el archivo para la " + ELEMENT_NAME;
							public const string NO_BCE_RESULT_ENTERED = "No se ha encontrado el resultado general de error crítico de negocios en el archivo para la " + ELEMENT_NAME;
							public const string NO_FCE_RESULT_ENTERED = "No se ha encontrado el resultado general de error crítico de cumplimiento en el archivo para la " + ELEMENT_NAME;
							public const string NO_NCE_RESULT_ENTERED = "No se ha encontrado el resultado general de error no crítico en el archivo para la " + ELEMENT_NAME;
						}

						public struct Accurate
						{
							public const string NO_FUCE_RESULT_ENTERED = "No se ha encontrado el resultado de precisión de error crítico de usuario final en el archivo para la " + ELEMENT_NAME;
							public const string NO_BCE_RESULT_ENTERED = "No se ha encontrado el resultado de precisión de error crítico de negocios en el archivo para la " + ELEMENT_NAME;
							public const string NO_FCE_RESULT_ENTERED = "No se ha encontrado el resultado de precisión de error crítico de cumplimiento en el archivo para la " + ELEMENT_NAME;
						}

						public struct Controllable
						{
							public const string NO_FUCE_RESULT_ENTERED = "No se ha encontrado el resultado controlable de error crítico de usuario final en el archivo para la " + ELEMENT_NAME;
							public const string NO_BCE_RESULT_ENTERED = "No se ha encontrado el resultado controlable de error crítico de negocios en el archivo para la " + ELEMENT_NAME;
							public const string NO_FCE_RESULT_ENTERED = "No se ha encontrado el resultado controlable de error crítico de cumplimiento en el archivo para la " + ELEMENT_NAME;
						}
                    }
				}
			}
		}
	}
}