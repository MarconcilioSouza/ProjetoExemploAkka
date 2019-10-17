using System;
using System.Text;

namespace ProcessadorPassagensProcessadasApi.CommandQuery.Dtos
{
    public class ProcedureStatusDto
    {      
        public bool Status { get; set; }
        public int? ErrorNumber { get; set; }
        public int? ErrorSeverity { get; set; }
        public int? ErrorState { get; set; }
        public string ErrorProcedure { get; set; }
        public int? ErrorLine { get; set; }
        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Status: {0} ",Status);

            if (ErrorNumber.HasValue)
                sb.AppendFormat("ErrorNumber: {0} ", ErrorNumber.Value);

            if (ErrorSeverity.HasValue)
                sb.AppendFormat("ErrorSeverity: {0} ", ErrorSeverity.Value);

            if (ErrorState.HasValue)
                sb.AppendFormat("ErrorState: {0} ", ErrorState.Value);

            if (!String.IsNullOrEmpty(ErrorProcedure))
                sb.AppendFormat("ErrorProcedure: {0} ", ErrorProcedure);

            if (ErrorLine.HasValue)
                sb.AppendFormat("ErrorLine: {0} ", ErrorLine.Value);

            if (!String.IsNullOrEmpty(ErrorMessage))
                sb.AppendFormat("ErrorMessage: {0} ", ErrorMessage);

            return sb.ToString();
        }

    }
}
