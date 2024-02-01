using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCC_BL.Settings.HTML_Content.Transaction
{
    public class DownloadPDF
    {
        public const string PATH = "~/HTML/Transaction/DownloadPDF/DownloadPDF.html";
        public const string CSS_PATH = "~/Content/Custom/Transaction/download-pdf.css";
        public const string SUBJECT = "SCC - PDF de transacción";

        public struct ReplaceableValues
        {
            public const string STYLE_CONTENT = "%STYLE_CONTENT%";
            public const string VERSION = "%VERSION%";
            public const string TRANSACTION_IDENTIFIER = "%TRANSACTION_IDENTIFIER%";
            public const string PROGRAM_NAME = "%PROGRAM_NAME%";
            public const string EVALUATED_USER_PERSON_NAME = "%EVALUATED_USER_PERSON_NAME%";
            public const string EVALUATOR_USER_PERSON_NAME = "%EVALUATOR_USER_PERSON_NAME%";
            public const string EVALUATION_DATE = "%EVALUATION_DATE%";
            public const string TRANSACTION_DATE = "%TRANSACTION_DATE%";
            public const string TRANSACTION_COMMENT = "%TRANSACTION_COMMENT%";
            public const string TRANSACTION_DEVOLUTION_STRENGTHS_CONTENT = "%TRANSACTION_DEVOLUTION_STRENGTHS_CONTENT%";
            public const string TRANSACTION_DEVOLUTION_IMPROVEMENT_STEPS_CONTENT = "%TRANSACTION_DEVOLUTION_IMPROVEMENT_STEPS_CONTENT%";
            public const string CUSTOM_CONTROL_LABEL = "%CUSTOM_CONTROL_LABEL%";
            public const string CUSTOM_CONTROL_VALUE = "%CUSTOM_CONTROL_VALUE%";
            public const string TRANSACTION_GLOBAL_RESULT = "%TRANSACTION_GLOBAL_RESULT%";
            public const string TRANSACTION_GLOBAL_RESULT_FUCE = "%TRANSACTION_GLOBAL_RESULT_FUCE%";
            public const string TRANSACTION_GLOBAL_RESULT_BCE = "%TRANSACTION_GLOBAL_RESULT_BCE%";
            public const string TRANSACTION_GLOBAL_RESULT_FCE = "%TRANSACTION_GLOBAL_RESULT_FCE%";
            public const string TRANSACTION_GLOBAL_RESULT_NCE = "%TRANSACTION_GLOBAL_RESULT_NCE%";
            public const string TRANSACTION_CUSTOM_CONTROL_CONTENT = "%TRANSACTION_CUSTOM_CONTROL_CONTENT%";
            public const string TRANSACTION_ATTRIBUTE_CONTENT = "%TRANSACTION_ATTRIBUTE_CONTENT%";
            public const string TRANSACTION_PARENT_ATTRIBUTE_TYPE_CONTENT = "%TRANSACTION_PARENT_ATTRIBUTE_TYPE_CONTENT%";
            public const string TRANSACTION_PARENT_ATTRIBUTE_CONTENT = "%TRANSACTION_PARENT_ATTRIBUTE_CONTENT%";
            public const string TRANSACTION_CHILD_ATTRIBUTE_CONTENT = "%TRANSACTION_CHILD_ATTRIBUTE_CONTENT%";
            public const string TRANSACTION_BUSINESS_INTELLIGENCE_CONTENT = "%TRANSACTION_BUSINESS_INTELLIGENCE_CONTENT%";
        }

        public struct Content
        {
            public const string CHECKED_SYMBOL = "&#x2611";
            public const string UNCHECKED_SYMBOL = "&#x2610";

            public struct TransactionResults
            {
                public struct TransactionResult
                {
                    /*public const string SUCCESS =
                        "<div class=\"d-block text-center p-2 result-general-success\">\r\n" +
                            "<span class=\"bg-success p-2 border border-rounded border-success-strong text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-block text-center p-2 result-general-fail\">\r\n" +
                            "<span class=\"bg-danger p-2 border border-rounded border-fail-strong text-fail-strong\">Falló</span>\r\n" +
                        "</div>";*/

                    public const string SUCCESS =
                        "<div class=\"d-inline-block text-center p-2 result-general-success\">\r\n" +
                            "<span class=\"text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-inline-block text-center p-2 result-general-fail\">\r\n" +
                            "<span class=\"text-fail-strong\">Falló</span>\r\n" +
                        "</div>";
                }

                public struct TransactionResultFUCE
                {
                    /*public const string SUCCESS =
                        "<div class=\"d-block text-center p-2 result-general-fuce-success\">\r\n" +
                            "<span class=\"bg-success p-2 border border-rounded  border-success-strong text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-block text-center p-2 result-general-fuce-fail\">\r\n" +
                            "<span class=\"bg-danger p-2 border border-rounded border-fail-strong text-fail-strong\">Falló</span>\r\n" +
                        "</div>";*/

                    public const string SUCCESS =
                        "<div class=\"d-inline-block text-center p-2 result-general-fuce-success\">\r\n" +
                            "<span class=\"text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-inline-block text-center p-2 result-general-fuce-fail\">\r\n" +
                            "<span class=\"text-fail-strong\">Falló</span>\r\n" +
                        "</div>";
                }

                public struct TransactionResultBCE
                {
                    /*public const string SUCCESS =
                        "<div class=\"d-block text-center p-2 result-general-bce-success\">\r\n" +
                            "<span class=\"bg-success p-2 border border-rounded  border-success-strong text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-block text-center p-2 result-general-bce-fail\">\r\n" +
                            "<span class=\"bg-danger p-2 border border-rounded border-fail-strong text-fail-strong\">Falló</span>\r\n" +
                        "</div>";*/

                    public const string SUCCESS =
                        "<div class=\"d-inline-block text-center p-2 result-general-bce-success\">\r\n" +
                            "<span class=\"text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-inline-block text-center p-2 result-general-bce-fail\">\r\n" +
                            "<span class=\"text-fail-strong\">Falló</span>\r\n" +
                        "</div>";
                }

                public struct TransactionResultFCE
                {
                    /*public const string SUCCESS =
                        "<div class=\"d-block text-center p-2 result-general-fce-success\">\r\n" +
                            "<span class=\"bg-success p-2 border border-rounded  border-success-strong text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-block text-center p-2 result-general-fce-fail\">\r\n" +
                            "<span class=\"bg-danger p-2 border border-rounded border-fail-strong text-fail-strong\">Falló</span>\r\n" +
                        "</div>";*/

                    public const string SUCCESS =
                        "<div class=\"d-inline-block text-center p-2 result-general-fce-success\">\r\n" +
                            "<span class=\"text-success-strong\">Pasó</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-inline-block text-center p-2 result-general-fce-fail\">\r\n" +
                            "<span class=\"text-fail-strong\">Falló</span>\r\n" +
                        "</div>";
                }

                public struct TransactionResultNCE
                {
                    public const string REPLACE_NCR_SCORE = "%REPLACE_NCR_SCORE%";

                    /*public const string SUCCESS =
                        "<div class=\"d-block text-center p-2 result-nce-success\">\r\n" +
                            "<span class=\"bg-success p-2 border border-rounded  border-success-strong text-success-strong result-general-nce-score\">" + REPLACE_NCR_SCORE + "</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-block text-center p-2 result-nce-fail\">\r\n" +
                            "<span class=\"bg-danger p-2 border border-rounded  border-fail-strong text-fail-strong result-general-nce-score\">" + REPLACE_NCR_SCORE + "</span>\r\n" +
                        "</div>";*/

                    public const string SUCCESS =
                        "<div class=\"d-inline-block text-center p-2 result-nce-success\">\r\n" +
                            "<span class=\"text-success-strong result-general-nce-score\">" + REPLACE_NCR_SCORE + "</span>\r\n" +
                        "</div>";
                    public const string FAIL =
                        "<div class=\"d-inline-block text-center p-2 result-nce-fail\">\r\n" +
                            "<span class=\"text-fail-strong result-general-nce-score\">" + REPLACE_NCR_SCORE + "</span>\r\n" +
                        "</div>";
                }
            }

            public const string TAB_VALUE = "&emsp;&emsp;&emsp;";
            public const string TAB_CONTENT = "%TAB_CONTENT%";

            public struct Attribute
            {
                public const string ATTRIBUTE_BODY_CONTENT = "%ATTRIBUTE_BODY_CONTENT%";

                public const string CONTENT =
                    ReplaceableValues.TRANSACTION_PARENT_ATTRIBUTE_TYPE_CONTENT +
                    ATTRIBUTE_BODY_CONTENT;

                public const string ATTRIBUTE_NAME = "%ATTRIBUTE_NAME%";
                public const string ATTRIBUTE_VALUE = "%ATTRIBUTE_VALUE%";
                public const string ATTRIBUTE_COMMENT = "%ATTRIBUTE_COMMENT%";
                public const string ATTRIBUTE_CHECKED = "%ATTRIBUTE_CHECKED%";
                public const string ATTRIBUTE_TYPE = "%ATTRIBUTE_TYPE%";

                public struct AttributeType
                {
                    public const string CONTENT = 
                        "<tr class=\"bg-primary text-light\">\r\n" +
                            "<td class=\"p-2\">\r\n" +
                                ATTRIBUTE_TYPE + "\r\n" +
                            "</td>\r\n" +
                            "<td class=\"p-2\">\r\n" +
                            "</td>\r\n" +
                            "<td class=\"p-2\">\r\n" +
                            "</td>\r\n" +
                        "</tr>\r\n";
                }

                public struct ParentAttribute
                {
                    public const string CONTENT = "\r\n" + 
                        "<tr class=\"border border-bottom border-secondary-dark\">\r\n" + 
                            "<td class=\"p-2\">\r\n" +
                                ATTRIBUTE_NAME + "\r\n" + 
                            "</td>\r\n" + 
                            "<td class=\"text-center p-2\">\r\n" +
                                ATTRIBUTE_VALUE + "\r\n" + 
                            "</td>\r\n" + 
                            "<td class=\"p-2\">\r\n" +
                                ATTRIBUTE_COMMENT + "\r\n" + 
                            "</td>\r\n" + 
                        "</tr>";
                }

                public struct ChildAttribute
                {
                    /*public const string CHILD_ATTRIBUTE_CONTENT = 
                        TAB_CONTENT + "<input type=\"checkbox\" value=\"\" " + ATTRIBUTE_CHECKED + " disabled />\r\n" + 
                        "<label> " + ATTRIBUTE_NAME + "</label>\r\n" + 
                        "<br>\r\n";*/

                    public const string CHILD_ATTRIBUTE_CONTENT = 
                        TAB_CONTENT + ATTRIBUTE_CHECKED + 
                        "<label> " + ATTRIBUTE_NAME + "</label>\r\n" + 
                        "<br>\r\n";

                    public const string CONTENT = "\r\n" + 
                        "<tr class=\"bg-secondary border border-bottom border-dark\">\r\n" + 
                            "<td class=\"p-2\">\r\n" +
                                CHILD_ATTRIBUTE_CONTENT +
                            "</td>\r\n" + 
                            "<td class=\"p-2\">\r\n" +
                                ATTRIBUTE_VALUE + "\r\n" +
                            "</td>\r\n" + 
                            "<td class=\"p-2\">\r\n" +
                                ATTRIBUTE_COMMENT + "\r\n" +
                            "</td>\r\n" + 
                        "</tr>";
                }
            }

            public struct Commentaries
            {
                public const string FULL_TRANSACTION_COMMENTARIES = "%FULL_TRANSACTION_COMMENTARIES%";

                public const string TRANSACTION_DISPUTATION_CONTAINER_CONTENT = "%TRANSACTION_DISPUTATION_CONTAINER_CONTENT%";
                public const string TRANSACTION_INVALIDATION_CONTAINER_CONTENT = "%TRANSACTION_INVALIDATION_CONTAINER_CONTENT%";
                public const string TRANSACTION_DEVOLUTION_CONTAINER_CONTENT = "%TRANSACTION_DEVOLUTION_CONTAINER_CONTENT%";

                public const string TRANSACTION_DISPUTATION_DATE = "%TRANSACTION_DISPUTATION_DATE%";
                public const string TRANSACTION_DISPUTATION_COMMENT_BY_USER_USERNAME = "%TRANSACTION_DISPUTATION_COMMENT_BY_USER_USERNAME%";
                public const string TRANSACTION_DISPUTATION_COMMENT_CONTENT = "%TRANSACTION_DISPUTATION_COMMENT_CONTENT%";

                public const string TRANSACTION_INVALIDATION_DATE = "%TRANSACTION_INVALIDATION_DATE%";
                public const string TRANSACTION_INVALIDATION_COMMENT_BY_USER_USERNAME = "%TRANSACTION_INVALIDATION_COMMENT_BY_USER_USERNAME%";
                public const string TRANSACTION_INVALIDATION_COMMENT_CONTENT = "%TRANSACTION_INVALIDATION_COMMENT_CONTENT%";

                public const string TRANSACTION_DEVOLUTION_DATE = "%TRANSACTION_DEVOLUTION_DATE%";
                public const string TRANSACTION_DEVOLUTION_COMMENT_BY_USER_USERNAME = "%TRANSACTION_DEVOLUTION_COMMENT_BY_USER_USERNAME%";
                public const string TRANSACTION_DEVOLUTION_COMMENT_CONTENT = "%TRANSACTION_DEVOLUTION_COMMENT_CONTENT%";
                public const string TRANSACTION_DEVOLUTION_STRENGTHS_CONTENT = "%TRANSACTION_DEVOLUTION_STRENGTHS_CONTENT%";
                public const string TRANSACTION_DEVOLUTION_IMPROVEMENT_STEPS_CONTENT = "%TRANSACTION_DEVOLUTION_IMPROVEMENT_STEPS_CONTENT%";

                public const string TRANSACTION_DISPUTATION_CONTAINER = "\r\n" +
                            "<fieldset class=\"border border-secondary p-2\">\r\n" + 
                                "<legend>\r\n" + 
                                    "<strong>\r\n" + 
                                        "Disputa\r\n" + 
                                    "</strong>\r\n" + 
                                "</legend>\r\n" + 
                                "<div class=\"grid-container\">\r\n" + 
                                    "<p>\r\n" + 
                                        "Comentarios de disputa:\r\n" + 
                                        "<br>\r\n" + 
                                        "<br>\r\n" + 
                                        "%TRANSACTION_DISPUTATION_COMMENT_CONTENT%\r\n" + 
                                    "</p>\r\n" + 
                                "</div>\r\n" + 
                            "</fieldset>\r\n" + 
                            "<br>\r\n";

                public const string TRANSACTION_INVALIDATION_CONTAINER = "\r\n" +
                            "<fieldset class=\"border border-secondary p-2\">\r\n" + 
                                "<legend>\r\n" + 
                                    "<strong>\r\n" + 
                                        "Invalidación\r\n" + 
                                    "</strong>\r\n" + 
                                "</legend>\r\n" + 
                                "<div class=\"grid-container\">\r\n" + 
                                    "<p>\r\n" + 
                                        "Comentarios de invalidación:\r\n" + 
                                        "<br>\r\n" + 
                                        "<br>\r\n" + 
                                        "%TRANSACTION_INVALIDATION_COMMENT_CONTENT%\r\n" + 
                                    "</p>\r\n" + 
                                "</div>\r\n" + 
                            "</fieldset>\r\n" + 
                            "<br>\r\n";

                public const string TRANSACTION_DEVOLUTION_CONTAINER = "\r\n" +
                            "<fieldset class=\"border border-secondary p-2\">\r\n" + 
                                "<legend>\r\n" + 
                                    "<strong>\r\n" + 
                                        "Devolución\r\n" + 
                                    "</strong>\r\n" + 
                                "</legend>\r\n" + 
                                "<div class=\"grid-container\">\r\n" + 
                                    "<p>\r\n" + 
                                        "Comentarios de devolución:\r\n" + 
                                        "<br>\r\n" + 
                                        "<br>\r\n" + 
                                        "%TRANSACTION_DEVOLUTION_COMMENT_CONTENT%\r\n" + 
                                    "</p>\r\n" + 
                                    "<p>\r\n" + 
                                        "Fortalezas del usuario:\r\n" + 
                                        "<br>\r\n" + 
                                        "<br>\r\n" + 
                                        "%TRANSACTION_DEVOLUTION_STRENGTHS_CONTENT%\r\n" + 
                                    "</p>\r\n" + 
                                    "<p>\r\n" + 
                                        "Pasos de mejora:\r\n" + 
                                        "<br>\r\n" + 
                                        "<br>\r\n" + 
                                        "%TRANSACTION_DEVOLUTION_IMPROVEMENT_STEPS_CONTENT%\r\n" + 
                                    "</p>\r\n" + 
                                "</div>\r\n" + 
                            "</fieldset>\r\n" + 
                            "<br>\r\n";

                public const string FULL_TRANSACTION_COMMENTARIES_CONTENT = "\r\n" + 
                    "<div class=\"grid-container p-2\">\r\n" + 
                        "<fieldset class=\"border border-secondary p-3\">\r\n" + 
                            "<legend class=\"p-2\">\r\n" + 
                                "<strong>\r\n" + 
                                    "Comentarios de la transacción\r\n" + 
                                "</strong>\r\n" + 
                            "</legend>\r\n" +
                            "%TRANSACTION_DISPUTATION_CONTAINER_CONTENT%\r\n" +
                            "%TRANSACTION_INVALIDATION_CONTAINER_CONTENT%\r\n" +
                            "%TRANSACTION_DEVOLUTION_CONTAINER_CONTENT%\r\n" +
                        "</fieldset>\r\n" + 
                    "</div>";
            }

            public struct CustomControl
            {
                public const string CUSTOM_CONTROL_COUNT = "%CUSTOM_CONTROL_COUNT%";
                public const string CUSTOM_CONTROL_LABEL = "%CUSTOM_CONTROL_LABEL%";
                public const string CUSTOM_CONTROL_VALUE = "%CUSTOM_CONTROL_VALUE%";

                public const string CONTENT = "\r\n" +
                    "<p>" + CUSTOM_CONTROL_COUNT + " - " + CUSTOM_CONTROL_LABEL + ": " + CUSTOM_CONTROL_VALUE + "</p>\r\n";
                    //"<br>\r\n";
            }

            public struct BusinessIntelligence
            {
                public const string FULL_TRANSACTION_BUSINESS_INTELLIGENCE = "%FULL_TRANSACTION_BUSINESS_INTELLIGENCE%";
                public const string BUSINESS_INTELLIGENCE_NAME = "%BUSINESS_INTELLIGENCE_NAME%";
                public const string BUSINESS_INTELLIGENCE_CHECKED = "%BUSINESS_INTELLIGENCE_CHECKED%";
                public const string TITLE = "%TITLE%";

                public const string FULL_TRANSACTION_BUSINESS_INTELLIGENCE_CONTENT = "\r\n" + 
                    "<fieldset class=\"border border-secondary\">\r\n" + 
                        "<legend>\r\n" + 
                            "<strong>\r\n" + 
                                "<u>\r\n" + 
                                    "Inteligencia de negocios\r\n" + 
                                "</u>\r\n" + 
                            "</strong>\r\n" + 
                        "</legend>\r\n" + 
                        "%TRANSACTION_BUSINESS_INTELLIGENCE_CONTENT%\r\n" + 
                    "</fieldset>";

                public const string TITLE_CONTENT =
                    "<b>\r\n" +
                        "<i>\r\n" +
                            TITLE + "\r\n" +
                        "</i>\r\n" +
                    "</b>\r\n" +
                    "<br>\r\n";

                /*public const string CONTENT = "\r\n" +
                    TAB_CONTENT + "<input type=\"checkbox\" value=\"\" " + BUSINESS_INTELLIGENCE_CHECKED + " disabled />\r\n" +
                    "<label> " + BUSINESS_INTELLIGENCE_NAME + "</label>\r\n" +
                    "<br>\r\n";*/

                public const string CONTENT = "\r\n" +
                    TAB_CONTENT + BUSINESS_INTELLIGENCE_CHECKED +
                    "<label> " + BUSINESS_INTELLIGENCE_NAME + "</label>\r\n" +
                    "<br>\r\n";
            }
        }
    }
}
