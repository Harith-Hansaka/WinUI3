using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNDAI.MODELS.MASTER
{
    public class AlarmHistoryMasterModel
    {
        public int? ID { get; set; }
        public String? Date { get; set; }
        public String? Time { get; set; }
        public string? AlarmName { get; set; }
    }
}
