

using Infrastructure.Models;

namespace BatchIDnumber.Models
{
    public class ReportViewModel : ProcessCount
    {
        public ReportViewModel() => CopyParam = new();

        /// <summary>
        /// 
        /// </summary>
        public CopyParam CopyParam { get; set; }

        public string ToLog()
         => $@"Accno:{CopyParam.AccNo} IDNumber:{CopyParam.IDNumber} NewIDNumber:{CopyParam.NewIDNumber}";
    }
}
