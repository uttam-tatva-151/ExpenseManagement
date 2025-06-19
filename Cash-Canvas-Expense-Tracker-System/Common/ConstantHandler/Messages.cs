namespace CashCanvas.Common.ConstantHandler
{
    public class Messages
    {
         #region  Success Messages
            public const string SUCCESS_READ = "{0} fetch successfully";
            public const string SUCCESS_ADDED = "{0} added successfully.";
            public const string SUCCESS_UPDATED = "{0} updated successfully.";
            public const string SUCCESS_DELETED = "{0} deleted successfully.";
            public const string GENERAL_SUCCESS = "Operation completed successfully.";
            public const string SUCCESS_LOGIN = "Login successfully.";
            public const string SUCCESS_LOGOUT = "Logout successfully.";
            public const string SUCCESS_FORGOT_PASSWORD = "Password reset link sent to your email.";
            public const string SUCCESS_PASSWORD_CHANGED = "Password changed successfully.";
            public const string SUCCESS_EMAIL_SENT = "Email sent successfully.";
            public const string SUCCESS_TOKEN_REFRESHED = "Token refreshed successfully.";
            public const string ANALYSIS_COMPLETED = "Records analysis has been successfully completed.";
            public const string LOGOUT_CONFIRMATION = "Are you sure you want to Logout this ID?";

            #endregion
            #region  Error Messages

            public const string ERROR_FEATCHING = "An error occurred while featching {0}. Please try again.";
            public const string ERROR_ADDING = "An error occurred while adding {0}. Please try again.";
            public const string ERROR_UPDATING = "An error occurred while updating {0}. Please try again.";
            public const string ERROR_DELETING = "An error occurred while deleting {0}. Please try again.";
            public const string ERROR_NOT_FOUND = "{0} not found.";
            public const string GENERAL_ERROR = "An error occurred. Please try again.";
            public const string ERROR_LOGIN = "Login failed. Please check your Email.";
            public const string ERROR_PASSWORD_MISMATCH = "Password is Incorrect.";
            public const string ERROR_FORGOT_PASSWORD = "An error occurred while sending the password reset link. Please try again.";
            public const string ERROR_EMAIL_SENDING = "An error occurred while sending the email. Please try again.";
            public const string ERROR_SENDING_LOGIN_DETAILS_EMAIL = "Error to send Login Details to user's MailId.";
            public const string ERROR_MISSING_DB_CONNECTION = "Database connection string is missing or empty in configuration.";
            public const string ERROR_MISSING_JWT_SECTION = "JWT configuration section is missing or invalid.";
            public const string ERROR_INVALID_JWT_VALUES = "JWT configuration values (Key, Issuer, Audience) must not be null or empty.";
            public const string ACCOUNT_LOCKED = "User account is locked due to multiple failed login attempts. Please try again after {0} Minutes.";
            public const string EXCEPTION_TYPE_AND_STATUS_CODE_REQUIRED = "ExceptionType and StatusCode are required.";
            #endregion

            #region  Warning Messages
            public const string WARNING_INVALID_INPUT = "Invalid input provided for {0}.";
            public const string WARNING_INVALID_CREDENTIALS = "Invalid credentials.";
            public const string WARNING_ALL_READY_EXISTS = "{0} already exists.";
            public const string WARNING_DELETE_CONFIRMATION = "Are you sure want to delete this {0}?";
            public const string WARNING_MULTIPLE_DELETE_CONFIRMATION = "Are you sure want to delete this selected {0}s?";
            public const string WARNING_EMAIL_NOT_FOUND = "Email not found.";
            public const string WARNING_EMAIL_ALL_READY_EXISTS = "Email already exists.";
            public const string WARNING_NOTHING_SELECTED = "Please select at least one {0}.";
            public const string WARNING_RESET_TOKEN_EXPIRED = "Reset Password Link Expired";
            #endregion
            #region  Info Messages
            public const string INFO_LOADING = "Loading {0}...";
            public const string INFO_NO_RECORDS_FOUND = "No {0} records found.";
            #endregion
    }
}

