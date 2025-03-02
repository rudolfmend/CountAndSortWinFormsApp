using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountAndSortWinFormsAppNetFr4
{
    public class ClientInfo
    {
        public string PersonalId { get; set; }
        public string Name { get; set; }
        public string Identifier => $"{PersonalId} {Name}";
        public int ServiceCount { get; set; }
        public int TotalPoints { get; set; }
        public decimal TotalValue { get; set; }
        public decimal Coefficient { get; set; }
        public List<string> Codes { get; set; } = new List<string>();
        public List<string> ServiceCodes { get; set; } = new List<string>();
        public string InsuranceProvider { get; set; }

        public decimal AverageValuePerService => ServiceCount > 0 ? TotalValue / ServiceCount : 0;
        public int AveragePointsPerService => ServiceCount > 0 ? TotalPoints / ServiceCount : 0;
    }
}
