namespace ApiPrincipal.Routes
{
    public class RoutesPath
    {
        #region ControllerBase
        public const string ProducesJson = "application/json";
        public const string Base = "/ApiPrincipal";
        public const string BasePagination = "/{Pagina?}/{Total?}";
        public const string BaseOrder = "/{Columna?}/{Orden?}";
        #endregion

        #region AplicationValidateController
        public const string AplicationValidateController = "Aplicacion";
        public const string AplicationValidateController_LoginPKI = Base + "/" + AplicationValidateController + "/Login/";

        #endregion

        #region  GatewayController
        public const string GatewayController = "Gateway";
        public const string GatewayController_GatewayStarter = Base + "/" + GatewayController + "/GatewayStarter/";
        public const string GatewayController_GatewayBank = Base + "/" + GatewayController + "/GatewayBank/";
        public const string GatewayController_GatewayCreated = Base + "/" + GatewayController + "/GatewayCreated/";
        public const string GatewayController_GatewayGetDataTransaction = Base + "/" + GatewayController + "/GatewayGetDataTransaction/";
        public const string GatewayController_ResumePay = Base + "/" + GatewayController + "/ResumePay/";
        public const string GatewayController_GetMetodPagoXUsuario = Base + "/" + GatewayController + "/GetMetodPagoXUsuario/";
        public const string GatewayController_GatewayNequiPush = Base + "/" + GatewayController + "/NequiPush/";

        #endregion

        #region DashboardController
        public const string DashboardController = "DashBoard";
        public const string DashboardController_HistorialTransacciones = Base + "/" + DashboardController + "/HistorialTransacciones/";
        public const string DashboardController_PorcentajeMensual= Base + "/" + DashboardController + "/PorcentajeMensual/";
        public const string DashboardController_TransaccionesAño = Base + "/" + DashboardController + "/TransaccionesAño/";
        #endregion

        #region UserPortalController
        public const string UserPortalController = "UserPortal";
        public const string UserPortalController_LoginUser = Base + "/" + UserPortalController + "/LoginUser/";
        public const string UserPortalController_ValidOtp = Base + "/" + UserPortalController + "/ValidOtp/";
        #endregion

        #region ReportsController
        public const string ReportsController = "Reports";
        public const string ReportsController_Transactions = Base + "/" + ReportsController + "/Transactions/";
        public const string ReportsController_Dispersion = Base + "/" + ReportsController + "/Dispersion/";

        #endregion
        #region DispersionController
        public const string DispersionController = "Dispersion";
        public const string DispersionController_Desicion = Base + "/" + DispersionController + "/Desicion/";
        public const string DispersionController_BilleteraCliente = Base + "/" + DispersionController + "/BilleteraCliente/";

        #endregion
    }
}
