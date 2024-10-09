using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiCareSystem.Common
{

    public class Const
    {
        #region Error Codes

        public static int ERROR_EXCEPTION = -4;

        #endregion

        #region Success Codes

        public static int SUCCESS_CREATE_CODE = 1;
        public static string SUCCESS_CREATE_MSG = "Save data success";
        public static int SUCCESS_READ_CODE = 1;
        public static string SUCCESS_READ_MSG = "Get data success";
        public static int SUCCESS_UPDATE_CODE = 1;
        public static string SUCCESS_UPDATE_MSG = "Update data success";
        public static int SUCCESS_DELETE_CODE = 1;
        public static string SUCCESS_DELETE_MSG = "Delete data success";


        #endregion

        #region Fail code

        public static int FAIL_CREATE_CODE = -1;
        public static string FAIL_CREATE_MSG = "Save data fail";
        public static int FAIL_READ_CODE = -1;
        public static string FAIL_READ_MSG = "Get data fail";
        public static int FAIL_UPDATE_CODE = -1;
        public static string FAIL_UPDATE_MSG = "Update data fail";
        public static int FAIL_DELETE_CODE = -1;
        public static string FAIL_DELETE_MSG = "Delete data fail";

        #endregion

        #region Warning Code

        public static int WARNING_NO_DATA_CODE = 2;
        public static string WARNING_NO_DATA_MSG = "Warning: No data";

        #endregion

        #region Invalid Data

        public static int ERROR_INVALID_DATA = 3;
        public static string ERROR_INVALID_DATA_MSG = "Error: No data";

        #endregion

        #region Error Verify

        public static int ERROR_UNAUTHORIZED = 4;
        public static string ERROR_UNAUTHORIZED_MSG = "Sai thông tin";

        #endregion

    }
}


