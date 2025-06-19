namespace CashCanvas.Common.ConstantHandler
{
    public class Constants
    {
        #region  Module/Entity Name
        public const string USER = "User";
        public const string TRANSACTION = "Transaction";
        public const string BILL = "Bill";
        public const string BUDGET = "Budget";
        public const string PASSWORD = "Password";
        public const string USER_LIST = "User List";
        public const string TRANSACTION_LIST = "Transaction List";
        public const string BILL_LIST = "Bill List";
        public const string PAYMENT_LIST = "Payment List";
        public const string BUDGET_LIST = "Budget List";
        public const string RESET_PASSWORD_TOKEN = "Reset Password Token";
        public const string WAITING_TOKEN = "Waiting Token";
        public const string MAPPING_RELATIONS = "Mapping Relations";
        public const string DEFAULT_ENTITY = "Entity";
        public const string PAGINATION = "Pagination Details";
        #endregion
        #region Database Actions
        public const string DATABASE_ACTION_READ = "Read";
        public const string DATABASE_ACTION_CREATE = "Create";
        public const string DATABASE_ACTION_UPDATE = "Update";
        public const string DATABASE_ACTION_DELETE = "Delete";

        #endregion
        #region  Email Subject
        public const string EMAIL_SUBJECT_FORGOT_PASSWORD = "Password Reset Request";
        public const string EMAIL_SUBJECT_ADD_USER = "New User Registration";
        #endregion
        #region HTTP State Related
        public const string COOKIE_KEY = "Cookie key";
        public const string COOKIE_VALUE = "Cookie value";
        public const string COOKIE_NAME = "Cookie name";
        public const string SESSION_KEY = "Cookie key";
        public const string JWT_KEY = "JWT key";
        #endregion

        #region Transaction Types
        public const string TRANSACTION_TYPE_EXPENSE = "Expense";
        public const string TRANSACTION_TYPE_INCOME = "Income";
        #endregion
        #region Column Order 
        public const string ASC_ORDER = "asc";
        public const string DESC_ORDER = "desc";
        #endregion

        #region  Layouts
        public const string LOGIN_LAYOUT = "_AuthLayout";
        public const string MAIN_LAYOUT = "_Layout";
        public const string LAYOUT_VARIABLE_NAME = "LayoutName";
        #endregion

        #region  Tokens
        public const string ACCESS_TOKEN = "AccessToken";
        public const string REFRESH_TOKEN = "RefreshToken";
        public const string INVALID_ACCESS_TOKEN = "Invalid access token. Possible tampering detected.";
        public const string INVALID_REFRESH_TOKEN = "Invalid refresh token";
        public const string SESSION_EMAIL = "Email";
        public const string SESSION_USERNAME = "UserName";

        #endregion

        #region Views and Controller

        public const string DASHBOARD_VIEW = "Index";
        public const string USER_LIST_VIEW = "UserList";
        public const string LOGIN_VIEW = "Index";
        public const string EXPORT_BILLS_VIEW = "BillListExport";
        public const string EXPORT_TRANSACTION_VIEW = "TransactionsListExport";
        public const string ERROR_VIEW = "HttpStatusCodeHandler";
        public const string HOME_CONTROLLER = "Home";
        public const string LOGIN_CONTROLLER = "Authentication";
        public const string USER_CONTROLLER = "User";
        public const string ERROR_CONTROLLER = "ErrorHandler";

        public const string UNKNOWN_ACTION_NAME = "Unknown";
        public const string UNKNOWN_CONTROLLER_NAME = "Unknown";
        public const string ERROR_HANDLER_HTTP_STATUS_CODE_HANDLER_ROUTE = "/ErrorHandler/HttpStatusCodeHandler/{0}";
        public const string ERROR_HANDLER_HTTP_STATUS_CODE_500_ROUTE = "/ErrorHandler/HttpStatusCodeHandler/500";
        public const string ERROR_HANDLER_HTTP_STATUS_CODE_404_ROUTE = "/ErrorHandler/HttpStatusCodeHandler/404";
        public const string ERROR_HANDLER_ROUTE = "/ErrorHandler";

        #endregion

        #region Partial Views
        public const string PARTIAL_DASHBOARD_GRID = "_parial_Dashboard_Grid";
        public const string PARTIAL_USER_GRID = "_partial_UserGrid";
        #endregion


        #region  Module Names

        public const string USERS_MODULE = "Users";
        public const string ROLE_AND_PERMISSION_MODULE = "RoleAndPermission";

        #endregion

        #region  Static Files Related

        public const string IMAGE_TYPE = "image/jpeg";
        public const string FORGOT_PASSWORD_FILE = "ForgotPasswordFormat.html";
        public const string EXPORT_FILE_GENERATION_ERROR = "An error occurred while generating the export file. Please try again later.";
        public const string EXPORT_FILE_GENERATION_SUCCESS = "The export file was generated successfully.";

        public const string TEMPLATE_NOT_FOUND = "Template not found.";

        public const string DATE_FORMATE = "yyyy-MM-dd";
        public const string PDF_CONTENT_TYPE = "application/pdf";
        public const string IMAGE_FORMATE = "data:image/jpeg;base64";
        public const string JSON_CONTENT_TYPE = "application/json";
        #endregion

        #region sort column
        public const string SORT_BY_DATE = "Date";


        #endregion

        #region Configuration Strings
        public const string JWT_CONFIG = "JwtConfig";
        public const string APP_BASE_URL = "AppBaseUrl";
        public const string EMAIL_CONFIG = "EmailSettings";
        public const string DATABASE_DEFAULT_CONNECTION = "DefaultConnection";
        public const string MAX_FILE_UPLOAD_SIZE = "FileUpload:MaxMultipartBodyLengthInBytes";
        public const string DEFAULT_ROUTE_CONFIG = "RouteSettings";
        public const int SESSION_IDLE_TIME_OUT_HOURS = 10;
        public const string UNKNOWN_IP = "Unknown IP";
        public const string LOGGING_PATH_URL = "Logging:ErrorLogPath";
        public static string BILL_LIST_EXCEL_FORMAT_FILE = "BillListExcelFormat.xlsx";
        #endregion

    }
}
